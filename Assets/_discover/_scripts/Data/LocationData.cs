using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "LocationData", menuName = "Antura/Discover/Location")]
    public class LocationData : IdentifiedData
    {
        public LocalizedString Name;
        public string Url;
        public Countries Country;
    }
}
