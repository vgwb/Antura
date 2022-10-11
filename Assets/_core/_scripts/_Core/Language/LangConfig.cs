using Antura.Core;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

namespace Antura.Language
{
    [CreateAssetMenu(menuName = "Antura/Config Language")]

    public class LangConfig : ScriptableObject
    {
        public LanguageCode Code;

        [Tooltip("This language won't be assigned by default to learning contents and must instead be added manually to the Overriden Native Languages list.")]
        public bool ExtraLanguage; // Used only if added manually as an override
        public string LocalizedName;
        public string Iso3;
        public TextDirection TextDirection;
        public Sprite FlagIcon;
        public AlphabetFamily AlphabetFamily;
        public string TutorialLetterId;

        public AbstractLanguageHelper LanguageHelper;
        public DiacriticsComboData DiacriticsComboData;

        [Header("Language Font")]
        public TMP_FontAsset LanguageFont;
        public Material OutlineLanguageFontMaterial;
        [Header("UI Font")]
        public TMP_FontAsset UIFont;
        [Header("Drawings Font")]
        public TMP_FontAsset DrawingsFont;
        public Material OutlineDrawingFontMaterial;

        public bool IsRightToLeft()
        {
            return TextDirection == TextDirection.RightToLeft;
        }
    }
}
