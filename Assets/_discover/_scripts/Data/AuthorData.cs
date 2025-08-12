using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "AuthorData", menuName = "Antura/Discover/Author", order = 1)]
    public class AuthorData : ScriptableObject
    {
        public string Name;
        public string Url;
        public Countries Country;
    }
}
