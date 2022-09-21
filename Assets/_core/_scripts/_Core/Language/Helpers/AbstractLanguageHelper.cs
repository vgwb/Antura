using Antura.Language;
using Antura.Database;
using Antura.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.DeExtensions;
using UnityEngine;

namespace Antura.Language
{
    public enum MarkType
    {
        SingleLetter,
        FromStartToLetter,
        FromLetterToEnd
    }

    public abstract class AbstractLanguageHelper : ScriptableObject, ILanguageHelper
    {
        public virtual string ProcessString(string str)
        {
            return str;
        }

        #region Unicode
        // TODO: move out of here, not language dependant

        public string GetStringUnicodes(string str)
        {
            char[] chars = str.ToCharArray();
            string output = "";
            for (int i = 0; i < chars.Length; i++)
            {
                char character = chars[i];
                output += GetHexUnicodeFromChar(character) + ", ";
            }
            return output;
        }

        /// <summary>
        /// Return single letter string start from unicode hex code.
        /// </summary>
        /// <param name="hexCode">string Hexadecimal number</param>
        /// <returns>string char</returns>
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

        /// <summary>
        /// Get char hex code.
        /// </summary>
        /// <param name="_char"></param>
        /// <param name="unicodePrefix"></param>
        /// <returns></returns>
        public string GetHexUnicodeFromChar(char _char, bool unicodePrefix = false)
        {
            return string.Format("{1}{0:X4}", Convert.ToUInt16(_char), unicodePrefix ? "/U" : string.Empty);
        }

        #endregion

        /// <summary>
        /// Return a string of a word without a character. Warning: the word is already reversed and fixed for rendering.
        /// This is mandatory since PrepareArabicStringForDisplay should be called before adding removedLetterChar.
        /// </summary>
        public string GetWordWithMissingLetterText(WordData wordData, StringPart partToRemove, string removedLetterChar = "\u2588", string removedLetterColor = "#F1BB3D")
        {
            string text = ProcessString(wordData.Text);

            var colorStart = $"<color={removedLetterColor}>";
            var colorEnd = "</color>";
            int toCharacterIndex = partToRemove.toCharacterIndex + 1;
            text = $"{text.Substring(0, partToRemove.fromCharacterIndex)}{colorStart}{removedLetterChar}{colorEnd}{(toCharacterIndex >= text.Length ? "" : text.Substring(toCharacterIndex))}";

            return text;
        }

        public List<StringPart> FindLetter(DatabaseManager database, WordData wordData, LetterData letterToFind, LetterEqualityStrictness strictness)
        {
            var stringParts = new List<StringPart>();

            bool findSameForm = strictness == LetterEqualityStrictness.WithActualForm || strictness == LetterEqualityStrictness.WithVisualForm;
            var parts = SplitWord(database, wordData, separateDiacritics: false, //separateVariations: letterToFind.Kind != LetterDataKind.LetterVariation,
                keepFormInsideLetter: findSameForm);

            for (int i = 0, count = parts.Count; i < count; ++i)
            {
                if (parts[i].letter.IsSameLetterAs(letterToFind, strictness))
                {
                    stringParts.Add(parts[i]);
                }
            }

            return stringParts;
        }

        public List<StringPart> SplitWord(DatabaseManager databaseManager, WordData wordData,
            bool separateDiacritics = false, bool separateVariations = true, bool keepFormInsideLetter = false)
        {
            return SplitWord(databaseManager.StaticDatabase, wordData, separateDiacritics, separateVariations, keepFormInsideLetter);
        }

