#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Antura.Discover.Editor
{
    [CustomEditor(typeof(KnowledgeClusterData))]
    public class KnowledgeClusterDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            KnowledgeClusterData cluster = (KnowledgeClusterData)target;

            // Custom header
            EditorGUILayout.Space();
            var style = new GUIStyle(EditorStyles.boldLabel);
            style.fontSize = 16;
            EditorGUILayout.LabelField($"Knowledge Cluster: {cluster.clusterName}", style);
            EditorGUILayout.Space();

            // Priority color coding
            var oldColor = GUI.backgroundColor;
            switch (cluster.priority)
            {
                case ClusterPriority.Critical:
                    GUI.backgroundColor = Color.red;
                    break;
                case ClusterPriority.High:
                    GUI.backgroundColor = Color.yellow;
                    break;
                case ClusterPriority.Medium:
                    GUI.backgroundColor = Color.green;
                    break;
                case ClusterPriority.Low:
                    GUI.backgroundColor = Color.gray;
                    break;
            }

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Priority: {cluster.priority}", EditorStyles.boldLabel);
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
                cluster.ValidateCluster();
            }
            if (GUILayout.Button("Auto-arrange Discovery Path"))
            {
                // Simple auto-arrange: core card first, then by connection strength
                cluster.discoveryPath.Clear();
                if (cluster.coreCard != null)
                    cluster.discoveryPath.Add(cluster.coreCard);

                var sortedConnections = new List<CardConnection>(cluster.connections);
                sortedConnections.Sort((a, b) => b.connectionStrength.CompareTo(a.connectionStrength));

                foreach (var connection in sortedConnections)
                {
                    if (connection.connectedCard != null && !cluster.discoveryPath.Contains(connection.connectedCard))
                        cluster.discoveryPath.Add(connection.connectedCard);
                }

                EditorUtility.SetDirty(cluster);
            }
            EditorGUILayout.EndHorizontal();

            // Stats
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Cluster Statistics", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Total Cards: {cluster.GetAllCards().Count}");
            EditorGUILayout.LabelField($"Connections: {cluster.connections.Count}");
            EditorGUILayout.LabelField($"Discovery Path Length: {cluster.discoveryPath.Count}");
            EditorGUILayout.EndVertical();
        }
    }
}
#endif
