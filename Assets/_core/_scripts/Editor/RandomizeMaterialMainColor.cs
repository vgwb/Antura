#if UNITY_EDITOR
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Antura.EditorTools
{
    public static class RandomizeMaterialMainColor
    {
        private const string MenuPath = "Assets/Antura/Materials/Randomize Main Color";
        private const string MenuPathFromName = "Assets/Antura/Materials/Set Main Color From Filename Hex";

        private static readonly Regex HexColorRegex = new Regex(@"(?i)(?:#|_)?([0-9a-f]{6})(?![0-9a-f])", RegexOptions.Compiled);

        [MenuItem(MenuPath, priority = 210)]
        public static void RandomizeSelected()
        {
            var materials = Selection.GetFiltered<Material>(SelectionMode.Assets)
                ?.Where(m => m != null)
                .Distinct()
                .ToArray() ?? new Material[0];

            if (materials.Length == 0)
            {
                EditorUtility.DisplayDialog("Randomize Main Color", "Select one or more Materials in the Project window.", "OK");
                return;
            }

            Undo.RecordObjects(materials, "Randomize Material Main Color");

            int changed = 0;
            int skipped = 0;

            foreach (var material in materials)
            {
                var colorProperty = GetMainColorProperty(material);
                if (string.IsNullOrEmpty(colorProperty))
                {
                    skipped++;
                    continue;
                }

                var oldColor = material.GetColor(colorProperty);
                var newColor = UnityEngine.Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.6f, 1f);
                newColor.a = oldColor.a;

                material.SetColor(colorProperty, newColor);
                EditorUtility.SetDirty(material);
                changed++;
            }

            if (changed > 0)
                AssetDatabase.SaveAssets();

            Debug.Log($"Randomized main color for {changed} material(s). Skipped {skipped} material(s) without a known color property.");
        }

        [MenuItem(MenuPathFromName, priority = 211)]
        public static void SetFromFilenameHex()
        {
            var materials = Selection.GetFiltered<Material>(SelectionMode.Assets)
                ?.Where(m => m != null)
                .Distinct()
                .ToArray() ?? Array.Empty<Material>();

            if (materials.Length == 0)
            {
                EditorUtility.DisplayDialog("Set Main Color From Filename Hex", "Select one or more Materials in the Project window.\n\nExample filename: MyMaterial_FFFFFF.mat", "OK");
                return;
            }

            Undo.RecordObjects(materials, "Set Material Main Color From Filename Hex");

            int changed = 0;
            int skippedNoProperty = 0;
            int skippedNoHex = 0;

            foreach (var material in materials)
            {
                var colorProperty = GetMainColorProperty(material);
                if (string.IsNullOrEmpty(colorProperty))
                {
                    skippedNoProperty++;
                    continue;
                }

                if (!TryGetHexColorFromFilename(material, out var newColor))
                {
                    skippedNoHex++;
                    continue;
                }

                var oldColor = material.GetColor(colorProperty);
                newColor.a = oldColor.a;

                material.SetColor(colorProperty, newColor);
                EditorUtility.SetDirty(material);
                changed++;
            }

            if (changed > 0)
                AssetDatabase.SaveAssets();

            Debug.Log($"Set main color from filename hex for {changed} material(s). Skipped {skippedNoHex} without hex, {skippedNoProperty} without a known color property.");
        }

        [MenuItem(MenuPathFromName, true)]
        private static bool SetFromFilenameHex_Validate()
        {
            return Selection.GetFiltered<Material>(SelectionMode.Assets)?.Any(m => m != null) == true;
        }

        [MenuItem(MenuPath, true)]
        private static bool RandomizeSelected_Validate()
        {
            return Selection.GetFiltered<Material>(SelectionMode.Assets)?.Any(m => m != null) == true;
        }

        private static string GetMainColorProperty(Material material)
        {
            if (material == null)
                return null;

            // URP/HDRP commonly use _BaseColor; built-in Standard uses _Color.
            if (material.HasProperty("_BaseColor"))
                return "_BaseColor";
            if (material.HasProperty("_Color"))
                return "_Color";

            // A couple of common fallbacks.
            if (material.HasProperty("_TintColor"))
                return "_TintColor";
            if (material.HasProperty("_MainColor"))
                return "_MainColor";

            return null;
        }

        private static bool TryGetHexColorFromFilename(Material material, out Color color)
        {
            color = default;
            if (material == null)
                return false;

            var assetPath = AssetDatabase.GetAssetPath(material);
            if (string.IsNullOrEmpty(assetPath))
                return false;

            var fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            if (string.IsNullOrEmpty(fileName))
                return false;

            var match = HexColorRegex.Match(fileName);
            if (!match.Success || match.Groups.Count < 2)
                return false;

            var hex = match.Groups[1].Value;
            if (hex.Length != 6)
                return false;

            if (!int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var rgb))
                return false;

            var r = ((rgb >> 16) & 0xFF) / 255f;
            var g = ((rgb >> 8) & 0xFF) / 255f;
            var b = (rgb & 0xFF) / 255f;
            color = new Color(r, g, b, 1f);
            return true;
        }
    }
}
#endif
