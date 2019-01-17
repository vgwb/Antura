using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Replacement
{
    public class ParseAssetAsText : EditorWindow
    {
        static Object obj;

        [MenuItem("Antura/Replacement/Parse Asset As Text")]
        static void Init()
        {
            GetWindow<ParseAssetAsText>().Show();
        }

        void OnGUI()
        {
            obj = EditorGUI.ObjectField(new Rect(3, 3, position.width - 6, 20), "Asset to Parse", obj, typeof(Object), false);

            if (obj)
            {
                if (GUI.Button(new Rect(3, 25, position.width - 6, 20), "Parse!"))
                {
                    var path = AssetDatabase.GetAssetPath(obj);
                    using (var reader = File.OpenText(path))
                    {
                        ParseContents(reader);
                    }

                }
            }
            else
            {
                EditorGUI.LabelField(new Rect(3, 25, position.width - 6, 20), "Missing:", "Select an object first");
            }
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        protected virtual void ParseContents(StreamReader reader)
        {
            string s = "";
            while (!reader.EndOfStream)
            {
                s += reader.ReadLine() +"\n";
            }
            Debug.Log(s);
        }
    }

}

