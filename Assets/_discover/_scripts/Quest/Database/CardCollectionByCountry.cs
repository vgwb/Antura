using UnityEngine;

namespace Antura.Discover.Achievements
{
    [CreateAssetMenu(fileName = "CardCollectionByCountry", menuName = "Antura/Discover/Card Collection (Country)")]
    public class CardCollectionByCountry : ScriptableObject
    {
        public Countries Country;
        public CardDefinition[] Cards;
    }
}
