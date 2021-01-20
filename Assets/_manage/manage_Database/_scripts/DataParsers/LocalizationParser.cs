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

            data.Id = ToString(dict["Id"]);
            data.AudioKey = ToString(dict["audio_key"]);

            data._LocalizedDatas = new LocalizedData[Enum.GetNames(typeof(LanguageCode)).Length];

            foreach (LanguageCode lang in Enum.GetValues(typeof(LanguageCode))) {
                if (lang == LanguageCode.COUNT || lang == LanguageCode.NONE || lang == LanguageCode.arabic_legacy) { continue; }
                var langData = new LocalizedData();
                Debug.Log(lang);
                langData.Text = ToString(dict[lang.ToString().ToLower()]);
                if (dict.ContainsKey(lang.ToString().ToLower() + "_F")) {
                    langData.TextF = ToString(dict[lang.ToString().ToLower() + "_F"]);
                }
                data._LocalizedDatas[(int)lang - 1] = langData;
            }

            //var engData = new LocalizedData();
            //engData.Text = ToString(dict["english"]);

            //var spanishData = new LocalizedData();
            //spanishData.Text = ToString(dict["spanish"]);

            //var arabicData = new LocalizedData();
            //arabicData.Text = ToString(dict["arabic"]);
            //arabicData.TextF = ToString(dict["arabic_F"]);

            //var itaData = new LocalizedData();
            //itaData.Text = ToString(dict["italian"]);

            //data._LocalizedDatas = new[] { engData, arabicData, spanishData, itaData };
            return data;
        }

        protected override void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list)
        {
            ExtractEnum(rowdicts_list, "Id", addNoneValue: true);
        }
    }
}
