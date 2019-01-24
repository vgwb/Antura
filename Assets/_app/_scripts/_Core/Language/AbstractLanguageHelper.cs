using Antura.Language;
using UnityEngine;
using System.Collections.Generic;
using Antura.Database;
using Antura.Helpers;

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
            return hexCode;
        }

        public virtual string GetHexUnicodeFromChar(char _char, bool unicodePrefix = false)
        {
            return "";
        }

        public virtual string GetWordWithMissingLetterText(WordData arabicWord, StringPart partToRemove, string removedLetterChar = "_")
        {
            return "";
        }

        public virtual List<StringPart> FindLetter(DatabaseManager database, WordData arabicWord, LetterData letterToFind, bool findSameForm)
        {
            return new List<StringPart>();
        }

        public virtual List<StringPart> SplitWord(DatabaseManager database, WordData arabicWord,
            bool separateDiacritics = false, bool separateVariations = false)
        {
            return new List<StringPart>();
        }


        public virtual List<StringPart> SplitWord(DatabaseObject staticDatabase, WordData arabicWord,
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