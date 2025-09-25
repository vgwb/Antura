#if UNITY_EDITOR
using AdventurEd.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Antura.Discover.Audio.Editor
{
    public class VoiceoverManagerWindow : EditorWindow
    {
        private const string CardsAudioTableName = "Cards audio";
        private readonly AddressablesVoService _addressablesSvc = new AddressablesVoService();
        private static void ApplyVorbisQuality(string assetPath, float quality01)
        {
            if (string.IsNullOrEmpty(assetPath))
                return;
            var ext = Path.GetExtension(assetPath);
            if (!string.Equals(ext, ".ogg", StringComparison.OrdinalIgnoreCase))
                return; // Only apply to OGG imports
            var importer = AssetImporter.GetAtPath(assetPath) as AudioImporter;
            if (importer == null)
                return;
            var settings = importer.defaultSampleSettings;
            settings.compressionFormat = AudioCompressionFormat.Vorbis;
            settings.quality = Mathf.Clamp01(quality01);
            importer.defaultSampleSettings = settings;
            importer.SaveAndReimport();
        }

        private static void AssignCardAudioToAssetTable(Locale locale, string key, string assetPath)
        {
            if (locale == null || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(assetPath))
                return;
            var at = LocalizationSettings.AssetDatabase.GetTable(CardsAudioTableName, locale);
            if (at == null)
            {
                Debug.LogWarning($"[QVM] '{CardsAudioTableName}' Asset Table not found for locale {locale.Identifier.Code}. Skipping assignment for key '{key}'.");
                return;
            }
            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            var entry = at.GetEntry(key) ?? at.AddEntry(key, guid ?? string.Empty);
            if (!string.IsNullOrEmpty(guid))
            {
                entry.Guid = guid;
                EditorUtility.SetDirty(at);
                if (at.SharedData != null)
                    EditorUtility.SetDirty(at.SharedData);
            }
        }
        private Vector2 _scroll;

        // Generation options
        private bool _onlyGenerateMissing = true; // Only generate files that are missing
        private bool _convertToOgg = false;
        private string _ffmpegPath = "ffmpeg";
        private bool _keepMp3AfterConversion = false;
        private bool _trimSilence = false;
        private bool _trimHeadAlso = false;
        private float _trimThresholdDb = -40f;
        private float _trimTailDurationSec = 0.2f;
        private float _trimHeadDurationSec = 0.05f;

        private const string PrefKeyFfmpegPath = "Antura.Audio.QVM.ffmpegPath";
        private const string PrefKeyVoiceProfileGuid = "Antura.Audio.QVM.voiceProfileGuid";
        private const string PrefKeyVoiceCatalogGuid = "Antura.Audio.QVM.voiceCatalogGuid";
        private const string PrefKeyConvertToOgg = "Antura.Audio.QVM.convertToOgg";
        private const string PrefKeyTrimSilence = "Antura.Audio.QVM.trimSilence";
        private const string PrefKeyTrimAlsoHead = "Antura.Audio.QVM.trimAlsoHead";
        private const string PrefKeyTrimThresholdDb = "Antura.Audio.QVM.trimThresholdDb";
        private const string PrefKeyTrimTailSec = "Antura.Audio.QVM.trimTailSec";
        private const string PrefKeyTrimHeadSec = "Antura.Audio.QVM.trimHeadSec";

        // Run state
        private bool _isRunning = false;
        private bool _cancelRequested = false;

        // Data sources
        private readonly List<QuestData> _quests = new();
        private readonly List<Locale> _locales = new();
        private readonly List<CardData> _cards = new();

        // Selections
        private int _selectedQuestIndex = 0;
        private int _selectedLocaleIndex = 0; // 0 = All
        private string _search = string.Empty;
        private VoiceProfileCatalog _voiceCatalog;
        private VoiceActors _selectedActor = VoiceActors.Default;
        private VoiceProfileData _voiceProfile;
        private LocalSecrets _secrets;
        private bool _overwriteExisting = false;
        private int _createCapIndex = 2; // 0=1, 1=5, 2=All
        private bool _showPreviewCounts = true;
        private bool _cardsIncludeDescriptions = false;

        private ITtsService _tts = new ElevenLabsTtsService();

        // Selection helpers
        private string _selectedLineKey = null; // normalized (e.g., "line:abc123") when search matches a specific line
        private string _onlyLineKey = null;     // internal limiter for generation
        private bool _forceRecreateSelected = false; // bypass skip-existing logic for the selected line

        private const string LangBundlesRoot = "Assets/_lang_bundles/_quests";
        private const string CardsBundlesRoot = "Assets/_lang_bundles/_cards";

        [MenuItem("Antura/Audio/Voiceover Manager")]
        public static void ShowWindow()
        {
            var w = GetWindow<VoiceoverManagerWindow>(false, "Quest Voiceover", true);
            w.minSize = new Vector2(520, 420);
            w.Show();
        }

        private void OnEnable()
        {
            RefreshQuests();
            RefreshLocales();
            RefreshCards();
            _ffmpegPath = EditorPrefs.GetString(PrefKeyFfmpegPath, _ffmpegPath);
            _convertToOgg = EditorPrefs.GetBool(PrefKeyConvertToOgg, _convertToOgg);
            _trimSilence = EditorPrefs.GetBool(PrefKeyTrimSilence, _trimSilence);
            _trimHeadAlso = EditorPrefs.GetBool(PrefKeyTrimAlsoHead, _trimHeadAlso);
            _trimThresholdDb = EditorPrefs.GetFloat(PrefKeyTrimThresholdDb, _trimThresholdDb);
            _trimTailDurationSec = EditorPrefs.GetFloat(PrefKeyTrimTailSec, _trimTailDurationSec);
            _trimHeadDurationSec = EditorPrefs.GetFloat(PrefKeyTrimHeadSec, _trimHeadDurationSec);
            // Restore saved Voice Profile
            var vpGuid = EditorPrefs.GetString(PrefKeyVoiceProfileGuid, string.Empty);
            if (!string.IsNullOrEmpty(vpGuid))
            {
                var vpPath = AssetDatabase.GUIDToAssetPath(vpGuid);
                if (!string.IsNullOrEmpty(vpPath))
                {
                    var vp = AssetDatabase.LoadAssetAtPath<VoiceProfileData>(vpPath);
                    if (vp != null)
                        _voiceProfile = vp;
                }
            }
            // Restore saved Voice Catalog
            var vcGuid = EditorPrefs.GetString(PrefKeyVoiceCatalogGuid, string.Empty);
            if (!string.IsNullOrEmpty(vcGuid))
            {
                var vcPath = AssetDatabase.GUIDToAssetPath(vcGuid);
                if (!string.IsNullOrEmpty(vcPath))
                {
                    var vc = AssetDatabase.LoadAssetAtPath<VoiceProfileCatalog>(vcPath);
                    if (vc != null)
                        _voiceCatalog = vc;
                }
            }
            if (string.IsNullOrEmpty(_ffmpegPath))
            {
                var p = FFmpegUtils.FindOnPath();
                if (!string.IsNullOrEmpty(p))
                    _ffmpegPath = p;
            }
            if (_secrets == null)
            {
                var guids = AssetDatabase.FindAssets("t:LocalSecrets");
                if (guids != null && guids.Length > 0)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _secrets = AssetDatabase.LoadAssetAtPath<LocalSecrets>(path);
                }
            }
        }

        private void OnDisable()
        {
            // save ffmpeg path
            EditorPrefs.SetString(PrefKeyFfmpegPath, _ffmpegPath ?? string.Empty);
            // save Convert to OGG flag
            EditorPrefs.SetBool(PrefKeyConvertToOgg, _convertToOgg);
            EditorPrefs.SetBool(PrefKeyTrimSilence, _trimSilence);
            EditorPrefs.SetBool(PrefKeyTrimAlsoHead, _trimHeadAlso);
            EditorPrefs.SetFloat(PrefKeyTrimThresholdDb, _trimThresholdDb);
            EditorPrefs.SetFloat(PrefKeyTrimTailSec, _trimTailDurationSec);
            EditorPrefs.SetFloat(PrefKeyTrimHeadSec, _trimHeadDurationSec);
            // save Voice Profile and Catalog GUIDs
            if (_voiceProfile != null)
            {
                var path = AssetDatabase.GetAssetPath(_voiceProfile);
                var guid = string.IsNullOrEmpty(path) ? string.Empty : AssetDatabase.AssetPathToGUID(path);
                EditorPrefs.SetString(PrefKeyVoiceProfileGuid, guid ?? string.Empty);
            }
            else
            {
                EditorPrefs.SetString(PrefKeyVoiceProfileGuid, string.Empty);
            }
            if (_voiceCatalog != null)
            {
                var path = AssetDatabase.GetAssetPath(_voiceCatalog);
                var guid = string.IsNullOrEmpty(path) ? string.Empty : AssetDatabase.AssetPathToGUID(path);
                EditorPrefs.SetString(PrefKeyVoiceCatalogGuid, guid ?? string.Empty);
            }
            else
            {
                EditorPrefs.SetString(PrefKeyVoiceCatalogGuid, string.Empty);
            }
        }

        private void RefreshQuests()
        {
            _quests.Clear();
            foreach (var guid in AssetDatabase.FindAssets("t:QuestData"))
            {
                var p = AssetDatabase.GUIDToAssetPath(guid);
                var a = AssetDatabase.LoadAssetAtPath<QuestData>(p);
                if (a != null)
                    _quests.Add(a);
            }
            _quests.Sort((a, b) => string.Compare(a.Id ?? a.name, b.Id ?? b.name, StringComparison.OrdinalIgnoreCase));
        }

        private void RefreshLocales()
        {
            _locales.Clear();
            var list = LocalizationSettings.AvailableLocales?.Locales;
            if (list != null)
                _locales.AddRange(list);
            _locales.Sort((a, b) => string.Compare(a.Identifier.Code, b.Identifier.Code, StringComparison.OrdinalIgnoreCase));
        }

        private void RefreshCards()
        {
            _cards.Clear();
            foreach (var guid in AssetDatabase.FindAssets("t:CardData"))
            {
                var p = AssetDatabase.GUIDToAssetPath(guid);
                var a = AssetDatabase.LoadAssetAtPath<CardData>(p);
                if (a != null)
                    _cards.Add(a);
            }
            _cards.Sort((a, b) => string.Compare(a.Id ?? a.name, b.Id ?? b.name, StringComparison.OrdinalIgnoreCase));
        }

        private void OnGUI()
        {
            using (var scroll = new EditorGUILayout.ScrollViewScope(_scroll))
            {
                _scroll = scroll.scrollPosition;

                // Filters and selections
                using (new EditorGUILayout.HorizontalScope())
                {
                    _search = EditorGUILayout.TextField("Search", _search);
                    if (GUILayout.Button("Refresh", GUILayout.Width(80)))
                    { RefreshQuests(); RefreshLocales(); }
                }

                // Do NOT filter quests by Search; Search applies only to lines in the selected quest (and to cards list)
                var filtered = _quests;

                var questNames = filtered.Select(q =>
                {
                    var id = string.IsNullOrEmpty(q.Id) ? q.name : q.Id;
                    var title = q.TitleEn;
                    return string.IsNullOrWhiteSpace(title) ? id : ($"{id} — {title}");
                }).ToArray();
                if (questNames.Length == 0)
                { EditorGUILayout.HelpBox("No quests found.", MessageType.Info); return; }
                _selectedQuestIndex = Mathf.Clamp(_selectedQuestIndex, 0, questNames.Length - 1);
                _selectedQuestIndex = EditorGUILayout.Popup("Quest", _selectedQuestIndex, questNames);

                var quest = filtered[_selectedQuestIndex];

                // Locale selection
                var localeNames = new List<string> { "All" };
                localeNames.AddRange(_locales.Select(l => l.Identifier.Code));
                _selectedLocaleIndex = EditorGUILayout.Popup("Locale", _selectedLocaleIndex, localeNames.ToArray());

                // Attempt to resolve search token as a specific line selection
                _selectedLineKey = null;
                if (!string.IsNullOrWhiteSpace(_search) && quest != null)
                {
                    var token = _search.Trim();
                    var lineKey = NormalizeLineKey(token);
                    var lineIdShort = StripLinePrefix(lineKey);
                    // Check existence in first target locale (fallback to selected locale)
                    var firstLocale = GetTargetLocales().FirstOrDefault() ?? LocalizationSettings.SelectedLocale;
                    try
                    {
                        var st = LocalizationSettings.StringDatabase.GetTable(quest.QuestStringsTable.TableReference, firstLocale);
                        if (st != null && st.GetEntry(lineKey) != null)
                        {
                            _selectedLineKey = lineKey;
                            var meta = YarnLineMapBuilder.BuildMeta(quest);
                            var nodeTitle = ResolveNodeTitleForKey(lineIdShort, meta.Titles);
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.HelpBox($"Selected line: {lineKey} — node: {nodeTitle}", MessageType.Info);
                                using (new EditorGUILayout.VerticalScope(GUILayout.Width(140)))
                                {
                                    if (GUILayout.Button("Ping Audio", GUILayout.Height(22)))
                                    {
                                        TryPingAudioForLine(quest, firstLocale, lineKey);
                                    }
                                    if (GUILayout.Button("Create only this", GUILayout.Height(22)))
                                    {
                                        RunCreateAudioForSelectedLine(lineKey);
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }

                // Voice
                _voiceCatalog = (VoiceProfileCatalog)EditorGUILayout.ObjectField("Voice Catalog", _voiceCatalog, typeof(VoiceProfileCatalog), false);
                _voiceProfile = (VoiceProfileData)EditorGUILayout.ObjectField("Voice Profile", _voiceProfile, typeof(VoiceProfileData), false);
                _selectedActor = (VoiceActors)EditorGUILayout.EnumPopup("Actor", _selectedActor);
                EditorGUILayout.HelpBox(
                    "Voice precedence: VoiceProfile > Actor > Yarn per-line > Default.\n" +
                    "If Actor is set to SILENT, audio will not be generated.\n" +
                    "If a Yarn per-line actor is SILENT, that line will be skipped.",
                    MessageType.Info);
                _secrets = (LocalSecrets)EditorGUILayout.ObjectField("Local Secrets", _secrets, typeof(LocalSecrets), false);
                if (_secrets != null)
                {
                    EditorGUILayout.LabelField("API key", MaskApiKey(_secrets.elevenLabsApiKey));
                }

                // Options
                _overwriteExisting = EditorGUILayout.ToggleLeft("Overwrite existing", _overwriteExisting);
                _onlyGenerateMissing = EditorGUILayout.ToggleLeft("Only generate missing", _onlyGenerateMissing);

                _convertToOgg = EditorGUILayout.ToggleLeft("Convert to OGG (ffmpeg)", _convertToOgg);
                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUI.DisabledScope(!_convertToOgg))
                {
                    _ffmpegPath = EditorGUILayout.TextField("ffmpeg path", _ffmpegPath);
                    _keepMp3AfterConversion = EditorGUILayout.ToggleLeft("Keep MP3 after conversion", _keepMp3AfterConversion);
                }

                _trimSilence = EditorGUILayout.ToggleLeft("Trim silence (ffmpeg)", _trimSilence);
                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUI.DisabledScope(!_trimSilence))
                {
                    _trimThresholdDb = EditorGUILayout.Slider("Threshold (dB)", _trimThresholdDb, -90f, 0f);
                    _trimTailDurationSec = EditorGUILayout.Slider("Tail min duration (s)", _trimTailDurationSec, 0f, 1f);
                    _trimHeadAlso = EditorGUILayout.ToggleLeft("Also trim leading silence", _trimHeadAlso);
                    using (new EditorGUI.IndentLevelScope())
                    using (new EditorGUI.DisabledScope(!_trimHeadAlso))
                    {
                        _trimHeadDurationSec = EditorGUILayout.Slider("Head min duration (s)", _trimHeadDurationSec, 0f, 0.5f);
                    }
                }

                _showPreviewCounts = EditorGUILayout.ToggleLeft("Show creation preview", _showPreviewCounts);

                // Preview planned creation counts
                var locales = GetTargetLocales().ToList();
                if (_showPreviewCounts && quest != null && locales.Count > 0)
                {
                    var meta = YarnLineMapBuilder.BuildMeta(quest);
                    var eligiblePerLoc = new Dictionary<string, int>();
                    int eligibleTotal = 0;
                    foreach (var loc in locales)
                    {
                        int eligible = ComputeEligibleCreateCount(quest, loc, meta, _onlyGenerateMissing, _convertToOgg, _overwriteExisting);
                        eligiblePerLoc[loc.Identifier.Code] = eligible;
                        eligibleTotal += eligible;
                    }
                    int cap = _createCapIndex == 0 ? 1 : (_createCapIndex == 1 ? 5 : int.MaxValue);
                    var plannedPerLoc = new Dictionary<string, int>();
                    int plannedTotal = 0;
                    foreach (var kv in eligiblePerLoc)
                    {
                        int planned = Mathf.Min(kv.Value, cap);
                        plannedPerLoc[kv.Key] = planned;
                        plannedTotal += planned;
                    }
                    string plannedStr = string.Join(", ", plannedPerLoc.Select(kv => kv.Key + ": " + kv.Value));
                    string eligibleStr = string.Join(", ", eligiblePerLoc.Select(kv => kv.Key + ": " + kv.Value));
                    EditorGUILayout.HelpBox($"Will create now (after cap): {plannedTotal}  —  {plannedStr}.\nEligible (uncapped): {eligibleTotal}  —  {eligibleStr}.", MessageType.Info);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Status", GUILayout.Height(24)))
                        RunStatus();
                    if (GUILayout.Button("Clear and Init", GUILayout.Height(24)))
                        RunClearAndInit();
                    if (GUILayout.Button("Ping Quest", GUILayout.Height(24)))
                        RunPingQuest();
                    if (GUILayout.Button("Show Audio Folder", GUILayout.Height(24)))
                        RunShowQuestAudioFolder();
                    if (GUILayout.Button("Update Addressables", GUILayout.Height(24)))
                        RunUpdateAddressablesQuest();
                    if (GUILayout.Button("Validate Addressables", GUILayout.Height(24)))
                        RunValidateAddressablesQuest();
                    GUILayout.FlexibleSpace();
                    if (_isRunning)
                    {
                        var old = GUI.backgroundColor;
                        GUI.backgroundColor = new Color(0.85f, 0.25f, 0.25f);
                        if (GUILayout.Button("Abort", GUILayout.Height(24), GUILayout.Width(80)))
                            _cancelRequested = true;
                        GUI.backgroundColor = old;
                    }
                    EditorGUILayout.LabelField("Create Count", GUILayout.Width(90));
                    _createCapIndex = EditorGUILayout.Popup(_createCapIndex, new[] { "1", "5", "All" }, GUILayout.Width(70));
                    if (GUILayout.Button("Create quest audio files", GUILayout.Height(24)))
                        RunCreateAudioFiles();
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    using (new EditorGUI.DisabledScope(!_convertToOgg))
                    {
                        if (GUILayout.Button("Convert MP3 → OGG (selected)", GUILayout.Height(22)))
                            RunConvertMp3ToOgg();
                    }
                    using (new EditorGUI.DisabledScope(!_trimSilence))
                    {
                        if (GUILayout.Button("Trim Silence (selected)", GUILayout.Height(22)))
                            RunTrimSilence();
                    }
                }

                EditorGUILayout.Space(8);
                EditorGUILayout.LabelField("Cards TTS", EditorStyles.boldLabel);
                // Info line with filtered count (search applies to Card Id only)
                var filteredCards = string.IsNullOrWhiteSpace(_search)
                    ? _cards
                    : _cards.Where(c => !string.IsNullOrEmpty(c.Id) && c.Id.IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                EditorGUILayout.LabelField($"Filtered cards: {filteredCards.Count} / {_cards.Count}");
                _cardsIncludeDescriptions = EditorGUILayout.ToggleLeft("Include Descriptions (.desc)", _cardsIncludeDescriptions);
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Update Addressables (Cards)", GUILayout.Height(22)))
                    {
                        RunUpdateAddressablesCards();
                    }
                    if (GUILayout.Button("Validate (Cards)", GUILayout.Height(22)))
                    {
                        RunValidateAddressablesCards();
                    }
                    if (GUILayout.Button("Create card audio files", GUILayout.Height(22)))
                    {
                        RunCreateCardAudio(filteredCards);
                    }
                }
            }
        }

        private QuestData GetSelectedQuest()
        {
            if (_quests == null || _quests.Count == 0)
                return null;
            return _quests[Mathf.Clamp(_selectedQuestIndex, 0, _quests.Count - 1)];
        }

        private IEnumerable<Locale> GetTargetLocales()
        {
            if (_selectedLocaleIndex <= 0)
                return _locales; // All
            int idx = _selectedLocaleIndex - 1;
            if (idx >= 0 && idx < _locales.Count)
                return new[] { _locales[idx] };
            return Enumerable.Empty<Locale>();
        }

        private void RunCreateCardAudio(List<CardData> cards)
        {
            if (cards == null || cards.Count == 0)
            { EditorUtility.DisplayDialog("Cards Audio", "No cards to process (check Search filter).", "OK"); return; }

            if (_secrets == null || string.IsNullOrEmpty(_secrets.elevenLabsApiKey))
            { EditorUtility.DisplayDialog("Cards Audio", "Please set the ElevenLabs API key in a LocalSecrets asset.", "OK"); return; }

            var locales = GetTargetLocales().ToList();
            if (locales.Count == 0)
            { EditorUtility.DisplayDialog("Cards Audio", "No locales configured.", "OK"); return; }

            int maxToCreate = _createCapIndex == 0 ? 1 : (_createCapIndex == 1 ? 5 : int.MaxValue);
            _isRunning = true;
            _cancelRequested = false;
            // Actor: Default (as requested), resolve via catalog or fallback profile
            EditorCoroutineUtility.StartCoroutineOwnerless(CreateCardsAudioCoroutine(cards, locales, _voiceCatalog, _voiceProfile, _secrets, _overwriteExisting, maxToCreate, _onlyGenerateMissing, _convertToOgg, _ffmpegPath, _keepMp3AfterConversion));
        }

        private IEnumerator CreateCardsAudioCoroutine(List<CardData> cards, List<Locale> locales, VoiceProfileCatalog catalog, VoiceProfileData fallbackVoice, LocalSecrets secrets, bool overwrite, int maxToCreate, bool onlyMissing, bool convertToOgg, string ffmpegPath, bool keepMp3)
        {
            int totalCreated = 0;
            int totalItems = cards.Count * locales.Count * (_cardsIncludeDescriptions ? 2 : 1); // approx for progress
            int processed = 0;
            // Track per-locale generation attempts so the cap applies like quest flow (cap per locale, not per card)
            var attemptsByLocale = new Dictionary<string, int>();
            foreach (var loc in locales)
                attemptsByLocale[loc.Identifier.Code] = 0;
            foreach (var card in cards)
            {
                string baseName = SanitizeFileNamePart(card.Id ?? card.name);
                string cardFolder = (CardsBundlesRoot + "/" + baseName).Replace('\\', '/');
                EnsureFolder(cardFolder);

                foreach (var locale in locales)
                {
                    // If we've reached the cap for this locale, skip further items for it
                    var locCodeCap = locale.Identifier.Code;
                    if (maxToCreate != int.MaxValue && attemptsByLocale.TryGetValue(locCodeCap, out var atts) && atts >= maxToCreate)
                        continue;

                    VoiceProfileData voice = null;
                    IVoiceProvider provider = catalog != null ? catalog : VoiceProviderManager.I?.Provider;
                    var forceProfile = fallbackVoice != null;
                    var forceActor = _selectedActor != VoiceActors.Default;
                    if (provider == null)
                    {
                        var guids = AssetDatabase.FindAssets("t:VoiceProfileCatalog");
                        if (guids != null && guids.Length > 0)
                        {
                            var p = AssetDatabase.GUIDToAssetPath(guids[0]);
                            var cat = AssetDatabase.LoadAssetAtPath<VoiceProfileCatalog>(p);
                            provider = cat;
                        }
                    }
                    if (forceProfile)
                    {
                        voice = fallbackVoice;
                    }
                    else if (provider != null)
                    {
                        var actorForDefault = forceActor ? _selectedActor : VoiceActors.Default;
                        voice = provider.GetProfile(locale, actorForDefault);
                    }
                    if (voice == null)
                    { Debug.LogError($"[QVM] No voice profile for {locale.Identifier.Code} (cards)"); continue; }

                    // Resolve strings from localized tables
                    string titleText = string.Empty;
                    string descText = string.Empty;
                    try
                    {
                        if (card.Title != null)
                            titleText = LocalizationSettings.StringDatabase.GetLocalizedString(card.Title.TableReference, card.Title.TableEntryReference, locale);
                    }
                    catch { }
                    try
                    {
                        if (card.Description != null)
                            descText = LocalizationSettings.StringDatabase.GetLocalizedString(card.Description.TableReference, card.Description.TableEntryReference, locale);
                    }
                    catch { }

                    var items = new List<(string kind, string text)>();
                    if (!string.IsNullOrWhiteSpace(titleText))
                        items.Add(("title", titleText));
                    if (_cardsIncludeDescriptions && !string.IsNullOrWhiteSpace(descText))
                        items.Add(("desc", descText));
                    if (items.Count == 0)
                        continue; // skip this card/locale if both strings are empty

                    foreach (var (kind, text) in items)
                    {
                        // Check per-locale cap before attempting generation
                        if (maxToCreate != int.MaxValue && attemptsByLocale[locCodeCap] >= maxToCreate)
                            break;
                        bool generatedNow = false;
                        processed++;

                        string ext = convertToOgg ? ".ogg" : ".mp3";
                        string nameCore = kind == "desc" ? ($"{baseName}.desc.{locale.Identifier.Code}") : ($"{baseName}.{locale.Identifier.Code}");
                        string finalAssetPath = CombinePath(cardFolder, nameCore + ext);
                        string mp3TempPath = CombinePath(cardFolder, nameCore + ".mp3");

                        float progress = Mathf.Clamp01(processed / Mathf.Max(1f, totalItems));
                        bool canceled = EditorUtility.DisplayCancelableProgressBar("Cards Audio", $"{card.Id ?? card.name} [{locale.Identifier.Code}] {kind}", progress);
                        if (canceled || _cancelRequested)
                        {
                            EditorUtility.ClearProgressBar();
                            _isRunning = false;
                            EditorUtility.DisplayDialog("Cards Audio", "Canceled.", "OK");
                            yield break;
                        }

                        bool skipExisting = onlyMissing || !overwrite;
                        if (skipExisting && File.Exists(finalAssetPath))
                            continue;
                        if (skipExisting && convertToOgg && File.Exists(mp3TempPath) && !File.Exists(finalAssetPath))
                        {
                            bool okConv = FFmpegUtils.ConvertMp3ToOgg(ffmpegPath, mp3TempPath, finalAssetPath, out string convErr2);
                            if (!okConv)
                            { Debug.LogError($"[QVM] ffmpeg conversion failed: {convErr2}. Keeping MP3."); }
                            else
                            {
                                AssetDatabase.ImportAsset(finalAssetPath, ImportAssetOptions.ForceSynchronousImport);
                                ApplyVorbisQuality(finalAssetPath, 0.7f);
                                if (!keepMp3)
                                    AssetDatabase.DeleteAsset(mp3TempPath);
                                var obj2 = AssetDatabase.LoadAssetAtPath<AudioClip>(finalAssetPath);
                                if (obj2 != null)
                                    Debug.Log($"[QVM] Created [cards {locale.Identifier.Code}] {Path.GetFileName(finalAssetPath)} ← {finalAssetPath}", obj2);
                                else
                                    Debug.Log($"[QVM] Created [cards {locale.Identifier.Code}] {Path.GetFileName(finalAssetPath)} ← {finalAssetPath}");

                                // Assign into Cards audio Asset Table with the same key as the string table
                                var key = kind == "desc"
                                    ? (!string.IsNullOrEmpty(card.Description?.TableEntryReference.Key) ? card.Description.TableEntryReference.Key : (card.Id ?? card.name) + ".desc")
                                    : (!string.IsNullOrEmpty(card.Title?.TableEntryReference.Key) ? card.Title.TableEntryReference.Key : (card.Id ?? card.name));
                                AssignCardAudioToAssetTable(locale, key, finalAssetPath);
                                // Ensure Addressables entry in official Localization-Assets group with VO/cards address and labels
                                _addressablesSvc.UpdateAddressableForClip(locale, finalAssetPath, questIdOrNull: null, isQuestClip: false, keyOrId: (card.Id ?? card.name));
                                totalCreated++;
                            }
                            continue;
                        }

                        // If Actor is explicitly set to SILENT, skip cards rendering
                        if (_selectedActor == VoiceActors.SILENT)
                            continue;

                        byte[] bytes = null;
                        yield return _tts.SynthesizeMp3Coroutine(secrets.elevenLabsApiKey, voice, text, b => bytes = b);
                        if (bytes == null || bytes.Length == 0)
                        { Debug.LogError($"[QVM] TTS failed for card {card.Id ?? card.name} ({locale.Identifier.Code}) {kind}"); continue; }

                        string assignPath = null;
                        try
                        {
                            File.WriteAllBytes(mp3TempPath, bytes);
                            AssetDatabase.ImportAsset(mp3TempPath, ImportAssetOptions.ForceSynchronousImport);
                            assignPath = mp3TempPath;
                            if (convertToOgg)
                            {
                                bool ok = FFmpegUtils.ConvertMp3ToOgg(ffmpegPath, mp3TempPath, finalAssetPath, out string convErr);
                                if (!ok)
                                { Debug.LogError($"[QVM] ffmpeg conversion failed: {convErr}. Keeping MP3."); assignPath = mp3TempPath; }
                                else
                                {
                                    AssetDatabase.ImportAsset(finalAssetPath, ImportAssetOptions.ForceSynchronousImport);
                                    ApplyVorbisQuality(finalAssetPath, 0.7f);
                                    assignPath = finalAssetPath;
                                    if (_trimSilence)
                                    {
                                        if (TryTrimFileWithFfmpeg(ffmpegPath, assignPath, _trimThresholdDb, _trimTailDurationSec, _trimHeadAlso, _trimHeadDurationSec))
                                        {
                                            AssetDatabase.ImportAsset(assignPath, ImportAssetOptions.ForceSynchronousImport);
                                        }
                                    }
                                    if (!keepMp3)
                                        AssetDatabase.DeleteAsset(mp3TempPath);
                                }
                            }
                            totalCreated++;
                            generatedNow = true;
                        }
                        catch (Exception ex)
                        { Debug.LogError($"[QVM] Write/import failed for {finalAssetPath}: {ex.Message}"); continue; }

                        // Clickable log
                        if (_trimSilence)
                        {
                            // If we didn't convert, we may still want to trim the MP3
                            if (assignPath != null && TryTrimFileWithFfmpeg(ffmpegPath, assignPath, _trimThresholdDb, _trimTailDurationSec, _trimHeadAlso, _trimHeadDurationSec))
                            {
                                AssetDatabase.ImportAsset(assignPath, ImportAssetOptions.ForceSynchronousImport);
                            }
                        }
                        var obj = AssetDatabase.LoadAssetAtPath<AudioClip>(assignPath);
                        if (obj != null)
                            Debug.Log($"[QVM] Created [cards {locale.Identifier.Code}] {Path.GetFileName(assignPath)} ← {assignPath}", obj);
                        else
                            Debug.Log($"[QVM] Created [cards {locale.Identifier.Code}] {Path.GetFileName(assignPath)} ← {assignPath}");

                        // Assign into Cards audio Asset Table with the same key as the string table
                        {
                            var key = kind == "desc"
                                ? (!string.IsNullOrEmpty(card.Description?.TableEntryReference.Key) ? card.Description.TableEntryReference.Key : (card.Id ?? card.name) + ".desc")
                                : (!string.IsNullOrEmpty(card.Title?.TableEntryReference.Key) ? card.Title.TableEntryReference.Key : (card.Id ?? card.name));
                            AssignCardAudioToAssetTable(locale, key, assignPath);
                            // Ensure Addressables entry in official Localization-Assets group with VO/cards address and labels
                            _addressablesSvc.UpdateAddressableForClip(locale, assignPath, questIdOrNull: null, isQuestClip: false, keyOrId: (card.Id ?? card.name));
                        }

                        // After a successful new generation, count toward the per-locale cap
                        if (generatedNow)
                        {
                            attemptsByLocale[locCodeCap] = attemptsByLocale[locCodeCap] + 1;
                            if (maxToCreate != int.MaxValue && attemptsByLocale[locCodeCap] >= maxToCreate)
                                break;
                        }
                    }
                }
                AssetDatabase.SaveAssets();
            }
            EditorUtility.ClearProgressBar();
            _isRunning = false;
            EditorUtility.DisplayDialog("Cards Audio", $"Done. Created {totalCreated} file(s).", "OK");
        }

        // ------------------------------- Actions -------------------------------
        private void RunStatus()
        {
            var quest = GetSelectedQuest();
            if (!ValidateBasics(quest, requireVoice: false))
                return;

            var locales = GetTargetLocales().ToList();
            if (locales.Count == 0)
            { EditorUtility.DisplayDialog("Status", "No locales configured in project.", "OK"); return; }

            // Make sure we have the latest view of assets and GUID mappings before checking
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var report = new StringBuilder();
            report.AppendLine($"Quest: {quest.Id ?? quest.name}");
            foreach (var locale in locales)
            {
                try
                {
                    var st = LocalizationSettings.StringDatabase.GetTable(quest.QuestStringsTable.TableReference, locale);
                    var at = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, locale);
                    int strings = st != null ? st.Count : 0;
                    int assets = at != null ? at.Count : 0;
                    report.AppendLine($"- {locale.Identifier.Code}: strings={strings}, assets={assets}");

                    IEnumerable<string> keysEnum = st?.Values.Select(e => e.SharedEntry?.Key).Where(k => !string.IsNullOrEmpty(k)) ?? Enumerable.Empty<string>();
                    if (!string.IsNullOrWhiteSpace(_search))
                    {
                        var token = _search.Trim();
                        keysEnum = keysEnum.Where(k => MatchesLineSearch(k, token));
                    }
                    var stringKeys = new HashSet<string>(keysEnum);
                    var assetKeys = new HashSet<string>(at?.Values.Select(e => e.SharedEntry?.Key).Where(k => !string.IsNullOrEmpty(k)) ?? Enumerable.Empty<string>());

                    var missingInAssets = stringKeys.Where(k => !assetKeys.Contains(k)).ToList();
                    var extraInAssets = assetKeys.Where(k => !stringKeys.Contains(k)).ToList();
                    if (missingInAssets.Count > 0)
                    {
                        report.AppendLine($"  Missing asset entries: {missingInAssets.Count}");
                        foreach (var k in missingInAssets.Take(10))
                            report.AppendLine($"    - {k}");
                        if (missingInAssets.Count > 10)
                            report.AppendLine("    ...");
                    }
                    if (extraInAssets.Count > 0)
                    {
                        report.AppendLine($"  Extra asset entries: {extraInAssets.Count}");
                        foreach (var k in extraInAssets.Take(10))
                            report.AppendLine($"    - {k}");
                        if (extraInAssets.Count > 10)
                            report.AppendLine("    ...");
                    }

                    // Count valid assigned assets and collect empty/invalid ones (to allow re-creation)
                    int validAssigned = 0;
                    var emptyOrInvalid = new List<string>();
                    foreach (var key in stringKeys)
                    {
                        var e = at?.GetEntry(key);
                        if (e == null || string.IsNullOrEmpty(e.Guid))
                        {
                            emptyOrInvalid.Add(key);
                            continue;
                        }
                        var p = AssetDatabase.GUIDToAssetPath(e.Guid);
                        if (string.IsNullOrEmpty(p))
                        {
                            emptyOrInvalid.Add(key);
                            continue;
                        }
                        var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(p);
                        if (clip == null || clip.length <= 0f)
                        {
                            emptyOrInvalid.Add(key);
                            continue;
                        }
                        validAssigned++;
                    }
                    report.AppendLine($"  Valid audio assets: {validAssigned}/{stringKeys.Count}");
                    if (emptyOrInvalid.Count > 0)
                    {
                        report.AppendLine($"  Empty/invalid audio assets: {emptyOrInvalid.Count}");
                        foreach (var k in emptyOrInvalid.Take(10))
                            report.AppendLine($"    - {k}");
                        if (emptyOrInvalid.Count > 10)
                            report.AppendLine("    ...");
                    }
                }
                catch (Exception ex)
                {
                    report.AppendLine($"- {locale.Identifier.Code}: ERROR {ex.Message}");
                }
            }

            Debug.Log(report.ToString());
            EditorUtility.DisplayDialog("Quest Voiceover Status", "Status computed. See Console for details.", "OK");
        }

        private void RunClearAndInit()
        {
            var quest = GetSelectedQuest();
            if (!ValidateBasics(quest, requireVoice: false))
                return;
            var locales = GetTargetLocales().ToList();
            if (locales.Count == 0)
            { EditorUtility.DisplayDialog("Clear and Init", "No locales configured.", "OK"); return; }

            if (!EditorUtility.DisplayDialog("Clear and Init", "This will delete existing audio files and reset asset table entries for the selected languages. Continue?", "Yes", "No"))
                return;

            try
            {
                for (int i = 0; i < locales.Count; i++)
                {
                    var locale = locales[i];
                    EditorUtility.DisplayProgressBar("Clear and Init", $"{quest.Id} — {locale.Identifier.Code}", (i + 1f) / locales.Count);

                    var folder = GetQuestLangFolder(quest, locale);
                    if (AssetDatabase.IsValidFolder(folder))
                    {
                        if (!AssetDatabase.DeleteAsset(folder))
                        {
                            foreach (var asset in AssetDatabase.FindAssets("*", new[] { folder }))
                            {
                                var p = AssetDatabase.GUIDToAssetPath(asset);
                                if (p != folder)
                                    AssetDatabase.DeleteAsset(p);
                            }
                        }
                    }
                    EnsureFolder(folder);

                    var st = LocalizationSettings.StringDatabase.GetTable(quest.QuestStringsTable.TableReference, locale);
                    var at = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, locale);
                    if (st == null || at == null)
                        throw new InvalidOperationException("Missing string or asset table for locale " + locale.Identifier.Code);

                    var stringKeys = new HashSet<string>(st.Values.Select(e => e.SharedEntry?.Key).Where(k => !string.IsNullOrEmpty(k)));
                    var assetKeys = new HashSet<string>(at.Values.Select(e => e.SharedEntry?.Key).Where(k => !string.IsNullOrEmpty(k)));

                    foreach (var k in assetKeys.Where(k => !stringKeys.Contains(k)).ToList())
                        at.RemoveEntry(k);
                    foreach (var k in stringKeys.Where(k => !assetKeys.Contains(k)))
                        at.AddEntry(k, string.Empty);

                    EditorUtility.SetDirty(at);
                    if (at.SharedData != null)
                        EditorUtility.SetDirty(at.SharedData);
                }
                AssetDatabase.SaveAssets();
            }
            finally { EditorUtility.ClearProgressBar(); }

            EditorUtility.DisplayDialog("Clear and Init", "Done.", "OK");
        }

        private void RunPingQuest()
        {
            var quest = GetSelectedQuest();
            if (quest == null)
            { EditorUtility.DisplayDialog("Ping Quest", "Select a quest first.", "OK"); return; }
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = quest;
            EditorGUIUtility.PingObject(quest);
        }

        private void RunShowQuestAudioFolder()
        {
            var quest = GetSelectedQuest();
            if (quest == null)
            { EditorUtility.DisplayDialog("Audio Folder", "Select a quest first.", "OK"); return; }

            var locales = GetTargetLocales().ToList();
            string folder;
            if (_selectedLocaleIndex > 0 && locales.Count == 1)
            {
                folder = GetQuestLangFolder(quest, locales[0]);
            }
            else
            {
                folder = Path.Combine(LangBundlesRoot, SanitizeFolderName(quest.Id ?? quest.name));
            }
            folder = folder.Replace('\\', '/');

            if (!AssetDatabase.IsValidFolder(folder))
            { EditorUtility.DisplayDialog("Audio Folder", $"Folder not found:\n{folder}", "OK"); return; }

            var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(folder);
            if (obj != null)
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
            }
        }

        private void RunCreateAudioFiles()
        {
            var quest = GetSelectedQuest();
            if (!ValidateBasics(quest, requireVoice: true))
                return;
            var locales = GetTargetLocales().ToList();
            if (locales.Count == 0)
            { EditorUtility.DisplayDialog("Create Audio", "No locales configured.", "OK"); return; }

            int maxToCreate = _createCapIndex == 0 ? 1 : (_createCapIndex == 1 ? 5 : int.MaxValue);

            _isRunning = true;
            _cancelRequested = false;
            EditorCoroutineUtility.StartCoroutineOwnerless(CreateAudioCoroutine(quest, locales, _voiceCatalog, _selectedActor, _voiceProfile, _secrets, _overwriteExisting, maxToCreate, _onlyGenerateMissing, _convertToOgg, _ffmpegPath, _keepMp3AfterConversion));
        }

        private bool ValidateBasics(QuestData quest, bool requireVoice)
        {
            if (quest == null)
            { EditorUtility.DisplayDialog("Quest Voiceover", "Please select a quest.", "OK"); return false; }
            if (quest.QuestStringsTable == null || quest.QuestAssetsTable == null)
            { EditorUtility.DisplayDialog("Quest Voiceover", "Quest must have both QuestStringsTable and QuestAssetsTable assigned.", "OK"); return false; }
            // When running generation, ensure secrets are present; VoiceProfile is optional.
            if (requireVoice && (_secrets == null || string.IsNullOrEmpty(_secrets.elevenLabsApiKey)))
            { EditorUtility.DisplayDialog("Quest Voiceover", "Please set the ElevenLabs API key in a LocalSecrets asset.", "OK"); return false; }
            return true;
        }

        private IEnumerator CreateAudioCoroutine(QuestData quest, List<Locale> locales, VoiceProfileCatalog catalog, VoiceActors actor, VoiceProfileData fallbackVoice, LocalSecrets secrets, bool overwrite, int maxToCreate, bool onlyMissing, bool convertToOgg, string ffmpegPath, bool keepMp3)
        {
            var meta = YarnLineMapBuilder.BuildMeta(quest);
            int totalCreated = 0;

            int localeIndex = 0;
            foreach (var locale in locales)
            {
                localeIndex++;
                int attemptsThisLocale = 0;
                var st = LocalizationSettings.StringDatabase.GetTable(quest.QuestStringsTable.TableReference, locale);
                var at = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, locale);

                // Voice selection precedence:
                // 1) If a VoiceProfile is explicitly assigned (_voiceProfile), always use it
                // 2) Else if an Actor is explicitly selected (_selectedActor != Default), force that actor's voice
                // 3) Else use catalog default voice and allow Yarn per-line actor overrides
                var forceProfile = fallbackVoice != null;
                var forceActor = actor != VoiceActors.Default;
                // Prefer an explicit catalog, else use the global provider
                IVoiceProvider provider = catalog != null ? catalog : VoiceProviderManager.I?.Provider;

                // Base/default voice for this locale
                VoiceProfileData localeDefaultVoice = null;
                if (forceProfile)
                {
                    localeDefaultVoice = fallbackVoice;
                }
                else if (provider != null)
                {
                    var actorForDefault = forceActor ? actor : VoiceActors.Default;
                    localeDefaultVoice = provider.GetProfile(locale, actorForDefault);
                }
                if (localeDefaultVoice == null)
                { Debug.LogError($"[QVM] No voice profile for {locale.Identifier.Code}"); continue; }
                if (st == null || at == null)
                { Debug.LogWarning($"[QVM] Missing tables for {quest.Id} / {locale.Identifier.Code}"); continue; }

                var entries = st.Values.Where(e => e != null && e.SharedEntry != null).ToList();
                if (string.IsNullOrEmpty(_onlyLineKey) && !string.IsNullOrWhiteSpace(_search))
                {
                    var token = _search.Trim();
                    entries = entries.Where(e => e.SharedEntry != null && MatchesLineSearch(e.SharedEntry.Key, token)).ToList();
                }
                int total = entries.Count;
                int processed = 0;

                var folder = GetQuestLangFolder(quest, locale);
                EnsureFolder(folder);

                foreach (var e in entries)
                {
                    if (!string.IsNullOrEmpty(_onlyLineKey) && !string.Equals(e.SharedEntry.Key, _onlyLineKey, StringComparison.Ordinal))
                        continue;
                    if (attemptsThisLocale >= maxToCreate)
                        break;

                    processed++;
                    string key = e.SharedEntry.Key;
                    string text = e.Value;
                    if (string.IsNullOrWhiteSpace(text))
                        continue; // skip empty localized strings
                    string lineIdShort = StripLinePrefix(key);
                    string nodeTitle = ResolveNodeTitleForKey(lineIdShort, meta.Titles);
                    // When searching by free text (and not single-line mode), only process lines whose Yarn node title matches the filter
                    if (string.IsNullOrEmpty(_onlyLineKey) && !string.IsNullOrWhiteSpace(_search))
                    {
                        bool matchesTitle = (nodeTitle?.IndexOf(_search, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0;
                        if (!matchesTitle)
                            continue;
                    }
                    // Resolve voice per line using the precedence above
                    VoiceProfileData voiceForLine = localeDefaultVoice;
                    if (!forceProfile && !forceActor)
                    {
                        // Only allow Yarn actor override when no forced profile/actor
                        if (meta.Actors != null && meta.Actors.TryGetValue(lineIdShort, out var actorOpt) && actorOpt.HasValue)
                        {
                            var v = provider != null ? provider.GetProfile(locale.Identifier.Code, actorOpt.Value) : null;
                            if (v != null)
                                voiceForLine = v;
                        }
                    }
                    // Actor metadata stored alongside manifest/hash: forced actor if set, else Yarn actor, else Default
                    VoiceActors actorForFile = VoiceActors.Default;
                    if (forceActor)
                        actorForFile = actor;
                    else if (meta.Actors != null && meta.Actors.TryGetValue(lineIdShort, out var aOpt) && aOpt.HasValue)
                        actorForFile = aOpt.Value;
                    // Filenames now simplified: questId_lineId (no nodeTitle, no actor)
                    string fileBase = $"{SanitizeFileNamePart(quest.Id)}_{SanitizeFileNamePart(lineIdShort)}";
                    string finalExt = convertToOgg ? ".ogg" : ".mp3";
                    string finalAssetPath = CombinePath(folder, fileBase + finalExt);
                    string mp3TempPath = CombinePath(folder, fileBase + ".mp3");

                    float totalPerAll = Mathf.Max(1f, total * Mathf.Max(1, locales.Count));
                    float progressGen = (processed + (localeIndex - 1) * total) / totalPerAll;
                    bool canceled = EditorUtility.DisplayCancelableProgressBar("Generating Audio", $"{quest.Id} [{locale.Identifier.Code}] {processed}/{total}: {key}", Mathf.Clamp01(progressGen));
                    if (canceled || _cancelRequested)
                    {
                        EditorUtility.ClearProgressBar();
                        _isRunning = false;
                        EditorUtility.DisplayDialog("Create Audio", "Canceled.", "OK");
                        yield break;
                    }

                    bool forceThisLine = !string.IsNullOrEmpty(_onlyLineKey) && string.Equals(key, _onlyLineKey, StringComparison.Ordinal);
                    bool skipExisting = forceThisLine && _forceRecreateSelected ? false : (onlyMissing || !overwrite);
                    if (skipExisting && File.Exists(finalAssetPath))
                    {
                        var guid = AssetDatabase.AssetPathToGUID(finalAssetPath);
                        var entry = at.GetEntry(key) ?? at.AddEntry(key, guid ?? string.Empty);
                        if (!string.IsNullOrEmpty(guid))
                            entry.Guid = guid;

                        // Mark table and shared data dirty to persist after domain reload
                        EditorUtility.SetDirty(at);
                        if (at.SharedData != null)
                            EditorUtility.SetDirty(at.SharedData);

                        // Manifest upsert for existing file path
                        var audioFileName = Path.GetFileName(finalAssetPath);
                        var normText = VoiceoverManifestUtil.NormalizeText(text);
                        var textHash = VoiceoverManifestUtil.ComputeTextHash(normText, (voiceForLine ?? localeDefaultVoice)?.Id, actorForFile.ToString(), locale.Identifier.Code);
                        VoiceoverManifestUtil.Upsert(quest.Id, locale, key: StripLinePrefix(key), audioFileName: audioFileName, textHash: textHash, durationMs: null, voiceProfileId: (voiceForLine ?? localeDefaultVoice)?.Id, actorId: actorForFile.ToString(), nodeTitle: nodeTitle, sourceText: text);
                        // Ensure Addressables entry in official Localization-Assets group with VO/quest address and labels
                        _addressablesSvc.UpdateAddressableForClip(locale, finalAssetPath, quest.Id, isQuestClip: true, keyOrId: key);
                        continue;
                    }

                    if (skipExisting)
                    {
                        var assignedEntry = at.GetEntry(key);
                        if (assignedEntry != null && !string.IsNullOrEmpty(assignedEntry.Guid))
                        {
                            var assignedPath = AssetDatabase.GUIDToAssetPath(assignedEntry.Guid);
                            if (!string.IsNullOrEmpty(assignedPath) && File.Exists(assignedPath))
                                continue;
                        }
                        if (convertToOgg && File.Exists(mp3TempPath) && !File.Exists(finalAssetPath))
                        {
                            bool ok = FFmpegUtils.ConvertMp3ToOgg(ffmpegPath, mp3TempPath, finalAssetPath, out string convErr2);
                            if (!ok)
                            {
                                Debug.LogError($"[QVM] ffmpeg conversion failed: {convErr2}. Keeping MP3.");
                                var gmp3 = AssetDatabase.AssetPathToGUID(mp3TempPath);
                                var eAssign = at.GetEntry(key) ?? at.AddEntry(key, gmp3 ?? string.Empty);
                                if (!string.IsNullOrEmpty(gmp3))
                                    eAssign.Guid = gmp3;
                                EditorUtility.SetDirty(at);
                                if (at.SharedData != null)
                                    EditorUtility.SetDirty(at.SharedData);
                            }
                            else
                            {
                                AssetDatabase.ImportAsset(finalAssetPath, ImportAssetOptions.ForceSynchronousImport);
                                ApplyVorbisQuality(finalAssetPath, 0.7f);
                                if (_trimSilence)
                                {
                                    if (TryTrimFileWithFfmpeg(ffmpegPath, finalAssetPath, _trimThresholdDb, _trimTailDurationSec, _trimHeadAlso, _trimHeadDurationSec))
                                    {
                                        AssetDatabase.ImportAsset(finalAssetPath, ImportAssetOptions.ForceSynchronousImport);
                                    }
                                }
                                var gogg = AssetDatabase.AssetPathToGUID(finalAssetPath);
                                var eAssign = at.GetEntry(key) ?? at.AddEntry(key, gogg ?? string.Empty);
                                if (!string.IsNullOrEmpty(gogg))
                                    eAssign.Guid = gogg;
                                if (!keepMp3)
                                    AssetDatabase.DeleteAsset(mp3TempPath);
                                // Manifest upsert after conversion
                                var audioFileName = Path.GetFileName(finalAssetPath);
                                var normText = VoiceoverManifestUtil.NormalizeText(text);
                                var textHash = VoiceoverManifestUtil.ComputeTextHash(normText, (voiceForLine ?? localeDefaultVoice)?.Id, actorForFile.ToString(), locale.Identifier.Code);
                                VoiceoverManifestUtil.Upsert(quest.Id, locale, key: StripLinePrefix(key), audioFileName: audioFileName, textHash: textHash, durationMs: null, voiceProfileId: (voiceForLine ?? localeDefaultVoice)?.Id, actorId: actorForFile.ToString(), nodeTitle: nodeTitle, sourceText: text);
                                // Ensure Addressables entry in official Localization-Assets group with VO/quest address and labels
                                _addressablesSvc.UpdateAddressableForClip(locale, finalAssetPath, quest.Id, isQuestClip: true, keyOrId: key);
                            }
                            EditorUtility.SetDirty(at);
                            if (at.SharedData != null)
                                EditorUtility.SetDirty(at.SharedData);
                            continue;
                        }
                    }

                    attemptsThisLocale++;

                    // If actor is SILENT, skip rendering audio for this line
                    if (actorForFile == VoiceActors.SILENT)
                        continue;

                    byte[] bytes = null;
                    yield return _tts.SynthesizeMp3Coroutine(secrets.elevenLabsApiKey, voiceForLine, text, b => bytes = b);
                    if (bytes == null || bytes.Length == 0)
                    { Debug.LogError($"[QVM] TTS failed for key {key} in {quest.Id} ({locale.Identifier.Code})"); continue; }

                    string assignPath = null;
                    bool createdNow = false;
                    try
                    {
                        File.WriteAllBytes(mp3TempPath, bytes);
                        AssetDatabase.ImportAsset(mp3TempPath, ImportAssetOptions.ForceSynchronousImport);
                        assignPath = mp3TempPath;

                        if (convertToOgg)
                        {
                            bool ok = FFmpegUtils.ConvertMp3ToOgg(ffmpegPath, mp3TempPath, finalAssetPath, out string convErr);
                            if (!ok)
                            { Debug.LogError($"[QVM] ffmpeg conversion failed: {convErr}. Keeping MP3."); assignPath = mp3TempPath; }
                            else
                            {
                                AssetDatabase.ImportAsset(finalAssetPath, ImportAssetOptions.ForceSynchronousImport);
                                ApplyVorbisQuality(finalAssetPath, 0.7f);
                                assignPath = finalAssetPath;
                                if (!keepMp3)
                                    AssetDatabase.DeleteAsset(mp3TempPath);
                            }
                        }
                        if (_trimSilence && !string.IsNullOrEmpty(assignPath))
                        {
                            if (TryTrimFileWithFfmpeg(ffmpegPath, assignPath, _trimThresholdDb, _trimTailDurationSec, _trimHeadAlso, _trimHeadDurationSec))
                            {
                                AssetDatabase.ImportAsset(assignPath, ImportAssetOptions.ForceSynchronousImport);
                            }
                        }
                        totalCreated++;
                        createdNow = true;
                    }
                    catch (Exception ex)
                    { Debug.LogError($"[QVM] Write/import failed for {finalAssetPath}: {ex.Message}"); continue; }

                    if (string.IsNullOrEmpty(assignPath) || !File.Exists(assignPath))
                        assignPath = File.Exists(finalAssetPath) ? finalAssetPath : mp3TempPath;
                    var guidNew = AssetDatabase.AssetPathToGUID(assignPath);
                    var aEntry = at.GetEntry(key) ?? at.AddEntry(key, guidNew ?? string.Empty);
                    if (!string.IsNullOrEmpty(guidNew))
                        aEntry.Guid = guidNew;
                    EditorUtility.SetDirty(at);
                    if (at.SharedData != null)
                        EditorUtility.SetDirty(at.SharedData);

                    // Ensure Addressables entry in official Localization-Assets group with VO/quest address and labels
                    _addressablesSvc.UpdateAddressableForClip(locale, assignPath, quest.Id, isQuestClip: true, keyOrId: key);

                    // Log one clickable line per generated file (ping in Project on click)
                    if (createdNow)
                    {
                        var obj = AssetDatabase.LoadAssetAtPath<AudioClip>(assignPath);
                        if (obj != null)
                            Debug.Log($"[QVM] Created [{locale.Identifier.Code}] {Path.GetFileName(assignPath)} ← {assignPath}", obj);
                        else
                            Debug.Log($"[QVM] Created [{locale.Identifier.Code}] {Path.GetFileName(assignPath)} ← {assignPath}");
                        // Manifest upsert after generation
                        var audioFileName = Path.GetFileName(assignPath);
                        var normText = VoiceoverManifestUtil.NormalizeText(text);
                        var textHash = VoiceoverManifestUtil.ComputeTextHash(normText, (voiceForLine ?? localeDefaultVoice)?.Id, actorForFile.ToString(), locale.Identifier.Code);
                        VoiceoverManifestUtil.Upsert(quest.Id, locale, key: StripLinePrefix(key), audioFileName: audioFileName, textHash: textHash, durationMs: Mathf.RoundToInt((obj != null ? obj.length : 0f) * 1000f), voiceProfileId: (voiceForLine ?? localeDefaultVoice)?.Id, actorId: actorForFile.ToString(), nodeTitle: nodeTitle, sourceText: text);
                    }
                }

                AssetDatabase.SaveAssets();
            }

            EditorUtility.ClearProgressBar();
            _isRunning = false;
            // Clear single-line mode flags after finishing
            _onlyLineKey = null;
            _forceRecreateSelected = false;
            EditorUtility.DisplayDialog("Create Audio", $"Done. Created {totalCreated} file(s).", "OK");
        }

        private void RunConvertMp3ToOgg()
        {
            var quest = GetSelectedQuest();
            var locales = GetTargetLocales().ToList();
            if (quest == null || locales.Count == 0)
            { EditorUtility.DisplayDialog("Convert MP3 → OGG", "Select a quest and at least one locale.", "OK"); return; }
            EditorCoroutineUtility.StartCoroutineOwnerless(ConvertMp3ToOggCoroutine(quest, locales, _ffmpegPath, _keepMp3AfterConversion));
        }

        private void RunTrimSilence()
        {
            var quest = GetSelectedQuest();
            var locales = GetTargetLocales().ToList();
            if (quest == null || locales.Count == 0)
            { EditorUtility.DisplayDialog("Trim Silence", "Select a quest and at least one locale.", "OK"); return; }
            if (!_trimSilence)
            { EditorUtility.DisplayDialog("Trim Silence", "Enable 'Trim silence' first.", "OK"); return; }
            _isRunning = true;
            _cancelRequested = false;
            EditorCoroutineUtility.StartCoroutineOwnerless(TrimSilenceCoroutine(quest, locales, _ffmpegPath, _trimThresholdDb, _trimTailDurationSec, _trimHeadAlso, _trimHeadDurationSec));
        }

        private void RunUpdateAddressablesQuest()
        {
            var quest = GetSelectedQuest();
            if (!ValidateBasics(quest, requireVoice: false))
                return;
            var locales = GetTargetLocales().ToList();
            if (locales.Count == 0)
            { EditorUtility.DisplayDialog("Update Addressables", "No locales configured.", "OK"); return; }
            try
            {
                _addressablesSvc.UpdateQuestAddressables(quest, locales);
                EditorUtility.DisplayDialog("Update Addressables", "Quest addressables updated.", "OK");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[QVM] Update Addressables failed: {ex.Message}\n{ex}");
                EditorUtility.DisplayDialog("Update Addressables", "Failed. See Console.", "OK");
            }
        }

        private void RunUpdateAddressablesCards()
        {
            var locales = GetTargetLocales().ToList();
            if (locales.Count == 0)
            { EditorUtility.DisplayDialog("Update Addressables (Cards)", "No locales configured.", "OK"); return; }
            try
            {
                _addressablesSvc.UpdateCardsAddressables(locales);
                EditorUtility.DisplayDialog("Update Addressables (Cards)", "Cards addressables updated.", "OK");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[QVM] Update Addressables (Cards) failed: {ex.Message}\n{ex}");
                EditorUtility.DisplayDialog("Update Addressables (Cards)", "Failed. See Console.", "OK");
            }
        }

        private void RunValidateAddressablesQuest()
        {
            var quest = GetSelectedQuest();
            if (quest == null)
            { EditorUtility.DisplayDialog("Validate Addressables", "Select a quest first.", "OK"); return; }
            var locales = GetTargetLocales().ToList();
            if (locales.Count == 0)
            { EditorUtility.DisplayDialog("Validate Addressables", "No locales configured.", "OK"); return; }
            VoiceoverAddressablesValidator.ValidateQuest(quest, locales);
        }

        private void RunValidateAddressablesCards()
        {
            var locales = GetTargetLocales().ToList();
            if (locales.Count == 0)
            { EditorUtility.DisplayDialog("Validate Addressables (Cards)", "No locales configured.", "OK"); return; }
            VoiceoverAddressablesValidator.ValidateCards(locales);
        }

        private IEnumerator ConvertMp3ToOggCoroutine(QuestData quest, List<Locale> locales, string ffmpegPath, bool keepMp3)
        {
            int totalConverted = 0;
            foreach (var locale in locales)
            {
                string folder = GetQuestLangFolder(quest, locale);
                if (!AssetDatabase.IsValidFolder(folder))
                    continue;
                string[] mp3s = Directory.Exists(folder) ? Directory.GetFiles(folder, "*.mp3") : Array.Empty<string>();
                int idx = 0;
                int total = mp3s.Length;
                foreach (var mp3 in mp3s)
                {
                    idx++;
                    string nameNoExt = Path.GetFileNameWithoutExtension(mp3);
                    string ogg = Path.Combine(Path.GetDirectoryName(mp3), nameNoExt + ".ogg").Replace('\\', '/');
                    float denom = Mathf.Max(1f, total);
                    float progress = idx / denom;
                    bool canceled = EditorUtility.DisplayCancelableProgressBar("Convert MP3 → OGG", $"{quest.Id} [{locale.Identifier.Code}] {idx}/{total}: {Path.GetFileName(mp3)}", Mathf.Clamp01(progress));
                    if (canceled || _cancelRequested)
                    {
                        EditorUtility.ClearProgressBar();
                        _isRunning = false;
                        EditorUtility.DisplayDialog("Convert MP3 → OGG", "Canceled.", "OK");
                        yield break;
                    }

                    if (File.Exists(ogg))
                        continue;

                    if (!FFmpegUtils.ConvertMp3ToOgg(ffmpegPath, mp3, ogg, out string err))
                    { Debug.LogError($"[QVM] ffmpeg conversion failed: {err} for {mp3}"); continue; }
                    AssetDatabase.ImportAsset(ogg, ImportAssetOptions.ForceSynchronousImport);
                    ApplyVorbisQuality(ogg, 0.7f);
                    // Optional trimming after conversion
                    if (_trimSilence)
                    {
                        if (TryTrimFileWithFfmpeg(ffmpegPath, ogg, _trimThresholdDb, _trimTailDurationSec, _trimHeadAlso, _trimHeadDurationSec))
                        {
                            AssetDatabase.ImportAsset(ogg, ImportAssetOptions.ForceSynchronousImport);
                        }
                    }
                    totalConverted++;

                    var questAssets = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, locale);
                    if (questAssets != null)
                    {
                        var mp3Guid = AssetDatabase.AssetPathToGUID(mp3);
                        var entry = questAssets.Values.FirstOrDefault(v => string.Equals(v.Guid, mp3Guid, StringComparison.OrdinalIgnoreCase));
                        if (entry == null)
                        {
                            // New simplified format: questId_lineId
                            string lineIdShort = ExtractLastUnderscoreToken(nameNoExt);
                            string key = !string.IsNullOrEmpty(lineIdShort) ? ("line:" + lineIdShort) : null;
                            if (!string.IsNullOrEmpty(key))
                                entry = questAssets.GetEntry(key) ?? questAssets.AddEntry(key, string.Empty);
                        }
                        if (entry != null)
                        {
                            var oggGuid = AssetDatabase.AssetPathToGUID(ogg);
                            if (!string.IsNullOrEmpty(oggGuid))
                            {
                                entry.Guid = oggGuid;
                                EditorUtility.SetDirty(questAssets);
                                if (questAssets.SharedData != null)
                                    EditorUtility.SetDirty(questAssets.SharedData);
                            }
                            // Also update manifest using derived key
                            var audioFileName = Path.GetFileName(ogg);
                            var keyShort = entry?.SharedEntry?.Key;
                            if (!string.IsNullOrEmpty(keyShort) && keyShort.StartsWith("line:"))
                                keyShort = keyShort.Substring("line:".Length);
                            if (!string.IsNullOrEmpty(keyShort))
                            {
                                VoiceoverManifestUtil.Upsert(quest.Id, locale, key: keyShort, audioFileName: audioFileName, textHash: null, durationMs: null, voiceProfileId: null, actorId: null, nodeTitle: null, sourceText: null);
                            }
                        }
                    }

                    if (!keepMp3)
                    { AssetDatabase.DeleteAsset(mp3); }
                }
                AssetDatabase.SaveAssets();
            }
            EditorUtility.ClearProgressBar();
            _isRunning = false;
            EditorUtility.DisplayDialog("Convert MP3 → OGG", $"Done. Converted {totalConverted} file(s).", "OK");
        }

        private IEnumerator TrimSilenceCoroutine(QuestData quest, List<Locale> locales, string ffmpegPath, float thresholdDb, float tailMinSec, bool trimHead, float headMinSec)
        {
            int totalTrimmed = 0;
            foreach (var locale in locales)
            {
                string folder = GetQuestLangFolder(quest, locale);
                if (!AssetDatabase.IsValidFolder(folder))
                    continue;
                string[] clips = Directory.Exists(folder)
                    ? Directory.GetFiles(folder, "*.*").Where(p => p.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) || p.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase) || p.EndsWith(".wav", StringComparison.OrdinalIgnoreCase)).ToArray()
                    : Array.Empty<string>();
                int idx = 0;
                int total = clips.Length;
                foreach (var path in clips)
                {
                    idx++;
                    float denom = Mathf.Max(1f, total);
                    float progress = idx / denom;
                    bool canceled = EditorUtility.DisplayCancelableProgressBar("Trim Silence", $"{quest.Id} [{locale.Identifier.Code}] {idx}/{total}: {Path.GetFileName(path)}", Mathf.Clamp01(progress));
                    if (canceled || _cancelRequested)
                    {
                        EditorUtility.ClearProgressBar();
                        _isRunning = false;
                        EditorUtility.DisplayDialog("Trim Silence", "Canceled.", "OK");
                        yield break;
                    }

                    if (TryTrimFileWithFfmpeg(ffmpegPath, path, thresholdDb, tailMinSec, trimHead, headMinSec))
                    {
                        totalTrimmed++;
                        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
                    }
                }
                AssetDatabase.SaveAssets();
            }
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
            _isRunning = false;
            EditorUtility.DisplayDialog("Trim Silence", $"Done. Trimmed {totalTrimmed} file(s).", "OK");
        }

        private static bool TryTrimFileWithFfmpeg(string ffmpegPath, string assetPath, float thresholdDb, float tailMinSec, bool trimHead, float headMinSec)
        {
            if (string.IsNullOrEmpty(ffmpegPath) || string.IsNullOrEmpty(assetPath))
                return false;
            var ext = Path.GetExtension(assetPath).ToLowerInvariant();
            if (ext != ".mp3" && ext != ".ogg" && ext != ".wav")
                return false;

            string input = Path.GetFullPath(assetPath);
            string tmpOut = input + ".trimtmp" + ext;

            string headFilter = trimHead ? $"silenceremove=start_periods=1:start_duration={headMinSec.ToString(System.Globalization.CultureInfo.InvariantCulture)}:start_threshold={thresholdDb}dB" : null;
            string tailFilter = $"areverse,silenceremove=start_periods=1:start_duration={tailMinSec.ToString(System.Globalization.CultureInfo.InvariantCulture)}:start_threshold={thresholdDb}dB,areverse";
            string filter = !string.IsNullOrEmpty(headFilter) ? headFilter + "," + tailFilter : tailFilter;

            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-y -i \"{input}\" -af \"{filter}\" \"{tmpOut}\"",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            try
            {
                using (var proc = System.Diagnostics.Process.Start(psi))
                {
                    proc.WaitForExit();
                    if (proc.ExitCode == 0 && File.Exists(tmpOut))
                    {
                        File.Copy(tmpOut, input, true);
                        File.Delete(tmpOut);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[QVM] ffmpeg trim failed for {assetPath}: {ex.Message}");
            }
            finally
            {
                if (File.Exists(tmpOut))
                {
                    try
                    { File.Delete(tmpOut); }
                    catch { }
                }
            }
            return false;
        }

        private static string GetQuestLangFolder(QuestData quest, Locale locale)
        {
            var path = Path.Combine(LangBundlesRoot, SanitizeFolderName(quest.Id ?? quest.name), locale.Identifier.Code);
            return path.Replace('\\', '/');
        }

        private static void EnsureFolder(string folder)
        {
            if (AssetDatabase.IsValidFolder(folder))
                return;
            var parts = folder.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }

        private static string MaskApiKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;
            if (key.Length <= 6)
                return new string('*', key.Length);
            return key.Substring(0, 3) + new string('*', key.Length - 6) + key[^3..];
        }

        private static string SanitizeFolderName(string name)
        {
            var invalid = new string(Path.GetInvalidFileNameChars()) + "/\\";
            var safe = new string(name.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray());
            return safe;
        }

        private static string SanitizeFileNamePart(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "untitled";
            name = name.Trim();
            name = Regex.Replace(name, "\\s+", "_");
            var invalid = new string(Path.GetInvalidFileNameChars()) + "/\\";
            name = new string(name.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray());
            return name;
        }

        private static string BuildFileBase(string questId, string nodeTitle, string lineIdShort)
        {
            return $"{SanitizeFileNamePart(questId)}_{SanitizeFileNamePart(nodeTitle)}_{SanitizeFileNamePart(lineIdShort)}";
        }

        private static string BuildFileBaseWithActor(string questId, string nodeTitle, string lineIdShort, string actorName)
        {
            return $"{SanitizeFileNamePart(questId)}_{SanitizeFileNamePart(nodeTitle)}_{SanitizeFileNamePart(lineIdShort)}_{SanitizeFileNamePart(actorName)}";
        }

        private static string CombinePath(string folder, string file)
        {
            return folder.TrimEnd('/') + "/" + file;
        }

        private static string ResolveNodeTitleForKey(string key, Dictionary<string, string> nodeMap)
        {
            if (string.IsNullOrEmpty(key))
                return "node";
            if (nodeMap != null && nodeMap.TryGetValue(key, out var title) && !string.IsNullOrEmpty(title))
                return title;
            return "node";
        }

        private static string StripLinePrefix(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;
            return key.StartsWith("line:", StringComparison.OrdinalIgnoreCase) ? key.Substring("line:".Length) : key;
        }

        private static string ExtractLastUnderscoreToken(string nameNoExt)
        {
            if (string.IsNullOrEmpty(nameNoExt))
                return string.Empty;
            int idx = nameNoExt.LastIndexOf('_');
            if (idx >= 0 && idx + 1 < nameNoExt.Length)
                return nameNoExt[(idx + 1)..];
            return nameNoExt;
        }

        private static string ExtractNthTokenFromEnd(string nameNoExt, int nFromEnd)
        {
            if (string.IsNullOrEmpty(nameNoExt) || nFromEnd < 1)
                return string.Empty;
            // Support both legacy underscore and new hyphen separators
            var tokens = nameNoExt.Contains('-') ? nameNoExt.Split('-') : nameNoExt.Split('_');
            if (tokens.Length < nFromEnd)
                return string.Empty;
            return tokens[tokens.Length - nFromEnd];
        }

        private static bool MatchesLineSearch(string key, string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return true;
            if (string.IsNullOrEmpty(key))
                return false;
            // Case-insensitive contains; allows searching raw id (e.g., "02846e8") against keys like "line:02846e8".
            return key.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string NormalizeLineKey(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return string.Empty;
            token = token.Trim();
            return token.StartsWith("line:", StringComparison.OrdinalIgnoreCase) ? token : ("line:" + token);
        }

        private void TryPingAudioForLine(QuestData quest, Locale locale, string lineKey)
        {
            if (quest == null || locale == null || string.IsNullOrEmpty(lineKey))
                return;
            var at = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, locale);
            var entry = at != null ? at.GetEntry(lineKey) : null;
            if (entry == null || string.IsNullOrEmpty(entry.Guid))
            { EditorUtility.DisplayDialog("Ping Audio", "No audio assigned to this line.", "OK"); return; }
            var path = AssetDatabase.GUIDToAssetPath(entry.Guid);
            var obj = string.IsNullOrEmpty(path) ? null : AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if (obj != null)
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
            }
            else
            {
                EditorUtility.DisplayDialog("Ping Audio", "Assigned audio not found on disk.", "OK");
            }
        }

        private void RunCreateAudioForSelectedLine(string lineKey)
        {
            _onlyLineKey = lineKey;
            _forceRecreateSelected = true;
            RunCreateAudioFiles();
        }

        private int ComputeEligibleCreateCount(QuestData quest, Locale locale, YarnLineMapBuilder.YarnLineMeta meta, bool onlyMissing, bool convertToOgg, bool overwrite)
        {
            int count = 0;
            try
            {
                var st = LocalizationSettings.StringDatabase.GetTable(quest.QuestStringsTable.TableReference, locale);
                var at = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, locale);
                if (st == null || at == null)
                    return 0;
                string folder = GetQuestLangFolder(quest, locale);
                EnsureFolder(folder);
                foreach (var e in st.Values.Where(v => v != null && v.SharedEntry != null))
                {
                    string key = e.SharedEntry.Key;
                    string text = e.Value;
                    if (string.IsNullOrWhiteSpace(text))
                        continue;
                    string idShort = StripLinePrefix(key);
                    string title = ResolveNodeTitleForKey(idShort, meta.Titles);
                    // Apply search filter by Yarn node title only (include all lines of matched nodes)
                    if (!string.IsNullOrWhiteSpace(_search))
                    {
                        bool matchesTitle = (title?.IndexOf(_search, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0;
                        if (!matchesTitle)
                            continue;
                    }
                    // Determine the voice id used in filenames for this line
                    VoiceProfileData defaultVoice = _voiceCatalog != null ? _voiceCatalog.GetProfile(locale, _selectedActor) : _voiceProfile;
                    var useVoice = defaultVoice;
                    if (meta.Actors != null && meta.Actors.TryGetValue(idShort, out var actorOpt) && actorOpt.HasValue)
                    {
                        var vp = _voiceCatalog != null ? _voiceCatalog.GetProfile(locale.Identifier.Code, actorOpt.Value) : null;
                        if (vp != null)
                            useVoice = vp;
                    }
                    // Filenames simplified: questId_lineId only
                    string baseName = $"{SanitizeFileNamePart(quest.Id)}_{SanitizeFileNamePart(idShort)}";
                    string finalExt = convertToOgg ? ".ogg" : ".mp3";
                    string finalPath = CombinePath(folder, baseName + finalExt);
                    if (onlyMissing)
                    {
                        bool fileExists = File.Exists(finalPath);
                        var entry = at.GetEntry(key);
                        bool guidExists = entry != null && !string.IsNullOrEmpty(entry.Guid) && !string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(entry.Guid));
                        if (!fileExists && !guidExists)
                            count++;
                    }
                    else if (overwrite || !File.Exists(finalPath))
                    {
                        count++;
                    }
                }
            }
            catch { }
            return count;
        }


    }

    // ------------------------- Addressables VO Service -------------------------
    internal sealed class AddressablesVoService
    {
        private const string LocalizationAssetTablesPrefix = "Localization-Assets-";

        public void UpdateQuestAddressables(QuestData quest, List<Locale> locales)
        {
            if (quest == null || locales == null || locales.Count == 0)
                return;
            foreach (var locale in locales)
            {
                var at = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, locale);
                if (at == null)
                    continue;
                var entries = at.Values;
                int idx = 0;
                int total = entries.Count;
                foreach (var e in entries)
                {
                    idx++;
                    if (string.IsNullOrEmpty(e.Guid))
                        continue;
                    var path = AssetDatabase.GUIDToAssetPath(e.Guid);
                    if (string.IsNullOrEmpty(path))
                        continue;
                    if (AssetDatabase.GetMainAssetTypeAtPath(path) != typeof(AudioClip))
                        continue;
                    EditorUtility.DisplayProgressBar("Update Addressables", $"{quest.Id} — {locale.Identifier.Code} ({idx}/{total})", Mathf.Clamp01((float)idx / Mathf.Max(1, total)));
                    var sharedKey = e.SharedEntry != null ? e.SharedEntry.Key : null;
                    UpdateAddressableForClip(locale, path, quest.Id, isQuestClip: true, keyOrId: sharedKey);
                }
            }
            EditorUtility.ClearProgressBar();
        }

        public void UpdateCardsAddressables(List<Locale> locales)
        {
            if (locales == null || locales.Count == 0)
                return;
            foreach (var locale in locales)
            {
                var at = LocalizationSettings.AssetDatabase.GetTable("Cards audio", locale);
                if (at == null)
                    continue;
                var entries = at.Values;
                int idx = 0;
                int total = entries.Count;
                foreach (var e in entries)
                {
                    idx++;
                    if (string.IsNullOrEmpty(e.Guid))
                        continue;
                    var path = AssetDatabase.GUIDToAssetPath(e.Guid);
                    if (string.IsNullOrEmpty(path))
                        continue;
                    if (AssetDatabase.GetMainAssetTypeAtPath(path) != typeof(AudioClip))
                        continue;
                    EditorUtility.DisplayProgressBar("Update Addressables (Cards)", $"{locale.Identifier.Code} ({idx}/{total})", Mathf.Clamp01((float)idx / Mathf.Max(1, total)));
                    var idFromKey = e.SharedEntry != null ? ExtractCardIdFromKey(e.SharedEntry.Key) : null;
                    UpdateAddressableForClip(locale, path, questIdOrNull: null, isQuestClip: false, keyOrId: idFromKey);
                }
            }
            EditorUtility.ClearProgressBar();
        }

        public void UpdateAddressableForClip(Locale locale, string assetPath, string questIdOrNull, bool isQuestClip, string keyOrId = null)
        {
            if (locale == null || string.IsNullOrEmpty(assetPath))
                return;
            var settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogWarning("[QVM] Addressables settings not found. Enable Addressables in the project first.");
                return;
            }

            var group = GetOrCreateLocaleAssetsGroup(settings, locale);
            if (group == null)
                return;

            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            if (string.IsNullOrEmpty(guid))
                return;

            var entry = settings.FindAssetEntry(guid);
            if (entry == null || entry.parentGroup != group)
            {
                entry = settings.CreateOrMoveEntry(guid, group, false, false);
            }

            // Address format
            string code = locale.Identifier.Code;
            string questId = questIdOrNull ?? ExtractQuestIdFromPath(assetPath);
            // Final segment should be quest line key (as line-<id>) or card id
            string lastSegment = null;
            if (!string.IsNullOrEmpty(keyOrId))
            {
                lastSegment = isQuestClip ? NormalizeQuestKey(keyOrId) : NormalizeCardId(keyOrId);
            }
            if (string.IsNullOrEmpty(lastSegment))
            {
                lastSegment = Path.GetFileNameWithoutExtension(assetPath);
            }
            string address = isQuestClip && !string.IsNullOrEmpty(questId)
                ? $"VO/{code}/quest/{questId}/{lastSegment}"
                : $"VO/{code}/cards/{lastSegment}";
            if (!string.Equals(entry.address, address, StringComparison.Ordinal))
            {
                entry.SetAddress(address, false);
            }

            // Labels
            EnsureLabel(settings, $"type:vo");
            EnsureLabel(settings, $"lang:{code}");
            EnsureLabel(settings, $"Locale-{code}"); // Required by Unity Localization
            entry.SetLabel($"type:vo", true, true);
            entry.SetLabel($"lang:{code}", true, true);
            entry.SetLabel($"Locale-{code}", true, true);
            var qid = questId;
            if (!string.IsNullOrEmpty(qid))
            {
                string qLabel = $"quest:{qid}";
                EnsureLabel(settings, qLabel);
                entry.SetLabel(qLabel, true, true);
            }

            EditorUtility.SetDirty(group);
            EditorUtility.SetDirty(settings);
        }

        private static void EnsureLabel(AddressableAssetSettings settings, string label)
        {
            if (settings == null || string.IsNullOrEmpty(label))
                return;
            var labels = settings.GetLabels();
            if (labels == null || !labels.Contains(label))
            {
                settings.AddLabel(label);
            }
        }

        private AddressableAssetGroup GetOrCreateLocaleAssetsGroup(AddressableAssetSettings settings, Locale locale)
        {
            string code = locale.Identifier.Code;
            string englishName = locale.Identifier.CultureInfo != null ? locale.Identifier.CultureInfo.EnglishName : (locale.name ?? code);
            // Preferred name created by Unity Localization
            string preferred = $"{LocalizationAssetTablesPrefix}{englishName} ({code})";

            // Try preferred exactly
            var group = settings.FindGroup(preferred);
            if (group != null)
                return group;

            // Try to find any group that matches known naming conventions for this locale
            group = settings.groups.FirstOrDefault(g => g != null && g.Name.EndsWith($"({code})", StringComparison.Ordinal) && g.Name.StartsWith("Localization-Assets-", StringComparison.Ordinal));
            if (group != null)
                return group;

            // As a last resort, create the preferred group to avoid dumping into Shared
            group = settings.CreateGroup(preferred, false, false, false, null,
                typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema),
                typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.ContentUpdateGroupSchema));
            Debug.Log($"[QVM] Created Addressables group '{preferred}'.");
            return group;
        }

        private static string ExtractQuestIdFromPath(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return string.Empty;
            // Look for /_quests/<questId>/  segment in the path
            var path = assetPath.Replace('\\', '/');
            var idx = path.IndexOf("/_quests/", StringComparison.OrdinalIgnoreCase);
            if (idx >= 0)
            {
                idx++;
                // remove leading underscore for label consistency
                var start = idx + "/quests/".Length;
                int end = path.IndexOf('/', start);
                if (end > start)
                {
                    return path.Substring(start, end - start);
                }
            }
            // Fallback to _lang_bundles/_quests/<questId>/ pattern
            var marker = "/_lang_bundles/_quests/";
            idx = path.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (idx >= 0)
            {
                var start = idx + marker.Length;
                int end = path.IndexOf('/', start);
                if (end > start)
                {
                    return path.Substring(start, end - start);
                }
            }
            return string.Empty;
        }

        private static string NormalizeQuestKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;
            if (key.StartsWith("line:", StringComparison.OrdinalIgnoreCase))
            {
                return SafeSegment("line-" + key.Substring("line:".Length));
            }
            if (key.StartsWith("line-", StringComparison.OrdinalIgnoreCase))
            {
                return SafeSegment(key);
            }
            return SafeSegment(key);
        }

        private static string NormalizeCardId(string keyOrId)
        {
            if (string.IsNullOrEmpty(keyOrId))
                return string.Empty;
            // strip trailing ".desc" etc.
            int dot = keyOrId.IndexOf('.');
            var id = dot > 0 ? keyOrId.Substring(0, dot) : keyOrId;
            return SafeSegment(id);
        }

        private static string ExtractCardIdFromKey(string sharedKey)
        {
            return NormalizeCardId(sharedKey);
        }

        private static string SafeSegment(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            var sb = new StringBuilder(s.Length);
            foreach (var ch in s)
            {
                if (char.IsLetterOrDigit(ch) || ch == '-' || ch == '_')
                    sb.Append(ch);
                else if (char.IsWhiteSpace(ch) || ch == ':' || ch == '/' || ch == '\\' || ch == '.')
                    sb.Append('-');
                else
                    sb.Append(ch);
            }
            var seg = Regex.Replace(sb.ToString(), "-+", "-").Trim('-');
            return string.IsNullOrEmpty(seg) ? "item" : seg;
        }
    }
}
#endif
