#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace Antura.Discover
{
    [CustomEditor(typeof(IdentifiedData), true)]
    public class IdentifiedDataEditor : UnityEditor.Editor
    {
        SerializedProperty _idProp;

        void OnEnable()
        {
            if (serializedObject != null)
            {
                serializedObject.Update();
                _idProp = serializedObject.FindProperty("Id");
            }
        }

        public override void OnInspectorGUI()
        {
            if (serializedObject == null || target == null)
            {
                base.OnInspectorGUI();
                return;
            }

            serializedObject.Update();

            EditorGUILayout.LabelField("Identity", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope("box"))
            {
                // ID field
                EditorGUI.BeginChangeCheck();
                string currentId = ((IdentifiedData)target).Id ?? string.Empty;
                string newId = EditorGUILayout.TextField("Id", currentId);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Edit Id");
                    ((IdentifiedData)target).Editor_SetId(newId);
                    if (_idProp != null)
                        _idProp.stringValue = ((IdentifiedData)target).Id;
                    EditorUtility.SetDirty(target);
                }

                if (string.IsNullOrEmpty(((IdentifiedData)target).Id))
                {
                    EditorGUILayout.HelpBox("ID is empty. Use 'Set from filename' or type a value. It will be sanitized (lowercase snake_case).", MessageType.Info);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Set from filename", GUILayout.MaxWidth(160)))
                    {
                        var path = AssetDatabase.GetAssetPath(target);
                        var fileBase = System.IO.Path.GetFileNameWithoutExtension(path);
                        Undo.RecordObject(target, "Set Id from filename");
                        var prefix = TryGetCountryPrefix(target);
                        var idBase = string.IsNullOrEmpty(prefix) ? fileBase : PrefixOnce(prefix, fileBase);
                        ((IdentifiedData)target).Editor_SetId(idBase);
                        if (_idProp != null)
                            _idProp.stringValue = ((IdentifiedData)target).Id;
                        EditorUtility.SetDirty(target);
                    }
                }
            }

            EditorGUILayout.Space();
            DrawPropertiesExcluding(serializedObject, "m_Script", "Id");
            serializedObject.ApplyModifiedProperties();
        }

        // Derive a 2-letter lowercase prefix from a Countries enum field named "Country" if present
        private static string TryGetCountryPrefix(Object obj)
        {
            if (obj == null)
                return null;
            var t = obj.GetType();
            var field = t.GetField("Country", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field == null)
                return null;
            var value = field.GetValue(obj);
            if (value == null)
                return null;
            var name = value.ToString();
            if (string.IsNullOrEmpty(name))
                return null;
            switch (name)
            {
                case "Global":
                    return null; // No prefix for global items
                case "France":
                    return "fr";
                case "Italy":
                    return "it";
                case "Poland":
                    return "pl";
                case "Spain":
                    return "es";
                case "Germany":
                    return "de";
                case "UnitedKingdom":
                    return "uk";
                case "UnitedStates":
                    return "us";
                case "SaudiArabia":
                    return "sa";
                case "UnitedArabEmirates":
                    return "ae";
                case "SouthKorea":
                    return "kr";
                case "Japan":
                    return "jp";
                case "China":
                    return "cn";
                default:
                    return name.Length >= 2 ? name.Substring(0, 2).ToLowerInvariant() : name.ToLowerInvariant();
            }
        }

        private static string PrefixOnce(string prefix, string baseName)
        {
            if (string.IsNullOrEmpty(prefix))
                return baseName;
            var expected = prefix.ToLowerInvariant() + "_";
            if (!string.IsNullOrEmpty(baseName) && baseName.Length >= expected.Length && baseName.Substring(0, expected.Length).ToLowerInvariant() == expected)
                return baseName;
            return expected + baseName;
        }
    }
}
#endif
