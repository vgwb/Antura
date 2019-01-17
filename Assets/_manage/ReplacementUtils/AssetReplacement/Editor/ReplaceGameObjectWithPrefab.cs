using Antura;
using Antura.Helpers;
using UnityEditor;
using UnityEngine;

namespace Replacement
{
    public class ReplaceGameObjectWithPrefab : EditorWindow
    {
        static GameObject go;
        static GameObject prefab;

        [MenuItem("Antura/Replacement/Replace GameObject With Prefab")]
        static void Init()
        {
            GetWindow<ReplaceGameObjectWithPrefab>().Show();
        }

        void OnGUI()
        {
            //go = EditorGUI.ObjectField(new Rect(3, 3, position.width - 6, 20), "GameObject to replace", go, typeof(GameObject), true) as GameObject;
            prefab = EditorGUI.ObjectField(new Rect(3, 25, position.width - 6, 20), "Prefab to use", prefab, typeof(GameObject), false) as GameObject;

            var selectedGOs = Selection.objects;

            if (selectedGOs.Length > 0 && prefab)
            {
                if (GUI.Button(new Rect(3, 45, position.width - 6, 20), "Replace!"))
                {
                    Perform(selectedGOs, prefab);
                }
            }
            else
            {
                EditorGUI.LabelField(new Rect(3, 45, position.width - 6, 20), "Missing:", "Select an object first");
            }
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        void Perform(Object[] originalObjects, GameObject prefab)
        {
            foreach (var originalObject in originalObjects)
            {
                var originalGo = originalObject as GameObject;
                var newGo = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                newGo.name = originalGo.name;
                newGo.transform.SetParent(originalGo.transform.parent);
                newGo.transform.position = originalGo.transform.position;
                newGo.transform.localEulerAngles = originalGo.transform.localEulerAngles;
                newGo.transform.localScale = originalGo.transform.localScale;
                newGo.transform.SetSiblingIndex(originalGo.transform.GetSiblingIndex());
                PrefabUtility.ReconnectToLastPrefab(newGo);
                DestroyImmediate(originalGo);
            }
        }

    }

}

