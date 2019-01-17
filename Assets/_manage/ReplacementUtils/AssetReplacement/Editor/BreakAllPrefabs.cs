using Antura;
using Antura.Helpers;
using UnityEditor;
using UnityEngine;

namespace Replacement
{
    public class BreakAllPrefabs : EditorWindow
    {
        [MenuItem("Antura/Replacement/Break All Prefabs")]
        static void Init()
        {
            GetWindow<BreakAllPrefabs>().Show();
        }

        void OnGUI()
        {
            var selectedGOs = Selection.objects;

            if (selectedGOs.Length > 0)
            {
                if (GUI.Button(new Rect(3, 45, position.width - 6, 20), "Break!"))
                {
                    Perform(selectedGOs);
                }
            }
            else
            {
                EditorGUI.LabelField(new Rect(3, 45, position.width - 6, 20), "Missing:", "Select at least one GameObject first");
            }
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        void Perform(Object[] selectedObjects)
        {
            foreach (var selectedObject in selectedObjects)
            {
                var selectedGo = selectedObject as GameObject;
                foreach (var trasform in selectedGo.GetComponentsInChildren<Transform>())
                {
                    PrefabUtility.DisconnectPrefabInstance(trasform.gameObject);
                }
            }
        }

    }

}

