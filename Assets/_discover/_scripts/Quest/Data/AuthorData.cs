using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "AuthorData", menuName = "Antura/Discover/Author")]
    public class AuthorDefinition : ScriptableObject
    {
        public string Name;
        public string Url;
        public Countries Country;
    }
}
