#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Antura.Discover.Editor
{
    [CustomEditor(typeof(TopicData))]
    public class TopicDataEditor : UnityEditor.Editor
    {
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
            }

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Priority: {topic.Importance}", EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();
            GUI.backgroundColor = oldColor;

            EditorGUILayout.Space();

            // Default inspector
            DrawDefaultInspector();

            EditorGUILayout.Space();

            // Utility buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Validate Topic"))
            {
                topic.ValidateTopic();
            }
            if (GUILayout.Button("Auto-arrange Discovery Path"))
            {
                // Simple auto-arrange: core card first, then by connection strength
                topic.DiscoveryPath.Clear();
                if (topic.CoreCard != null)
                    topic.DiscoveryPath.Add(topic.CoreCard);

                var sortedConnections = new List<CardConnection>(topic.Connections);
                sortedConnections.Sort((a, b) => b.ConnectionStrength.CompareTo(a.ConnectionStrength));

                foreach (var connection in sortedConnections)
                {
                    if (connection.ConnectedCard != null && !topic.DiscoveryPath.Contains(connection.ConnectedCard))
                        topic.DiscoveryPath.Add(connection.ConnectedCard);
                }

                EditorUtility.SetDirty(topic);
            }
            EditorGUILayout.EndHorizontal();

            // Stats
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Topic Statistics", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Total Cards: {topic.GetAllCards().Count}");
            EditorGUILayout.LabelField($"Connections: {topic.Connections.Count}");
            EditorGUILayout.LabelField($"Discovery Path Length: {topic.DiscoveryPath.Count}");
            EditorGUILayout.EndVertical();
        }
    }
}
#endif
