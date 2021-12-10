using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            // Get all assets in the lang paths
            var lang = "persian_dari"; // @note: change this manually
            var guids = AssetDatabase.FindAssets("", new[] { "Assets/_lang_bundles/" + lang });
            Debug.Log("Fixing addressable for lang: " + lang);
            var group = AddressableAssetSettingsDefaultObject.Settings.groups.FirstOrDefault(x => x.name.ToLower().Contains(lang));
            foreach (var guid in guids) {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var entry = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(guid, group);
                var removedPathLength = "Assets/_lang_bundles/".Length;
                path = path.Replace(".asset", "");
                entry.address = path.Substring(removedPathLength, path.Length - removedPathLength);
            }
            Debug.Log("FINISHED Fixing addressable for lang: " + lang);
        }
    }
}
