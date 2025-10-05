using Antura.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Localization;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "WordData", menuName = "Antura/Discover Data/Word Data")]
    public class WordData : IdentifiedData
    {
        public bool Active;
        public Status DevStatus = Status.Draft;
        public LocalizedString TextLocalized;

        [Tooltip("English text, used for sorting and fallback.")]
        public string TextEn;

        [Tooltip("Target age range for this word.")]
        public AgeRange targetAge = AgeRange.Ages6to10;

        public WordDataKind Kind;
        public WordDataCategory Category;
        public WordDataForm Form;

        // [Tooltip("If the word is plural, this points to the singular version")]
        // TODO public WordData RelativeWord;

        public string Value;
        public string SortValue;

        [Tooltip("Options if the Font drawing is not available")]
        public Sprite Drawing;
        public string DrawingUnicode; // drawing unicode
        public string DrawingValue; // used for display (like colours)
        public string DrawingAtlas;


        public string GetLocalizedString()
        {
            return TextEn;
        }
    }
}
