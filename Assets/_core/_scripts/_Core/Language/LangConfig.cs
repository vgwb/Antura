using System;
using Antura.Language;
using UnityEngine;
using TMPro;
using Antura.Core;
using UnityEngine.Serialization;

namespace Antura.Language
{
    [CreateAssetMenu]
    public class LangConfig : ScriptableObject
    {
        public LanguageCode Code;
        public string Iso2;
        public TextDirection TextDirection;
        public Sprite FlagIcon;
        public string LocalizedName;
        public AlphabetCode Alphabet;
        public string TutorialLetterId;

        [Space(20)]
        [Header("Fonts")]
        public bool OverrideTextFonts;
        [FormerlySerializedAs("Fonts")]
        public TMP_FontAsset LetterFont;
        public TMP_FontAsset DrawingsFont;
        public TMP_FontAsset TextFont;
        [FormerlySerializedAs("OutlineFontMaterial")]
        public Material OutlineLetterFontMaterial;
        public Material OutlineDrawingFontMaterial;

        public bool IsRightToLeft()
        {
            return TextDirection == TextDirection.RightToLeft;
        }
    }
}