#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover
{
    public static class AssetDataLinker
    {
        private static readonly string[] ImageExtensions = { ".png", ".jpg", ".jpeg" };

        [MenuItem("Antura/Discover/Utility/Link Sprites to AssetData (Scan & Fix)", priority = 200)]
        public static void LinkAll()
        {
            ProcessFolders(EnumerateTargetFolders(), SearchOption.TopDirectoryOnly);
        }

        // Context menu on selected folder(s) in the Project window
        [MenuItem("Assets/Antura/Link Sprites to AssetData (Scan & Fix)", priority = 200)]
        public static void LinkInSelectedFolders()
        {
            var folders = GetSelectedFolders();
            if (folders.Count == 0)
            {
                EditorUtility.DisplayDialog("AssetData Linker", "Please select one or more folders in the Project window.", "OK");
                return;
            }
            ProcessFolders(folders, SearchOption.AllDirectories);
        }

        // Validate to show item only when a folder is selected
        [MenuItem("Assets/Antura/Link Sprites to AssetData (Scan & Fix)", validate = true)]
        private static bool LinkInSelectedFolders_Validate()
        {
            return GetSelectedFolders().Count > 0;
        }

        private static IEnumerable<string> EnumerateTargetFolders()
        {
            // 1) Assets/_discover/_data/Assets
            var direct = "Assets/_discover/_data/Assets";
            yield return direct;

            // 2) All folders named 'assets' (case-insensitive) under Assets/_discover/_data/Quests
            var questsRoot = "Assets/_discover/_data/Quests";
            var absRoot = ToAbsolute(questsRoot);
            if (Directory.Exists(absRoot))
            {
                foreach (var dir in Directory.EnumerateDirectories(absRoot, "*", SearchOption.AllDirectories))
                {
                    var name = Path.GetFileName(dir);
                    if (string.Equals(name, "assets", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return ToAssetPath(dir);
                    }
                }
            }
        }

        private static List<string> GetSelectedFolders()
        {
            var list = new List<string>();
            foreach (var obj in Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (AssetDatabase.IsValidFolder(path))
                {
                    list.Add(path);
                }
            }
            return list;
        }

        private static void ProcessFolders(IEnumerable<string> folders, SearchOption searchOption)
        {
            int created = 0, updated = 0, fixedImport = 0;
            foreach (var folder in folders)
            {
                var absFolder = ToAbsolute(folder);
                if (!Directory.Exists(absFolder))
                    continue;
                foreach (var file in Directory.EnumerateFiles(absFolder, "*", searchOption))
                {
                    var ext = Path.GetExtension(file).ToLowerInvariant();
                    if (Array.IndexOf(ImageExtensions, ext) < 0)
                        continue;

                    var assetPath = ToAssetPath(file);
                    fixedImport += EnsureSpriteImporter(assetPath) ? 1 : 0;

                    var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                    if (sprite == null)
                        continue; // import failed, skip

                    var fileBase = Path.GetFileNameWithoutExtension(assetPath);
                    var dir = Path.GetDirectoryName(assetPath);
                    var assetDataPath = Path.Combine(dir ?? string.Empty, fileBase + ".asset").Replace("\\", "/");
                    // Infer country context from path
                    var code = InferCountryCodeFromPath(assetPath);
                    var country = MapCountryCodeToEnum(code);

                    var data = AssetDatabase.LoadAssetAtPath<AssetData>(assetDataPath);
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<AssetData>();
                        data.Title = fileBase;
                        data.Type = AssetType.Image;
                        data.Image = sprite;
                        data.Country = country;
                        data.Editor_SetId(IdentifiedData.BuildSanitizedId(fileBase, code));
                        AssetDatabase.CreateAsset(data, assetDataPath);
                        created++;
                    }
                    else
                    {
                        bool dirty = false;
                        if (data.Image != sprite)
                        { data.Image = sprite; dirty = true; }
                        if (data.Type != AssetType.Image)
                        { data.Type = AssetType.Image; dirty = true; }
                        if (string.IsNullOrWhiteSpace(data.Title))
                        { data.Title = fileBase; dirty = true; }
                        // Set country if still Global/default and context provides one
                        if (data.Country.Equals(Countries.Global) && !country.Equals(Countries.Global))
                        { data.Country = country; dirty = true; }
                        // Ensure an ID exists; if it's the bare filename and we have a code, upgrade to prefixed
                        if (string.IsNullOrWhiteSpace(data.Id))
                        {
                            data.Editor_SetId(IdentifiedData.BuildSanitizedId(fileBase, code));
                            dirty = true;
                        }
                        else if (!string.IsNullOrEmpty(code))
                        {
                            var bare = IdentifiedData.SanitizeId(fileBase);
                            if (string.Equals(data.Id, bare, StringComparison.Ordinal))
                            {
                                data.Editor_SetId(IdentifiedData.BuildSanitizedId(fileBase, code));
                                dirty = true;
                            }
                        }
                        if (dirty)
                        { EditorUtility.SetDirty(data); updated++; }
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Optional: run DataHealth to sanitize/ensure uniqueness
            try
            {
                var logs = new List<string>();
                DataHealthUtility.ScanAndFixAll(applyChanges: true, logs: logs, verbose: false);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[AssetDataLinker] DataHealthUtility failed: {ex.Message}");
            }

            Debug.Log($"[AssetDataLinker] Created: {created}, Updated: {updated}, FixedImport: {fixedImport}");
        }

        private static string InferCountryCodeFromPath(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return null;
            // Look for segments like "FR_", "IT_", etc.
            var parts = assetPath.Split('/', '\\');
            foreach (var p in parts)
            {
                if (p != null && p.Length >= 3 && p[2] == '_')
                {
                    var a = char.ToLowerInvariant(p[0]);
                    var b = char.ToLowerInvariant(p[1]);
                    if (a >= 'a' && a <= 'z' && b >= 'a' && b <= 'z')
                    {
                        return new string(new[] { a, b });
                    }
                }
            }
            return null;
        }

        private static Countries MapCountryCodeToEnum(string code)
        {
            if (string.IsNullOrEmpty(code))
                return Countries.Global;
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
                    return Countries.Global;
            }
        }

        private static bool EnsureSpriteImporter(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
                return false;
            bool changed = false;
            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                changed = true;
            }
            if (importer.spriteImportMode != SpriteImportMode.Single)
            {
                importer.spriteImportMode = SpriteImportMode.Single;
                changed = true;
            }
            if (importer.mipmapEnabled)
            {
                importer.mipmapEnabled = false;
                changed = true;
            }
            if (changed)
            {
                try
                { importer.SaveAndReimport(); }
                catch { }
            }
            return changed;
        }

        private static string ToAbsolute(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return string.Empty;
            if (assetPath.StartsWith("Assets/"))
                return Path.Combine(Application.dataPath, assetPath.Substring("Assets/".Length)).Replace("\\", "/");
            return assetPath;
        }

        private static string ToAssetPath(string absolutePath)
        {
            var dataPath = Application.dataPath.Replace("\\", "/");
            var full = absolutePath.Replace("\\", "/");
            if (full.StartsWith(dataPath))
                return "Assets/" + full.Substring(dataPath.Length + 1);
            return absolutePath;
        }
    }
}
#endif
