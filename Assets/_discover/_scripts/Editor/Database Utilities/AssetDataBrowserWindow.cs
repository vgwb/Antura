#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover
{
    public class AssetDataBrowserWindow : EditorWindow
    {
        private enum CountryFilter { All, Global, France, Poland }
        private enum TypeFilter { All, Images, Audio, Model3D }

        private Vector2 _scroll;
        private List<AssetData> _allAssets = new List<AssetData>();
        private CountryFilter _filter = CountryFilter.All;
        private TypeFilter _typeFilter = TypeFilter.All;
        private string _search = string.Empty;

        private const float ColOpen = 60f;
        private const float ColId = 200f;
        private const float ColTitle = 240f;
        private const float ColImage = 80f; // bigger preview square
        private const float ColLicense = 110f;
        private const float ColCopyright = 300f;
        private const float RowHeight = ColImage + 12f;

        [MenuItem("Antura/Discover/Asset Browser", priority = 210)]
        public static void ShowWindow()
        {
            var wnd = GetWindow<AssetDataBrowserWindow>(false, "AssetData Browser", true);
            wnd.minSize = new Vector2(600, 300);
            wnd.RefreshList();
            wnd.Show();
        }

        private void OnEnable()
        {
            RefreshList();
        }

        private void RefreshList()
        {
            _allAssets.Clear();
            var guids = AssetDatabase.FindAssets("t:AssetData");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var data = AssetDatabase.LoadAssetAtPath<AssetData>(path);
                if (data != null)
                    _allAssets.Add(data);
            }
            // Sort by Id then Title for readability
            _allAssets = _allAssets
                .OrderBy(a => string.IsNullOrEmpty(a.Id) ? "~" : a.Id)
                .ThenBy(a => string.IsNullOrEmpty(a.Title) ? "~" : a.Title)
                .ToList();
            Repaint();
        }

        private void OnGUI()
        {
            DrawToolbar();
            GUILayout.Space(4);
            DrawHeader();
            DrawRows();
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                var newFilter = (CountryFilter)EditorGUILayout.EnumPopup(_filter, EditorStyles.toolbarPopup, GUILayout.Width(120));
                if (newFilter != _filter)
                { _filter = newFilter; Repaint(); }

                GUILayout.Space(6);
                var newType = (TypeFilter)EditorGUILayout.EnumPopup(_typeFilter, EditorStyles.toolbarPopup, GUILayout.Width(120));
                if (newType != _typeFilter)
                { _typeFilter = newType; Repaint(); }

                GUILayout.Space(8);
                var newSearch = EditorGUILayout.TextField(_search, EditorStyles.toolbarTextField, GUILayout.MinWidth(180));
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
                    RefreshList();
                }
            }
        }

        private void DrawHeader()
        {
            var header = EditorStyles.boldLabel;
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Open", header, GUILayout.Width(ColOpen));
                GUILayout.Label("Image", header, GUILayout.Width(ColImage));
                GUILayout.Label("Id", header, GUILayout.Width(ColId));
                GUILayout.Label("Title", header, GUILayout.Width(ColTitle));
                GUILayout.Label("License", header, GUILayout.Width(ColLicense));
                GUILayout.Label("Copyright", header, GUILayout.Width(ColCopyright));
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
                EditorGUILayout.HelpBox("No AssetData found for the current filter.", MessageType.Info);
                return;
            }

            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            int index = 0;
            foreach (var a in items)
            {
                var rowRect = GUILayoutUtility.GetRect(position.width, RowHeight);
                // Alternating background across full width
                if ((index++ % 2) == 0)
                    EditorGUI.DrawRect(new Rect(0, rowRect.y, position.width, rowRect.height), new Color(0, 0, 0, 0.04f));

                float x = 4f;
                float y = rowRect.y + 4f;
                // Open button first
                var rOpen = new Rect(x, rowRect.y + (rowRect.height - 20f) * 0.5f, ColOpen - 12f, 20f);
                if (GUI.Button(rOpen, "Open"))
                {
                    EditorGUIUtility.PingObject(a);
                    Selection.activeObject = a;
                }
                x += ColOpen + 6f;
                // Image (second column)
                var rImg = new Rect(x, rowRect.y + 2f, ColImage, ColImage);
                if (a.Image != null)
                {
                    // Try a proper asset preview, then mini thumbnail, then raw texture
                    var tex = AssetPreview.GetAssetPreview(a.Image) ?? AssetPreview.GetMiniThumbnail(a.Image) ?? a.Image.texture;
                    if (tex != null)
                        GUI.DrawTexture(rImg, tex, ScaleMode.ScaleToFit);
                    else
                        Repaint();
                }
                x += ColImage + 6f;
                // Id
                var rId = new Rect(x, y, ColId, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(rId, a.Id ?? string.Empty);
                x += ColId + 6f;
                // Title
                var rTitle = new Rect(x, y, ColTitle, EditorGUIUtility.singleLineHeight);
                EditorGUI.BeginChangeCheck();
                string newTitle = EditorGUI.TextField(rTitle, a.Title ?? string.Empty);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(a, "Edit AssetData Title");
                    a.Title = newTitle;
                    EditorUtility.SetDirty(a);
                }
                x += ColTitle + 6f;
                // License
                var rLic = new Rect(x, y, ColLicense, EditorGUIUtility.singleLineHeight);
                EditorGUI.BeginChangeCheck();
                var newLic = (LicenseType)EditorGUI.EnumPopup(rLic, a.License);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(a, "Edit AssetData License");
                    a.License = newLic;
                    EditorUtility.SetDirty(a);
                }
                x += ColLicense + 6f;
                // Copyright (first line)
                var rCopy = new Rect(x, y, ColCopyright, EditorGUIUtility.singleLineHeight);
                EditorGUI.BeginChangeCheck();
                string newCopy = EditorGUI.TextField(rCopy, a.Copyright ?? string.Empty);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(a, "Edit AssetData Copyright");
                    a.Copyright = newCopy;
                    EditorUtility.SetDirty(a);
                }

                // Source URL (second line under copyright)
                var rSrc = new Rect(x, y + EditorGUIUtility.singleLineHeight + 4f, ColCopyright, EditorGUIUtility.singleLineHeight);
                EditorGUI.BeginChangeCheck();
                string newSrc = EditorGUI.TextField(rSrc, a.SourceUrl ?? string.Empty);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(a, "Edit AssetData SourceUrl");
                    a.SourceUrl = newSrc;
                    EditorUtility.SetDirty(a);
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private IEnumerable<AssetData> Filtered()
        {
            // Country filter first
            IEnumerable<AssetData> set;
            switch (_filter)
            {
                case CountryFilter.Global:
                    set = _allAssets.Where(a => a != null && a.Country == Countries.Global);
                    break;
                case CountryFilter.France:
                    set = _allAssets.Where(a => a != null && a.Country == Countries.France);
                    break;
                case CountryFilter.Poland:
                    set = _allAssets.Where(a => a != null && a.Country == Countries.Poland);
                    break;
                case CountryFilter.All:
                default:
                    set = _allAssets;
                    break;
            }
            // Then type filter
            switch (_typeFilter)
            {
                case TypeFilter.All:
                    break;
                case TypeFilter.Images:
                    set = set.Where(a => a != null && a.Type == AssetType.Image);
                    break;
                case TypeFilter.Audio:
                    set = set.Where(a => a != null && a.Type == AssetType.Audio);
                    break;
                case TypeFilter.Model3D:
                    set = set.Where(a => a != null && a.Type == AssetType.Model3D);
                    break;
                default:
                    break;
            }
            // Search filter (Id, Title, Copyright, SourceUrl)
            if (!string.IsNullOrEmpty(_search))
            {
                var term = _search.Trim();
                if (term.Length > 0)
                {
                    set = set.Where(a => a != null && (
                        (!string.IsNullOrEmpty(a.Id) && a.Id.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(a.Title) && a.Title.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(a.Copyright) && a.Copyright.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(a.SourceUrl) && a.SourceUrl.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    ));
                }
            }
            return set;
        }
    }
}
#endif
