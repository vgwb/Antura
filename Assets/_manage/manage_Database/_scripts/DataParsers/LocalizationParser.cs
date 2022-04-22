using UnityEngine;
using System;
using System.Collections.Generic;
using Antura.Language;

namespace Antura.Database.Management
{
    /// <summary>
    /// Custom JSON parser for LocalizationData
    /// </summary>
    public class LocalizationParser : DataParser<LocalizationData, LocalizationTable>
    {
        override protected LocalizationData CreateData(Dictionary<string, object> dict, DatabaseObject db,
            LanguageCode language) // TODO: Deprecate "language"
        {
            var data = new LocalizationData();
            //Debug.Log("Parse " + dict["Id"] + " - used: " + dict["Used"]);
            if (ToInt(dict["Used"]) == 0)
            {
                return null;
            }
            data.Id = ToString(dict["Id"]);
            data.AudioKey = ToString(dict["audio_key"]);

            data._LocalizedDatas = new LocalizedData[Enum.GetNames(typeof(LanguageCode)).Length];

            foreach (LanguageCode lang in Enum.GetValues(typeof(LanguageCode)))
            {
                if (lang == LanguageCode.COUNT || lang == LanguageCode.NONE || lang == LanguageCode.arabic_legacy)
                { continue; }
                var langData = new LocalizedData();
                langData.Text = ToString(dict[lang.ToString().ToLower()]);
                if (dict.ContainsKey(lang.ToString().ToLower() + "_F"))
                {
                    langData.TextF = ToString(dict[lang.ToString().ToLower() + "_F"]);
                }
                data._LocalizedDatas[(int)lang - 1] = langData;
            }
            return data;
        }

        protected override void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list)
        {
            ExtractEnum(rowdicts_list, "Id", addNoneValue: true, valueColumnKey: "EnumId");
        }
    }
}
