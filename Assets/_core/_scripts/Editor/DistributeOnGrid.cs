using UnityEngine;
using UnityEditor;

namespace Antura.Editor
{
    public class DistributeOnGrid : EditorWindow
    {
        private float distance = 5.0f; // Set the default distance to 5 meters

        [MenuItem("Antura/Utility/Distribute on Grid")]
        public static void ShowWindow()
        {
            GetWindow<DistributeOnGrid>("Distribute on Grid");
        }

        private void OnGUI()
        {
            GUILayout.Label("Grid Distribution Settings", EditorStyles.boldLabel);
            distance = EditorGUILayout.FloatField("Distance", distance);

            if (GUILayout.Button("Distribute Selected Objects"))
            {
                DistributeSelectedObjects();
            }
        }

        private void DistributeSelectedObjects()
        {
            GameObject[] selectedObjects = Selection.gameObjects;

            if (selectedObjects.Length == 0)
            {
                Debug.LogWarning("No objects selected to distribute.");
                return;
            }

            Undo.RecordObjects(selectedObjects, "Distribute on Grid");

            int gridSize = Mathf.CeilToInt(Mathf.Sqrt(selectedObjects.Length));

            for (int i = 0; i < selectedObjects.Length; i++)
            {
                int row = i / gridSize;
                int col = i % gridSize;
                Vector3 newPosition = new Vector3(col * distance, 0, row * distance);
                selectedObjects[i].transform.position = newPosition;
            }
        }
    }
}
