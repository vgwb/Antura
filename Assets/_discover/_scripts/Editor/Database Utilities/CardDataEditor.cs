#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Antura.Discover
{
    [CustomEditor(typeof(CardData))]
    [CanEditMultipleObjects]
    public class CardDataEditor : UnityEditor.Editor
    {
        SerializedProperty _scriptProp;

        void OnEnable()
        {
            _scriptProp = serializedObject.FindProperty("m_Script");
        }

        public override void OnInspectorGUI()
        {
            if (serializedObject == null || target == null)
            {
                base.OnInspectorGUI();
                return;
            }

            serializedObject.Update();

            // Draw default inspector except m_Script
            DrawPropertiesExcluding(serializedObject, "m_Script");

            // Preview section for ImageAsset
            var previewCard = (CardData)target;
            if (previewCard != null && previewCard.ImageAsset != null && previewCard.ImageAsset.Image != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);
                using (new EditorGUILayout.VerticalScope("box"))
                {
                    float maxW = EditorGUIUtility.currentViewWidth - 40f;
                    float size = Mathf.Min(256f, maxW);
                    var tex = previewCard.ImageAsset.Image.texture;
                    if (tex != null)
                    {
                        Rect r = GUILayoutUtility.GetRect(size, size, GUILayout.ExpandWidth(false));
                        EditorGUI.DrawPreviewTexture(r, tex, null, ScaleMode.ScaleToFit);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("ImageAsset has no texture.", MessageType.Info);
                    }
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Utilities", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope("box"))
            {
                if (GUILayout.Button("Set Id (Type + TitleEn)", GUILayout.MaxWidth(280)))
                {
                    foreach (var t in targets)
                    {
                        if (t is CardData card)
                        {
                            Undo.RecordObject(card, "Set Id (Type + TitleEn)");
                            string country = IdentifiedData.CountryNameToCode(card.Country.ToString());
                            string typePart = card.Type.ToString();
                            string titlePart = string.IsNullOrWhiteSpace(card.TitleEn) ? card.name : card.TitleEn;
                            string baseName = string.Concat(typePart, "_", titlePart);
                            // string withCountry = string.IsNullOrEmpty(country) ? baseName : string.Concat(baseName, "_", country);
                            string sanitized = IdentifiedData.SanitizeId(baseName);
                            card.Editor_SetId(sanitized);
                            EditorUtility.SetDirty(card);
                        }
                    }
                }

                if (GUILayout.Button("Set Id (First Topic + TitleEn)", GUILayout.MaxWidth(320)))
                {
                    foreach (var t in targets)
                    {
                        if (t is CardData card)
                        {
                            Undo.RecordObject(card, "Set Id (First Topic + TitleEn)");
                            string country = IdentifiedData.CountryNameToCode(card.Country.ToString());
                            string topicPart = (card.Topics != null && card.Topics.Count > 0)
                                ? card.Topics[0].ToString()
                                : card.Type.ToString();
                            string titlePart = string.IsNullOrWhiteSpace(card.TitleEn) ? card.name : card.TitleEn;
                            string baseName = string.Concat(topicPart, "_", titlePart);
                            // string withCountry = string.IsNullOrEmpty(country) ? baseName : string.Concat(baseName, "_", country);
                            string sanitized = IdentifiedData.SanitizeId(baseName);
                            card.Editor_SetId(sanitized);
                            EditorUtility.SetDirty(card);
                        }
                    }
                }

                if (GUILayout.Button("Set Id (filename)", GUILayout.MaxWidth(200)))
                {
                    foreach (var t in targets)
                    {
                        if (t is CardData card)
                        {
                            string path = AssetDatabase.GetAssetPath(card);
                            if (string.IsNullOrEmpty(path))
                            {
                                Debug.LogWarning("Cannot set Id from filename: asset path not found.", card);
                                continue;
                            }
                            string fileName = Path.GetFileNameWithoutExtension(path);
                            if (string.IsNullOrWhiteSpace(fileName))
                            {
                                Debug.LogWarning("Cannot set Id from filename: filename is empty.", card);
                                continue;
                            }
                            Undo.RecordObject(card, "Set Id from filename");
                            string sanitized = IdentifiedData.SanitizeId(fileName);
                            card.Editor_SetId(sanitized);
                            EditorUtility.SetDirty(card);
                        }
                    }
                }
                GUILayout.Space(10);

                if (GUILayout.Button("Rename asset file(s) to Id", GUILayout.MaxWidth(240)))
                {
                    foreach (var t in targets)
                    {
                        if (t is CardData card)
                        {
                            var id = card.Id;
                            if (string.IsNullOrWhiteSpace(id))
                            {
                                Debug.LogWarning("Cannot rename: Id is empty.", card);
                                continue;
                            }
                            string path = AssetDatabase.GetAssetPath(card);
                            if (string.IsNullOrEmpty(path))
                            {
                                Debug.LogWarning("Cannot rename: asset path not found.", card);
                                continue;
                            }
                            string error = AssetDatabase.RenameAsset(path, id);
                            if (!string.IsNullOrEmpty(error))
                            {
                                Debug.LogError($"Rename failed for {path} -> {id}: {error}", card);
                                continue;
                            }
                        }
                    }
                    AssetDatabase.SaveAssets();
                }

            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
