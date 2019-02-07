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

            data.LearningText = ToString(dict["english"]);
            data.InstructionText = ToString(dict["spanish"]);
            data.LocalizedTextFemale = ToString(dict["spanish_F"]);
            data.AudioFile = ToString(dict["spanish_AUDIO"]);
            return data;
        }

        protected override void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list)
        {
            ExtractEnum(rowdicts_list, "Id", addNoneValue: true);
        }
    }
}
