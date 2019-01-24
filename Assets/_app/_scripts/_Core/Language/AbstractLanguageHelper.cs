using Antura.Language;
using UnityEngine;
using System.Collections.Generic;
using Antura.Database;
using Antura.Helpers;
using System;

namespace Antura.Language 
{
    public abstract class AbstractLanguageHelper : ScriptableObject, ILanguageHelper
    {
        public virtual string ProcessArabicString(string str)
        {
            return str;
        }
        public virtual string GetStringUnicodes(string str)
        {
            return str;
        }

        public virtual string GetLetterFromUnicode(string hexCode)
        {
            if (hexCode == "")
            {
                Debug.LogError(
                    "Letter requested with an empty hexacode (data is probably missing from the DataBase). Returning - for now.");
                hexCode = "002D";
            }

            int unicode = int.Parse(hexCode, System.Globalization.NumberStyles.HexNumber);
            var character = (char)unicode;
            return character.ToString();
        }

        public virtual string GetHexUnicodeFromChar(char _char, bool unicodePrefix = false)
        {
            return string.Format("{1}{0:X4}", Convert.ToUInt16(_char), unicodePrefix ? "/U" : string.Empty);
        }

        public virtual string GetWordWithMissingLetterText(WordData arabicWord, StringPart partToRemove, string removedLetterChar = "_")
        {
            return "";
        }

        public virtual List<StringPart> FindLetter(DatabaseManager database, WordData arabicWord, LetterData letterToFind, bool findSameForm)
        {
            return new List<StringPart>();
        }

        public virtual List<StringPart> SplitWord(DatabaseManager database, WordData wordData,
            bool separateDiacritics = false, bool separateVariations = false)
        {
            return new List<StringPart>();
        }

        public virtual List<StringPart> SplitWord(DatabaseObject staticDatabase, WordData wordData,
            bool separateDiacritics = false, bool separateVariations = false)
        {
            return new List<StringPart>();
        }


        public virtual List<StringPart> SplitPhrase(DatabaseManager database, PhraseData phrase,
            bool separateDiacritics = false,
            bool separateVariations = true)
        {
            return new List<StringPart>();
        }


        public virtual List<StringPart> SplitPhrase(DatabaseObject staticDatabase, PhraseData phrase,
            bool separateDiacritics = false,
            bool separateVariations = true)
        {
            return new List<StringPart>();
        }


        public virtual List<StringPart> AnalyzeArabicString(DatabaseObject staticDatabase, string processedArabicString,
            bool separateDiacritics = false, bool separateVariations = true)
        {
            return new List<StringPart>();
        }


        public virtual bool FixTMProDiacriticPositions(TMPro.TMP_TextInfo textInfo)
        {
            return true;
        }

        public virtual string DebugShowDiacriticFix(string unicode1, string unicode2)
        {
            return "";
        }

    }
}