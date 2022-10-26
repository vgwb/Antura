using Antura.Core;
using Antura.Helpers;
using Antura.Language;
using Antura.Teacher;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Antura.Database
{

    /// <summary>
    /// Provides helpers to get correct letter/word/phrase data according to the teacher's logic and based on the player's progression
    /// </summary>
    public class VocabularyHelper
    {
        private DatabaseManager dbManager;

        public VocabularyHelper(DatabaseManager _dbManager)
        {
            dbManager = _dbManager;
        }

        #region Letter Utilities

        public bool CheckFilters(LetterFilters filters, LetterData data)
        {
            if (filters.requireDiacritics && !data.IsOfKindCategory(LetterKindCategory.DiacriticCombo)) { return false; }
            if (!FilterByDiacritics(filters.excludeDiacritics, data)) { return false; }
            if (!FilterByLetterVariations(filters.excludeLetterVariations, data)) { return false; }
            if (!FilterByDipthongs(filters.excludeDiphthongs, data)) { return false; }
            if (!FilterByKind(!filters.includeAccentedLetters, data, LetterDataKind.AccentedLetter)) { return false; }
            if (!FilterByKind(!filters.includeSpecialCharacters, data, LetterDataKind.SpecialChar)) { return false; }
            if (filters.excludeMultiCharacterLetters && data.Isolated.Length > 1) { return false; }

            // always skip symbols
            if (data.IsOfKindCategory(LetterKindCategory.Symbol))
            {
                return false;
            }
            return true;
        }


        public bool FilterByDiacritics(LetterFilters.ExcludeDiacritics excludeDiacritics, LetterData data)
        {
            switch (excludeDiacritics)
            {
                case LetterFilters.ExcludeDiacritics.All:
                    if (data.IsOfKindCategory(LetterKindCategory.DiacriticCombo))
                    {
                        return false;
                    }
                    break;
                case LetterFilters.ExcludeDiacritics.AllButMain:
                    var symbol = GetSymbolOf(data.Id);
                    if (symbol != null && data.IsOfKindCategory(LetterKindCategory.DiacriticCombo) &&
                        symbol.Tag != "MainDiacritic")
                    {
                        return false;
                    }
                    break;
                default:
                    break;
            }
            return true;
        }

        public bool FilterByLetterVariations(LetterFilters.ExcludeLetterVariations excludeLetterVariations, LetterData data)
        {
            switch (excludeLetterVariations)
            {
                case LetterFilters.ExcludeLetterVariations.All:
                    if (data.IsOfKindCategory(LetterKindCategory.LetterVariation))
                    {
                        return false;
                    }
                    break;
                case LetterFilters.ExcludeLetterVariations.AllButAlefHamza:
                    if (data.IsOfKindCategory(LetterKindCategory.LetterVariation) && data.Tag != "AlefHamzaVariation")
                    {
                        return false;
                    }
                    break;
                default:
                    break;
            }
            return true;
        }

        public bool FilterByDipthongs(bool excludeDiphthongs, LetterData data)
        {
            if (excludeDiphthongs && data.Kind == LetterDataKind.Diphthong)
            {
                return false;
            }
            return true;
        }

        public bool FilterByKind(bool exclude, LetterData data, LetterDataKind kind)
        {
            if (exclude && data.Kind == kind)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Letter -> Letter

        public List<LetterData> GetAllBaseLetters()
        {
            var p = new LetterFilters(excludeDiacritics: LetterFilters.ExcludeDiacritics.All,
                excludeLetterVariations: LetterFilters.ExcludeLetterVariations.All, excludeDiphthongs: true);
            return GetAllLetters(p);
        }

        public List<LetterData> GetAllLetters(LetterFilters filters)
        {
            return dbManager.FindLetterData(x => CheckFilters(filters, x));
        }

        public List<LetterData> GetAllLettersAndForms(LetterFilters filters)
        {
            var baseLetters = dbManager.FindLetterData(x => CheckFilters(filters, x));
            var lettersWithForms = ExtractLettersWithForms(baseLetters);
            return lettersWithForms;
        }

        public List<LetterData> GetLettersNotIn(LetterEqualityStrictness equalityStrictness, LetterFilters filters, params LetterData[] tabooArray)
        {
            var comparer = new StrictLetterDataComparer(equalityStrictness);
            var lettersWithForms = GetAllLettersAndForms(filters);
            return lettersWithForms.Where(x => !tabooArray.Contains(x, comparer)).ToList();
        }

        public List<LetterData> GetLettersByKind(LetterDataKind choice)
        {
            return dbManager.FindLetterData(x => x.Kind == choice); // @note: this does not use filters, special case
        }

        public List<LetterData> GetLettersBySunMoon(LetterDataSunMoon choice, LetterFilters filters)
        {
            return dbManager.FindLetterData(x => x.SunMoon == choice && CheckFilters(filters, x));
        }

        public List<LetterData> GetConsonantLetter(LetterFilters filters)
        {
            return dbManager.FindLetterData(x =>
                x.Type == LetterDataType.Consonant || x.Type == LetterDataType.Powerful && CheckFilters(filters, x));
        }

        public List<LetterData> GetVowelLetter(LetterFilters filters)
        {
            return dbManager.FindLetterData(x => x.Type == LetterDataType.LongVowel && CheckFilters(filters, x));
        }

        public List<LetterData> GetLettersByType(LetterDataType choice, LetterFilters filters)
        {
            return dbManager.FindLetterData(x => x.Type == choice && CheckFilters(filters, x));
        }

        public LetterData GetAccentedBase(string letterId)
        {
            var data = dbManager.GetLetterDataById(letterId);

            if (!data.IsOfKindCategory(LetterKindCategory.AccentedLetter))
                return data;

            if (string.IsNullOrEmpty(data.BaseLetter))
                return data;

            return dbManager.FindLetterData(x => x.Id == data.BaseLetter)[0];
        }

        public LetterData GetSymbolOf(string letterId)
        {
            var data = dbManager.GetLetterDataById(letterId);
            if (data.Symbol == "")
            {
                return null;
            }

            var symbols = dbManager.FindLetterData(x => x.Id == data.Symbol);
            if (symbols.Count == 0)
            {
                throw new Exception("Could not find any symbol named " + data.Symbol);
            }
            return symbols[0];
        }

        public List<LetterData> GetLettersWithBase(string letterId)
        {
            var baseData = dbManager.GetLetterDataById(letterId);
            return dbManager.FindLetterData(x => x.BaseLetter == baseData.Id);
        }


        public List<LetterData> ExtractLettersWithForms(IEnumerable<LetterData> letters)
        {
            List<LetterData> lettersWithForms = new List<LetterData>();
            foreach (var letter in letters)
            {
                lettersWithForms.AddRange(ExtractLettersWithForms(letter));
            }
            return lettersWithForms;
        }

        public List<LetterData> ExtractLettersWithForms(LetterData baseForVariation)
        {
            return new List<LetterForm>(baseForVariation.GetAvailableForms()).ConvertAll(f =>
            {
                var l = baseForVariation.Clone();
                l.ForcedLetterForm = f;
                return l;
            });
        }

        public LetterData ConvertToLetterWithForcedForm(LetterData baseForVariation, LetterForm form)
        {
            var l = baseForVariation.Clone();
            l.ForcedLetterForm = form;
            return l;
        }

        public List<LetterData> GetAllLetterAlterations(List<LetterData> baseLetters, LetterAlterationFilters letterAlterationFilters)
        {
            List<LetterData> letterPool = new List<LetterData>();

            // Filter: only 1 base or multiple bases?
            if (!letterAlterationFilters.differentBaseLetters)
            {
                var chosenLetter = baseLetters.RandomSelectOne();
                baseLetters.Clear();
                baseLetters.Add(chosenLetter);
            }

            // Get all alterations for the given bases
            foreach (var baseLetter in baseLetters)
            {
                // Check all alterations of this base letter
                var letterAlterations = GetLettersWithBase(baseLetter.GetId());
                List<LetterData> availableVariations = new List<LetterData>();
                foreach (var letterData in letterAlterations)
                {
                    if (letterAlterationFilters.requireDiacritics && !letterData.IsOfKindCategory(LetterKindCategory.DiacriticCombo))
                        continue;
                    if (!FilterByDiacritics(letterAlterationFilters.ExcludeDiacritics, letterData))
                        continue;
                    if (!FilterByLetterVariations(letterAlterationFilters.ExcludeLetterVariations, letterData))
                        continue;
                    if (!FilterByDipthongs(letterAlterationFilters.excludeDipthongs, letterData))
                        continue;
                    availableVariations.Add(letterData);
                }
                //Debug.Log("N availableVariations  " + availableVariations.Count + "  for " + baseLetter.GetId());

                List<LetterData> basesForForms = new List<LetterData>(availableVariations);
                basesForForms.Add(baseLetter);
                //Debug.Log("N bases for forms: " +  basesForForms.Count);
                if (letterAlterationFilters.includeForms)
                {
                    // Place forms only inside the pool, if needed
                    foreach (var baseForForm in basesForForms)
                    {
                        var availableForms = ExtractLettersWithForms(baseForForm);

                        if (letterAlterationFilters.oneFormPerLetter)
                        {
                            // If we are using forms and only one form per letter must appear, add just one at random
                            letterPool.Add(availableForms.RandomSelectOne());
                        }
                        else
                        {
                            // Add all the (different) forms
                            var visualFormComparer = new StrictLetterDataComparer(LetterEqualityStrictness.WithVisualForm);
                            foreach (var availableForm in availableForms)
                            {
                                if (letterAlterationFilters.visuallyDifferentForms && letterPool.Contains(availableForm, visualFormComparer))
                                {
                                    continue;
                                }
                                letterPool.Add(availableForm);
                            }
                        }
                    }
                }
                else
                {
                    // Place just the isolated versions
                    letterPool.AddRange(basesForForms);
                }
            }

            return letterPool;
        }

        #endregion

        #region Word -> Letter

        private Dictionary<string, List<LetterData>> wordsToLetterCache = new Dictionary<string, List<LetterData>>();

        public List<LetterData> GetLettersInWord(WordData wordData, bool keepBasesOnly = false)
        {
            // @note: this will always retrieve all letters with their forms, the strictness will then define whether that has any consequence or not
            List<LetterData> letters = null;
            var dictCache = wordsToLetterCache;
            if (!dictCache.ContainsKey(wordData.Id))
            {
                var parts = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).SplitWord(dbManager, wordData);
                letters = parts.ConvertAll(x => ConvertToLetterWithForcedForm(x.letter, x.letterForm));
                dictCache[wordData.Id] = letters;
            }
            letters = dictCache[wordData.Id];

            if (keepBasesOnly)
            {
                letters = letters.ConvertAll(ld => ConvertToLetterWithForcedForm(ld.Base, ld.Form));
            }

            return letters;
        }


        public List<LetterData> GetLettersNotInWords(LetterEqualityStrictness equalityStrictness = LetterEqualityStrictness.LetterBase, params WordData[] tabooArray)
        {
            return GetLettersNotInWords(LetterKindCategory.Real, equalityStrictness, tabooArray);
        }

        public List<LetterData> GetLettersNotInWords(LetterKindCategory category = LetterKindCategory.Real, LetterEqualityStrictness equalityStrictness = LetterEqualityStrictness.LetterBase, params WordData[] tabooArray)
        {
            var comparer = new StrictLetterDataComparer(equalityStrictness);
            var lettersInWords = new HashSet<LetterData>(comparer);
            foreach (var tabooWordData in tabooArray)
            {
                var tabooWordDataLetters = GetLettersInWord(tabooWordData);
                lettersInWords.UnionWith(tabooWordDataLetters);
            }
            var lettersNotInWords = dbManager.FindLetterData(x => !lettersInWords.Contains(x, comparer) && x.IsOfKindCategory(category));
            return lettersNotInWords;
        }

        public List<LetterData> GetLettersNotInWord(WordData wordData, LetterKindCategory category = LetterKindCategory.Real, LetterEqualityStrictness equalityStrictness = LetterEqualityStrictness.LetterBase)
        {
            var comparer = new StrictLetterDataComparer(equalityStrictness);
            var lettersInWord = GetLettersInWord(wordData);
            var lettersNotInWord = dbManager.FindLetterData(x => !lettersInWord.Contains(x, comparer) && x.IsOfKindCategory(category));
            return lettersNotInWord;
        }

        public List<LetterData> GetCommonLettersInWords(LetterEqualityStrictness letterEqualityStrictness, params WordData[] words)
        {
            var comparer = new StrictLetterDataComparer(letterEqualityStrictness);
            Dictionary<LetterData, int> countDict = new Dictionary<LetterData, int>(comparer);
            foreach (var word in words)
            {
                var nonRepeatingLettersOfWord = new HashSet<LetterData>(comparer);

                var letters = GetLettersInWord(word);
                foreach (var letter in letters)
                {
                    nonRepeatingLettersOfWord.Add(letter);
                }

                foreach (var letter in nonRepeatingLettersOfWord)
                {
                    if (!countDict.ContainsKey(letter))
                        countDict[letter] = 0;
                    countDict[letter] += 1;
                }
            }

            // Get only these letters that are in all words
            var commonLettersList = new List<LetterData>();
            foreach (var letter in countDict.Keys)
            {
                if (countDict[letter] == words.Length)
                {
                    commonLettersList.Add(letter);
                }
            }

            return commonLettersList;
        }

        public List<LetterData> GetNotCommonLettersInWords(LetterFilters letterFilters, LetterEqualityStrictness letterEqualityStrictness, WordData[] words)
        {
            var commonLetters = GetCommonLettersInWords(letterEqualityStrictness, words);
            var nonCommonLetters = GetAllLettersAndForms(letterFilters);
            var nonCommonLettersWithComparer = new HashSet<LetterData>(new StrictLetterDataComparer(letterEqualityStrictness));
            nonCommonLettersWithComparer.UnionWith(nonCommonLetters);
            foreach (var commonLetter in commonLetters)
            {
                nonCommonLettersWithComparer.Remove(commonLetter);
            }
            return nonCommonLettersWithComparer.ToList();
        }

        #endregion

        #region Word Utilities

        private bool CheckFilters(WordFilters filters, WordData data)
        {
            if (filters.allowedCategories != null && !filters.allowedCategories.Contains(data.Category))
                return false;

            if (filters.excludeArticles && data.Article != WordDataArticle.None)
            {
                return false;
            }
            if (filters.requireDrawings && !data.HasDrawing())
            {
                return false;
            }
            if (filters.excludeColorWords && data.Category == WordDataCategory.Colors)
            {
                return false;
            }
            if (filters.excludePluralDual && data.Form != WordDataForm.Singular)
            {
                return false;
            }
            if (filters.excludeDiacritics && WordHasDiacriticCombo(data))
            {
                return false;
            }
            if (filters.excludeLetterVariations && WordHasLetterVariations(data))
            {
                return false;
            }
            if (filters.requireDiacritics && !WordHasDiacriticCombo(data))
            {
                return false;
            }
            if (filters.excludeDipthongs && WordHasDipthongs(data))
            {
                return false;
            }
            if (filters.excludeDuplicateLetters && WordContainsDuplicateLetters(data))
            {
                return false;
            }
            if (filters.excludeSpaces && WordContainsLetter(data, AppManager.I.DB.GetLetterDataById(" ")))
            {
                return false;
            }
            return true;
        }

        private bool WordHasDiacriticCombo(WordData data)
        {
            foreach (var letter in GetLettersInWord(data))
            {
                if (letter.IsOfKindCategory(LetterKindCategory.DiacriticCombo))
                {
                    return true;
                }
            }
            return false;
        }

        private bool WordHasDipthongs(WordData word)
        {
            foreach (var letter in GetLettersInWord(word))
            {
                if (letter.Kind == LetterDataKind.Diphthong)
                {
                    return true;
                }
            }
            return false;
        }

        private bool WordHasLetterVariations(WordData data)
        {
            foreach (var letter in GetLettersInWord(data))
            {
                if (letter.IsOfKindCategory(LetterKindCategory.LetterVariation))
                {
                    return true;
                }
            }
            return false;
        }

        public bool WordContainsDuplicateLetters(WordData selectedWord, LetterEqualityStrictness letterEqualityStrictness = LetterEqualityStrictness.LetterBase)
        {
            var wordLetters = GetLettersInWord(selectedWord);
            return wordLetters.GroupBy(x => x.Id).Any(g => g.Count() > 1);
        }

        public bool WordHasSpecialCharacters(WordData selectedWord)
        {
            var wordLetters = GetLettersInWord(selectedWord);
            return wordLetters.Any(x => x.IsOfKindCategory(LetterKindCategory.SpecialChar));
        }

        public int WordContainsLetterTimes(WordData selectedWord, LetterData containedLetter, LetterEqualityStrictness letterEqualityStrictness = LetterEqualityStrictness.LetterBase)
        {
            var wordLetters = GetLettersInWord(selectedWord);
            int count = 0;
            foreach (var letter in wordLetters)
                if (letter.IsSameLetterAs(containedLetter, letterEqualityStrictness))
                    count++;
            return count;
        }

        public bool WordContainsLetter(WordData selectedWord, LetterData containedLetter, LetterEqualityStrictness letterEqualityStrictness = LetterEqualityStrictness.LetterBase)
        {
            //if (containedLetter.Id == "lam_alef") Debug.Log("Looking for lam-alef in " + selectedWord);
            //foreach (var l in ArabicAlphabetHelper.FindLetter(dbManager, selectedWord, containedLetter))
            //if (l.letter.Id == "lam_alef") Debug.Log("Lam alef form " + l.letterForm + " in word " + selectedWord);
            var lettersInWord = GetLettersInWord(selectedWord);
            return lettersInWord.Any(x => x.IsSameLetterAs(containedLetter, letterEqualityStrictness));
        }

        /// <summary>
        /// tranformsf the hex string of the glyph into the corresponding char
        /// </summary>
        /// <returns>The drawing string</returns>
        /// <param name="word">WordData.</param>
        public string GetWordDrawing(WordData word)
        {
            //Debug.Log("the int of hex:" + word.Drawing + " is " + int.Parse(word.Drawing, NumberStyles.HexNumber));
            if (word.DrawingId != "")
            {
                var data = word.GetDrawingData();
                if (data != null)
                {
                    string drawingHexCode = data.Unicode;

                    if (int.TryParse(drawingHexCode, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out int result))
                    {
                        return ((char)result).ToString();
                    }
                    return "";
                }
                else
                {
                    Debug.LogError("Drawing not found - " + word.Id);
                    return "";
                }
            }
            return "";
        }

        #endregion

        #region Word -> Word

        public List<WordData> GetAllWords(WordFilters filters)
        {
            return dbManager.FindWordData(x => CheckFilters(filters, x));
        }

        public List<WordData> GetWordsNotIn(WordFilters filters, List<WordData> tabooWords)
        {
            return dbManager.FindWordData(word => !tabooWords.Contains(word) && CheckFilters(filters, word));
        }

        public IEnumerable<WordData> GetWordsNotInOptimized(WordFilters filters, List<WordData> tabooWords)
        {
            return dbManager.FindWordDataOptimized(word => !tabooWords.Contains(word) && CheckFilters(filters, word));
        }

        public List<WordData> GetWordsNotIn(WordFilters filters, params WordData[] tabooWords)
        {
            var tabooList = new List<WordData>(tabooWords);
            return GetWordsNotIn(filters, tabooList);
        }

        public List<WordData> GetWordsByArticle(WordDataArticle choice, WordFilters filters)
        {
            return dbManager.FindWordData(x => x.Article == choice && CheckFilters(filters, x));
        }

        public List<WordData> GetWordsByForm(WordDataForm choice, WordFilters filters)
        {
            return dbManager.FindWordData(x => x.Form == choice && CheckFilters(filters, x));
        }

        public List<WordData> GetWordsByKind(WordDataKind choice, WordFilters filters)
        {
            return dbManager.FindWordData(x => x.Kind == choice && CheckFilters(filters, x));
        }

        #endregion

        #region Letter -> Word

        public List<WordData> GetWordsWithLetter(WordFilters filters, LetterData okLetter, LetterEqualityStrictness letterEqualityStrictness = LetterEqualityStrictness.LetterBase)
        {
            return GetWordsByLetters(filters, new[] { okLetter }, null, letterEqualityStrictness);
        }

        public List<WordData> GetWordsWithLetters(WordFilters filters, LetterEqualityStrictness letterEqualityStrictness = LetterEqualityStrictness.LetterBase, params LetterData[] okLetters)
        {
            return GetWordsByLetters(filters, okLetters, null, letterEqualityStrictness);
        }

        public List<WordData> GetWordsWithoutLetter(WordFilters filters, LetterData tabooLetter, LetterEqualityStrictness letterEqualityStrictness = LetterEqualityStrictness.Letter)
        {
            return GetWordsByLetters(filters, null, new[] { tabooLetter }, letterEqualityStrictness);
        }

        public List<WordData> GetWordsWithoutLetters(WordFilters filters, LetterEqualityStrictness letterEqualityStrictness = LetterEqualityStrictness.LetterBase, params LetterData[] tabooLetters)
        {
            return GetWordsByLetters(filters, null, tabooLetters, letterEqualityStrictness);
        }

        private List<WordData> GetWordsByLetters(WordFilters filters, LetterData[] okLettersArray, LetterData[] tabooLettersArray, LetterEqualityStrictness letterEqualityStrictness = LetterEqualityStrictness.LetterBase)
        {
            if (okLettersArray == null)
                okLettersArray = new LetterData[] { };
            if (tabooLettersArray == null)
                tabooLettersArray = new LetterData[] { };

            var okLetters = new HashSet<LetterData>(okLettersArray);
            var tabooLetters = new HashSet<LetterData>(tabooLettersArray);

            var comparer = new StrictLetterDataComparer(letterEqualityStrictness);

            List<WordData> wordsByLetters = dbManager.FindWordData(word =>
            {
                if (!CheckFilters(filters, word))
                { return false; }

                var lettersInWord = GetLettersInWord(word);

                if (tabooLetters.Count > 0)
                {
                    foreach (var letter in lettersInWord)
                    {
                        if (tabooLetters.Contains(letter, comparer))
                        {
                            return false;
                        }
                    }
                }

                if (okLetters.Count > 0)
                {
                    bool hasAllOkLetters = true;
                    foreach (var okLetter in okLetters)
                    {
                        bool hasThisLetter = false;
                        foreach (var letter in lettersInWord)
                        {
                            if (letter.IsSameLetterAs(okLetter, letterEqualityStrictness))
                            {
                                hasThisLetter = true;
                                break;
                            }
                        }
                        if (!hasThisLetter)
                        {
                            hasAllOkLetters = false;
                            break;
                        }
                    }
                    if (!hasAllOkLetters)
                        return false;
                }
                return true;
            }
            );
            return wordsByLetters;
        }

        public bool WordContainsAnyLetter(WordData word, IEnumerable<LetterData> letters, LetterEqualityStrictness equalityStrictness = LetterEqualityStrictness.LetterBase)
        {
            var comparer = new StrictLetterDataComparer(equalityStrictness);
            var containedLetters = GetLettersInWord(word);
            foreach (var letter in letters)
            {
                if (containedLetters.Contains(letter, comparer))
                {
                    return true;
                }
            }
            return false;
        }

        public bool WordHasAllLettersInCommonWith(WordData word, List<WordData> words, LetterEqualityStrictness equalityStrictness = LetterEqualityStrictness.LetterBase)
        {
            var lettersInWord = GetLettersInWord(word);
            foreach (var letter in lettersInWord)
            {
                if (!IsLetterContainedInAnyWord(letter, words, equalityStrictness))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsLetterContainedInAnyWord(LetterData letter, List<WordData> words, LetterEqualityStrictness equalityStrictness = LetterEqualityStrictness.LetterBase)
        {
            var comparer = new StrictLetterDataComparer(equalityStrictness);
            foreach (var word in words)
            {
                var containedLetters = GetLettersInWord(word);
                if (containedLetters.Contains(letter, comparer))
                {
                    return true;
                }
            }
            return false;
        }

        public bool AnyWordContainsLetter(LetterData letter, IEnumerable<WordData> words, LetterEqualityStrictness equalityStrictness = LetterEqualityStrictness.LetterBase)
        {
            var comparer = new StrictLetterDataComparer(equalityStrictness);
            foreach (var word in words)
            {
                if (GetLettersInWord(word).Contains(letter, comparer))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Phrase -> Word

        /// <summary>
        /// Gets the words in phrase, taken from field Words of data Pharse. these words are set manually in the db
        /// </summary>
        /// <returns>The words in phrase.</returns>
        /// <param name="phraseId">Phrase identifier.</param>
        /// <param name="wordFilters">Word filters.</param>
        public List<WordData> GetWordsInPhrase(string phraseId, WordFilters wordFilters = null)
        {
            if (wordFilters == null)
            { wordFilters = new WordFilters(); }
            var phraseData = dbManager.GetPhraseDataById(phraseId);
            return GetWordsInPhrase(phraseData, wordFilters);
        }

        // Words contained in the 'Words' column of the Phrase Data, for playing purposes
        public List<WordData> GetWordsInPhrase(PhraseData phraseData, WordFilters wordFilters = null)
        {
            if (wordFilters == null)
            { wordFilters = new WordFilters(); }
            var words_ids_list = new List<string>(phraseData.Words);
            var inputList = dbManager.FindWordData(x => words_ids_list.Contains(x.Id) && CheckFilters(wordFilters, x));
            var orderedOutputList = new List<WordData>();
            words_ids_list.ForEach(id =>
            {
                var word = inputList.Find(x => x.Id.Equals(id));
                if (word != null)
                {
                    orderedOutputList.Add(word);
                }
            });
            return orderedOutputList;
        }

        bool IsAllUpperCase(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                // @note: this does not consider non-letter chars
                if (Char.IsLetter(input[i]) && !Char.IsUpper(input[i]))
                    return false;
            }
            return true;
        }

        public class WordDataWrapper
        {
            public bool Selected;
            public WordData WD;
        }

        // Words contained in the text of the phrase
        public List<WordDataWrapper> GetWordsFromPhraseText(PhraseData phraseData, WordFilters wordFilters = null)
        {
            var gameWords = GetWordsInPhrase(phraseData, wordFilters);

            var outputDatas = new List<WordDataWrapper>();
            var phraseText = phraseData.Text;
            var wordsInString = phraseText.Split(new[] { ' ' }, StringSplitOptions.None).ToList();
            for (int iWord = wordsInString.Count - 1; iWord >= 0; iWord--)
            {
                // Separate apostrophes and place them on the first word
                var splits = wordsInString[iWord].Split(new[] { '\''}, StringSplitOptions.None).ToList();
                if (splits.Count >= 2)
                {
                    wordsInString.RemoveAt(iWord);
                    wordsInString.Insert(iWord, splits[1]);
                    wordsInString.Insert(iWord, splits[0] + "\'");
                }
            }
            var uppercaseWords = wordsInString.Where(s => s.Length >= 2 && IsAllUpperCase(s));

            var uppercaseText = "";
            foreach (string uppercaseWord in uppercaseWords) uppercaseText += uppercaseWord + " ";
            uppercaseText = uppercaseText.Trim();
            uppercaseText = new string(uppercaseText.Where(c => !char.IsPunctuation(c) || c is '-').ToArray());

            var foundUppercase = false;
            for (var i = 0; i < wordsInString.Count; i++)
            {
                var wordText = wordsInString[i];
                // We ignore apostrophes when deciding if a word is found or not
                var splits = wordText.Split('\'');
                var strippedWordText = splits[splits.Length - 1];

                if (strippedWordText.Length >= 2 && IsAllUpperCase(strippedWordText))
                {
                    if (foundUppercase) continue;
                    foundUppercase = true;
                    strippedWordText = uppercaseText;
                    wordText = uppercaseText;
                }

                var wd = gameWords.FirstOrDefault(w => w.Text.Equals(strippedWordText, StringComparison.OrdinalIgnoreCase));
                var wrapper = new WordDataWrapper();
                if (wd != null)
                    wrapper.WD = new WordData { Id = wd.Id, Text = wordText.ToUpper() };
                else
                    wrapper.WD = new WordData { Id = $"RUNTIME-{wordText}", Text = wordText.ToUpper() };
                wrapper.Selected = strippedWordText.Length >= 2 && IsAllUpperCase(strippedWordText);
                outputDatas.Add(wrapper);
            }

            return outputDatas;
        }

        public List<WordData> GetAnswersToPhrase(PhraseData phraseData, WordFilters wordFilters = null)
        {
            if (wordFilters == null)
            { wordFilters = new WordFilters(); }
            var words_ids_list = new List<string>(phraseData.Answers);
            var list = dbManager.FindWordData(x => words_ids_list.Contains(x.Id) && CheckFilters(wordFilters, x));
            return list;
        }

        #endregion

        #region Phrase filters

        private bool CheckFilters(WordFilters wordFilters, PhraseFilters phraseFilters, PhraseData data)
        {
            // Words are checked with filters. At least 1 must fulfill the requirement.
            var words = GetWordsInPhrase(data, wordFilters);
            int nOkWords = words.Count;

            var answers = GetAnswersToPhrase(data, wordFilters);
            int nOkAnswers = answers.Count;

            var wordsInPhraseText = GetWordsFromPhraseText(data, wordFilters);

            if (phraseFilters.maxLength != 0 && data.Text.Length > phraseFilters.maxLength)
            {
                return false;
            }

            if (phraseFilters.maxWords != 0 && wordsInPhraseText.Count > phraseFilters.maxWords)
            {
                return false;
            }

            if (phraseFilters.requireWords && nOkWords == 0)
            {
                return false;
            }
            if (phraseFilters.requireAnswers && nOkAnswers == 0)
            {
                return false;
            }
            if (phraseFilters.requireAtLeastTwoWords && nOkWords <= 1)
            {
                return false;
            }
            if (phraseFilters.requireAnswersOrWords && nOkAnswers == 0 && nOkWords == 0)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Phrase -> Phrase

        public List<PhraseData> GetAllPhrases(WordFilters wordFilters, PhraseFilters phraseFilters)
        {
            return dbManager.FindPhraseData(x => CheckFilters(wordFilters, phraseFilters, x));
        }

        public List<PhraseData> GetPhrasesByCategory(PhraseDataCategory choice, WordFilters wordFilters, PhraseFilters phraseFilters)
        {
            return dbManager.FindPhraseData(x => x.Category == choice && CheckFilters(wordFilters, phraseFilters, x));
        }

        public List<PhraseData> GetPhrasesNotIn(WordFilters wordFilters, PhraseFilters phraseFilters, params PhraseData[] tabooArray)
        {
            var tabooList = new List<PhraseData>(tabooArray);
            return dbManager.FindPhraseData(x => !tabooList.Contains(x) && CheckFilters(wordFilters, phraseFilters, x));
        }

        public PhraseData GetLinkedPhraseOf(string startPhraseId)
        {
            var data = dbManager.GetPhraseDataById(startPhraseId);
            return GetLinkedPhraseOf(data);
        }

        public PhraseData GetLinkedPhraseOf(PhraseData data)
        {
            if (data.Linked == "")
            { return null; }
            return dbManager.FindPhraseData(x => x.Id == data.Linked)[0];
        }

        #endregion

        #region Word -> Phrase

        public List<PhraseData> GetPhrasesWithWords(params string[] okWordsArray)
        {
            if (okWordsArray == null)
            { okWordsArray = new string[] { }; }

            var okWords = new HashSet<string>(okWordsArray);

            var phrasesList = dbManager.FindPhraseData(x =>
            {
                if (okWords.Count > 0)
                {
                    bool hasAllOkWords = true;
                    foreach (var okWord in okWords)
                    {
                        bool hasThisWord = false;
                        foreach (var word_id in x.Words)
                        {
                            if (word_id == okWord)
                            {
                                hasThisWord = true;
                                break;
                            }
                        }
                        if (!hasThisWord)
                        {
                            hasAllOkWords = false;
                            break;
                        }
                    }
                    if (!hasAllOkWords)
                    { return false; }
                }
                return true;
            }
            );
            return phrasesList;
        }

        #endregion

    }
}
