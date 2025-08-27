#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover
{
    public static class CreateFromSelection
    {
        private static readonly string[] ImageExtensions = { ".png", ".jpg", ".jpeg" };

        // Create AssetData from selected image(s)
        [MenuItem("Assets/Antura/Create AssetData from Image", priority = 210)]
        public static void CreateAssetDataFromImage()
        {
            var objs = Selection.objects;
            if (objs == null || objs.Length == 0)
                return;

            int created = 0, updated = 0, skipped = 0;
            AssetDatabase.StartAssetEditing();
            try
            {
                foreach (var obj in objs)
                {
                    if (obj == null)
                    { skipped++; continue; }
                    var assetPath = AssetDatabase.GetAssetPath(obj);
                    if (string.IsNullOrEmpty(assetPath))
                    { skipped++; continue; }

                    // Check extension or object type
                    var ext = Path.GetExtension(assetPath).ToLowerInvariant();
                    if (!(Array.IndexOf(ImageExtensions, ext) >= 0 || obj is Texture2D || obj is Sprite))
                    { skipped++; continue; }

                    // Ensure sprite import and load sprite
                    EnsureSpriteImporter(assetPath);
                    var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                    if (sprite == null)
                    { skipped++; continue; }

                    var dir = Path.GetDirectoryName(assetPath);
                    var fileBase = Path.GetFileNameWithoutExtension(assetPath);
                    var defaultPath = Path.Combine(dir ?? string.Empty, fileBase + ".asset").Replace("\\", "/");

                    // Infer country and build id
                    var code = InferCountryCodeFromPath(assetPath);
                    var country = MapCountryCodeToEnum(code);
                    var id = IdentifiedData.BuildSanitizedId(fileBase, code);

                    var existing = AssetDatabase.LoadAssetAtPath<AssetData>(defaultPath);
                    if (existing == null)
                    {
                        var createPath = AssetDatabase.GenerateUniqueAssetPath(defaultPath);
                        var data = ScriptableObject.CreateInstance<AssetData>();
                        data.Type = AssetType.Image;
                        data.Image = sprite;
                        data.Country = country;
                        data.Editor_SetId(id);
                        AssetDatabase.CreateAsset(data, createPath);
                        created++;
                    }
                    else
                    {
                        Undo.RecordObject(existing, "Update AssetData from Image");
                        existing.Type = AssetType.Image;
                        existing.Image = sprite;
                        if (existing.Country == Countries.International && country != Countries.International)
                            existing.Country = country;
                        if (string.IsNullOrWhiteSpace(existing.Id))
                            existing.Editor_SetId(id);
                        EditorUtility.SetDirty(existing);
                        updated++;
                    }
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (created + updated > 0)
            {
                Debug.Log($"[CreateFromSelection] AssetData - Created: {created}, Updated: {updated}, Skipped: {skipped}");
            }
            else
            {
                EditorUtility.DisplayDialog("Create AssetData", "No valid images selected.", "OK");
            }
        }

        [MenuItem("Assets/Antura/Create AssetData from Image", validate = true)]
        private static bool ValidateCreateAssetDataFromImage()
        {
            var objs = Selection.objects;
            if (objs == null || objs.Length == 0)
                return false;
            foreach (var obj in objs)
            {
                if (obj is Sprite || obj is Texture2D)
                    return true;
                var path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path))
                {
                    var ext = Path.GetExtension(path).ToLowerInvariant();
                    if (Array.IndexOf(ImageExtensions, ext) >= 0)
                        return true;
                }
            }
            return false;
        }

        // Create CardData from selected AssetData (supports multi-selection)
        [MenuItem("Assets/Antura/Create CardData from AssetData", priority = 211)]
        public static void CreateCardDataFromAssetData()
        {
            var objs = Selection.objects;
            if (objs == null || objs.Length == 0)
            {
                EditorUtility.DisplayDialog("Create CardData", "Please select one or more AssetData.", "OK");
                return;
            }

            int created = 0, skipped = 0;
            UnityEngine.Object lastCreated = null;
            AssetDatabase.StartAssetEditing();
            try
            {
                foreach (var obj in objs)
                {
                    AssetData asset = obj as AssetData;
                    if (asset == null)
                    {
                        var p = AssetDatabase.GetAssetPath(obj);
                        if (!string.IsNullOrEmpty(p))
                            asset = AssetDatabase.LoadAssetAtPath<AssetData>(p);
                    }
                    if (asset == null)
                    { skipped++; continue; }

                    var assetPath = AssetDatabase.GetAssetPath(asset);
                    var dir = Path.GetDirectoryName(assetPath);
                    var baseName = string.IsNullOrEmpty(asset.Id) ? Path.GetFileNameWithoutExtension(assetPath) : asset.Id;
                    var cardPath = Path.Combine(dir ?? string.Empty, baseName + "_Card.asset").Replace("\\", "/");
                    cardPath = AssetDatabase.GenerateUniqueAssetPath(cardPath);

                    var code = InferCountryCodeFromPath(assetPath);
                    var id = IdentifiedData.BuildSanitizedId(baseName, code);

                    var card = ScriptableObject.CreateInstance<CardData>();
                    card.ImageAsset = asset;
                    card.Country = asset.Country;
                    card.Editor_SetId(id);

                    AssetDatabase.CreateAsset(card, cardPath);
                    created++;
                    lastCreated = card;
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (lastCreated != null)
            {
                EditorGUIUtility.PingObject(lastCreated);
                Selection.activeObject = lastCreated;
            }
            Debug.Log($"[CreateFromSelection] CardData - Created: {created}, Skipped: {skipped}");
        }

        [MenuItem("Assets/Antura/Create CardData from AssetData", validate = true)]
        private static bool ValidateCreateCardDataFromAssetData()
        {
            var objs = Selection.objects;
            if (objs == null || objs.Length == 0)
                return false;
            foreach (var obj in objs)
            {
                if (obj is AssetData)
                    return true;
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path))
                    continue;
                var data = AssetDatabase.LoadAssetAtPath<AssetData>(path);
                if (data != null)
                    return true;
            }
            return false;
        }

        // Helpers (small, local copies for convenience)
        private static bool EnsureSpriteImporter(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
                return false;
            bool changed = false;
            if (importer.textureType != TextureImporterType.Sprite)
            { importer.textureType = TextureImporterType.Sprite; changed = true; }
            if (importer.spriteImportMode != SpriteImportMode.Single)
            { importer.spriteImportMode = SpriteImportMode.Single; changed = true; }
            if (importer.mipmapEnabled)
            { importer.mipmapEnabled = false; changed = true; }
            if (changed)
            {
                try
                { importer.SaveAndReimport(); }
                catch { }
            }
            return changed;
        }

        private static string InferCountryCodeFromPath(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return null;
            var parts = assetPath.Split('/', '\\');
            foreach (var p in parts)
            {
                if (p != null && p.Length >= 3 && p[2] == '_')
                {
                    var a = char.ToLowerInvariant(p[0]);
                    var b = char.ToLowerInvariant(p[1]);
                    if (a >= 'a' && a <= 'z' && b >= 'a' && b <= 'z')
                        return new string(new[] { a, b });
                }
            }
            return null;
        }

        private static Countries MapCountryCodeToEnum(string code)
        {
            if (string.IsNullOrEmpty(code))
                return Countries.International;
            switch (code.ToLowerInvariant())
            {
                case "fr":
                    return Countries.France;
                case "it":
                    return Countries.Italy;
                case "pl":
                    return Countries.Poland;
                case "es":
                    return Countries.Spain;
                case "de":
                    return Countries.Germany;
                case "uk":
                    return Countries.UnitedKingdom;
                case "pt":
                    return Countries.Portugal;
                case "gr":
                    return Countries.Greece;
                default:
                    return Countries.International;
            }
        }
    }
}
#endif
