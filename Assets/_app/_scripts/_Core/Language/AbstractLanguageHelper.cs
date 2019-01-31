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
        // TODO: should be renamed
        public virtual string ProcessString(string str)
        {
            return str;
        }

        public virtual string GetStringUnicodes(string str)
        {
            throw new NotImplementedException();
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

        public virtual string GetWordWithMissingLetterText(WordData wordData, StringPart partToRemove, string removedLetterChar = "_")
        {
            throw new NotImplementedException();
        }

        public virtual List<StringPart> FindLetter(DatabaseManager database, WordData wordData, LetterData letterToFind, bool findSameForm)
        {
            var stringParts = new List<StringPart>();
            var parts = SplitWord(database, wordData, false, letterToFind.Kind != LetterDataKind.LetterVariation);

            for (int i = 0, count = parts.Count; i < count; ++i)
            {
                if (parts[i].letter.Id == letterToFind.Id &&
                    (!findSameForm || (parts[i].letterForm == letterToFind.Form)))
                {
                    stringParts.Add(parts[i]);
                }
            }

            return stringParts;
        }

        public List<StringPart> SplitWord(DatabaseManager databaseManager, WordData wordData,
            bool separateDiacritics = false, bool separateVariations = false)
        {
            return SplitWord(databaseManager.StaticDatabase, wordData, separateDiacritics, separateVariations);
        }

        public virtual List<StringPart> SplitWord(DatabaseObject staticDatabase, WordData wordData,
            bool separateDiacritics = false, bool separateVariations = false)
        {
            var stringParts = new List<StringPart>();
            char[] chars = wordData.Text.ToCharArray();
            for (int iChar = 0; iChar < chars.Length; iChar++)
            {
                var letterDataID = chars[iChar].ToString();
                var letterData = staticDatabase.GetById(staticDatabase.GetLetterTable(), letterDataID);
                stringParts.Add(new StringPart(letterData, iChar, iChar, LetterForm.Isolated));
            }
            return stringParts;
        }


        public virtual List<StringPart> SplitPhrase(DatabaseManager databaseManager, PhraseData phrase,
            bool separateDiacritics = false,
            bool separateVariations = true)
        {
            return SplitPhrase(databaseManager.StaticDatabase, phrase, separateDiacritics, separateVariations);
        }

        public virtual List<StringPart> SplitPhrase(DatabaseObject staticDatabase, PhraseData phrase,
            bool separateDiacritics = false,
            bool separateVariations = true)
        {
            throw new NotImplementedException();
        }

        // TODO: rename
        public virtual List<StringPart> AnalyzeArabicString(DatabaseObject staticDatabase, string processedArabicString,
            bool separateDiacritics = false, bool separateVariations = true)
        {
            throw new NotImplementedException();
        }

        // TODO: remove from here
        public virtual bool FixTMProDiacriticPositions(TMPro.TMP_TextInfo textInfo)
        {
            return true;
        }

        // TODO: remove from here
        public virtual string DebugShowDiacriticFix(string unicode1, string unicode2)
        {
            throw new NotImplementedException();
        }

    }
}