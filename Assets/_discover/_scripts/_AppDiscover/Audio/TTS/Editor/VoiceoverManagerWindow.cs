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
        // UI state
        private Vector2 _scroll;

        // Generation options
        private bool _onlyGenerateMissing = true; // Only generate files that are missing
        private bool _convertToOgg = false;
        private string _ffmpegPath = "ffmpeg";
        private bool _keepMp3AfterConversion = false;

        private const string PrefKeyFfmpegPath = "Antura.Audio.QVM.ffmpegPath";

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
        private VoiceProfileData _voiceProfile;
        private VoiceProfileCatalog _voiceCatalog; // Optional per-language/actor
        private VoiceActors _selectedActor = VoiceActors.Default;
        private LocalSecrets _secrets;
        private bool _overwriteExisting = false;
        private int _createCapIndex = 2; // 0=1, 1=5, 2=All
        private bool _showPreviewCounts = true;
        private bool _cardsIncludeDescriptions = false;

        private ITtsService _tts = new ElevenLabsTtsService();

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

                var questNames = filtered.Select(q => string.IsNullOrEmpty(q.Id) ? q.name : q.Id).ToArray();
                if (questNames.Length == 0)
                { EditorGUILayout.HelpBox("No quests found.", MessageType.Info); return; }
                _selectedQuestIndex = Mathf.Clamp(_selectedQuestIndex, 0, questNames.Length - 1);
                _selectedQuestIndex = EditorGUILayout.Popup("Quest", _selectedQuestIndex, questNames);

                var quest = filtered[_selectedQuestIndex];

                // Locale selection
                var localeNames = new List<string> { "All" };
                localeNames.AddRange(_locales.Select(l => l.Identifier.Code));
                _selectedLocaleIndex = EditorGUILayout.Popup("Locale", _selectedLocaleIndex, localeNames.ToArray());

                // Voice
                _voiceProfile = (VoiceProfileData)EditorGUILayout.ObjectField("Voice Profile", _voiceProfile, typeof(VoiceProfileData), false);
                _voiceCatalog = (VoiceProfileCatalog)EditorGUILayout.ObjectField("Voice Catalog", _voiceCatalog, typeof(VoiceProfileCatalog), false);
                _selectedActor = (VoiceActors)EditorGUILayout.EnumPopup("Actor", _selectedActor);
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
                    if (GUILayout.Button("Create audio files", GUILayout.Height(24)))
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
                    if (GUILayout.Button("Create card audio (Title + Desc)", GUILayout.Height(22)))
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
            foreach (var card in cards)
            {
                string baseName = SanitizeFileNamePart(card.Id ?? card.name);
                string cardFolder = (CardsBundlesRoot + "/" + baseName).Replace('\\', '/');
                EnsureFolder(cardFolder);

                foreach (var locale in locales)
                {
                    VoiceProfileData voice = null;
                    IVoiceProvider provider = catalog != null ? catalog : VoiceProviderManager.I?.Provider;
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
                    if (provider != null)
                        voice = provider.GetProfile(locale, VoiceActors.Default);
                    if (voice == null)
                        voice = fallbackVoice;
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

                    int attempts = 0;
                    foreach (var (kind, text) in items)
                    {
                        if (attempts >= maxToCreate)
                            break;
                        attempts++;
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
                                    if (!keepMp3)
                                        AssetDatabase.DeleteAsset(mp3TempPath);
                                }
                            }
                            totalCreated++;
                        }
                        catch (Exception ex)
                        { Debug.LogError($"[QVM] Write/import failed for {finalAssetPath}: {ex.Message}"); continue; }

                        // Clickable log
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

                    var stringKeys = new HashSet<string>(st?.Values.Select(e => e.SharedEntry?.Key).Where(k => !string.IsNullOrEmpty(k)) ?? Enumerable.Empty<string>());
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

                    int assigned = 0;
                    foreach (var e in at?.Values ?? Enumerable.Empty<AssetTableEntry>())
                    {
                        if (!string.IsNullOrEmpty(e.Guid))
                        {
                            var p = AssetDatabase.GUIDToAssetPath(e.Guid);
                            if (!string.IsNullOrEmpty(p))
                                assigned++;
                        }
                    }
                    report.AppendLine($"  Assigned audio assets: {assigned}/{stringKeys.Count}");
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
            if (requireVoice && _voiceProfile == null)
            { EditorUtility.DisplayDialog("Quest Voiceover", "Please select a VoiceProfile.", "OK"); return false; }
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

                // Default voice for this locale (used when no per-line actor override is available)
                var localeDefaultVoice = catalog != null ? catalog.GetProfile(locale, actor) : null;
                if (localeDefaultVoice == null)
                    localeDefaultVoice = fallbackVoice;
                if (localeDefaultVoice == null)
                { Debug.LogError($"[QVM] No voice profile for {locale.Identifier.Code}"); continue; }
                if (st == null || at == null)
                { Debug.LogWarning($"[QVM] Missing tables for {quest.Id} / {locale.Identifier.Code}"); continue; }

                var entries = st.Values.Where(e => e != null && e.SharedEntry != null).ToList();
                int total = entries.Count;
                int processed = 0;

                var folder = GetQuestLangFolder(quest, locale);
                EnsureFolder(folder);

                foreach (var e in entries)
                {
                    if (attemptsThisLocale >= maxToCreate)
                        break;

                    processed++;
                    string key = e.SharedEntry.Key;
                    string text = e.Value;
                    if (string.IsNullOrWhiteSpace(text))
                        continue; // skip empty localized strings
                    string lineIdShort = StripLinePrefix(key);
                    string nodeTitle = ResolveNodeTitleForKey(lineIdShort, meta.Titles);
                    // When searching, only process lines whose Yarn node title OR lineId matches the filter
                    if (!string.IsNullOrWhiteSpace(_search))
                    {
                        bool matchesTitle = (nodeTitle?.IndexOf(_search, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0;
                        bool matchesLineId = (!string.IsNullOrEmpty(lineIdShort) && lineIdShort.IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0);
                        if (!matchesTitle && !matchesLineId)
                            continue;
                    }
                    // Resolve voice per line based on Yarn actor header when available
                    VoiceProfileData voiceForLine = localeDefaultVoice;
                    if (meta.Actors != null && meta.Actors.TryGetValue(lineIdShort, out var actorOpt) && actorOpt.HasValue)
                    {
                        var v = catalog != null ? catalog.GetProfile(locale.Identifier.Code, actorOpt.Value) : null;
                        if (v != null)
                            voiceForLine = v;
                    }
                    VoiceActors actorForFile = _selectedActor;
                    if (meta.Actors != null && meta.Actors.TryGetValue(lineIdShort, out var aOpt) && aOpt.HasValue)
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

                    bool skipExisting = onlyMissing || !overwrite;
                    if (skipExisting && File.Exists(finalAssetPath))
                    {
                        var guid = AssetDatabase.AssetPathToGUID(finalAssetPath);
                        var entry = at.GetEntry(key) ?? at.AddEntry(key, guid ?? string.Empty);
                        if (!string.IsNullOrEmpty(guid))
                            entry.Guid = guid;
                        // Manifest upsert for existing file path
                        var audioFileName = Path.GetFileName(finalAssetPath);
                        var normText = VoiceoverManifestUtil.NormalizeText(text);
                        var textHash = VoiceoverManifestUtil.ComputeTextHash(normText, localeDefaultVoice?.Id, actorForFile.ToString(), locale.Identifier.Code);
                        VoiceoverManifestUtil.Upsert(quest.Id, locale, key: StripLinePrefix(key), audioFileName: audioFileName, textHash: textHash, durationMs: null, voiceProfileId: localeDefaultVoice?.Id, actorId: actorForFile.ToString(), nodeTitle: nodeTitle, sourceText: text);
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
                            }
                            else
                            {
                                AssetDatabase.ImportAsset(finalAssetPath, ImportAssetOptions.ForceSynchronousImport);
                                ApplyVorbisQuality(finalAssetPath, 0.7f);
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
                            continue;
                        }
                    }

                    attemptsThisLocale++;

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
                            { entry.Guid = oggGuid; EditorUtility.SetDirty(questAssets); }
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
                    // Apply search filter by Yarn node title OR lineId if present
                    if (!string.IsNullOrWhiteSpace(_search))
                    {
                        bool matchesTitle = (title?.IndexOf(_search, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0;
                        bool matchesLineId = (!string.IsNullOrEmpty(idShort) && idShort.IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0);
                        if (!matchesTitle && !matchesLineId)
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
        // Unity Localization creates per-locale groups like: "Localization-Assets-Tables-English (en)".
        private const string LocalizationAssetTablesPrefix = "Localization-Assets-Tables-";

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

            // Try to find any group that matches the naming convention for this locale
            group = settings.groups.FirstOrDefault(g => g != null && g.Name.EndsWith($"({code})", StringComparison.Ordinal) && g.Name.StartsWith("Localization-Assets-Tables-", StringComparison.Ordinal));
            if (group != null)
                return group;

            // Fallback to older prefix used previously (kept for compatibility)
            string legacy = $"Localization-Assets-{englishName} ({code})";
            group = settings.FindGroup(legacy);
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
            // Look for /_quests/<questId>/ or /quests/<questId>/ segment in the path
            var path = assetPath.Replace('\\', '/');
            var idx = path.IndexOf("/quests/", StringComparison.OrdinalIgnoreCase);
            if (idx < 0)
            {
                idx = path.IndexOf("/_quests/", StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                    idx++; // remove leading underscore for label consistency
            }
            if (idx >= 0)
            {
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
