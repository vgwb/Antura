using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Achievements
{
    [CreateAssetMenu(fileName = "CardDatabase", menuName = "Antura/Discover/Card Database (Master)")]
    public class CardDatabase : ScriptableObject
    {
        public CardCollectionByCountry[] Collections;

        // Built at runtime
        [NonSerialized] public Dictionary<string, CardDefinition> ById;
        [NonSerialized] public Dictionary<Antura.Discover.Countries, List<CardDefinition>> ByCountry;
    }
}
