using System.Collections;
using Antura.Helpers;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Antura.Database;
using Antura.Language;

namespace Antura.UI
{
    // TODO: remove Arabic dependency here
    public static class ArabicTextUtilities
    {
        public enum MarkType
        {
            SingleLetter,
            FromStartToLetter,
            FromLetterToEnd
        }

        /// <summary>
        /// Return a string of a word with the "color" tag enveloping a character. The word is already reversed and fixed for rendering.
        /// </summary>
        public static string GetWordWithMarkedLetterText(Database.WordData arabicWord, StringPart letterToMark,
            Color color, MarkType type)
        {
            string tagStart = "<color=#" + GenericHelper.ColorToHex(color) + ">";
            string tagEnd = "</color>";

            string text = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessArabicString(arabicWord.Text);


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
        public static string GetWordWithMarkedLettersText(Database.WordData arabicWord, List<StringPart> lettersToMark,
            Color color)
        {
            // Sort letters To Mark
            lettersToMark.Sort((g1, g2) => g1.fromCharacterIndex.CompareTo(g2.fromCharacterIndex));

            // Remove duplicates
            for (int i=0; i< lettersToMark.Count; ++i)
            {
                var toCheck = lettersToMark[i];

                for (int j = i+1; j < lettersToMark.Count; ++j)
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

            string text = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessArabicString(arabicWord.Text);

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
        public static IEnumerator GetWordWithFlashingText(Database.WordData arabicWord, int fromIndexToFlash, int toIndexToFlash, Color flashColor,
            float cycleDuration, int numCycles, System.Action<string> callback, bool markPrecedingLetters = false)
        {
            string text = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessArabicString(arabicWord.Text);

            string markTagStart = "<color=#" + GenericHelper.ColorToHex(flashColor) + ">";
            string markTagEnd = "</color>";

            float timeElapsed = 0f;
            int numCompletedCycles = 0;

            float halfDuration = cycleDuration * 0.5f;

            while (numCompletedCycles < numCycles)
            {
                float interpolant = timeElapsed < halfDuration
                    ? timeElapsed / halfDuration
                    : 1 - ((timeElapsed - halfDuration) / halfDuration);
                string flashTagStart = "<color=#" + GenericHelper.ColorToHex(Color.Lerp(Color.black, flashColor, interpolant)) + ">";
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
        public static string GetWordWithMarkedText(Database.WordData arabicWord, Color color)
        {
            string tagStart = "<color=#" + GenericHelper.ColorToHex(color) + ">";
            string tagEnd = "</color>";

            string text = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessArabicString(arabicWord.Text);

            return tagStart + text + tagEnd;
        }
    }
}