        public virtual List<StringPart> SplitWord(DatabaseObject staticDatabase, WordData wordData,
            bool separateDiacritics = false, bool separateVariations = true, bool keepFormInsideLetter = false)
        {

            var stringParts = new List<StringPart>();
            var availableLettersTable = staticDatabase.GetLetterTable();

            // To split a word, we first check whether we can find subsets of long available letters
            var availableLettersList = availableLettersTable.GetList().ConvertAll(x => x as LetterData);
            availableLettersList.Sort((x, y) => y.Isolated.Length - x.Isolated.Length);

            var depletingWord = wordData.Text;
            int maxLetterLength = availableLettersList[0].Isolated.Length;
            // From the beginning of the word, check substrings of max length
            int index = 0;
            while(depletingWord.Length > 0)
            {
                bool foundLetter = false;

                for (int l = Mathf.Min(depletingWord.Length, maxLetterLength); l >= 1; l--)
                {
                    var sub = depletingWord.Substring(0, l);
                    var ld = availableLettersList.FirstOrDefault(x => string.Equals(x.Isolated, sub, StringComparison.OrdinalIgnoreCase));
                    if (ld != null)
                    {
                        if (ld.LigatureSplit != null && ld.LigatureSplit.Length > 0)
                        {
                            foreach (var letterId in ld.LigatureSplit)
                            {
                                ld = staticDatabase.GetById(availableLettersTable, letterId);
                                stringParts.Add(new StringPart(ld, index, index, LetterForm.Isolated));
                            }
                            depletingWord = depletingWord.Substring(1, depletingWord.Length - 1);
                            index += 1;
                        }
                        else
                        {
                            stringParts.Add(new StringPart(ld, index, index + ld.Isolated.Length-1, LetterForm.Isolated));
                            depletingWord = depletingWord.Substring(l, depletingWord.Length - l);
                            index += ld.Isolated.Length;
                        }
                        foundLetter = true;
                        break;
                    }
                }

                if (!foundLetter)
                {
                    throw new Exception($"Could not find any letter in word {wordData} for the remainder of {depletingWord}");
                }
            }

            //Debug.LogError($"Split {wordData}: {stringParts.Select(x => x.letter.GetStringForDisplay()).ToJoinedString()}");

            return stringParts;
        }


        public virtual List<StringPart> SplitPhrase(DatabaseManager databaseManager, PhraseData phrase,
            bool separateDiacritics = false, bool separateVariations = true, bool keepFormInsideLetter = false)
        {
            return SplitPhrase(databaseManager.StaticDatabase, phrase, separateDiacritics, separateVariations, keepFormInsideLetter);
        }

        public virtual List<StringPart> SplitPhrase(DatabaseObject staticDatabase, PhraseData phrase,
            bool separateDiacritics = false, bool separateVariations = true, bool keepFormInsideLetter = false)
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



        #region Text Utilities


        /// <summary>
        /// Return a string of a word with the "color" tag enveloping a character. The word is already reversed and fixed for rendering.
        /// </summary>
        public string GetWordWithMarkedLetterText(WordData wordData, StringPart letterToMark, Color color, MarkType type)
        {
            string tagStart = "<color=#" + GenericHelper.ColorToHex(color) + ">";
            string tagEnd = "</color>";

            string text = ProcessString(wordData.Text);

            string startText = text.Substring(0, letterToMark.fromCharacterIndex);
            string letterText = text.Substring(letterToMark.fromCharacterIndex,
                letterToMark.toCharacterIndex - letterToMark.fromCharacterIndex + 1);
            string endText = (letterToMark.toCharacterIndex >= text.Length - 1 ? "" : text.Substring(letterToMark.toCharacterIndex + 1));

            if (type == MarkType.SingleLetter)
            {
                return startText + tagStart + letterText + tagEnd + endText;
            }
            else if (type == MarkType.FromStartToLetter)
            {
                return tagStart + startText + letterText + tagEnd + endText;
            }
            else
            {
                return startText + tagStart + letterText + endText + tagEnd;
            }
        }

