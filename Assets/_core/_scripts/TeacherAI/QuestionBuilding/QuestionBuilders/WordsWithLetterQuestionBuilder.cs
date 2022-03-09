using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Antura.Teacher
{
    /// <summary>
    /// Selects words given a letter
    /// * Question: The letter
    /// * Correct answers: Words with the letter
    /// * Wrong answers: Words without the letter
    /// </summary>
    public class WordsWithLetterQuestionBuilder : IQuestionBuilder
    {
        // focus: Words & Letters
        // pack history filter: enabled
        // journey: enabled

        private int nRounds;
        private int nPacksPerRound;
        private int nCorrect;
        private int nWrong;
        //private bool packsUsedTogether;
        private bool forceUnseparatedLetters;
        private QuestionBuilderParameters parameters;

        public QuestionBuilderParameters Parameters
        {
            get { return this.parameters; }
        }

        public WordsWithLetterQuestionBuilder(
            int nRounds, int nPacksPerRound = 1, int nCorrect = 1, int nWrong = 0,
              QuestionBuilderParameters parameters = null)
        {
            if (parameters == null)
                parameters = new QuestionBuilderParameters();
            this.nRounds = nRounds;
            this.nPacksPerRound = nPacksPerRound;
            //this.packsUsedTogether = nPacksPerRound > 1;
            this.nCorrect = nCorrect;
            this.nWrong = nWrong;
            this.parameters = parameters;
        }

        private List<string> previousPacksIDs_letters = new List<string>();
        private List<string> previousPacksIDs_words = new List<string>();

        private List<LetterData> currentRound_letters = new List<LetterData>();
        private List<WordData> currentRound_words = new List<WordData>();

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            previousPacksIDs_letters.Clear();
            previousPacksIDs_words.Clear();
            List<QuestionPackData> packs = new List<QuestionPackData>();

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

            for (int round_i = 0; round_i < nRounds; round_i++)
            {
                // At each round, we must make sure to not repeat some words / letters
                currentRound_letters.Clear();
                currentRound_words.Clear();

                for (int pack_i = 0; pack_i < nPacksPerRound; pack_i++)
                {
                    packs.Add(CreateSingleQuestionPackData(pack_i, availableLettersWithForms));
                }
            }
            //return null;

            return packs;
        }

        private QuestionPackData CreateSingleQuestionPackData(int inRoundPackIndex, HashSet<LetterData> availableLettersWithForms)
        {
            var teacher = AppManager.I.Teacher;

            /*bool useJourneyForLetters = parameters.useJourneyForCorrect;
            // @note: we also force the journey if the packs must be used together, as the data filters for journey clash with the new filter
            if (packsUsedTogether)
            {
                useJourneyForLetters = false;
            }*/

            int SAFE_COUNT = 0;
            while (true)
            {
                var commonLetter = availableLettersWithForms.ToList().RandomSelectOne();

                var correctLetters = new List<LetterData>();
                correctLetters.Add(commonLetter);
                availableLettersWithForms.Remove(commonLetter);
                currentRound_letters.Add(commonLetter);
                //Debug.Log(availableLettersWithForms.ToDebugStringNewline());

                // Find words with that letter
                // Check if it has enough words
                var selectionParams2 =
                    new SelectionParameters(SelectionSeverity.AllRequired, nCorrect,
                        useJourney: parameters.useJourneyForCorrect,
                        packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs_words);
                selectionParams2.AssignJourney(parameters.insideJourney);
                List<WordData> wordsWithCommonLetter = null;
                try
                {

                    wordsWithCommonLetter = teacher.VocabularyAi.SelectData(
                        () => FindWordsWithCommonLetter(commonLetter), selectionParams2);
                }
                catch (Exception)
                {
                    //Debug.LogWarning(e);
                    SAFE_COUNT++;
                    if (SAFE_COUNT == 100)
                    {
                        throw new Exception("Could not find enough data for WordsWithLetter");
                    }
                    continue;
                }
                var correctWords = wordsWithCommonLetter;
                currentRound_words.AddRange(correctWords);

                // Get words without the letter (only for the first pack of a round)
                var wrongWords = new List<WordData>();
                if (inRoundPackIndex == 0)
                {
                    var selectionParams3 =
                        new SelectionParameters(parameters.wrongSeverity, nWrong,
                            useJourney: parameters.useJourneyForWrong,
                            packListHistory: PackListHistory.NoFilter,
                            journeyFilter: parameters.JourneyFilter);
                    selectionParams3.AssignJourney(parameters.insideJourney);

                    wrongWords = teacher.VocabularyAi.SelectData(
                        () => FindWrongWords(correctWords), selectionParams3);
                    currentRound_words.AddRange(wrongWords);
                }


                /*
                // Get a letter
                var usableLetters = teacher.VocabularyAi.SelectData(
              () => FindLettersThatAppearInWords(atLeastNWords: nCorrect),
                new SelectionParameters(parameters.correctSeverity, 1, useJourney: useJourneyForLetters,
                        packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs_letters));
            var commonLetter = usableLetters[0];
            currentRound_letters.Add(commonLetter);
            */
                /*
                    // Get words with the letter
                    // (but without the previous letters)
                    var correctWords = teacher.VocabularyAi.SelectData(
                        () => FindWordsWithCommonLetter(commonLetter),
                        new SelectionParameters(parameters.correctSeverity, nCorrect,
                            useJourney: parameters.useJourneyForCorrect,
                            packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs_words));
                    currentRound_words.AddRange(correctWords);

                    // Get words without the letter (only for the first pack of a round)
                    var wrongWords = new List<WordData>();
                    if (inRoundPackIndex == 0)
                    {
                        wrongWords = teacher.VocabularyAi.SelectData(
                            () => FindWrongWords(correctWords),
                            new SelectionParameters(parameters.wrongSeverity, nWrong,
                                useJourney: parameters.useJourneyForWrong,
                                packListHistory: PackListHistory.NoFilter,
                                journeyFilter: SelectionParameters.JourneyFilter.CurrentJourney));
                        currentRound_words.AddRange(wrongWords);
                    }
                    */
                var pack = QuestionPackData.Create(commonLetter, correctWords, wrongWords);

                if (ConfigAI.VerboseQuestionPacks)
                {
                    string debugString = "--------- TEACHER: question pack result ---------";
                    debugString += "\nQuestion: " + commonLetter;
                    debugString += "\nCorrect Answers: " + correctWords.Count;
                    foreach (var l in correctWords)
                        debugString += " " + l;
                    debugString += "\nWrong Answers: " + wrongWords.Count;
                    foreach (var l in wrongWords)
                        debugString += " " + l;
                    ConfigAI.AppendToTeacherReport(debugString);
                }

                return pack;
            }
        }


        /*
    private List<LetterData> FindLettersThatAppearInWords(int atLeastNWords)
    {
        var eligibleLetters = new List<LetterData>();
        var vocabularyHelper = AppManager.I.VocabularyHelper;
        var allLetters = vocabularyHelper.GetAllLetters(parameters.letterFilters);
        var badWords = new List<WordData>(currentRound_words);
        foreach (var letter in allLetters)
        {
            // Check number of words
            var wordsWithLetterFull = vocabularyHelper.GetWordsWithLetter(parameters.wordFilters, letter, parameters.letterEqualityStrictness);
            wordsWithLetterFull.RemoveAll(x => badWords.Contains(x));  // Remove the already used words
            wordsWithLetterFull.RemoveAll(x => vocabularyHelper.ProblematicWordIds.Contains(x.Id));  // HACK: Skip the problematic words (for now)

            if (wordsWithLetterFull.Count == 0)
            {
                continue;
            }

            var wordsWithLetter = AppManager.I.Teacher.VocabularyAi.SelectData(
                () => wordsWithLetterFull,
                    new SelectionParameters(SelectionSeverity.AsManyAsPossible, getMaxData: true, useJourney: true), canReturnZero: true);

            int nWords = wordsWithLetter.Count;

            if (nWords < atLeastNWords)
            {
                continue;
            }

            // Avoid using letters that already appeared in the current round's words
            if (packsUsedTogether && vocabularyHelper.AnyWordContainsLetter(letter, currentRound_words))
            {
                continue;
            }

            //UnityEngine.Debug.Log("OK letter: " + letter + " with nwords: "  + wordsWithLetter.Count);

            eligibleLetters.Add(letter);
        }
        return eligibleLetters;
    }*/

        // Caching due to too much lists being created
        private Dictionary<LetterData, List<WordData>> lettersToWordCache;
        private List<WordData> allWordsCache = new List<WordData>();
        private List<WordData> eligibleWordsCache = new List<WordData>();
        private List<WordData> badWordsCache = new List<WordData>();
        private List<LetterData> badLettersCache = new List<LetterData>();

        private List<WordData> FindWordsWithCommonLetter(LetterData commonLetter)
        {
            var vocabularyHelper = AppManager.I.VocabularyHelper;

            // Cache words
            if (lettersToWordCache == null)
            {
                lettersToWordCache =
                    new Dictionary<LetterData, List<WordData>>(new StrictLetterDataComparer(LetterEqualityStrictness.WithVisualForm));

            }
            if (!lettersToWordCache.ContainsKey(commonLetter))
            {
                //Debug.Log("Creating cache for " + commonLetter);
                lettersToWordCache[commonLetter] = vocabularyHelper.GetWordsWithLetter(parameters.wordFilters, commonLetter, parameters.letterEqualityStrictness);
            }

            allWordsCache.Clear();
            allWordsCache.AddRange(lettersToWordCache[commonLetter]);

            badLettersCache.Clear();
            badLettersCache.AddRange(currentRound_letters);
            badLettersCache.Remove(commonLetter);

            badWordsCache.Clear();
            badWordsCache.AddRange(currentRound_words);

            eligibleWordsCache.Clear();
            foreach (var w in allWordsCache)
            {
                // Not words that appeared already this round
                if (badWordsCache.Contains(w))
                {
                    continue;
                }

                // Not words that have one of the previous letters (but the current one)
                if (vocabularyHelper.WordContainsAnyLetter(w, badLettersCache))
                {
                    continue;
                }

                eligibleWordsCache.Add(w);
            }

            //Debug.LogWarning(allWordsCache.ToDebugStringNewline());
            //Debug.LogWarning(eligibleWordsCache.ToDebugStringNewline());
            return eligibleWordsCache;
        }

        private List<WordData> FindWrongWords(List<WordData> correctWords)
        {
            //var eligibleWords = new List<WordData>();
            var vocabularyHelper = AppManager.I.VocabularyHelper;

            allWordsCache.Clear();
            allWordsCache.AddRange(vocabularyHelper.GetWordsNotInOptimized(parameters.wordFilters, correctWords));

            var badLetters = new List<LetterData>(currentRound_letters);

            badLettersCache.Clear();
            badLettersCache.AddRange(currentRound_letters);

            eligibleWordsCache.Clear();

            foreach (var w in allWordsCache)
            {
                // Not words that have one of the previous letters
                if (vocabularyHelper.WordContainsAnyLetter(w, badLetters))
                {
                    continue;
                }

                eligibleWordsCache.Add(w);
            }
            return eligibleWordsCache;
        }

    }
}
