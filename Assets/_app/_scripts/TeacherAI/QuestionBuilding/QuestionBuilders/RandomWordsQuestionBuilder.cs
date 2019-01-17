using System.Collections.Generic;
using Antura.Core;
using Antura.Database;

namespace Antura.Teacher
{
    /// <summary>
    /// Selects words at random
    /// * Question: The word to find
    /// * Correct answers: The correct word
    /// * Wrong answers: Wrong words
    /// </summary>
    public class RandomWordsQuestionBuilder : IQuestionBuilder
    {
        // Focus: Words
        // pack history filter: parameterized
        // journey: enabled

        private int nPacks;
        private int nCorrect;
        private int nWrong;
        private bool firstCorrectIsQuestion;
        private Database.WordDataCategory category;
        private QuestionBuilderParameters parameters;

        public QuestionBuilderParameters Parameters
        {
            get { return this.parameters; }
        }
        public RandomWordsQuestionBuilder(int nPacks, int nCorrect = 1, int nWrong = 0,
            bool firstCorrectIsQuestion = false, Database.WordDataCategory category = Database.WordDataCategory.None, QuestionBuilderParameters parameters = null)
        {
            if (parameters == null)
            {
                parameters = new QuestionBuilderParameters();
            }

            this.nPacks = nPacks;
            this.nCorrect = nCorrect;
            this.nWrong = nWrong;
            this.firstCorrectIsQuestion = firstCorrectIsQuestion;
            this.category = category;
            this.parameters = parameters;
        }

        private List<string> previousPacksIDs = new List<string>();

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            previousPacksIDs.Clear();

            var vocabularyHelper = AppManager.I.VocabularyHelper;
            words_cache.Clear();
            words_cache.AddRange(vocabularyHelper.GetWordsByCategory(category, parameters.wordFilters));

            var packs = new List<QuestionPackData>();
            for (int pack_i = 0; pack_i < nPacks; pack_i++)
            {
                var pack = CreateSingleQuestionPackData();
                packs.Add(pack);
            }

            return packs;
        }

        private List<WordData> words_cache = new List<WordData>();
        private List<WordData> wrongWords_cache = new List<WordData>();

        private QuestionPackData CreateSingleQuestionPackData()
        {
            var teacher = AppManager.I.Teacher;
            var vocabularyHelper = AppManager.I.VocabularyHelper;

            var correctWords = teacher.VocabularyAi.SelectData(
                () => words_cache,
                   new SelectionParameters(parameters.correctSeverity, nCorrect, useJourney: parameters.useJourneyForCorrect,
                      packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs)
                );

            
            wrongWords_cache.Clear();
            wrongWords_cache.AddRange(vocabularyHelper.GetWordsNotInOptimized(parameters.wordFilters, correctWords));
            var wrongWords = teacher.VocabularyAi.SelectData(
                () => wrongWords_cache,
                    new SelectionParameters(parameters.wrongSeverity, nWrong, useJourney: parameters.useJourneyForWrong,
                        packListHistory: PackListHistory.NoFilter,
                        journeyFilter: SelectionParameters.JourneyFilter.CurrentJourney)
                );

            var question = firstCorrectIsQuestion ? correctWords[0] : null;

            if (ConfigAI.VerboseQuestionPacks)
            {
                string debugString = "--------- TEACHER: question pack result ---------";
                debugString += "\nCorrect Words: " + correctWords.Count;
                foreach (var l in correctWords) debugString += " " + l;
                debugString += "\nWrong Words: " + wrongWords.Count;
                foreach (var l in wrongWords) debugString += " " + l;
                ConfigAI.AppendToTeacherReport(debugString);
            }

            return QuestionPackData.Create(question, correctWords, wrongWords);
        }

    }
}