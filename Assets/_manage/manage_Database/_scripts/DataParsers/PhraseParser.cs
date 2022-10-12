using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Helpers;
using Antura.Language;

namespace Antura.Database.Management
{
    /// <summary>
    /// Custom JSON parser for PhraseData
    /// </summary>
    public class PhraseParser : DataParser<PhraseData, PhraseTable>
    {
        override protected PhraseData CreateData(Dictionary<string, object> dict, DatabaseObject db,
            LanguageCode language)
        {
            var data = new PhraseData();

            data.Id = ToString(dict["Id"]);
            data.Active = (ToInt(dict["Active"]) == 1);
            if (!data.Active)
                return null;  // Skip this data if inactive

            string langKey = language.ToString().ToUpper()[0] + language.ToString().Substring(1);
            //            Debug.Log("langKey: " + langKey);
            data.Text = ToString(dict[langKey]);
            data.Category = ParseEnum<PhraseDataCategory>(data, dict["Category"]);
            data.Linked = ToString(dict["Linked"]);
            data.Words = ParseIDArray<WordData, WordTable>(data, (string)dict["Words"], db.GetWordTable());
            data.Answers = ParseIDArray<WordData, WordTable>(data, (string)dict["Answers"], db.GetWordTable());
            data.Complexity = ToFloat(dict["Complexity"]);
            data.PlaySessionLinks = ParseStringsArray(dict["PlaySessionLink"]);

            return data;
        }

        protected override void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list)
        {
            ExtractEnum(rowdicts_list, "Category", addNoneValue: true);
        }

        protected override void FinalValidation(PhraseTable table, DatabaseObject db)
        {
            // Field 'Linked' is validated with a final validation step, since it is based on this same table
            foreach (var data in table.GetValuesTyped())
            {
                if (data.Linked != "" && table.GetValue(data.Linked) == null)
                {
                    LogValidationError(data, "Cannot find id of PhraseData for Linked value " + data.Linked + " (found in phrase " + data.Id + ")");
                }

                // Debug.LogError("PHRASE: " + data.Id.ToString());
                var foundActualWords = new List<WordData>();
                bool foundHighlightedWord = false;
                WordData selectedWord = null;

                foreach (var word in AppManager.I.VocabularyHelper.GetWordsFromPhraseText(data))
                {
                    if (word.Selected)
                    {
                        selectedWord = word.WD;
                        foundHighlightedWord = true;
                    }

                    if (!word.WD.Id.Contains("RUNTIME-"))
                    {
                        foundActualWords.Add(word.WD);
                    }
                }

                if (!foundHighlightedWord)
                {
                    LogValidationError(data, $"Phrase '{data.Id}' with text '{data.Text}' has no highlighted word");
                }

                foreach (var w in data.Answers)
                {
                    if (!foundActualWords.Any(x => x.Id == w))
                    {
                        var actualWordData = AppManager.I.DB.GetWordDataById(w);
                        Debug.Log(AppManager.I.VocabularyHelper.GetWordsFromPhraseText(data).Select(x => x.WD.Text).ToJoinedString());
                        LogValidationWarning(data, $"{actualWordData.Text} =/= {(selectedWord == null ? "???" : selectedWord.Text)} \t Phrase '{data.Id}' with text '{data.Text}' lists '{w}' in its Answers, but it could not be found in the phrase text. (text should be {actualWordData.Text}, while highlighted word is {(selectedWord == null ? "???" : selectedWord.Text)})");
                    }
                }
            }


        }

    }
}
