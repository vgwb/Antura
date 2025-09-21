#if UNITY_EDITOR
using AdventurEd.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Text.RegularExpressions;

namespace Antura.Discover.Audio.Editor
{
    public class TTSVoiceLabWindow : EditorWindow
    {
        private const string ApiKeyPrefsKey = "Antura.ElevenLabs.ApiKey";

        [SerializeField] private VoiceProfileData selectedProfile;
        [SerializeField] private VoiceProfileCatalog voiceCatalog; // optional catalog with per-language/actor mappings
        [SerializeField] private VoiceActors selectedActor = VoiceActors.Default;
        private List<Locale> locales = new List<Locale>();
        private int selectedLocaleIndex = 0; // 0..n-1

        private string voiceId = "";
        private string modelId = "eleven_flash_v2_5";
        private string testText = "Hello, this is a test of the ElevenLabs TTS.";
        private float stability = 0.5f;
        private float similarityBoost = 0.8f;
        private float styleExaggeration = 0.0f;
        private bool useSpeakerBoost = false;
        private int sampleRate = 24000;

        private AudioClip testClip;
        private bool isPlaying = false;
        private float previewVolume = 1f;
        private static AudioSource previewSource;

        // ElevenLabs API key
        private LocalSecrets _secrets;

        private const string ProfilesRoot = "Assets/_discover/_data/Voices";

        [MenuItem("Antura/Audio/TTS Voice Lab")]
        public static void ShowWindow()
        {
            GetWindow<TTSVoiceLabWindow>("TTS Voice Lab");
        }

        private void OnEnable()
        {
            try
            {
                locales = LocalizationSettings.AvailableLocales != null
                    ? new List<Locale>(LocalizationSettings.AvailableLocales.Locales)
                    : new List<Locale>();
                if (selectedLocaleIndex >= locales.Count)
                    selectedLocaleIndex = Mathf.Max(0, locales.Count - 1);
            }
            catch { }

            if (_secrets == null)
            {
                var guid = AssetDatabase.FindAssets("t:LocalSecrets").FirstOrDefault();
                if (!string.IsNullOrEmpty(guid))
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    _secrets = AssetDatabase.LoadAssetAtPath<LocalSecrets>(path);
                }
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("TTS Voice Lab", EditorStyles.boldLabel);

            // Voice Profile selector
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Voice Profile", EditorStyles.boldLabel);
                var newProfile = (VoiceProfileData)EditorGUILayout.ObjectField("Profile Asset", selectedProfile, typeof(VoiceProfileData), false);
                if (newProfile != selectedProfile)
                {
                    selectedProfile = newProfile;
                    if (selectedProfile != null)
                    {
                        LoadFromProfile(selectedProfile);
                    }
                }

                if (selectedProfile != null)
                {
                    EditorGUILayout.LabelField("Actor", string.IsNullOrEmpty(selectedProfile.ActorKey) ? "(none)" : selectedProfile.ActorKey);
                    EditorGUILayout.LabelField("Display Name", string.IsNullOrEmpty(selectedProfile.DisplayName) ? "(none)" : selectedProfile.DisplayName);
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Load From Profile"))
                        {
                            LoadFromProfile(selectedProfile);
                        }
                        if (GUILayout.Button("Save To Profile"))
                        {
                            SaveToProfile(selectedProfile);
                        }
                    }
                }
            }

            // Catalog + Locale + Actor for resolved previews
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Resolved Voice (Catalog)", EditorStyles.boldLabel);
                voiceCatalog = (VoiceProfileCatalog)EditorGUILayout.ObjectField("Voice Catalog (optional)", voiceCatalog, typeof(VoiceProfileCatalog), false);
                selectedActor = (VoiceActors)EditorGUILayout.EnumPopup("Actor", selectedActor);
                if (locales == null || locales.Count == 0)
                {
                    EditorGUILayout.HelpBox("No Locales found. Configure Localization Project Locales.", MessageType.Info);
                }
                else
                {
                    var options = locales.ConvertAll(l => $"{l.Identifier.Code} — {l.LocaleName}");
                    selectedLocaleIndex = EditorGUILayout.Popup("Locale", Mathf.Clamp(selectedLocaleIndex, 0, locales.Count - 1), options.ToArray());
                }

