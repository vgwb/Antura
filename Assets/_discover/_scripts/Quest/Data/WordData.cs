using Antura.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Localization;

namespace Antura.Discover
{
    //[CreateAssetMenu(fileName = "WordData", menuName = "Antura/Discover/Words Library")]
    [Serializable]
    public class WordData
    {
        public string Id;
        public bool Active;
        public string Text;
        public WordDataKind Kind;
        public WordDataCategory Category;
        public WordDataForm Form;
        public string Value;
        public string SortValue;

        public string DrawingUnicode; // drawing unicode
        public string DrawingValue; // used for display (like colours)
        public string DrawingAtlas;
    }


}
