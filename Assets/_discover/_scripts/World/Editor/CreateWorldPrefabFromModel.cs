#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover
{
    // This class  createa a WorldPrefab from a selected model in the Project Window.
    public static class CreateWorldPrefabFromModel
    {
        [MenuItem("Assets/Antura/Create WorldPrefab from selected model", true)]
        private static bool Validate()
        {
            return Selection.objects.OfType<GameObject>()
                .Any(go => PrefabUtility.GetPrefabAssetType(go) == PrefabAssetType.Model);
        }

        [MenuItem("Assets/Antura/Create WorldPrefab from selected model")]
        private static void Create()
        {
            int created = 0;
            foreach (var go in Selection.objects.OfType<GameObject>())
            {
                if (PrefabUtility.GetPrefabAssetType(go) != PrefabAssetType.Model)
                    continue;
                if (CreateOne(go))
                    created++;
            }
            EditorUtility.DisplayDialog("Create World Prefabs", $"Created {created} prefab(s) into Prefabs/ next to each model.", "OK");
        }

        private static bool CreateOne(GameObject modelAsset)
        {
            var modelPath = AssetDatabase.GetAssetPath(modelAsset);
            if (string.IsNullOrEmpty(modelPath))
                return false;

            var outFolder = GetRelativePrefabsFolder(modelPath);
            if (!EnsureFolders(outFolder))
            {
                Debug.LogError($"[CreateWorldPrefab] Invalid output folder: {outFolder}");
                return false;
            }

            var rootName = modelAsset.name;
            var root = new GameObject(rootName);
            GameObject instance = null;
            try
            {
                instance = Object.Instantiate(modelAsset);
                instance.name = modelAsset.name;
                instance.transform.SetParent(root.transform, false);
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localRotation = Quaternion.identity;
                instance.transform.localScale = Vector3.one;

                // Add MeshColliders to child hierarchy wherever MeshFilter exists
                AddMeshColliders(instance);

                // Add WorldPrefabData to root and set stable Id (parentFolder + fileName sanitized)
                var comp = root.GetComponent<WorldPrefabData>();
                if (comp == null)
                    comp = root.AddComponent<WorldPrefabData>();
                var stableId = BuildStablePrefabId(modelPath);
#if UNITY_EDITOR
                comp.__EditorSetPrefabId(stableId);
#else
                comp.Id = stableId;
#endif

                // Save as prefab under Prefabs folder next to the model
                var fileName = rootName + ".prefab";
                var outPath = Path.Combine(outFolder, fileName).Replace("\\", "/");
                outPath = AssetDatabase.GenerateUniqueAssetPath(outPath);

                var prefab = PrefabUtility.SaveAsPrefabAsset(root, outPath, out bool success);
                if (!success || prefab == null)
                {
                    Debug.LogError($"[CreateWorldPrefab] Failed to save prefab: {outPath}");
                    return false;
                }

                return true;
            }
            finally
            {
                if (root != null)
                    Object.DestroyImmediate(root);
                if (instance != null && instance != root)
                    Object.DestroyImmediate(instance);
            }
        }

        private static void AddMeshColliders(GameObject root)
        {
            var filters = root.GetComponentsInChildren<MeshFilter>(true);
            foreach (var mf in filters)
            {
                var go = mf.gameObject;
                if (!go.TryGetComponent<MeshCollider>(out var mc))
                    mc = go.AddComponent<MeshCollider>();
                mc.sharedMesh = mf.sharedMesh;
                mc.convex = false;
            }
        }

        private static string GetRelativePrefabsFolder(string modelAssetPath)
        {
            var dir = Path.GetDirectoryName(modelAssetPath)?.Replace("\\", "/");
            if (string.IsNullOrEmpty(dir))
                return "Assets/Prefabs";
            var prefabs = (dir.EndsWith("/") ? dir + "Prefabs" : dir + "/Prefabs");
            return prefabs;
        }

        private static bool EnsureFolders(string projectRelativeFolder)
        {
            if (AssetDatabase.IsValidFolder(projectRelativeFolder))
                return true;
            var parts = projectRelativeFolder.Split('/')
                .Where(p => !string.IsNullOrEmpty(p)).ToArray();
            if (parts.Length == 0 || parts[0] != "Assets")
                return false;
            string current = "Assets";
            for (int i = 1; i < parts.Length; i++)
            {
                var next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }
                current = next;
            }
            return AssetDatabase.IsValidFolder(projectRelativeFolder);
        }

        private static string BuildStablePrefabId(string assetPath)
        {
            try
            {
                var file = Path.GetFileNameWithoutExtension(assetPath) ?? string.Empty;
                var dir = Path.GetDirectoryName(assetPath) ?? string.Empty;
                var parent = Path.GetFileName(dir) ?? string.Empty;
                var raw = string.IsNullOrEmpty(parent) ? file : parent + "_" + file;
                return IdentifiedData.SanitizeId(raw);
            }
            catch
            {
                return IdentifiedData.SanitizeId(Path.GetFileNameWithoutExtension(assetPath) ?? "prefab");
            }
        }
    }
}
#endif