                var resolved = GetResolvedProfile();
                if (resolved != null)
                {
                    EditorGUILayout.LabelField("Resolved VoiceId", string.IsNullOrEmpty(resolved.VoiceId) ? "(none)" : resolved.VoiceId);
                    EditorGUILayout.LabelField("Model", string.IsNullOrEmpty(resolved.ModelId) ? "(default)" : resolved.ModelId);
                }
                else
                {
                    EditorGUILayout.HelpBox("No resolved profile (catalog missing or no mapping). The manual profile/fields below will be used.", MessageType.None);
                }
            }

            // API key field (persisted)
            _secrets = (LocalSecrets)EditorGUILayout.ObjectField("Local Secrets", _secrets, typeof(LocalSecrets), false);
            var apiPreview = _secrets != null && !string.IsNullOrEmpty(_secrets.elevenLabsApiKey)
                ? MaskApiKey(_secrets.elevenLabsApiKey)
                : "(not set)";
            EditorGUILayout.LabelField("API Key", apiPreview);
            voiceId = EditorGUILayout.TextField("Voice ID", voiceId);
            modelId = EditorGUILayout.TextField("Model ID", modelId);

            testText = EditorGUILayout.TextArea(testText, GUILayout.Height(60));

            stability = EditorGUILayout.Slider("Stability", stability, 0, 1);
            similarityBoost = EditorGUILayout.Slider("Similarity Boost", similarityBoost, 0, 1);
            styleExaggeration = EditorGUILayout.Slider("Style Exaggeration", styleExaggeration, 0, 1);
            useSpeakerBoost = EditorGUILayout.Toggle("Use Speaker Boost", useSpeakerBoost);
            sampleRate = EditorGUILayout.IntField("Sample Rate", sampleRate);
            previewVolume = EditorGUILayout.Slider("Preview Volume", previewVolume, 0f, 1f);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Test Manual Voice"))
                {
                    TestTTS(manual: true);
                }
                if (GUILayout.Button("Test Resolved Voice"))
                {
                    TestTTS(manual: false);
                }
            }

            // Download all personal voices and create VoiceProfiles
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("ElevenLabs Library", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("Downloads all voices from your ElevenLabs library and creates/updates a VoiceProfile for each.", MessageType.Info);
                if (GUILayout.Button("Download personal voices and create profiles"))
                {
                    var helper = new GameObject("TTSHelper").AddComponent<TTSCoroutineHelper>();
                    helper.StartCoroutine(DownloadCollectionVoicesAndCreateProfiles(helper));
                }
            }

            if (testClip != null)
            {
                if (GUILayout.Button(isPlaying ? "Stop" : "Play"))
                {
                    if (isPlaying)
                    {
                        StopPlaying();
                    }
                    else
                    {
                        PlayClip();
                    }
                }
            }
        }

        private void TestTTS(bool manual)
        {
            VoiceProfileData useProfile = null;
            if (manual)
            {
                if (selectedProfile != null && string.IsNullOrEmpty(voiceId))
                {
                    // Ensure we use the profile's id if the field is empty
                    voiceId = selectedProfile.VoiceId;
                }
            }
            else
            {
                useProfile = GetResolvedProfile() ?? selectedProfile;
                if (useProfile != null)
                {
                    voiceId = useProfile.VoiceId;
                    modelId = useProfile.ModelId;
                    stability = useProfile.Stability;
                    similarityBoost = useProfile.SimilarityBoost;
                    styleExaggeration = useProfile.StyleExaggeration;
                    useSpeakerBoost = useProfile.UseSpeakerBoost;
                    sampleRate = useProfile.SampleRate;
                }
            }

            if (string.IsNullOrEmpty(voiceId))
            {
                Debug.LogError("Voice ID is required.");
                return;
            }

            var helper = new GameObject("TTSHelper").AddComponent<TTSCoroutineHelper>();
            helper.StartCoroutine(TestTTSCoroutine(helper));
        }

        private VoiceProfileData GetResolvedProfile()
        {
            if (voiceCatalog == null || locales == null || locales.Count == 0)
                return null;
            var idx = Mathf.Clamp(selectedLocaleIndex, 0, Mathf.Max(0, locales.Count - 1));
            var locale = locales.Count > 0 ? locales[idx] : null;
            if (locale == null)
                return null;
            return voiceCatalog.GetProfile(locale, selectedActor);
        }

        private void LoadFromProfile(VoiceProfileData profile)
        {
            if (profile == null)
                return;
            voiceId = profile.VoiceId;
            modelId = profile.ModelId;
            stability = profile.Stability;
            similarityBoost = profile.SimilarityBoost;
            styleExaggeration = profile.StyleExaggeration;
            useSpeakerBoost = profile.UseSpeakerBoost;
            sampleRate = profile.SampleRate;
            Repaint();
        }

        private void SaveToProfile(VoiceProfileData profile)
        {
            if (profile == null)
                return;
            profile.VoiceId = voiceId;
            profile.ModelId = modelId;
            profile.Stability = stability;
            profile.SimilarityBoost = similarityBoost;
            profile.StyleExaggeration = styleExaggeration;
            profile.UseSpeakerBoost = useSpeakerBoost;
            profile.SampleRate = sampleRate;
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
        }

        private IEnumerator TestTTSCoroutine(MonoBehaviour helper)
        {
            var apiKey = _secrets != null ? _secrets.elevenLabsApiKey : string.Empty;
            if (string.IsNullOrEmpty(apiKey))
            {
                EditorUtility.DisplayDialog("TTS", "Please set the ElevenLabs API key in a LocalSecrets asset.", "OK");
                yield break;
            }

            var url = $"https://api.elevenlabs.io/v1/text-to-speech/{voiceId}";
            var request = new UnityWebRequest(url, "POST");
            request.SetRequestHeader("xi-api-key", apiKey);
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "audio/mpeg");

            // Map sample rate to a supported output format. Prefer safe default.
            string outputFormat = sampleRate >= 44100 ? "mp3_44100_128" : "mp3_44100_128";

            var requestBody = new TTSRequestPayload
            {
                text = testText,
                model_id = string.IsNullOrEmpty(modelId) ? "eleven_flash_v2_5" : modelId,
                voice_settings = new VoiceSettingsPayload
                {
                    stability = stability,
                    similarity_boost = similarityBoost,
                    style = styleExaggeration,
                    use_speaker_boost = useSpeakerBoost
                },
                output_format = outputFormat
            };

            var json = JsonUtility.ToJson(requestBody);
            var bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerAudioClip(request.url, AudioType.MPEG);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                testClip = DownloadHandlerAudioClip.GetContent(request);
                Debug.Log("TTS test successful. Audio clip created.");
            }
            else
            {
                string serverMsg = null;
                try
                {
                    var bytes = request.downloadHandler != null ? request.downloadHandler.data : null;
                    if (bytes != null && bytes.Length > 0)
                    {
                        serverMsg = Encoding.UTF8.GetString(bytes);
                    }
                }
                catch { }

                Debug.LogError($"TTS test failed: {request.error}\nResponse: {serverMsg}");
            }

            DestroyImmediate(helper.gameObject);
        }

        private void PlayClip()
        {
            if (testClip == null)
                return;
            EnsurePreviewSource();
            previewSource.Stop();
            previewSource.clip = testClip;
            previewSource.volume = previewVolume;
            previewSource.spatialBlend = 0f;
            previewSource.Play();
            isPlaying = true;
        }

        private void StopPlaying()
        {
            if (previewSource != null)
            {
                previewSource.Stop();
            }
            isPlaying = false;
        }

        private void EnsurePreviewSource()
        {
            if (previewSource != null)
                return;
            var go = EditorUtility.CreateGameObjectWithHideFlags("TTS Preview Audio", HideFlags.HideAndDontSave, typeof(AudioSource));
            var src = go.GetComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = false;
            src.spatialBlend = 0f; // 2D
            src.volume = previewVolume;
            previewSource = src;
        }

        private void OnDisable()
        {
            if (previewSource != null)
            {
                var go = previewSource.gameObject;
                previewSource = null;
                if (go != null)
                    DestroyImmediate(go);
            }
        }

        private static string MaskApiKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            { return string.Empty; }
            if (key.Length <= 6)
            { return new string('*', key.Length); }
            return key.Substring(0, 3) + new string('*', key.Length - 6) + key.Substring(key.Length - 3);
        }

        private IEnumerator DownloadCollectionVoicesAndCreateProfiles(MonoBehaviour helper)
        {
            var apiKey = _secrets != null ? _secrets.elevenLabsApiKey : string.Empty;
            if (string.IsNullOrEmpty(apiKey))
            {
                EditorUtility.DisplayDialog("TTS", "Please set the ElevenLabs API key in a LocalSecrets asset.", "OK");
                DestroyImmediate(helper.gameObject);
                yield break;
            }

            EnsureFolder(ProfilesRoot);

            var url = "https://api.elevenlabs.io/v2/voices?page_size=30&collection_id=nrYb8pGN3mGNvf8nmrkp";
            var req = new UnityWebRequest(url, "GET");
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("xi-api-key", apiKey);

            EditorUtility.DisplayProgressBar("ElevenLabs", "Fetching collection voices…", 0.05f);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[TTSVoiceLab] Failed to fetch voices: {req.error}\n{req.downloadHandler.text}");
                EditorUtility.ClearProgressBar();
                DestroyImmediate(helper.gameObject);
                yield break;
            }

            V2VoicesResponse data = null;
            try
            {
                data = JsonUtility.FromJson<V2VoicesResponse>(SafeJsonObject(req.downloadHandler.text));
            }
            catch (Exception ex)
            {
                Debug.LogError($"[TTSVoiceLab] Parse error: {ex.Message}");
            }

            if (data == null || data.voices == null || data.voices.Length == 0)
            {
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog("TTS Voice Lab", "No personal voices found.", "OK");
                DestroyImmediate(helper.gameObject);
                yield break;
            }

            int created = 0, updated = 0;
            for (int i = 0; i < data.voices.Length; i++)
            {
                var v = data.voices[i];
                float p = Mathf.Clamp01((i + 1f) / data.voices.Length);
                EditorUtility.DisplayProgressBar("ElevenLabs", $"Processing {v.name}", p);

                // verified_languages as comma list
                var verifiedLangs = v.verified_languages != null && v.verified_languages.Length > 0
                    ? string.Join(",", v.verified_languages.Select(x => x != null ? x.language : null).Where(s => !string.IsNullOrEmpty(s)).Distinct())
                    : ExtractVerifiedLanguagesCsv(req.downloadHandler.text); // best-effort fallback if structure differs

                // Create/update VoiceProfile asset
                var safeName = SanitizeFileName($"{v.name}");
                var assetPath = $"{ProfilesRoot}/{safeName}.asset";
                var profile = AssetDatabase.LoadAssetAtPath<VoiceProfileData>(assetPath);
                bool isNew = profile == null;
                if (isNew)
                {
                    profile = ScriptableObject.CreateInstance<VoiceProfileData>();
                    AssetDatabase.CreateAsset(profile, assetPath);
                    created++;
                }
                else
                {
                    updated++;
                }

                // Populate fields
                profile.VoiceId = v.voice_id;
                profile.ModelId = string.IsNullOrEmpty(profile.ModelId) ? "eleven_flash_v2_5" : profile.ModelId;
                profile.DisplayName = string.IsNullOrEmpty(v.name) ? v.voice_id : v.name;
                profile.VerifiedLanguages = verifiedLangs ?? string.Empty;
                profile.description = v.description ?? profile.description;
                profile.preview_url = v.preview_url ?? profile.preview_url;
                profile.language = v.labels.language;
                if (!string.IsNullOrEmpty(v.labels.age))
                { profile.age = v.labels.age; }
                if (!string.IsNullOrEmpty(v.labels.gender))
                { profile.gender = v.labels.gender; }


                EditorUtility.SetDirty(profile);
                AssetDatabase.SaveAssets();
            }

            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("TTS Voice Lab", $"Done.\nCreated: {created}\nUpdated: {updated}", "OK");
            DestroyImmediate(helper.gameObject);
        }

        [Serializable]
        private class V2VoicesResponse
        {
            public V2Voice[] voices;
        }

        [Serializable]
        private class V2Voice
        {
            public string voice_id;
            public string name;
            public string description;
            public string preview_url;
            public V2VerifiedLanguage[] verified_languages;
            public V2Labels labels;
        }

        [Serializable]
        private class V2VerifiedLanguage
        {
            public string language;
        }

        [Serializable]
        private class V2Labels
        {
            public string age;
            public string gender;
            public string language;
            public string descriptive;

        }

        private static string ExtractVerifiedLanguagesCsv(string json)
        {
            if (string.IsNullOrEmpty(json))
            { return string.Empty; }
            var mArray = Regex.Match(json, "\"verified_languages\"\\s*:\\s*\\[([^\\]]*)\\]", RegexOptions.IgnoreCase);
            if (!mArray.Success)
            { return string.Empty; }
            var body = mArray.Groups[1].Value;
            // match "language":"xx" or "xx" entries
            var langs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (Match m in Regex.Matches(body, "\"language\"\\s*:\\s*\"([^\"]+)\""))
            {
                langs.Add(m.Groups[1].Value);
            }
            if (langs.Count == 0)
            {
                foreach (Match m in Regex.Matches(body, "\"([^\"]+)\""))
                {
                    langs.Add(m.Groups[1].Value);
                }
            }
            return string.Join(",", langs);
        }

        private static string SafeJsonObject(string json)
        {
            if (string.IsNullOrEmpty(json))
            { return "{}"; }
            var trimmed = json.TrimStart();
            return trimmed.StartsWith("{") ? json : "{\"voices\":" + json + "}";
        }

        private static void EnsureFolder(string folder)
        {
            if (AssetDatabase.IsValidFolder(folder))
            { return; }
            var parts = folder.Split('/');
            var cur = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                var next = $"{cur}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(cur, parts[i]);
                }
                cur = next;
            }
        }

        private static string SanitizeFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
            { return "voice"; }
            var invalid = System.IO.Path.GetInvalidFileNameChars();
            var sb = new StringBuilder(name.Length);
            foreach (var ch in name)
            {
                sb.Append(invalid.Contains(ch) ? '_' : ch);
            }
            return sb.ToString();
        }
    }

    [Serializable]
    internal class VoiceSettingsPayload
    {
        public float stability;
        public float similarity_boost;
        public float style; // ElevenLabs expects `style`
        public bool use_speaker_boost;
    }

    [Serializable]
    internal class TTSRequestPayload
    {
        public string text;
        public string model_id;
        public VoiceSettingsPayload voice_settings;
        public string output_format;
    }

    public class TTSCoroutineHelper : MonoBehaviour { }
}
#endif
