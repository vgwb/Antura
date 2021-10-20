using System.Collections;
using System.Collections.Generic;
using DG.DeExtensions;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEngine.UIElements.PopupWindow;
#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.UIElements;
#endif

namespace Antura.Language
{
    [System.Serializable]
    public class DiacriticEntryKey
    {
        public string unicode1;
        public string unicode2;

        public int offsetX;
        public int offsetY;

    }
    public class DiacriticsComboData : ScriptableObject
    {
         public   int page = 0;
        public List<DiacriticEntryKey> Keys = new List<DiacriticEntryKey>();
    }

    #if UNITY_EDITOR


    [CustomEditor(typeof(DiacriticsComboData))]
    public class DiacriticsComboDataEditor : Editor
    {
        private SerializedProperty keys;
        void OnEnable()
        {
            keys = serializedObject.FindProperty("Keys");
        }

        int pageSize = 30;
        public override void OnInspectorGUI()
        {
            var data = target as DiacriticsComboData;
            serializedObject.Update();


            GUILayout.BeginHorizontal();
            if (Book.Book.I != null)
            {
                if (GUILayout.Button("Editing At Runtime: " + Book.Book.I.EditDiacritics))
                {
                    Book.Book.I.EditDiacritics = !Book.Book.I.EditDiacritics;
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Page: {data.page}");
            if (GUILayout.Button("<"))
            {
                data.page -= 1;
                if (data.page < 0) data.page = 0;
            };
            if (GUILayout.Button(">"))
            {
                data.page += 1;
                if (data.page >= data.Keys.Count / pageSize) data.page = data.Keys.Count/pageSize;
            };
            GUILayout.EndHorizontal();

            for (int i = data.page*pageSize; i < (data.page+1)*pageSize; i++)
            {
                if (i >= data.Keys.Count) break;
                EditorGUILayout.PropertyField(keys.GetArrayElementAtIndex(i));
            }

            if (GUILayout.Button("+"))
            {
                data.Keys.Add(new DiacriticEntryKey());
            };

            serializedObject.ApplyModifiedProperties();

        }
    }

    [CustomPropertyDrawer(typeof(DiacriticEntryKey))]
    public class DiacriticEntryDrawer : PropertyDrawer
    {
        static Dictionary<string, string> unicodeToLetterCache = new Dictionary<string, string>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyValue = GetSerializedValue<DiacriticEntryKey>(this, property);
            var letter1 = GetLetter(propertyValue.unicode1);
            var letter2 = GetLetter(propertyValue.unicode2);

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            GUIStyle bSty = new GUIStyle(GUI.skin.label);
            bSty.fontSize = 30;

            EditorGUILayout.TextField(letter1, bSty, GUILayout.Width(35), GUILayout.Height(40));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("unicode1"), GUIContent.none, GUILayout.Width(50));
            EditorGUILayout.TextField(" " +letter2, bSty, GUILayout.Width(35), GUILayout.Height(40));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("unicode2"), GUIContent.none, GUILayout.Width(50));
            EditorGUILayout.BeginVertical();
            propertyValue.offsetX = EditorGUILayout.IntSlider(propertyValue.offsetX, -100,100);
            propertyValue.offsetY = EditorGUILayout.IntSlider(propertyValue.offsetY, -100,100);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private string GetLetter(string unicode)
        {
            var letter = "?";
            if (!unicode.IsNullOrEmpty() && !unicodeToLetterCache.TryGetValue(unicode, out letter))
            {
                try
                {
                    var intValue = (int)new System.ComponentModel.Int32Converter().ConvertFromString($"0x{unicode}");
                    var isDiacritic = intValue >= 0x064E && intValue <= 0x0655;
                    if (isDiacritic) unicode = $@" \u{unicode}";
                    else unicode = $@"\u{unicode}";
                    letter = System.Text.RegularExpressions.Regex.Unescape(unicode);
                }
                catch (Exception)
                {
                    letter = "?";
                }
                unicodeToLetterCache[unicode] = letter;
            }
            return letter;
        }

        public static T GetSerializedValue<T>(PropertyDrawer propertyDrawer, SerializedProperty property)
        {
            object @object = propertyDrawer.fieldInfo.GetValue(property.serializedObject.targetObject);

            // UnityEditor.PropertyDrawer.fieldInfo returns FieldInfo:
            // - about the array, if the serialized object of property is inside the array or list;
            // - about the object itself, if the object is not inside the array or list;

            // We need to handle both situations.
            if (((IList) @object.GetType().GetInterfaces()).Contains(typeof(IList<T>)))
            {
                var start = property.propertyPath.LastIndexOf('[');
                var end = property.propertyPath.LastIndexOf(']');
                int propertyIndex = int.Parse(property.propertyPath.Substring(start + 1, end - start - 1));
                return ((IList<T>) @object)[propertyIndex];
            }
            else
            {
                return (T) @object;
            }
        }
    }

#endif
}
