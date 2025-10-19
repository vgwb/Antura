#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover.Editor
{
    /// <summary>
    /// Bulk repair utility to ensure all AssetData addressable references are grouped & labeled.
    /// Runs OnValidate by forcing an import on each asset.
    /// </summary>
    public static class AssetDataAddressablesUtility
    {
        [MenuItem("Antura/Addressables/Repair AssetData Addressables", priority = 200)]
        private static void RepairAll()
        {
            var guids = AssetDatabase.FindAssets("t:AssetData");
            if (guids.Length == 0)
            {
                EditorUtility.DisplayDialog("AssetData Repair", "No AssetData assets found.", "OK");
                return;
            }

            try
            {
                int processed = 0;
                for (int i = 0; i < guids.Length; i++)
                {
                    var guid = guids[i];
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    EditorUtility.DisplayProgressBar("Repairing AssetData", path, (float)i / guids.Length);
                    // Force reimport so AssetData.OnValidate runs and creates groups/labels.
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                    processed++;
                }
                AssetDatabase.SaveAssets();
                Debug.Log($"[AssetDataAddressablesUtility] Repaired {processed} AssetData assets.");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}
#endif
