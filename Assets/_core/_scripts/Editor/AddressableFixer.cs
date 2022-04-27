using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Language;
using DG.DeExtensions;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace Antura.Tools
{
    public class AddressableFixer : MonoBehaviour
    {
        [MenuItem("Antura/Tools/Fix Addressables")]
        public static void FixAddressables()
        {
            // TODO: Get all assets in the lang paths for the current edition
            var lang = "persian_dari"; // @note: change this manually
            var guids = AssetDatabase.FindAssets("", new[] { "Assets/_lang_bundles/" + lang });
            Debug.Log("Fixing addressable for lang: " + lang);
            var group = AddressableAssetSettingsDefaultObject.Settings.groups.FirstOrDefault(x => x.name.ToLower().Contains(lang));
            // Fixing common addressables
            Debug.Log("Fixing addressable for shape data.");
            group = AddressableAssetSettingsDefaultObject.Settings.groups.FirstOrDefault(x => x.name.ToLower().Contains(lang));
            guids = AssetDatabase.FindAssets("", new[] { "Assets/_core/Fonts/"});
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (!path.Contains("ShapeData/shapedata")) continue;

                var entry = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(guid, group);
                var splits = path.Split('.')[0].Split('/');
                var fontName = splits[splits.Length - 3].Split(' ')[1];
                var assetName = splits[splits.Length - 1];
                var ch = assetName.Last();
                if (!char.IsDigit(ch))
                {
                    var hexCode = string.Format("{0:X4}", Convert.ToUInt16(ch));
                    assetName = assetName.Substring(0, assetName.Length - 1) + hexCode;
                }
                AssetDatabase.RenameAsset(path, assetName);
                entry.address =  fontName + "/" + assetName;
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("FINISHED Fixing addressable for shape data");
        }

        [MenuItem("Antura/Tools/Rename Side Data")]
        public static void RenameSideData()
        {
            // Get all assets in ALL lang paths
            foreach (var lang in (LanguageCode[])System.Enum.GetValues(typeof(LanguageCode)))
            {
                if (lang == LanguageCode.NONE || lang == LanguageCode.COUNT)
                    continue;

                Debug.Log("Fixing data for lang: " + lang);
                var guids = AssetDatabase.FindAssets("", new[] { $"Assets/_lang_bundles/{lang}" });
                foreach (var guid in guids)
                {
                    var oldpath = AssetDatabase.GUIDToAssetPath(guid);
                    var oldname = oldpath.Split('/').Last();

                    if (oldname.Contains("sideletter"))
                    {
                        var newname = oldname.Replace("sideletter", "shapedata");
                        Debug.Log("Renaming to " + newname);
                        var result = AssetDatabase.RenameAsset(oldpath, newname);
                        if (!result.IsNullOrEmpty())
                            Debug.Log("Error: " + result);
                    }
                }
            }
            AssetDatabase.SaveAssets();
            Debug.Log("FINISHED Fixing side data");
        }
    }
}
