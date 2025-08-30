using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{

    [Serializable]
    public class KnowledgeBridge
    {
        public KnowledgeData From;
        public KnowledgeData To;
        public CardData BridgeCard;
        public string BridgeReason;
        [Range(0.1f, 1.0f)]
        public float BridgeStrength = 0.5f;
    }

    [CreateAssetMenu(fileName = "KnowledgeData", menuName = "Antura/Discover/Knowledge Data")]
    public class KnowledgeData : IdentifiedData
    {
        [Header("Knowledge")]
        public string Name;
        [TextArea(2, 4)]
        public string Description;
        public KnowledgeImportance Importance = KnowledgeImportance.Medium;
        public Color NodeColor = Color.white;

        [Header("Core Knowledge")]
        [Tooltip("The main card that represents this theme")]
        public CardData CoreCard;

        [Header("Connected Cards")]
        public List<CardConnection> Connections = new List<CardConnection>();

        [Header("Discovery Flow")]
        [Tooltip("Suggested order for discovering related cards")]
        public List<CardData> DiscoveryPath = new List<CardData>();

        [Header("Settings")]
        [Range(0.1f, 1.0f)]
        [Tooltip("How tightly related cards")]
        public float cohesionStrength = 0.8f;

        [Header("Educational Context")]
        [Tooltip("Age range this Knowledge is most suitable for")]
        public AgeRange targetAge = AgeRange.Ages6to10;

        [Tooltip("Topics this Knowledge helps teach")]
        public List<KnowledgeTopic> Topics = new List<KnowledgeTopic>();

        // === RUNTIME METHODS ===

        /// <summary>
        /// Get all cards in this Knowledge including core card
        /// </summary>
        public List<CardData> GetAllCards()
        {
            var allCards = new HashSet<CardData>();

            if (CoreCard != null)
                allCards.Add(CoreCard);

            foreach (var connection in Connections)
            {
                if (connection.connectedCard != null)
                    allCards.Add(connection.connectedCard);
            }

            foreach (var card in DiscoveryPath)
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
            foreach (var connection in Connections)
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
            if (card == CoreCard)
                return 1.0f;

            foreach (var connection in Connections)
            {
                if (connection.connectedCard == card)
                    return connection.connectionStrength;
            }
            return 0f;
        }

        /// <summary>
        /// Check if Knowledge contains a specific card
        /// </summary>
        public bool ContainsCard(CardData card)
        {
            if (CoreCard == card)
                return true;

            foreach (var connection in Connections)
            {
                if (connection.connectedCard == card)
                    return true;
            }

            return DiscoveryPath.Contains(card);
        }

        /// <summary>
        /// Get next suggested card in discovery path
        /// </summary>
        public CardData GetNextInDiscoveryPath(CardData currentCard)
        {
            int currentIndex = DiscoveryPath.IndexOf(currentCard);
            if (currentIndex >= 0 && currentIndex < DiscoveryPath.Count - 1)
            {
                return DiscoveryPath[currentIndex + 1];
            }
            return null;
        }

        // === VALIDATION ===
        [ContextMenu("Validate Knowledge")]
        public void ValidateKnowledge()
        {
            var issues = new List<string>();

            if (string.IsNullOrEmpty(Name))
                issues.Add("Knowledge name is empty");

            if (CoreCard == null)
                issues.Add("No core card assigned");

            if (Connections.Count == 0)
                issues.Add("No connections defined");

            // Check for duplicate connections
            var cardSet = new HashSet<CardData>();
            foreach (var connection in Connections)
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
            foreach (var card in DiscoveryPath)
            {
                if (card != null && !ContainsCard(card))
                    issues.Add($"Discovery path contains unconnected card: {card.name}");
            }

            if (issues.Count > 0)
            {
                Debug.LogWarning($"Knowledge '{Name}' has issues:\n- " + string.Join("\n- ", issues));
            }
            else
            {
                Debug.Log($"Knowledge '{Name}' validation passed!");
            }
        }
    }
}
