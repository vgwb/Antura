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
            //data.Character = ToString(dict["Character"]);
            //data.Area = ToString(dict["Area"]);
            //data.When = ToString(dict["When"]);
            //data.Context = ToString(dict["Context"]);
            data.InstructionText = ToString(dict["LocalizedText"]);
            //data.Italian = ToString(dict["Italian"]);
            data.LearningText = ToString(dict["LocalizedText"]);
            data.LocalizedTextFemale = ToString(dict["LocalizedTextFemale"]);
            data.AudioFile = ToString(dict["AudioFile"]);
            return data;
        }

        protected override void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list)
        {
            ExtractEnum(rowdicts_list, "Id", addNoneValue: true);
        }
    }
}
