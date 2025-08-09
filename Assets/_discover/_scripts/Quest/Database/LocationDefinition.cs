using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "LocationDefinition", menuName = "Antura/Discover/Location")]
    public class LocationDefinition : ScriptableObject
    {
        public LocalizedString Name;
        public string Url;
        public Countries Country;
    }
}
