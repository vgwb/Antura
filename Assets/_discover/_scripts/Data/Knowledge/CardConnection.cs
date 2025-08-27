using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public enum ConnectionType
    {
        Person = 1,          // Eiffel Tower -> Gustave Eiffel
        Location = 2,        // Eiffel Tower -> Paris
        Material = 3,        // Eiffel Tower -> Iron
        Historical = 4,      // Eiffel Tower -> 1889 World's Fair
        Cultural = 5,        // Paris -> French Culture
        Temporal = 6,        // Same time period
        Causal = 7,          // One caused the other
        Functional = 8,      // Same purpose/function
        Conceptual = 9,      // Abstract relationship
        Comparison = 10      // Comparing similar things
    }

    public enum ClusterPriority
    {
        Critical = 1,       // Essential for survival/safety
        High = 2,           // Important for integration
        Medium = 3,         // Valuable cultural knowledge
        Low = 4             // Nice to know
    }

    public enum AgeRange
    {
        Ages3to5 = 1,       // Preschool
        Ages6to10 = 2,      // Primary school
        Ages11to15 = 3,     // Secondary school
        Ages16Plus = 4      // Adult learning
    }

    [System.Serializable]
    public class CardConnection
    {
        [Tooltip("Card connected to the core card")]
        public CardData connectedCard;

        [Tooltip("Type of relationship")]
        public ConnectionType connectionType = ConnectionType.Conceptual;

        [Range(0.1f, 1.0f)]
        [Tooltip("How strongly related (0.1 = weak, 1.0 = very strong)")]
        public float connectionStrength = 0.7f;

        [TextArea(1, 3)]
        [Tooltip("Why these cards are connected")]
        public string connectionReason;

        [Tooltip("Keywords that link these cards")]
        public List<string> linkingKeywords = new List<string>();

        [Header("Educational Value")]
        [Tooltip("What children learn from this connection")]
        public string learningValue;
    }

}