        /// <summary>
        /// Return a string of a word with the "color" tag enveloping multiple characters. The word is already reversed and fixed for rendering.
        /// </summary>
        public string GetWordWithMarkedLettersText(WordData wordData, List<StringPart> lettersToMark, Color color)
        {
            // Sort letters To Mark
            lettersToMark.Sort((g1, g2) => g1.fromCharacterIndex.CompareTo(g2.fromCharacterIndex));

            // Remove duplicates
            for (int i = 0; i < lettersToMark.Count; ++i)
            {
                var toCheck = lettersToMark[i];

                for (int j = i + 1; j < lettersToMark.Count; ++j)
                {
                    if (toCheck.fromCharacterIndex == lettersToMark[j].fromCharacterIndex)
                    {
                        // Remove j
                        lettersToMark.RemoveAt(j);
                        --j;
                    }
                }
            }

            string tagStart = "<color=#" + GenericHelper.ColorToHex(color) + ">";
            string tagEnd = "</color>";

            string text = ProcessString(wordData.Text);

            string markedText = "";

            int currentPosition = 0;

            for (int i = 0, len = lettersToMark.Count; i < len; ++i)
            {
                var letterToMark = lettersToMark[i];
                if (currentPosition < letterToMark.fromCharacterIndex)
                    markedText += text.Substring(currentPosition, letterToMark.fromCharacterIndex - currentPosition);

                markedText += tagStart;

                markedText += text.Substring(letterToMark.fromCharacterIndex,
                    letterToMark.toCharacterIndex - letterToMark.fromCharacterIndex + 1);

                markedText += tagEnd;
                currentPosition = letterToMark.toCharacterIndex + 1;
            }

            markedText += (lettersToMark[lettersToMark.Count - 1].toCharacterIndex >= text.Length - 1 ? "" : text.Substring(lettersToMark[lettersToMark.Count - 1].toCharacterIndex + 1));

            return markedText;
        }


        /// <summary>
        /// Returns a coroutine which creates a string with a letter that flashes over frames, with an option to mark the text before it.
        /// </summary>
        public IEnumerator GetWordWithFlashingText(WordData wordData, int fromIndexToFlash, int toIndexToFlash, Color flashColor,
            float cycleDuration, int numCycles, Action<string> callback, bool markPrecedingLetters = false)
        {
            string text = ProcessString(wordData.Text);

            // Special behaviour for chars
            var markedPart = text.Substring(fromIndexToFlash, 1);
            if (markedPart == " ")
            {
                text = text.Replace(" ", "_");
            }

            string markTagStart = $"<color=#{GenericHelper.ColorToHex(flashColor)}>";
            string markTagEnd = "</color>";

            float timeElapsed = 0f;
            int numCompletedCycles = 0;

            float halfDuration = cycleDuration * 0.5f;

            while (numCompletedCycles < numCycles)
            {
                float interpolant = timeElapsed < halfDuration
                    ? timeElapsed / halfDuration
                    : 1 - ((timeElapsed - halfDuration) / halfDuration);
                string flashTagStart = $"<color=#{GenericHelper.ColorToHex(Color.Lerp(Color.black, flashColor, interpolant))}>";
                string flashTagEnd = "</color>";

                string resultOfThisFrame = "";

                if (markPrecedingLetters)
                {
                    resultOfThisFrame += markTagStart;
                }
                resultOfThisFrame += text.Substring(0, fromIndexToFlash);
                if (markPrecedingLetters)
                {
                    resultOfThisFrame += markTagEnd;
                }
                resultOfThisFrame += flashTagStart;
                resultOfThisFrame += text.Substring(fromIndexToFlash, toIndexToFlash - fromIndexToFlash + 1);
                resultOfThisFrame += flashTagEnd;
                if (toIndexToFlash + 1 < text.Length)
                {
                    resultOfThisFrame += text.Substring(toIndexToFlash + 1);
                }

                callback(resultOfThisFrame);

                timeElapsed += Time.fixedDeltaTime;
                if (timeElapsed >= cycleDuration)
                {
                    numCompletedCycles++;
                    timeElapsed = 0f;
                }

                yield return new WaitForFixedUpdate();
            }
        }

        /// <summary>
        /// Returns a completely colored string of an Arabic word.
        /// </summary>
        public string GetWordWithMarkedText(WordData wordData, Color color)
        {
            string tagStart = "<color=#" + GenericHelper.ColorToHex(color) + ">";
            string tagEnd = "</color>";

            string text = ProcessString(wordData.Text);

            return tagStart + text + tagEnd;
        }

        #endregion

    }
}
