using Antura.Core;
using Antura.Database;
using System.Collections.Generic;

namespace Antura.Teacher
{
    /// <summary>
    /// Selects letters inside a word
    /// * Question: Word
    /// * Correct answers: Letters contained in the word
    /// * Wrong answers: Letters not contained in the word
    /// </summary>
    public class LettersInWordQuestionBuilder : IQuestionBuilder
    {
        // focus: Letters & Words
        // pack history filter: enabled
        // journey: enabled

        private int nRounds;
        private int nPacksPerRound;
        private int nCorrect;
        private bool useAllCorrectLetters;
        private int nWrong;
        private int maximumWordLength;
        private bool packsUsedTogether;
        private WordDataCategory category;
        private bool forceUnseparatedLetters;
        private QuestionBuilderParameters parameters;

        public QuestionBuilderParameters Parameters
        {
            get { return this.parameters; }
        }

        public LettersInWordQuestionBuilder(
            int nRounds, int nPacksPerRound = 1, int nCorrect = 1, int nWrong = 0,
            bool useAllCorrectLetters = false, WordDataCategory category = WordDataCategory.None,
            int maximumWordLength = 20,
            QuestionBuilderParameters parameters = null)
        {
            if (parameters == null) {
                parameters = new QuestionBuilderParameters();
            }
            this.nRounds = nRounds;
            this.nPacksPerRound = nPacksPerRound;
            this.packsUsedTogether = nPacksPerRound > 1;
            this.nCorrect = nCorrect;
            this.nWrong = nWrong;
            this.useAllCorrectLetters = useAllCorrectLetters;
            this.category = category;
            this.maximumWordLength = maximumWordLength;
            this.parameters = parameters;
        }

        private List<string> previousPacksIDs_words = new List<string>();
        private List<string> previousPacksIDs_letters = new List<string>();

        private List<LetterData> currentRound_letters = new List<LetterData>();
        private List<WordData> currentRound_words = new List<WordData>();

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            previousPacksIDs_words.Clear();
            previousPacksIDs_letters.Clear();
            var packs = new List<QuestionPackData>();
            for (int round_i = 0; round_i < nRounds; round_i++) {
                // At each round, we must make sure to not repeat some words / letters
                currentRound_letters.Clear();
                currentRound_words.Clear();

                for (int pack_i = 0; pack_i < nPacksPerRound; pack_i++) {
                    packs.Add(CreateSingleQuestionPackData(pack_i));
                }
            }
            return packs;
        }

