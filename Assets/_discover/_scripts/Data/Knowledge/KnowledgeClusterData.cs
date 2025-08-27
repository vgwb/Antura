using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{

    [Serializable]
    public class ClusterBridge
    {
        public KnowledgeClusterData fromCluster;
        public KnowledgeClusterData toCluster;
        public CardData bridgeCard;
        public string bridgeReason;
        [Range(0.1f, 1.0f)]
        public float bridgeStrength = 0.5f;
    }

    [CreateAssetMenu(fileName = "KnowledgeCluster", menuName = "Antura/Discover/Knowledge Cluster")]
    public class KnowledgeClusterData : ScriptableObject
    {
        [Header("Cluster Identity")]
        public string clusterName;
        [TextArea(2, 4)]
        public string description;
        public ClusterPriority priority = ClusterPriority.Medium;
        public Color clusterColor = Color.white;

        [Header("Core Knowledge")]
        [Tooltip("The main card that represents this theme")]
        public CardData coreCard;

        [Header("Connected Cards")]
        public List<CardConnection> connections = new List<CardConnection>();

        [Header("Discovery Flow")]
        [Tooltip("Suggested order for discovering related cards")]
        public List<CardData> discoveryPath = new List<CardData>();

        [Header("Settings")]
        [Range(0.1f, 1.0f)]
        [Tooltip("How tightly related cards in this cluster are")]
        public float cohesionStrength = 0.8f;

        [Header("Educational Context")]
        [Tooltip("Age range this cluster is most suitable for")]
        public AgeRange targetAge = AgeRange.Ages6to10;

        [Tooltip("Topics this cluster helps teach")]
        public List<KnowledgeTopic> educationalTopics = new List<KnowledgeTopic>();

        // === RUNTIME METHODS ===

        /// <summary>
        /// Get all cards in this cluster including core card
        /// </summary>
        public List<CardData> GetAllCards()
        {
            var allCards = new HashSet<CardData>();

            if (coreCard != null)
                allCards.Add(coreCard);

            foreach (var connection in connections)
            {
                if (connection.connectedCard != null)
                    allCards.Add(connection.connectedCard);
            }

            foreach (var card in discoveryPath)
            {
                if (card != null)
                    allCards.Add(card);
            }

            return new List<CardData>(allCards);
        }

        /// <summary>
        /// Get cards by connection type
        /// </summary>
        public List<CardData> GetCardsByConnectionType(ConnectionType type)
        {
            var result = new List<CardData>();
            foreach (var connection in connections)
            {
                if (connection.connectionType == type && connection.connectedCard != null)
                    result.Add(connection.connectedCard);
            }
            return result;
        }

        /// <summary>
        /// Get connection strength to a specific card
        /// </summary>
        public float GetConnectionStrength(CardData card)
        {
            if (card == coreCard)
                return 1.0f;

            foreach (var connection in connections)
            {
                if (connection.connectedCard == card)
                    return connection.connectionStrength;
            }
            return 0f;
        }

        /// <summary>
        /// Check if cluster contains a specific card
        /// </summary>
        public bool ContainsCard(CardData card)
        {
            if (coreCard == card)
                return true;

            foreach (var connection in connections)
            {
                if (connection.connectedCard == card)
                    return true;
            }

            return discoveryPath.Contains(card);
        }

        /// <summary>
        /// Get next suggested card in discovery path
        /// </summary>
        public CardData GetNextInDiscoveryPath(CardData currentCard)
        {
            int currentIndex = discoveryPath.IndexOf(currentCard);
            if (currentIndex >= 0 && currentIndex < discoveryPath.Count - 1)
            {
                return discoveryPath[currentIndex + 1];
            }
            return null;
        }

        // === VALIDATION ===
        [ContextMenu("Validate Cluster")]
        public void ValidateCluster()
        {
            var issues = new List<string>();

            if (string.IsNullOrEmpty(clusterName))
                issues.Add("Cluster name is empty");

            if (coreCard == null)
                issues.Add("No core card assigned");

            if (connections.Count == 0)
                issues.Add("No connections defined");

            // Check for duplicate connections
            var cardSet = new HashSet<CardData>();
            foreach (var connection in connections)
            {
                if (connection.connectedCard != null)
                {
                    if (cardSet.Contains(connection.connectedCard))
                        issues.Add($"Duplicate connection to {connection.connectedCard.name}");
                    else
                        cardSet.Add(connection.connectedCard);
                }
            }

            // Check discovery path validity
            foreach (var card in discoveryPath)
            {
                if (card != null && !ContainsCard(card))
                    issues.Add($"Discovery path contains unconnected card: {card.name}");
            }

            if (issues.Count > 0)
            {
                Debug.LogWarning($"Cluster '{clusterName}' has issues:\n- " + string.Join("\n- ", issues));
            }
            else
            {
                Debug.Log($"Cluster '{clusterName}' validation passed!");
            }
        }
    }
}
