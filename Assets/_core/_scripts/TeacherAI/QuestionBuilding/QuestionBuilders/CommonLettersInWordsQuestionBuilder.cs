using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Antura.Teacher
{
    /// <summary>
    /// Selects words that have one letter in common.
    /// * Question: Words with letter in common
    /// * Correct answers: letter in common
    /// * Wrong answers: letters not in common
    /// @note: this now uses Strictness to define whether the common letters must have the same form or not
    /// @note: this has been rewritten following WordsWithLetter and simplified
    /// </summary>
    public class CommonLetterInWordQuestionBuilder : IQuestionBuilder
    {
        // focus: Letters & Words
        // pack history filter: DISABLED - the special logic needed makes it really hard to use a pack history filter here
        // journey: enabled

        private int nPacks;
        private int nWrong;
        private int nWords;
        private LetterEqualityStrictness letterEqualityStrictness;
        private QuestionBuilderParameters parameters;

        public QuestionBuilderParameters Parameters
        {
            get { return this.parameters; }
        }

        public CommonLetterInWordQuestionBuilder(int nPacks,
            int nWrong = 0, int nWords = 1,
            LetterEqualityStrictness letterEqualityStrictness = LetterEqualityStrictness.Letter,
            QuestionBuilderParameters parameters = null)
        {
            if (parameters == null)
                parameters = new QuestionBuilderParameters();
            this.nPacks = nPacks;
            this.nWrong = nWrong;
            this.nWords = nWords;
            this.letterEqualityStrictness = letterEqualityStrictness;
            this.parameters = parameters;
        }

        private List<string> previousPacksIDs_letters = new List<string>();
        private List<string> previousPacksIDs_words = new List<string>();

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            previousPacksIDs_letters.Clear();
            previousPacksIDs_words.Clear();
            var packs = new List<QuestionPackData>();

            // @note: all packs are created together in this builder, as a speed-up
            // Choose all letters we want to focus on
            var teacher = AppManager.I.Teacher;
            var vocabularyHelper = AppManager.I.VocabularyHelper;
            var selectionParams1 = new SelectionParameters(parameters.correctSeverity, getMaxData: true,
                useJourney: parameters.useJourneyForCorrect,
                packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs_letters);
            selectionParams1.AssignJourney(parameters.insideJourney);
            var availableLetters = teacher.VocabularyAi.SelectData(
              () => vocabularyHelper.GetAllLetters(parameters.letterFilters), selectionParams1);

            // Keep a list of letters we tried
            var availableLettersWithForms = new HashSet<LetterData>(new StrictLetterDataComparer(LetterEqualityStrictness.WithActualForm));

            // Add forms too (cannot be found in the SelectData call)
            availableLettersWithForms.UnionWith(vocabularyHelper.ExtractLettersWithForms(availableLetters));

            for (int pack_i = 0; pack_i < nPacks; pack_i++)
            {
                packs.Add(CreateSingleQuestionPackData(availableLettersWithForms));
            }
            return packs;
        }

        private QuestionPackData CreateSingleQuestionPackData(HashSet<LetterData> availableLettersWithForms)
        {
            QuestionPackData pack = null;
            var teacher = AppManager.I.Teacher;

            int SAFE_COUNT = 0;
            while (true)
            {
                var commonLetter = availableLettersWithForms.ToList().RandomSelectOne();

                var correctLetters = new List<LetterData>();
                correctLetters.Add(commonLetter);
                availableLettersWithForms.Remove(commonLetter);
                //Debug.Log("Test " + SAFE_COUNT + ": Trying letter " + commonLetter);

                // Check if it has enough words
                var selectionParams2 = new SelectionParameters(SelectionSeverity.AllRequired, nWords,
                    useJourney: parameters.useJourneyForCorrect,
                    packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs_words);
                List<WordData> wordsWithCommonLetter = null;
                try
                {
                    wordsWithCommonLetter = teacher.VocabularyAi.SelectData(
                        () => FindWordsWithCommonLetter(commonLetter), selectionParams2);

                }
                catch (Exception)
                {
                    SAFE_COUNT++;
                    if (SAFE_COUNT == 100)
                    {
                        throw new Exception("Could not find enough data for CommonLettersInWord");
                    }
                    continue;
                }
                //Debug.Log("Found letter " + commonLetter + " at trial count: " + SAFE_COUNT);

                var lettersNotInCommon = teacher.VocabularyAi.SelectData(
                () => FindLettersNotInCommon(wordsWithCommonLetter),
                    new SelectionParameters(parameters.wrongSeverity, nWrong, useJourney: parameters.useJourneyForWrong,
                        journeyFilter: SelectionParameters.JourneyFilter.CurrentJourney,
                        getMaxData: true // needed to skip priority filtering, which will filter out forms!
                        ));
                lettersNotInCommon = lettersNotInCommon.RandomSelect(nWrong);  // needed to skip priority filtering, which will filter out forms!

                pack = QuestionPackData.Create(wordsWithCommonLetter, correctLetters, lettersNotInCommon);

                if (ConfigAI.VerboseQuestionPacks)
                {
                    string debugString = "--------- TEACHER: question pack result ---------";
                    debugString += "\nQuestion: " + wordsWithCommonLetter.ToDebugString();
                    debugString += "\nCorrect Answers: " + correctLetters.Count;
                    foreach (var l in correctLetters)
                        debugString += " " + l;
                    debugString += "\nWrong Answers: " + lettersNotInCommon.Count;
                    foreach (var l in lettersNotInCommon)
                        debugString += " " + l;
                    ConfigAI.AppendToTeacherReport(debugString);
                }
                return pack;
            }
        }


        private Dictionary<LetterData, List<WordData>> lettersToWordCache;

        // DEPRECATED: this generated up to 2 GB of allocations, we use a trial-based test instead
        private List<LetterData> FindLettersThatAppearInWords(int atLeastNWords)
        {
            var eligibleLetters = new List<LetterData>();
            var vocabularyHelper = AppManager.I.VocabularyHelper;

            if (lettersToWordCache == null)
            {
                lettersToWordCache = new Dictionary<LetterData, List<WordData>>(new StrictLetterDataComparer(letterEqualityStrictness));

                var allLetters = vocabularyHelper.GetAllLettersAndForms(parameters.letterFilters);     // we consider different forms as different letters
                foreach (var letter in allLetters)
                {
                    lettersToWordCache[letter] = new List<WordData>();
                    // Check number of words
                    var wordsWithLetterForm = vocabularyHelper.GetWordsWithLetter(parameters.wordFilters, letter, letterEqualityStrictness);
                    //Debug.Log("N words for letter " + letter + " is " + wordsWithLetterFull.Count);
                    //wordsWithLetterForm.RemoveAll(x => vocabularyHelper.ProblematicWordIds.Contains(x.Id));  // HACK: Skip the problematic words (for now)

                    lettersToWordCache[letter].AddRange(wordsWithLetterForm);
                }
                Debug.Log(lettersToWordCache.Count);
            }

            foreach (var letter in lettersToWordCache.Keys)
            {
                var words = lettersToWordCache[letter];

                if (words.Count == 0)
                    continue;

                // Check number of words
                var wordsWithLetter = AppManager.I.Teacher.VocabularyAi.SelectData(
                    () => words,
                        new SelectionParameters(SelectionSeverity.AsManyAsPossible, getMaxData: true, useJourney: true), canReturnZero: true);

                if (wordsWithLetter.Count < atLeastNWords)
                {
                    continue;
                }

                //UnityEngine.Debug.Log("OK letter: " + letter + " with nwords: "  + wordsWithLetter.Count);
                eligibleLetters.Add(letter);
            }

            return eligibleLetters;
        }

        private List<WordData> FindWordsWithCommonLetter(LetterData commonLetter)
        {
            var eligibleWords = new List<WordData>();
            var vocabularyHelper = AppManager.I.VocabularyHelper;
            var words = vocabularyHelper.GetWordsWithLetter(parameters.wordFilters, commonLetter, letterEqualityStrictness);
            foreach (var w in words)
            {
                eligibleWords.Add(w);
            }
            //Debug.Log(eligibleWords.ToDebugStringNewline());
            return eligibleWords;
        }

        private List<LetterData> FindLettersNotInCommon(List<WordData> words)
        {
            var vocabularyHelper = AppManager.I.VocabularyHelper;
            var lettersNotInCommon = vocabularyHelper.GetNotCommonLettersInWords(parameters.letterFilters, letterEqualityStrictness, words.ToArray());
            return lettersNotInCommon;
        }

    }
}
