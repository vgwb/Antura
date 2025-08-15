using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "CardDatabase", menuName = "Antura/Discover/Card Database (Master)")]
    public class CardDatabaseData : ScriptableObject
    {
        public CardsByCountryData[] Collections;

        // Built at runtime
        [NonSerialized] public Dictionary<string, CardData> ById;
        [NonSerialized] public Dictionary<Countries, List<CardData>> ByCountry;
    }
}
