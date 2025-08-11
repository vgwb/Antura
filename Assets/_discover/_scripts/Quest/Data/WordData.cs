using Antura.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Localization;

namespace Antura.Discover
{
    [Serializable]
    [CreateAssetMenu(fileName = "WordData", menuName = "Antura/Discover/Word Data")]
    public class WordData : ScriptableObject
    {
        public string Id;
        public bool Active;
        public LocalizedString TextLocalized;
        public string TextEn;
        public WordDataKind Kind;
        public WordDataCategory Category;
        public WordDataForm Form;
        public string Value;
        public string SortValue;

        public string DrawingUnicode; // drawing unicode
        public string DrawingValue; // used for display (like colours)
        public string DrawingAtlas;

        public string GetLocalizedString()
        {
            return TextEn;
        }
    }
}
