using UnityEngine;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "CardsByCountryData", menuName = "Antura/Discover/Cards (by Country)")]
    public class CardsByCountryData : ScriptableObject
    {
        public Countries Country;
        public CardData[] Cards;
    }
}