        private QuestionPackData CreateSingleQuestionPackData(int inRoundPackIndex)
        {
            var teacher = AppManager.I.Teacher;
            var vocabularyHelper = AppManager.I.VocabularyHelper;

            // Choose a single eligible word
            var usableWords = teacher.VocabularyAi.SelectData(
                () => FindEligibleWords(maxWordLength: maximumWordLength),
                    new SelectionParameters(parameters.correctSeverity, 1, useJourney: parameters.useJourneyForCorrect,
                        packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs_words)
            );
            var wordQuestion = usableWords[0];
            currentRound_words.Add(wordQuestion);
            //UnityEngine.Debug.LogWarning("Chosen word: " + question);

            // Get letters of that word
            var wordLetters = vocabularyHelper.GetLettersInWord(wordQuestion);
            //UnityEngine.Debug.LogWarning("Found letters: " + wordLetters.ToArray().ToDebugString());

            bool useJourneyForLetters = parameters.useJourneyForCorrect;
            // @note: we force journey in this case to be off so that all letters can be found
            // @note: we also force the journey if the packs must be used together, as the data filters for journey clash with the new filter
            if (useAllCorrectLetters || packsUsedTogether) { useJourneyForLetters = false; }

            // Get some letters (from that word)
            var correctLetters = teacher.VocabularyAi.SelectData(
                () => FindCorrectLetters(wordQuestion, wordLetters),
                 new SelectionParameters(parameters.correctSeverity, nCorrect, getMaxData: useAllCorrectLetters,
                    useJourney: useJourneyForLetters, filteringIds: previousPacksIDs_letters));
            currentRound_letters.AddRange(correctLetters);

            // Get some wrong letters (not from that word, nor other words, nor previous letters)
            // Only for the first pack of the round
            var wrongLetters = new List<LetterData>();
            if (inRoundPackIndex == 0) {
                wrongLetters = teacher.VocabularyAi.SelectData(
                () => FindWrongLetters(wordQuestion, wordLetters),
                    new SelectionParameters(
                        parameters.wrongSeverity, nWrong, useJourney: parameters.useJourneyForWrong,
                        journeyFilter: SelectionParameters.JourneyFilter.CurrentJourney));
                currentRound_letters.AddRange(wrongLetters);
            }

            if (ConfigAI.VerboseQuestionPacks) {
                string debugString = "--------- TEACHER: question pack result ---------";
                debugString += "\nQuestion: " + wordQuestion;
                debugString += "\nCorrect Answers: " + correctLetters.Count;
                foreach (var l in correctLetters) debugString += " " + l;
                debugString += "\nWrong Answers: " + wrongLetters.Count;
                foreach (var l in wrongLetters) debugString += " " + l;
                ConfigAI.AppendToTeacherReport(debugString);
            }

            return QuestionPackData.Create(wordQuestion, correctLetters, wrongLetters);
        }

        public List<WordData> FindEligibleWords(int maxWordLength)
        {
            var vocabularyHelper = AppManager.I.VocabularyHelper;
            var eligibleWords = new List<WordData>();
            foreach (var word in vocabularyHelper.GetWordsByCategory(category, parameters.wordFilters)) {
                // Check max length
                if (word.Letters.Length > maxWordLength) {
                    continue;
                }

                // Avoid using words that contain previously chosen letters
                if (vocabularyHelper.WordContainsAnyLetter(word, currentRound_letters)) {
                    continue;
                }

                // Avoid using words that have ONLY letters that appeared in previous words
                if (vocabularyHelper.WordHasAllLettersInCommonWith(word, currentRound_words)) {
                    continue;
                }

                eligibleWords.Add(word);
            }
            //UnityEngine.Debug.Log("Eligible words: " + eligibleWords.Count + " out of " + teacher.VocabularyHelper.GetWordsByCategory(category, parameters.wordFilters).Count);
            return eligibleWords;
        }

        public List<LetterData> FindCorrectLetters(WordData selectedWord, List<LetterData> wordLetters)
        {
            var vocabularyHelper = AppManager.I.VocabularyHelper;
            var eligibleLetters = new List<LetterData>();
            var badWords = new List<WordData>(currentRound_words);
            badWords.Remove(selectedWord);
            foreach (var letter in wordLetters) {
                // Avoid using letters that appeared in previous words
                if (vocabularyHelper.IsLetterContainedInAnyWord(letter, badWords)) {
                    continue;
                }

                eligibleLetters.Add(letter);
            }
            return eligibleLetters;
        }

        public List<LetterData> FindWrongLetters(WordData selectedWord, List<LetterData> wordLetters)
        {
            var vocabularyHelper = AppManager.I.VocabularyHelper;
            var noWordLetters = vocabularyHelper.GetLettersNotIn(LetterEqualityStrictness.LetterOnly, parameters.letterFilters, wordLetters.ToArray());
            var eligibleLetters = new List<LetterData>();
            var badWords = new List<WordData>(currentRound_words);
            badWords.Remove(selectedWord);
            foreach (var letter in noWordLetters) {
                // Avoid using letters that appeared in previous words
                if (vocabularyHelper.IsLetterContainedInAnyWord(letter, badWords)) {
                    continue;
                }

                eligibleLetters.Add(letter);
            }
            return eligibleLetters;
        }

    }
}