using Antura.Discover.Activities;
using UnityEngine;
using UnityEngine.Localization;

namespace Antura.Discover
{
    /// <summary>
    /// Basic information about a country
    /// </summary>
    [CreateAssetMenu(fileName = "CountryData", menuName = "Antura/Discover/Country Data")]
    public class CountryData : ScriptableObject
    {
        public Countries CountryId;

        public LocalizedString CountryName;

        [Header("Facts")]
        public LocationData Capital;

        public MoneySet Currency;

        [Tooltip("Population estimate (in millions, or full number).")]
        public int Population;

        [Header("Visuals")]
        [Tooltip("Flag image for this country.")]
        public Sprite Flag;

        [Tooltip("Optional background or map image.")]
        public Sprite BackgroundImage;
    }
}
