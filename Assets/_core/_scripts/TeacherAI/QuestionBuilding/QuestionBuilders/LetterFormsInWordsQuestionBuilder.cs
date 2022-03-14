using System.Collections.Generic;
using Antura.Database;
using Antura.Helpers;
using Antura.Core;
using UnityEngine;

namespace Antura.Teacher
{
    /// <summary>
    /// Selects letter forms inside words
    /// * Question: Word
    /// * Correct answers: Letter contained in the word with the correct form
    /// * Wrong answers: Letter contained in the word with the wrong form
    /// * Different packs: same Letter will be in all packs, but with different forms
    /// </summary>
    public class LetterFormsInWordsQuestionBuilder : IQuestionBuilder
    {
        // focus: Letters & Words
        // pack history filter: enabled
        // journey: enabled

        private int nPacksPerRound;
        private int nRounds;
        private int maximumWordLength;
        private bool forceUnseparatedLetters;
        private QuestionBuilderParameters parameters;

        public QuestionBuilderParameters Parameters
        {
            get { return this.parameters; }
        }

        public LetterFormsInWordsQuestionBuilder(int nPacksPerRound, int nRounds,
            int maximumWordLength = 20,
            QuestionBuilderParameters parameters = null)
        {
            if (parameters == null)
                parameters = new QuestionBuilderParameters();
            this.nPacksPerRound = nPacksPerRound;
            this.nRounds = nRounds;
            this.maximumWordLength = maximumWordLength;
            this.parameters = parameters;
        }

        private List<string> previousPacksIDs_words = new List<string>();
        private List<string> previousPacksIDs_letters = new List<string>();

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            previousPacksIDs_words.Clear();
            previousPacksIDs_letters.Clear();
            var packs = new List<QuestionPackData>();

            for (int round_i = 0; round_i < nRounds; round_i++)
            {
                // First, choose a letter
                var teacher = AppManager.I.Teacher;
                var usableLetters = teacher.VocabularyAi.SelectData(
                    () => FindEligibleLettersAndForms(minFormsAppearing: 2, maxWordLength: maximumWordLength),
                        new SelectionParameters(parameters.correctSeverity, 1, useJourney: parameters.useJourneyForCorrect,
                            packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs_letters)
                );
                var letter = usableLetters[0];

                // Determine what forms the letter appears in
                List<LetterForm> usableForms = new List<LetterForm>(lettersAndForms[letter]);
                //Debug.Log("N USABLE FORMS: " + usableForms.Count + " for letter " + letter);

                // Packs are reduced to the number of available forms, if needed
                int nPacksFound = Mathf.Min(usableForms.Count, nPacksPerRound);

                // Randomly choose some forms (one per pack) and create packs
                for (int pack_i = 0; pack_i < nPacksFound; pack_i++)
                {
                    var form = RandomHelper.RandomSelectOne(usableForms);
                    usableForms.Remove(form);
                    packs.Add(CreateSingleQuestionPackData(letter, form));
                }
            }

            return packs;
        }

        private QuestionPackData CreateSingleQuestionPackData(LetterData letter, LetterForm correctForm)
        {
            var teacher = AppManager.I.Teacher;

            // Find a word with the letter in that form
            var usableWords = teacher.VocabularyAi.SelectData(
                () => FindEligibleWords(maximumWordLength, letter, correctForm),
                    new SelectionParameters(parameters.correctSeverity, 1, useJourney: parameters.useJourneyForCorrect,
                        packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs_words)
            );
            var question = usableWords[0];

            // Place the correct letter and form
            var correctAnswers = new List<LetterData>();
            var letterWithForm = AppManager.I.VocabularyHelper.ConvertToLetterWithForcedForm(letter, correctForm);
            correctAnswers.Add(letterWithForm);

            // Place the other forms as wrong forms
            var wrongAnswers = new List<LetterData>();
            foreach (var wrongForm in letter.GetAvailableForms())
            {
                if (wrongForm == correctForm)
                    continue;
                letterWithForm = AppManager.I.VocabularyHelper.ConvertToLetterWithForcedForm(letter, wrongForm);
                wrongAnswers.Add(letterWithForm);
            }

            if (ConfigAI.VerboseQuestionPacks)
            {
                string debugString = "--------- TEACHER: question pack result ---------";
                debugString += "\nQuestion: " + question;
                debugString += "\nCorrect Answers: " + correctAnswers.Count;
                foreach (var l in correctAnswers)
                { debugString += " " + l; }
                ConfigAI.AppendToTeacherReport(debugString);
            }

            return QuestionPackData.Create(question, correctAnswers, wrongAnswers);
        }

