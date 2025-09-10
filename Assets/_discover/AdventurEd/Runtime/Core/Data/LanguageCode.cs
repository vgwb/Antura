using System;
using UnityEngine;

namespace AdventurEd
{
    [Serializable]
    public class LanguageCode
    {
        [SerializeField]
        private string code = "en";

        public string Code => code;
        public string DisplayName => LanguageCodes.GetName(code);

        public static implicit operator string(LanguageCode lc) => lc.code;
    }
}
