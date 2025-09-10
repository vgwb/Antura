using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace AdventurEd.Editor
{
    [CustomPropertyDrawer(typeof(LanguageCode))]
    public class LanguageCodeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var codeProp = property.FindPropertyRelative("code");
            string current = codeProp.stringValue;

            // Build arrays for popup
            var codes = new List<string> { "" }; // first = undefined
            var names = new List<string> { "<Undefined>" };

            foreach (var kvp in LanguageCodes.Codes)
            {
                codes.Add(kvp.Key);
                names.Add(kvp.Value);
            }

            int index = Mathf.Max(0, codes.IndexOf(current));

            EditorGUI.BeginProperty(position, label, property);
            int newIndex = EditorGUI.Popup(position, label.text, index, names.ToArray());
            codeProp.stringValue = codes[newIndex]; // will be "" if undefined selected
            EditorGUI.EndProperty();
        }
    }
}
