using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "TopicCollectionData", menuName = "Antura/Discover/Topic Collection")]
    public class TopicCollectionData : ScriptableObject
    {
        [Header("Topics Organization")]
        [FormerlySerializedAs("AllKnowledges")]
        public List<TopicData> AllTopics = new List<TopicData>();

        [Header("Cross-Topics Connections")]
        public List<TopicBridge> Bridges = new List<TopicBridge>();

        // === QUERY METHODS ===

        /// <summary>
        /// Find which knowledge contains a specific card
        /// </summary>
        public TopicData FindKnowledgeForCard(CardData card)
        {
            foreach (var topic in AllTopics)
            {
                if (topic.ContainsCard(card))
                    return topic;
            }
            return null;
        }

        /// <summary>
        /// Get Topics by priority level
        /// </summary>
        public List<TopicData> topic(Importance priority)
        {
            var result = new List<TopicData>();
            foreach (var topic in AllTopics)
            {
                if (topic.Importance == priority)
                    result.Add(topic);
            }
            return result;
        }

        /// <summary>
        /// Get all cards that are discoverable based on known cards
        /// </summary>
        public List<CardData> GetDiscoverableCards(List<CardData> knownCards, Importance maxPriority = Importance.Low)
        {
            var discoverable = new HashSet<CardData>();

            foreach (var topic in AllTopics)
            {
                if (topic.Importance > maxPriority)
                    continue;

                // If player knows core card, they can discover connected cards
                if (knownCards.Contains(topic.CoreCard))
                {
                    foreach (var connection in topic.Connections)
                    {
                        if (connection.ConnectedCard != null && !knownCards.Contains(connection.ConnectedCard))
                            discoverable.Add(connection.ConnectedCard);
                    }
                }
            }

            return new List<CardData>(discoverable);
        }

        /// <summary>
        /// Get related knowledges through bridges
        /// </summary>
        public List<TopicData> GetRelatedKnowledges(TopicData knowledge)
        {
            var related = new List<TopicData>();

            foreach (var bridge in Bridges)
            {
                if (bridge.From == knowledge && bridge.To != null)
                    related.Add(bridge.To);
                else if (bridge.To == knowledge && bridge.From != null)
                    related.Add(bridge.From);
            }

            return related;
        }

        [ContextMenu("Validate All Topics")]
        public void ValidateAllTopics()
        {
            Debug.Log($"Validating {AllTopics.Count} Topics...");

            int issueCount = 0;
            foreach (var topic in AllTopics)
            {
                if (topic != null)
                {
                    topic.ValidateTopic();
                }
                else
                {
                    Debug.LogError("Null Topic found in collection!");
                    issueCount++;
                }
            }

            Debug.Log($"Topic validation complete. Check console for any issues.");
        }
    }
}
