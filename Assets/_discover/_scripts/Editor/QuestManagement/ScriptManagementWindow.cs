#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Antura.Discover;
using Antura.Discover.Audio;
using Antura.Discover.Audio.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using Newtonsoft.Json;

namespace Antura.Discover.EditorTools
{
    /// <summary>
    /// Cross-validates Yarn lines against String/Asset tables and provides per-locale controls.
    /// </summary>
    public class ScriptManagementWindow : EditorWindow
    {
        private Vector2 _scroll;

        // Data sources
        private readonly List<QuestData> _quests = new();
        private readonly List<Locale> _locales = new();
        private readonly Dictionary<string, CardData> _cardsById = new(StringComparer.Ordinal);

        // Selection
        private int _selectedQuest = 0;
        // Multi-select locales by code
        private readonly HashSet<string> _selectedLocaleCodes = new(StringComparer.OrdinalIgnoreCase);
        private string _filter = string.Empty;

        // View mode
        private enum ViewMode { Dialogue = 0, Card = 1 }
        private ViewMode _viewMode = ViewMode.Dialogue;

        // Audio preview
        private AudioSource _previewSource;
        private float _previewVolume = 1f;

        // Manifest cache (questId|lang -> lines by key)
        // Uses a local DTO (VoiceoverManifestUtil_ManifestEntry) defined inside this class
        private readonly Dictionary<string, Dictionary<string, VoiceoverManifestUtil_ManifestEntry>> _manifestCache = new();

        // Column widths for Dialogue table
        private const float ColNodeTitleW = 150f;
        private const float ColLineIdW = 100f;
        private const float ColTextW = 220f;
        private const float ColStringsW = 180f;

        [MenuItem("Antura/Quest/Script Management", priority = 22)]
        public static void ShowWindow()
        {
            var w = GetWindow<ScriptManagementWindow>(false, "Script Management", true);
            w.minSize = new Vector2(700, 420);
            w.Show();
        }

        private void OnEnable()
        {
            RefreshAll();
        }

        private void OnDisable()
        {
            if (_previewSource != null)
            {
                var go = _previewSource.gameObject;
                _previewSource = null;
                if (go != null)
                    DestroyImmediate(go);
            }
        }

        private void RefreshAll()
        {
            RefreshQuests();
            RefreshLocales();
            BuildCardsIndex();
            _manifestCache.Clear();
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
            _selectedQuest = Mathf.Clamp(_selectedQuest, 0, Math.Max(0, _quests.Count - 1));
        }

        private void RefreshLocales()
        {
            _locales.Clear();
            var list = LocalizationSettings.AvailableLocales?.Locales;
            if (list != null)
                _locales.AddRange(list);
            _locales.Sort((a, b) => string.Compare(a.Identifier.Code, b.Identifier.Code, StringComparison.OrdinalIgnoreCase));
            // Keep selection in sync; if nothing selected, default to all
            var existing = new HashSet<string>(_locales.Select(l => l.Identifier.Code), StringComparer.OrdinalIgnoreCase);
            _selectedLocaleCodes.RemoveWhere(code => !existing.Contains(code));
            if (_selectedLocaleCodes.Count == 0)
            {
                // Default to English if present, otherwise pick the first available locale
                var en = GetEnglishLocale();
                if (en != null)
                {
                    _selectedLocaleCodes.Add(en.Identifier.Code);
                }
                else if (_locales.Count > 0)
                {
                    _selectedLocaleCodes.Add(_locales[0].Identifier.Code);
                }
            }
        }

        private void BuildCardsIndex()
        {
            _cardsById.Clear();
            foreach (var guid in AssetDatabase.FindAssets("t:CardData"))
            {
                var p = AssetDatabase.GUIDToAssetPath(guid);
                var a = AssetDatabase.LoadAssetAtPath<CardData>(p);
                if (a != null && !string.IsNullOrEmpty(a.Id))
                {
                    if (!_cardsById.ContainsKey(a.Id))
                        _cardsById.Add(a.Id, a);
                }
            }
        }

        private void EnsurePreviewSource()
        {
            if (_previewSource != null)
                return;
            var go = EditorUtility.CreateGameObjectWithHideFlags("Script Preview Audio", HideFlags.HideAndDontSave, typeof(AudioSource));
            var src = go.GetComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = false;
            src.spatialBlend = 0f;
            src.volume = _previewVolume;
            _previewSource = src;
        }

        private IEnumerable<Locale> GetTargetLocales()
        {
            if (_selectedLocaleCodes == null || _selectedLocaleCodes.Count == 0)
                return Array.Empty<Locale>();
            return _locales.Where(l => _selectedLocaleCodes.Contains(l.Identifier.Code));
        }

        private static string NormalizeLineKey(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return string.Empty;
            token = token.Trim();
            return token.StartsWith("line:", StringComparison.Ordinal) ? token : ($"line:{token}");
        }

        private static string StripLinePrefix(string key)
        {
            if (string.IsNullOrEmpty(key))
                return key;
            return key.StartsWith("line:", StringComparison.Ordinal) ? key.Substring(5) : key;
        }

        private void OnGUI()
        {
            using (var scroll = new EditorGUILayout.ScrollViewScope(_scroll))
            {
                _scroll = scroll.scrollPosition;

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Filter", GUILayout.Width(48));
                    _filter = EditorGUILayout.TextField(_filter, GUILayout.Width(200));
                    if (GUILayout.Button("×", GUILayout.Width(22)))
                    { _filter = string.Empty; GUI.FocusControl(null); }
                    if (GUILayout.Button("Refresh", GUILayout.Width(90)))
                        RefreshAll();
                }

                if (_quests.Count == 0)
                {
                    EditorGUILayout.HelpBox("No quests found.", MessageType.Info);
                    return;
                }

                var questNames = _quests.Select(q =>
                {
                    var id = string.IsNullOrEmpty(q.Id) ? q.name : q.Id;
                    var title = q.TitleEn;
                    return string.IsNullOrWhiteSpace(title) ? id : ($"{id} — {title}");
                }).ToArray();
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Quest", GUILayout.Width(48));
                    _selectedQuest = EditorGUILayout.Popup(_selectedQuest, questNames, GUILayout.Width(200));
                    if (GUILayout.Button("Ping", GUILayout.Width(50)))
                    {
                        if (_selectedQuest >= 0 && _selectedQuest < _quests.Count && _quests[_selectedQuest] != null)
                            PingObject(_quests[_selectedQuest]);
                    }
                }
                var quest = _quests[_selectedQuest];

                // View mode selector
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("View", GUILayout.Width(48));
                    var idx = GUILayout.Toolbar((int)_viewMode, new[] { "Dialogue", "Card" }, GUILayout.Width(200));
                    _viewMode = (ViewMode)idx;
                }

