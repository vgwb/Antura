using System.Collections.Generic;

namespace Antura.Database.Management
{
    /// <summary>
    /// Custom JSON parser for LocalizationData
    /// </summary>
    public class LocalizationParser : DataParser<LocalizationData, LocalizationTable>
    {
        override protected LocalizationData CreateData(Dictionary<string, object> dict, DatabaseObject db)
        {
            var data = new LocalizationData();

            data.Id = ToString(dict["Id"]);
            data.Character = ToString(dict["Character"]);
            data.Area = ToString(dict["Area"]);
            data.When = ToString(dict["When"]);
            data.Context = ToString(dict["Context"]);
            data.English = ToString(dict["English"]);
            data.Italian = ToString(dict["Italian"]);
            data.Arabic = ToString(dict["Arabic"]);
            data.ArabicFemale = ToString(dict["ArabicFemale"]);
            data.AudioFile = ToString(dict["AudioFile"]);
            //data.HasF = (ToString(dict["ArabicFemale"]) == "1") ? (data.Arabic + "_F") : "";

            return data;
        }

        protected override void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list)
        {
            ExtractEnum(rowdicts_list, "Id", addNoneValue: true);
        }
    }
}
