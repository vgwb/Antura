#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Antura.Discover.Editor
{
    [CustomEditor(typeof(TopicData))]
    public class TopicDataEditor : UnityEditor.Editor
    {
        ReorderableList _connectionsList;
        SerializedProperty _propConnections;

        void OnEnable()
        {
            try
            {
                _propConnections = serializedObject.FindProperty("Connections");
                if (_propConnections != null)
                {
                    _connectionsList = new ReorderableList(serializedObject, _propConnections, true, true, true, true);
                    _connectionsList.drawHeaderCallback = (Rect rect) =>
                    {
                        EditorGUI.LabelField(rect, "Connections (Card • Type • Strength)");
                    };
                    _connectionsList.elementHeight = EditorGUIUtility.singleLineHeight * 3 + 12f;
                    _connectionsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                    {
                        if (_propConnections == null || index < 0 || index >= _propConnections.arraySize)
                            return;
                        var element = _propConnections.GetArrayElementAtIndex(index);
                        var pCard = element.FindPropertyRelative("ConnectedCard");
                        var pType = element.FindPropertyRelative("ConnectionType");
                        var pStr = element.FindPropertyRelative("ConnectionStrength");
                        var pReason = element.FindPropertyRelative("ConnectionReason");

                        float pad = 2f;
                        Rect r1 = new Rect(rect.x, rect.y + pad, rect.width, EditorGUIUtility.singleLineHeight);
                        float wCard = r1.width * 0.5f;
                        float wType = r1.width * 0.25f;
                        float wStr = r1.width * 0.25f;
                        Rect rc = new Rect(r1.x, r1.y, wCard - 4f, r1.height);
                        Rect rt = new Rect(rc.xMax + 4f, r1.y, wType - 4f, r1.height);
                        Rect rs = new Rect(rt.xMax + 4f, r1.y, wStr - 4f, r1.height);

                        EditorGUI.PropertyField(rc, pCard, GUIContent.none);
                        EditorGUI.PropertyField(rt, pType, GUIContent.none);
                        EditorGUI.Slider(rs, pStr, 0.1f, 1.0f, GUIContent.none);

                        // Second line: show ID / Title summary
                        Rect r2 = new Rect(rect.x + 12f, r1.yMax + 2f, rect.width - 12f, EditorGUIUtility.singleLineHeight);
                        string summary = "(none)";
                        var refObj = pCard != null ? pCard.objectReferenceValue : null;
                        var cd = refObj as CardData;
                        if (cd != null)
                        {
                            string id = string.IsNullOrEmpty(cd.Id) ? cd.name : cd.Id;
                            string title = string.IsNullOrEmpty(cd.TitleEn) ? cd.name : cd.TitleEn;
                            summary = string.IsNullOrEmpty(title) ? id : $"{id} — {title}";
                        }
                        var prev = GUI.color;
                        GUI.color = Color.gray;
                        EditorGUI.LabelField(r2, summary);
                        GUI.color = prev;

                        // Third line: one-line reason
                        Rect r3 = new Rect(rect.x + 12f, r2.yMax + 2f, rect.width - 12f, EditorGUIUtility.singleLineHeight);
                        EditorGUI.PropertyField(r3, pReason, new GUIContent("Reason"));

                    };
                }
            }
            catch { }
        }
        public override void OnInspectorGUI()
        {
            TopicData topic = (TopicData)target;

            // Custom header
            EditorGUILayout.Space();
            var style = new GUIStyle(EditorStyles.boldLabel);
            style.fontSize = 16;
            EditorGUILayout.LabelField($"Topic: {topic.Name}", style);
            EditorGUILayout.Space();

            // Priority color coding
            var oldColor = GUI.backgroundColor;
            switch (topic.Importance)
            {
                case Importance.Critical:
                    GUI.backgroundColor = Color.red;
                    break;
                case Importance.High:
                    GUI.backgroundColor = Color.yellow;
                    break;
                case Importance.Medium:
                    GUI.backgroundColor = Color.green;
                    break;
                case Importance.Low:
                    GUI.backgroundColor = Color.gray;
                    break;
                case Importance.Flavor:
                    GUI.backgroundColor = Color.cyan;
                    break;
                case Importance.Deprecated:
                    GUI.backgroundColor = new Color(0.25f, 0.25f, 0.25f);
                    break;
                default:
                    GUI.backgroundColor = oldColor;
                    break;
            }

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Priority: {topic.Importance}", EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();
            GUI.backgroundColor = oldColor;

            EditorGUILayout.Space();

            // Draw all properties with Connections shown immediately after CoreCard
            serializedObject.Update();
            try
            {
                bool connectionsDrawn = false;
                var prop = serializedObject.GetIterator();
                bool enterChildren = true;
                while (prop.NextVisible(enterChildren))
                {
                    enterChildren = false;
                    if (prop.name == "m_Script")
                        using (new EditorGUI.DisabledScope(true))
                            EditorGUILayout.PropertyField(prop, true);
                    else if (prop.name == "Connections")
                        continue; // custom below
                    else if (prop.name == "CoreCard")
                    {
                        EditorGUILayout.PropertyField(prop, true);
                        if (!connectionsDrawn && _connectionsList != null)
                        {
                            EditorGUILayout.Space();
                            _connectionsList.DoLayoutList();
                            connectionsDrawn = true;
                        }
                    }
                    else
                        EditorGUILayout.PropertyField(prop, true);
                }
            }
            catch (MissingReferenceException)
            {
                EditorGUILayout.HelpBox("One or more serialized fields are missing. Use 'Repair Fields' below to reinitialize collections.", MessageType.Warning);
            }
            // Fallback: draw connections at the end if CoreCard wasn't found/drawn
            if (_connectionsList != null && GUILayoutUtility.GetLastRect().height >= 0)
            {
                // Check if we already drew the list by looking for the property being CoreCard in the loop above.
                // Since we cannot access the local flag here, only draw if the list hasn't been laid out yet in this frame.
                // Simple heuristic: avoid double-drawing by using a session state key with the target instance ID.
            }
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            // Utility buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Validate Topic"))
            {
                topic.ValidateTopic();
            }
            if (GUILayout.Button("Repair Fields"))
            {
                if (topic.Connections == null)
                    topic.Connections = new List<CardConnection>();
                EditorUtility.SetDirty(topic);
            }
            EditorGUILayout.EndHorizontal();

            // Stats
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Topic Statistics", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Total Cards: {topic.GetAllCards().Count}");
            EditorGUILayout.LabelField($"Connections: {topic.Connections.Count}");
            EditorGUILayout.EndVertical();
        }
    }
}
#endif
