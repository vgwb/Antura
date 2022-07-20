using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.DeExtensions;
using UnityEngine;
#if UNITY_EDITOR
using Antura.Core;
using UnityEditor;
#endif

namespace Antura.Language
{

    [Serializable]
    public class DiacriticEntryKey
    {
        [Serializable]
        public class Letter
        {
            public string unicode;
            public string id;
            public int sortNumber;
            public int page;

            public override string ToString()
            {
                return $"[{unicode} {id}]";
            }
        }

        public Letter letter1;
        public Letter letter2;

        public int offsetX;
        public int offsetY;

        public override string ToString()
        {
            return $"{letter1} {letter2}";
        }
    }

    public class DiacriticsComboData : ScriptableObject
    {
        public int page = 0;
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

                if (GUILayout.Button("Test Shaddah: " + Book.Book.I.TestShaddah))
                {
                    Book.Book.I.TestShaddah = !Book.Book.I.TestShaddah;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.BeginFadeGroup(Application.isPlaying ? 1f : 0.1f);
            if (GUILayout.Button("Rebuild Diacritics Combo"))
            {
                ArabicLanguageHelper.REFRESH_DIACRITIC_ENTRY_TABLE_FROM_LETTERS_DB = true;
                (AppManager.I.LanguageSwitcher.GetHelper(LanguageUse.Learning) as ArabicLanguageHelper).RebuildDiacriticCombos();
            }
            EditorGUILayout.EndFadeGroup();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Page: {data.page}");

            if (GUILayout.Button("< 10"))
            {
                data.page -= 10;
                var maxPage = data.Keys.Max(x => x.letter1.page);
                if (data.page < 0)
                    data.page += maxPage;
            };
            if (GUILayout.Button("<"))
            {
                data.page -= 1;
                var maxPage = data.Keys.Max(x => x.letter1.page);
                if (data.page < 0)
                    data.page = maxPage;
            };
            if (GUILayout.Button(">"))
            {
                data.page += 1;
                var maxPage = data.Keys.Max(x => x.letter1.page);
                if (data.page > maxPage)
                    data.page = 0;
            };
            if (GUILayout.Button("> 10"))
            {
                data.page += 10;
                var maxPage = data.Keys.Max(x => x.letter1.page);
                if (data.page > maxPage)
                    data.page -= maxPage;
            };

            GUILayout.EndHorizontal();

            var list = keys.GetSerializedValue<List<DiacriticEntryKey>>();
            for (int i = 0; i < data.Keys.Count; i++)
            {
                if (list[i].letter1.page == data.page)
                {
                    EditorGUILayout.PropertyField(keys.GetArrayElementAtIndex(i));
                }
            }

            //if (GUILayout.Button("+")) { data.Keys.Add(new DiacriticEntryKey());};

            serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomPropertyDrawer(typeof(DiacriticEntryKey))]
    public class DiacriticEntryDrawer : PropertyDrawer
    {
        static Dictionary<string, string> unicodeToLetterCache = new Dictionary<string, string>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyValue = property.GetSerializedValue<DiacriticEntryKey>();
            var letter1 = GetLetter(propertyValue.letter1.unicode);
            var letter2 = GetLetter(propertyValue.letter2.unicode);

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            GUIStyle bSty = new GUIStyle(GUI.skin.label);
            bSty.fontSize = 30;
            GUIStyle tSty = new GUIStyle(GUI.skin.label);
            tSty.fontSize = 12;

            EditorGUILayout.TextField(letter1, bSty, GUILayout.Width(35), GUILayout.Height(40));
            EditorGUILayout.BeginVertical();
            EditorGUILayout.TextField(propertyValue.letter1.id, tSty, GUILayout.Width(50));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("letter1.unicode"), GUIContent.none, GUILayout.Width(50));
            EditorGUILayout.EndVertical();

            EditorGUILayout.TextField($" {letter2}", bSty, GUILayout.Width(35), GUILayout.Height(40));
            EditorGUILayout.BeginVertical();
            EditorGUILayout.TextField(propertyValue.letter2.id, tSty, GUILayout.Width(50));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("letter2.unicode"), GUIContent.none, GUILayout.Width(50));
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(property.FindPropertyRelative("offsetX"), GUILayout.ExpandWidth(true));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("offsetY"), GUILayout.ExpandWidth(true));
            // TODO: this won't work correctly, won't make the property dirty
            // limit = 300;
            //propertyValue.offsetX = EditorGUILayout.IntSlider(propertyValue.offsetX, -limit,limit);
            // propertyValue.offsetY = EditorGUILayout.IntSlider(propertyValue.offsetY, -limit,limit);
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
                    if (isDiacritic)
                        unicode = $@" \u{unicode}";
                    else
                        unicode = $@"\u{unicode}";
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
            if (((IList)@object.GetType().GetInterfaces()).Contains(typeof(IList<T>)))
            {
                var start = property.propertyPath.LastIndexOf('[');
                var end = property.propertyPath.LastIndexOf(']');
                int propertyIndex = int.Parse(property.propertyPath.Substring(start + 1, end - start - 1));
                return ((IList<T>)@object)[propertyIndex];
            }
            else
            {
                return (T)@object;
            }
        }
    }

    public static class SerializedPropertyExtensions
    {
        public static T GetSerializedValue<T>(this SerializedProperty property)
        {
            object @object = property.serializedObject.targetObject;
            string[] propertyNames = property.propertyPath.Split('.');

            // Clear the property path from "Array" and "data[i]".
            if (propertyNames.Length >= 3 && propertyNames[propertyNames.Length - 2] == "Array")
                propertyNames = propertyNames.Take(propertyNames.Length - 2).ToArray();

            // Get the last object of the property path.
            foreach (string path in propertyNames)
            {
                @object = @object.GetType()
                    .GetField(path, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .GetValue(@object);
            }

            if (@object.GetType().GetInterfaces().Contains(typeof(IList<T>)))
            {
                var start = property.propertyPath.LastIndexOf('[');
                var end = property.propertyPath.LastIndexOf(']');
                int propertyIndex = int.Parse(property.propertyPath.Substring(start + 1, end - start - 1));

                return ((IList<T>)@object)[propertyIndex];
            }
            else
                return (T)@object;
        }
    }
#endif
}
