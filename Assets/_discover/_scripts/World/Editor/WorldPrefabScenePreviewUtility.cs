#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Antura.Discover;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Antura.Discover.World.Editor
{
    public static class WorldPrefabScenePreviewUtility
    {
        private const string RootName = "WorldPrefabs_ByKit";
        private const int Columns = 10;
        private const float ExtraMargin = 0.5f; // small extra gap to avoid touching
        private const float GroupSpacing = 1000f; // separation between Kit groups along X

        [MenuItem("Antura/Discover/Prefabs/Populate Scene with World Prefabs (Grouped by Kit)", priority = 2010)]
        public static void PopulateSceneWithWorldPrefabs()
        {
            try
            {
                EditorUtility.DisplayProgressBar("World Prefabs", "Scanning prefabs...", 0f);

                // Find all prefabs under the target folder
                var guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/_discover/Prefabs" });
                var byKit = new Dictionary<WorldPrefabKit, List<(string path, GameObject prefab, WorldPrefabData data)>>();

                for (int i = 0; i < guids.Length; i++)
                {
                    string guid = guids[i];
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (prefab == null) continue;
                    var data = prefab.GetComponent<WorldPrefabData>();
                    if (data == null) continue; // only root-level WPD

                    if (!byKit.TryGetValue(data.Kit, out var list))
                    {
                        list = new List<(string, GameObject, WorldPrefabData)>();
                        byKit[data.Kit] = list;
                    }
                    list.Add((path, prefab, data));
                }

                // Prepare scene root
                var existingRoot = GameObject.Find(RootName);
                if (existingRoot != null)
                {
                    Undo.DestroyObjectImmediate(existingRoot);
                }
                var root = new GameObject(RootName);
                Undo.RegisterCreatedObjectUndo(root, "Create WorldPrefabs Root");

                // Place each kit group
                int kitIndex = 0;
                int total = byKit.Values.Sum(l => l.Count);
                int placed = 0;
                foreach (var kvp in byKit.OrderBy(k => k.Key.ToString()))
                {
                    var kit = kvp.Key;
                    var items = kvp.Value;

                    var kitGO = new GameObject(kit.ToString());
                    kitGO.transform.SetParent(root.transform, false);
                    kitGO.transform.position = new Vector3(kitIndex * GroupSpacing, 0f, 0f);
                    Undo.RegisterCreatedObjectUndo(kitGO, "Create Kit Group");

                    float xCursor = 0f;
                    float zCursor = 0f;
                    float maxRowDepth = 0f;
                    int col = 0;

                    for (int i = 0; i < items.Count; i++)
                    {
                        var (path, prefab, data) = items[i];
                        float progress = (placed + i + 1) / (float)Math.Max(1, total);
                        EditorUtility.DisplayProgressBar("World Prefabs", $"Placing {prefab.name}", progress);

                        // Instantiate under kit group to compute bounds at default scale
                        var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                        if (instance == null) continue;
                        Undo.RegisterCreatedObjectUndo(instance, "Instantiate World Prefab");
                        instance.transform.SetParent(kitGO.transform, false);

                        // compute bounds (local to world, parent unrotated)
                        var b = ComputeInstanceBounds(instance);
                        var width = Mathf.Max(0.01f, b.size.x) + ExtraMargin;
                        var depth = Mathf.Max(0.01f, b.size.z) + ExtraMargin;

                        // position instance
                        instance.transform.localPosition = new Vector3(xCursor, 0f, zCursor);

                        // advance grid
                        xCursor += width;
                        maxRowDepth = Mathf.Max(maxRowDepth, depth);
                        col++;
                        if (col >= Columns)
                        {
                            col = 0;
                            xCursor = 0f;
                            zCursor += maxRowDepth;
                            maxRowDepth = 0f;
                        }
                    }

                    placed += items.Count;
                    kitIndex++;
                }

                // Focus the new root
                Selection.activeGameObject = root;
                EditorGUIUtility.PingObject(root);
                EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static Bounds ComputeInstanceBounds(GameObject go)
        {
            // Prefer renderers (including disabled) for visual size
            var rends = go.GetComponentsInChildren<Renderer>(true);
            if (rends != null && rends.Length > 0)
            {
                var b = rends[0].bounds;
                for (int i = 1; i < rends.Length; i++) b.Encapsulate(rends[i].bounds);
                return b;
            }
            // Then colliders
            var cols = go.GetComponentsInChildren<Collider>(true);
            if (cols != null && cols.Length > 0)
            {
                var b = cols[0].bounds;
                for (int i = 1; i < cols.Length; i++) b.Encapsulate(cols[i].bounds);
                return b;
            }
            // Fallback: mesh filters transformed by localToWorld
            var mfs = go.GetComponentsInChildren<MeshFilter>(true);
            if (mfs != null && mfs.Length > 0)
            {
                var b = TransformBounds(mfs[0].transform.localToWorldMatrix, mfs[0].sharedMesh != null ? mfs[0].sharedMesh.bounds : new Bounds(Vector3.zero, Vector3.one));
                for (int i = 1; i < mfs.Length; i++)
                {
                    var mesh = mfs[i].sharedMesh;
                    if (mesh == null) continue;
                    var tb = TransformBounds(mfs[i].transform.localToWorldMatrix, mesh.bounds);
                    b.Encapsulate(tb);
                }
                return b;
            }
            // Default unit cube
            return new Bounds(go.transform.position, Vector3.one);
        }

        private static Bounds TransformBounds(Matrix4x4 localToWorld, Bounds b)
        {
            var center = localToWorld.MultiplyPoint3x4(b.center);
            var extents = b.extents;
            var axisX = localToWorld.MultiplyVector(new Vector3(extents.x, 0, 0));
            var axisY = localToWorld.MultiplyVector(new Vector3(0, extents.y, 0));
            var axisZ = localToWorld.MultiplyVector(new Vector3(0, 0, extents.z));
            var worldExtents = new Vector3(
                Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x),
                Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y),
                Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z)
            );
            return new Bounds(center, worldExtents * 2f);
        }
    }
}
#endif
