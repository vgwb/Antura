using System;
using System.Collections.Generic;
using Antura.Language;
using DG.DeExtensions;

namespace Antura.Database.Management
{
    /// <summary>
    /// Custom JSON parser for LetterData
    /// </summary>
    public class LetterParser : DataParser<LetterData, LetterTable>
    {
        override protected LetterData CreateData(Dictionary<string, object> dict, DatabaseObject db,
            LanguageCode language)
        {
            var data = new LetterData();

            data.Id = ToString(dict["Id"]);
            if (data.Id == "▲")
                data.Id = " ";
            data.Active = (ToInt(dict["Active"]) == 1);
            if (!data.Active)
                return null;  // Skip this data if inactive

            data.InBook = (ToInt(dict["InBook"]) == 1);
            data.Number = ToInt(dict["Number"]);
            data.Title = ToString(dict["Title"]);
            data.Kind = ParseEnum<LetterDataKind>(data, dict["Kind"]);
            data.BaseLetter = ToString(dict["BaseLetter"]);

            var ligatureTxt = dict.ContainsKey("LigatureSplit") ? ToString(dict["LigatureSplit"]) : string.Empty;
            data.LigatureSplit = ligatureTxt.IsNullOrEmpty() ? Array.Empty<string>() : ligatureTxt.Split(',');
            data.Symbol = ToString(dict["Symbol"]);
            data.Type = ParseEnum<LetterDataType>(data, dict["Type"]);
            data.Tag = ToString(dict["Tag"]);
            data.PlaySessionLinks = ParseStringsArray(dict["PlaySessionLink"]);
            data.Notes = ToString(dict["Notes"]);
            data.SunMoon = ParseEnum<LetterDataSunMoon>(data, dict["SunMoon"]);

            var nameSound = ToString(dict["NameSound"]);
            data.NameSound = (nameSound == "1" ? data.Id + "__lettername" : nameSound);

            data.PhonemeSound = ToString(dict["PhonemeSound"]);
            data.Sound = ToString(dict["Sound"]);
            data.SoundZone = ToString(dict["SoundZone"]);
            data.Isolated_Unicode = ToString(dict["Isolated_Unicode"]);
            data.Initial_Unicode = ToString(dict["Initial_Unicode"]);
            data.Medial_Unicode = ToString(dict["Medial_Unicode"]);
            data.Final_Unicode = ToString(dict["Final_Unicode"]);
            data.Symbol_Unicode = ToString(dict["Symbol_Unicode"]);
            data.InitialFix = ToString(dict["InitialFix"]);
            data.MedialFix = ToString(dict["MedialFix"]);
            data.FinalFix = ToString(dict["FinalFix"]);
            data.Complexity = ToFloat(dict["Complexity"]);

            data.HasDot = (ToInt(dict["HasDot"]) == 1);
            data.HasAccent = (ToInt(dict["HasAccent"]) == 1);
            data.HasDiacritic = (ToInt(dict["HasDiacritic"]) == 1);
            data.Orientations = ToInt(dict["Orientations"]);

            data.CanConnectBefore = dict.ContainsKey("Can_Connect_Before") && (ToInt(dict["Can_Connect_Before"]) == 1);
            data.CanConnectAfter = dict.ContainsKey("Can_Connect_After") && (ToInt(dict["Can_Connect_After"]) == 1);
            data.HasAccent = (ToInt(dict["HasAccent"]) == 1);

            if (dict.ContainsKey("LinkedWords"))
            {
                data.LinkedWords = ParseIDArray<WordData, WordTable>(data, (string)dict["LinkedWords"], db.GetWordTable());
            }

            return data;
        }

        protected override void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list)
        {
            ExtractEnum(rowdicts_list, "Kind");
            ExtractEnum(rowdicts_list, "Type", addNoneValue: true);
            ExtractEnum(rowdicts_list, "SunMoon", addNoneValue: true);
        }

        protected override void FinalValidation(LetterTable table, DatabaseObject db)
        {
            // Fields 'BaseLetter' and 'Symbol' are validated with a final validation step, since they are based on this same table
            // Also, Combination letters are validated with their BaseLetter and Symbol.
            foreach (var data in table.GetValuesTyped())
            {
                if (data.Kind == LetterDataKind.DiacriticCombo)
                {
                    if (data.BaseLetter == "")
                    {
                        LogValidationError(data, "LetterData with id  " + data.Id + " is a Combination but does not have a BaseLetter.");
                    }

                    if (data.Symbol == "")
                    {
                        LogValidationError(data, "LetterData with id  " + data.Id + " is a Combination but does not have a Symbol.");
                    }

                    if ((data.Id != data.BaseLetter + "_" + data.Symbol) && (data.Id != "alef_hamza_hi" && data.Id != "alef_hamza_low"))
                    {
                        // alef_hamza_hi and alef_hamza_low are the only exceptions in the name format on Combinations
                        LogValidationError(data, "LetterData with id  " + data.Id + " is a Combination, but the BaseLetter and Symbol do not match the Id.");
                    }
                }

                if (data.BaseLetter != "" && table.GetValue(data.BaseLetter) == null)
                {
                    LogValidationError(data, "Cannot find id of LetterData for BaseLetter value " + data.BaseLetter + " (found in letter " + data.Id + ")");
                }

                if (data.Symbol != "" && table.GetValue(data.Symbol) == null)
                {
                    LogValidationError(data, "Cannot find id of LetterData for Symbol value " + data.Symbol + " (found in letter " + data.Id + ")");
                }
            }
        }

    }
}
