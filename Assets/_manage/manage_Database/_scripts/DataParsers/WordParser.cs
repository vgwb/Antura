using System.Collections.Generic;
using Antura.Helpers;
using Antura.Language;

namespace Antura.Database.Management
{
    /// <summary>
    /// Custom JSON parser for WordData
    /// </summary>
    public class WordParser : DataParser<WordData, WordTable>
    {
        override protected WordData CreateData(Dictionary<string, object> dict, DatabaseObject db)
        {
            var data = new WordData();

            data.Id = ToString(dict["Id"]);
            data.Active = (ToInt(dict["Active"]) == 1);
            if (!data.Active) return null;  // Skip this data if inactive

            data.Kind = ParseEnum<WordDataKind>(data, dict["Kind"]);
            data.Category = ParseEnum<WordDataCategory>(data, dict["Category"]);
            data.Form = CustomParseForm(data, dict["Form"]);
            data.Article = ParseEnum<WordDataArticle>(data, dict["Article"]);
            data.Gender = CustomParseGender(data, dict["Gender"]);
            data.LinkedWord = ToString(dict["LinkedWord"]);
            data.Text = ToString(dict["Arabic"]);
            data.Value = ToString(dict["Value"]);
            data.Letters = CustomParseLetters(data, db);
            data.Drawing = ToString(dict["Drawing"]);
            data.Complexity = ToFloat(dict["Complexity"]);

            return data;
        }

        private string[] CustomParseLetters(WordData wordData, DatabaseObject db)
        {
            var parts = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).SplitWord(db, wordData);

            string[] letters = new string[parts.Count];

            for (int i = 0, count = letters.Length; i < count; ++i)
                letters[i] = parts[i].letter.Id;

            return letters;
        }

        private WordDataForm CustomParseForm(WordData data, object enum_object)
        {
            if (ToString(enum_object) == "") {
                return WordDataForm.Singular;
            } else {
                return ParseEnum<WordDataForm>(data, enum_object);
            }
        }

        private VocabularyDataGender CustomParseGender(WordData data, object enum_object)
        {
            if (ToString(enum_object) == "") {
                return VocabularyDataGender.None;
            } else {
                return ParseEnum<VocabularyDataGender>(data, enum_object);
            }
        }

        protected override void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list)
        {
            ExtractEnum(rowdicts_list, "Kind");
            ExtractEnum(rowdicts_list, "Category", addNoneValue: true);
            //ExtractEnum(rowdicts_list, "Form");   // @note: cannot auto-generate or Singular won't work
            ExtractEnum(rowdicts_list, "Article", addNoneValue: true);
        }

        protected override void FinalValidation(WordTable table, DatabaseObject db)
        {
            // Field 'LinkedWord' is validated with a final validation step, since it is based on this same table
            foreach (var data in table.GetValuesTyped()) {
                if (data.LinkedWord != "" && table.GetValue(data.LinkedWord) == null) {
                    LogValidation(data, "Cannot find id of WordData for Linked value " + data.LinkedWord + " (found in word " + data.Id + ")");
                }
            }

        }
    }
}
