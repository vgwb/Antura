using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "AuthorDefinition", menuName = "Antura/Discover/Author Definition")]
    public class AuthorDefinition : ScriptableObject
    {
        public string Name;
        public string Url;
        public Countries Country;
    }
}
