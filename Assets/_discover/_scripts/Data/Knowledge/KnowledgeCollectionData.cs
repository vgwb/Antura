using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "KnowledgeCollectionData", menuName = "Antura/Discover/Knowledge Collection")]
    public class KnowledgeCollectionData : ScriptableObject
    {
        [Header("Knowledge Organization")]
        public List<KnowledgeData> AllKnowledges = new List<KnowledgeData>();

        [Header("Cross-Knowledge Connections")]
        public List<KnowledgeBridge> Bridges = new List<KnowledgeBridge>();

        // === QUERY METHODS ===

        /// <summary>
        /// Find which knowledge contains a specific card
        /// </summary>
        public KnowledgeData FindKnowledgeForCard(CardData card)
        {
            foreach (var knowledge in AllKnowledges)
            {
                if (knowledge.ContainsCard(card))
                    return knowledge;
            }
            return null;
        }

        /// <summary>
        /// Get Knowledges by priority level
        /// </summary>
        public List<KnowledgeData> knowledge(KnowledgeImportance priority)
        {
            var result = new List<KnowledgeData>();
            foreach (var knowledge in AllKnowledges)
            {
                if (knowledge.Importance == priority)
                    result.Add(knowledge);
            }
            return result;
        }

        /// <summary>
        /// Get all cards that are discoverable based on known cards
        /// </summary>
        public List<CardData> GetDiscoverableCards(List<CardData> knownCards, KnowledgeImportance maxPriority = KnowledgeImportance.Low)
        {
            var discoverable = new HashSet<CardData>();

            foreach (var knowledge in AllKnowledges)
            {
                if (knowledge.Importance > maxPriority)
                    continue;

                // If player knows core card, they can discover connected cards
                if (knownCards.Contains(knowledge.CoreCard))
                {
                    foreach (var connection in knowledge.Connections)
                    {
                        if (connection.connectedCard != null && !knownCards.Contains(connection.connectedCard))
                            discoverable.Add(connection.connectedCard);
                    }
                }
            }

            return new List<CardData>(discoverable);
        }

        /// <summary>
        /// Get related knowledges through bridges
        /// </summary>
        public List<KnowledgeData> GetRelatedKnowledges(KnowledgeData knowledge)
        {
            var related = new List<KnowledgeData>();

            foreach (var bridge in Bridges)
            {
                if (bridge.From == knowledge && bridge.To != null)
                    related.Add(bridge.To);
                else if (bridge.To == knowledge && bridge.From != null)
                    related.Add(bridge.From);
            }

            return related;
        }

        [ContextMenu("Validate All Knowledges")]
        public void ValidateAllKnowledges()
        {
            Debug.Log($"Validating {AllKnowledges.Count} Knowledges...");

            int issueCount = 0;
            foreach (var knowledge in AllKnowledges)
            {
                if (knowledge != null)
                {
                    knowledge.ValidateKnowledge();
                }
                else
                {
                    Debug.LogError("Null Knowledge found in collection!");
                    issueCount++;
                }
            }

            Debug.Log($"Knowledge validation complete. Check console for any issues.");
        }
    }
}
