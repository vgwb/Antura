#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEditor.IMGUI.Controls;

namespace Antura.Discover
{
    public class GameDataBrowserWindow : EditorWindow
    {
        private Vector2 _scroll;
        private string _search = string.Empty;
        private const string DiscoverPathPrefix = "Assets/_discover/";

        private struct TypeOption
        {
            public string Label;     // what to show in the popup
            public Type Type;        // concrete type or base type (for aggregates)
            public bool IsAggregate; // when true, filter includes subclasses of Type
            public bool IsSeparator; // when true, non-selectable visual separator
        }

        private readonly List<TypeOption> _typeOptions = new List<TypeOption>();
        private int _selectedTypeIndex = 0; // index into _typeOptions

        // Country filter (applied only when data type exposes a Countries Country member)
        private enum CountryFilter { All, Global, France, Italy, Poland, Spain, Germany, UnitedKingdom, Portugal, Greece }
        private CountryFilter _countryFilter = CountryFilter.All;

        // Cached data
        private readonly List<IdentifiedData> _allData = new List<IdentifiedData>();
        // World Prefabs cached data
        private struct WorldPrefabEntry { public WorldPrefabData Comp; public string Path; }
        private readonly List<WorldPrefabEntry> _worldPrefabs = new List<WorldPrefabEntry>();
        // Sorting
        private string _sortKey = null;
        private bool _sortAsc = true;
        // Search field with lens icon
        private SearchField _searchField;
        // CardData quest filter state
        private QuestData _cardQuestFilter = null;
        // WordData Active filter state
        private enum WordActiveFilter { All, ActiveOnly, InactiveOnly }
        private WordActiveFilter _wordActive = WordActiveFilter.All;
        // QuestData DevStatus filter
        private Status? _devStatusFilter = null; // null => All

        // Layout sizes
        private const float ColOpen = 60f;
        private const float ColId = 180f;
        private const float ColImage = 80f;
        private const float ColPath = 380f;
        private const float ColTag = 140f;
        private const float ColLicense = 110f;
        private const float ColCopyright = 300f;
        private const float RowHeight = ColImage + 12f;

        // Quest-specific narrower columns
        private const float ColQId = 160f;
        private const float ColQDevStatus = 110f;
        private const float ColQLocation = 150f;
        private const float ColQKnowledge = 100f;
        // Card-specific knowledge column
        private const float ColKnowledge = 110f;

        // WorldPrefabData specific
        private const float ColWPCategory = 100f;
        private const float ColWPTags = 100f;
        private const float ColWPKit = 100f;
        private const float ColWPReplace = 120f;
        private WorldPrefabCategory? _wpCategoryFilter = null; // null => All categories
        private WorldPrefabTag? _wpTagFilter = null; // null => (Any)
        private WorldPrefabKit? _wpKitFilter = null; // null => (Any)

        // CardData specific
        private const float ColTitle = 220f;
        private const float ColCollectible = 90f;
        private const float ColYear = 60f;
        private const float ColCategory = 140f;
        private const float ColTopics = 220f;
        private const float ColCardWords = 260f;
        private const float ColQuests = 300f;
        private readonly Dictionary<CardData, string> _cardTitleCache = new Dictionary<CardData, string>();

        // WordData specific
        private const float ColWActive = 60f;
        private const float ColWTitle = 220f;
        private const float ColWCategory = 140f;
        private const float ColWImage = 80f;
        private const float ColWUnicode = 220f;

        // QuestData specific
        private const float ColTopic = 160f;
        private const float ColCards = 320f;
        private const float ColWords = 320f;
        private const float ColIdDisplay = 180f;
        private const float ColLocation = 200f;
        private const float ColDifficulty = 100f;
        // Export column removed; exports are handled by QuestExporterWindow
        private const float Gap = 6f;
        private const float MarginLeft = 4f;
        private readonly Color _gridLineColor = new Color(0, 0, 0, 0.12f);

        private float GetTableWidth()
        {
            float width = MarginLeft + ColOpen + Gap;
            bool hasImage = SelectedType == typeof(QuestData) || SelectedType == typeof(CardData) || SelectedType == typeof(ItemData) || SelectedType == typeof(AssetData) || SelectedType == typeof(WorldPrefabData);
            if (hasImage)
            {
                width += ColImage + Gap;
            }

            if (SelectedType == typeof(QuestData))
            {
                // DevStatus, Id(+IdDisplay), Location, Knowledge, Difficulty, MainTopic, LinkedCards, WordsUsed
                width += ColQDevStatus + Gap + ColQId + Gap + ColQLocation + Gap + ColQKnowledge + Gap + ColDifficulty + Gap + ColTopic + Gap + ColCards + Gap + ColWords;
            }
            else if (SelectedType == typeof(WorldPrefabData))
            {
                // Preview, Id, Category, Tags, Path
                width += ColWPReplace + Gap + ColId + Gap + ColWPCategory + Gap + ColWPTags + Gap + ColWPKit + Gap + ColPath;
            }
            else if (SelectedType == typeof(WordData))
            {
                // Id, Active, Title (TextEn), Category, Drawing (preview), Drawing Unicode
                width += ColId + Gap + ColWActive + Gap + ColWTitle + Gap + ColWCategory + Gap + ColWImage + Gap + ColWUnicode;
            }
            else if (SelectedType == typeof(CardData))
            {
                // Id, Collectible(+Icon), Year, Knowledge, Category, Topics, Words, Linked Quests, Path
                width += ColId + Gap + ColCollectible + Gap + ColYear + Gap + ColKnowledge + Gap + ColCategory + Gap + ColTopics + Gap + ColCardWords + Gap + ColQuests + Gap + ColPath;
            }
            else if (SelectedType == typeof(ItemData))
            {
                width += ColId + Gap + ColPath + Gap + ColTag;
            }
            else if (SelectedType == typeof(AssetData))
            {
                width += ColId + Gap + ColPath + Gap + ColLicense + Gap + ColCopyright;
            }
            return width;
        }

        // Draw a clickable, sortable header label that toggles sort order on repeat clicks
        private void HeaderLabel(string text, float width, string sortKey)
        {
            var rect = GUILayoutUtility.GetRect(width, EditorGUIUtility.singleLineHeight, GUILayout.Width(width));
            bool isCurrent = string.Equals(_sortKey, sortKey, StringComparison.Ordinal);
            string suffix = isCurrent ? (_sortAsc ? " ▲" : " ▼") : string.Empty;
            GUI.Label(rect, new GUIContent(text + suffix), EditorStyles.boldLabel);
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                if (isCurrent)
                    _sortAsc = !_sortAsc;
                else
                { _sortKey = sortKey; _sortAsc = true; }
                Repaint();
                Event.current.Use();
            }
        }

