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
    [CreateAssetMenu(fileName = "TopicData", menuName = "Antura/Discover Data/Topic Data")]
    public class TopicData : IdentifiedData
    {
        [Header("Topic")]
        public string Name;
        [TextArea(2, 4)]
        public string Description;
        public Importance Importance = Importance.Medium;
        public Color NodeColor = Color.white;
        public Countries Country;

        [Header("Core Topic")]
        [Tooltip("The main card that represents this theme")]
        public CardData CoreCard;

        [Header("Connected Cards")]
        public List<CardConnection> Connections = new List<CardConnection>();

        // Discovery path removed

        [Header("Settings")]
        [Range(0.1f, 1.0f)]
        [Tooltip("How tightly related cards")]
        public float CohesionStrength = 0.8f;

        [Header("Educational Context")]
        [Tooltip("Age range this Topic is most suitable for")]
        public AgeRange TargetAge = AgeRange.Ages6to10;

        [Tooltip("Subjects this Topic helps teach")]
        [FormerlySerializedAs("Topics")]
        public List<Subject> Subjects = new List<Subject>();

        [Header("Authoring Metadata")]
        public List<AuthorCredit> Credits;

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
                if (connection.ConnectedCard != null)
                    allCards.Add(connection.ConnectedCard);
            }

            // Discovery path removed

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
                if (connection.ConnectionType == type && connection.ConnectedCard != null)
                    result.Add(connection.ConnectedCard);
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
                if (connection.ConnectedCard == card)
                    return connection.ConnectionStrength;
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
                if (connection.ConnectedCard == card)
                    return true;
            }

            return false;
        }

        // Discovery path helpers removed

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
                if (connection.ConnectedCard != null)
                {
                    if (cardSet.Contains(connection.ConnectedCard))
                        issues.Add($"Duplicate connection to {connection.ConnectedCard.name}");
                    else
                        cardSet.Add(connection.ConnectedCard);
                }
            }

            // Discovery path checks removed

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
