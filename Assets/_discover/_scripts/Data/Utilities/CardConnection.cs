using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public enum TimeContextKind { Date, Period, Event }
    public enum CompareKind { Similar, Contrast, Analogy }
    public enum CulturalRole { Origin, Tradition, Symbol, Practice }
    public enum Directionality { Directed, Symmetric }

    public enum ConnectionType
    {
        CreatedBy = 1,       // agent (person/org) who made/discovered/commissioned
        LocatedIn = 2,       // where it is/was
        IsA = 3,             // type/category relation (Baguette is a Bread)
        PartOf = 4,          // whole/part relation (Crust is part of Baguette)
        MadeOf = 5,          // physical composition
        TimeContext = 6,     // period/event/date.
        CulturalContext = 7,  // origin/tradition/symbolism
        Causal = 8,           // clear cause→effect
        Purpose = 9,          // used for…
        Compare = 10,         // CompareKind similar/contrast/analogy
        RelatedTo = 11,       // catch-all
    }

    [System.Serializable]
    public class CardConnection
    {
        [Tooltip("Card connected to the core card")]
        public CardData ConnectedCard;

        [Tooltip("Type of relationship")]
        public ConnectionType ConnectionType = ConnectionType.IsA;

        [Range(0.1f, 1.0f)]
        [Tooltip("How strongly related (0.1 = weak, 1.0 = very strong)")]
        public float ConnectionStrength = 0.7f;

        [TextArea(1, 3)]
        [Tooltip("Why these cards are connected")]
        public string ConnectionReason;

        [Tooltip("Keywords that link these cards (search/filter)")]
        public List<string> LinkingKeywords = new List<string>();

        [Header("Educational Value")]
        [Tooltip("What children learn from this connection")]
        public string LearningValue;

        // --- Optional facets (used only for specific ConnectionTypes) ---
        [Tooltip("For TimeContext only: specify whether this is a Date, Period, or Event")]
        public TimeContextKind TimeKind = TimeContextKind.Period;

        [Tooltip("For Compare only: are we showing Similarity, Contrast, or Analogy?")]
        public CompareKind CompareKind = CompareKind.Similar;

        [Tooltip("For CulturalContext only: origin, tradition, symbol, or practice")]
        public CulturalRole CulturalRole = CulturalRole.Origin;

        public Directionality GetDirectionality()
        {
            switch (ConnectionType)
            {
                case ConnectionType.Causal:
                case ConnectionType.CreatedBy:
                case ConnectionType.LocatedIn:
                case ConnectionType.PartOf:
                case ConnectionType.MadeOf:
                case ConnectionType.Purpose:
                    return Directionality.Directed;
                default:
                    return Directionality.Symmetric;
            }
        }
    }
}
