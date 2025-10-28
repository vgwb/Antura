using UnityEngine;
using UnityEngine.Splines;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Generates wall colliders (and optionally visual meshes) along the segments of a SplineContainer.
/// </summary>
namespace Antura.Discover
{
    [ExecuteAlways]
    [RequireComponent(typeof(SplineContainer))]
    public class SplineSegmentWalls : MonoBehaviour
    {
        [Header("Wall Dimensions")]
        public float wallHeight = 5f;
        public float wallThickness = 0.1f;
        public bool loop = false;

        [Header("Mesh Settings")]
        public bool createMesh = true;
        public bool doubleFace = false;
        public bool invertNormals = false;
        public Material wallMaterial;

        [Header("Editor & Gizmos")]
        public bool showGizmos = true;

#if UNITY_EDITOR
        [HideInInspector] public bool generated = false;
#endif

        public void GenerateWalls()
        {
            var spline = GetComponent<SplineContainer>();
            if (spline == null || spline.Spline == null)
                return;

            // Remove previous walls
            var toDelete = new System.Collections.Generic.List<GameObject>();
            foreach (Transform child in transform)
                toDelete.Add(child.gameObject);
            foreach (var go in toDelete)
#if UNITY_EDITOR
                DestroyImmediate(go);
#else
            Destroy(go);
#endif

            var path = spline.Spline;
            int count = path.Count;
            for (int i = 0; i < count - 1; i++)
                CreateWallSegment(path[i].Position, path[i + 1].Position, i);

            if (loop)
                CreateWallSegment(path[count - 1].Position, path[0].Position, count - 1);

#if UNITY_EDITOR
            generated = true;
#endif
        }

        private void CreateWallSegment(Vector3 startLocal, Vector3 endLocal, int index)
        {
            Vector3 start = transform.TransformPoint(startLocal);
            Vector3 end = transform.TransformPoint(endLocal);

            Vector3 segment = end - start;
            float length = segment.magnitude;
            if (length < 0.001f)
                return;

            Vector3 mid = (start + end) / 2f;
            Quaternion rot = Quaternion.LookRotation(segment.normalized, Vector3.up);

            var wall = new GameObject($"WallSegment_{index}");
            wall.transform.SetParent(transform);
            wall.transform.SetPositionAndRotation(mid, rot);
            wall.layer = LayerMask.NameToLayer("AreaWall");

            // Create Collider
            var box = wall.AddComponent<BoxCollider>();
            box.size = new Vector3(wallThickness, wallHeight, length);
            box.center = new Vector3(0, wallHeight / 2f, 0);

            if (createMesh)
            {
                CreateVisualQuad(wall.transform, length, wallHeight, invertNormals, wallMaterial, "Front");

                if (doubleFace)
                    CreateVisualQuad(wall.transform, length, wallHeight, !invertNormals, wallMaterial, "Back", flip: true);
            }
        }

        private void CreateVisualQuad(Transform parent, float length, float height, bool invert, Material mat, string name, bool flip = false)
        {
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = name;
            quad.transform.SetParent(parent);
            quad.transform.localPosition = new Vector3(0, height / 2f, 0);
            quad.transform.localScale = new Vector3(length, height, 1);
            quad.transform.localRotation = Quaternion.Euler(0, invert ? -90 : 90, 0);

            if (flip)
                quad.transform.localPosition += Vector3.back * 0.001f;

            var renderer = quad.GetComponent<MeshRenderer>();
            if (mat != null)
                renderer.sharedMaterial = mat;

#if UNITY_EDITOR
            DestroyImmediate(quad.GetComponent<Collider>());
#else
        Destroy(quad.GetComponent<Collider>());
#endif
        }

        void OnDrawGizmos()
        {
            if (!showGizmos)
                return;
            var spline = GetComponent<SplineContainer>();
            if (spline == null || spline.Spline == null)
                return;

            Gizmos.color = Color.cyan;
            var path = spline.Spline;
            int count = path.Count;

            for (int i = 0; i < count - 1; i++)
                DrawSegment(path[i].Position, path[i + 1].Position);

            if (loop)
                DrawSegment(path[count - 1].Position, path[0].Position);
        }

        void DrawSegment(Vector3 startLocal, Vector3 endLocal)
        {
            Vector3 start = transform.TransformPoint(startLocal);
            Vector3 end = transform.TransformPoint(endLocal);
            Gizmos.DrawLine(start, end);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SplineSegmentWalls))]
    public class SplineSegmentWallsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Generate Walls from Spline"))
            {
                var s = (SplineSegmentWalls)target;
                Undo.RegisterFullObjectHierarchyUndo(s.gameObject, "Generate Spline Walls");
                s.GenerateWalls();
            }
            GUI.backgroundColor = Color.white;

            var sc = (SplineSegmentWalls)target;
            if (sc.generated)
                EditorGUILayout.HelpBox("Walls generated.", MessageType.Info);
        }
    }
#endif
}
