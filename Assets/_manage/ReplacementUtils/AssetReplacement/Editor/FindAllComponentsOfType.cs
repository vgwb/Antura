using Antura;
using Antura.Helpers;
using UnityEditor;
using UnityEngine;

// Find T components in all scenes
// Find missing component in all scenes
namespace Replacement
{
    public class FindAllComponentsOfType : EditorWindow
    {
        static Component comp;

        static string outputText;

        [MenuItem("Antura/Replacement/Find All Components Of Type")]
        static void Init()
        {
            GetWindow<FindAllComponentsOfType>().Show();
        }

        void OnGUI()
        {
            comp = EditorGUI.ObjectField(new Rect(3, 3, position.width - 6, 20), "Component to Find", comp, typeof(Component), true) as Component;

            if (comp) {
                if (GUI.Button(new Rect(3, 25, position.width - 6, 20), "Find!")) {
                    Perform(comp);
                }
            } else {
                EditorGUI.LabelField(new Rect(3, 25, position.width - 6, 20), "Missing:", "Select an object first");
            }

            EditorGUI.TextArea(new Rect(3, 50, position.width - 6, position.height - 50), outputText);
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        void Perform(Component comp)
        {
            Debug.Log(comp.GetType());

            var editorSceneInfos = EditorBuildSettings.scenes;

            outputText = "";
            string s = "";
            foreach (var editorSceneInfo in editorSceneInfos) {
                if (!editorSceneInfo.enabled) continue;

                s += "Scene " + editorSceneInfo.path;
                var dict = ReplacementUtility.CollectObjectsWithComponentsOfType(comp.GetType());

                foreach (var pair in dict) {
                    if (pair.Value.Count > 0) {
                        s += "\n > " + pair.Value.Count + " found in GameObject " + pair.Key;
                    }
                }
                s += "\n\n";

            }
            outputText = s;
            //Debug.Log(s);

        }

    }

}

