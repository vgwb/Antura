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

        private const float ColOpen = 60f;
        private const float ColId = 200f;
        private const float ColTitle = 220f;
        private const float ColImage = 56f; // preview square
        private const float ColLicense = 120f;
        private const float ColCopyright = 300f;

        [MenuItem("Antura/Discover/Assets/AssetData Browser", priority = 210)]
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
                GUILayout.Label("Id", header, GUILayout.Width(ColId));
                GUILayout.Label("Title", header, GUILayout.Width(ColTitle));
                GUILayout.Label("Image", header, GUILayout.Width(ColImage));
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
                var rowRect = GUILayoutUtility.GetRect(position.width, 56);
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
                // Id
                var rId = new Rect(x, y, ColId, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(rId, a.Id ?? string.Empty);
                x += ColId + 6f;
                // Title
                var rTitle = new Rect(x, y, ColTitle, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(rTitle, a.Title ?? string.Empty);
                x += ColTitle + 6f;
                // Image
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
                // License
                var rLic = new Rect(x, y, ColLicense, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(rLic, a.License.ToString());
                x += ColLicense + 6f;
                // Copyright
                var rCopy = new Rect(x, y, ColCopyright, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(rCopy, a.Copyright ?? string.Empty);
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
                    return set;
                case TypeFilter.Images:
                    return set.Where(a => a != null && a.Type == AssetType.Image);
                case TypeFilter.Audio:
                    return set.Where(a => a != null && a.Type == AssetType.Audio);
                case TypeFilter.Model3D:
                    return set.Where(a => a != null && a.Type == AssetType.Model3D);
                default:
                    return set;
            }
        }
    }
}
#endif
