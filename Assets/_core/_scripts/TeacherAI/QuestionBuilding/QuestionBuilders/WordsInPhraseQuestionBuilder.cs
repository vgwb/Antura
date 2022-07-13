using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using System.Collections.Generic;
using System.Diagnostics;

namespace Antura.Teacher
{
    /// <summary>
    /// Selects words inside / related to a phrase
    /// * Question: The phrase
    /// * Correct answers: The words related to the phrase
    /// * Wrong answers: Unrelated words
    /// </summary>
    public class WordsInPhraseQuestionBuilder : IQuestionBuilder
    {
        // Focus: Words & Phrases
        // pack history filter: enabled
        // journey: enabled

        private int nPacks;
        private int nCorrect;
        private int nWrong;
        private bool useAllCorrectWords;
        private bool usePhraseAnswersIfFound;
        private QuestionBuilderParameters parameters;

        public QuestionBuilderParameters Parameters => this.parameters;

        public WordsInPhraseQuestionBuilder(int nPacks, int nCorrect = 1, int nWrong = 0,
            bool useAllCorrectWords = false, bool usePhraseAnswersIfFound = false,
            QuestionBuilderParameters parameters = null)
        {
            if (parameters == null)
            {
                parameters = new QuestionBuilderParameters();
            }

            this.nPacks = nPacks;
            this.nCorrect = nCorrect;
            this.nWrong = nWrong;
            this.useAllCorrectWords = useAllCorrectWords;
            this.usePhraseAnswersIfFound = usePhraseAnswersIfFound;
            this.parameters = parameters;

            parameters.phraseFilters.requireAnswers = usePhraseAnswersIfFound;
        }

        private List<string> previousPacksIDs = new List<string>();

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            previousPacksIDs.Clear();
            var packs = new List<QuestionPackData>();
            for (int pack_i = 0; pack_i < nPacks; pack_i++)
            {
                packs.Add(CreateSingleQuestionPackData());
            }
            return packs;
        }

        private QuestionPackData CreateSingleQuestionPackData()
        {
            var teacher = AppManager.I.Teacher;
            var vocabularyHelper = AppManager.I.VocabularyHelper;
            parameters.wordFilters.allowedCategories = null; // Reset forced category

            // Get a phrase
            int nToUse = 1;
            var selectionParams1 =
                new SelectionParameters(parameters.correctSeverity, nToUse, useJourney: parameters.useJourneyForCorrect,
                    packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs);
            selectionParams1.AssignJourney(parameters.insideJourney);
            var usablePhrases = teacher.VocabularyAi.SelectData(
                () => vocabularyHelper.GetAllPhrases(
                    parameters.wordFilters,
                    parameters.phraseFilters), selectionParams1);
            var question = usablePhrases.RandomSelectOne();

            // Get words related to the phrase
            var correctWords = new List<WordData>();
            List<WordData> relatedWords = new List<WordData>();

            var phraseWords = vocabularyHelper.GetWordsInPhrase(question, parameters.wordFilters);
            relatedWords.AddRange(phraseWords);

            if (usePhraseAnswersIfFound && question.Answers.Length > 0)
            {
                var answerWords = vocabularyHelper.GetAnswersToPhrase(question, parameters.wordFilters);
                correctWords.AddRange(answerWords);
                relatedWords.AddRange(answerWords);
            }
            else
            {
                correctWords.AddRange(phraseWords);
            }

            // Choose the word/s we want to use
            if (!useAllCorrectWords)
            {
                correctWords = correctWords.RandomSelect(nCorrect);
            }

            // Get wrong words from the same category of the correct one
            parameters.wordFilters.allowedCategories = new[] { correctWords[0].Category };
            var selectionParams2 =
                new SelectionParameters(parameters.correctSeverity, nWrong, useJourney: parameters.useJourneyForCorrect,
                    packListHistory: PackListHistory.NoFilter,
                    journeyFilter: parameters.JourneyFilter);
            var wrongWords = teacher.VocabularyAi.SelectData(
                  () => vocabularyHelper.GetWordsNotIn(parameters.wordFilters, relatedWords.ToArray()),
                  selectionParams2
            );

            if (ConfigAI.VerboseQuestionPacks)
            {
                string debugString = "--------- TEACHER: question pack result ---------";
                debugString += "\nQuestion: " + question;
                debugString += "\nCorrect Answers: " + correctWords.Count;
                foreach (var l in correctWords)
                    debugString += " " + l;
                debugString += "\nWrong Answers: " + wrongWords.Count;
                foreach (var l in wrongWords)
                    debugString += " " + l;
                ConfigAI.AppendToTeacherReport(debugString);
            }

            return QuestionPackData.Create(question, correctWords, wrongWords);
        }


    }
}
