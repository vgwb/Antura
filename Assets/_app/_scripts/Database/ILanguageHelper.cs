using System;
using System.Collections.Generic;
using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using ArabicSupport;

namespace Antura.Language
{
    public struct StringPart
    {
        public LetterData letter;
        public int fromCharacterIndex;
        public int toCharacterIndex;
        public LetterForm letterForm;

        public StringPart(LetterData letter, int fromCharacterIndex, int toCharacterIndex, LetterForm letterForm)
        {
            this.letter = letter;
            this.fromCharacterIndex = fromCharacterIndex;
            this.toCharacterIndex = toCharacterIndex;
            this.letterForm = letterForm;
        }
    }

    public interface ILanguageHelper
    {
        string ProcessArabicString(string str);
        string GetStringUnicodes(string str);

        string GetLetterFromUnicode(string hexCode);

        string GetHexUnicodeFromChar(char _char, bool unicodePrefix = false);

        string GetWordWithMissingLetterText(WordData arabicWord, StringPart partToRemove, string removedLetterChar = "_");

        List<StringPart> FindLetter(DatabaseManager database, WordData arabicWord, LetterData letterToFind, bool findSameForm);

        List<StringPart> SplitWord(DatabaseManager database, WordData wordData,
            bool separateDiacritics = false, bool separateVariations = false);

        List<StringPart> SplitWord(DatabaseObject staticDatabase, WordData wordData,
            bool separateDiacritics = false, bool separateVariations = false);

        List<StringPart> SplitPhrase(DatabaseManager database, PhraseData phraseData,
            bool separateDiacritics = false,
            bool separateVariations = true);

        List<StringPart> SplitPhrase(DatabaseObject staticDatabase, PhraseData phraseData,
            bool separateDiacritics = false,
            bool separateVariations = true);

        List<StringPart> AnalyzeArabicString(DatabaseObject staticDatabase, string processedArabicString,
            bool separateDiacritics = false, bool separateVariations = true);

        bool FixTMProDiacriticPositions(TMPro.TMP_TextInfo textInfo);

        string DebugShowDiacriticFix(string unicode1, string unicode2);

    }
}