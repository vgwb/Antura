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
        private Vector2 scrollPos;                 // sidebar scroll
        private Vector2 graphScroll;               // graph scroll (content space)
        private Vector2 graphOffset = Vector2.zero; // legacy pan
        private float zoomLevel = 1.0f;
        private bool isDragging = false;
        private Vector2 lastMousePos;
        private Vector2 graphDrawOrigin = Vector2.zero; // top-left of content bounds

        // Graph layout
        private Dictionary<KnowledgeData, Vector2> clusterPositions = new Dictionary<KnowledgeData, Vector2>();
        private Dictionary<CardData, Vector2> cardPositions = new Dictionary<CardData, Vector2>();
        private List<CardData> uniqueCards = new List<CardData>();

        // Display options
        private bool showConnections = true;
        private bool showCardDetails = true;
        private bool showBridges = true;
        private bool autoLayout = true;
        private KnowledgeImportance filterPriority = KnowledgeImportance.Low;
        private int connectionTypeIndex = 0; // 0 = All, else enum selection

        private const string PrefKeyCollectionPath = "Antura.KnowledgeClusterGraphWindow.CollectionPath";

        // Selection
        private KnowledgeData selectedCluster;
        private CardData selectedCard;

        // Styling
        private GUIStyle clusterStyle;
        private GUIStyle cardStyle;
        private GUIStyle selectedStyle;
        private Dictionary<KnowledgeImportance, Color> priorityColors;

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
                var savedPath = EditorPrefs.GetString(PrefKeyCollectionPath, string.Empty);
                if (!string.IsNullOrEmpty(savedPath))
                {
                    clusterCollection = AssetDatabase.LoadAssetAtPath<KnowledgeCollectionData>(savedPath);
                }
                if (clusterCollection == null)
                {
                    string[] guids = AssetDatabase.FindAssets("t:KnowledgeClusterCollection");
                    if (guids.Length > 0)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        clusterCollection = AssetDatabase.LoadAssetAtPath<KnowledgeCollectionData>(path);
                    }
                }
                RefreshLayout();
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

            cardStyle = new GUIStyle(EditorStyles.miniButton)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 9,
                wordWrap = false,
                clipping = TextClipping.Clip,
                fixedHeight = 22
            };
            // Ensure readable contrast depending on skin
            cardStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;

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
            priorityColors = new Dictionary<KnowledgeImportance, Color>
            {
                { KnowledgeImportance.Critical, new Color(1f, 0.2f, 0.2f, 0.8f) },
                { KnowledgeImportance.High, new Color(1f, 0.8f, 0.2f, 0.8f) },
                { KnowledgeImportance.Medium, new Color(0.2f, 0.8f, 0.2f, 0.8f) },
                { KnowledgeImportance.Low, new Color(0.6f, 0.6f, 0.6f, 0.8f) }
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

            EditorGUI.BeginChangeCheck();
            var newCollection = (KnowledgeCollectionData)EditorGUILayout.ObjectField(
                clusterCollection, typeof(KnowledgeCollectionData), false, GUILayout.Width(220));
            if (EditorGUI.EndChangeCheck())
            {
                clusterCollection = newCollection;
                var p = clusterCollection ? AssetDatabase.GetAssetPath(clusterCollection) : string.Empty;
                EditorPrefs.SetString(PrefKeyCollectionPath, p);
                RefreshLayout();
            }

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
            filterPriority = (KnowledgeImportance)EditorGUILayout.EnumPopup(filterPriority, EditorStyles.toolbarPopup, GUILayout.Width(80));

            GUILayout.Space(10);
            // Connection type filter popup
            string[] connOptions = BuildConnectionTypeOptions();
            connectionTypeIndex = EditorGUILayout.Popup(connectionTypeIndex, connOptions, EditorStyles.toolbarPopup, GUILayout.Width(160));

            GUILayout.FlexibleSpace();

            EditorGUILayout.LabelField($"Zoom: {zoomLevel:F2}", GUILayout.Width(80));

            EditorGUILayout.EndHorizontal();
        }

        private void HandleInput()
        {
            Event e = Event.current;

            Rect graphArea = new Rect(0, EditorGUIUtility.singleLineHeight + 5, position.width - 250, position.height - EditorGUIUtility.singleLineHeight - 5);

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                if (!graphArea.Contains(e.mousePosition))
                    return;
                Vector2 local = e.mousePosition - new Vector2(graphArea.x, graphArea.y);
                Vector2 mousePos = (local / zoomLevel) + graphScroll;

                // Check cluster selection
                foreach (var kvp in clusterPositions)
                {
                    Rect clusterRect = GetClusterRect(kvp.Key, kvp.Value - graphDrawOrigin);
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
                        Rect cardRect = GetCardRect(kvp.Key, kvp.Value - graphDrawOrigin);
                        if (cardRect.Contains(mousePos))
                        {
                            selectedCard = kvp.Key;
                            selectedCluster = clusterCollection.FindKnowledgeForCard(kvp.Key);
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
                graphScroll -= delta / Mathf.Max(0.01f, zoomLevel);
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
            var bounds = ComputeGraphBounds();
            graphDrawOrigin = new Vector2(bounds.xMin, bounds.yMin);
            var contentSize = new Vector2(Mathf.Max(bounds.width, graphArea.width), Mathf.Max(bounds.height, graphArea.height));
            graphScroll = GUI.BeginScrollView(graphArea, graphScroll, new Rect(0, 0, contentSize.x, contentSize.y));

            Matrix4x4 oldMatrix = GUI.matrix;
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * zoomLevel);

            // Draw bridges first (behind everything)
            if (showBridges)
            {
                DrawClusterBridges();
            }

            // Draw connections
            if (showConnections)
            {
                DrawCardConnections();
                DrawClusterCardLinks();
            }

            // Draw clusters
            DrawClusters();

            // Draw cards
            if (showCardDetails)
            {
                DrawCards();
            }

            GUI.matrix = oldMatrix;
            GUI.EndScrollView();
        }

        private void DrawClusters()
        {
            foreach (var cluster in clusterCollection.AllKnowledges)
            {
                if (cluster == null || cluster.Importance > filterPriority)
                    continue;

                if (!clusterPositions.ContainsKey(cluster))
                    clusterPositions[cluster] = GetRandomPosition();

                Vector2 pos = clusterPositions[cluster];
                Rect rect = GetClusterRect(cluster, pos - graphDrawOrigin);

                // Set color based on priority
                Color oldColor = GUI.backgroundColor;
                GUI.backgroundColor = priorityColors[cluster.Importance];

                GUIStyle style = (cluster == selectedCluster) ? selectedStyle : clusterStyle;

                GUI.Box(rect, cluster.Name, style);

                // Draw priority indicator
                Rect priorityRect = new Rect(rect.x, rect.y - 15, rect.width, 12);
                GUI.Label(priorityRect, cluster.Importance.ToString(), EditorStyles.miniLabel);

                GUI.backgroundColor = oldColor;
            }
        }

        private void DrawCards()
        {
            foreach (var card in uniqueCards)
            {
                if (card == null)
                    continue;
                if (!cardPositions.TryGetValue(card, out var cardPos))
                    continue;

                Rect rect = GetCardRect(card, cardPos - graphDrawOrigin);

                Color oldColor = GUI.backgroundColor;
                if (card == selectedCard)
                    GUI.backgroundColor = Color.yellow;

                string label = !string.IsNullOrEmpty(card.Id) ? card.Id : card.name;
                if (GUI.Button(rect, label, cardStyle))
                {
                    selectedCard = card;
                    // Prefer to focus on one of its clusters
                    selectedCluster = clusterCollection.FindKnowledgeForCard(card);
                }

                GUI.backgroundColor = oldColor;
            }
        }

        private void DrawCardConnections()
        {
            foreach (var cluster in clusterCollection.AllKnowledges)
            {
                if (cluster == null || cluster.Importance > filterPriority)
                    continue;
                if (cluster.CoreCard == null)
                    continue;

                Vector2 corePos = Vector2.zero;
                if (cardPositions.ContainsKey(cluster.CoreCard))
                    corePos = cardPositions[cluster.CoreCard];

                foreach (var connection in cluster.Connections)
                {
                    if (connection.connectedCard == null)
                        continue;
                    if (!cardPositions.ContainsKey(connection.connectedCard))
                        continue;

                    Vector2 connectedPos = cardPositions[connection.connectedCard];
                    if (connectionTypeIndex > 0)
                    {
                        var types = (ConnectionType[])Enum.GetValues(typeof(ConnectionType));
                        var filterType = types[Mathf.Clamp(connectionTypeIndex - 1, 0, types.Length - 1)];
                        if (connection.connectionType != filterType)
                            continue;
                    }
                    Color connectionColor = GetConnectionTypeColor(connection.connectionType);
                    connectionColor.a = connection.connectionStrength;

                    DrawLine(corePos - graphDrawOrigin, connectedPos - graphDrawOrigin, connectionColor, connection.connectionStrength * 3f);
                }
            }
        }

        private void DrawClusterBridges()
        {
            foreach (var bridge in clusterCollection.Bridges)
            {
                if (bridge.From == null || bridge.To == null)
                    continue;
                if (bridge.From.Importance > filterPriority || bridge.To.Importance > filterPriority)
                    continue;

                if (!clusterPositions.ContainsKey(bridge.From) || !clusterPositions.ContainsKey(bridge.To))
                    continue;

                Vector2 fromPos = clusterPositions[bridge.From] - graphDrawOrigin;
                Vector2 toPos = clusterPositions[bridge.To] - graphDrawOrigin;

                DrawLine(fromPos, toPos, Color.magenta, bridge.BridgeStrength * 5f);
            }
        }

        private void DrawSidebar()
        {
            GUI.contentColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            Rect sidebarArea = new Rect(position.width - 250, EditorGUIUtility.singleLineHeight + 5, 250, position.height - EditorGUIUtility.singleLineHeight - 5);

            GUI.BeginGroup(sidebarArea, EditorStyles.helpBox);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            EditorGUILayout.LabelField("Graph Statistics", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Total Clusters: {clusterCollection.AllKnowledges.Count}");
            EditorGUILayout.LabelField($"Visible Clusters: {clusterCollection.AllKnowledges.Count(c => c != null && c.Importance <= filterPriority)}");
            EditorGUILayout.LabelField($"Bridges: {clusterCollection.Bridges.Count}");

            EditorGUILayout.Space();

            if (selectedCluster != null)
            {
                EditorGUILayout.LabelField("Selected Cluster", EditorStyles.boldLabel);
                EditorGUILayout.LabelField($"Name: {selectedCluster.Name}");
                EditorGUILayout.LabelField($"Priority: {selectedCluster.Importance}");
                EditorGUILayout.LabelField($"Cards: {selectedCluster.GetAllCards().Count}");
                EditorGUILayout.LabelField($"Cohesion: {selectedCluster.cohesionStrength:F2}");

                if (selectedCluster.CoreCard != null)
                {
                    EditorGUILayout.LabelField($"Core: {selectedCluster.CoreCard.name}");
                }

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Connections:", EditorStyles.boldLabel);
                foreach (var conn in selectedCluster.Connections)
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
                EditorGUILayout.LabelField($"Category: {selectedCard.Type}");

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
                foreach (var cluster in clusterCollection.AllKnowledges)
                {
                    if (cluster != null && !clusterPositions.ContainsKey(cluster))
                    {
                        clusterPositions[cluster] = GetRandomPosition();
                    }
                }
                LayoutCardsUnique();
            }
        }

        private void PerformAutoLayout()
        {
            var clusters = clusterCollection.AllKnowledges.Where(c => c != null).ToList();
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

            // Lay out unique cards on a larger ring
            LayoutCardsUnique(center, radius * 1.6f);
        }

        private void LayoutCardsUnique()
        {
            LayoutCardsUnique(new Vector2(300, 200), 320f);
        }

        private void LayoutCardsUnique(Vector2 center, float radius)
        {
            // Build unique set
            var set = new HashSet<CardData>();
            foreach (var cl in clusterCollection.AllKnowledges)
            {
                if (cl == null)
                    continue;
                var cards = cl.GetAllCards();
                foreach (var c in cards)
                    if (c != null)
                        set.Add(c);
            }
            uniqueCards = set.ToList();

            // Position cards on a ring
            cardPositions.Clear();
            for (int i = 0; i < uniqueCards.Count; i++)
            {
                float angle = (float)i / Math.Max(1, uniqueCards.Count) * Mathf.PI * 2;
                Vector2 pos = center + new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
                cardPositions[uniqueCards[i]] = pos;
            }
        }

        private Vector2 GetRandomPosition()
        {
            return new Vector2(
                UnityEngine.Random.Range(50, 500),
                UnityEngine.Random.Range(50, 300)
            );
        }

        private Rect GetClusterRect(KnowledgeData cluster, Vector2 pos)
        {
            return new Rect(pos.x - 60, pos.y - 30, 120, 60);
        }

        private Rect GetCardRect(CardData card, Vector2 pos)
        {
            return new Rect(pos.x - 50, pos.y - 11, 100, 22);
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
                case ConnectionType.Temporal:
                    return new Color(1f, 0.5f, 0f); // orange
                case ConnectionType.Causal:
                    return new Color(0.6f, 0.2f, 1f); // purple-ish
                case ConnectionType.Functional:
                    return Color.cyan;
                case ConnectionType.Conceptual:
                    return new Color(0.2f, 0.8f, 0.8f);
                case ConnectionType.Comparison:
                    return new Color(0.8f, 0.8f, 0.2f);
                default:
                    return Color.white;
            }
        }

        private void DrawClusterCardLinks()
        {
            foreach (var cluster in clusterCollection.AllKnowledges)
            {
                if (cluster == null || cluster.Importance > filterPriority)
                    continue;
                if (!clusterPositions.TryGetValue(cluster, out var cPos))
                    continue;

                var cards = cluster.GetAllCards();
                foreach (var card in cards)
                {
                    if (card == null)
                        continue;
                    if (!cardPositions.TryGetValue(card, out var kPos))
                        continue;
                    DrawLine(cPos - graphDrawOrigin, kPos - graphDrawOrigin, Color.gray, 1.5f);
                }
            }
        }

        private Rect ComputeGraphBounds()
        {
            float minX = float.PositiveInfinity, minY = float.PositiveInfinity;
            float maxX = float.NegativeInfinity, maxY = float.NegativeInfinity;

            void Enc(Rect r)
            {
                minX = Mathf.Min(minX, r.xMin);
                minY = Mathf.Min(minY, r.yMin);
                maxX = Mathf.Max(maxX, r.xMax);
                maxY = Mathf.Max(maxY, r.yMax);
            }

            foreach (var kv in clusterPositions)
            {
                if (kv.Key == null)
                    continue;
                Enc(new Rect(kv.Value.x - 60, kv.Value.y - 30, 120, 60));
            }
            foreach (var kv in cardPositions)
            {
                if (kv.Key == null)
                    continue;
                Enc(new Rect(kv.Value.x - 50, kv.Value.y - 11, 100, 22));
            }

            if (!float.IsFinite(minX) || !float.IsFinite(minY) || !float.IsFinite(maxX) || !float.IsFinite(maxY))
            {
                minX = minY = 0f;
                maxX = maxY = 1000f;
            }

            const float margin = 200f;
            return Rect.MinMaxRect(minX - margin, minY - margin, maxX + margin, maxY + margin);
        }

        private static string[] BuildConnectionTypeOptions()
        {
            var names = Enum.GetNames(typeof(ConnectionType));
            var list = new List<string>(names.Length + 1) { "Connections: All" };
            foreach (var n in names)
                list.Add($"Type: {n}");
            return list.ToArray();
        }

        private void DrawLine(Vector2 from, Vector2 to, Color color, float width)
        {
            Color oldColor = Handles.color;
            Handles.color = color;
            Handles.DrawAAPolyLine(width, from, to);
            Handles.color = oldColor;
        }

        private void FocusOnCluster(KnowledgeData cluster)
        {
            if (clusterPositions.ContainsKey(cluster))
            {
                Vector2 clusterPos = clusterPositions[cluster] - graphDrawOrigin;
                Rect graphArea = new Rect(0, EditorGUIUtility.singleLineHeight + 5, position.width - 250, position.height - EditorGUIUtility.singleLineHeight - 5);
                Vector2 viewportCenter = new Vector2(graphArea.width, graphArea.height) * 0.5f / Mathf.Max(0.01f, zoomLevel);
                graphScroll = clusterPos - viewportCenter;
                graphScroll.x = Mathf.Max(0, graphScroll.x);
                graphScroll.y = Mathf.Max(0, graphScroll.y);
            }
        }
    }
}
