#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Antura.Discover.Editor
{
    [CustomEditor(typeof(KnowledgeData))]
    public class KnowledgeClusterDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            KnowledgeData cluster = (KnowledgeData)target;

            // Custom header
            EditorGUILayout.Space();
            var style = new GUIStyle(EditorStyles.boldLabel);
            style.fontSize = 16;
            EditorGUILayout.LabelField($"Knowledge Cluster: {cluster.Name}", style);
            EditorGUILayout.Space();

            // Priority color coding
            var oldColor = GUI.backgroundColor;
            switch (cluster.Importance)
            {
                case KnowledgeImportance.Critical:
                    GUI.backgroundColor = Color.red;
                    break;
                case KnowledgeImportance.High:
                    GUI.backgroundColor = Color.yellow;
                    break;
                case KnowledgeImportance.Medium:
                    GUI.backgroundColor = Color.green;
                    break;
                case KnowledgeImportance.Low:
                    GUI.backgroundColor = Color.gray;
                    break;
            }

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Priority: {cluster.Importance}", EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();
            GUI.backgroundColor = oldColor;

            EditorGUILayout.Space();

            // Default inspector
            DrawDefaultInspector();

            EditorGUILayout.Space();

            // Utility buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Validate Cluster"))
            {
                cluster.ValidateKnowledge();
            }
            if (GUILayout.Button("Auto-arrange Discovery Path"))
            {
                // Simple auto-arrange: core card first, then by connection strength
                cluster.DiscoveryPath.Clear();
                if (cluster.CoreCard != null)
                    cluster.DiscoveryPath.Add(cluster.CoreCard);

                var sortedConnections = new List<CardConnection>(cluster.Connections);
                sortedConnections.Sort((a, b) => b.connectionStrength.CompareTo(a.connectionStrength));

                foreach (var connection in sortedConnections)
                {
                    if (connection.connectedCard != null && !cluster.DiscoveryPath.Contains(connection.connectedCard))
                        cluster.DiscoveryPath.Add(connection.connectedCard);
                }

                EditorUtility.SetDirty(cluster);
            }
            EditorGUILayout.EndHorizontal();

            // Stats
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Cluster Statistics", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Total Cards: {cluster.GetAllCards().Count}");
            EditorGUILayout.LabelField($"Connections: {cluster.Connections.Count}");
            EditorGUILayout.LabelField($"Discovery Path Length: {cluster.DiscoveryPath.Count}");
            EditorGUILayout.EndVertical();
        }
    }
}
#endif