                DrawLocalesSelector();

                // Ping helpers for selected locale tables
                DrawPingTablesRow(quest);

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Preview volume", GUILayout.Width(110));
                    _previewVolume = EditorGUILayout.Slider(_previewVolume, 0f, 1f, GUILayout.Width(200));
                }

                if (quest == null || quest.QuestStringsTable == null || quest.QuestAssetsTable == null)
                {
                    EditorGUILayout.HelpBox("Quest must have both String and Asset tables assigned.", MessageType.Warning);
                    return;
                }

                // Summary and batch actions
                DrawSummaryAndBatchActions(quest);

                // Legend
                EditorGUILayout.LabelField("Legend: !!! = needs regeneration (string changed since audio was generated); 'missing' = no audio", EditorStyles.miniLabel);

                // Reload staleness from manifest
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Reload manifest (re-read _index.json)", GUILayout.Width(260)))
                    {
                        _manifestCache.Clear();
                        AssetDatabase.Refresh();
                        Repaint();
                    }
                }

                DrawLinesTable(quest);
                EditorGUILayout.Space(8);
                DrawOrphansSection(quest);
            }
        }

        private void DrawLocalesSelector()
        {
            EditorGUILayout.LabelField("Locales", EditorStyles.boldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("All", GUILayout.Width(50)))
                {
                    foreach (var l in _locales)
                        _selectedLocaleCodes.Add(l.Identifier.Code);
                }
                if (GUILayout.Button("None", GUILayout.Width(60)))
                {
                    _selectedLocaleCodes.Clear();
                }
            }
            // Checkboxes (wrap to multiple rows)
            int perRow = 6;
            int i = 0;
            using (new EditorGUILayout.VerticalScope("box"))
            {
                while (i < _locales.Count)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        for (int j = 0; j < perRow && i < _locales.Count; j++, i++)
                        {
                            var loc = _locales[i];
                            bool selected = _selectedLocaleCodes.Contains(loc.Identifier.Code);
                            bool newSelected = EditorGUILayout.ToggleLeft(GetLocaleDisplayName(loc), selected, GUILayout.Width(90));
                            if (newSelected != selected)
                            {
                                if (newSelected)
                                    _selectedLocaleCodes.Add(loc.Identifier.Code);
                                else
                                    _selectedLocaleCodes.Remove(loc.Identifier.Code);
                            }
                        }
                    }
                }
            }
        }

        private void DrawPingTablesRow(QuestData quest)
        {
            var selectedLocales = GetTargetLocales().ToList();
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(selectedLocales.Count != 1))
                {
                    GUILayout.Label("Ping tables (selected locale):", GUILayout.Width(180));
                    if (GUILayout.Button("String Table", GUILayout.Width(110)))
                    {
                        var st = LocalizationSettings.StringDatabase.GetTable(quest.QuestStringsTable.TableReference, selectedLocales[0]);
                        if (st != null)
                            EditorGUIUtility.PingObject(st);
                    }
                    if (GUILayout.Button("Asset Table", GUILayout.Width(110)))
                    {
                        var at = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, selectedLocales[0]);
                        if (at != null)
                            EditorGUIUtility.PingObject(at);
                    }
                }
            }
        }

        private sealed class AudioInfo
        {
            public bool HasEntry;
            public bool HasGuid;
            public string Path;
            public UnityEngine.Object Obj;
            public AudioClip Clip;
            public bool IsZeroLength;
            public bool MissingOnDisk; // Guid present but no asset found at path
        }

        // Represents one row in the dialogue table, preserving script order and shadow occurrences
        private struct RowItem
        {
            public string ShortId; // without prefix
            public bool IsShadow;  // true if it comes from a #shadow: occurrence
        }

        private AudioInfo GetAudioInfo(QuestData quest, Locale locale, string key)
        {
            var info = new AudioInfo();
            try
            {
                var at = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, locale);
                var e = at != null ? at.GetEntry(key) : null;
                if (e == null)
                {
                    info.HasEntry = false;
                    return info;
                }
                info.HasEntry = true;
                info.HasGuid = !string.IsNullOrEmpty(e.Guid);
                if (!info.HasGuid)
                    return info;
                info.Path = AssetDatabase.GUIDToAssetPath(e.Guid);
                if (string.IsNullOrEmpty(info.Path))
                {
                    info.MissingOnDisk = true;
                    return info;
                }
                info.Obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(info.Path);
                info.Clip = info.Obj as AudioClip;
                info.IsZeroLength = info.Clip != null && info.Clip.length <= 0.001f;
                return info;
            }
            catch
            {
                return info;
            }
        }

        private void DrawLinesTable(QuestData quest)
        {
            if (_viewMode == ViewMode.Card)
            {
                DrawCardTable(quest);
                return;
            }
            // Yarn metadata
            var meta = YarnLineMapBuilder.BuildMeta(quest);
            var rows = GetRowsInScriptOrder(quest, meta);
            if (!string.IsNullOrWhiteSpace(_filter))
            {
                var f = _filter.Trim();
                rows = rows.Where(r =>
                {
                    if (r.ShortId.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                    if (meta.Titles.TryGetValue(r.ShortId, out var title))
                        return title != null && title.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0;
                    return false;
                }).ToList();
            }

            var targetLocales = GetTargetLocales().ToList();
            if (targetLocales.Count == 0)
            {
                EditorGUILayout.HelpBox("No locales configured in Project Settings/Localization.", MessageType.Info);
                return;
            }

            // pick EN for the main text column; second text column if another locale is selected
            var enLocale = GetEnglishLocale();
            var displayLocale = enLocale ?? targetLocales.FirstOrDefault();
            var secondTextLocale = targetLocales.FirstOrDefault(l => displayLocale != null && !string.Equals(l.Identifier.Code, displayLocale.Identifier.Code, StringComparison.OrdinalIgnoreCase));

            // Table header (Dialogue mode)
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColNodeTitleW)))
                { GUILayout.Label("Node title", EditorStyles.boldLabel, GUILayout.Width(ColNodeTitleW)); }
                using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColLineIdW)))
                { GUILayout.Label("Line Id", EditorStyles.boldLabel, GUILayout.Width(ColLineIdW)); }
                using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColTextW)))
                { GUILayout.Label($"Text ({GetLocaleDisplayName(displayLocale)})", EditorStyles.boldLabel, GUILayout.Width(ColTextW)); }
                if (secondTextLocale != null)
                {
                    using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColTextW)))
                    { GUILayout.Label($"Text ({GetLocaleDisplayName(secondTextLocale)})", EditorStyles.boldLabel, GUILayout.Width(ColTextW)); }
                }
                if (targetLocales.Count > 1)
                {
                    using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColStringsW)))
                    { GUILayout.Label("Strings (per locale)", EditorStyles.boldLabel, GUILayout.Width(ColStringsW)); }
                    using (new EditorGUILayout.VerticalScope("box"))
                    { GUILayout.Label("Audio (per locale)", EditorStyles.boldLabel); }
                }
                else
                {
                    using (new EditorGUILayout.VerticalScope("box"))
                    { GUILayout.Label("Audio", EditorStyles.boldLabel); }
                }
            }

            foreach (var row in rows)
            {
                var shortId = row.ShortId;
                var key = (row.IsShadow ? "shadow:" : "line:") + shortId;
                var displayId = row.IsShadow ? ($"shadow:{shortId}") : shortId;
                var nodeTitle = meta.Titles.TryGetValue(shortId, out var t) ? t : string.Empty;

                using (new EditorGUILayout.HorizontalScope())
                {
                    // First col: node title
                    using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColNodeTitleW)))
                    { GUILayout.Label(nodeTitle, EditorStyles.wordWrappedLabel, GUILayout.Width(ColNodeTitleW)); }
                    // Selectable line id
                    using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColLineIdW)))
                    { EditorGUILayout.SelectableLabel(displayId, GUILayout.Height(18)); }

                    // Shadow alias row: show empty text and keep columns aligned
                    if (row.IsShadow)
                    {
                        var oldCol = GUI.color;
                        GUI.color = Color.gray;
                        using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColTextW)))
                        { GUILayout.Label(string.Empty, EditorStyles.wordWrappedLabel, GUILayout.Width(ColTextW)); }
                        if (secondTextLocale != null)
                        {
                            using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColTextW)))
                            { GUILayout.Label(string.Empty, EditorStyles.wordWrappedLabel, GUILayout.Width(ColTextW)); }
                        }
                        GUI.color = oldCol;

                        if (targetLocales.Count > 1)
                        {
                            using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColStringsW)))
                            { /* leave empty */ }
                            using (new EditorGUILayout.VerticalScope("box"))
                            { /* leave empty */ }
                        }
                        else
                        {
                            using (new EditorGUILayout.VerticalScope("box"))
                            { /* leave empty */ }
                        }
                        continue;
                    }

                    string entryTextEn = displayLocale != null ? (GetStringValue(quest, displayLocale, key) ?? string.Empty) : string.Empty;
                    using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColTextW)))
                    {
                        GUILayout.Label(entryTextEn, EditorStyles.wordWrappedLabel, GUILayout.Width(ColTextW));
                        if (displayLocale != null && IsLineStale(quest, displayLocale, key))
                        { GUILayout.Label("!!!", GUILayout.Width(26)); }
                    }
                    if (secondTextLocale != null)
                    {
                        string entryText2 = GetStringValue(quest, secondTextLocale, key) ?? string.Empty;
                        using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColTextW)))
                        {
                            GUILayout.Label(entryText2, EditorStyles.wordWrappedLabel, GUILayout.Width(ColTextW));
                            if (IsLineStale(quest, secondTextLocale, key))
                            { GUILayout.Label("!!!", GUILayout.Width(26)); }
                        }
                    }

                    if (targetLocales.Count > 1)
                    {
                        using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColStringsW)))
                        {
                            // Strings subtables inside this box
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                foreach (var loc in targetLocales)
                                {
                                    bool has = HasString(quest, loc, key);
                                    using (new EditorGUI.DisabledScope(true))
                                    { GUILayout.Toggle(has, loc.Identifier.Code, GUILayout.Width(50)); }
                                }
                            }
                        }
                        using (new EditorGUILayout.VerticalScope("box"))
                        { DrawAllLocalesRow_AudioOnly(quest, key, targetLocales); }
                    }
                    else
                    {
                        using (new EditorGUILayout.VerticalScope("box"))
                        { DrawSingleLocaleRow(quest, key, targetLocales[0]); }
                    }
                }
            }
        }

        // Build ordered rows including both #line and #shadow occurrences; fallback to meta titles when parsing fails
        private List<RowItem> GetRowsInScriptOrder(QuestData quest, Antura.Discover.Audio.Editor.YarnLineMapBuilder.YarnLineMeta meta)
        {
            var rows = new List<RowItem>();
            try
            {
                string text = quest?.YarnScript != null ? quest.YarnScript.text : null;
                if (!string.IsNullOrEmpty(text))
                {
                    var rx = new Regex(@"#(line|shadow):([A-Za-z0-9_-]+)");
                    foreach (Match m in rx.Matches(text))
                    {
                        var kind = m.Groups[1].Value;
                        var id = m.Groups[2].Value;
                        if (string.IsNullOrEmpty(id))
                            continue;
                        bool isShadow = string.Equals(kind, "shadow", StringComparison.OrdinalIgnoreCase);
                        // Prefer listing real lines even if meta lacks; for shadows we list as-is
                        rows.Add(new RowItem { ShortId = id, IsShadow = isShadow });
                    }
                }
            }
            catch { }

            if (rows.Count == 0 && meta != null)
            {
                // Fallback: meta titles as normal lines
                foreach (var id in meta.Titles.Keys)
                {
                    rows.Add(new RowItem { ShortId = id, IsShadow = false });
                }
            }
            return rows;
        }

        // Audio-only part for multi-locale rows (keeps vertical box for the column)
        private void DrawAllLocalesRow_AudioOnly(QuestData quest, string lineKey, List<Locale> locales)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                foreach (var loc in locales)
                {
                    var info = GetAudioInfo(quest, loc, lineKey);
                    bool stale = IsLineStale(quest, loc, lineKey);
                    if (info.Clip != null && !info.IsZeroLength)
                    {
                        if (stale)
                        { GUILayout.Label("!!!", GUILayout.Width(26)); }
                        if (GUILayout.Button("▶", GUILayout.Width(24)))
                        { PlayClip(info.Clip); }
                        if (GUILayout.Button("R", GUILayout.Width(22)))
                        { RegenerateLineAudioViaVoManager(quest, loc, lineKey); }
                        if (GUILayout.Button("Ping", GUILayout.Width(40)))
                        { PingObject(info.Obj); }
                    }
                    else
                    {
                        if (GUILayout.Button("Generate", GUILayout.Width(80)))
                        { RegenerateLineAudioViaVoManager(quest, loc, lineKey); }
                    }
                }
            }
        }

        private sealed class CardOccurrence
        {
            public string NodeTitle;
            public string CardId;
        }

        private void DrawCardTable(QuestData quest)
        {
            var targetLocales = GetTargetLocales().ToList();
            var displayLocale = GetEnglishLocale() ?? targetLocales.FirstOrDefault();
            if (displayLocale == null)
            {
                EditorGUILayout.HelpBox("No locales available.", MessageType.Info);
                return;
            }

            var occurrences = GetCardOccurrencesInScript(quest);
            if (!string.IsNullOrWhiteSpace(_filter))
            {
                var f = _filter.Trim();
                occurrences = occurrences.Where(o =>
                    (!string.IsNullOrEmpty(o.NodeTitle) && o.NodeTitle.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (!string.IsNullOrEmpty(o.CardId) && o.CardId.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Node Title", EditorStyles.boldLabel, GUILayout.Width(300));
                GUILayout.Label("Card Title", EditorStyles.boldLabel, GUILayout.Width(300));
                GUILayout.Label("Card Audio", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Regenerate missing card title audio", GUILayout.Width(280)))
                {
                    RegenerateMissingCardTitlesForOccurrences(occurrences);
                }
            }

            foreach (var occ in occurrences)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label(occ.NodeTitle ?? string.Empty, EditorStyles.wordWrappedLabel, GUILayout.Width(300));
                    DrawCardInfoInline(occ.CardId, displayLocale);
                }
            }
        }

        private List<CardOccurrence> GetCardOccurrencesInScript(QuestData quest)
        {
            var list = new List<CardOccurrence>();
            try
            {
                string text = quest?.YarnScript != null ? quest.YarnScript.text : null;
                if (string.IsNullOrEmpty(text))
                    return list;
                string currentTitle = string.Empty;
                var rxTitle = new Regex(@"^\s*title:\s*(.+)$", RegexOptions.Multiline);
                var rxCard = new Regex(@"<<\s*card\s+([A-Za-z0-9_\-]+)\s*>>", RegexOptions.Multiline);

                // Build an index of line positions to titles
                var titleMatches = rxTitle.Matches(text).Cast<Match>().ToList();
                var cardMatches = rxCard.Matches(text).Cast<Match>().ToList();

                // For each card match, find the nearest preceding title
                foreach (var cm in cardMatches)
                {
                    string nodeTitle = string.Empty;
                    int pos = cm.Index;
                    for (int i = titleMatches.Count - 1; i >= 0; i--)
                    {
                        if (titleMatches[i].Index <= pos)
                        {
                            nodeTitle = titleMatches[i].Groups[1].Value.Trim();
                            break;
                        }
                    }
                    list.Add(new CardOccurrence { NodeTitle = nodeTitle, CardId = cm.Groups[1].Value });
                }
            }
            catch { }
            return list;
        }

        private void RegenerateMissingCardTitlesForOccurrences(List<CardOccurrence> occurrences)
        {
            if (occurrences == null || occurrences.Count == 0)
            { EditorUtility.DisplayDialog("Cards Audio", "No card lines found in this script.", "OK"); return; }

            // Resolve distinct cards used in the script
            var ids = new HashSet<string>(occurrences.Where(o => !string.IsNullOrEmpty(o.CardId)).Select(o => o.CardId), StringComparer.Ordinal);
            var cards = ids.Select(id => _cardsById.TryGetValue(id, out var c) ? c : null).Where(c => c != null).Distinct().ToList();
            if (cards.Count == 0)
            { EditorUtility.DisplayDialog("Cards Audio", "No matching Card assets found.", "OK"); return; }

            var voWindow = Resources.FindObjectsOfTypeAll<Antura.Discover.Audio.Editor.VoiceoverManagerWindow>().FirstOrDefault()
                           ?? GetWindow<Antura.Discover.Audio.Editor.VoiceoverManagerWindow>();
            if (voWindow == null)
            { Debug.LogWarning("Voiceover Manager window not found."); return; }

            try
            {
                var t = voWindow.GetType();
                var onlyMissingField = t.GetField("_onlyGenerateMissing", BindingFlags.Instance | BindingFlags.NonPublic);
                var includeDescField = t.GetField("_cardsIncludeDescriptions", BindingFlags.Instance | BindingFlags.NonPublic);
                var createCapField = t.GetField("_createCapIndex", BindingFlags.Instance | BindingFlags.NonPublic);
                var localesField = t.GetField("_locales", BindingFlags.Instance | BindingFlags.NonPublic);
                var localeIdxField = t.GetField("_selectedLocaleIndex", BindingFlags.Instance | BindingFlags.NonPublic);
                var runMethod = t.GetMethod("RunCreateCardAudio", BindingFlags.Instance | BindingFlags.NonPublic);

                onlyMissingField?.SetValue(voWindow, true);
                includeDescField?.SetValue(voWindow, false);
                createCapField?.SetValue(voWindow, 2); // All

                // Set locale selection: if multi-selected, use All; if one, set that locale
                int selIndex = 0; // All
                var selectedLocales = GetTargetLocales().ToList();
                if (selectedLocales.Count == 1)
                {
                    var voLocales = localesField?.GetValue(voWindow) as System.Collections.IList;
                    if (voLocales != null)
                    {
                        for (int i = 0; i < voLocales.Count; i++)
                        {
                            var loc = voLocales[i] as Locale;
                            if (loc != null && loc.Identifier.Code == selectedLocales[0].Identifier.Code)
                            { selIndex = i + 1; break; } // +1 due to 0=All
                        }
                    }
                }
                localeIdxField?.SetValue(voWindow, selIndex);

                // Invoke generation with our filtered cards
                runMethod?.Invoke(voWindow, new object[] { cards });
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to call VO Manager (cards): {ex.Message}");
            }
        }

        private HashSet<string> GetShadowIdsInScript(QuestData quest)
        {
            var set = new HashSet<string>(StringComparer.Ordinal);
            try
            {
                string text = quest?.YarnScript != null ? quest.YarnScript.text : null;
                if (string.IsNullOrEmpty(text))
                    return set;
                var rx = new Regex(@"#shadow:([A-Za-z0-9_-]+)");
                foreach (Match m in rx.Matches(text))
                {
                    var id = m.Groups[1].Value;
                    if (!string.IsNullOrEmpty(id))
                        set.Add(id);
                }
            }
            catch { }
            return set;
        }

        private Locale GetEnglishLocale()
        {
            var exact = _locales.FirstOrDefault(l => string.Equals(l.Identifier.Code, "en", StringComparison.OrdinalIgnoreCase));
            if (exact != null)
                return exact;
            return _locales.FirstOrDefault(l => l.Identifier.Code.StartsWith("en", StringComparison.OrdinalIgnoreCase));
        }

        private static string GetLocaleDisplayName(Locale loc)
        {
            if (loc == null)
                return "-";
            try
            {
                var ci = loc.Identifier.CultureInfo;
                if (ci != null)
                {
                    var name = ci.EnglishName;
                    if (!string.IsNullOrEmpty(name))
                        return name;
                }
            }
            catch { }
            // Fallback to asset name if it's meaningful, else code
            if (!string.IsNullOrEmpty(loc.name) && !string.Equals(loc.name, loc.Identifier.Code, StringComparison.OrdinalIgnoreCase))
                return loc.name;
            return loc.Identifier.Code;
        }

        private bool HasCardCommand(string entryText, out string cardId)
        {
            cardId = null;
            if (string.IsNullOrEmpty(entryText))
                return false;
            var rxCard = new Regex(@"<<\s*card\s+([A-Za-z0-9_\-]+)\s*>>");
            var m = rxCard.Match(entryText);
            if (!m.Success)
                return false;
            cardId = m.Groups[1].Value;
            return !string.IsNullOrEmpty(cardId);
        }

        private void DrawCardInfoInline(string cardId, Locale localeForTitle)
        {
            if (string.IsNullOrEmpty(cardId) || !_cardsById.TryGetValue(cardId, out var card) || card == null)
            {
                GUILayout.Label("Card: -", GUILayout.Width(300));
                GUILayout.Label("-", GUILayout.Width(120));
                return;
            }
            // Localized card title
            string cardTitle = GetLocalizedString(card.Title.TableReference, card.Title.TableEntryReference, localeForTitle) ?? card.Title.GetLocalizedString();
            GUILayout.Label($"Card: {cardId} — {cardTitle}", EditorStyles.wordWrappedLabel, GUILayout.Width(300));
            if (GUILayout.Button("Ping Card", GUILayout.Width(90)))
            {
                PingObject(card);
            }

            // Card title audio via Cards audio table (use same locale)
            var at = LocalizationSettings.AssetDatabase.GetTable("Cards audio", localeForTitle);
            AudioClip clip = null;
            UnityEngine.Object obj = null;
            if (at != null)
            {
                var e = at.GetEntry(card.Id);
                if (e != null && !string.IsNullOrEmpty(e.Guid))
                {
                    var path = AssetDatabase.GUIDToAssetPath(e.Guid);
                    obj = string.IsNullOrEmpty(path) ? null : AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                    clip = obj as AudioClip;
                }
            }
            if (clip != null)
            {
                if (GUILayout.Button("Play", GUILayout.Width(44)))
                    PlayClip(clip);
                if (GUILayout.Button("Ping", GUILayout.Width(44)))
                    PingObject(obj);
            }
            else
            {
                GUILayout.Label("card audio missing");
            }
        }

        private void DrawAllLocalesRow(QuestData quest, string lineKey, List<Locale> locales)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                // Strings presence as toggles
                using (new EditorGUILayout.HorizontalScope(GUILayout.Width(180)))
                {
                    foreach (var loc in locales)
                    {
                        bool has = HasString(quest, loc, lineKey);
                        using (new EditorGUI.DisabledScope(true))
                        {
                            GUILayout.Toggle(has, loc.Identifier.Code, GUILayout.Width(50));
                        }
                    }
                }

                // Audio: small play buttons if assigned, else "-"; prepend !!! when stale
                using (new EditorGUILayout.HorizontalScope())
                {
                    foreach (var loc in locales)
                    {
                        var info = GetAudioInfo(quest, loc, lineKey);
                        bool stale = IsLineStale(quest, loc, lineKey);
                        if (info.Clip != null && !info.IsZeroLength)
                        {
                            if (stale)
                            {
                                GUILayout.Label("!!!", GUILayout.Width(26));
                            }
                            if (GUILayout.Button("▶", GUILayout.Width(24)))
                            {
                                PlayClip(info.Clip);
                            }
                            // Always show a small Regenerate button next to Play
                            if (GUILayout.Button("R", GUILayout.Width(22)))
                            {
                                RegenerateLineAudioViaVoManager(quest, loc, lineKey);
                            }
                            if (GUILayout.Button("Ping", GUILayout.Width(40)))
                            {
                                PingObject(info.Obj);
                            }
                        }
                        else
                        {
                            // Offer a Generate action when audio is missing
                            if (GUILayout.Button("Generate", GUILayout.Width(80)))
                            {
                                RegenerateLineAudioViaVoManager(quest, loc, lineKey);
                            }
                        }
                    }
                }
            }
        }

        private void DrawSingleLocaleRow(QuestData quest, string lineKey, Locale locale)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var info = GetAudioInfo(quest, locale, lineKey);
                bool stale = IsLineStale(quest, locale, lineKey);
                if (info.Clip != null && !info.IsZeroLength)
                {
                    if (stale)
                    {
                        GUILayout.Label("!!!", GUILayout.Width(26));
                    }
                    if (GUILayout.Button("Play", GUILayout.Width(44)))
                        PlayClip(info.Clip);
                    // Small inline regenerate next to Play
                    if (GUILayout.Button("R", GUILayout.Width(24)))
                        RegenerateLineAudioViaVoManager(quest, locale, lineKey);
                    if (GUILayout.Button("Ping", GUILayout.Width(44)))
                        PingObject(info.Obj);
                    if (GUILayout.Button("Delete", GUILayout.Width(58)))
                        ClearAudioAssignment(quest, locale, lineKey);
                }
                else
                {
                    if (info.MissingOnDisk)
                    {
                        GUILayout.Label("missing on disk", GUILayout.Width(100));
                        using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(info.Path)))
                        {
                            if (!string.IsNullOrEmpty(info.Path) && GUILayout.Button("Reimport", GUILayout.Width(70)))
                            {
                                AssetDatabase.ImportAsset(info.Path, ImportAssetOptions.ForceUpdate);
                            }
                        }
                        if (GUILayout.Button("Clear", GUILayout.Width(50)))
                        {
                            ClearAudioAssignment(quest, locale, lineKey);
                        }
                        // Also allow generating a fresh clip if the file is gone
                        if (GUILayout.Button("Generate", GUILayout.Width(90)))
                            RegenerateLineAudioViaVoManager(quest, locale, lineKey);
                    }
                    else
                    {
                        // Offer Generate when no audio is assigned
                        if (GUILayout.Button("Generate", GUILayout.Width(90)))
                            RegenerateLineAudioViaVoManager(quest, locale, lineKey);
                    }
                }
            }

            // Card addon: if text contains <<card cardid>> show card title localisation and audio controls
            var addonText = GetStringValue(quest, locale, lineKey) ?? string.Empty;
            TryDrawCardAddon(locale, addonText);
        }

        private void TryDrawCardAddon(Locale locale, string entryText)
        {
            if (string.IsNullOrEmpty(entryText))
                return;
            var rxCard = new Regex(@"<<\s*card\s+([A-Za-z0-9_\-]+)\s*>>");
            var m = rxCard.Match(entryText);
            if (!m.Success)
                return;
            var cardId = m.Groups[1].Value;
            if (!_cardsById.TryGetValue(cardId, out var card) || card == null)
                return;

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(584); // align under first two columns (title/id 320 + text 260 + small padding)

                // Localized card title (from card.Title table)
                string cardTitle = GetLocalizedString(card.Title.TableReference, card.Title.TableEntryReference, locale) ?? card.Title.GetLocalizedString();
                GUILayout.Label($"Card: {cardId} — {cardTitle}", EditorStyles.wordWrappedLabel, GUILayout.Width(300));

                // Card title audio via Cards audio table
                var at = LocalizationSettings.AssetDatabase.GetTable("Cards audio", locale);
                AudioClip clip = null;
                UnityEngine.Object obj = null;
                if (at != null)
                {
                    var e = at.GetEntry(card.Id);
                    if (e != null && !string.IsNullOrEmpty(e.Guid))
                    {
                        var path = AssetDatabase.GUIDToAssetPath(e.Guid);
                        obj = string.IsNullOrEmpty(path) ? null : AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                        clip = obj as AudioClip;
                    }
                }
                if (clip != null)
                {
                    if (GUILayout.Button("Play", GUILayout.Width(44)))
                        PlayClip(clip);
                    if (GUILayout.Button("Ping", GUILayout.Width(44)))
                        PingObject(obj);
                }
                else
                {
                    GUILayout.Label("card audio missing");
                }
            }
        }

        private void DrawOrphansSection(QuestData quest)
        {
            GUILayout.Label("Orphan cleanup", EditorStyles.boldLabel);
            var meta = YarnLineMapBuilder.BuildMeta(quest);
            var validKeys = new HashSet<string>(meta.Titles.Keys.Select(k => "line:" + k), StringComparer.Ordinal);

            var locales = GetTargetLocales().ToList();
            if (locales.Count == 0)
                return;

            foreach (var loc in locales)
            {
                using (new EditorGUILayout.VerticalScope("box"))
                {
                    GUILayout.Label($"Locale: {GetLocaleDisplayName(loc)}", EditorStyles.miniBoldLabel);

                    // String table orphans
                    var st = LocalizationSettings.StringDatabase.GetTable(quest.QuestStringsTable.TableReference, loc);
                    var stringOrphans = new List<StringTableEntry>();
                    if (st != null)
                    {
                        foreach (var e in st.Values)
                        {
                            var key = e?.SharedEntry?.Key;
                            if (string.IsNullOrEmpty(key))
                                continue;
                            if (key.StartsWith("line:", StringComparison.Ordinal) && !validKeys.Contains(key))
                                stringOrphans.Add(e);
                        }
                    }

                    GUILayout.Label($"String orphans: {stringOrphans.Count}");
                    if (stringOrphans.Count > 0)
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            if (GUILayout.Button("Delete all string orphans", GUILayout.Width(220)))
                            {
                                if (EditorUtility.DisplayDialog("Delete all string orphans?", $"This will remove {stringOrphans.Count} string entries not present in the Yarn script.", "Delete", "Cancel"))
                                {
                                    foreach (var e in stringOrphans)
                                        st.RemoveEntry(e.SharedEntry.Id);
                                    MarkDirty(st);
                                }
                            }
                        }
                    }

                    // Asset table orphans
                    var at = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, loc);
                    var assetOrphans = new List<AssetTableEntry>();
                    if (at != null)
                    {
                        foreach (var e in at.Values)
                        {
                            var key = e?.SharedEntry?.Key;
                            if (string.IsNullOrEmpty(key))
                                continue;
                            if (key.StartsWith("line:", StringComparison.Ordinal) && !validKeys.Contains(key))
                                assetOrphans.Add(e);
                        }
                    }
                    GUILayout.Label($"Audio orphans: {assetOrphans.Count}");
                    if (assetOrphans.Count > 0)
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            if (GUILayout.Button("Delete all audio orphans", GUILayout.Width(200)))
                            {
                                if (EditorUtility.DisplayDialog("Delete all audio orphans?", $"This will remove {assetOrphans.Count} audio entries not present in the Yarn script.", "Delete", "Cancel"))
                                {
                                    foreach (var e in assetOrphans)
                                        at.RemoveEntry(e.SharedEntry.Id);
                                    MarkDirty(at);
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<string> GetLineIdsInScriptOrder(QuestData quest, Antura.Discover.Audio.Editor.YarnLineMapBuilder.YarnLineMeta meta)
        {
            var ordered = new List<string>();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            try
            {
                string text = quest?.YarnScript != null ? quest.YarnScript.text : null;
                if (!string.IsNullOrEmpty(text))
                {
                    var rxLine = new Regex(@"#line:([A-Za-z0-9_-]+)");
                    foreach (Match m in rxLine.Matches(text))
                    {
                        var id = m.Groups[1].Value;
                        if (!seen.Contains(id) && meta.Titles.ContainsKey(id))
                        {
                            seen.Add(id);
                            ordered.Add(id);
                        }
                    }
                }
            }
            catch { }
            // Fallback: meta keys (order unspecified) for any not found via parsing
            if (ordered.Count == 0)
            {
                ordered.AddRange(meta.Titles.Keys);
            }
            return ordered;
        }

        private void DrawSummaryAndBatchActions(QuestData quest)
        {
            var meta = YarnLineMapBuilder.BuildMeta(quest);
            var allIds = GetLineIdsInScriptOrder(quest, meta);
            if (!string.IsNullOrWhiteSpace(_filter))
            {
                var f = _filter.Trim();
                allIds = allIds.Where(id =>
                {
                    if (id.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                    if (meta.Titles.TryGetValue(id, out var title))
                        return title != null && title.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0;
                    return false;
                }).ToList();
            }
            var locales = GetTargetLocales().ToList();
            int totalLines = allIds.Count;
            int missingStrings = 0;
            int missingAudio = 0;
            int changedAudio = 0;
            foreach (var id in allIds)
            {
                var key = "line:" + id;
                foreach (var loc in locales)
                {
                    if (!HasNonEmptyString(quest, loc, key))
                        missingStrings++;
                    var ai = GetAudioInfo(quest, loc, key);
                    if (!(ai.Clip != null && !ai.IsZeroLength))
                        missingAudio++;
                    else if (IsLineStale(quest, loc, key))
                        changedAudio++;
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label($"Lines: {totalLines}", GUILayout.Width(100));
                GUILayout.Label($"Missing strings: {missingStrings}", GUILayout.Width(160));
                GUILayout.Label($"Missing audio: {missingAudio}", GUILayout.Width(150));
                GUILayout.Label($"Changed audio: {changedAudio}", GUILayout.Width(160));

                var selectedLocales = locales;
                using (new EditorGUI.DisabledScope(missingAudio == 0 || selectedLocales.Count == 0))
                {
                    if (GUILayout.Button("Regenerate all missing (selected)", GUILayout.Width(240)))
                    {
                        RegenerateAllMissingForSelectedLocales(quest, selectedLocales);
                    }
                }
                using (new EditorGUI.DisabledScope(changedAudio == 0 || selectedLocales.Count == 0))
                {
                    if (GUILayout.Button("Regenerate all changed (selected)", GUILayout.Width(250)))
                    {
                        RegenerateAllChangedForSelectedLocales(quest, selectedLocales, allIds);
                    }
                }
            }
        }

        private bool HasNonEmptyString(QuestData quest, Locale locale, string key)
        {
            try
            {
                var st = LocalizationSettings.StringDatabase.GetTable(quest.QuestStringsTable.TableReference, locale);
                var e = st != null ? st.GetEntry(key) : null;
                return e != null && !string.IsNullOrWhiteSpace(e.Value);
            }
            catch { return false; }
        }

        private void RegenerateAllMissingForSelectedLocales(QuestData quest, List<Locale> locales)
        {
            foreach (var locale in locales)
            {
                RegenerateAllMissingForLocale(quest, locale);
            }
        }

        private void RegenerateAllMissingForLocale(QuestData quest, Locale locale)
        {
            if (locale == null)
                return;

            // Open or get existing VO Manager window
            var voWindow = Resources.FindObjectsOfTypeAll<Antura.Discover.Audio.Editor.VoiceoverManagerWindow>().FirstOrDefault()
                           ?? GetWindow<Antura.Discover.Audio.Editor.VoiceoverManagerWindow>();
            if (voWindow == null)
            {
                Debug.LogWarning("Voiceover Manager window not found.");
                return;
            }
            try
            {
                var t = voWindow.GetType();
                var questsField = t.GetField("_quests", BindingFlags.Instance | BindingFlags.NonPublic);
                var localesField = t.GetField("_locales", BindingFlags.Instance | BindingFlags.NonPublic);
                var questIdxField = t.GetField("_selectedQuestIndex", BindingFlags.Instance | BindingFlags.NonPublic);
                var localeIdxField = t.GetField("_selectedLocaleIndex", BindingFlags.Instance | BindingFlags.NonPublic);
                var onlyMissingField = t.GetField("_onlyGenerateMissing", BindingFlags.Instance | BindingFlags.NonPublic);
                var capField = t.GetField("_createCapIndex", BindingFlags.Instance | BindingFlags.NonPublic);
                var runMethod = t.GetMethod("RunCreateAudioFiles", BindingFlags.Instance | BindingFlags.NonPublic);

                var voQuests = questsField?.GetValue(voWindow) as System.Collections.IList;
                var voLocales = localesField?.GetValue(voWindow) as System.Collections.IList;

                int qIndex = 0;
                if (voQuests != null)
                {
                    for (int i = 0; i < voQuests.Count; i++)
                    {
                        if (ReferenceEquals(voQuests[i], quest))
                        { qIndex = i; break; }
                    }
                }
                questIdxField?.SetValue(voWindow, qIndex);

                int lIndex = 0; // default All in VO Manager
                if (voLocales != null)
                {
                    for (int i = 0; i < voLocales.Count; i++)
                    {
                        var loc = voLocales[i] as Locale;
                        if (loc != null && loc.Identifier.Code == locale.Identifier.Code)
                        { lIndex = i + 1; break; }
                    }
                }
                localeIdxField?.SetValue(voWindow, lIndex);

                onlyMissingField?.SetValue(voWindow, true);
                capField?.SetValue(voWindow, 2); // All

                runMethod?.Invoke(voWindow, null);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to call VO Manager batch: {ex.Message}");
            }
        }

        private void RegenerateAllChangedForSelectedLocales(QuestData quest, List<Locale> locales, List<string> allIds)
        {
            try
            {
                int total = allIds.Count * locales.Count;
                int done = 0;
                foreach (var id in allIds)
                {
                    var key = "line:" + id;
                    foreach (var loc in locales)
                    {
                        if (IsLineStale(quest, loc, key))
                        {
                            RegenerateLineAudioViaVoManager(quest, loc, key);
                        }
                        done++;
                        if (done % 10 == 0)
                        {
                            EditorUtility.DisplayProgressBar("Regenerate changed", $"{done}/{total}…", (float)done / Math.Max(1, total));
                        }
                    }
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static bool HasString(QuestData quest, Locale locale, string key)
        {
            try
            {
                var st = LocalizationSettings.StringDatabase.GetTable(quest.QuestStringsTable.TableReference, locale);
                return st != null && st.GetEntry(key) != null;
            }
            catch { return false; }
        }

        private static string GetStringValue(QuestData quest, Locale locale, string key)
        {
            try
            {
                var st = LocalizationSettings.StringDatabase.GetTable(quest.QuestStringsTable.TableReference, locale);
                var e = st != null ? st.GetEntry(key) : null;
                return e != null ? e.Value : null;
            }
            catch { return null; }
        }

        private static string GetLocalizedString(TableReference table, TableEntryReference entryRef, Locale locale)
        {
            try
            {
                // Use StringDatabase helper to avoid ambiguous GetEntry overloads
                return LocalizationSettings.StringDatabase.GetLocalizedString(table, entryRef, locale, FallbackBehavior.UseFallback);
            }
            catch { return null; }
        }

        private AudioClip GetAudioClip(QuestData quest, Locale locale, string key, out UnityEngine.Object obj)
        {
            obj = null;
            try
            {
                var at = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, locale);
                if (at == null)
                    return null;
                var e = at.GetEntry(key);
                if (e == null || string.IsNullOrEmpty(e.Guid))
                    return null;
                var path = AssetDatabase.GUIDToAssetPath(e.Guid);
                obj = string.IsNullOrEmpty(path) ? null : AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                var clip = obj as AudioClip;
                if (clip == null)
                    return null;
                // Treat zero-length as missing
                if (clip.length <= 0.001f)
                    return null;
                return clip;
            }
            catch { return null; }
        }

        private static void ClearAudioAssignment(QuestData quest, Locale locale, string key)
        {
            try
            {
                var at = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, locale);
                if (at == null)
                    return;
                var e = at.GetEntry(key);
                if (e == null)
                    return;
                e.Guid = string.Empty;
                MarkDirty(at);
            }
            catch { }
        }

        private void PlayClip(AudioClip clip)
        {
            if (clip == null)
                return;
            EnsurePreviewSource();
            _previewSource.Stop();
            _previewSource.clip = clip;
            _previewSource.volume = _previewVolume;
            _previewSource.spatialBlend = 0f;
            _previewSource.Play();
        }

        private static void PingObject(UnityEngine.Object obj)
        {
            if (obj == null)
                return;
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }

        private void RegenerateLineAudioViaVoManager(QuestData quest, Locale locale, string lineKey)
        {
            // Open or get existing VO Manager window
            var voWindow = Resources.FindObjectsOfTypeAll<Antura.Discover.Audio.Editor.VoiceoverManagerWindow>().FirstOrDefault()
                           ?? GetWindow<Antura.Discover.Audio.Editor.VoiceoverManagerWindow>();
            if (voWindow == null)
            {
                Debug.LogWarning("Voiceover Manager window not found.");
                return;
            }

            // Reflection to set selected quest/locale in VO Manager
            try
            {
                var t = voWindow.GetType();
                var questsField = t.GetField("_quests", BindingFlags.Instance | BindingFlags.NonPublic);
                var localesField = t.GetField("_locales", BindingFlags.Instance | BindingFlags.NonPublic);
                var questIdxField = t.GetField("_selectedQuestIndex", BindingFlags.Instance | BindingFlags.NonPublic);
                var localeIdxField = t.GetField("_selectedLocaleIndex", BindingFlags.Instance | BindingFlags.NonPublic);
                var runMethod = t.GetMethod("RunCreateAudioForSelectedLine", BindingFlags.Instance | BindingFlags.NonPublic);

                var voQuests = questsField?.GetValue(voWindow) as System.Collections.IList;
                var voLocales = localesField?.GetValue(voWindow) as System.Collections.IList;

                int qIndex = 0;
                if (voQuests != null)
                {
                    for (int i = 0; i < voQuests.Count; i++)
                    {
                        if (ReferenceEquals(voQuests[i], quest))
                        { qIndex = i; break; }
                    }
                }
                questIdxField?.SetValue(voWindow, qIndex);

                int lIndex = 0; // default All
                if (voLocales != null)
                {
                    for (int i = 0; i < voLocales.Count; i++)
                    {
                        var loc = voLocales[i] as Locale;
                        if (loc != null && loc.Identifier.Code == locale.Identifier.Code)
                        { lIndex = i + 1; break; } // +1 as VO uses 0=All
                    }
                }
                localeIdxField?.SetValue(voWindow, lIndex);

                runMethod?.Invoke(voWindow, new object[] { lineKey });
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to call VO Manager: {ex.Message}");
            }
        }

        private static void MarkDirty(LocalizationTable table)
        {
            if (table == null)
                return;
            EditorUtility.SetDirty(table);
            if (table.SharedData != null)
                EditorUtility.SetDirty(table.SharedData);
        }

        // -------------- Manifest staleness detection --------------
        // Use the shared manifest entry DTO defined in VoiceoverManifestUtil
        private class VoiceoverManifestUtil_ManifestEntry
        {
            public string key;
            public string nodeTitle;
            public string sourceText;
            public string audioFile;
            public string voiceProfile;
            public string actorId;
            public int? durationMs;
            public string updatedAt;
            public string textHash;
        }

        private Dictionary<string, VoiceoverManifestUtil_ManifestEntry> GetManifestLines(QuestData quest, Locale locale)
        {
            try
            {
                if (quest == null || locale == null)
                    return null;
                var qid = !string.IsNullOrEmpty(quest.Id) ? quest.Id : quest.name;
                var cacheKey = qid + "|" + locale.Identifier.Code;
                if (_manifestCache.TryGetValue(cacheKey, out var cached))
                    return cached;
                var path = Antura.Discover.Audio.Editor.VoiceoverManifestUtil.GetManifestPath(qid, locale.Identifier.Code);
                if (!File.Exists(path))
                { _manifestCache[cacheKey] = null; return null; }
                var json = File.ReadAllText(path);
                // Parse minimally for the 'lines' array; we can't access internal types directly
                var root = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                Dictionary<string, VoiceoverManifestUtil_ManifestEntry> dict = null;
                if (root != null && root.TryGetValue("lines", out var linesObj) && linesObj != null)
                {
                    var j = linesObj.ToString();
                    var entries = JsonConvert.DeserializeObject<List<VoiceoverManifestUtil_ManifestEntry>>(j);
                    if (entries != null)
                        dict = entries.Where(l => l != null && !string.IsNullOrEmpty(l.key)).ToDictionary(l => l.key, l => l, StringComparer.Ordinal);
                }
                _manifestCache[cacheKey] = dict;
                return dict;
            }
            catch
            {
                return null;
            }
        }

        private bool IsLineStale(QuestData quest, Locale locale, string lineKey)
        {
            try
            {
                // First, if English source changed, mark stale for all locales
                var en = GetEnglishLocale();
                if (en != null)
                {
                    var enMap = GetManifestLines(quest, en);
                    if (enMap != null && enMap.TryGetValue(lineKey, out var enLine) && enLine != null)
                    {
                        var curEn = GetStringValue(quest, en, lineKey) ?? string.Empty;
                        var curEnNorm = Antura.Discover.Audio.Editor.VoiceoverManifestUtil.NormalizeText(curEn);
                        if (!string.IsNullOrEmpty(enLine.sourceText))
                        {
                            var srcEnNorm = Antura.Discover.Audio.Editor.VoiceoverManifestUtil.NormalizeText(enLine.sourceText);
                            if (!string.Equals(curEnNorm, srcEnNorm, StringComparison.Ordinal))
                                return true;
                        }
                        else
                        {
                            var enHash = Antura.Discover.Audio.Editor.VoiceoverManifestUtil.ComputeTextHash(curEnNorm, enLine.voiceProfile, enLine.actorId, en.Identifier.Code);
                            if (!string.Equals(enHash, enLine.textHash, StringComparison.Ordinal))
                                return true;
                        }
                    }
                }

                // Then, check staleness for the current locale
                var map = GetManifestLines(quest, locale);
                if (map == null || !map.TryGetValue(lineKey, out var line) || line == null)
                    return false; // no data to compare
                var currentText = GetStringValue(quest, locale, lineKey) ?? string.Empty;
                var curNorm = Antura.Discover.Audio.Editor.VoiceoverManifestUtil.NormalizeText(currentText);

                if (!string.IsNullOrEmpty(line.sourceText))
                {
                    var srcNorm = Antura.Discover.Audio.Editor.VoiceoverManifestUtil.NormalizeText(line.sourceText);
                    return !string.Equals(curNorm, srcNorm, StringComparison.Ordinal);
                }
                // Fallback: compare computed hash using stored voice/actor/lang
                var hash = Antura.Discover.Audio.Editor.VoiceoverManifestUtil.ComputeTextHash(curNorm, line.voiceProfile, line.actorId, locale.Identifier.Code);
                return !string.Equals(hash, line.textHash, StringComparison.Ordinal);
            }
            catch { return false; }
        }
    }
}
#endif