        List<LetterData> lettersWithManyForms = new List<LetterData>();
        Dictionary<LetterData, List<LetterForm>> lettersAndForms = new Dictionary<LetterData, List<LetterForm>>();

        List<LetterData> FindEligibleLettersAndForms(int minFormsAppearing, int maxWordLength)
        {
            var vocabularyHelper = AppManager.I.VocabularyHelper;
            var eligibleLetters = new List<LetterData>();

            if (lettersAndForms.Count == 0)
            {
                var allWords = AppManager.I.Teacher.VocabularyAi.SelectData(
                    () => vocabularyHelper.GetAllWords(parameters.wordFilters),
                        new SelectionParameters(parameters.correctSeverity, getMaxData: true, useJourney: parameters.useJourneyForCorrect,
                            packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs_words));

                // The chosen letter should actually have words that contain it in different forms.
                // This can be quite slow, so we do this only at the start.
                var allLetters = vocabularyHelper.GetAllLetters(parameters.letterFilters);
                foreach (var letterData in allLetters)
                {
                    int nFormsAppearing = 0;
                    var availableForms = new List<LetterForm>();
                    foreach (var form in letterData.GetAvailableForms())
                    {
                        foreach (var wordData in allWords)
                        {
                            if (WordIsEligible(wordData, letterData, form, maxWordLength))
                            {
                                nFormsAppearing++;
                                availableForms.Add(form);
                                break;
                            }
                        }
                    }
                    if (nFormsAppearing >= minFormsAppearing)
                    {
                        lettersWithManyForms.Add(letterData);
                        lettersAndForms[letterData] = availableForms;
                        //Debug.Log("Letter " + letterData + " is cool as it appears " + nFormsAppearing);
                    }
                }
            }

            eligibleLetters = lettersWithManyForms;

            return eligibleLetters;
        }

        Dictionary<KeyValuePair<LetterData, LetterForm>, List<WordData>> eligibleWordsForLetters = new Dictionary<KeyValuePair<LetterData, LetterForm>, List<WordData>>();

        public List<WordData> FindEligibleWords(int maxWordLength, LetterData containedLetter, LetterForm form)
        {
            var vocabularyHelper = AppManager.I.VocabularyHelper;
            var eligibleWords = new List<WordData>();

            var pair = new KeyValuePair<LetterData, LetterForm>(containedLetter, form);
            if (!eligibleWordsForLetters.ContainsKey(pair))
            {
                foreach (var word in vocabularyHelper.GetAllWords(parameters.wordFilters))
                {
                    if (!WordIsEligible(word, containedLetter, form, maxWordLength))
                        continue;
                    eligibleWords.Add(word);
                    //Debug.Log("Letter: " + containedLetter + " form: " + form + " Word: " + word);
                }
                eligibleWordsForLetters[pair] = eligibleWords;
            }
            eligibleWords = eligibleWordsForLetters[pair];

            //Debug.LogWarning("Eligible words: " + eligibleWords.Count + " out of " + vocabularyHelper.GetWordsByCategory(category, parameters.wordFilters).Count);
            return eligibleWords;
        }

        private bool WordIsEligible(WordData word, LetterData containedLetter, LetterForm form, int maxWordLength)
        {
            // Check max length
            if (AppManager.I.VocabularyHelper.GetLettersInWord(word).Count > maxWordLength)
            {
                return false;
            }

            // Check that it contains the letter only once
            if (AppManager.I.VocabularyHelper.WordContainsLetterTimes(word, containedLetter) > 1)
            {
                return false;
            }

            // Check that it contains a letter in the correct form
            var letterWithForm = AppManager.I.VocabularyHelper.ConvertToLetterWithForcedForm(containedLetter, form);
            if (!AppManager.I.VocabularyHelper.WordContainsLetter(word, letterWithForm, LetterEqualityStrictness.WithActualForm))
            {
                return false;
            }

            return true;
        }

    }
}
