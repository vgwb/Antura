#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventurEd.Editor;
using UnityEditor;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Antura.Discover.Audio.Editor
{
    public class VoiceoverManagerWindow : EditorWindow
    {
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

        private ITtsService _tts = new ElevenLabsTtsService();

        private const string LangBundlesRoot = "Assets/_lang_bundles/_quests";

        [MenuItem("Tools/Discover/Quest Voiceover Manager")]
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

                var filtered = string.IsNullOrWhiteSpace(_search)
                    ? _quests
                    : _quests.Where(q =>
                        (!string.IsNullOrEmpty(q.Id) && q.Id.IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(q.TitleEn) && q.TitleEn.IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (q.name.IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();

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
                _voiceCatalog = (VoiceProfileCatalog)EditorGUILayout.ObjectField("Voice Catalog (opt)", _voiceCatalog, typeof(VoiceProfileCatalog), false);
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

                // Actions
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Status", GUILayout.Height(24)))
                        RunStatus();
                    if (GUILayout.Button("Clear and Init", GUILayout.Height(24)))
                        RunClearAndInit();
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
            }
        }

        private QuestData GetSelectedQuest()
        {
            var candidates = string.IsNullOrWhiteSpace(_search)
                ? _quests
                : _quests.Where(q =>
                    (!string.IsNullOrEmpty(q.Id) && q.Id.IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(q.TitleEn) && q.TitleEn.IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (q.name.IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();
            if (candidates.Count == 0)
                return null;
            return candidates[Mathf.Clamp(_selectedQuestIndex, 0, candidates.Count - 1)];
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
                    string lineIdShort = StripLinePrefix(key);
                    string nodeTitle = ResolveNodeTitleForKey(lineIdShort, meta.Titles);
                    // Resolve voice per line based on Yarn actor header when available
                    VoiceProfileData voiceForLine = localeDefaultVoice;
                    if (meta.Actors != null && meta.Actors.TryGetValue(lineIdShort, out var actorOpt) && actorOpt.HasValue)
                    {
                        var v = catalog != null ? catalog.GetProfile(locale.Identifier.Code, actorOpt.Value) : null;
                        if (v != null)
                            voiceForLine = v;
                    }
                    string voiceIdForName = !string.IsNullOrEmpty(voiceForLine?.Id) ? voiceForLine.Id : (localeDefaultVoice?.Id ?? "default");
                    string fileBase = BuildFileBaseWithVoice(quest.Id, nodeTitle, lineIdShort, voiceIdForName);
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
                                var gogg = AssetDatabase.AssetPathToGUID(finalAssetPath);
                                var eAssign = at.GetEntry(key) ?? at.AddEntry(key, gogg ?? string.Empty);
                                if (!string.IsNullOrEmpty(gogg))
                                    eAssign.Guid = gogg;
                                if (!keepMp3)
                                    AssetDatabase.DeleteAsset(mp3TempPath);
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

                    // Log one clickable line per generated file (ping in Project on click)
                    if (createdNow)
                    {
                        var obj = AssetDatabase.LoadAssetAtPath<AudioClip>(assignPath);
                        if (obj != null)
                            Debug.Log($"[QVM] Created [{locale.Identifier.Code}] {Path.GetFileName(assignPath)} ← {assignPath}", obj);
                        else
                            Debug.Log($"[QVM] Created [{locale.Identifier.Code}] {Path.GetFileName(assignPath)} ← {assignPath}");
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
                    totalConverted++;

                    var questAssets = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, locale);
                    if (questAssets != null)
                    {
                        var mp3Guid = AssetDatabase.AssetPathToGUID(mp3);
                        var entry = questAssets.Values.FirstOrDefault(v => string.Equals(v.Guid, mp3Guid, StringComparison.OrdinalIgnoreCase));
                        if (entry == null)
                        {
                            // Filenames are quest_node_line_voiceId; try last token, then second-last as fallback
                            string lineIdShort = ExtractNthTokenFromEnd(nameNoExt, 2);
                            if (string.IsNullOrEmpty(lineIdShort))
                                lineIdShort = ExtractLastUnderscoreToken(nameNoExt);
                            string key = !string.IsNullOrEmpty(lineIdShort) ? ("line:" + lineIdShort) : null;
                            if (!string.IsNullOrEmpty(key))
                                entry = questAssets.GetEntry(key) ?? questAssets.AddEntry(key, string.Empty);
                        }
                        if (entry != null)
                        {
                            var oggGuid = AssetDatabase.AssetPathToGUID(ogg);
                            if (!string.IsNullOrEmpty(oggGuid))
                            { entry.Guid = oggGuid; EditorUtility.SetDirty(questAssets); }
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

        private static string BuildFileBaseWithVoice(string questId, string nodeTitle, string lineIdShort, string voiceProfileId)
        {
            return $"{SanitizeFileNamePart(questId)}_{SanitizeFileNamePart(nodeTitle)}_{SanitizeFileNamePart(lineIdShort)}_{SanitizeFileNamePart(voiceProfileId)}";
        }

        private static string CombinePath(string folder, string file)
        {
            return (folder.TrimEnd('/') + "/" + file);
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
            var tokens = nameNoExt.Split('_');
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
                    // Determine the voice id used in filenames for this line
                    VoiceProfileData defaultVoice = _voiceCatalog != null ? _voiceCatalog.GetProfile(locale, _selectedActor) : _voiceProfile;
                    var useVoice = defaultVoice;
                    if (meta.Actors != null && meta.Actors.TryGetValue(idShort, out var actorOpt) && actorOpt.HasValue)
                    {
                        var vp = _voiceCatalog != null ? _voiceCatalog.GetProfile(locale.Identifier.Code, actorOpt.Value) : null;
                        if (vp != null)
                            useVoice = vp;
                    }
                    string voiceIdForName = !string.IsNullOrEmpty(useVoice?.Id) ? useVoice.Id : (defaultVoice?.Id ?? "default");
                    string baseName = BuildFileBaseWithVoice(quest.Id, title, idShort, voiceIdForName);
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
}
#endif
