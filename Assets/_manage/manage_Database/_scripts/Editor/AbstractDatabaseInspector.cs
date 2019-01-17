using UnityEditor;
using UnityEngine;

namespace Antura.Database.Management.Editor
{
    public class AbstractDatabaseInspector : UnityEditor.Editor
    {
        SerializedObject sobj;

        void OnEnable()
        {
            sobj = new SerializedObject(target);
        }

        public override void OnInspectorGUI()
        {
            sobj.Update();

            var iterator = sobj.GetIterator();
            iterator.Next(true);
            do
            {
                var innerList = iterator.FindPropertyRelative("innerList");
                if (innerList != null)
                { 
                    EditorGUILayout.PropertyField(innerList, new GUIContent(iterator.displayName), true);
                }
            } while (iterator.Next(false));

            sobj.ApplyModifiedProperties();
        }

    }
}