        // Rect-based version for explicit header layout
        private void HeaderLabelRect(string text, Rect rect, string sortKey)
        {
            bool isCurrent = string.Equals(_sortKey, sortKey, StringComparison.Ordinal);
            string suffix = isCurrent ? (_sortAsc ? " ▲" : " ▼") : string.Empty;
            GUI.Label(rect, new GUIContent(text + suffix), EditorStyles.boldLabel);
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                if (isCurrent)
                    _sortAsc = !_sortAsc;
                else
                { _sortKey = sortKey; _sortAsc = true; }
                Repaint();
                Event.current.Use();
            }
        }

        [MenuItem("Antura/Game Data Browser", priority = 10)]
        public static void ShowWindow()
        {
            var wnd = GetWindow<GameDataBrowserWindow>(false, "Game Data Browser", true);
            wnd.minSize = new Vector2(800, 400);
            wnd.RefreshTypes();
            wnd.RefreshList();
            wnd.Show();
        }

        private void OnEnable()
        {
            RefreshTypes();
            RefreshList();
            if (_searchField == null)
                _searchField = new SearchField();
        }

        private void RefreshTypes()
        {
            // Remember current selection to preserve it after refresh
            Type prevType = null;
            bool prevIsAggregate = false;
            string prevLabel = null;
            if (_typeOptions.Count > 0 && _selectedTypeIndex >= 0 && _selectedTypeIndex < _typeOptions.Count)
            {
                prevType = _typeOptions[_selectedTypeIndex].Type;
                prevIsAggregate = _typeOptions[_selectedTypeIndex].IsAggregate;
                prevLabel = _typeOptions[_selectedTypeIndex].Label;
            }

            _typeOptions.Clear();
            try
            {
                var baseType = typeof(IdentifiedData);
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var concreteTypes = new List<Type>();
                foreach (var asm in assemblies)
                {
                    Type[] types;
                    try
                    { types = asm.GetTypes(); }
                    catch (ReflectionTypeLoadException ex) { types = ex.Types.Where(t => t != null).ToArray(); }
                    foreach (var t in types)
                    {
                        if (t == null || t.IsAbstract)
                            continue;
                        if (baseType.IsAssignableFrom(t))
                            concreteTypes.Add(t);
                    }
                }
                concreteTypes = concreteTypes.Distinct().ToList();
                concreteTypes.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));

                // Preferred order: QuestData, CardData, AssetData, WordData, TaskData
                var topTypes = new List<Type>();
                var questType = concreteTypes.FirstOrDefault(t => t.Name == nameof(QuestData));
                var cardType = concreteTypes.FirstOrDefault(t => t.Name == nameof(CardData));
                var assetType = concreteTypes.FirstOrDefault(t => t.Name == nameof(AssetData));
                var wordType = concreteTypes.FirstOrDefault(t => t.Name == nameof(WordData));
                var taskType = concreteTypes.FirstOrDefault(t => t.Name == nameof(TaskData));
                if (questType != null)
                    topTypes.Add(questType);
                if (cardType != null)
                    topTypes.Add(cardType);
                if (assetType != null)
                    topTypes.Add(assetType);
                if (wordType != null)
                    topTypes.Add(wordType);
                if (taskType != null)
                    topTypes.Add(taskType);

                foreach (var t in topTypes)
                    _typeOptions.Add(new TypeOption { Label = NicifyTypeLabel(t), Type = t, IsAggregate = false, IsSeparator = false });

                // Insert World Prefabs dataset right after TaskData as an important dataset
                var wpType = FindTypeByName("WorldPrefabData");
                if (wpType != null)
                {
                    _typeOptions.Add(new TypeOption { Label = "World Prefabs", Type = wpType, IsAggregate = false, IsSeparator = false });
                }

                // Separator
                _typeOptions.Add(new TypeOption { Label = "—", Type = null, IsAggregate = false, IsSeparator = true });

                // Add "All Data" after separator
                _typeOptions.Add(new TypeOption { Label = "All Data", Type = null, IsAggregate = false, IsSeparator = false });

                // Aggregate: Activities (all ActivitySettingsAbstract and subclasses)
                var activitiesBase = FindTypeByName("ActivitySettingsAbstract");
                if (activitiesBase != null)
                {
                    _typeOptions.Add(new TypeOption { Label = "Activities (All)", Type = activitiesBase, IsAggregate = true, IsSeparator = false });
                }

                // Others (alphabetical), excluding the top types
                foreach (var t in concreteTypes)
                {
                    if (topTypes.Contains(t))
                        continue;
                    if (t.Name == nameof(AchievementData))
                        continue;
                    _typeOptions.Add(new TypeOption { Label = NicifyTypeLabel(t), Type = t, IsAggregate = false, IsSeparator = false });
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[GameDataBrowser] Failed to gather IdentifiedData types: {ex.Message}");
            }

            // Try restore previous selection first
            int restore = -1;
            if (prevLabel != null)
            {
                restore = _typeOptions.FindIndex(o => o.Label == prevLabel && o.IsAggregate == prevIsAggregate && (o.Type == prevType || (o.Type != null && prevType != null && o.Type.Name == prevType.Name)));
            }
            if (restore < 0 && prevType != null)
            {
                restore = _typeOptions.FindIndex(o => o.Type == prevType && o.IsAggregate == prevIsAggregate);
                if (restore < 0)
                    restore = _typeOptions.FindIndex(o => o.Type != null && prevType != null && o.Type.Name == prevType.Name);
            }

            if (restore >= 0)
            {
                _selectedTypeIndex = restore;
            }
            else
            {
                if (_selectedTypeIndex < 0 || _selectedTypeIndex >= _typeOptions.Count)
                {
                    var idx = _typeOptions.FindIndex(o => o.Type != null && o.Type.Name == nameof(AssetData));
                    _selectedTypeIndex = idx >= 0 ? idx : 0;
                }
            }
        }

        private void RefreshList()
        {
            _allData.Clear();
            _worldPrefabs.Clear();
            try
            {
                // Load all assets derived from IdentifiedData in one search
                var guids = AssetDatabase.FindAssets("t:IdentifiedData");
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var obj = AssetDatabase.LoadAssetAtPath<IdentifiedData>(path);
                    if (obj != null)
                        _allData.Add(obj);
                }

                // Load prefabs containing WorldPrefabData component on the ROOT only (not children),
                // limited to the Discover Prefabs folder for speed.
                var wpGuids = AssetDatabase.FindAssets("t:Prefab", new[] { DiscoverPathPrefix + "Prefabs" });
                foreach (var guid in wpGuids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (go == null)
                        continue;
                    var comp = go.GetComponent<WorldPrefabData>();
                    if (comp != null)
                        _worldPrefabs.Add(new WorldPrefabEntry { Comp = comp, Path = path });
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[GameDataBrowser] Failed to load data: {ex.Message}");
            }
            Repaint();
        }

        private void OnGUI()
        {
            DrawToolbar();
            GUILayout.Space(6);
            DrawHeader();
            DrawRows();
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                // Left: Data selector (narrower width)
                // GUILayout.Label("Data:", GUILayout.Width(40));
                var labels = _typeOptions.Select(o => o.Label).ToArray();
                var selIndex = Mathf.Clamp(_selectedTypeIndex, 0, Mathf.Max(0, _typeOptions.Count - 1));
                var newIndex = EditorGUILayout.Popup(selIndex, labels, EditorStyles.toolbarPopup, GUILayout.Width(120));
                if (newIndex != _selectedTypeIndex)
                {
                    // Prevent selecting separator
                    if (newIndex >= 0 && newIndex < _typeOptions.Count && _typeOptions[newIndex].IsSeparator)
                    {
                        // no-op
                    }
                    else
                    {
                        _selectedTypeIndex = newIndex;
                    }
                    Repaint();
                }

                GUILayout.Space(8);
                // Country selector (applies to data types with a Countries member; also for WorldPrefabData)
                GUILayout.Label("Country:", GUILayout.Width(56));
                var cOpts = GetCountryOptions();
                var cLabels = cOpts.Select(o => o.Label).ToArray();
                int cIndex = Mathf.Max(0, cOpts.FindIndex(o => !o.IsSeparator && o.Value.Equals(_countryFilter)));
                int newCIndex = EditorGUILayout.Popup(cIndex, cLabels, EditorStyles.toolbarPopup, GUILayout.Width(100));
                if (newCIndex != cIndex)
                {
                    if (!cOpts[newCIndex].IsSeparator)
                    {
                        _countryFilter = cOpts[newCIndex].Value;
                        Repaint();
                    }
                }

                // DevStatus selector (only when browsing quests)
                if (SelectedType == typeof(QuestData))
                {
                    GUILayout.Space(8);
                    GUILayout.Label("Status:", GUILayout.Width(48));
                    var devValues = (Status[])Enum.GetValues(typeof(Status));
                    var devLabels = new List<string> { "All" };
                    devLabels.AddRange(devValues.Select(v => v.ToString()));
                    int current = _devStatusFilter.HasValue ? (Array.IndexOf(devValues, _devStatusFilter.Value) + 1) : 0;
                    int chosen = EditorGUILayout.Popup(current, devLabels.ToArray(), EditorStyles.toolbarPopup, GUILayout.Width(80));
                    if (chosen != current)
                    {
                        _devStatusFilter = chosen <= 0 ? default(Status?) : devValues[chosen - 1];
                        Repaint();
                    }
                }

                // WorldPrefabData Category + Tag + Kit filters
                if (SelectedType == typeof(WorldPrefabData))
                {
                    GUILayout.Label("Category:", GUILayout.Width(60));
                    var values = (WorldPrefabCategory[])Enum.GetValues(typeof(WorldPrefabCategory));
                    // Labels with All at the end
                    var catLabels = new List<string>(values.Select(v => v.ToString()));
                    catLabels.Add("All");
                    int curCat = _wpCategoryFilter.HasValue ? Array.IndexOf(values, _wpCategoryFilter.Value) : values.Length;
                    if (curCat < 0)
                        curCat = 0;
                    int pickCat = EditorGUILayout.Popup(curCat, catLabels.ToArray(), EditorStyles.toolbarPopup, GUILayout.Width(80));
                    if (pickCat != curCat && values.Length > 0)
                    {
                        _wpCategoryFilter = (pickCat >= values.Length) ? default(WorldPrefabCategory?) : values[pickCat];
                        _wpTagFilter = null; // reset tag when category changes
                        _wpKitFilter = null; // keep Kit independent but reset to be safe when scope changes
                        Repaint();
                    }

                    GUILayout.Label("Tag:", GUILayout.Width(30));
                    // Build tags actually present among prefabs in the selected category (or all when category is All)
                    var presentTags = _worldPrefabs
                        .Where(e => !_wpCategoryFilter.HasValue || GetWorldPrefabCategory(e.Comp) == _wpCategoryFilter.Value)
            .Where(e => CountryMatches(e.Comp, _countryFilter))
            .Where(e => !_wpKitFilter.HasValue || GetWorldPrefabKit(e.Comp) == _wpKitFilter.Value)
                        .SelectMany(e => GetWorldPrefabTags(e.Comp) ?? new List<WorldPrefabTag>())
                        .Distinct()
                        .OrderBy(t => (int)t)
                        .ToList();
                    if (presentTags.Count == 0)
                    {
                        using (new EditorGUI.DisabledScope(true))
                        {
                            EditorGUILayout.Popup(0, new[] { "(None)" }, EditorStyles.toolbarPopup, GUILayout.Width(80));
                        }
                        _wpTagFilter = null;
                    }
                    else
                    {
                        if (_wpTagFilter.HasValue && !presentTags.Contains(_wpTagFilter.Value))
                            _wpTagFilter = null;
                        var tagLabels = new List<string> { "(Any)" };
                        tagLabels.AddRange(presentTags.Select(v => v.ToString()));
                        int curTag = _wpTagFilter.HasValue ? (presentTags.IndexOf(_wpTagFilter.Value) + 1) : 0;
                        if (curTag < 0)
                            curTag = 0;
                        int pickTag = EditorGUILayout.Popup(curTag, tagLabels.ToArray(), EditorStyles.toolbarPopup, GUILayout.Width(80));
                        if (pickTag != curTag)
                        {
                            _wpTagFilter = pickTag <= 0 ? default(WorldPrefabTag?) : presentTags[pickTag - 1];
                            Repaint();
                        }
                    }

                    GUILayout.Label("Kit:", GUILayout.Width(24));
                    // Build kits actually present in the current scope
                    var presentKits = _worldPrefabs
                        .Where(e => !_wpCategoryFilter.HasValue || GetWorldPrefabCategory(e.Comp) == _wpCategoryFilter.Value)
                        .Where(e => CountryMatches(e.Comp, _countryFilter))
                        .Select(e => GetWorldPrefabKit(e.Comp))
                        .Distinct()
                        .OrderBy(k => (int)k)
                        .ToList();
                    if (presentKits.Count == 0)
                    {
                        using (new EditorGUI.DisabledScope(true))
                        {
                            EditorGUILayout.Popup(0, new[] { "(None)" }, EditorStyles.toolbarPopup, GUILayout.Width(100));
                        }
                        _wpKitFilter = null;
                    }
                    else
                    {
                        if (_wpKitFilter.HasValue && !presentKits.Contains(_wpKitFilter.Value))
                            _wpKitFilter = null;
                        var kitLabels = new List<string> { "(Any)" };
                        kitLabels.AddRange(presentKits.Select(v => v.ToString()));
                        int curKit = _wpKitFilter.HasValue ? (presentKits.IndexOf(_wpKitFilter.Value) + 1) : 0;
                        if (curKit < 0)
                            curKit = 0;
                        int pickKit = EditorGUILayout.Popup(curKit, kitLabels.ToArray(), EditorStyles.toolbarPopup, GUILayout.Width(100));
                        if (pickKit != curKit)
                        {
                            _wpKitFilter = pickKit <= 0 ? default(WorldPrefabKit?) : presentKits[pickKit - 1];
                            Repaint();
                        }
                    }
                }

                // When browsing CardData, show a Quest filter popup
                if (SelectedType == typeof(CardData))
                {
                    GUILayout.Space(8);
                    GUILayout.Label("Quest:", GUILayout.Width(44));
                    // Build quest list (IdDisplay fallback to Id/name)
                    var allQuests = _allData.OfType<QuestData>().OrderBy(q => q.Id ?? q.name).ToList();
                    var questLabels = new List<string> { "(All)" };
                    foreach (var q in allQuests)
                        questLabels.Add(string.IsNullOrEmpty(q.Id) ? q.name : q.Id);
                    int currentIndex = 0;
                    if (_cardQuestFilter != null)
                    {
                        var idx = allQuests.IndexOf(_cardQuestFilter);
                        if (idx >= 0)
                            currentIndex = idx + 1;
                    }
                    int newQ = EditorGUILayout.Popup(currentIndex, questLabels.ToArray(), EditorStyles.toolbarPopup, GUILayout.Width(110));
                    if (newQ != currentIndex)
                    {
                        _cardQuestFilter = newQ <= 0 ? null : allQuests[newQ - 1];
                        Repaint();
                    }
                }

                // When browsing WordData, show an Active filter toggle
                if (SelectedType == typeof(WordData))
                {
                    GUILayout.Space(8);
                    GUILayout.Label("Active:", GUILayout.Width(48));
                    int actIndex = (int)_wordActive;
                    string[] actLabels = new[] { "All", "Active", "Inactive" };
                    int newAct = EditorGUILayout.Popup(actIndex, actLabels, EditorStyles.toolbarPopup, GUILayout.Width(100));
                    if (newAct != actIndex)
                    { _wordActive = (WordActiveFilter)newAct; Repaint(); }
                }

                //GUILayout.Space(12);
                if (_searchField == null)
                    _searchField = new SearchField();
                var newSearch = _searchField.OnToolbarGUI(_search, GUILayout.MinWidth(100));
                if (!string.Equals(newSearch, _search, StringComparison.Ordinal))
                { _search = newSearch; Repaint(); }

                GUILayout.FlexibleSpace();
                // Show count of visible items
                try
                {
                    int count = (SelectedType == typeof(WorldPrefabData)) ? FilteredWorldPrefabs().Count() : Filtered().Count();
                    GUILayout.Label($"{count} items", EditorStyles.miniLabel);
                }
                catch { }

                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
                {
                    RefreshTypes();
                    RefreshList();
                }
                //                GUILayout.Space(8);

                if (GUILayout.Button("Export CSV", EditorStyles.toolbarButton))
                {
                    ExportCsv();
                }
                // Export is now handled in QuestExporterWindow
                // Refresh button already placed at the start
            }
        }


        private void DrawHeader()
        {
            float headerHeight = EditorGUIUtility.singleLineHeight + 6f;
            float tableWidth = GetTableWidth();
            var headerRect = GUILayoutUtility.GetRect(position.width, headerHeight, GUILayout.ExpandWidth(true));
            var viewRect = new Rect(0, 0, tableWidth, headerHeight);
            var headerScroll = new Vector2(_scroll.x, 0f);
            var newHeaderScroll = GUI.BeginScrollView(headerRect, headerScroll, viewRect, false, false, GUIStyle.none, GUIStyle.none);
            // Optional subtle background for header
            EditorGUI.DrawRect(new Rect(0, 0, tableWidth, headerHeight), new Color(0, 0, 0, 0.03f));

            float x = MarginLeft;
            float lineH = EditorGUIUtility.singleLineHeight;
            float y = (headerHeight - lineH) * 0.5f;
            // Open (not sortable)
            GUI.Label(new Rect(x, y, ColOpen, lineH), "Open", EditorStyles.boldLabel);
            x += ColOpen + Gap;

            if (SelectedType == typeof(QuestData))
            {
                GUI.Label(new Rect(x, y, ColImage, lineH), "Thumbnail", EditorStyles.boldLabel);
                x += ColImage + Gap;
                HeaderLabelRect("DevStatus", new Rect(x, y, ColQDevStatus, lineH), "DevStatus");
                x += ColQDevStatus + Gap;
                HeaderLabelRect("Id — IdDisplay", new Rect(x, y, ColQId, lineH), "IdConcat");
                x += ColQId + Gap;
                HeaderLabelRect("Location.Id", new Rect(x, y, ColQLocation, lineH), "Location.Id");
                x += ColQLocation + Gap;
                HeaderLabelRect("Knowledge", new Rect(x, y, ColQKnowledge, lineH), "KnowledgeValue");
                x += ColQKnowledge + Gap;
                HeaderLabelRect("Difficulty", new Rect(x, y, ColDifficulty, lineH), "Difficulty");
                x += ColDifficulty + Gap;
                HeaderLabelRect("Category / Topics", new Rect(x, y, ColTopic, lineH), "MainTopic");
                x += ColTopic + Gap;
                HeaderLabelRect("Linked Cards", new Rect(x, y, ColCards, lineH), "LinkedCards");
                x += ColCards + Gap;
                HeaderLabelRect("Words Used", new Rect(x, y, ColWords, lineH), "WordsUsed");
                x += ColWords + Gap;
            }
            else if (SelectedType == typeof(WorldPrefabData))
            {
                GUI.Label(new Rect(x, y, ColImage, lineH), "Preview", EditorStyles.boldLabel);
                x += ColImage + Gap;
                HeaderLabelRect("Action", new Rect(x, y, ColWPReplace, lineH), "WPReplace");
                x += ColWPReplace + Gap;
                HeaderLabelRect("Id", new Rect(x, y, ColId, lineH), nameof(IdentifiedData.Id));
                x += ColId + Gap;
                HeaderLabelRect("Category", new Rect(x, y, ColWPCategory, lineH), "WPCategory");
                x += ColWPCategory + Gap;
                HeaderLabelRect("Tags", new Rect(x, y, ColWPTags, lineH), "WPTags");
                x += ColWPTags + Gap;
                HeaderLabelRect("Kit", new Rect(x, y, ColWPKit, lineH), "WPKit");
                x += ColWPKit + Gap;
                HeaderLabelRect("Path", new Rect(x, y, ColPath, lineH), "Path");
                x += ColPath + Gap;
            }
            else if (SelectedType == typeof(WordData))
            {
                HeaderLabelRect("Id", new Rect(x, y, ColId, lineH), nameof(IdentifiedData.Id));
                x += ColId + Gap;
                HeaderLabelRect("Active", new Rect(x, y, ColWActive, lineH), "Active");
                x += ColWActive + Gap;
                HeaderLabelRect("Title", new Rect(x, y, ColWTitle, lineH), "TitleEn");
                x += ColWTitle + Gap;
                HeaderLabelRect("Category", new Rect(x, y, ColWCategory, lineH), "Category");
                x += ColWCategory + Gap;
                GUI.Label(new Rect(x, y, ColWImage, lineH), "Drawing", EditorStyles.boldLabel);
                x += ColWImage + Gap;
                HeaderLabelRect("Drawing Unicode", new Rect(x, y, ColWUnicode, lineH), "DrawingUnicode");
                x += ColWUnicode + Gap;
            }
            else if (SelectedType == typeof(CardData))
            {
                GUI.Label(new Rect(x, y, ColImage, lineH), "Image", EditorStyles.boldLabel);
                x += ColImage + Gap;
                HeaderLabelRect("Id", new Rect(x, y, ColId, lineH), nameof(IdentifiedData.Id));
                x += ColId + Gap;
                HeaderLabelRect("Collectible", new Rect(x, y, ColCollectible, lineH), "Collectible");
                x += ColCollectible + Gap;
                HeaderLabelRect("Year", new Rect(x, y, ColYear, lineH), "Year");
                x += ColYear + Gap;
                HeaderLabelRect("Knowledge", new Rect(x, y, ColKnowledge, lineH), "KnowledgeValue");
                x += ColKnowledge + Gap;
                HeaderLabelRect("Category", new Rect(x, y, ColCategory, lineH), "Category");
                x += ColCategory + Gap;
                HeaderLabelRect("Topics", new Rect(x, y, ColTopics, lineH), "Topics");
                x += ColTopics + Gap;
                HeaderLabelRect("Words", new Rect(x, y, ColCardWords, lineH), "CardWords");
                x += ColCardWords + Gap;
                HeaderLabelRect("Linked Quests", new Rect(x, y, ColQuests, lineH), "LinkedQuests");
                x += ColQuests + Gap;
                HeaderLabelRect("Path", new Rect(x, y, ColPath, lineH), "Path");
                x += ColPath + Gap;
            }
            else if (SelectedType == typeof(ItemData))
            {
                GUI.Label(new Rect(x, y, ColImage, lineH), "Icon", EditorStyles.boldLabel);
                x += ColImage + Gap;
                HeaderLabelRect("Id", new Rect(x, y, ColId, lineH), nameof(IdentifiedData.Id));
                x += ColId + Gap;
                HeaderLabelRect("Path", new Rect(x, y, ColPath, lineH), "Path");
                x += ColPath + Gap;
                HeaderLabelRect("Tag", new Rect(x, y, ColTag, lineH), "Tag");
                x += ColTag + Gap;
            }
            else if (SelectedType == typeof(AssetData))
            {
                GUI.Label(new Rect(x, y, ColImage, lineH), "Image", EditorStyles.boldLabel);
                x += ColImage + Gap;
                HeaderLabelRect("Id", new Rect(x, y, ColId, lineH), nameof(IdentifiedData.Id));
                x += ColId + Gap;
                HeaderLabelRect("Path", new Rect(x, y, ColPath, lineH), "Path");
                x += ColPath + Gap;
                HeaderLabelRect("License", new Rect(x, y, ColLicense, lineH), "License");
                x += ColLicense + Gap;
                HeaderLabelRect("Copyright / Source", new Rect(x, y, ColCopyright, lineH), "Copyright");
                x += ColCopyright + Gap;
            }
            else
            {
                HeaderLabelRect("Id", new Rect(x, y, ColId, lineH), nameof(IdentifiedData.Id));
                x += ColId + Gap;
                HeaderLabelRect("Path", new Rect(x, y, ColPath, lineH), "Path");
                x += ColPath + Gap;
            }

            // Vertical column lines in header
            var colXs = GetColumnBoundariesX();
            foreach (var gx in colXs)
            {
                EditorGUI.DrawRect(new Rect(gx, 0, 1, headerHeight), _gridLineColor);
            }

            GUI.EndScrollView();

            // Bottom separator line
            var rect = GUILayoutUtility.GetRect(1, 2);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, position.width, 1), new Color(0, 0, 0, 0.2f));
            // Sync header horizontal with content
            if (!Mathf.Approximately(newHeaderScroll.x, _scroll.x))
            { _scroll.x = newHeaderScroll.x; Repaint(); }
        }

        private void DrawRows()
        {
            if (SelectedType == typeof(WorldPrefabData))
            {
                DrawWorldPrefabRows();
                return;
            }

            var items = Filtered().ToList();
            if (items.Count == 0)
            {
                GUILayout.Space(8);
                EditorGUILayout.HelpBox("No data found.", MessageType.Info);
                return;
            }

            float tableWidth = GetTableWidth();
            var outer = GUILayoutUtility.GetRect(position.width, position.height - 160, GUILayout.ExpandHeight(true));
            // Fixed-height rows for all types
            float totalHeight = RowHeight * items.Count;
            var inner = new Rect(0, 0, tableWidth, totalHeight);
            _scroll = GUI.BeginScrollView(outer, _scroll, inner, true, true);
            int index = 0;
            float yCursor = 0f;
            foreach (var obj in items)
            {
                float rowH = RowHeight;
                var rowRect = new Rect(0, yCursor, tableWidth, rowH);
                if ((index++ % 2) == 0)
                    EditorGUI.DrawRect(new Rect(0, rowRect.y, tableWidth, rowRect.height), new Color(0, 0, 0, 0.04f));

                if (SelectedType == typeof(AssetData) && obj is AssetData ad)
                {
                    DrawAssetDataRow(rowRect, ad);
                }
                else if (SelectedType == typeof(ItemData) && obj is ItemData it)
                {
                    DrawItemDataRow(rowRect, it);
                }
                else if (SelectedType == typeof(CardData) && obj is CardData cd)
                {
                    DrawCardDataRow(rowRect, cd);
                }
                else if (SelectedType == typeof(QuestData) && obj is QuestData qd)
                {
                    DrawQuestDataRow(rowRect, qd);
                }
                else if (SelectedType == typeof(WordData) && obj is WordData wd)
                {
                    DrawWordDataRow(rowRect, wd);
                }
                else
                {
                    DrawGenericRow(rowRect, obj);
                }

                // Vertical grid lines per row
                var colXs = GetColumnBoundariesX();
                foreach (var x in colXs)
                {
                    EditorGUI.DrawRect(new Rect(x, rowRect.y, 1, rowRect.height), _gridLineColor);
                }
                yCursor += rowH;
            }
            GUI.EndScrollView();
        }

        private void DrawWorldPrefabRows()
        {
            var items = FilteredWorldPrefabs().ToList();
            if (items.Count == 0)
            {
                GUILayout.Space(8);
                EditorGUILayout.HelpBox("No world prefabs found.", MessageType.Info);
                return;
            }

            float tableWidth = GetTableWidth();
            var outer = GUILayoutUtility.GetRect(position.width, position.height - 160, GUILayout.ExpandHeight(true));
            float totalHeight = RowHeight * items.Count;
            var inner = new Rect(0, 0, tableWidth, totalHeight);
            _scroll = GUI.BeginScrollView(outer, _scroll, inner, true, true);
            int index = 0;
            float yCursor = 0f;
            foreach (var entry in items)
            {
                float rowH = RowHeight;
                var rowRect = new Rect(0, yCursor, tableWidth, rowH);
                if ((index++ % 2) == 0)
                    EditorGUI.DrawRect(new Rect(0, rowRect.y, tableWidth, rowRect.height), new Color(0, 0, 0, 0.04f));

                float x = MarginLeft;
                float lineH = EditorGUIUtility.singleLineHeight;
                float y = rowRect.y + (rowRect.height - lineH) * 0.5f;
                // Open
                var rOpen = new Rect(x, rowRect.y + (rowRect.height - 20f) * 0.5f, ColOpen - 12f, 20f);
                if (GUI.Button(rOpen, "Open"))
                {
                    var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(entry.Path);
                    EditorGUIUtility.PingObject(obj);
                    Selection.activeObject = obj;
                }
                x += ColOpen + Gap;

                // Preview
                var rImg = new Rect(x, rowRect.y + 2f, ColImage, ColImage);
                var goForPreview = AssetDatabase.LoadAssetAtPath<GameObject>(entry.Path);
                if (goForPreview != null)
                {
                    var tex = AssetPreview.GetAssetPreview(goForPreview) ?? AssetPreview.GetMiniThumbnail(goForPreview);
                    if (tex != null)
                        GUI.DrawTexture(rImg, tex, ScaleMode.ScaleToFit);
                    else
                        Repaint();
                }
                x += ColImage + Gap;

                // Replace button
                var rRep = new Rect(x, rowRect.y + (rowRect.height - 20f) * 0.5f, ColWPReplace, 20f);
                if (GUI.Button(rRep, "Replace"))
                {
                    var sel = Selection.gameObjects;
                    if (sel == null || sel.Length == 0)
                    {
                        EditorUtility.DisplayDialog("Replace", "Select one or more scene objects in the Hierarchy to replace.", "OK");
                    }
                    else
                    {
                        ReplaceSelectedWithPrefab(entry.Path);
                    }
                }
                x += ColWPReplace + Gap;

                // Id
                var rId = new Rect(x, y, ColId, lineH);
                EditorGUI.LabelField(rId, GetWorldPrefabId(entry.Comp));
                x += ColId + Gap;

                // Category
                var rCat = new Rect(x, y, ColWPCategory, lineH);
                EditorGUI.LabelField(rCat, GetWorldPrefabCategory(entry.Comp).ToString());
                x += ColWPCategory + Gap;

                // Tags (one per line)
                var tags = GetWorldPrefabTags(entry.Comp);
                string tagText = tags != null ? string.Join("\n", tags.Select(t => t.ToString())) : string.Empty;
                float tagHeight = EditorStyles.label.CalcHeight(new GUIContent(tagText), ColWPTags);
                float maxTagHeight = Mathf.Min(tagHeight, rowRect.height - 4f);
                var rTags = new Rect(x, rowRect.y + (rowRect.height - maxTagHeight) * 0.5f, ColWPTags, maxTagHeight);
                GUI.Label(rTags, tagText, EditorStyles.label);
                x += ColWPTags + Gap;

                // Kit
                var rKit = new Rect(x, y, ColWPKit, lineH);
                EditorGUI.LabelField(rKit, GetWorldPrefabKit(entry.Comp).ToString());
                x += ColWPKit + Gap;

                // Path
                var rPath = new Rect(x, y, ColPath, lineH);
                EditorGUI.LabelField(rPath, FormatPath(entry.Path));

                // Vertical grid lines per row
                var colXs = GetColumnBoundariesX();
                foreach (var gx in colXs)
                {
                    EditorGUI.DrawRect(new Rect(gx, rowRect.y, 1, rowRect.height), _gridLineColor);
                }
                yCursor += rowH;
            }
            GUI.EndScrollView();
        }

        // Compute X coordinates (from 0) where vertical grid lines should be drawn for the current table
        private List<float> GetColumnBoundariesX()
        {
            var xs = new List<float>();
            float x = 0f;
            // Left margin
            x += MarginLeft;
            // Open column
            x += ColOpen + Gap;
            xs.Add(x);

            if (SelectedType == typeof(QuestData))
            {
                x += ColImage + Gap;
                xs.Add(x);
                x += ColQDevStatus + Gap;
                xs.Add(x);
                x += ColQId + Gap;
                xs.Add(x);
                x += ColQLocation + Gap;
                xs.Add(x);
                x += ColQKnowledge + Gap;
                xs.Add(x);
                x += ColDifficulty + Gap;
                xs.Add(x);
                x += ColTopic + Gap;
                xs.Add(x);
                x += ColCards + Gap;
                xs.Add(x);
                x += ColWords + Gap;
                xs.Add(x);
                return xs;
            }
            if (SelectedType == typeof(WorldPrefabData))
            {
                x += ColImage + Gap;
                xs.Add(x);
                x += ColWPReplace + Gap;
                xs.Add(x);
                x += ColId + Gap;
                xs.Add(x);
                x += ColWPCategory + Gap;
                xs.Add(x);
                x += ColWPTags + Gap;
                xs.Add(x);
                x += ColWPKit + Gap;
                xs.Add(x);
                x += ColPath + Gap;
                xs.Add(x);
                return xs;
            }
            if (SelectedType == typeof(WordData))
            {
                x += ColId + Gap;
                xs.Add(x);
                x += ColWActive + Gap;
                xs.Add(x);
                x += ColWTitle + Gap;
                xs.Add(x);
                x += ColWCategory + Gap;
                xs.Add(x);
                x += ColWImage + Gap;
                xs.Add(x);
                x += ColWUnicode + Gap;
                xs.Add(x);
                return xs;
            }
            if (SelectedType == typeof(CardData))
            {
                x += ColImage + Gap;
                xs.Add(x);
                x += ColId + Gap;
                xs.Add(x);
                x += ColCollectible + Gap;
                xs.Add(x);
                x += ColYear + Gap;
                xs.Add(x);
                x += ColKnowledge + Gap;
                xs.Add(x);
                x += ColCategory + Gap;
                xs.Add(x);
                x += ColTopics + Gap;
                xs.Add(x);
                x += ColCardWords + Gap;
                xs.Add(x);
                x += ColQuests + Gap;
                xs.Add(x);
                x += ColPath + Gap;
                xs.Add(x);
                return xs;
            }
            if (SelectedType == typeof(ItemData))
            {
                x += ColImage + Gap;
                xs.Add(x);
                x += ColId + Gap;
                xs.Add(x);
                x += ColPath + Gap;
                xs.Add(x);
                x += ColTag + Gap;
                xs.Add(x);
                return xs;
            }
            if (SelectedType == typeof(AssetData))
            {
                x += ColImage + Gap;
                xs.Add(x);
                x += ColId + Gap;
                xs.Add(x);
                x += ColPath + Gap;
                xs.Add(x);
                x += ColLicense + Gap;
                xs.Add(x);
                x += ColCopyright + Gap;
                xs.Add(x);
                return xs;
            }
            // Generic: Id, Path
            x += ColId + Gap;
            xs.Add(x);
            x += ColPath + Gap;
            xs.Add(x);
            return xs;
        }

        private void DrawAssetDataRow(Rect rowRect, AssetData a)
        {
            float x = MarginLeft;
            float lineH = EditorGUIUtility.singleLineHeight;
            float y = rowRect.y + (rowRect.height - lineH) * 0.5f;
            // Open
            var rOpen = new Rect(x, rowRect.y + (rowRect.height - 20f) * 0.5f, ColOpen - 12f, 20f);
            if (GUI.Button(rOpen, "Open"))
            { EditorGUIUtility.PingObject(a); Selection.activeObject = a; }
            x += ColOpen + Gap;
            // Image (second column)
            var rImg = new Rect(x, rowRect.y + 2f, ColImage, ColImage);
            if (a.Image != null)
            {
                var tex = AssetPreview.GetAssetPreview(a.Image) ?? AssetPreview.GetMiniThumbnail(a.Image) ?? a.Image.texture;
                if (tex != null)
                    GUI.DrawTexture(rImg, tex, ScaleMode.ScaleToFit);
                else
                    Repaint();
            }
            x += ColImage + Gap;
            // Id
            var rId = new Rect(x, y, ColId, lineH);
            EditorGUI.LabelField(rId, a.Id ?? string.Empty);
            x += ColId + Gap;
            // Path
            var rPath = new Rect(x, y, ColPath, lineH);
            EditorGUI.LabelField(rPath, FormatPath(AssetDatabase.GetAssetPath(a)));
            x += ColPath + Gap;
            // License
            var rLic = new Rect(x, y, ColLicense, lineH);
            EditorGUI.BeginChangeCheck();
            var newLic = (LicenseType)EditorGUI.EnumPopup(rLic, a.License);
            if (EditorGUI.EndChangeCheck())
            { Undo.RecordObject(a, "Edit License"); a.License = newLic; EditorUtility.SetDirty(a); }
            x += ColLicense + Gap;
            // Copyright
            var rCopy = new Rect(x, y, ColCopyright, lineH);
            EditorGUI.BeginChangeCheck();
            string newCopy = EditorGUI.TextField(rCopy, a.Copyright ?? string.Empty);
            if (EditorGUI.EndChangeCheck())
            { Undo.RecordObject(a, "Edit Copyright"); a.Copyright = newCopy; EditorUtility.SetDirty(a); }
            // Source under it
            var rSrc = new Rect(x, y + lineH + 4f, ColCopyright, lineH);
            EditorGUI.BeginChangeCheck();
            string newSrc = EditorGUI.TextField(rSrc, a.SourceUrl ?? string.Empty);
            if (EditorGUI.EndChangeCheck())
            { Undo.RecordObject(a, "Edit SourceUrl"); a.SourceUrl = newSrc; EditorUtility.SetDirty(a); }
        }

        private void DrawGenericRow(Rect rowRect, IdentifiedData d)
        {
            float x = MarginLeft;
            float lineH = EditorGUIUtility.singleLineHeight;
            float y = rowRect.y + (rowRect.height - lineH) * 0.5f;
            // Open
            var rOpen = new Rect(x, rowRect.y + (rowRect.height - 20f) * 0.5f, ColOpen - 12f, 20f);
            if (GUI.Button(rOpen, "Open"))
            { EditorGUIUtility.PingObject(d); Selection.activeObject = d; }
            x += ColOpen + Gap;
            // Id
            var rId = new Rect(x, y, ColId, lineH);
            EditorGUI.LabelField(rId, d.Id ?? string.Empty);
            x += ColId + Gap;
            // Path
            var rPath = new Rect(x, y, ColPath, lineH);
            EditorGUI.LabelField(rPath, FormatPath(AssetDatabase.GetAssetPath(d)));
            x += ColPath + Gap;
        }

        private void DrawItemDataRow(Rect rowRect, ItemData it)
        {
            float x = 4f;
            float lineH = EditorGUIUtility.singleLineHeight;
            float y = rowRect.y + (rowRect.height - lineH) * 0.5f;
            // Open
            var rOpen = new Rect(x, rowRect.y + (rowRect.height - 20f) * 0.5f, ColOpen - 12f, 20f);
            if (GUI.Button(rOpen, "Open"))
            { EditorGUIUtility.PingObject(it); Selection.activeObject = it; }
            x += ColOpen + Gap;
            // Icon (second column)
            var rIcon = new Rect(x, rowRect.y + 2f, ColImage, ColImage);
            if (it.Icon != null)
            {
                var tex = AssetPreview.GetAssetPreview(it.Icon) ?? AssetPreview.GetMiniThumbnail(it.Icon) ?? it.Icon.texture;
                if (tex != null)
                    GUI.DrawTexture(rIcon, tex, ScaleMode.ScaleToFit);
                else
                    Repaint();
            }
            x += ColImage + Gap;
            // Id
            var rId = new Rect(x, y, ColId, lineH);
            EditorGUI.LabelField(rId, it.Id ?? string.Empty);
            x += ColId + Gap;
            // Path
            var rPath = new Rect(x, y, ColPath, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(rPath, FormatPath(AssetDatabase.GetAssetPath(it)));
            x += ColPath + Gap;
            // Tag
            var rTag = new Rect(x, y, ColTag, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(rTag, it.Tag.ToString());
        }

        private void DrawCardDataRow(Rect rowRect, CardData c)
        {
            float x = MarginLeft;
            float lineH = EditorGUIUtility.singleLineHeight;
            float y = rowRect.y + (rowRect.height - lineH) * 0.5f;
            // Open
            var rOpen = new Rect(x, rowRect.y + (rowRect.height - 20f) * 0.5f, ColOpen - 12f, 20f);
            if (GUI.Button(rOpen, "Open"))
            { EditorGUIUtility.PingObject(c); Selection.activeObject = c; }
            x += ColOpen + Gap;
            // Image (second column)
            var sprite = c.ImageAsset != null ? c.ImageAsset.Image : null;
            var rImg = new Rect(x, rowRect.y + 2f, ColImage, ColImage);
            if (sprite != null)
            {
                var tex = AssetPreview.GetAssetPreview(sprite) ?? AssetPreview.GetMiniThumbnail(sprite) ?? sprite.texture;
                if (tex != null)
                    GUI.DrawTexture(rImg, tex, ScaleMode.ScaleToFit);
                else
                    Repaint();
            }
            x += ColImage + Gap;
            // Id
            var rId = new Rect(x, y, ColId, lineH);
            EditorGUI.LabelField(rId, c.Id ?? string.Empty);
            x += ColId + Gap;
            // Collectible
            var rCol = new Rect(x, y, ColCollectible, lineH);
            using (new EditorGUI.DisabledScope(true))
            { EditorGUI.ToggleLeft(rCol, GUIContent.none, c.IsCollectible); }
            // If collectible, draw ItemIcon.Sprite preview within this cell (on the right side)
            if (c.IsCollectible && c.ItemIcon != null && c.ItemIcon.Icon != null)
            {
                const float iconSize = 32f; // doubled from 16
                var iconRect = new Rect(rCol.x + ColCollectible - iconSize - 4f, rowRect.y + (rowRect.height - iconSize) * 0.5f, iconSize, iconSize);
                var tex = AssetPreview.GetAssetPreview(c.ItemIcon.Icon) ?? AssetPreview.GetMiniThumbnail(c.ItemIcon.Icon) ?? c.ItemIcon.Icon.texture;
                if (tex != null)
                    GUI.DrawTexture(iconRect, tex, ScaleMode.ScaleToFit);
                else
                    Repaint();
            }
            x += ColCollectible + Gap;
            // Year
            var rYear = new Rect(x, y, ColYear, lineH);
            EditorGUI.LabelField(rYear, c.Year.ToString());
            x += ColYear + Gap;
            // KnowledgeValue as progress bar 0..10
            var rKv = new Rect(x, y, ColKnowledge, lineH);
            float kv = Mathf.Clamp(c.Points, 0, 10);
            EditorGUI.ProgressBar(rKv, kv / 10f, c.Points.ToString());
            x += ColKnowledge + Gap;
            // Category
            var rCat = new Rect(x, y, ColCategory, lineH);
            EditorGUI.LabelField(rCat, c.Type.ToString());
            x += ColCategory + Gap;
            // Topics
            var topics = c.Topics != null ? string.Join(", ", c.Topics.Select(t => t.ToString())) : string.Empty;
            var rTop = new Rect(x, y, ColTopics, lineH);
            EditorGUI.LabelField(rTop, topics);
            x += ColTopics + Gap;
            // Words linked to this card
            string words = string.Empty;
            if (c.Words != null && c.Words.Count > 0)
            { words = string.Join(", ", c.Words.Where(w => w != null).Select(w => string.IsNullOrEmpty(w.Id) ? w.name : w.Id)); }
            var rWords = new Rect(x, y, ColCardWords, lineH);
            EditorGUI.LabelField(rWords, words);
            x += ColCardWords + Gap;
            // Linked Quests
            string quests = string.Empty;
            if (c.Quests != null && c.Quests.Count > 0)
            { quests = string.Join(", ", c.Quests.Where(q => q != null).Select(q => string.IsNullOrEmpty(q.Id) ? q.name : q.Id)); }
            var rQuests = new Rect(x, y, ColQuests, lineH);
            EditorGUI.LabelField(rQuests, quests);
            x += ColQuests + Gap;
            // Path (last column)
            var rPath = new Rect(x, y, ColPath, lineH);
            EditorGUI.LabelField(rPath, FormatPath(AssetDatabase.GetAssetPath(c)));
        }

        private void DrawQuestDataRow(Rect rowRect, QuestData q)
        {
            float x = MarginLeft;
            float lineH = EditorGUIUtility.singleLineHeight;
            float y = rowRect.y + (rowRect.height - lineH) * 0.5f;
            // Open
            var rOpen = new Rect(x, rowRect.y + (rowRect.height - 20f) * 0.5f, ColOpen - 12f, 20f);
            if (GUI.Button(rOpen, "Open"))
            { EditorGUIUtility.PingObject(q); Selection.activeObject = q; }
            x += ColOpen + Gap;
            // Thumbnail (second column)
            var rThumb = new Rect(x, rowRect.y + 2f, ColImage, ColImage);
            if (q.Thumbnail != null)
            {
                var tex = AssetPreview.GetAssetPreview(q.Thumbnail.Image) ?? AssetPreview.GetMiniThumbnail(q.Thumbnail.Image) ?? q.Thumbnail.Image.texture;
                if (tex != null)
                    GUI.DrawTexture(rThumb, tex, ScaleMode.ScaleToFit);
                else
                    Repaint();
            }
            x += ColImage + Gap;
            // DevStatus
            var rDev = new Rect(x, y, ColQDevStatus, lineH);
            EditorGUI.LabelField(rDev, q.DevStatus.ToString());
            x += ColQDevStatus + Gap;
            // Id + "  - " + IdDisplay
            var rId = new Rect(x, y, ColQId, lineH);
            string idConcat = (q.Id ?? string.Empty) + (string.IsNullOrEmpty(q.IdDisplay) ? string.Empty : ("  -  " + q.IdDisplay));
            EditorGUI.LabelField(rId, idConcat);
            x += ColQId + Gap;
            // Location.Id (prefer IdentifiedData.Id when available)
            var locId = q.Location != null ? (!string.IsNullOrEmpty(q.Location.Id) ? q.Location.Id : q.Location.name) : string.Empty;
            var rLoc = new Rect(x, y, ColQLocation, lineH);
            EditorGUI.LabelField(rLoc, locId);
            x += ColQLocation + Gap;
            // Knowledge Value (sum of linked card values if any)
            int sumKv = 0;
            if (q.Cards != null)
            {
                foreach (var cc in q.Cards)
                {
                    if (cc != null)
                        sumKv += Mathf.Clamp(cc.Points, 0, 10);
                }
            }
            var rQKv = new Rect(x, y, ColQKnowledge, lineH);
            EditorGUI.LabelField(rQKv, sumKv.ToString());
            x += ColQKnowledge + Gap;
            // Difficulty
            var rDiff = new Rect(x, y, ColDifficulty, lineH);
            EditorGUI.LabelField(rDiff, q.Difficulty.ToString());
            x += ColDifficulty + Gap;
            // Category / Topics: show MainTopic and, on a new line, a bullet list of unique sorted card topics
            var rTopic = new Rect(x, rowRect.y + 4f, ColTopic, rowRect.height - 8f);
            string topicsText = q.MainTopic.ToString();
            if (q.Cards != null && q.Cards.Count > 0)
            {
                var allTopics = new List<string>();
                foreach (var cc in q.Cards)
                {
                    if (cc != null && cc.Topics != null)
                        allTopics.AddRange(cc.Topics.Select(t => t.ToString()));
                }
                var uniq = allTopics.Where(s => !string.IsNullOrEmpty(s)).Distinct().OrderBy(s => s).ToList();
                if (uniq.Count > 0)
                {
                    var bullets = "\n" + string.Join("\n", uniq.Select(s => "- " + s));
                    topicsText += bullets;
                }
            }
            GUI.Label(rTopic, topicsText, EditorStyles.wordWrappedLabel);
            x += ColTopic + Gap;
            // Linked Cards
            var cards = q.Cards != null ? string.Join(", ", q.Cards.Where(c => c != null).Select(c => string.IsNullOrEmpty(c.Id) ? c.name : c.Id)) : string.Empty;
            var rCards = new Rect(x, y, ColCards, lineH);
            EditorGUI.LabelField(rCards, cards);
            x += ColCards + Gap;
            // Words Used
            var words = q.Words != null ? string.Join(", ", q.Words.Where(w => w != null).Select(w => string.IsNullOrEmpty(w.Id) ? w.name : w.Id)) : string.Empty;
            var rWords = new Rect(x, y, ColWords, lineH);
            EditorGUI.LabelField(rWords, words);
            x += ColWords + Gap;
        }

        private void DrawWordDataRow(Rect rowRect, WordData w)
        {
            float x = MarginLeft;
            float lineH = EditorGUIUtility.singleLineHeight;
            float y = rowRect.y + (rowRect.height - lineH) * 0.5f;
            // Open
            var rOpen = new Rect(x, rowRect.y + (rowRect.height - 20f) * 0.5f, ColOpen - 12f, 20f);
            if (GUI.Button(rOpen, "Open"))
            { EditorGUIUtility.PingObject(w); Selection.activeObject = w; }
            x += ColOpen + Gap;
            // Id (read-only)
            var rId = new Rect(x, y, ColId, lineH);
            EditorGUI.LabelField(rId, w.Id ?? string.Empty);
            x += ColId + Gap;
            // Active
            var rActive = new Rect(x + 8f, y, ColWActive - 16f, lineH);
            EditorGUI.BeginChangeCheck();
            bool newActive = EditorGUI.Toggle(rActive, w.Active);
            if (EditorGUI.EndChangeCheck())
            { Undo.RecordObject(w, "Toggle Active"); w.Active = newActive; EditorUtility.SetDirty(w); }
            x += ColWActive + Gap;
            // Title (TextEn) read-only
            var rTitle = new Rect(x, y, ColWTitle, lineH);
            EditorGUI.LabelField(rTitle, w.TextEn ?? string.Empty);
            x += ColWTitle + Gap;
            // Category read-only
            var rCat = new Rect(x, y, ColWCategory, lineH);
            EditorGUI.LabelField(rCat, w.Category.ToString());
            x += ColWCategory + Gap;

            // Drawing preview
            var rImg = new Rect(x, rowRect.y + 2f, ColWImage, ColWImage);
            if (w.Drawing != null)
            {
                var tex = AssetPreview.GetAssetPreview(w.Drawing) ?? AssetPreview.GetMiniThumbnail(w.Drawing) ?? w.Drawing.texture;
                if (tex != null)
                    GUI.DrawTexture(rImg, tex, ScaleMode.ScaleToFit);
                else
                    Repaint();
            }
            x += ColWImage + Gap;

            // Drawing Unicode read-only
            var rU = new Rect(x, y, ColWUnicode, lineH);
            EditorGUI.LabelField(rU, w.DrawingUnicode ?? string.Empty);
        }

        private MemberInfo GetBestNameMember(Type t)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            // Prefer fields (includes [SerializeField] private)
            var fields = t.GetFields(flags)
                .Where(f => f.FieldType == typeof(string) && NameMatches(f.Name, "Title", "Name", "_title", "title", "m_Title", "mName", "_name"))
                .OrderBy(f => f.Name.Length)
                .ToList();
            if (fields.Count > 0)
                return fields[0];
            // Then properties
            var props = t.GetProperties(flags)
                .Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite && NameMatches(p.Name, "Title", "Name"))
                .OrderBy(p => p.Name.Length)
                .ToList();
            if (props.Count > 0)
                return props[0];
            return null;
        }

        private static bool NameMatches(string name, params string[] candidates)
        {
            foreach (var c in candidates)
            {
                if (string.Equals(name, c, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private string GetStringMemberValue(object obj, MemberInfo member)
        {
            if (obj == null || member == null)
                return null;
            if (member is FieldInfo fi)
                return fi.GetValue(obj) as string;
            if (member is PropertyInfo pi)
                return pi.GetValue(obj, null) as string;
            return null;
        }

        private void SetStringMemberValue(object obj, MemberInfo member, string value)
        {
            if (obj == null || member == null)
                return;
            if (member is FieldInfo fi)
            { fi.SetValue(obj, value); return; }
            if (member is PropertyInfo pi && pi.CanWrite)
            { pi.SetValue(obj, value, null); }
        }

        private Type SelectedType
        {
            get
            {
                if (_selectedTypeIndex < 0 || _selectedTypeIndex >= _typeOptions.Count)
                    return null;
                var opt = _typeOptions[_selectedTypeIndex];
                // When aggregate is selected, return null and handle filtering separately
                return opt.IsAggregate ? null : opt.Type;
            }
        }

        private IEnumerable<IdentifiedData> Filtered()
        {
            IEnumerable<IdentifiedData> set = _allData;
            if (_selectedTypeIndex >= 0 && _selectedTypeIndex < _typeOptions.Count)
            {
                var opt = _typeOptions[_selectedTypeIndex];
                if (opt.Type != null)
                {
                    if (opt.IsAggregate)
                        set = set.Where(a => a != null && opt.Type.IsAssignableFrom(a.GetType()));
                    else
                        set = set.Where(a => a != null && opt.Type.IsAssignableFrom(a.GetType()));
                }
            }

            // Country filter (only where applicable)
            if (_countryFilter != CountryFilter.All)
            {
                set = set.Where(a => a != null && MatchesCountryFilter(a));
            }

            // CardData-specific quest filter
            if (SelectedType == typeof(CardData) && _cardQuestFilter != null)
            {
                set = set.Where(a => a is CardData c && c.Quests != null && c.Quests.Contains(_cardQuestFilter));
            }
            // WordData Active filter
            if (SelectedType == typeof(WordData) && _wordActive != WordActiveFilter.All)
            {
                set = set.Where(a => a is WordData w && (_wordActive == WordActiveFilter.ActiveOnly ? w.Active : !w.Active));
            }
            // QuestData DevStatus filter
            if (SelectedType == typeof(QuestData) && _devStatusFilter.HasValue)
            {
                var status = _devStatusFilter.Value;
                set = set.Where(a => a is QuestData q && q.DevStatus == status);
            }

            // Search filter across Id, Title/Name and for AssetData also Copyright/Source
            var term = _search?.Trim();
            if (!string.IsNullOrEmpty(term))
            {
                set = set.Where(a => a != null && MatchesSearch(a, term));
            }

            // Apply sorting
            set = ApplySorting(set);
            return set;
        }

        private IEnumerable<IdentifiedData> ApplySorting(IEnumerable<IdentifiedData> set)
        {
            Func<IdentifiedData, string> keySelector = a => string.Empty;
            if (string.IsNullOrEmpty(_sortKey))
            {
                keySelector = a => a?.Id ?? string.Empty;
            }
            else
            {
                switch (_sortKey)
                {
                    case nameof(IdentifiedData.Id):
                        keySelector = a => a?.Id ?? string.Empty;
                        break;
                    case "Path":
                        keySelector = a => FormatPath(AssetDatabase.GetAssetPath(a)) ?? string.Empty;
                        break;
                    case "Title":
                        keySelector = a => a is CardData cd ? (GetCardTitle(cd) ?? string.Empty) : string.Empty;
                        break;
                    case "Category":
                        keySelector = a => a is WordData w3 ? w3.Category.ToString() : (a is CardData c1 ? c1.Type.ToString() : string.Empty);
                        break;
                    case "Active":
                        keySelector = a => a is WordData w0 ? (w0.Active ? "1" : "0") : string.Empty;
                        break;
                    case "TitleEn":
                        keySelector = a => a is WordData w1 ? (w1.TextEn ?? string.Empty) : string.Empty;
                        break;
                    case "DrawingUnicode":
                        keySelector = a => a is WordData w2 ? (w2.DrawingUnicode ?? string.Empty) : string.Empty;
                        break;
                    case "Topics":
                        keySelector = a => a is CardData c2 ? string.Join(",", c2.Topics?.Select(t => t.ToString()) ?? Array.Empty<string>()) : string.Empty;
                        break;
                    case "CardWords":
                        keySelector = a => a is CardData cw ? string.Join(";", cw.Words?.Where(w => w != null).Select(w => string.IsNullOrEmpty(w.Id) ? w.name : w.Id) ?? Array.Empty<string>()) : string.Empty;
                        break;
                    case "Collectible":
                        keySelector = a => a is CardData c3 ? (c3.IsCollectible ? "1" : "0") : string.Empty;
                        break;
                    case "Year":
                        keySelector = a => a is CardData c4 ? c4.Year.ToString("D4") : string.Empty;
                        break;
                    case "KnowledgeValue":
                        keySelector = a => a is CardData c5 ? c5.Points.ToString("D2") : (a is QuestData q0 ? (q0.Cards != null ? q0.Cards.Where(cc => cc != null).Sum(cc => Mathf.Clamp(cc.Points, 0, 10)).ToString("D3") : "") : string.Empty);
                        break;
                    case "MainTopic":
                        keySelector = a => a is QuestData q1 ? q1.MainTopic.ToString() : string.Empty;
                        break;
                    case "IdDisplay":
                        keySelector = a => a is QuestData q2 ? (q2.IdDisplay ?? string.Empty) : string.Empty;
                        break;
                    case "Location.Id":
                        keySelector = a => a is QuestData q3 ? (!string.IsNullOrEmpty(q3.Location?.Id) ? q3.Location.Id : (q3.Location?.name ?? string.Empty)) : string.Empty;
                        break;
                    case "Difficulty":
                        keySelector = a => a is QuestData q4 ? q4.Difficulty.ToString() : string.Empty;
                        break;
                    case "LinkedCards":
                        keySelector = a => a is QuestData q5 ? string.Join(";", q5.Cards?.Where(c => c != null).Select(c => string.IsNullOrEmpty(c.Id) ? c.name : c.Id) ?? Array.Empty<string>()) : string.Empty;
                        break;
                    case "WordsUsed":
                        keySelector = a => a is QuestData q6 ? string.Join(";", q6.Words?.Where(w => w != null).Select(w => string.IsNullOrEmpty(w.Id) ? w.name : w.Id) ?? Array.Empty<string>()) : string.Empty;
                        break;
                    case "LinkedQuests":
                        keySelector = a => a is CardData c6 ? string.Join(";", c6.Quests?.Where(q => q != null).Select(q => string.IsNullOrEmpty(q.Id) ? q.name : q.Id) ?? Array.Empty<string>()) : string.Empty;
                        break;
                    case "Tag":
                        keySelector = a => a is ItemData it ? it.Tag.ToString() : string.Empty;
                        break;
                    case "License":
                        keySelector = a => a is AssetData ad1 ? ad1.License.ToString() : string.Empty;
                        break;
                    case "Copyright":
                        keySelector = a => a is AssetData ad2 ? (ad2.Copyright ?? string.Empty) : string.Empty;
                        break;
                    default:
                        keySelector = a => a?.Id ?? string.Empty;
                        break;
                }
            }
            var ordered = _sortAsc ? set.OrderBy(keySelector) : set.OrderByDescending(keySelector);
            return ordered;
        }

        private IEnumerable<WorldPrefabEntry> FilteredWorldPrefabs()
        {
            IEnumerable<WorldPrefabEntry> set = _worldPrefabs;

            // Category filter
            if (_wpCategoryFilter.HasValue)
            {
                var cat = _wpCategoryFilter.Value;
                set = set.Where(e => GetWorldPrefabCategory(e.Comp) == cat);
            }

            // Country filter (reuse window-level country selector)
            set = set.Where(e => CountryMatches(e.Comp, _countryFilter));

            // Tag filter (optional)
            if (_wpTagFilter.HasValue)
            {
                var tag = _wpTagFilter.Value;
                set = set.Where(e =>
                {
                    var tags = GetWorldPrefabTags(e.Comp);
                    return tags != null && tags.Contains(tag);
                });
            }

            // Kit filter (optional)
            if (_wpKitFilter.HasValue)
            {
                var kit = _wpKitFilter.Value;
                set = set.Where(e => GetWorldPrefabKit(e.Comp) == kit);
            }

            // Search filter over Id, Category, Tags, Path
            var term = _search?.Trim();
            if (!string.IsNullOrEmpty(term))
            {
                set = set.Where(e =>
                {
                    var id = GetWorldPrefabId(e.Comp) ?? string.Empty;
                    var cat = GetWorldPrefabCategory(e.Comp).ToString();
                    var tags = GetWorldPrefabTags(e.Comp)?.Select(t => t.ToString()) ?? Enumerable.Empty<string>();
                    var path = FormatPath(e.Path) ?? string.Empty;
                    return id.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0
                        || cat.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0
                        || tags.Any(t => t.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                        || path.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;
                });
            }

            // Sorting
            Func<WorldPrefabEntry, string> keySelector = e => GetWorldPrefabId(e.Comp) ?? string.Empty;
            if (!string.IsNullOrEmpty(_sortKey))
            {
                switch (_sortKey)
                {
                    case nameof(IdentifiedData.Id):
                        keySelector = e => GetWorldPrefabId(e.Comp) ?? string.Empty;
                        break;
                    case "WPCategory":
                        keySelector = e => GetWorldPrefabCategory(e.Comp).ToString();
                        break;
                    case "WPTags":
                        keySelector = e => string.Join(",", GetWorldPrefabTags(e.Comp)?.Select(t => t.ToString()) ?? Array.Empty<string>());
                        break;
                    case "WPKit":
                        keySelector = e => GetWorldPrefabKit(e.Comp).ToString();
                        break;
                    case "Path":
                        keySelector = e => FormatPath(e.Path) ?? string.Empty;
                        break;
                    default:
                        keySelector = e => GetWorldPrefabId(e.Comp) ?? string.Empty;
                        break;
                }
            }
            set = _sortAsc ? set.OrderBy(keySelector) : set.OrderByDescending(keySelector);
            return set;
        }

        // Reflection helpers for WorldPrefabData private fields
        private static string GetWorldPrefabId(WorldPrefabData c)
        {
            if (c == null)
                return string.Empty;
            // Try reflection first
            var fi = typeof(WorldPrefabData).GetField("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi != null)
            {
                var v = fi.GetValue(c) as string;
                if (!string.IsNullOrEmpty(v))
                    return v;
            }
            // Fallback to SerializedObject
#if UNITY_EDITOR
            var so = new UnityEditor.SerializedObject(c);
            var sp = so.FindProperty("Id");
            if (sp != null)
                return sp.stringValue ?? string.Empty;
#endif
            return string.Empty;
        }
        private static WorldPrefabCategory GetWorldPrefabCategory(WorldPrefabData c)
        {
            if (c == null)
                return default;
            // Reflection
            var fi = typeof(WorldPrefabData).GetField("Category", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi != null)
            {
                try
                { return (WorldPrefabCategory)fi.GetValue(c); }
                catch { }
            }
            // SerializedObject fallback
#if UNITY_EDITOR
            var so = new UnityEditor.SerializedObject(c);
            var sp = so.FindProperty("Category");
            if (sp != null)
            {
                if (sp.propertyType == UnityEditor.SerializedPropertyType.Enum)
                {
                    // Use underlying int value to support non-sequential enum numbers (10,20,...)
                    return (WorldPrefabCategory)sp.intValue;
                }
                if (sp.propertyType == UnityEditor.SerializedPropertyType.Integer)
                {
                    return (WorldPrefabCategory)sp.intValue;
                }
            }
#endif
            return default;
        }
        private static List<WorldPrefabTag> GetWorldPrefabTags(WorldPrefabData c)
        {
            if (c == null)
                return null;
            // Reflection
            var fi = typeof(WorldPrefabData).GetField("Tags", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi != null)
            {
                var list = fi.GetValue(c) as List<WorldPrefabTag>;
                if (list != null)
                    return list;
            }
            // SerializedObject fallback
#if UNITY_EDITOR
            var so = new UnityEditor.SerializedObject(c);
            var sp = so.FindProperty("Tags");
            if (sp != null && sp.isArray)
            {
                var result = new List<WorldPrefabTag>(sp.arraySize);
                for (int i = 0; i < sp.arraySize; i++)
                {
                    var elem = sp.GetArrayElementAtIndex(i);
                    if (elem.propertyType == UnityEditor.SerializedPropertyType.Enum)
                    {
                        // Use underlying int value to support non-sequential enum numbers
                        result.Add((WorldPrefabTag)elem.intValue);
                    }
                    else if (elem.propertyType == UnityEditor.SerializedPropertyType.Integer)
                    {
                        result.Add((WorldPrefabTag)elem.intValue);
                    }
                }
                return result;
            }
#endif
            return null;
        }

        // Read WorldPrefabData.Country and Kit with reflection and SerializedObject fallbacks
        private static Countries GetWorldPrefabCountry(WorldPrefabData c)
        {
            if (c == null)
                return Countries.International;
            var fi = typeof(WorldPrefabData).GetField("Country", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi != null)
            {
                try
                { return (Countries)fi.GetValue(c); }
                catch { }
            }
#if UNITY_EDITOR
            var so = new UnityEditor.SerializedObject(c);
            var sp = so.FindProperty("Country");
            if (sp != null)
            {
                if (sp.propertyType == UnityEditor.SerializedPropertyType.Enum || sp.propertyType == UnityEditor.SerializedPropertyType.Integer)
                    return (Countries)sp.intValue;
            }
#endif
            return Countries.International;
        }

        private static WorldPrefabKit GetWorldPrefabKit(WorldPrefabData c)
        {
            if (c == null)
                return WorldPrefabKit.None;
            var fi = typeof(WorldPrefabData).GetField("Kit", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi != null)
            {
                try
                { return (WorldPrefabKit)fi.GetValue(c); }
                catch { }
            }
#if UNITY_EDITOR
            var so = new UnityEditor.SerializedObject(c);
            var sp = so.FindProperty("Kit");
            if (sp != null)
            {
                if (sp.propertyType == UnityEditor.SerializedPropertyType.Enum || sp.propertyType == UnityEditor.SerializedPropertyType.Integer)
                    return (WorldPrefabKit)sp.intValue;
            }
#endif
            return WorldPrefabKit.None;
        }

        // Apply the window's CountryFilter to a WorldPrefabData component
        private static bool CountryMatches(WorldPrefabData comp, CountryFilter filter)
        {
            var value = GetWorldPrefabCountry(comp);
            switch (filter)
            {
                case CountryFilter.All:
                    return true;
                case CountryFilter.Global:
                    return value == Countries.International;
                case CountryFilter.France:
                    return value == Countries.France;
                case CountryFilter.Italy:
                    return value == Countries.Italy;
                case CountryFilter.Poland:
                    return value == Countries.Poland;
                case CountryFilter.Spain:
                    return value == Countries.Spain;
                case CountryFilter.Germany:
                    return value == Countries.Germany;
                case CountryFilter.UnitedKingdom:
                    return value == Countries.UnitedKingdom;
                case CountryFilter.Portugal:
                    return value == Countries.Portugal;
                case CountryFilter.Greece:
                    return value == Countries.Greece;
                default:
                    return true;
            }
        }

        private bool MatchesSearch(IdentifiedData a, string term)
        {
            if (!string.IsNullOrEmpty(a.Id) && a.Id.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            var nameVal = GetStringMemberValue(a, GetBestNameMember(a.GetType()));
            if (!string.IsNullOrEmpty(nameVal) && nameVal.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            if (a is AssetData ad)
            {
                if (!string.IsNullOrEmpty(ad.Copyright) && ad.Copyright.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
                if (!string.IsNullOrEmpty(ad.SourceUrl) && ad.SourceUrl.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            if (a is ItemData it)
            {
                if (it.Tag.ToString().IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            if (a is CardData cd)
            {
                var title = GetCardTitle(cd) ?? string.Empty;
                if (!string.IsNullOrEmpty(title) && title.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
                if (cd.Type.ToString().IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
                if (cd.Year.ToString().IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
                if (cd.Topics != null && cd.Topics.Any(t => t.ToString().IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0))
                    return true;
                if (cd.Quests != null && cd.Quests.Any(q => q != null && ((string.IsNullOrEmpty(q.Id) ? q.name : q.Id).IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)))
                    return true;
                if (cd.Words != null && cd.Words.Any(w => w != null && ((string.IsNullOrEmpty(w.Id) ? w.name : w.Id).IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)))
                    return true;
                if (cd.IsCollectible && ("collectible".IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 || "true".IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0))
                    return true;
            }
            if (a is QuestData qd)
            {
                if (qd.MainTopic.ToString().IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
                if (qd.Location != null)
                {
                    var locId = !string.IsNullOrEmpty(qd.Location.Id) ? qd.Location.Id : qd.Location.name;
                    if (!string.IsNullOrEmpty(locId) && locId.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                }
                if (qd.Cards != null && qd.Cards.Any(c => c != null && ((string.IsNullOrEmpty(c.Id) ? c.name : c.Id).IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)))
                    return true;
                if (qd.Words != null && qd.Words.Any(w => w != null && ((string.IsNullOrEmpty(w.Id) ? w.name : w.Id).IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)))
                    return true;
            }
            return false;
        }

        private static Type FindTypeByName(string name)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var t = asm.GetTypes().FirstOrDefault(x => x != null && x.Name == name);
                    if (t != null)
                        return t;
                }
                catch (ReflectionTypeLoadException ex)
                {
                    var t = ex.Types.FirstOrDefault(x => x != null && x.Name == name);
                    if (t != null)
                        return t;
                }
            }
            return null;
        }

        // Replace currently selected scene objects with the prefab at given asset path, preserving hierarchy and transforms
        private static void ReplaceSelectedWithPrefab(string prefabAssetPath)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabAssetPath);
            if (prefab == null)
            {
                Debug.LogWarning($"[GameDataBrowser] Prefab not found at {prefabAssetPath}");
                return;
            }

            var selection = Selection.gameObjects;
            if (selection == null || selection.Length == 0)
                return;

            Undo.IncrementCurrentGroup();
            int group = Undo.GetCurrentGroup();
            var newSelection = new System.Collections.Generic.List<GameObject>(selection.Length);
            foreach (var go in selection)
            {
                if (go == null)
                    continue;
                if (!go.scene.IsValid() || !go.scene.isLoaded)
                    continue; // only hierarchy objects
                var parent = go.transform.parent;
                int sibling = go.transform.GetSiblingIndex();
                var localPos = go.transform.localPosition;
                var localRot = go.transform.localRotation;
                var localScale = go.transform.localScale;
                var name = go.name;

                // Instantiate prefab
                GameObject newObj = PrefabUtility.InstantiatePrefab(prefab, go.scene) as GameObject;
                if (newObj == null)
                    newObj = UnityEngine.Object.Instantiate(prefab);

                Undo.RegisterCreatedObjectUndo(newObj, "Replace With Prefab");
                // Rename to child name if available, otherwise keep original replaced object's name
                if (newObj.transform.childCount > 0)
                {
                    newObj.name = newObj.transform.GetChild(0).name;
                }
                else
                {
                    newObj.name = name;
                }
                newObj.transform.SetParent(parent, worldPositionStays: false);
                newObj.transform.SetSiblingIndex(sibling);
                newObj.transform.localPosition = localPos;
                newObj.transform.localRotation = localRot;
                newObj.transform.localScale = localScale;

                Undo.DestroyObjectImmediate(go);
                newSelection.Add(newObj);
            }
            Undo.CollapseUndoOperations(group);
            if (newSelection.Count > 0)
            {
                UnityEngine.Object[] arr = new UnityEngine.Object[newSelection.Count];
                for (int i = 0; i < newSelection.Count; i++)
                    arr[i] = newSelection[i];
                Selection.objects = arr;
            }
            SceneView.RepaintAll();
        }

        private static string NicifyTypeLabel(Type t)
        {
            if (t == null)
                return "All Data";
            if (t.Name == "ActivitySettingsAbstract")
                return "Activities";
            return ObjectNames.NicifyVariableName(t.Name);
        }

        // Country selector options with custom order and separator
        private struct CountryOption
        {
            public string Label;
            public CountryFilter Value;
            public bool IsSeparator;
        }

        private List<CountryOption> GetCountryOptions()
        {
            var list = new List<CountryOption>();
            // First: All, Global, France, Poland
            list.Add(new CountryOption { Label = "All", Value = CountryFilter.All, IsSeparator = false });
            list.Add(new CountryOption { Label = "Global", Value = CountryFilter.Global, IsSeparator = false });
            list.Add(new CountryOption { Label = "France", Value = CountryFilter.France, IsSeparator = false });
            list.Add(new CountryOption { Label = "Poland", Value = CountryFilter.Poland, IsSeparator = false });
            // Separator
            list.Add(new CountryOption { Label = "—", Value = _countryFilter, IsSeparator = true });
            // Others in a fixed order
            list.Add(new CountryOption { Label = "Germany", Value = CountryFilter.Germany, IsSeparator = false });
            list.Add(new CountryOption { Label = "Greece", Value = CountryFilter.Greece, IsSeparator = false });
            list.Add(new CountryOption { Label = "Italy", Value = CountryFilter.Italy, IsSeparator = false });
            list.Add(new CountryOption { Label = "Portugal", Value = CountryFilter.Portugal, IsSeparator = false });
            list.Add(new CountryOption { Label = "Spain", Value = CountryFilter.Spain, IsSeparator = false });
            list.Add(new CountryOption { Label = "United Kingdom", Value = CountryFilter.UnitedKingdom, IsSeparator = false });
            return list;
        }

        private string FormatPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            if (path.StartsWith(DiscoverPathPrefix, StringComparison.Ordinal))
                return path.Substring(DiscoverPathPrefix.Length);
            return path;
        }

        private bool MatchesCountryFilter(IdentifiedData a)
        {
            try
            {
                var t = a.GetType();
                var fi = t.GetField("Country", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var pi = t.GetProperty("Country", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi == null && pi == null)
                    return true; // no-op
                Countries value;
                if (fi != null && fi.FieldType == typeof(Countries))
                {
                    value = (Countries)fi.GetValue(a);
                }
                else if (pi != null && pi.PropertyType == typeof(Countries) && pi.CanRead)
                {
                    value = (Countries)pi.GetValue(a, null);
                }
                else
                    return true;

                switch (_countryFilter)
                {
                    case CountryFilter.Global:
                        return value == Countries.International;
                    case CountryFilter.France:
                        return value == Countries.France;
                    case CountryFilter.Italy:
                        return value == Countries.Italy;
                    case CountryFilter.Poland:
                        return value == Countries.Poland;
                    case CountryFilter.Spain:
                        return value == Countries.Spain;
                    case CountryFilter.Germany:
                        return value == Countries.Germany;
                    case CountryFilter.UnitedKingdom:
                        return value == Countries.UnitedKingdom;
                    case CountryFilter.Portugal:
                        return value == Countries.Portugal;
                    case CountryFilter.Greece:
                        return value == Countries.Greece;
                    case CountryFilter.All:
                    default:
                        return true;
                }
            }
            catch { return true; }
        }

        private string GetCardTitle(CardData c)
        {
            if (c == null)
                return null;
            if (_cardTitleCache.TryGetValue(c, out var cached) && !string.IsNullOrEmpty(cached))
                return cached;
            try
            {
                var s = c.Title.GetLocalizedString();
                if (!string.IsNullOrEmpty(s))
                { _cardTitleCache[c] = s; return s; }
            }
            catch { }
            return null;
        }

        private void ExportCsv()
        {
            var list = Filtered().ToList();
            if (list.Count == 0)
            {
                EditorUtility.DisplayDialog("Export CSV", "No items to export.", "OK");
                return;
            }
            // Default filename: Antura - dataname - yyyy-mm-dd.csv
            string dataName = (_selectedTypeIndex >= 0 && _selectedTypeIndex < _typeOptions.Count) ? _typeOptions[_selectedTypeIndex].Label : "Data";
            if (string.IsNullOrEmpty(dataName))
                dataName = "Data";
            // sanitize filename
            foreach (var ch in System.IO.Path.GetInvalidFileNameChars())
                dataName = dataName.Replace(ch, '-');
            var dateStr = DateTime.Now.ToString("yyyy-MM-dd");
            var defaultName = $"Antura - {dataName} - {dateStr}.csv";
            var path = EditorUtility.SaveFilePanel("Export CSV", Application.dataPath, defaultName, "csv");
            if (string.IsNullOrEmpty(path))
                return;

            var sb = new System.Text.StringBuilder();
            if (SelectedType == typeof(AssetData))
            {
                sb.AppendLine("Type,Id,Path,License,Copyright,SourceUrl");
                foreach (var a in list.Cast<AssetData>())
                {
                    sb.AppendLine(Escape("AssetData") + "," + Escape(a.Id) + "," + Escape(FormatPath(AssetDatabase.GetAssetPath(a))) + "," + Escape(a.License.ToString()) + "," + Escape(a.Copyright) + "," + Escape(a.SourceUrl));
                }
            }
            else if (SelectedType == typeof(WorldPrefabData))
            {
                var wpList = FilteredWorldPrefabs().ToList();
                sb.AppendLine("Type,Id,Category,Tags,Path");
                foreach (var e in wpList)
                {
                    var id = GetWorldPrefabId(e.Comp);
                    var cat = GetWorldPrefabCategory(e.Comp).ToString();
                    var tags = string.Join("; ", GetWorldPrefabTags(e.Comp)?.Select(t => t.ToString()) ?? Array.Empty<string>());
                    sb.AppendLine(Escape("WorldPrefab") + "," + Escape(id) + "," + Escape(cat) + "," + Escape(tags) + "," + Escape(FormatPath(e.Path)));
                }
            }
            else if (SelectedType == typeof(ItemData))
            {
                sb.AppendLine("Type,Id,Path,Tag");
                foreach (var it in list.Cast<ItemData>())
                {
                    sb.AppendLine(Escape("ItemData") + "," + Escape(it.Id) + "," + Escape(FormatPath(AssetDatabase.GetAssetPath(it))) + "," + Escape(it.Tag.ToString()));
                }
            }
            else if (SelectedType == typeof(CardData))
            {
                sb.AppendLine("Type,Id,Path,Title,Collectible,Year,Category,Topics,LinkedQuests");
                foreach (var c in list.Cast<CardData>())
                {
                    var topics = c.Topics != null ? string.Join("; ", c.Topics.Select(t => t.ToString())) : string.Empty;
                    var quests = c.Quests != null ? string.Join("; ", c.Quests.Where(q => q != null).Select(q => string.IsNullOrEmpty(q.Id) ? q.name : q.Id)) : string.Empty;
                    sb.AppendLine(Escape("CardData") + "," + Escape(c.Id) + "," + Escape(FormatPath(AssetDatabase.GetAssetPath(c))) + "," + Escape(GetCardTitle(c)) + "," + Escape(c.IsCollectible ? "true" : "false") + "," + Escape(c.Year.ToString()) + "," + Escape(c.Type.ToString()) + "," + Escape(topics) + "," + Escape(quests));
                }
            }
            else if (SelectedType == typeof(QuestData))
            {
                sb.AppendLine("Type,DevStatus,Id,IdDisplay,LocationId,Knowledge,Difficulty,MainTopic,LinkedCards,WordsUsed");
                foreach (var q in list.Cast<QuestData>())
                {
                    string locId = q.Location != null ? (!string.IsNullOrEmpty(q.Location.Id) ? q.Location.Id : q.Location.name) : string.Empty;
                    string linkedCards = q.Cards != null ? string.Join("; ", q.Cards.Where(c => c != null).Select(c => string.IsNullOrEmpty(c.Id) ? c.name : c.Id)) : string.Empty;
                    string words = q.Words != null ? string.Join("; ", q.Words.Where(w => w != null).Select(w => string.IsNullOrEmpty(w.Id) ? w.name : w.Id)) : string.Empty;
                    sb.AppendLine(
                        Escape("QuestData") + "," +
                        Escape(q.DevStatus.ToString()) + "," +
                        Escape(q.Id) + "," +
                        Escape(q.IdDisplay) + "," +
                        Escape(locId) + "," +
                        Escape(q.KnowledgeValue.ToString()) + "," +
                        Escape(q.Difficulty.ToString()) + "," +
                        Escape(q.MainTopic.ToString()) + "," +
                        Escape(linkedCards) + "," +
                        Escape(words)
                    );
                }
            }
            else
            {
                sb.AppendLine("Type,Id,Path");
                foreach (var d in list)
                {
                    sb.AppendLine(Escape(d.GetType().Name) + "," + Escape(d.Id) + "," + Escape(FormatPath(AssetDatabase.GetAssetPath(d))));
                }
            }

            try
            { System.IO.File.WriteAllText(path, sb.ToString()); }
            catch (Exception ex)
            {
                Debug.LogError($"[GameDataBrowser] Failed to write CSV: {ex.Message}");
            }
        }

        private static string Escape(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            s = s.Replace("\r", " ").Replace("\n", " ");
            if (s.Contains(",") || s.Contains("\"") || s.Contains(";"))
            {
                s = s.Replace("\"", "\"\"");
                return "\"" + s + "\"";
            }
            return s;
        }

    }
}
#endif
