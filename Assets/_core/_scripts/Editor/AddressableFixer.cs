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
            var lang = LanguageCode.french.ToString(); // @note: change this manually
            var guids = AssetDatabase.FindAssets("", new[] { "Assets/_lang_bundles/" + lang });
            Debug.Log("Fixing addressable for lang: " + lang);
            var group = AddressableAssetSettingsDefaultObject.Settings.groups.FirstOrDefault(x => x.name.ToLower().Contains(lang));
            foreach (var guid in guids) {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var entry = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(guid, group);
                var removedPathLength = "Assets/_lang_bundles/".Length;
                var splits = path.Split('.');
                if (splits.Length > 1) {
                    var extension = splits[splits.Length - 1];
                    path = path.Substring(0, path.Length - (extension.Length + 1));
                }
                entry.address = path.Substring(removedPathLength, path.Length - removedPathLength);
            }
            Debug.Log("FINISHED Fixing addressable for lang: " + lang);
        }

        [MenuItem("Antura/Tools/Rename Side Data")]
        public static void RenameSideData()
        {
            // Get all assets in ALL lang paths
            foreach (var lang in (LanguageCode[])System.Enum.GetValues(typeof(LanguageCode))) {
                if (lang == LanguageCode.NONE || lang == LanguageCode.COUNT) continue;

                Debug.Log("Fixing data for lang: " + lang);
                var guids = AssetDatabase.FindAssets("", new[] { $"Assets/_lang_bundles/{lang}" });
                foreach (var guid in guids) {
                    var oldpath = AssetDatabase.GUIDToAssetPath(guid);
                    var oldname = oldpath.Split('/').Last();

                    if (oldname.Contains("sideletter")) {
                        var newname = oldname.Replace("sideletter", "shapedata");
                        Debug.Log("Renaming to " + newname);
                        var result = AssetDatabase.RenameAsset(oldpath, newname);
                        if (!result.IsNullOrEmpty()) Debug.Log("Error: " + result);
                    }
                }
            }
            AssetDatabase.SaveAssets();
            Debug.Log("FINISHED Fixing side data");
        }
    }
}