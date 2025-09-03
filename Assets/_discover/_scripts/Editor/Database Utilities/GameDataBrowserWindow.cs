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
        // Pagination
        private int _pageIndex = 0;
        private const int PageSize = 100;

        // Layout sizes
        private const float ColOpen = 60f;
        private const float ColId = 180f;
        private const float ColImage = 80f;
        private const float ColPath = 380f;
        private const float ColTag = 140f;
        private const float ColLicense = 110f;
        private const float ColCopyright = 300f;
        private const float ColSource = 220f; // AssetData: Source URL column
        private const float ColACards = 260f; // AssetData: Cards column
        private const float ColAType = 100f;   // AssetData: Type column
        private const float RowHeight = ColImage + 12f;

        // Quest-specific narrower columns
        private const float ColQId = 160f;
        private const float ColQDevStatus = 110f;
        private const float ColQTitle = 150f;
        private const float ColQLocation = 100f;
        private const float ColQKnowledge = 60f;
        // Card-specific knowledge column
        private const float ColPoints = 80f;
        private const float ColQTopics = 200f;
        // QuestData Knowledges column (list of KnowledgeData items)
        private const float ColQKnowledges = 100f;

        // WorldPrefabData specific
        private const float ColWPCategory = 100f;
        private const float ColWPTags = 100f;
        private const float ColWPKit = 100f;
        private const float ColWPReplace = 120f;
        private WorldPrefabCategory? _wpCategoryFilter = null; // null => All categories
        private WorldPrefabTag? _wpTagFilter = null; // null => (Any)
        private WorldPrefabKit? _wpKitFilter = null; // null => (Any)

        // CardData specific
        private const float ColTitle = 140f;
        private const float ColCollectible = 60f;
        private const float ColYear = 60f;
        private const float ColCategory = 100f;
        private const float ColImportance = 80f;
        private const float ColTopics = 100f;
        private const float ColCardKnowledges = 200f;
        private const float ColCardWords = 140f;
        private const float ColQuests = 100f;
        private readonly Dictionary<CardData, string> _cardTitleCache = new Dictionary<CardData, string>();
        // Preview cache to avoid recomputing AssetPreview each frame
        private readonly Dictionary<UnityEngine.Object, Texture> _previewCache = new Dictionary<UnityEngine.Object, Texture>();

        // KnowledgeData specific
        private const float ColKName = 200f;
        private const float ColKDesc = 260f;
        private const float ColKImportance = 120f;
        private const float ColKCoreCard = 180f;
        private const float ColKConnections = 300f;
        private const float ColKAge = 120f;
        private const float ColKTopics = 220f;
        private const float ColKQuests = 260f;

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

        // CardData toolbar filters
        private CardType? _cardTypeFilter = null; // null => (Any)
        private Importance? _cardImportanceFilter = null; // null => (Any)
        private Status? _cardStatusFilter = null; // null => All
        private Subject? _cardTopicFilter = null; // null => (Any)

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
                // DevStatus, Title, Id(+IdDisplay), Location, Topic, Difficulty, Category, Subjects, LinkedCards, WordsUsed
                width += ColQDevStatus + Gap + ColQTitle + Gap + ColQId + Gap + ColQLocation + Gap + ColQKnowledge + Gap + ColDifficulty + Gap + ColTopic + Gap + ColQTopics + Gap + ColQKnowledges + Gap + ColCards + Gap + ColWords;
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
                // Status, Id, Title, Importance, Type, Topics, Collectible(+Icon), Year, Points, Knowledge(list), Words, Linked Quests, Path (Image already counted above)
                width += ColQDevStatus + Gap + ColId + Gap + ColTitle + Gap + ColImportance + Gap + ColCategory + Gap + ColTopics + Gap + ColCollectible + Gap + ColYear + Gap + ColPoints + Gap + ColCardKnowledges + Gap + ColCardWords + Gap + ColQuests + Gap + ColPath;
            }
            else if (SelectedType == typeof(TopicData))
            {
                // Id, Name, Description, Importance, CoreCard, Connections, targetAge, Topics, Quests
                width += ColId + Gap + ColKName + Gap + ColKDesc + Gap + ColKImportance + Gap + ColKCoreCard + Gap + ColKConnections + Gap + ColKAge + Gap + ColKTopics + Gap + ColKQuests;
            }
            else if (SelectedType == typeof(ItemData))
            {
                width += ColId + Gap + ColPath + Gap + ColTag;
            }
            else if (SelectedType == typeof(AssetData))
            {
                // Status, Id, Type, Cards, License, Source, Copyright, Path (Image already counted above)
                width += ColQDevStatus + Gap + ColId + Gap + ColAType + Gap + ColACards + Gap + ColLicense + Gap + ColSource + Gap + ColCopyright + Gap + ColPath;
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
                var knowledgeType = concreteTypes.FirstOrDefault(t => t.Name == nameof(TopicData));
                var assetType = concreteTypes.FirstOrDefault(t => t.Name == nameof(AssetData));
                var wordType = concreteTypes.FirstOrDefault(t => t.Name == nameof(WordData));
                var taskType = concreteTypes.FirstOrDefault(t => t.Name == nameof(TaskData));
                var activityType = concreteTypes.FirstOrDefault(t => t.Name == nameof(ActivityData));
                var locationType = concreteTypes.FirstOrDefault(t => t.Name == nameof(LocationData));
                var bonusmalusType = concreteTypes.FirstOrDefault(t => t.Name == nameof(BonusMalusData));
                var itemType = concreteTypes.FirstOrDefault(t => t.Name == nameof(ItemData));
                if (questType != null)
                    topTypes.Add(questType);
                if (cardType != null)
                    topTypes.Add(cardType);
                if (knowledgeType != null)
                    topTypes.Add(knowledgeType);
                if (assetType != null)
                    topTypes.Add(assetType);
                if (wordType != null)
                    topTypes.Add(wordType);
                if (taskType != null)
                    topTypes.Add(taskType);
                if (activityType != null)
                    topTypes.Add(activityType);
                if (itemType != null)
                    topTypes.Add(itemType);
                if (locationType != null)
                    topTypes.Add(locationType);
                if (bonusmalusType != null)
                    topTypes.Add(bonusmalusType);

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
                //_typeOptions.Add(new TypeOption { Label = "All Data", Type = null, IsAggregate = false, IsSeparator = false });

                // Aggregate: Activities (all ActivitySettingsAbstract and subclasses)
                // var activitiesBase = FindTypeByName("ActivitySettingsAbstract");
                // if (activitiesBase != null)
                // {
                //     _typeOptions.Add(new TypeOption { Label = "Activities (All)", Type = activitiesBase, IsAggregate = true, IsSeparator = false });
                // }

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
            GUILayout.Space(4);
            DrawFooter();
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                bool searchDrawn = false; // ensure we render the search box only once
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
                        _pageIndex = 0;
                    }
                    Repaint();
                }

                // Reset button to clear all filters, search, sorting and scroll
                if (GUILayout.Button("Reset", EditorStyles.toolbarButton, GUILayout.Width(54)))
                {
                    _search = string.Empty;
                    _countryFilter = CountryFilter.All;
                    _devStatusFilter = null;
                    _cardQuestFilter = null;
                    _wordActive = WordActiveFilter.All;
                    _wpCategoryFilter = null;
                    _wpTagFilter = null;
                    _wpKitFilter = null;
                    _cardTypeFilter = null;
                    _cardTopicFilter = null;
                    _cardImportanceFilter = null;
                    _cardStatusFilter = null;
                    _sortKey = null;
                    _sortAsc = true;
                    _scroll = Vector2.zero;
                    _pageIndex = 0;
                    RefreshList();
                    Repaint();
                }

                // For CardData, show the search box immediately after Reset so it's the first control in the header after Reset
                if (SelectedType == typeof(CardData))
                {
                    GUILayout.Space(6);
                    if (_searchField == null)
                        _searchField = new SearchField();
                    var newSearchEarly = _searchField.OnToolbarGUI(_search, GUILayout.MinWidth(160));
                    if (!string.Equals(newSearchEarly, _search, StringComparison.Ordinal))
                    { _search = newSearchEarly; _pageIndex = 0; Repaint(); }
                    searchDrawn = true;
                }

                GUILayout.Space(6);
                // Country selector (compact): first entry is the label => no filter (All)
                var rawCOpts = GetCountryOptions();
                var cOpts = rawCOpts.Where(o => !o.IsSeparator).ToList();
                var cLabels = new List<string> { "Country" };
                cLabels.AddRange(cOpts.Select(o => o.Label));
                int cIndex = 0;
                if (_countryFilter != CountryFilter.All)
                {
                    int optIdx = cOpts.FindIndex(o => o.Value.Equals(_countryFilter));
                    cIndex = optIdx >= 0 ? optIdx + 1 : 0;
                }
                int newCIndex = EditorGUILayout.Popup(cIndex, cLabels.ToArray(), EditorStyles.toolbarPopup, GUILayout.Width(120));
                if (newCIndex != cIndex)
                {
                    _countryFilter = (newCIndex <= 0) ? CountryFilter.All : cOpts[newCIndex - 1].Value;
                    _pageIndex = 0;
                    Repaint();
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
                        _pageIndex = 0;
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
                        _pageIndex = 0;
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
                            _pageIndex = 0;
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
                            _pageIndex = 0;
                            Repaint();
                        }
                    }
                }

                // When browsing CardData, show compact filter popups with label as first option
                if (SelectedType == typeof(CardData))
                {
                    GUILayout.Space(6);
                    // Quest filter (first option is the label => no filter)
                    var allQuests = _allData.OfType<QuestData>().OrderBy(q => q.Id ?? q.name).ToList();
                    var questLabels = new List<string> { "Quest" };
                    questLabels.AddRange(allQuests.Select(q => string.IsNullOrEmpty(q.Id) ? q.name : q.Id));
                    int currentIndex = 0;
                    if (_cardQuestFilter != null)
                    {
                        var idx = allQuests.IndexOf(_cardQuestFilter);
                        if (idx >= 0)
                            currentIndex = idx + 1;
                    }
                    int newQ = EditorGUILayout.Popup(currentIndex, questLabels.ToArray(), EditorStyles.toolbarPopup, GUILayout.Width(120));
                    if (newQ != currentIndex)
                    {
                        _cardQuestFilter = newQ <= 0 ? null : allQuests[newQ - 1];
                        _pageIndex = 0;
                        Repaint();
                    }

                    GUILayout.Space(4);
                    // Type filter
                    var typeValues = (CardType[])Enum.GetValues(typeof(CardType));
                    var typeLabels = new List<string> { "Type" };
                    typeLabels.AddRange(typeValues.Select(v => v.ToString()));
                    int curType = _cardTypeFilter.HasValue ? (Array.IndexOf(typeValues, _cardTypeFilter.Value) + 1) : 0;
                    int pickType = EditorGUILayout.Popup(curType, typeLabels.ToArray(), EditorStyles.toolbarPopup, GUILayout.Width(110));
                    if (pickType != curType)
                    {
                        _cardTypeFilter = pickType <= 0 ? default(CardType?) : typeValues[pickType - 1];
                        _pageIndex = 0;
                        Repaint();
                    }

                    GUILayout.Space(4);
                    // Subjects filter
                    var topicValues = (Subject[])Enum.GetValues(typeof(Subject));
                    var topicLabels = new List<string> { "Subjects" };
                    topicLabels.AddRange(topicValues.Select(v => v.ToString()));
                    int curTopic = _cardTopicFilter.HasValue ? (Array.IndexOf(topicValues, _cardTopicFilter.Value) + 1) : 0;
                    int pickTopic = EditorGUILayout.Popup(curTopic, topicLabels.ToArray(), EditorStyles.toolbarPopup, GUILayout.Width(120));
                    if (pickTopic != curTopic)
                    {
                        _cardTopicFilter = pickTopic <= 0 ? default(Subject?) : topicValues[pickTopic - 1];
                        _pageIndex = 0;
                        Repaint();
                    }

                    GUILayout.Space(4);
                    // Importance filter
                    var impValues = (Importance[])Enum.GetValues(typeof(Importance));
                    var impLabels = new List<string> { "Importance" };
                    impLabels.AddRange(impValues.Select(v => v.ToString()));
                    int curImp = _cardImportanceFilter.HasValue ? (Array.IndexOf(impValues, _cardImportanceFilter.Value) + 1) : 0;
                    int pickImp = EditorGUILayout.Popup(curImp, impLabels.ToArray(), EditorStyles.toolbarPopup, GUILayout.Width(120));
                    if (pickImp != curImp)
                    {
                        _cardImportanceFilter = pickImp <= 0 ? default(Importance?) : impValues[pickImp - 1];
                        _pageIndex = 0;
                        Repaint();
                    }

                    GUILayout.Space(4);
                    // Status filter
                    var stValues = (Status[])Enum.GetValues(typeof(Status));
                    var stLabels = new List<string> { "Status" };
                    stLabels.AddRange(stValues.Select(v => v.ToString()));
                    int curStatus = _cardStatusFilter.HasValue ? (Array.IndexOf(stValues, _cardStatusFilter.Value) + 1) : 0;
                    int pickStatus = EditorGUILayout.Popup(curStatus, stLabels.ToArray(), EditorStyles.toolbarPopup, GUILayout.Width(100));
                    if (pickStatus != curStatus)
                    {
                        _cardStatusFilter = pickStatus <= 0 ? default(Status?) : stValues[pickStatus - 1];
                        _pageIndex = 0;
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
                    { _wordActive = (WordActiveFilter)newAct; _pageIndex = 0; Repaint(); }
                }

                // Draw the generic search box only if not already drawn earlier
                if (!searchDrawn)
                {
                    if (_searchField == null)
                        _searchField = new SearchField();
                    var newSearch = _searchField.OnToolbarGUI(_search, GUILayout.MinWidth(100));
                    if (!string.Equals(newSearch, _search, StringComparison.Ordinal))
                    { _search = newSearch; _pageIndex = 0; Repaint(); }
                }

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
                HeaderLabelRect("Status", new Rect(x, y, ColQDevStatus, lineH), "Status");
                x += ColQDevStatus + Gap;
                HeaderLabelRect("Title", new Rect(x, y, ColQTitle, lineH), "TitleEn");
                x += ColQTitle + Gap;
                HeaderLabelRect("Id — Display", new Rect(x, y, ColQId, lineH), "IdConcat");
                x += ColQId + Gap;
                HeaderLabelRect("Location", new Rect(x, y, ColQLocation, lineH), "Location");
                x += ColQLocation + Gap;
                HeaderLabelRect("Points", new Rect(x, y, ColQKnowledge, lineH), "KnowledgeValue");
                x += ColQKnowledge + Gap;
                HeaderLabelRect("Difficulty", new Rect(x, y, ColDifficulty, lineH), "Difficulty");
                x += ColDifficulty + Gap;
                HeaderLabelRect("Category", new Rect(x, y, ColTopic, lineH), "MainTopic");
                x += ColTopic + Gap;
                HeaderLabelRect("Subjects", new Rect(x, y, ColQTopics, lineH), "Topics");
                x += ColQTopics + Gap;
                HeaderLabelRect("Knowledges", new Rect(x, y, ColQKnowledges, lineH), "Knowledges");
                x += ColQKnowledges + Gap;
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
                HeaderLabelRect("Status", new Rect(x, y, ColQDevStatus, lineH), "Status");
                x += ColQDevStatus + Gap;
                HeaderLabelRect("Id", new Rect(x, y, ColId, lineH), nameof(IdentifiedData.Id));
                x += ColId + Gap;
                HeaderLabelRect("Title", new Rect(x, y, ColTitle, lineH), "Title");
                x += ColTitle + Gap;
                HeaderLabelRect("Importance", new Rect(x, y, ColImportance, lineH), "Importance");
                x += ColImportance + Gap;
                HeaderLabelRect("Type", new Rect(x, y, ColCategory, lineH), "Category");
                x += ColCategory + Gap;
                HeaderLabelRect("Subjects", new Rect(x, y, ColTopics, lineH), "Subjects");
                x += ColTopics + Gap;
                HeaderLabelRect("Collectible", new Rect(x, y, ColCollectible, lineH), "Collectible");
                x += ColCollectible + Gap;
                HeaderLabelRect("Year", new Rect(x, y, ColYear, lineH), "Year");
                x += ColYear + Gap;
                HeaderLabelRect("Points", new Rect(x, y, ColPoints, lineH), "KnowledgeValue");
                x += ColPoints + Gap;
                HeaderLabelRect("Knowledge", new Rect(x, y, ColCardKnowledges, lineH), "CardKnowledges");
                x += ColCardKnowledges + Gap;
                HeaderLabelRect("Words", new Rect(x, y, ColCardWords, lineH), "CardWords");
                x += ColCardWords + Gap;
                HeaderLabelRect("Linked Quests", new Rect(x, y, ColQuests, lineH), "LinkedQuests");
                x += ColQuests + Gap;
                HeaderLabelRect("Path", new Rect(x, y, ColPath, lineH), "Path");
                x += ColPath + Gap;
            }
            else if (SelectedType == typeof(TopicData))
            {
                HeaderLabelRect("Id", new Rect(x, y, ColId, lineH), nameof(IdentifiedData.Id));
                x += ColId + Gap;
                HeaderLabelRect("Name", new Rect(x, y, ColKName, lineH), "Name");
                x += ColKName + Gap;
                HeaderLabelRect("Description", new Rect(x, y, ColKDesc, lineH), "Description");
                x += ColKDesc + Gap;
                HeaderLabelRect("Importance", new Rect(x, y, ColKImportance, lineH), "Importance");
                x += ColKImportance + Gap;
                HeaderLabelRect("Core Card", new Rect(x, y, ColKCoreCard, lineH), "CoreCard");
                x += ColKCoreCard + Gap;
                HeaderLabelRect("Connections", new Rect(x, y, ColKConnections, lineH), "Connections");
                x += ColKConnections + Gap;
                HeaderLabelRect("Target Age", new Rect(x, y, ColKAge, lineH), "TargetAge");
                x += ColKAge + Gap;
                HeaderLabelRect("Subjects", new Rect(x, y, ColKTopics, lineH), "Subjects");
                x += ColKTopics + Gap;
                HeaderLabelRect("Quests", new Rect(x, y, ColKQuests, lineH), "KQuests");
                x += ColKQuests + Gap;
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
                HeaderLabelRect("Status", new Rect(x, y, ColQDevStatus, lineH), "Status");
                x += ColQDevStatus + Gap;
                HeaderLabelRect("Id", new Rect(x, y, ColId, lineH), nameof(IdentifiedData.Id));
                x += ColId + Gap;
                HeaderLabelRect("Type", new Rect(x, y, ColAType, lineH), "AType");
                x += ColAType + Gap;
                HeaderLabelRect("Cards", new Rect(x, y, ColACards, lineH), "AssetCards");
                x += ColACards + Gap;
                HeaderLabelRect("License", new Rect(x, y, ColLicense, lineH), "License");
                x += ColLicense + Gap;
                HeaderLabelRect("Source", new Rect(x, y, ColSource, lineH), "Source");
                x += ColSource + Gap;
                HeaderLabelRect("Copyright", new Rect(x, y, ColCopyright, lineH), "Copyright");
                x += ColCopyright + Gap;
                HeaderLabelRect("Path", new Rect(x, y, ColPath, lineH), "Path");
                x += ColPath + Gap;
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

            var fullItems = Filtered().ToList();
            int totalCount = fullItems.Count;
            int totalPages = Mathf.Max(1, Mathf.CeilToInt(totalCount / (float)PageSize));
            _pageIndex = Mathf.Clamp(_pageIndex, 0, Mathf.Max(0, totalPages - 1));
            var items = (totalCount > PageSize) ? fullItems.Skip(_pageIndex * PageSize).Take(PageSize).ToList() : fullItems;
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
                else if (SelectedType == typeof(TopicData) && obj is TopicData kd)
                {
                    DrawKnowledgeDataRow(rowRect, kd);
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
            var fullItems = FilteredWorldPrefabs().ToList();
            int totalCount = fullItems.Count;
            int totalPages = Mathf.Max(1, Mathf.CeilToInt(totalCount / (float)PageSize));
            _pageIndex = Mathf.Clamp(_pageIndex, 0, Mathf.Max(0, totalPages - 1));
            var items = (totalCount > PageSize) ? fullItems.Skip(_pageIndex * PageSize).Take(PageSize).ToList() : fullItems;
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
                x += ColQTitle + Gap;
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
                x += ColQTopics + Gap;
                xs.Add(x);
                x += ColQKnowledges + Gap;
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
                x += ColQDevStatus + Gap;
                xs.Add(x);
                x += ColId + Gap;
                xs.Add(x);
                x += ColTitle + Gap;
                xs.Add(x);
                x += ColImportance + Gap;
                xs.Add(x);
                x += ColCategory + Gap;
                xs.Add(x);
                x += ColTopics + Gap;
                xs.Add(x);
                x += ColCollectible + Gap;
                xs.Add(x);
                x += ColYear + Gap;
                xs.Add(x);
                x += ColPoints + Gap;
                xs.Add(x);
                x += ColCardKnowledges + Gap;
                xs.Add(x);
                x += ColCardWords + Gap;
                xs.Add(x);
                x += ColQuests + Gap;
                xs.Add(x);
                x += ColPath + Gap;
                xs.Add(x);
                return xs;
            }
            if (SelectedType == typeof(TopicData))
            {
                x += ColId + Gap;
                xs.Add(x);
                x += ColKName + Gap;
                xs.Add(x);
                x += ColKDesc + Gap;
                xs.Add(x);
                x += ColKImportance + Gap;
                xs.Add(x);
                x += ColKCoreCard + Gap;
                xs.Add(x);
                x += ColKConnections + Gap;
                xs.Add(x);
                x += ColKAge + Gap;
                xs.Add(x);
                x += ColKTopics + Gap;
                xs.Add(x);
                x += ColKQuests + Gap;
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
                xs.Add(x); // after Image
                x += ColQDevStatus + Gap;
                xs.Add(x); // after Status
                x += ColId + Gap;
                xs.Add(x); // after Id
                x += ColAType + Gap;
                xs.Add(x); // after Type
                x += ColACards + Gap;
                xs.Add(x); // after Cards
                x += ColLicense + Gap;
                xs.Add(x); // after License
                x += ColSource + Gap;
                xs.Add(x); // after Source
                x += ColCopyright + Gap;
                xs.Add(x); // after Copyright
                x += ColPath + Gap;
                xs.Add(x); // after Path
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
            // Image (second column) with preview cache
            var rImg = new Rect(x, rowRect.y + 2f, ColImage, ColImage);
            if (a.Image != null)
            {
                Texture tex;
                if (!_previewCache.TryGetValue(a.Image, out tex) || tex == null)
                {
                    tex = AssetPreview.GetAssetPreview(a.Image) ?? AssetPreview.GetMiniThumbnail(a.Image) ?? a.Image.texture;
                    _previewCache[a.Image] = tex;
                }
                if (tex != null)
                    GUI.DrawTexture(rImg, tex, ScaleMode.ScaleToFit);
                else
                    Repaint();
            }
            x += ColImage + Gap;
            // Status (readonly label)
            var rStat = new Rect(x, y, ColQDevStatus, lineH);
            EditorGUI.LabelField(rStat, a.Status.ToString());
            x += ColQDevStatus + Gap;
            // Id
            var rId = new Rect(x, y, ColId, lineH);
            EditorGUI.LabelField(rId, a.Id ?? string.Empty);
            x += ColId + Gap;
            // Type
            var rType = new Rect(x, y, ColAType, lineH);
            EditorGUI.LabelField(rType, a.Type.ToString());
            x += ColAType + Gap;
            // Cards using this AssetData (one per line, clickable links to CardData)
            var rCards = new Rect(x, rowRect.y + 4f, ColACards, rowRect.height - 8f);
            float cy = rCards.y;
            var cardsUsing = _allData.OfType<CardData>()
                .Where(cd => cd != null && (cd.ImageAsset == a || cd.AudioAsset == a))
                .OrderBy(cd => cd.Id ?? cd.name)
                .ToList();
            if (cardsUsing.Count > 0)
            {
                int total = cardsUsing.Count;
                int maxLines = Mathf.Max(1, Mathf.FloorToInt(rCards.height / lineH));
                int linesToShow = Mathf.Min(total, maxLines);
                cy = rCards.y + (rCards.height - linesToShow * lineH) * 0.5f;
                int shown = 0;
                foreach (var cd in cardsUsing)
                {
                    if (shown >= linesToShow)
                        break;
                    string label = string.IsNullOrEmpty(cd.Id) ? cd.name : cd.Id;
                    var rc = new Rect(rCards.x, cy, rCards.width, lineH);
                    GUI.Label(rc, label, EditorStyles.label);
                    EditorGUIUtility.AddCursorRect(rc, MouseCursor.Link);
                    if (Event.current.type == EventType.MouseDown && rc.Contains(Event.current.mousePosition))
                    {
                        EditorGUIUtility.PingObject(cd);
                        Selection.activeObject = cd;
                        // Also focus browser onto CardData and search this Id
                        try
                        {
                            int idx = _typeOptions.FindIndex(o => o.Type != null && o.Type.Name == nameof(CardData));
                            if (idx >= 0)
                            {
                                _selectedTypeIndex = idx;
                                _search = cd.Id ?? cd.name;
                                _sortKey = nameof(IdentifiedData.Id);
                                _sortAsc = true;
                                _scroll = Vector2.zero;
                                RefreshList();
                                Repaint();
                            }
                        }
                        catch { }
                        Event.current.Use();
                    }
                    cy += lineH;
                    shown++;
                }
            }
            x += ColACards + Gap;
            // License
            var rLic = new Rect(x, y, ColLicense, lineH);
            EditorGUI.BeginChangeCheck();
            var newLic = (LicenseType)EditorGUI.EnumPopup(rLic, a.License);
            if (EditorGUI.EndChangeCheck())
            { Undo.RecordObject(a, "Edit License"); a.License = newLic; EditorUtility.SetDirty(a); }
            x += ColLicense + Gap;
            // Source
            var rSrc = new Rect(x, y, ColSource, lineH);
            EditorGUI.BeginChangeCheck();
            string newSrc = EditorGUI.TextField(rSrc, a.SourceUrl ?? string.Empty);
            if (EditorGUI.EndChangeCheck())
            { Undo.RecordObject(a, "Edit SourceUrl"); a.SourceUrl = newSrc; EditorUtility.SetDirty(a); }
            x += ColSource + Gap;
            // Copyright
            var rCopy = new Rect(x, y, ColCopyright, lineH);
            EditorGUI.BeginChangeCheck();
            string newCopy = EditorGUI.TextField(rCopy, a.Copyright ?? string.Empty);
            if (EditorGUI.EndChangeCheck())
            { Undo.RecordObject(a, "Edit Copyright"); a.Copyright = newCopy; EditorUtility.SetDirty(a); }
            x += ColCopyright + Gap;
            // Path (last)
            var rPath = new Rect(x, y, ColPath, lineH);
            EditorGUI.LabelField(rPath, FormatPath(AssetDatabase.GetAssetPath(a)));
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
                // Click on preview to open/select the AssetData that owns this image
                if (c.ImageAsset != null)
                {
                    EditorGUIUtility.AddCursorRect(rImg, MouseCursor.Link);
                    if (Event.current.type == EventType.MouseDown && rImg.Contains(Event.current.mousePosition))
                    {
                        EditorGUIUtility.PingObject(c.ImageAsset);
                        Selection.activeObject = c.ImageAsset;
                        // Also focus the Game Data Browser on AssetData and search this Id
                        try
                        {
                            // find AssetData entry option index
                            int idx = _typeOptions.FindIndex(o => o.Type != null && o.Type.Name == nameof(AssetData));
                            if (idx >= 0)
                            {
                                _selectedTypeIndex = idx;
                                _search = c.ImageAsset.Id ?? c.ImageAsset.name;
                                _sortKey = nameof(IdentifiedData.Id);
                                _sortAsc = true;
                                _scroll = Vector2.zero;
                                RefreshList();
                                Repaint();
                            }
                        }
                        catch { }
                        Event.current.Use();
                    }
                }
            }
            x += ColImage + Gap;
            // Status
            var rStatus = new Rect(x, y, ColQDevStatus, lineH);
            EditorGUI.LabelField(rStatus, c.Status.ToString());
            x += ColQDevStatus + Gap;
            // Id
            var rId = new Rect(x, y, ColId, lineH);
            EditorGUI.LabelField(rId, c.Id ?? string.Empty);
            x += ColId + Gap;
            // Title
            var rTitle = new Rect(x, y, ColTitle, lineH);
            EditorGUI.LabelField(rTitle, GetCardTitle(c) ?? string.Empty);
            x += ColTitle + Gap;
            // Importance
            var rImp = new Rect(x, y, ColImportance, lineH);
            EditorGUI.LabelField(rImp, c.Importance.ToString());
            x += ColImportance + Gap;
            // Type
            var rCat = new Rect(x, y, ColCategory, lineH);
            EditorGUI.LabelField(rCat, c.Type.ToString());
            x += ColCategory + Gap;
            // Topics (one per line)
            string topicsText = c.Subjects != null ? string.Join("\n", c.Subjects.Select(t => t.ToString())) : string.Empty;
            var rTop = new Rect(x, rowRect.y + 4f, ColTopics, rowRect.height - 8f);
            GUI.Label(rTop, topicsText, EditorStyles.label);
            x += ColTopics + Gap;
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
            // Points (0..10)
            var rKv = new Rect(x, y, ColPoints, lineH);
            float kv = Mathf.Clamp(c.Points, 0, 10);
            EditorGUI.ProgressBar(rKv, kv / 10f, c.Points.ToString());
            x += ColPoints + Gap;
            // Knowledge: list all KnowledgeData where this card is CoreCard or in Connections
            var rKList = new Rect(x, rowRect.y + 4f, ColCardKnowledges, rowRect.height - 8f);
            float ky = rKList.y;
            var knows = _allData.OfType<TopicData>()
                .Where(kd => kd != null && ((kd.CoreCard == c) || (kd.Connections != null && kd.Connections.Any(conn => conn != null && conn.ConnectedCard == c))))
                .OrderBy(kd => kd.Id ?? kd.name)
                .ToList();
            if (knows.Count > 0)
            {
                int total = knows.Count;
                int maxLines = Mathf.Max(1, Mathf.FloorToInt(rKList.height / lineH));
                int linesToShow = Mathf.Min(total, maxLines);
                ky = rKList.y + (rKList.height - linesToShow * lineH) * 0.5f;
                int shown = 0;
                foreach (var kd in knows)
                {
                    if (shown >= linesToShow)
                        break;
                    string kLabel = string.IsNullOrEmpty(kd.Id) ? kd.name : kd.Id;
                    var rk = new Rect(rKList.x, ky, rKList.width, lineH);
                    GUI.Label(rk, kLabel, EditorStyles.label);
                    EditorGUIUtility.AddCursorRect(rk, MouseCursor.Link);
                    if (Event.current.type == EventType.MouseDown && rk.Contains(Event.current.mousePosition))
                    {
                        EditorGUIUtility.PingObject(kd);
                        Selection.activeObject = kd;
                        // Focus browser to KnowledgeData and search this Id
                        try
                        {
                            int kdIdx = _typeOptions.FindIndex(o => o.Type != null && o.Type.Name == nameof(TopicData));
                            if (kdIdx >= 0)
                            {
                                _selectedTypeIndex = kdIdx;
                                _search = kd.Id ?? kd.name;
                                _sortKey = nameof(IdentifiedData.Id);
                                _sortAsc = true;
                                _scroll = Vector2.zero;
                                RefreshList();
                                Repaint();
                            }
                        }
                        catch { }
                        Event.current.Use();
                    }
                    ky += lineH;
                    shown++;
                }
            }
            // leave empty when none
            x += ColCardKnowledges + Gap;
            // Words linked to this card (one per line)
            string words = string.Empty;
            if (c.Words != null && c.Words.Count > 0)
            { words = string.Join("\n", c.Words.Where(w => w != null).Select(w => string.IsNullOrEmpty(w.Id) ? w.name : w.Id)); }
            var rWords = new Rect(x, rowRect.y + 4f, ColCardWords, rowRect.height - 8f);
            GUI.Label(rWords, words, EditorStyles.label);
            x += ColCardWords + Gap;
            // Linked Quests (one per line, clickable)
            var rQuests = new Rect(x, rowRect.y + 4f, ColQuests, rowRect.height - 8f);
            float qy = rQuests.y;
            var questList = (c.Quests ?? new List<QuestData>()).Where(q => q != null).OrderBy(q => q.Id ?? q.name).ToList();
            if (questList.Count > 0)
            {
                int total = questList.Count;
                int maxLines = Mathf.Max(1, Mathf.FloorToInt(rQuests.height / lineH));
                int linesToShow = Mathf.Min(total, maxLines);
                qy = rQuests.y + (rQuests.height - linesToShow * lineH) * 0.5f;
                int shown = 0;
                foreach (var q in questList)
                {
                    if (shown >= linesToShow)
                        break;
                    string qLabel = string.IsNullOrEmpty(q.Id) ? q.name : q.Id;
                    var rq = new Rect(rQuests.x, qy, rQuests.width, lineH);
                    GUI.Label(rq, qLabel, EditorStyles.label);
                    EditorGUIUtility.AddCursorRect(rq, MouseCursor.Link);
                    if (Event.current.type == EventType.MouseDown && rq.Contains(Event.current.mousePosition))
                    {
                        EditorGUIUtility.PingObject(q);
                        Selection.activeObject = q;
                        // Focus browser to QuestData and search this Id
                        try
                        {
                            int qIdx = _typeOptions.FindIndex(o => o.Type != null && o.Type.Name == nameof(QuestData));
                            if (qIdx >= 0)
                            {
                                _selectedTypeIndex = qIdx;
                                _search = q.Id ?? q.name;
                                _sortKey = nameof(IdentifiedData.Id);
                                _sortAsc = true;
                                _scroll = Vector2.zero;
                                RefreshList();
                                Repaint();
                            }
                        }
                        catch { }
                        Event.current.Use();
                    }
                    qy += lineH;
                    shown++;
                }
            }
            // leave empty when none
            x += ColQuests + Gap;
            // Path (last column)
            var rPath = new Rect(x, y, ColPath, lineH);
            EditorGUI.LabelField(rPath, FormatPath(AssetDatabase.GetAssetPath(c)));
        }

        private void DrawKnowledgeDataRow(Rect rowRect, TopicData k)
        {
            float x = MarginLeft;
            float lineH = EditorGUIUtility.singleLineHeight;
            float y = rowRect.y + (rowRect.height - lineH) * 0.5f;
            // Open
            var rOpen = new Rect(x, rowRect.y + (rowRect.height - 20f) * 0.5f, ColOpen - 12f, 20f);
            if (GUI.Button(rOpen, "Open"))
            { EditorGUIUtility.PingObject(k); Selection.activeObject = k; }
            x += ColOpen + Gap;
            // Id
            var rId = new Rect(x, y, ColId, lineH);
            EditorGUI.LabelField(rId, k.Id ?? string.Empty);
            x += ColId + Gap;
            // Name
            var rName = new Rect(x, y, ColKName, lineH);
            EditorGUI.LabelField(rName, k.Name ?? string.Empty);
            x += ColKName + Gap;
            // Description (word wrapped within cell height)
            var rDesc = new Rect(x, rowRect.y + 4f, ColKDesc, rowRect.height - 8f);
            GUI.Label(rDesc, k.Description ?? string.Empty, EditorStyles.wordWrappedLabel);
            x += ColKDesc + Gap;
            // Importance
            var rImp = new Rect(x, y, ColKImportance, lineH);
            EditorGUI.LabelField(rImp, k.Importance.ToString());
            x += ColKImportance + Gap;
            // Core Card (clickable)
            var rCore = new Rect(x, y, ColKCoreCard, lineH);
            if (k.CoreCard != null)
            {
                string coreLabel = string.IsNullOrEmpty(k.CoreCard.Id) ? k.CoreCard.name : k.CoreCard.Id;
                GUI.Label(rCore, coreLabel, EditorStyles.label);
                EditorGUIUtility.AddCursorRect(rCore, MouseCursor.Link);
                if (Event.current.type == EventType.MouseDown && rCore.Contains(Event.current.mousePosition))
                {
                    EditorGUIUtility.PingObject(k.CoreCard);
                    Selection.activeObject = k.CoreCard;
                    // Also focus the Game Data Browser on CardData and search this Id
                    try
                    {
                        int cdIdx = _typeOptions.FindIndex(o => o.Type != null && o.Type.Name == nameof(CardData));
                        if (cdIdx >= 0)
                        {
                            _selectedTypeIndex = cdIdx;
                            _search = k.CoreCard.Id ?? k.CoreCard.name;
                            _sortKey = nameof(IdentifiedData.Id);
                            _sortAsc = true;
                            _scroll = Vector2.zero;
                            RefreshList();
                            Repaint();
                        }
                    }
                    catch { }
                    Event.current.Use();
                }
            }
            else
            {
                GUI.Label(rCore, "(None)", EditorStyles.miniLabel);
            }
            x += ColKCoreCard + Gap;
            // Connections: every connection per line, clickable to open the CardData, show "CardId (connectionType)"
            var rConnArea = new Rect(x, rowRect.y + 4f, ColKConnections, rowRect.height - 8f);
            float lineY = rConnArea.y;
            if (k.Connections != null && k.Connections.Count > 0)
            {
                int total = k.Connections.Count(cn => cn != null && cn.ConnectedCard != null);
                int maxLines = Mathf.Max(1, Mathf.FloorToInt(rConnArea.height / lineH));
                int linesToShow = Mathf.Min(total, maxLines);
                lineY = rConnArea.y + (rConnArea.height - linesToShow * lineH) * 0.5f;
                int shown = 0;
                foreach (var c in k.Connections)
                {
                    if (c == null || c.ConnectedCard == null)
                        continue;
                    string cardLabel = string.IsNullOrEmpty(c.ConnectedCard.Id) ? c.ConnectedCard.name : c.ConnectedCard.Id;
                    string text = cardLabel + " (" + c.ConnectionType.ToString() + ")";
                    var rc = new Rect(rConnArea.x, lineY, rConnArea.width, lineH);
                    GUI.Label(rc, text, EditorStyles.label);
                    EditorGUIUtility.AddCursorRect(rc, MouseCursor.Link);
                    if (Event.current.type == EventType.MouseDown && rc.Contains(Event.current.mousePosition))
                    {
                        EditorGUIUtility.PingObject(c.ConnectedCard);
                        Selection.activeObject = c.ConnectedCard;
                        Event.current.Use();
                    }
                    lineY += lineH;
                    shown++;
                    if (shown >= linesToShow)
                        break; // avoid drawing beyond cell; keeps UI responsive
                }
            }
            else
            {
                var rc = new Rect(rConnArea.x, lineY, rConnArea.width, lineH);
                GUI.Label(rc, "(No connections)", EditorStyles.miniLabel);
            }
            x += ColKConnections + Gap;
            // Target Age
            var rAge = new Rect(x, y, ColKAge, lineH);
            EditorGUI.LabelField(rAge, k.TargetAge.ToString());
            x += ColKAge + Gap;
            // Topics
            var tops = k.Subjects != null ? string.Join(", ", k.Subjects.Select(t => t.ToString())) : string.Empty;
            var rTop = new Rect(x, y, ColKTopics, lineH);
            EditorGUI.LabelField(rTop, tops);
            x += ColKTopics + Gap;
            // Quests referencing this Knowledge (one per line, clickable, and focus browser to QuestData)
            var rQ = new Rect(x, rowRect.y + 4f, ColKQuests, rowRect.height - 8f);
            float qy = rQ.y;
            // gather quests having this knowledge in their Knowledges list
            var quests = _allData.OfType<QuestData>()
                .Where(qd => qd != null && qd.Topics != null && qd.Topics.Contains(k))
                .OrderBy(qd => qd.Id ?? qd.name)
                .ToList();
            if (quests.Count > 0)
            {
                int total = quests.Count;
                int maxLines = Mathf.Max(1, Mathf.FloorToInt(rQ.height / lineH));
                int linesToShow = Mathf.Min(total, maxLines);
                qy = rQ.y + (rQ.height - linesToShow * lineH) * 0.5f;
                int shown = 0;
                foreach (var qd in quests)
                {
                    if (shown >= linesToShow)
                        break;
                    string qLabel = string.IsNullOrEmpty(qd.Id) ? qd.name : qd.Id;
                    var rq = new Rect(rQ.x, qy, rQ.width, lineH);
                    GUI.Label(rq, qLabel, EditorStyles.label);
                    EditorGUIUtility.AddCursorRect(rq, MouseCursor.Link);
                    if (Event.current.type == EventType.MouseDown && rq.Contains(Event.current.mousePosition))
                    {
                        EditorGUIUtility.PingObject(qd);
                        Selection.activeObject = qd;
                        // Also focus the Game Data Browser on QuestData and search this Id
                        try
                        {
                            int qIdx = _typeOptions.FindIndex(o => o.Type != null && o.Type.Name == nameof(QuestData));
                            if (qIdx >= 0)
                            {
                                _selectedTypeIndex = qIdx;
                                _search = qd.Id ?? qd.name;
                                _sortKey = nameof(IdentifiedData.Id);
                                _sortAsc = true;
                                _scroll = Vector2.zero;
                                RefreshList();
                                Repaint();
                            }
                        }
                        catch { }
                        Event.current.Use();
                    }
                    qy += lineH;
                    shown++;
                }
            }
            else
            {
                GUI.Label(new Rect(rQ.x, qy, rQ.width, lineH), "(No quests)", EditorStyles.miniLabel);
            }
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
            EditorGUI.LabelField(rDev, q.Status.ToString());
            x += ColQDevStatus + Gap;
            // TitleEn
            var rTitle = new Rect(x, y, ColQTitle, lineH);
            EditorGUI.LabelField(rTitle, q.TitleEn ?? string.Empty);
            x += ColQTitle + Gap;
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
            // Category
            var rTopic = new Rect(x, y, ColTopic, lineH);
            EditorGUI.LabelField(rTopic, q.Subject.ToString());
            x += ColTopic + Gap;
            // Topics aggregated from cards (one per line)
            string topicsText = string.Empty;
            if (q.Cards != null && q.Cards.Count > 0)
            {
                var allTopics = new List<string>();
                foreach (var cc in q.Cards)
                {
                    if (cc != null && cc.Subjects != null)
                        allTopics.AddRange(cc.Subjects.Select(t => t.ToString()));
                }
                var uniq = allTopics.Where(s => !string.IsNullOrEmpty(s)).Distinct().OrderBy(s => s).ToList();
                if (uniq.Count > 0)
                {
                    topicsText = string.Join("\n", uniq);
                }
            }
            var rTopics = new Rect(x, rowRect.y + 4f, ColQTopics, rowRect.height - 8f);
            GUI.Label(rTopics, topicsText, EditorStyles.label);
            x += ColQTopics + Gap;
            // Knowledges (one per line, clickable to navigate to KnowledgeData)
            var rKnows = new Rect(x, rowRect.y + 4f, ColQKnowledges, rowRect.height - 8f);
            float ky = rKnows.y;
            if (q.Topics != null && q.Topics.Count > 0)
            {
                int total = q.Topics.Count(kd => kd != null);
                int maxLines = Mathf.Max(1, Mathf.FloorToInt(rKnows.height / lineH));
                int linesToShow = Mathf.Min(total, maxLines);
                ky = rKnows.y + (rKnows.height - linesToShow * lineH) * 0.5f;
                int shown = 0;
                foreach (var k in q.Topics)
                {
                    if (k == null)
                        continue;
                    if (shown >= linesToShow)
                        break;
                    string label = string.IsNullOrEmpty(k.Id) ? k.name : k.Id;
                    var rk = new Rect(rKnows.x, ky, rKnows.width, lineH);
                    GUI.Label(rk, label, EditorStyles.label);
                    EditorGUIUtility.AddCursorRect(rk, MouseCursor.Link);
                    if (Event.current.type == EventType.MouseDown && rk.Contains(Event.current.mousePosition))
                    {
                        EditorGUIUtility.PingObject(k);
                        Selection.activeObject = k;
                        // Also focus the Game Data Browser on KnowledgeData and search this Id
                        try
                        {
                            int idx = _typeOptions.FindIndex(o => o.Type != null && o.Type.Name == nameof(TopicData));
                            if (idx >= 0)
                            {
                                _selectedTypeIndex = idx;
                                _search = k.Id ?? k.name;
                                _sortKey = nameof(IdentifiedData.Id);
                                _sortAsc = true;
                                _scroll = Vector2.zero;
                                RefreshList();
                                Repaint();
                            }
                        }
                        catch { }
                        Event.current.Use();
                    }
                    ky += lineH;
                    shown++;
                }
            }
            // leave empty when none
            x += ColQKnowledges + Gap;
            // Linked Cards
            var rCards = new Rect(x, rowRect.y + 4f, ColCards, rowRect.height - 8f);
            float cy = rCards.y;
            if (q.Cards != null && q.Cards.Count > 0)
            {
                foreach (var c in q.Cards)
                {
                    if (c == null)
                        continue;
                    string cardLabel = string.IsNullOrEmpty(c.Id) ? c.name : c.Id;
                    var rc = new Rect(rCards.x, cy, rCards.width, lineH);
                    GUI.Label(rc, cardLabel, EditorStyles.label);
                    EditorGUIUtility.AddCursorRect(rc, MouseCursor.Link);
                    if (Event.current.type == EventType.MouseDown && rc.Contains(Event.current.mousePosition))
                    {
                        EditorGUIUtility.PingObject(c);
                        Selection.activeObject = c;
                        // Focus the browser onto CardData and search this Id
                        try
                        {
                            int cdIdx = _typeOptions.FindIndex(o => o.Type != null && o.Type.Name == nameof(CardData));
                            if (cdIdx >= 0)
                            {
                                _selectedTypeIndex = cdIdx;
                                _search = c.Id ?? c.name;
                                _sortKey = nameof(IdentifiedData.Id);
                                _sortAsc = true;
                                _scroll = Vector2.zero;
                                RefreshList();
                                Repaint();
                            }
                        }
                        catch { }
                        Event.current.Use();
                    }
                    cy += lineH;
                    if (cy > rCards.yMax - lineH)
                        break;
                }
            }
            // leave empty when none
            x += ColCards + Gap;
            // Words Used (one per line, clickable and navigates to WordData)
            var rWords = new Rect(x, rowRect.y + 4f, ColWords, rowRect.height - 8f);
            float wy = rWords.y;
            if (q.Words != null && q.Words.Count > 0)
            {
                int total = q.Words.Count(ww => ww != null);
                int maxLines = Mathf.Max(1, Mathf.FloorToInt(rWords.height / lineH));
                int linesToShow = Mathf.Min(total, maxLines);
                wy = rWords.y + (rWords.height - linesToShow * lineH) * 0.5f;
                int shown = 0;
                foreach (var w in q.Words)
                {
                    if (w == null)
                        continue;
                    if (shown >= linesToShow)
                        break;
                    string wLabel = string.IsNullOrEmpty(w.Id) ? w.name : w.Id;
                    var rw = new Rect(rWords.x, wy, rWords.width, lineH);
                    GUI.Label(rw, wLabel, EditorStyles.label);
                    EditorGUIUtility.AddCursorRect(rw, MouseCursor.Link);
                    if (Event.current.type == EventType.MouseDown && rw.Contains(Event.current.mousePosition))
                    {
                        EditorGUIUtility.PingObject(w);
                        Selection.activeObject = w;
                        // Focus the browser onto WordData and search this Id
                        try
                        {
                            int wIdx = _typeOptions.FindIndex(o => o.Type != null && o.Type.Name == nameof(WordData));
                            if (wIdx >= 0)
                            {
                                _selectedTypeIndex = wIdx;
                                _search = w.Id ?? w.name;
                                _sortKey = nameof(IdentifiedData.Id);
                                _sortAsc = true;
                                _scroll = Vector2.zero;
                                RefreshList();
                                Repaint();
                            }
                        }
                        catch { }
                        Event.current.Use();
                    }
                    wy += lineH;
                    shown++;
                }
            }
            // leave empty when none
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
            // CardData Status filter
            if (SelectedType == typeof(CardData) && _cardStatusFilter.HasValue)
            {
                var s = _cardStatusFilter.Value;
                set = set.Where(a => a is CardData c && c.Status == s);
            }
            // CardData Type filter
            if (SelectedType == typeof(CardData) && _cardTypeFilter.HasValue)
            {
                var t = _cardTypeFilter.Value;
                set = set.Where(a => a is CardData c && c.Type == t);
            }
            // CardData Topics filter
            if (SelectedType == typeof(CardData) && _cardTopicFilter.HasValue)
            {
                var tp = _cardTopicFilter.Value;
                set = set.Where(a => a is CardData c && c.Subjects != null && c.Subjects.Contains(tp));
            }
            // CardData Importance filter
            if (SelectedType == typeof(CardData) && _cardImportanceFilter.HasValue)
            {
                var imp = _cardImportanceFilter.Value;
                set = set.Where(a => a is CardData c && c.Importance == imp);
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
                set = set.Where(a => a is QuestData q && q.Status == status);
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
                    case "Status":
                        keySelector = a => a is CardData cs ? cs.Status.ToString() : (a is AssetData asd ? asd.Status.ToString() : (a is QuestData qs ? qs.Status.ToString() : string.Empty));
                        break;
                    case "Name":
                        keySelector = a => a is TopicData k0 ? (k0.Name ?? string.Empty) : string.Empty;
                        break;
                    case "Description":
                        keySelector = a => a is TopicData k1 ? (k1.Description ?? string.Empty) : string.Empty;
                        break;
                    case "Active":
                        keySelector = a => a is WordData w0 ? (w0.Active ? "1" : "0") : string.Empty;
                        break;
                    case "TitleEn":
                        keySelector = a => a is WordData w1 ? (w1.TextEn ?? string.Empty) : (a is QuestData qT ? (qT.TitleEn ?? string.Empty) : string.Empty);
                        break;
                    case "DrawingUnicode":
                        keySelector = a => a is WordData w2 ? (w2.DrawingUnicode ?? string.Empty) : string.Empty;
                        break;
                    case "Topics":
                        keySelector = a =>
                            a is CardData c2 ? string.Join(",", c2.Subjects?.Select(t => t.ToString()) ?? Array.Empty<string>()) :
                            (a is TopicData k2 ? string.Join(",", k2.Subjects?.Select(t => t.ToString()) ?? Array.Empty<string>()) :
                            (a is QuestData qTop ? string.Join(",", (qTop.Cards ?? new List<CardData>())
                                .Where(c => c != null && c.Subjects != null)
                                .SelectMany(c => c.Subjects.Select(t => t.ToString()))
                                .Distinct()
                                .OrderBy(s => s)) : string.Empty));
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
                    case "Importance":
                        keySelector = a => a is CardData c7 ? c7.Importance.ToString() : (a is TopicData k3 ? k3.Importance.ToString() : string.Empty);
                        break;
                    case "CoreCard":
                        keySelector = a => a is TopicData kcc ? (string.IsNullOrEmpty(kcc.CoreCard?.Id) ? (kcc.CoreCard?.name ?? string.Empty) : kcc.CoreCard.Id) : string.Empty;
                        break;
                    case "KnowledgeValue":
                        keySelector = a => a is CardData c5 ? c5.Points.ToString("D2") : (a is QuestData q0 ? (q0.Cards != null ? q0.Cards.Where(cc => cc != null).Sum(cc => Mathf.Clamp(cc.Points, 0, 10)).ToString("D3") : "") : string.Empty);
                        break;
                    case "MainTopic":
                        keySelector = a => a is QuestData q1 ? q1.Subject.ToString() : string.Empty;
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
                    case "IdConcat":
                        keySelector = a => a is QuestData qI ? ((qI.Id ?? string.Empty) + "  -  " + (qI.IdDisplay ?? string.Empty)) : string.Empty;
                        break;
                    case "LinkedCards":
                        keySelector = a => a is QuestData q5 ? string.Join(";", q5.Cards?.Where(c => c != null).Select(c => string.IsNullOrEmpty(c.Id) ? c.name : c.Id) ?? Array.Empty<string>()) : string.Empty;
                        break;
                    case "Knowledges":
                        keySelector = a => a is QuestData qK ? string.Join(";", qK.Topics?.Where(k => k != null).Select(k => string.IsNullOrEmpty(k.Id) ? k.name : k.Id) ?? Array.Empty<string>()) : string.Empty;
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
                    case "Source":
                        keySelector = a => a is AssetData ad3 ? (ad3.SourceUrl ?? string.Empty) : string.Empty;
                        break;
                    case "AssetCards":
                        keySelector = a => a is AssetData ad4 ? (_allData.OfType<CardData>().Count(cd => cd != null && (cd.ImageAsset == ad4 || cd.AudioAsset == ad4))).ToString("D4") : string.Empty;
                        break;
                    case "AType":
                        keySelector = a => a is AssetData at ? at.Type.ToString() : string.Empty;
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
                if (cd.Subjects != null && cd.Subjects.Any(t => t.ToString().IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0))
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
                if (qd.Subject.ToString().IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
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
            // First: All, International, France, Poland
            list.Add(new CountryOption { Label = "All", Value = CountryFilter.All, IsSeparator = false });
            list.Add(new CountryOption { Label = "International", Value = CountryFilter.Global, IsSeparator = false });
            list.Add(new CountryOption { Label = "France", Value = CountryFilter.France, IsSeparator = false });
            list.Add(new CountryOption { Label = "Poland", Value = CountryFilter.Poland, IsSeparator = false });
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

        private void DrawFooter()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                // Items count
                int count = 0;
                try
                {
                    count = (SelectedType == typeof(WorldPrefabData)) ? FilteredWorldPrefabs().Count() : Filtered().Count();
                }
                catch { }
                GUILayout.Label($"{count} items", EditorStyles.miniLabel, GUILayout.Width(100));

                // Pagination (only when exceeding page size)
                if (count > PageSize)
                {
                    int totalPages = Mathf.Max(1, Mathf.CeilToInt(count / (float)PageSize));
                    _pageIndex = Mathf.Clamp(_pageIndex, 0, totalPages - 1);

                    using (new EditorGUI.DisabledScope(_pageIndex <= 0))
                    {
                        if (GUILayout.Button("◀ Prev", EditorStyles.toolbarButton, GUILayout.Width(70)))
                        { _pageIndex = Mathf.Max(0, _pageIndex - 1); Repaint(); }
                    }
                    GUILayout.Label($"Page {_pageIndex + 1} / {totalPages}", EditorStyles.miniLabel, GUILayout.Width(120));
                    using (new EditorGUI.DisabledScope(_pageIndex >= totalPages - 1))
                    {
                        if (GUILayout.Button("Next ▶", EditorStyles.toolbarButton, GUILayout.Width(70)))
                        { _pageIndex = Mathf.Min(totalPages - 1, _pageIndex + 1); Repaint(); }
                    }
                }

                // Left aligned: Refresh + Export CSV
                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(70)))
                {
                    RefreshTypes();
                    RefreshList();
                }
                if (GUILayout.Button("Export CSV", EditorStyles.toolbarButton, GUILayout.Width(90)))
                {
                    ExportCsv();
                }
                GUILayout.FlexibleSpace();
            }
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
            return c.TitleEn;
            // if (_cardTitleCache.TryGetValue(c, out var cached) && !string.IsNullOrEmpty(cached))
            //     return cached;
            // try
            // {
            //     var s = c.Title.GetLocalizedString();
            //     if (!string.IsNullOrEmpty(s))
            //     { _cardTitleCache[c] = s; return s; }
            // }
            // catch { }
            // return null;
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
                // Delegate to centralized CardDataExchangeUtility for AI-centric CSV
                try
                {
                    var cards = list.Cast<CardData>();
                    var csv = CardDataExchangeUtility.BuildCardsCsv(cards);
                    sb.Append(csv);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[GameDataBrowser] Failed to build CardData CSV via exchange utility: {ex.Message}");
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
                        Escape(q.Status.ToString()) + "," +
                        Escape(q.Id) + "," +
                        Escape(q.IdDisplay) + "," +
                        Escape(locId) + "," +
                        Escape(q.KnowledgeValue.ToString()) + "," +
                        Escape(q.Difficulty.ToString()) + "," +
                        Escape(q.Subject.ToString()) + "," +
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
