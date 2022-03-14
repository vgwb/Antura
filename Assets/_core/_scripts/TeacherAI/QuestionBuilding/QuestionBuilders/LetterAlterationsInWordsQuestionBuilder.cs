using System.Collections.Generic;
using System.Linq;
using Antura.Database;
using Antura.Helpers;
using Antura.Core;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Antura.Teacher
{
    /// <summary>
    /// Selects letters inside words, then shows wrong alterations
    /// * Question: Word
    /// * Correct answers: Letter contained in the word with the correct alteration
    /// * Wrong answers: Letter contained in the word with the wrong alteration
    /// * Different packs: same Letter will be in all packs, but with different alterations
    /// </summary>
    public class LetterAlterationsInWordsQuestionBuilder : IQuestionBuilder
    {
        // focus: Letters & Words
        // pack history filter: enabled
        // journey: enabled

        private int nPacksPerRound;
        private int nRounds;
        private int nWrongs;
        private int maximumWordLength;
        private bool keepBasesOnly;
        private QuestionBuilderParameters parameters;
        private LetterAlterationFilters letterAlterationFilters;

        public QuestionBuilderParameters Parameters => this.parameters;

        public LetterAlterationsInWordsQuestionBuilder(int nPacksPerRound, int nRounds,
            int nWrongs = 4,
            int maximumWordLength = 20,
            bool keepBasesOnly = true,
            LetterAlterationFilters letterAlterationFilters = null,
            QuestionBuilderParameters parameters = null)
        {
            if (parameters == null)
                parameters = new QuestionBuilderParameters();
            this.nPacksPerRound = nPacksPerRound;
            this.nRounds = nRounds;
            this.nWrongs = nWrongs;
            this.maximumWordLength = maximumWordLength;
            this.parameters = parameters;
            this.letterAlterationFilters = letterAlterationFilters;
            this.keepBasesOnly = keepBasesOnly;

            // Forced parameters
            this.parameters.letterFilters.excludeDiacritics = LetterFilters.ExcludeDiacritics.All;
            this.parameters.letterFilters.excludeLetterVariations = LetterFilters.ExcludeLetterVariations.All;
        }

        private List<string> previousPacksIDs_words = new List<string>();
        private List<string> previousPacksIDs_letters = new List<string>();

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            previousPacksIDs_words.Clear();
            previousPacksIDs_letters.Clear();
            var packs = new List<QuestionPackData>();

            for (int round_i = 0; round_i < nRounds * nPacksPerRound; round_i++)
            {
                packs.Add(CreateSingleQuestionPackData());
            }

            return packs;
        }

        private QuestionPackData CreateSingleQuestionPackData()
        {
            var teacher = AppManager.I.Teacher;
            var vocabularyHelper = AppManager.I.VocabularyHelper;

            //Debug.Log(FindLettersThatAppearInWords(maxWordLength: maximumWordLength).ToDebugStringNewline());

            // First, choose a letter (base only, due to filters)
            var selectionParams1 = new SelectionParameters(parameters.correctSeverity, 1, useJourney: parameters.useJourneyForCorrect,
                packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs_letters);
            selectionParams1.AssignJourney(parameters.insideJourney);

            var eligibleLetters = teacher.VocabularyAi.SelectData(() => FindLettersThatAppearInWords(maxWordLength: maximumWordLength), selectionParams1);
            //Debug.Log(eligibleLetters.ToDebugStringNewline());

            // Choose one letter randomly from the eligible ones
            var chosenLetter = eligibleLetters.RandomSelectOne();
            //Debug.Log("Chosen: " + chosenLetter);

            // Find a word with the letter (strict)
            var selectionParams2 = new SelectionParameters(parameters.correctSeverity, 1, useJourney: parameters.useJourneyForCorrect,
                packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs_words);
            selectionParams2.AssignJourney(parameters.insideJourney);
            var usableWords = teacher.VocabularyAi.SelectData(
                () => FindWordsWithLetterStrict(chosenLetter, maximumWordLength), selectionParams2);

            var question = usableWords[0];
            // Get the correct form inside the word
            //Debug.Log("Word is: " + question.ToString());
            //Debug.Log("Letters: " + vocabularyHelper.GetLettersInWord(question).ToDebugString());
            //Debug.Log("Letters correct: " + vocabularyHelper.GetLettersInWord(question, removeAccents).Where(l => l.IsSameLetterAs(chosenLetter, LetterEqualityStrictness.LetterBase)).ToDebugString());
            var chosenLetterWithForm = vocabularyHelper.GetLettersInWord(question, keepBasesOnly).Where(l => l.IsSameLetterAs(chosenLetter, LetterEqualityStrictness.LetterBase)).ToList().RandomSelectOne();
            //chosenLetterWithForm = vocabularyHelper.ExtractLettersWithForms(chosenLetterWithForm);
            //Debug.Log("Correct form: " + chosenLetterWithForm + " form is " + chosenLetterWithForm.Form);

            // Place the correct alteration in the correct list
            var correctAnswers = new List<LetterData>();
            correctAnswers.Add(chosenLetterWithForm);

            // Place some alterations in the wrong list
            List<LetterData> baseLetters;
            if (letterAlterationFilters.differentBaseLetters)
                baseLetters = AppManager.I.VocabularyHelper.GetAllLetters(parameters.letterFilters);
            else
                baseLetters = eligibleLetters;

            // Filter out unknown letters
            var filteredBaseLetters = teacher.VocabularyAi.SelectData(
                () => baseLetters,
                    new SelectionParameters(parameters.wrongSeverity, getMaxData: true, useJourney: true,
                        packListHistory: PackListHistory.NoFilter)
            );
            //Debug.LogWarning("Filtered bases: " + filteredBaseLetters.ToDebugStringNewline());

            var alterationsPool = AppManager.I.VocabularyHelper.GetAllLetterAlterations(filteredBaseLetters, letterAlterationFilters);
            var wrongAnswers = new List<LetterData>();
            //Debug.Log("N Alterations before remove correct: " + alterationsPool.Count + " " + alterationsPool.ToDebugString());

            // Remove the correct alteration (making sure to get the actual form)
            for (int i = 0; i < alterationsPool.Count; i++)
            {
                if (alterationsPool[i].IsSameLetterAs(chosenLetterWithForm, LetterEqualityStrictness.WithVisualForm))
                {
                    alterationsPool.RemoveAt(i);
                }
            }

            //Debug.Log("N Alterations after remove correct: " + alterationsPool.Count + " " + alterationsPool.ToDebugString());
            wrongAnswers.AddRange(alterationsPool.RandomSelect(nWrongs));

            if (ConfigAI.VerboseQuestionPacks)
            {
                string debugString = "--------- TEACHER: question pack result ---------";
                debugString += "\nQuestion: " + question;
                debugString += "\nCorrect Answers: " + correctAnswers.Count;
                foreach (var l in correctAnswers)
                { debugString += " " + l; }
                debugString += "\nWrong Answers: " + wrongAnswers.Count;
                foreach (var l in wrongAnswers)
                    debugString += " " + l;
                ConfigAI.AppendToTeacherReport(debugString);
            }

            return QuestionPackData.Create(question, correctAnswers, wrongAnswers);
        }

        List<LetterData> lettersThatAppearInWords = new List<LetterData>();

        List<LetterData> FindLettersThatAppearInWords(int maxWordLength)
        {
            int minTimesAppearing = 1;   // at least once

            var vocabularyHelper = AppManager.I.VocabularyHelper;
            var eligibleLetters = new List<LetterData>();


            if (lettersThatAppearInWords.Count == 0)
            {
                var allWords = AppManager.I.Teacher.VocabularyAi.SelectData(
                    () => vocabularyHelper.GetAllWords(parameters.wordFilters),
                        new SelectionParameters(parameters.correctSeverity, getMaxData: true, useJourney: parameters.useJourneyForCorrect,
                            packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs_words));
                //Debug.Log("All words: " + allWords.Count);

                // The chosen letter should actually have words that contain it multiple times.
                // This can be quite slow, so we do this only at the start.
                // @note: we are interested at finding the letter in any form
                var baseLetters = vocabularyHelper.GetAllLetters(parameters.letterFilters);
                //var allLettersWithForms = vocabularyHelper.GetAllLetterAlterations(baseLetters, LetterAlterationFilters.FormsOfMultipleLetters);
                foreach (var letterData in baseLetters)
                {
                    int nTimesAppearing = 0;
                    foreach (var wordData in allWords)
                    {
                        if (WordContainsLetter(wordData, letterData, maxWordLength, LetterEqualityStrictness.LetterBase))
                        {
                            nTimesAppearing++;
                            break;
                        }
                    }
                    if (nTimesAppearing >= minTimesAppearing)
                    {
                        lettersThatAppearInWords.Add(letterData);
                        //Debug.Log("Letter " + letterData + " is cool as it appears " + nTimesAppearing);
                    }
                }
                //Debug.Log("Eligible letters: " + lettersThatAppearInWords.Count);
            }

            eligibleLetters = lettersThatAppearInWords;

            return eligibleLetters;
        }

        Dictionary<LetterData, List<WordData>> eligibleWordsForLetters = new Dictionary<LetterData, List<WordData>>(new StrictLetterDataComparer(LetterEqualityStrictness.WithActualForm));

        private List<WordData> FindWordsWithLetterStrict(LetterData containedLetter, int maxWordLength)
        {
            var vocabularyHelper = AppManager.I.VocabularyHelper;
            var eligibleWords = new List<WordData>();

            if (!eligibleWordsForLetters.ContainsKey(containedLetter))
            {
                foreach (var word in vocabularyHelper.GetAllWords(parameters.wordFilters))
                {
                    if (!WordContainsLetter(word, containedLetter, maxWordLength, LetterEqualityStrictness.LetterBase))
                        continue;
                    eligibleWords.Add(word);
                    //Debug.Log("Letter: " + containedLetter + " in Word: " + word);
                }
                eligibleWordsForLetters[containedLetter] = eligibleWords;
            }
            eligibleWords = eligibleWordsForLetters[containedLetter];

            //Debug.LogWarning("Eligible words: " + eligibleWords.Count + " out of " + vocabularyHelper.GetAllWords(parameters.wordFilters).Count);
            return eligibleWords;
        }

        private bool WordContainsLetter(WordData word, LetterData letter, int maxWordLength, LetterEqualityStrictness strictness)
        {
            // Check max length
            if (AppManager.I.VocabularyHelper.GetLettersInWord(word).Count > maxWordLength)
            {
                return false;
            }

            // Check that it contains the letter at least once
            if (AppManager.I.VocabularyHelper.WordContainsLetterTimes(word, letter, strictness) >= 1)
            {
                //Debug.Log("Letter " + letter + " is in word " + word + " " + AppManager.I.VocabularyHelper.WordContainsLetterTimes(word, letter, LetterEqualityStrictness.WithActualForm) + " times");
                return true;
            }

            return false;
        }

    }
}
