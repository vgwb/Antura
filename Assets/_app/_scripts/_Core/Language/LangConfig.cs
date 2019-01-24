using System;
using Antura.Language;
using UnityEngine;
using TMPro;
using Antura.Core;

namespace Antura.Language
{
    [CreateAssetMenu]
    public class LangConfig : ScriptableObject
    {
        public LanguageCode Code;
        public TMP_FontAsset font;
        public TextDirection TextDirection;
    }
}