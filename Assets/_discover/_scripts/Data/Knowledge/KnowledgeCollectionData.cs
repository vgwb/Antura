using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "KnowledgeCollectionData", menuName = "Antura/Discover/Cluster Collection")]
    public class KnowledgeCollectionData : ScriptableObject
    {
        [Header("Cluster Organization")]
        public List<KnowledgeClusterData> allClusters = new List<KnowledgeClusterData>();

        [Header("Cross-Cluster Connections")]
        public List<ClusterBridge> bridges = new List<ClusterBridge>();

        // === QUERY METHODS ===

        /// <summary>
        /// Find which cluster contains a specific card
        /// </summary>
        public KnowledgeClusterData FindClusterForCard(CardData card)
        {
            foreach (var cluster in allClusters)
            {
                if (cluster.ContainsCard(card))
                    return cluster;
            }
            return null;
        }

        /// <summary>
        /// Get clusters by priority level
        /// </summary>
        public List<KnowledgeClusterData> GetClustersByPriority(ClusterPriority priority)
        {
            var result = new List<KnowledgeClusterData>();
            foreach (var cluster in allClusters)
            {
                if (cluster.priority == priority)
                    result.Add(cluster);
            }
            return result;
        }

        /// <summary>
        /// Get all cards that are discoverable based on known cards
        /// </summary>
        public List<CardData> GetDiscoverableCards(List<CardData> knownCards, ClusterPriority maxPriority = ClusterPriority.Low)
        {
            var discoverable = new HashSet<CardData>();

            foreach (var cluster in allClusters)
            {
                if (cluster.priority > maxPriority)
                    continue;

                // If player knows core card, they can discover connected cards
                if (knownCards.Contains(cluster.coreCard))
                {
                    foreach (var connection in cluster.connections)
                    {
                        if (connection.connectedCard != null && !knownCards.Contains(connection.connectedCard))
                            discoverable.Add(connection.connectedCard);
                    }
                }
            }

            return new List<CardData>(discoverable);
        }

        /// <summary>
        /// Get related clusters through bridges
        /// </summary>
        public List<KnowledgeClusterData> GetRelatedClusters(KnowledgeClusterData cluster)
        {
            var related = new List<KnowledgeClusterData>();

            foreach (var bridge in bridges)
            {
                if (bridge.fromCluster == cluster && bridge.toCluster != null)
                    related.Add(bridge.toCluster);
                else if (bridge.toCluster == cluster && bridge.fromCluster != null)
                    related.Add(bridge.fromCluster);
            }

            return related;
        }

        [ContextMenu("Validate All Clusters")]
        public void ValidateAllClusters()
        {
            Debug.Log($"Validating {allClusters.Count} clusters...");

            int issueCount = 0;
            foreach (var cluster in allClusters)
            {
                if (cluster != null)
                {
                    cluster.ValidateCluster();
                }
                else
                {
                    Debug.LogError("Null cluster found in collection!");
                    issueCount++;
                }
            }

            Debug.Log($"Cluster validation complete. Check console for any issues.");
        }
    }
}
