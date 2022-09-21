using System;
using System.Collections;
using System.Collections.Generic;
using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using UnityEngine;

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
        string ProcessString(string str);

        string GetStringUnicodes(string str);
        string GetLetterFromUnicode(string hexCode);
        string GetHexUnicodeFromChar(char _char, bool unicodePrefix = false);

        // u2588: █
        // u25A1: □
        string GetWordWithMissingLetterText(WordData wordData, StringPart partToRemove, string removedLetterChar = "\u25A1", string removedLetterColor = "#F1BB3D");

        /// <summary>
        /// Find all the occurrences of "letterToFind" in "wordData"
        /// </summary>
        /// <returns>the list of occurrences</returns>
        List<StringPart> FindLetter(DatabaseManager databaseManager, WordData wordData, LetterData letterToFind, LetterEqualityStrictness strictness);

        List<StringPart> SplitWord(DatabaseManager databaseManager, WordData wordData,
            bool separateDiacritics = false, bool separateVariations = true, bool keepFormInsideLetter = false);

        List<StringPart> SplitWord(DatabaseObject staticDatabase, WordData wordData,
            bool separateDiacritics = false, bool separateVariations = true, bool keepFormInsideLetter = false);

        List<StringPart> SplitPhrase(DatabaseManager databaseManager, PhraseData phraseData,
            bool separateDiacritics = false, bool separateVariations = true, bool keepFormInsideLetter = false);

        List<StringPart> SplitPhrase(DatabaseObject staticDatabase, PhraseData phraseData,
            bool separateDiacritics = false, bool separateVariations = true, bool keepFormInsideLetter = false);

        bool FixTMProDiacriticPositions(TMPro.TMP_TextInfo textInfo);

        string DebugShowDiacriticFix(string unicode1, string unicode2);


        // Text Utilities
        string GetWordWithMarkedLetterText(WordData wordData, StringPart letterToMark, Color color, MarkType type);
        string GetWordWithMarkedLettersText(WordData wordData, List<StringPart> lettersToMark, Color color);
        IEnumerator GetWordWithFlashingText(WordData wordData, int fromIndexToFlash, int toIndexToFlash, Color flashColor,
            float cycleDuration, int numCycles, Action<string> callback, bool markPrecedingLetters = false);
        string GetWordWithMarkedText(WordData wordData, Color color);

    }
}
