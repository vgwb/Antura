using System.Collections.Generic;
using Antura.Language;

namespace Antura.Database.Management
{
    /// <summary>
    /// Custom JSON parser for StageData
    /// </summary>
    public class StageParser : DataParser<StageData, StageTable>
    {
        override protected StageData CreateData(Dictionary<string, object> dict, DatabaseObject db,
            LanguageCode language)
        {
            var data = new StageData();

            data.Id = ToString(dict["Id"]);
            data.Title_En = ToString(dict["Title_En"]);
            data.Title_Ar = ToString(dict["Title_Ar"]);
            data.Description = ToString(dict["Description"]);

            return data;
        }

        protected override void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list)
        {
        }
    }

}
