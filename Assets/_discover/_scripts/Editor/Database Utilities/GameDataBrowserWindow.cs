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
        // Sorting
        private string _sortKey = null;
        private bool _sortAsc = true;
        // Search field with lens icon
        private SearchField _searchField;

        // Layout sizes
        private const float ColOpen = 60f;
        private const float ColId = 220f;
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

        // CardData specific
        private const float ColTitle = 220f;
        private const float ColCollectible = 90f;
        private const float ColYear = 60f;
        private const float ColCategory = 140f;
        private const float ColTopics = 220f;
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
        private const float ColExport = 110f;
        private const float Gap = 6f;
        private const float MarginLeft = 4f;
        private readonly Color _gridLineColor = new Color(0, 0, 0, 0.12f);

        private float GetTableWidth()
        {
            float width = MarginLeft + ColOpen + Gap;
            bool hasImage = SelectedType == typeof(QuestData) || SelectedType == typeof(CardData) || SelectedType == typeof(ItemData) || SelectedType == typeof(AssetData);
            if (hasImage)
                width += ColImage + Gap;

            if (SelectedType == typeof(QuestData))
            {
                // DevStatus, Id(+IdDisplay), Location, Knowledge, Difficulty, MainTopic, LinkedCards, WordsUsed
                width += ColQDevStatus + Gap + ColQId + Gap + ColQLocation + Gap + ColQKnowledge + Gap + ColDifficulty + Gap + ColTopic + Gap + ColCards + Gap + ColWords + Gap + ColExport;
            }
            else if (SelectedType == typeof(WordData))
            {
                // Active, Title (TextEn), Category, Drawing (preview), Drawing Unicode
                width += ColWActive + Gap + ColWTitle + Gap + ColWCategory + Gap + ColWImage + Gap + ColWUnicode;
            }
            else if (SelectedType == typeof(CardData))
            {
                // Id, Collectible(+Icon), Year, Knowledge, Category, Topics, Linked Quests, Path
                width += ColId + Gap + ColCollectible + Gap + ColYear + Gap + ColKnowledge + Gap + ColCategory + Gap + ColTopics + Gap + ColQuests + Gap + ColPath;
            }
            else if (SelectedType == typeof(ItemData))
            {
                width += ColId + Gap + ColPath + Gap + ColTag;
            }
            else if (SelectedType == typeof(AssetData))
            {
                width += ColId + Gap + ColPath + Gap + ColLicense + Gap + ColCopyright;
            }
            else
            {
                width += ColId + Gap + ColPath;
            }
            return width + Gap; // ending gap
        }

        private List<float> GetColumnBoundariesX()
        {
            var xs = new List<float>();
            float x = MarginLeft;
            // after Open
            x += ColOpen + Gap;
            xs.Add(x);
            bool hasImage = SelectedType == typeof(QuestData) || SelectedType == typeof(CardData) || SelectedType == typeof(ItemData) || SelectedType == typeof(AssetData);
            if (hasImage)
            {
                x += ColImage + Gap;
                xs.Add(x);
            }

            if (SelectedType == typeof(QuestData))
            {
                // DevStatus
                x += ColQDevStatus + Gap;
                xs.Add(x);
                // Id (+ IdDisplay)
                x += ColQId + Gap;
                xs.Add(x);
                // Location
                x += ColQLocation + Gap;
                xs.Add(x);
                // Knowledge
                x += ColQKnowledge + Gap;
                xs.Add(x);
                // Difficulty
                x += ColDifficulty + Gap;
                xs.Add(x);
                // Main Topic
                x += ColTopic + Gap;
                xs.Add(x);
                // Linked Cards
                x += ColCards + Gap;
                xs.Add(x);
                // Words Used (end)
                x += ColWords + Gap;
                xs.Add(x);
                // Export
                x += ColExport + Gap;
                xs.Add(x);
                return xs;
            }
            if (SelectedType == typeof(CardData))
            {
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
                x += ColQuests + Gap;
                xs.Add(x);
                x += ColPath + Gap;
                xs.Add(x);
                return xs;
            }
            if (SelectedType == typeof(WordData))
            {
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
            if (SelectedType == typeof(ItemData))
            {
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
            // Generic
            x += ColId + Gap;
            xs.Add(x);
            x += ColPath + Gap;
            xs.Add(x);
            return xs;
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

        [MenuItem("Antura/Discover/Game Data Browser", priority = 20)]
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

                // Preferred order: QuestData, CardData, AssetData
                var topTypes = new List<Type>();
                var questType = concreteTypes.FirstOrDefault(t => t.Name == nameof(QuestData));
                var cardType = concreteTypes.FirstOrDefault(t => t.Name == nameof(CardData));
                var assetType = concreteTypes.FirstOrDefault(t => t.Name == nameof(AssetData));
                var wordType = concreteTypes.FirstOrDefault(t => t.Name == nameof(WordData));
                if (questType != null)
                    topTypes.Add(questType);
                if (cardType != null)
                    topTypes.Add(cardType);
                if (assetType != null)
                    topTypes.Add(assetType);
                if (wordType != null)
                    topTypes.Add(wordType);

                foreach (var t in topTypes)
                    _typeOptions.Add(new TypeOption { Label = NicifyTypeLabel(t), Type = t, IsAggregate = false, IsSeparator = false });

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
                // Left: Data selector
                GUILayout.Label("Data:", GUILayout.Width(40));
                var labels = _typeOptions.Select(o => o.Label).ToArray();
                var selIndex = Mathf.Clamp(_selectedTypeIndex, 0, Mathf.Max(0, _typeOptions.Count - 1));
                var newIndex = EditorGUILayout.Popup(selIndex, labels, EditorStyles.toolbarPopup, GUILayout.MinWidth(200));
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
                // Country selector (custom order with separator; no-op for types without Country)
                GUILayout.Label("Country:", GUILayout.Width(60));
                var cOpts = GetCountryOptions();
                var cLabels = cOpts.Select(o => o.Label).ToArray();
                int cIndex = Mathf.Max(0, cOpts.FindIndex(o => !o.IsSeparator && o.Value.Equals(_countryFilter)));
                int newCIndex = EditorGUILayout.Popup(cIndex, cLabels, EditorStyles.toolbarPopup, GUILayout.Width(200));
                if (newCIndex != cIndex)
                {
                    if (!cOpts[newCIndex].IsSeparator)
                    {
                        _countryFilter = cOpts[newCIndex].Value;
                        Repaint();
                    }
                }

                GUILayout.Space(12);
                if (_searchField == null)
                    _searchField = new SearchField();
                var newSearch = _searchField.OnToolbarGUI(_search, GUILayout.MinWidth(220));
                if (!string.Equals(newSearch, _search, StringComparison.Ordinal))
                { _search = newSearch; Repaint(); }

                GUILayout.FlexibleSpace();
                // Show count of visible items
                try
                {
                    int count = Filtered().Count();
                    GUILayout.Label($"{count} items", EditorStyles.miniLabel);
                }
                catch { }
                if (GUILayout.Button("Export CSV", EditorStyles.toolbarButton))
                {
                    ExportCsv();
                }
                if (SelectedType == typeof(QuestData))
                {
                    if (GUILayout.Button("Export Files", EditorStyles.toolbarButton))
                    {
                        ExportFiles();
                    }
                }
                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
                {
                    RefreshList();
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
                GUI.Label(new Rect(x, y, ColExport, lineH), "Export", EditorStyles.boldLabel);
                x += ColExport + Gap;
            }
            else if (SelectedType == typeof(WordData))
            {
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
            var items = Filtered().ToList();
            if (items.Count == 0)
            {
                GUILayout.Space(8);
                EditorGUILayout.HelpBox("No data found.", MessageType.Info);
                return;
            }

            float tableWidth = GetTableWidth();
            var outer = GUILayoutUtility.GetRect(position.width, position.height - 160, GUILayout.ExpandHeight(true));
            // compute dynamic total height (Quest rows can grow based on topics)
            float totalHeight = 0f;
            var rowHeights = new List<float>(items.Count);
            foreach (var obj in items)
            {
                float h = RowHeight;
                if (SelectedType == typeof(QuestData) && obj is QuestData qh)
                {
                    // estimate extra lines from topics
                    int n = 0;
                    if (qh.Cards != null)
                    {
                        var set = new HashSet<string>();
                        foreach (var cc in qh.Cards)
                            if (cc != null && cc.Topics != null)
                                foreach (var t in cc.Topics)
                                    set.Add(t.ToString());
                        n = set.Count;
                    }
                    if (n > 0)
                        h = Mathf.Max(RowHeight, RowHeight + (n - 1) * (EditorGUIUtility.singleLineHeight + 2f));
                }
                rowHeights.Add(h);
                totalHeight += h;
            }
            var inner = new Rect(0, 0, tableWidth, totalHeight);
            _scroll = GUI.BeginScrollView(outer, _scroll, inner, true, true);
            int index = 0;
            float yCursor = 0f;
            foreach (var obj in items)
            {
                float rowH = rowHeights[index];
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
            if (sprite == null)
                sprite = c.Image;
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
            float kv = Mathf.Clamp(c.KnowledgeValue, 0, 10);
            EditorGUI.ProgressBar(rKv, kv / 10f, c.KnowledgeValue.ToString());
            x += ColKnowledge + Gap;
            // Category
            var rCat = new Rect(x, y, ColCategory, lineH);
            EditorGUI.LabelField(rCat, c.Category.ToString());
            x += ColCategory + Gap;
            // Topics
            var topics = c.Topics != null ? string.Join(", ", c.Topics.Select(t => t.ToString())) : string.Empty;
            var rTop = new Rect(x, y, ColTopics, lineH);
            EditorGUI.LabelField(rTop, topics);
            x += ColTopics + Gap;
            // Linked Quests
            string quests = string.Empty;
            if (c.LinkedQuests != null && c.LinkedQuests.Count > 0)
            { quests = string.Join(", ", c.LinkedQuests.Where(q => q != null).Select(q => string.IsNullOrEmpty(q.Id) ? q.name : q.Id)); }
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
                var tex = AssetPreview.GetAssetPreview(q.Thumbnail) ?? AssetPreview.GetMiniThumbnail(q.Thumbnail) ?? q.Thumbnail.texture;
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
                        sumKv += Mathf.Clamp(cc.KnowledgeValue, 0, 10);
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
            var words = q.WordsUsed != null ? string.Join(", ", q.WordsUsed.Where(w => w != null).Select(w => string.IsNullOrEmpty(w.Id) ? w.name : w.Id)) : string.Empty;
            var rWords = new Rect(x, y, ColWords, lineH);
            EditorGUI.LabelField(rWords, words);
            x += ColWords + Gap;
            // Export button (last column)
            var rExport = new Rect(x, rowRect.y + (rowRect.height - 20f) * 0.5f, ColExport - 12f, 20f);
            if (GUI.Button(rExport, "Export"))
            {
                ExportSingleQuest(q);
            }
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

            // Active
            var rActive = new Rect(x + 8f, y, ColWActive - 16f, lineH);
            EditorGUI.BeginChangeCheck();
            bool newActive = EditorGUI.Toggle(rActive, w.Active);
            if (EditorGUI.EndChangeCheck())
            { Undo.RecordObject(w, "Toggle Active"); w.Active = newActive; EditorUtility.SetDirty(w); }
            x += ColWActive + Gap;

            // Title (TextEn)
            var rTitle = new Rect(x, y, ColWTitle, lineH);
            EditorGUI.BeginChangeCheck();
            string newTitle = EditorGUI.TextField(rTitle, w.TextEn ?? string.Empty);
            if (EditorGUI.EndChangeCheck())
            { Undo.RecordObject(w, "Edit Title"); w.TextEn = newTitle; EditorUtility.SetDirty(w); }
            x += ColWTitle + Gap;

            // Category (use SerializedObject to avoid hard dependency on enum type)
            var rCat = new Rect(x, y, ColWCategory, lineH);
            var so = new SerializedObject(w);
            var spCat = so.FindProperty("Category");
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(rCat, spCat, GUIContent.none);
            if (EditorGUI.EndChangeCheck())
            { so.ApplyModifiedProperties(); EditorUtility.SetDirty(w); }
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

            // Drawing Unicode
            var rU = new Rect(x, y, ColWUnicode, lineH);
            EditorGUI.BeginChangeCheck();
            string newU = EditorGUI.TextField(rU, w.DrawingUnicode ?? string.Empty);
            if (EditorGUI.EndChangeCheck())
            { Undo.RecordObject(w, "Edit Drawing Unicode"); w.DrawingUnicode = newU; EditorUtility.SetDirty(w); }
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
                        keySelector = a => a is WordData w3 ? w3.Category.ToString() : (a is CardData c1 ? c1.Category.ToString() : string.Empty);
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
                    case "Collectible":
                        keySelector = a => a is CardData c3 ? (c3.IsCollectible ? "1" : "0") : string.Empty;
                        break;
                    case "Year":
                        keySelector = a => a is CardData c4 ? c4.Year.ToString("D4") : string.Empty;
                        break;
                    case "KnowledgeValue":
                        keySelector = a => a is CardData c5 ? c5.KnowledgeValue.ToString("D2") : (a is QuestData q0 ? (q0.Cards != null ? q0.Cards.Where(cc => cc != null).Sum(cc => Mathf.Clamp(cc.KnowledgeValue, 0, 10)).ToString("D3") : "") : string.Empty);
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
                        keySelector = a => a is QuestData q6 ? string.Join(";", q6.WordsUsed?.Where(w => w != null).Select(w => string.IsNullOrEmpty(w.Id) ? w.name : w.Id) ?? Array.Empty<string>()) : string.Empty;
                        break;
                    case "LinkedQuests":
                        keySelector = a => a is CardData c6 ? string.Join(";", c6.LinkedQuests?.Where(q => q != null).Select(q => string.IsNullOrEmpty(q.Id) ? q.name : q.Id) ?? Array.Empty<string>()) : string.Empty;
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
                if (cd.Category.ToString().IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
                if (cd.Year.ToString().IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
                if (cd.Topics != null && cd.Topics.Any(t => t.ToString().IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0))
                    return true;
                if (cd.LinkedQuests != null && cd.LinkedQuests.Any(q => q != null && ((string.IsNullOrEmpty(q.Id) ? q.name : q.Id).IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)))
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
                if (qd.WordsUsed != null && qd.WordsUsed.Any(w => w != null && ((string.IsNullOrEmpty(w.Id) ? w.name : w.Id).IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)))
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
                        return value == Countries.Global;
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
                    var quests = c.LinkedQuests != null ? string.Join("; ", c.LinkedQuests.Where(q => q != null).Select(q => string.IsNullOrEmpty(q.Id) ? q.name : q.Id)) : string.Empty;
                    sb.AppendLine(Escape("CardData") + "," + Escape(c.Id) + "," + Escape(FormatPath(AssetDatabase.GetAssetPath(c))) + "," + Escape(GetCardTitle(c)) + "," + Escape(c.IsCollectible ? "true" : "false") + "," + Escape(c.Year.ToString()) + "," + Escape(c.Category.ToString()) + "," + Escape(topics) + "," + Escape(quests));
                }
            }
            else if (SelectedType == typeof(QuestData))
            {
                sb.AppendLine("Type,DevStatus,Id,IdDisplay,LocationId,Knowledge,Difficulty,MainTopic,LinkedCards,WordsUsed");
                foreach (var q in list.Cast<QuestData>())
                {
                    string locId = q.Location != null ? (!string.IsNullOrEmpty(q.Location.Id) ? q.Location.Id : q.Location.name) : string.Empty;
                    string linkedCards = q.Cards != null ? string.Join("; ", q.Cards.Where(c => c != null).Select(c => string.IsNullOrEmpty(c.Id) ? c.name : c.Id)) : string.Empty;
                    string words = q.WordsUsed != null ? string.Join("; ", q.WordsUsed.Where(w => w != null).Select(w => string.IsNullOrEmpty(w.Id) ? w.name : w.Id)) : string.Empty;
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

        private static string GetQuestExportFileName(QuestData q)
        {
            string code = string.IsNullOrEmpty(q.Id) ? q.name : q.Id;
            foreach (var ch in System.IO.Path.GetInvalidFileNameChars())
                code = code.Replace(ch, '-');
            var dateStr = DateTime.Now.ToString("yyyy-MM-dd");
            return $"Antura Quest - {code} - {dateStr}.md";
        }

        private static System.Text.StringBuilder BuildQuestMarkdown(QuestData q)
        {
            var sb = new System.Text.StringBuilder();
            // YAML front matter with title
            string title = !string.IsNullOrEmpty(q.TitleText) ? q.TitleText : (!string.IsNullOrEmpty(q.Id) ? q.Id : q.name);
            sb.AppendLine("---");
            sb.AppendLine("title: Antura Discover Quest - " + title);
            sb.AppendLine("---");
            sb.AppendLine();
            // Heading per request
            sb.AppendLine("# Antura Discover Quest - " + title);
            // Second line: current date
            sb.AppendLine("Date: " + DateTime.Now.ToString("yyyy-MM-dd"));
            sb.AppendLine("Status: " + q.DevStatus);
            sb.AppendLine();

            // Description
            string desc = null;
            try
            { desc = q.Description.GetLocalizedString(); }
            catch { }
            if (!string.IsNullOrEmpty(desc))
            {
                sb.AppendLine(desc);
                sb.AppendLine();
            }

            // Data section
            sb.AppendLine("## Informations");
            sb.AppendLine();
            // Title and Description first
            sb.AppendLine("- Title: " + q.TitleText);
            sb.AppendLine("- Description: " + q.DescriptionText);
            string locId = q.Location != null ? (!string.IsNullOrEmpty(q.Location.Id) ? q.Location.Id : q.Location.name) : string.Empty;
            sb.AppendLine("- Location: " + q.Country + " - " + locId);

            sb.AppendLine("## Content");
            sb.AppendLine("- Category: " + q.MainTopic);
            sb.AppendLine("- Knowledge points: " + q.KnowledgeValue);

            // Topics (unique from cards)
            if (q.Cards != null && q.Cards.Count > 0)
            {
                var set = new HashSet<string>();
                foreach (var cc in q.Cards)
                    if (cc != null && cc.Topics != null)
                        foreach (var t in cc.Topics)
                            set.Add(t.ToString());
                if (set.Count > 0)
                {
                    sb.AppendLine("- Topics:");
                    foreach (var t in set.OrderBy(s => s))
                        sb.AppendLine("  - " + t);
                }
            }

            // Linked cards
            if (q.Cards != null && q.Cards.Count > 0)
            {
                sb.AppendLine("- Linked Cards: " + string.Join(", ", q.Cards.Where(c => c != null).Select(c => string.IsNullOrEmpty(c.Id) ? c.name : c.Id)));
            }
            // Words Used
            if (q.WordsUsed != null && q.WordsUsed.Count > 0)
            {
                sb.AppendLine("- Words Used: " + string.Join(", ", q.WordsUsed.Where(w => w != null).Select(w => string.IsNullOrEmpty(w.Id) ? w.name : w.Id)));
            }

            sb.AppendLine("## Gameplay");

            sb.AppendLine("- Difficulty: " + q.Difficulty);
            sb.AppendLine("- Duration (min): " + q.Duration);
            if (q.Gameplay != null && q.Gameplay.Count > 0)
            {
                sb.AppendLine("- Kind:");
                foreach (var g in q.Gameplay)
                    sb.AppendLine("  - " + g);
            }

            // Credits
            sb.AppendLine("## Credits");
            void AppendCredits(string titleLbl, List<AuthorData> list)
            {
                if (list != null && list.Count > 0)
                {
                    sb.AppendLine("" + titleLbl + ": " + string.Join(", ", list.Where(a => a != null).Select(FormatAuthor)));
                }
            }
            AppendCredits("- Content", q.CreditsContent);
            AppendCredits("- Design", q.CreditsDesign);
            AppendCredits("- Development", q.CreditsDevelopment);

            // Optional Additional Resources section between Data and Script
            if (q.AdditionalResources != null && !string.IsNullOrEmpty(q.AdditionalResources.text))
            {
                sb.AppendLine();
                sb.AppendLine("## Additional Resources");
                sb.AppendLine();
                sb.AppendLine(q.AdditionalResources.text);
            }

            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

            // Script section (published only if IsScriptPublic)
            if (q.IsScriptPublic)
            {
                sb.AppendLine("## Script");
                sb.AppendLine();
                if (q.YarnScript != null)
                {
                    sb.AppendLine("```yarn");
                    sb.AppendLine(q.YarnScript.text);
                    sb.AppendLine("```");
                }
                else
                {
                    sb.AppendLine("(No YarnScript attached)");
                }
            }

            return sb;
        }

        private static string GetQuestPublishFileName(QuestData q)
        {
            string code = string.IsNullOrEmpty(q.Id) ? q.name : q.Id;
            foreach (var ch in System.IO.Path.GetInvalidFileNameChars())
                code = code.Replace(ch, '-');
            return code + ".md";
        }

        private void ExportSingleQuest(QuestData q)
        {
            string folder = EditorUtility.SaveFolderPanel("Export Quest Markdown File", Application.dataPath, "QuestExports");
            if (string.IsNullOrEmpty(folder))
                return;
            try
            {
                string path = System.IO.Path.Combine(folder, GetQuestExportFileName(q));
                var sb = BuildQuestMarkdown(q);
                System.IO.File.WriteAllText(path, sb.ToString());
                EditorUtility.RevealInFinder(path);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GameDataBrowser] Failed to write markdown for '{q?.Id}': {ex.Message}");
            }
        }

        private void ExportFiles()
        {
            // Only for QuestData
            var list = Filtered().OfType<QuestData>().ToList();
            if (list.Count == 0)
            {
                EditorUtility.DisplayDialog("Export Files", "No quests to export.", "OK");
                return;
            }
            string folder = EditorUtility.SaveFolderPanel("Export Quest Markdown Files", Application.dataPath, "QuestExports");
            if (string.IsNullOrEmpty(folder))
                return;

            foreach (var q in list)
            {
                try
                {
                    string path = System.IO.Path.Combine(folder, GetQuestExportFileName(q));
                    var sb = BuildQuestMarkdown(q);
                    System.IO.File.WriteAllText(path, sb.ToString());
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[GameDataBrowser] Failed to write markdown for '{q?.Id}': {ex.Message}");
                }
            }

            EditorUtility.RevealInFinder(folder);
        }

        private static string FormatAuthor(AuthorData a)
        {
            if (a == null)
                return string.Empty;
            string name = a.name;
            string country = string.Empty;
            string url = string.Empty;
            try
            {
                var t = a.GetType();
                var nameFi = t.GetField("Name", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var namePi = t.GetProperty("Name", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (nameFi != null && nameFi.FieldType == typeof(string))
                    name = nameFi.GetValue(a) as string ?? name;
                else if (namePi != null && namePi.PropertyType == typeof(string) && namePi.CanRead)
                    name = namePi.GetValue(a, null) as string ?? name;

                var countryFi = t.GetField("Country", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var countryPi = t.GetProperty("Country", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (countryFi != null && countryFi.FieldType == typeof(Countries))
                    country = ((Countries)countryFi.GetValue(a)).ToString();
                else if (countryPi != null && countryPi.PropertyType == typeof(Countries) && countryPi.CanRead)
                    country = ((Countries)countryPi.GetValue(a, null)).ToString();

                var urlFi = t.GetField("Url", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var urlPi = t.GetProperty("Url", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (urlFi != null && urlFi.FieldType == typeof(string))
                    url = urlFi.GetValue(a) as string ?? string.Empty;
                else if (urlPi != null && urlPi.PropertyType == typeof(string) && urlPi.CanRead)
                    url = urlPi.GetValue(a, null) as string ?? string.Empty;
            }
            catch { }

            // Build display: link around the name if URL exists, and keep country in parentheses
            string displayName = name;
            if (!string.IsNullOrEmpty(url))
            {
                displayName = $"[{name}]({url})";
            }
            if (!string.IsNullOrEmpty(country))
            {
                return $"{displayName} ({country})";
            }
            return displayName;
        }

        [MenuItem("Antura/Discover/Publish Quests Website", priority = 300)]
        public static void PublishQuestsToDocs()
        {
            try
            {
                string projectRoot = System.IO.Directory.GetParent(Application.dataPath).FullName;
                string folder = System.IO.Path.Combine(projectRoot, "docs", "manual", "quests");
                System.IO.Directory.CreateDirectory(folder);

                var guids = AssetDatabase.FindAssets("t:QuestData");
                int ok = 0, fail = 0;
                var grouped = new Dictionary<Countries, List<KeyValuePair<string, string>>>();
                foreach (var guid in guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    var q = AssetDatabase.LoadAssetAtPath<QuestData>(assetPath);
                    if (q == null)
                    { fail++; continue; }
                    if (!q.IsPublic)
                    { continue; }
                    try
                    {
                        string fileName = GetQuestPublishFileName(q);
                        string outPath = System.IO.Path.Combine(folder, fileName);
                        var sb = BuildQuestMarkdown(q);
                        System.IO.File.WriteAllText(outPath, sb.ToString());
                        // add to grouped for index
                        var title = q.TitleText;
                        if (string.IsNullOrEmpty(title))
                            title = string.IsNullOrEmpty(q.Id) ? q.name : q.Id;
                        title = title?.Replace("\r", " ").Replace("\n", " ");
                        if (!grouped.TryGetValue(q.Country, out var list))
                        { list = new List<KeyValuePair<string, string>>(); grouped[q.Country] = list; }
                        list.Add(new KeyValuePair<string, string>(title, fileName));
                        ok++;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"[GameDataBrowser] Failed publishing '{q?.Id}': {ex.Message}");
                        fail++;
                    }
                }
                // write index.md grouped by country
                try
                {
                    var indexSb = new System.Text.StringBuilder();
                    indexSb.AppendLine("---");
                    indexSb.AppendLine("title: Antura Discover Quests");
                    indexSb.AppendLine("---");
                    indexSb.AppendLine();

                    Countries[] order = new[] {
                        Countries.France, Countries.Poland, Countries.Germany, Countries.Greece,
                        Countries.Italy, Countries.Portugal, Countries.Spain, Countries.UnitedKingdom,
                        Countries.Global
                    };
                    Func<Countries, string> countryLabel = c =>
                    {
                        var s = c.ToString();
                        if (s == nameof(Countries.UnitedKingdom))
                            return "United Kingdom";
                        return s;
                    };
                    foreach (var c in order)
                    {
                        if (!grouped.TryGetValue(c, out var list) || list == null || list.Count == 0)
                            continue;
                        indexSb.AppendLine($"## {countryLabel(c)}");
                        indexSb.AppendLine();
                        foreach (var kv in list.OrderBy(k => k.Key, StringComparer.OrdinalIgnoreCase))
                        {
                            indexSb.AppendLine($"- [{kv.Key}](./{kv.Value})");
                        }
                        indexSb.AppendLine();
                    }
                    string indexPath = System.IO.Path.Combine(folder, "index.md");
                    System.IO.File.WriteAllText(indexPath, indexSb.ToString());
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[GameDataBrowser] Failed to write quests index.md: {ex.Message}");
                }
                Debug.Log($"[GameDataBrowser] Published {ok} quest files to {folder}. Failures: {fail}");
                EditorUtility.RevealInFinder(folder);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GameDataBrowser] Publish failed: {ex.Message}");
            }
        }
    }
}
#endif
