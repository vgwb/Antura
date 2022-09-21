using Antura.Database;
using Antura.Helpers;
using Antura.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using DG.DeExtensions;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Antura.Language
{
    // TODO refactor: this class needs a large refactoring as it is used for several different purposes
    public class ArabicLanguageHelper : AbstractLanguageHelper
    {
        public bool ConvertFarsiYehToAlefMaqsura;

        struct UnicodeLookUpEntry
        {
            public LetterData data;
            public LetterForm form;

            public UnicodeLookUpEntry(Database.LetterData data, Database.LetterForm form)
            {
                this.data = data;
                this.form = form;
            }
        }

        struct DiacriticComboLookUpEntry
        {
            public string symbolID;
            public string LetterID;

            public DiacriticComboLookUpEntry(string symbolID, string LetterID)
            {
                this.symbolID = symbolID;
                this.LetterID = LetterID;
            }
        }

        static List<LetterData> allLetterData;
        static Dictionary<string, UnicodeLookUpEntry> unicodeLookUpCache = new Dictionary<string, UnicodeLookUpEntry>();

        static Dictionary<DiacriticComboLookUpEntry, LetterData> diacriticComboLookUpCache =
            new Dictionary<DiacriticComboLookUpEntry, LetterData>();


        /// <summary>
        /// Collapses diacritics and letters, collapses multiple words variations (e.g. lam + alef), selects correct forms unicodes, and reverses the string.
        /// </summary>
        /// <returns>The string, ready for display or further processing.</returns>
        public override string ProcessString(string str)
        {
            SetupMappings();

            ArabicFixer.ConvertFarsiYehToAlefMaqsura = ConvertFarsiYehToAlefMaqsura;
            return GenericHelper.ReverseText(ArabicFixer.Fix(str, true, true));
        }


        public override List<StringPart> SplitWord(DatabaseObject staticDatabase, WordData wordData,
            bool separateDiacritics = false, bool separateVariations = true, bool keepFormInsideLetter = false)
        {
            // Use ArabicFixer to deal only with combined unicodes
            return AnalyzeArabicString(staticDatabase, ProcessString(wordData.Text), separateDiacritics,
                separateVariations, keepFormInsideLetter);
        }

        public override List<StringPart> SplitPhrase(DatabaseObject staticDatabase, PhraseData phrase,
            bool separateDiacritics = false, bool separateVariations = true, bool keepFormInsideLetter = false)
        {
            // Use ArabicFixer to deal only with combined unicodes
            return AnalyzeArabicString(staticDatabase, ProcessString(phrase.Text), separateDiacritics,
                separateVariations, keepFormInsideLetter);
        }

        #region Private

        List<StringPart> AnalyzeArabicString(DatabaseObject staticDatabase, string processedArabicString,
            bool separateDiacritics = false, bool separateVariations = true, bool keepFormInsideLetter = false)
        {
            if (allLetterData == null)
            {
                allLetterData = new List<LetterData>(staticDatabase.GetLetterTable().GetValuesTyped());

                for (int l = 0; l < allLetterData.Count; ++l)
                {
                    var data = allLetterData[l];

                    foreach (var form in data.GetAvailableForms())
                    {
                        if (data.Kind == LetterDataKind.Letter)
                        {
                            // Overwrite
                            unicodeLookUpCache[data.GetUnicode(form)] = new UnicodeLookUpEntry(data, form);
                        }
                        else
                        {
                            var unicode = data.GetUnicode(form);

                            if (!unicodeLookUpCache.ContainsKey(unicode))
                            {
                                unicodeLookUpCache.Add(unicode, new UnicodeLookUpEntry(data, form));
                            }
                        }
                    }

                    if (data.Kind == LetterDataKind.DiacriticCombo)
                    {
                        diacriticComboLookUpCache.Add(new DiacriticComboLookUpEntry(data.Symbol, data.BaseLetter), data);
                    }
                }
            }

            var result = new List<StringPart>();

            // If we used ArabicFixer, this char array will contain only combined unicodes
            char[] chars = processedArabicString.ToCharArray();

            int stringIndex = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                char character = chars[i];

                // Skip spaces and arabic "?"
                if (character == ' ' || character == '؟')
                {
                    ++stringIndex;
                    continue;
                }

                string unicodeString = GetHexUnicodeFromChar(character);

                if (unicodeString == "0640")
                {
                    // it's an arabic tatweel
                    // just extends previous character
                    for (int t = result.Count - 1; t >= 0; --t)
                    {
                        var previous = result[t];

                        if (previous.toCharacterIndex == stringIndex - 1)
                        {
                            ++previous.toCharacterIndex;
                            result[t] = previous;
                        }
                        else
                        {
                            break;
                        }
                    }

                    ++stringIndex;
                    continue;
                }

                // Find the letter, and check its form
                LetterForm letterForm = LetterForm.None;
                LetterData letterData = null;

                UnicodeLookUpEntry entry;
                if (unicodeLookUpCache.TryGetValue(unicodeString, out entry))
                {
                    letterForm = entry.form;
                    letterData = entry.data;
                    if (keepFormInsideLetter)
                        letterData = letterData.Clone();  // We need to clone the data, as it may be overriden later, if we want to keep forms inside it
                }

                if (letterData != null)
                {
                    if (letterData.Kind == LetterDataKind.DiacriticCombo && separateDiacritics)
                    {
                        // It's a diacritic combo
                        // Separate Letter and Diacritic
                        result.Add(
                            new StringPart(
                                staticDatabase.GetById(staticDatabase.GetLetterTable(), letterData.BaseLetter),
                                stringIndex, stringIndex, letterForm));
                        result.Add(
                            new StringPart(
                                staticDatabase.GetById(staticDatabase.GetLetterTable(), letterData.Symbol),
                                stringIndex, stringIndex, letterForm));
                    }
                    else if (letterData.Kind == LetterDataKind.Symbol &&
                             letterData.Type == LetterDataType.DiacriticSymbol && !separateDiacritics)
                    {
                        // It's a diacritic
                        // Merge Letter and Diacritic

                        var symbolId = letterData.Id;
                        var lastLetterData = result[result.Count - 1];
                        var baseLetterId = lastLetterData.letter.Id;

                        LetterData diacriticLetterData = null;
                        if (AppConfig.DisableShaddah)
                        {
                            if (symbolId == "shaddah")
                            {
                                diacriticLetterData = lastLetterData.letter;
                            }
                            else
                            {
                                diacriticComboLookUpCache.TryGetValue(
                                    new DiacriticComboLookUpEntry(symbolId, baseLetterId), out diacriticLetterData);
                            }
                        }
                        else
                        {
                            diacriticComboLookUpCache.TryGetValue(
                                new DiacriticComboLookUpEntry(symbolId, baseLetterId), out diacriticLetterData);
                        }

                        if (diacriticLetterData == null)
                        {
                            Debug.LogError("Cannot find a single character for " + baseLetterId + " + " + symbolId +
                                           ". Diacritic removed in (" + processedArabicString + ").");
                        }
                        else
                        {
                            var previous = result[result.Count - 1];
                            previous.letter = diacriticLetterData;
                            ++previous.toCharacterIndex;
                            result[result.Count - 1] = previous;
                        }
                    }
                    else if (letterData.Kind == LetterDataKind.LetterVariation && separateVariations &&
                             letterData.BaseLetter == "lam")
                    {
                        // it's a lam-alef combo
                        // Separate Lam and Alef
                        result.Add(
                            new StringPart(
                                staticDatabase.GetById(staticDatabase.GetLetterTable(), letterData.BaseLetter),
                                stringIndex, stringIndex, letterForm));

                        var secondPart = staticDatabase.GetById(staticDatabase.GetLetterTable(), letterData.Symbol);

                        if (secondPart.Kind == LetterDataKind.DiacriticCombo && separateDiacritics)
                        {
                            // It's a diacritic combo
                            // Separate Letter and Diacritic
                            result.Add(
                                new StringPart(
                                    staticDatabase.GetById(staticDatabase.GetLetterTable(), secondPart.BaseLetter),
                                    stringIndex, stringIndex, letterForm));
                            result.Add(
                                new StringPart(
                                    staticDatabase.GetById(staticDatabase.GetLetterTable(), secondPart.Symbol),
                                    stringIndex, stringIndex, letterForm));
                        }
                        else
                        {
                            result.Add(new StringPart(secondPart, stringIndex, stringIndex, letterForm));
                        }
                    }
                    else
                    {
                        result.Add(new StringPart(letterData, stringIndex, stringIndex, letterForm));
                    }
                }
                else
                {
                    Debug.LogWarning($"Cannot parse letter {character} ({unicodeString}) in {processedArabicString}");
                }

                ++stringIndex;
            }

            if (keepFormInsideLetter)
            {
                foreach (var stringPart in result)
                {
                    stringPart.letter.ForcedLetterForm = stringPart.letterForm;
                }
            }
            return result;
        }

        #region Diacritic Fix

        public const string Fathah = "064E";
        public const string Dammah = "064F";
        public const string Kasrah = "0650";
        public const string Sukun = "0652";
        public const string Shaddah = "0651";

        List<string> diacriticsSortOrder = new List<string> { Fathah, Dammah, Kasrah, Sukun, Shaddah };

        private struct DiacriticComboEntry
        {
            public string Unicode1;
            public string Unicode2;

            public DiacriticComboEntry(string _unicode1, string _unicode2)
            {
                Unicode1 = _unicode1;
                Unicode2 = _unicode2;
            }
        }

        private static bool hasSetupMappings = false;
        private static void SetupMappings()
        {
            if (hasSetupMappings) return;
            hasSetupMappings = true;

            var db = AppManager.I.DB;
            var mappings = new HashSet<ArabicMapping>();

            // Get all letter DBs from all editions
            foreach (ContentEditionConfig contentEditionConfig in AppManager.I.RootConfig.LoadedAppEdition.ContentEditions)
            {
                foreach (var ld in contentEditionConfig.LetterDB.table.GetValuesTyped())
                {
                    // @note: in our tables, we use the General (x06__ version) directly as the Isolated (xF___ version) as it is the same character
                    try
                    {
                        var mapping = new ArabicMapping(
                            Convert.ToInt32($"0x{ld.Isolated_Unicode}", 16),
                            Convert.ToInt32($"0x{ld.Isolated_Unicode}", 16),
                            ld.Initial_Unicode.IsNullOrEmpty() ? 0 : Convert.ToInt32($"0x{ld.Initial_Unicode}", 16),
                            ld.Medial_Unicode.IsNullOrEmpty() ? 0 : Convert.ToInt32($"0x{ld.Medial_Unicode}", 16),
                            ld.Final_Unicode.IsNullOrEmpty() ? 0 : Convert.ToInt32($"0x{ld.Final_Unicode}", 16),
                            ld.CanConnectBefore, ld.CanConnectAfter
                        );
                        mappings.Add(mapping);
                    }
                    catch (Exception)
                    {
                        // Ignore those that cannot be added
                    }
                }
            }

            ArabicFixer.AddToMappingTable(mappings.ToList());
        }

        private static Dictionary<DiacriticComboEntry, Vector2> DiacriticCombos2Fix = null;

        /// <summary>
        /// these are manually configured positions of diacritic symbols relative to the main letter
        /// since TextMesh Pro can't manage these automatically and some letters are too tall, with the symbol overlapping
        /// </summary>
        public void BuildDiacriticCombos2Fix()
        {

            DiacriticCombos2Fix = new Dictionary<DiacriticComboEntry, Vector2>();

            var diacriticsComboData = LanguageSwitcher.I.GetDiacriticsComboData(LanguageUse.Learning);

            void RefreshEntrySorting(DiacriticEntryKey.Letter entryLetter, LetterData letterData)
            {
                entryLetter.sortNumber = letterData.Number;
                entryLetter.id = letterData.Id;
                entryLetter.page = letterData.Base.Number;
                if (entryLetter.id.StartsWith("alef_hamza"))
                    entryLetter.page = 29;
                if (entryLetter.id.StartsWith("lam_alef"))
                    entryLetter.page = 30;
                if (entryLetter.id.StartsWith("alef_maqsura"))
                    entryLetter.page = 31;
                if (entryLetter.id.StartsWith("hamza"))
                    entryLetter.page = 32;
                if (entryLetter.id.StartsWith("teh_marbuta"))
                    entryLetter.page = 33;
                if (entryLetter.id.StartsWith("yeh_hamza"))
                    entryLetter.page = 34;
                if (letterData.IsOfKindCategory(LetterKindCategory.Symbol))
                    entryLetter.page = 35;
            }

            DiacriticEntryKey.Letter FindLetter(List<LetterData> dbLetters, string unicode)
            {
                var entryLetter = new DiacriticEntryKey.Letter();
                entryLetter.unicode = unicode;
                var l = dbLetters.FirstOrDefault(x => x.Isolated_Unicode == unicode);
                if (l != null)
                {
                    RefreshEntrySorting(entryLetter, l);
                    return entryLetter;
                }
                l = dbLetters.FirstOrDefault(x => x.Initial_Unicode == unicode);
                if (l != null)
                {
                    RefreshEntrySorting(entryLetter, l);
                    return entryLetter;
                }
                l = dbLetters.FirstOrDefault(x => x.Medial_Unicode == unicode);
                if (l != null)
                {
                    RefreshEntrySorting(entryLetter, l);
                    return entryLetter;
                }
                l = dbLetters.FirstOrDefault(x => x.Final_Unicode == unicode);
                if (l != null)
                {
                    RefreshEntrySorting(entryLetter, l);
                    return entryLetter;
                }
                return null;
            }

            // @note: use this to refill the table with all letters in the DB, if not already present
#if UNITY_EDITOR
            if (REFRESH_DIACRITIC_ENTRY_TABLE_FROM_LETTERS_DB)
            {
                var dbLetters = AppManager.I.DB.GetAllLetterData();

                // First, get rid of data that uses diacritics that we do not have
                var nBefore = diacriticsComboData.Keys.Count;
                diacriticsComboData.Keys.RemoveAll(k => !dbLetters.Any(l => l.GetUnicode().Equals(k.letter2.unicode)));
                var nAfter = diacriticsComboData.Keys.Count;

                Debug.LogError("Get rid of " + (nBefore - nAfter) + " wrong diacritic combos");

                var sortedDiacritics = dbLetters.Where(x => x.Type == LetterDataType.DiacriticSymbol).ToList();
                sortedDiacritics.Sort((d1, d2) => d1.Number - d2.Number);

                Debug.LogError("SORTED DIACRITICS: " + sortedDiacritics.ToJoinedString());

                // Then, fill with data from the letter DB
                for (var i = 0; i < dbLetters.Count; i++)
                {
                    var letter = dbLetters[i];
                    if (!letter.Active)
                        continue;
                    bool isSymbol = letter.IsOfKindCategory(LetterKindCategory.Symbol);
                    if (!letter.IsOfKindCategory(LetterKindCategory.BaseAndVariations) && !isSymbol)
                        continue;

                    Debug.LogError("Got Letter: " + letter.Id);

                    foreach (var letterForm in letter.GetAvailableForms())
                    {
                        var formUnicode = letter.GetUnicode(letterForm);
                        Debug.LogError($"With Letter form: {letterForm}({formUnicode})");

                        foreach (var diacritic in sortedDiacritics)
                        {
                            var diacriticUnicode = diacritic.GetUnicode();
                            Debug.LogError($"compare to diacritic: {diacritic}({diacriticUnicode})");

                            bool Comparison(DiacriticEntryKey x)
                            {
                                return x.letter1.unicode.Equals(formUnicode, StringComparison.OrdinalIgnoreCase) && x.letter2.unicode.Equals(diacriticUnicode, StringComparison.OrdinalIgnoreCase);
                            }

                            if ((isSymbol && diacriticUnicode != Shaddah) || formUnicode == Shaddah)
                            {
                                // Should not appear Remove them if there are
                                Debug.LogError("Removing keys!");
                                diacriticsComboData.Keys.RemoveAll(Comparison);
                                continue;
                            }

                            DiacriticEntryKey key = null;
                            try
                            {
                                if (diacriticsComboData.Keys.Any(Comparison))
                                {
                                    Debug.Log($"We already have key with unicode {formUnicode} and diacritic {diacriticUnicode}");
                                    key = diacriticsComboData.Keys.First(Comparison);

                                    // Refresh the ID with the current DB tho (names of letters may be different)
                                    key.letter1.id = letter.Id;
                                    key.letter2.id = diacritic.Id;
                                }
                                else
                                {
                                    Debug.LogError($"Generating key with unicode {formUnicode} and diacritic {diacriticUnicode}");
                                    key = new DiacriticEntryKey
                                    {
                                        letter1 = FindLetter(dbLetters, formUnicode),
                                        letter2 = FindLetter(dbLetters, diacriticUnicode)
                                    };
                                    diacriticsComboData.Keys.Add(key);
                                }
                            }
                            catch (System.Exception e) { Debug.LogWarning($"Ignoring exception: {e.Message}"); }

                            // Refresh page & sorting
                            //Debug.LogError("Check " + key.letter1.id + " " + key.letter2.id);
                            try
                            { if (key != null) RefreshEntrySorting(key.letter1, AppManager.I.DB.GetLetterDataById(key.letter1.id)); }
                            catch (System.Exception e) { Debug.LogWarning($"Ignoring exception: {e.Message}"); }
                            try
                            { if (key != null) RefreshEntrySorting(key.letter2, AppManager.I.DB.GetLetterDataById(key.letter2.id)); }
                            catch (System.Exception e) { Debug.LogWarning($"Ignoring exception: {e.Message}"); }
                        }
                    }
                }

            }
#endif

            if (REFRESH_DIACRITIC_ENTRY_TABLE_FROM_LETTERS_DB || REPOPULATE_DIACRITIC_ENTRY_TABLE_FROM_HARDCODED_COMBOS)
            {
                // Auto-sort the data like in the book
                diacriticsComboData.Keys = diacriticsComboData.Keys.OrderBy(key =>
                {
                    var letter = AppManager.I.DB.GetLetterDataById(key.letter1.id);
                    if (letter == null)
                        return 0;
                    var symbolOrder = diacriticsSortOrder.IndexOf(key.letter2.unicode);
                    switch (letter.Kind)
                    {
                        case LetterDataKind.LetterVariation:
                            return 10000 + symbolOrder;
                        case LetterDataKind.Symbol:
                            return 20000 + diacriticsSortOrder.IndexOf(key.letter1.unicode) * 100 + symbolOrder;
                            ;
                        default:
                            return key.letter1.sortNumber * 100 + symbolOrder;
                    }
                }).ToList();

#if UNITY_EDITOR
                EditorUtility.SetDirty(diacriticsComboData);
#endif
            }

            // Use the values in the data table instead
            DiacriticCombos2Fix.Clear();
            foreach (var key in diacriticsComboData.Keys)
            {
                DiacriticCombos2Fix.Add(new DiacriticComboEntry(key.letter1.unicode, key.letter2.unicode), new Vector2(key.offsetX, key.offsetY));
            }
        }


        private static bool REPOPULATE_DIACRITIC_ENTRY_TABLE_FROM_HARDCODED_COMBOS = false;
        public static bool REFRESH_DIACRITIC_ENTRY_TABLE_FROM_LETTERS_DB = false;

        public void RebuildDiacriticCombos()
        {
            BuildDiacriticCombos2Fix();
        }

        private Vector2 FindDiacriticCombo2Fix(string Unicode1, string Unicode2)
        {
            if (DiacriticCombos2Fix == null)
            {
                BuildDiacriticCombos2Fix();
            }

            Vector2 newDelta = new Vector2(0, 0);
            var diacriticsComboData = LanguageSwitcher.I.GetDiacriticsComboData(LanguageUse.Learning);
            var combo = diacriticsComboData.Keys.FirstOrDefault(x => x.letter1.unicode == Unicode1
                                                                     && x.letter2.unicode == Unicode2);
            if (combo != null)
            {
                newDelta.x = combo.offsetX;
                newDelta.y = combo.offsetY;
            }
            return newDelta;
        }

        /// <summary>
        /// Adjusts the diacritic positions.
        /// </summary>
        /// <returns><c>true</c>, if diacritic positions was adjusted, <c>false</c> otherwise.</returns>
        /// <param name="textInfo">Text info.</param>
        public override bool FixTMProDiacriticPositions(TMPro.TMP_TextInfo textInfo)
        {
            int characterCount = textInfo.characterCount;
            if (characterCount <= 1)
                return false;

            bool changed = false;
            for (int charPosition = 0; charPosition < characterCount - 1; charPosition++)
            {
                var char1Pos = charPosition;
                var char2Pos = charPosition + 1;
                var UnicodeChar1 = GetHexUnicodeFromChar(textInfo.characterInfo[char1Pos].character);
                var UnicodeChar2 = GetHexUnicodeFromChar(textInfo.characterInfo[char2Pos].character);

                changed = true;


                bool char1IsDiacritic = (UnicodeChar1 == Dammah || UnicodeChar1 == Fathah || UnicodeChar1 == Sukun || UnicodeChar1 == Kasrah);
                bool char2IsDiacritic = (UnicodeChar2 == Dammah || UnicodeChar2 == Fathah || UnicodeChar2 == Sukun || UnicodeChar2 == Kasrah);

                bool char1IsShaddah = UnicodeChar1 == Shaddah;
                bool char2IsShaddah = UnicodeChar2 == Shaddah;

                if (char1IsDiacritic && char2IsShaddah)
                {
                    CopyPosition(textInfo, char2Pos, char1Pos);             // Place the diacritic where the shaddah is
                    ApplyOffset(textInfo, char1Pos, FindDiacriticCombo2Fix(UnicodeChar1, UnicodeChar2));    // then, move the diacritic in respect to the shaddah using the delta
                }
                else if (char1IsShaddah && char2IsDiacritic)
                {
                    CopyPosition(textInfo, char1Pos, char2Pos);             // Place the diacritic where the shaddah is
                    ApplyOffset(textInfo, char2Pos, FindDiacriticCombo2Fix(UnicodeChar2, UnicodeChar1));    // then, move the diacritic in respect to the shaddah using the delta
                }
                else
                {
                    // Move the symbol in respect to the base letter
                    //Debug.LogError($"Mod for {UnicodeChar1} to {UnicodeChar2}: {modificationDelta}");
                    ApplyOffset(textInfo, char2Pos, FindDiacriticCombo2Fix(UnicodeChar1, UnicodeChar2));

                    // If we get a Diacritic and the next char is a Shaddah, however, we need to instead first move the shaddah, then move the diacritic in respect to the shaddah
                    if (charPosition < characterCount - 2)
                    {
                        var UnicodeChar3 = GetHexUnicodeFromChar(textInfo.characterInfo[charPosition + 2].character);
                        bool char3IsDiacritic = (UnicodeChar3 == Dammah || UnicodeChar3 == Fathah || UnicodeChar3 == Sukun || UnicodeChar3 == Kasrah);
                        bool char3IsShaddah = UnicodeChar3 == Shaddah;
                        var char3Pos = charPosition + 2;

                        // Place this Shaddah in respect to the letter
                        if (char2IsDiacritic && char3IsShaddah)
                        {
                            ApplyOffset(textInfo, char3Pos, FindDiacriticCombo2Fix(UnicodeChar1, UnicodeChar3));
                        }
                        else if (char2IsShaddah && char3IsDiacritic)
                        {
                            //Debug.LogWarning(textInfo.textComponent.text + " " + " has weird shaddah & diacritic placement (shaddah is before the diacritic)");
                            ApplyOffset(textInfo, char2Pos, FindDiacriticCombo2Fix(UnicodeChar1, UnicodeChar2));
                        }
                    }

                }
            }
            return changed;
        }

        private void ApplyOffset(TMPro.TMP_TextInfo textInfo, int charPosition, Vector2 modificationDelta)
        {
            int materialIndex2 = textInfo.characterInfo[charPosition].materialReferenceIndex;
            int vertexIndex2 = textInfo.characterInfo[charPosition].vertexIndex;
            Vector3[] Vertices2 = textInfo.meshInfo[materialIndex2].vertices;

            float charsize2 = (Vertices2[vertexIndex2 + 2].y - Vertices2[vertexIndex2 + 0].y);
            float dx2 = charsize2 * modificationDelta.x / 100f;
            float dy2 = charsize2 * modificationDelta.y / 100f;
            Vector3 offset2 = new Vector3(dx2, dy2, 0f);

            Vertices2[vertexIndex2 + 0] = Vertices2[vertexIndex2 + 0] + offset2;
            Vertices2[vertexIndex2 + 1] = Vertices2[vertexIndex2 + 1] + offset2;
            Vertices2[vertexIndex2 + 2] = Vertices2[vertexIndex2 + 2] + offset2;
            Vertices2[vertexIndex2 + 3] = Vertices2[vertexIndex2 + 3] + offset2;
        }

        private void CopyPosition(TMPro.TMP_TextInfo textInfo, int charFrom, int charTo)
        {
            int materialIndex2 = textInfo.characterInfo[charTo].materialReferenceIndex;
            int vertexIndex2 = textInfo.characterInfo[charTo].vertexIndex;
            Vector3[] Vertices2 = textInfo.meshInfo[materialIndex2].vertices;

            int materialIndex1 = textInfo.characterInfo[charFrom].materialReferenceIndex;
            int vertexIndex1 = textInfo.characterInfo[charFrom].vertexIndex;
            Vector3[] Vertices1 = textInfo.meshInfo[materialIndex1].vertices;

            Vertices2[vertexIndex2 + 0] = Vertices1[vertexIndex1 + 0];
            Vertices2[vertexIndex2 + 1] = Vertices1[vertexIndex1 + 1];
            Vertices2[vertexIndex2 + 2] = Vertices1[vertexIndex1 + 2];
            Vertices2[vertexIndex2 + 3] = Vertices1[vertexIndex1 + 3];
        }

        public override string DebugShowDiacriticFix(string unicode1, string unicode2)
        {
            var delta = FindDiacriticCombo2Fix(unicode1, unicode2);
            return
                string.Format(
                    "DiacriticCombos2Fix.Add(new DiacriticComboEntry(\"{0}\", \"{1}\"), new Vector2({2}, {3}));",
                    unicode1, unicode2, delta.x, delta.y);
        }

        #endregion


        #endregion

    }
}
