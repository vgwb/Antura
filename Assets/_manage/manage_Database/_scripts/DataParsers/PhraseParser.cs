using System.Collections.Generic;

namespace Antura.Database.Management
{
    /// <summary>
    /// Custom JSON parser for PhraseData
    /// </summary>
    public class PhraseParser : DataParser<PhraseData, PhraseTable>
    {
        override protected PhraseData CreateData(Dictionary<string, object> dict, DatabaseObject db)
        {
            var data = new PhraseData();

            data.Id = ToString(dict["Id"]);
            data.Active = (ToInt(dict["Active"]) == 1);
            if (!data.Active) return null;  // Skip this data if inactive

            data.English = ToString(dict["English"]);
            data.Arabic = ToString(dict["Arabic"]);
            data.Category = ParseEnum<PhraseDataCategory>(data, dict["Category"]);
            data.Linked = ToString(dict["Linked"]);
            data.Words = ParseIDArray<WordData, WordTable>(data, (string)dict["Words"], db.GetWordTable());
            data.Answers = ParseIDArray<WordData, WordTable>(data, (string)dict["Answers"], db.GetWordTable());
            data.Complexity = ToFloat(dict["Complexity"]);

            return data;
        }

        protected override void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list)
        {
            ExtractEnum(rowdicts_list, "Category", addNoneValue: true);
        }

        protected override void FinalValidation(PhraseTable table, DatabaseObject db)
        {
            // Field 'Linked' is validated with a final validation step, since it is based on this same table
            foreach (var data in table.GetValuesTyped()) {
                if (data.Linked != "" && table.GetValue(data.Linked) == null) {
                    LogValidation(data, "Cannot find id of PhraseData for Linked value " + data.Linked + " (found in phrase " + data.Id + ")");
                }
            }

        }

    }
}
