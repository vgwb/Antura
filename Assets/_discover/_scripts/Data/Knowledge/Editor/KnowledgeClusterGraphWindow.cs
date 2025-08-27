using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Antura.Discover.Editor
{
    public class KnowledgeClusterGraphWindow : EditorWindow
    {
        private KnowledgeCollectionData clusterCollection;
        private Vector2 scrollPos;
        private Vector2 graphOffset = Vector2.zero;
        private float zoomLevel = 1.0f;
        private bool isDragging = false;
        private Vector2 lastMousePos;

        // Graph layout
        private Dictionary<KnowledgeClusterData, Vector2> clusterPositions = new Dictionary<KnowledgeClusterData, Vector2>();
        private Dictionary<CardData, Vector2> cardPositions = new Dictionary<CardData, Vector2>();

        // Display options
        private bool showConnections = true;
        private bool showCardDetails = true;
        private bool showBridges = true;
        private bool autoLayout = true;
        private ClusterPriority filterPriority = ClusterPriority.Low;

        // Selection
        private KnowledgeClusterData selectedCluster;
        private CardData selectedCard;

        // Styling
        private GUIStyle clusterStyle;
        private GUIStyle cardStyle;
        private GUIStyle selectedStyle;
        private Dictionary<ClusterPriority, Color> priorityColors;

        [MenuItem("Antura/Discover/Knowledge/Knowledge Graph")]
        public static void ShowWindow()
        {
            GetWindow<KnowledgeClusterGraphWindow>("Cluster Graph");
        }

        private void OnEnable()
        {
            InitializeStyles();
            InitializePriorityColors();

            // Try to find cluster collection automatically
            if (clusterCollection == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:KnowledgeClusterCollection");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    clusterCollection = AssetDatabase.LoadAssetAtPath<KnowledgeCollectionData>(path);
                    RefreshLayout();
                }
            }
        }

        private void InitializeStyles()
        {
            clusterStyle = new GUIStyle("box")
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 10,
                wordWrap = true
            };

            cardStyle = new GUIStyle("button")
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 8,
                wordWrap = true
            };

            selectedStyle = new GUIStyle("box")
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 10,
                wordWrap = true,
                normal = { background = Texture2D.whiteTexture }
            };
        }

        private void InitializePriorityColors()
        {
            priorityColors = new Dictionary<ClusterPriority, Color>
            {
                { ClusterPriority.Critical, new Color(1f, 0.2f, 0.2f, 0.8f) },
                { ClusterPriority.High, new Color(1f, 0.8f, 0.2f, 0.8f) },
                { ClusterPriority.Medium, new Color(0.2f, 0.8f, 0.2f, 0.8f) },
                { ClusterPriority.Low, new Color(0.6f, 0.6f, 0.6f, 0.8f) }
            };
        }

        private void OnGUI()
        {
            DrawToolbar();

            if (clusterCollection == null)
            {
                EditorGUILayout.HelpBox("No KnowledgeClusterCollection assigned. Please select one to visualize.", MessageType.Info);
                return;
            }

            HandleInput();
            DrawGraph();
            DrawSidebar();

            Repaint();
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            clusterCollection = (KnowledgeCollectionData)EditorGUILayout.ObjectField(
                clusterCollection, typeof(KnowledgeCollectionData), false, GUILayout.Width(200));

            if (GUILayout.Button("Refresh Layout", EditorStyles.toolbarButton))
            {
                RefreshLayout();
            }

            GUILayout.Space(10);

            autoLayout = GUILayout.Toggle(autoLayout, "Auto Layout", EditorStyles.toolbarButton);
            showConnections = GUILayout.Toggle(showConnections, "Connections", EditorStyles.toolbarButton);
            showCardDetails = GUILayout.Toggle(showCardDetails, "Card Details", EditorStyles.toolbarButton);
            showBridges = GUILayout.Toggle(showBridges, "Bridges", EditorStyles.toolbarButton);

            GUILayout.Space(10);

            EditorGUILayout.LabelField("Filter:", GUILayout.Width(40));
            filterPriority = (ClusterPriority)EditorGUILayout.EnumPopup(filterPriority, EditorStyles.toolbarPopup, GUILayout.Width(80));

            GUILayout.FlexibleSpace();

            EditorGUILayout.LabelField($"Zoom: {zoomLevel:F2}", GUILayout.Width(80));

            EditorGUILayout.EndHorizontal();
        }

        private void HandleInput()
        {
            Event e = Event.current;

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                Vector2 mousePos = e.mousePosition;
                mousePos = (mousePos - graphOffset) / zoomLevel;

                // Check cluster selection
                foreach (var kvp in clusterPositions)
                {
                    Rect clusterRect = GetClusterRect(kvp.Key, kvp.Value);
                    if (clusterRect.Contains(mousePos))
                    {
                        selectedCluster = kvp.Key;
                        selectedCard = null;
                        GUI.changed = true;
                        e.Use();
                        return;
                    }
                }

                // Check card selection
                if (showCardDetails)
                {
                    foreach (var kvp in cardPositions)
                    {
                        Rect cardRect = GetCardRect(kvp.Key, kvp.Value);
                        if (cardRect.Contains(mousePos))
                        {
                            selectedCard = kvp.Key;
                            selectedCluster = clusterCollection.FindClusterForCard(kvp.Key);
                            GUI.changed = true;
                            e.Use();
                            return;
                        }
                    }
                }
            }

            // Pan and zoom
            if (e.type == EventType.MouseDrag && e.button == 0)
            {
                if (!isDragging)
                {
                    isDragging = true;
                    lastMousePos = e.mousePosition;
                }

                Vector2 delta = e.mousePosition - lastMousePos;
                graphOffset += delta;
                lastMousePos = e.mousePosition;
                e.Use();
            }

            if (e.type == EventType.MouseUp)
            {
                isDragging = false;
            }

            if (e.type == EventType.ScrollWheel)
            {
                float newZoom = zoomLevel - e.delta.y * 0.1f;
                zoomLevel = Mathf.Clamp(newZoom, 0.2f, 3.0f);
                e.Use();
            }
        }

        private void DrawGraph()
        {
            if (clusterCollection == null)
                return;

            Rect graphArea = new Rect(0, EditorGUIUtility.singleLineHeight + 5, position.width - 250, position.height - EditorGUIUtility.singleLineHeight - 5);

            GUI.BeginGroup(graphArea);

            // Apply zoom and offset
            Matrix4x4 oldMatrix = GUI.matrix;
            GUI.matrix = Matrix4x4.TRS(graphOffset, Quaternion.identity, Vector3.one * zoomLevel);

            // Draw bridges first (behind everything)
            if (showBridges)
            {
                DrawClusterBridges();
            }

            // Draw connections
            if (showConnections)
            {
                DrawCardConnections();
            }

            // Draw clusters
            DrawClusters();

            // Draw cards
            if (showCardDetails)
            {
                DrawCards();
            }

            GUI.matrix = oldMatrix;
            GUI.EndGroup();
        }

        private void DrawClusters()
        {
            foreach (var cluster in clusterCollection.allClusters)
            {
                if (cluster == null || cluster.priority > filterPriority)
                    continue;

                if (!clusterPositions.ContainsKey(cluster))
                    clusterPositions[cluster] = GetRandomPosition();

                Vector2 pos = clusterPositions[cluster];
                Rect rect = GetClusterRect(cluster, pos);

                // Set color based on priority
                Color oldColor = GUI.backgroundColor;
                GUI.backgroundColor = priorityColors[cluster.priority];

                GUIStyle style = (cluster == selectedCluster) ? selectedStyle : clusterStyle;

                GUI.Box(rect, cluster.clusterName, style);

                // Draw priority indicator
                Rect priorityRect = new Rect(rect.x, rect.y - 15, rect.width, 12);
                GUI.Label(priorityRect, cluster.priority.ToString(), EditorStyles.miniLabel);

                GUI.backgroundColor = oldColor;
            }
        }

        private void DrawCards()
        {
            foreach (var cluster in clusterCollection.allClusters)
            {
                if (cluster == null || cluster.priority > filterPriority)
                    continue;
                if (!clusterPositions.ContainsKey(cluster))
                    continue;

                Vector2 clusterPos = clusterPositions[cluster];
                var allCards = cluster.GetAllCards();

                for (int i = 0; i < allCards.Count; i++)
                {
                    var card = allCards[i];
                    if (card == null)
                        continue;

                    Vector2 cardPos = GetCardPositionInCluster(clusterPos, i, allCards.Count);
                    cardPositions[card] = cardPos;

                    Rect rect = GetCardRect(card, cardPos);

                    Color oldColor = GUI.backgroundColor;
                    if (card == selectedCard)
                        GUI.backgroundColor = Color.yellow;
                    else if (card == cluster.coreCard)
                        GUI.backgroundColor = Color.cyan;

                    if (GUI.Button(rect, card.name, cardStyle))
                    {
                        selectedCard = card;
                        selectedCluster = cluster;
                    }

                    GUI.backgroundColor = oldColor;
                }
            }
        }

        private void DrawCardConnections()
        {
            foreach (var cluster in clusterCollection.allClusters)
            {
                if (cluster == null || cluster.priority > filterPriority)
                    continue;
                if (cluster.coreCard == null)
                    continue;

                Vector2 corePos = Vector2.zero;
                if (cardPositions.ContainsKey(cluster.coreCard))
                    corePos = cardPositions[cluster.coreCard];

                foreach (var connection in cluster.connections)
                {
                    if (connection.connectedCard == null)
                        continue;
                    if (!cardPositions.ContainsKey(connection.connectedCard))
                        continue;

                    Vector2 connectedPos = cardPositions[connection.connectedCard];

                    Color connectionColor = GetConnectionTypeColor(connection.connectionType);
                    connectionColor.a = connection.connectionStrength;

                    DrawLine(corePos, connectedPos, connectionColor, connection.connectionStrength * 3f);
                }
            }
        }

        private void DrawClusterBridges()
        {
            foreach (var bridge in clusterCollection.bridges)
            {
                if (bridge.fromCluster == null || bridge.toCluster == null)
                    continue;
                if (bridge.fromCluster.priority > filterPriority || bridge.toCluster.priority > filterPriority)
                    continue;

                if (!clusterPositions.ContainsKey(bridge.fromCluster) || !clusterPositions.ContainsKey(bridge.toCluster))
                    continue;

                Vector2 fromPos = clusterPositions[bridge.fromCluster];
                Vector2 toPos = clusterPositions[bridge.toCluster];

                DrawLine(fromPos, toPos, Color.magenta, bridge.bridgeStrength * 5f);
            }
        }

        private void DrawSidebar()
        {
            Rect sidebarArea = new Rect(position.width - 250, EditorGUIUtility.singleLineHeight + 5, 250, position.height - EditorGUIUtility.singleLineHeight - 5);

            GUI.BeginGroup(sidebarArea, EditorStyles.helpBox);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            EditorGUILayout.LabelField("Graph Statistics", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Total Clusters: {clusterCollection.allClusters.Count}");
            EditorGUILayout.LabelField($"Visible Clusters: {clusterCollection.allClusters.Count(c => c != null && c.priority <= filterPriority)}");
            EditorGUILayout.LabelField($"Bridges: {clusterCollection.bridges.Count}");

            EditorGUILayout.Space();

            if (selectedCluster != null)
            {
                EditorGUILayout.LabelField("Selected Cluster", EditorStyles.boldLabel);
                EditorGUILayout.LabelField($"Name: {selectedCluster.clusterName}");
                EditorGUILayout.LabelField($"Priority: {selectedCluster.priority}");
                EditorGUILayout.LabelField($"Cards: {selectedCluster.GetAllCards().Count}");
                EditorGUILayout.LabelField($"Cohesion: {selectedCluster.cohesionStrength:F2}");

                if (selectedCluster.coreCard != null)
                {
                    EditorGUILayout.LabelField($"Core: {selectedCluster.coreCard.name}");
                }

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Connections:", EditorStyles.boldLabel);
                foreach (var conn in selectedCluster.connections)
                {
                    if (conn.connectedCard != null)
                    {
                        EditorGUILayout.LabelField($"• {conn.connectedCard.name} ({conn.connectionType})");
                    }
                }

                EditorGUILayout.Space();
                if (GUILayout.Button("Focus on Selection"))
                {
                    FocusOnCluster(selectedCluster);
                }
                if (GUILayout.Button("Edit Cluster"))
                {
                    Selection.activeObject = selectedCluster;
                }
            }

            if (selectedCard != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Selected Card", EditorStyles.boldLabel);
                EditorGUILayout.LabelField($"Name: {selectedCard.name}");
                EditorGUILayout.LabelField($"Category: {selectedCard.Category}");

                if (GUILayout.Button("Edit Card"))
                {
                    Selection.activeObject = selectedCard;
                }
            }

            EditorGUILayout.EndScrollView();
            GUI.EndGroup();
        }

        private void RefreshLayout()
        {
            if (clusterCollection == null)
                return;

            if (autoLayout)
            {
                PerformAutoLayout();
            }
            else
            {
                // Random positions if not auto-layout
                foreach (var cluster in clusterCollection.allClusters)
                {
                    if (cluster != null && !clusterPositions.ContainsKey(cluster))
                    {
                        clusterPositions[cluster] = GetRandomPosition();
                    }
                }
            }
        }

        private void PerformAutoLayout()
        {
            var clusters = clusterCollection.allClusters.Where(c => c != null).ToList();
            if (clusters.Count == 0)
                return;

            // Simple circular layout
            float radius = 200f;
            Vector2 center = new Vector2(300, 200);

            for (int i = 0; i < clusters.Count; i++)
            {
                float angle = (float)i / clusters.Count * Mathf.PI * 2;
                Vector2 pos = center + new Vector2(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius
                );
                clusterPositions[clusters[i]] = pos;
            }
        }

        private Vector2 GetRandomPosition()
        {
            return new Vector2(
                UnityEngine.Random.Range(50, 500),
                UnityEngine.Random.Range(50, 300)
            );
        }

        private Rect GetClusterRect(KnowledgeClusterData cluster, Vector2 pos)
        {
            return new Rect(pos.x - 60, pos.y - 30, 120, 60);
        }

        private Rect GetCardRect(CardData card, Vector2 pos)
        {
            return new Rect(pos.x - 25, pos.y - 10, 50, 20);
        }

        private Vector2 GetCardPositionInCluster(Vector2 clusterPos, int cardIndex, int totalCards)
        {
            if (totalCards == 1)
                return clusterPos;

            float angle = (float)cardIndex / totalCards * Mathf.PI * 2;
            float radius = 80f;

            return clusterPos + new Vector2(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius
            );
        }

        private Color GetConnectionTypeColor(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.Person:
                    return Color.blue;
                case ConnectionType.Location:
                    return Color.green;
                case ConnectionType.Material:
                    return Color.yellow;
                case ConnectionType.Historical:
                    return Color.red;
                case ConnectionType.Cultural:
                    return Color.magenta;
                case ConnectionType.Functional:
                    return Color.cyan;
                default:
                    return Color.white;
            }
        }

        private void DrawLine(Vector2 from, Vector2 to, Color color, float width)
        {
            Color oldColor = Handles.color;
            Handles.color = color;
            Handles.DrawAAPolyLine(width, from, to);
            Handles.color = oldColor;
        }

        private void FocusOnCluster(KnowledgeClusterData cluster)
        {
            if (clusterPositions.ContainsKey(cluster))
            {
                Vector2 clusterPos = clusterPositions[cluster];
                graphOffset = -clusterPos * zoomLevel + new Vector2(position.width * 0.3f, position.height * 0.5f);
            }
        }
    }
}
