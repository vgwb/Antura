using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    [System.Serializable]
    public class AuthorCredit
    {
        public AuthorData Author;
        public bool Content;
        public bool Design;
        public bool Development;
        public bool Validation;
    }

    [CreateAssetMenu(fileName = "AuthorData", menuName = "Antura/Discover/Author", order = 1)]
    public class AuthorData : ScriptableObject
    {
        public string Name;
        public string Url;
        public Countries Country;
    }
}
