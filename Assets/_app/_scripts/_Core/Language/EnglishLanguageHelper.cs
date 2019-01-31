using Antura.Database;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Language
{
    public class EnglishLanguageHelper : AbstractLanguageHelper
    {
        /*
        public override List<StringPart> SplitWord(DatabaseManager databaseManager, WordData wordData,
            bool separateDiacritics = false, bool separateVariations = false)
        {
            return SplitWord(databaseManager.StaticDatabase, wordData, separateDiacritics, separateVariations);
        }

        public override List<StringPart> SplitWord(DatabaseObject staticDatabase, WordData wordData,
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
        }*/

    }
}