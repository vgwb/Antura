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
            data.LearningText_F = ToString(dict["english_F"]);
            data.LearningAudio = ToString(dict["english_AUDIO"]);

            data.NativeText = ToString(dict["spanish"]);
            data.NativeText_F = ToString(dict["spanish_F"]);
            data.NativeAudio = ToString(dict["spanish_AUDIO"]);
            return data;
        }

        protected override void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list)
        {
            ExtractEnum(rowdicts_list, "Id", addNoneValue: true);
        }
    }
}
