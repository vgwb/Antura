#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover
{
    public class GameDataBrowserWindow : EditorWindow
    {
        private Vector2 _scroll;
        private string _search = string.Empty;

        private struct TypeOption
        {
            public string Label;     // what to show in the popup
            public Type Type;        // concrete type or base type (for aggregates)
            public bool IsAggregate; // when true, filter includes subclasses of Type
        }

        private readonly List<TypeOption> _typeOptions = new List<TypeOption>();
        private int _selectedTypeIndex = 0; // index into _typeOptions

        // Cached data
        private readonly List<IdentifiedData> _allData = new List<IdentifiedData>();

        // Layout sizes
        private const float ColOpen = 60f;
        private const float ColId = 220f;
        private const float ColTitle = 260f;
        private const float ColImage = 80f;
        private const float ColLicense = 110f;
        private const float ColCopyright = 300f;
        private const float RowHeight = ColImage + 12f;

        [MenuItem("Antura/Discover/Data/Game Data Browser", priority = 215)]
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
        }

        private void RefreshTypes()
        {
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
                        {
                            concreteTypes.Add(t);
                        }
                    }
                }
                concreteTypes = concreteTypes.Distinct().ToList();
                concreteTypes.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));

                // Build options
                _typeOptions.Add(new TypeOption { Label = "All Data", Type = null, IsAggregate = false });

                // Aggregate: Activities (all ActivitySettingsAbstract and subclasses)
                var activitiesBase = FindTypeByName("ActivitySettingsAbstract");
                if (activitiesBase != null)
                {
                    _typeOptions.Add(new TypeOption { Label = "Activities (All)", Type = activitiesBase, IsAggregate = true });
                }

                foreach (var t in concreteTypes)
                {
                    _typeOptions.Add(new TypeOption { Label = NicifyTypeLabel(t), Type = t, IsAggregate = false });
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[GameDataBrowser] Failed to gather IdentifiedData types: {ex.Message}");
            }

            // Prefer AssetData if present, else All
            var idx = _typeOptions.FindIndex(o => o.Type != null && o.Type.Name == nameof(AssetData));
            _selectedTypeIndex = idx >= 0 ? idx : 0;
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
            GUILayout.Space(4);
            DrawTypeSelectorPopup();
            GUILayout.Space(6);
            DrawHeader();
            DrawRows();
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                var newSearch = EditorGUILayout.TextField(_search, EditorStyles.toolbarTextField, GUILayout.MinWidth(220));
                if (!string.Equals(newSearch, _search, StringComparison.Ordinal))
                {
                    _search = newSearch;
                    Repaint();
                }
                if (GUILayout.Button("x", EditorStyles.toolbarButton, GUILayout.Width(20)))
                {
                    if (!string.IsNullOrEmpty(_search))
                    {
                        _search = string.Empty;
                        GUI.FocusControl(null);
                        Repaint();
                    }
                }

                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
                {
                    RefreshTypes();
                    RefreshList();
                }
            }
        }

        private void DrawTypeSelectorPopup()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Data:", GUILayout.Width(40));
                var labels = _typeOptions.Select(o => o.Label).ToArray();
                var newIndex = EditorGUILayout.Popup(_selectedTypeIndex, labels, GUILayout.MinWidth(260));
                if (newIndex != _selectedTypeIndex)
                {
                    _selectedTypeIndex = newIndex;
                    Repaint();
                }
            }
        }

        private void DrawHeader()
        {
            var header = EditorStyles.boldLabel;
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Open", header, GUILayout.Width(ColOpen));
                GUILayout.Label("Id", header, GUILayout.Width(ColId));
                // For AssetData we show Image/License specific columns
                if (SelectedType == typeof(AssetData))
                {
                    GUILayout.Label("Image", header, GUILayout.Width(ColImage));
                }
                GUILayout.Label("Title/Name", header, GUILayout.Width(ColTitle));
                if (SelectedType == typeof(AssetData))
                {
                    GUILayout.Label("License", header, GUILayout.Width(ColLicense));
                    GUILayout.Label("Copyright / Source", header, GUILayout.Width(ColCopyright));
                }
            }
            var rect = GUILayoutUtility.GetRect(1, 2);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, position.width, 1), new Color(0, 0, 0, 0.2f));
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

            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            int index = 0;
            foreach (var obj in items)
            {
                var rowRect = GUILayoutUtility.GetRect(position.width, RowHeight);
                if ((index++ % 2) == 0)
                    EditorGUI.DrawRect(new Rect(0, rowRect.y, position.width, rowRect.height), new Color(0, 0, 0, 0.04f));

                if (SelectedType == typeof(AssetData) && obj is AssetData ad)
                {
                    DrawAssetDataRow(rowRect, ad);
                }
                else
                {
                    DrawGenericRow(rowRect, obj);
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawAssetDataRow(Rect rowRect, AssetData a)
        {
            float x = 4f;
            float y = rowRect.y + 4f;
            // Open
            var rOpen = new Rect(x, rowRect.y + (rowRect.height - 20f) * 0.5f, ColOpen - 12f, 20f);
            if (GUI.Button(rOpen, "Open"))
            { EditorGUIUtility.PingObject(a); Selection.activeObject = a; }
            x += ColOpen + 6f;
            // Id
            var rId = new Rect(x, y, ColId, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(rId, a.Id ?? string.Empty);
            x += ColId + 6f;
            // Image
            var rImg = new Rect(x, rowRect.y + 2f, ColImage, ColImage);
            if (a.Image != null)
            {
                var tex = AssetPreview.GetAssetPreview(a.Image) ?? AssetPreview.GetMiniThumbnail(a.Image) ?? a.Image.texture;
                if (tex != null)
                    GUI.DrawTexture(rImg, tex, ScaleMode.ScaleToFit);
                else
                    Repaint();
            }
            x += ColImage + 6f;
            // Title
            var rTitle = new Rect(x, y, ColTitle, EditorGUIUtility.singleLineHeight);
            EditorGUI.BeginChangeCheck();
            string newTitle = EditorGUI.TextField(rTitle, a.Title ?? string.Empty);
            if (EditorGUI.EndChangeCheck())
            { Undo.RecordObject(a, "Edit Title"); a.Title = newTitle; EditorUtility.SetDirty(a); }
            x += ColTitle + 6f;
            // License
            var rLic = new Rect(x, y, ColLicense, EditorGUIUtility.singleLineHeight);
            EditorGUI.BeginChangeCheck();
            var newLic = (LicenseType)EditorGUI.EnumPopup(rLic, a.License);
            if (EditorGUI.EndChangeCheck())
            { Undo.RecordObject(a, "Edit License"); a.License = newLic; EditorUtility.SetDirty(a); }
            x += ColLicense + 6f;
            // Copyright
            var rCopy = new Rect(x, y, ColCopyright, EditorGUIUtility.singleLineHeight);
            EditorGUI.BeginChangeCheck();
            string newCopy = EditorGUI.TextField(rCopy, a.Copyright ?? string.Empty);
            if (EditorGUI.EndChangeCheck())
            { Undo.RecordObject(a, "Edit Copyright"); a.Copyright = newCopy; EditorUtility.SetDirty(a); }
            // Source under it
            var rSrc = new Rect(x, y + EditorGUIUtility.singleLineHeight + 4f, ColCopyright, EditorGUIUtility.singleLineHeight);
            EditorGUI.BeginChangeCheck();
            string newSrc = EditorGUI.TextField(rSrc, a.SourceUrl ?? string.Empty);
            if (EditorGUI.EndChangeCheck())
            { Undo.RecordObject(a, "Edit SourceUrl"); a.SourceUrl = newSrc; EditorUtility.SetDirty(a); }
        }

        private void DrawGenericRow(Rect rowRect, IdentifiedData d)
        {
            float x = 4f;
            float y = rowRect.y + 4f;
            // Open
            var rOpen = new Rect(x, rowRect.y + (rowRect.height - 20f) * 0.5f, ColOpen - 12f, 20f);
            if (GUI.Button(rOpen, "Open"))
            { EditorGUIUtility.PingObject(d); Selection.activeObject = d; }
            x += ColOpen + 6f;
            // Id
            var rId = new Rect(x, y, ColId, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(rId, d.Id ?? string.Empty);
            x += ColId + 6f;
            // Title/Name (best-effort) — support public/private fields or properties
            var nameMember = GetBestNameMember(d.GetType());
            var titleValue = GetStringMemberValue(d, nameMember) ?? string.Empty;
            var rTitle = new Rect(x, y, ColTitle, EditorGUIUtility.singleLineHeight);
            EditorGUI.BeginChangeCheck();
            string newTitle = EditorGUI.TextField(rTitle, titleValue);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(d, "Edit Title/Name");
                SetStringMemberValue(d, nameMember, newTitle);
                EditorUtility.SetDirty(d);
            }
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

            // Search filter across Id, Title/Name and for AssetData also Copyright/Source
            var term = _search?.Trim();
            if (!string.IsNullOrEmpty(term))
            {
                set = set.Where(a => a != null && MatchesSearch(a, term));
            }

            // Sort by Id then Title/Name
            set = set.OrderBy(a => string.IsNullOrEmpty(a?.Id) ? "~" : a.Id)
                     .ThenBy(a => GetStringMemberValue(a, GetBestNameMember(a.GetType())) ?? "~");
            return set;
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
    }
}
#endif
