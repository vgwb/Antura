using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Discover
{

    [Serializable]
    public class TopicBridge
    {
        public TopicData From;
        public TopicData To;
        public CardData BridgeCard;
        public string BridgeReason;
        [Range(0.1f, 1.0f)]
        public float BridgeStrength = 0.5f;
    }

    // This is the "Knowledge Molecule" of the Discover module.
    // It contains a core card and a set of connected cards, each with a connection type and strength.
    [CreateAssetMenu(fileName = "TopicData", menuName = "Antura/Discover/Topic Data")]
    public class TopicData : IdentifiedData
    {
        [Header("Topic")]
        public string Name;
        [TextArea(2, 4)]
        public string Description;
        public Importance Importance = Importance.Medium;
        public Color NodeColor = Color.white;

        [Header("Core Topic")]
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
        [Tooltip("Age range this Topic is most suitable for")]
        public AgeRange targetAge = AgeRange.Ages6to10;

        [Tooltip("Subjects this Topic helps teach")]
        [FormerlySerializedAs("Topics")]
        public List<Subject> Subjects = new List<Subject>();

        // === RUNTIME METHODS ===

        /// <summary>
        /// Get all cards in this Topic including core card
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
        /// Check if Topic contains a specific card
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
        [ContextMenu("Validate Topic")]
        public void ValidateTopic()
        {
            var issues = new List<string>();

            if (string.IsNullOrEmpty(Name))
                issues.Add("Topic name is empty");

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
                Debug.LogWarning($"Topic '{Name}' has issues:\n- " + string.Join("\n- ", issues));
            }
            else
            {
                Debug.Log($"Topic '{Name}' validation passed!");
            }
        }
    }
}
