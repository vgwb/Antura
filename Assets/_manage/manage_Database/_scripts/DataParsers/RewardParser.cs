using System.Collections.Generic;
using Antura.Language;

namespace Antura.Database.Management
{
    /// <summary>
    /// Custom JSON parser for RewardData
    /// </summary>
    // refactor: this is not used for now!
    public class RewardParser : DataParser<RewardData, RewardTable>
    {
        override protected RewardData CreateData(Dictionary<string, object> dict, DatabaseObject db,
            LanguageCode language)
        {
            var data = new RewardData();

            data.Id = ToString(dict["Id"]);
            data.Title = ToString(dict["Title"]);
            //data.Category = ParseEnum<RewardDataCategory>(data, dict["Category"]);

            return data;
        }

        protected override void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list)
        {
            ExtractEnum(rowdicts_list, "Category");
        }
    }
}
