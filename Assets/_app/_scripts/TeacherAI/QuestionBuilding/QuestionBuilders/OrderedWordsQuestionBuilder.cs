using System.Collections.Generic;
using Antura.Core;

namespace Antura.Teacher
{
    /// <summary>
    /// Selects words in a given order
    /// * Correct answers: Ordered words
    /// </summary>
    public class OrderedWordsQuestionBuilder : IQuestionBuilder
    {
        // focus: Words
        // pack history filter: only 1 pack
        // journey: enabled

        private Database.WordDataCategory category;
        private QuestionBuilderParameters parameters;
        private bool skipWordZero;

        public QuestionBuilderParameters Parameters
        {
            get { return this.parameters; }
        }

        public OrderedWordsQuestionBuilder(Database.WordDataCategory category, QuestionBuilderParameters parameters = null, bool skipWordZero = false)
        {
            if (parameters == null)
            {
                parameters = new QuestionBuilderParameters();
            }

            this.category = category;
            this.parameters = parameters;
            this.skipWordZero = skipWordZero;
        }

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            var packs = new List<QuestionPackData>();
            packs.Add(CreateSingleQuestionPackData());
            return packs;
        }

        private QuestionPackData CreateSingleQuestionPackData()
        {
            var teacher = AppManager.I.Teacher;
            var vocabularyHelper = AppManager.I.VocabularyHelper;

            // Ordered words
            var words = teacher.VocabularyAi.SelectData(
                 () => vocabularyHelper.GetWordsByCategory(category, parameters.wordFilters),
                 new SelectionParameters(parameters.correctSeverity, getMaxData: true, useJourney: parameters.useJourneyForCorrect)
               );

            // sort by id
            words.Sort((x, y) =>
                {
                    return x.Id.CompareTo(y.Id);
                }
            );
            if (skipWordZero)
            {
                words.RemoveAt(0);
            }

            if (ConfigAI.VerboseQuestionPacks)
            {
                string debugString = "Words: " + words.Count;
                foreach (var w in words)
                {
                    debugString += " " + w;
                }
                ConfigAI.AppendToTeacherReport(debugString);
            }

            return QuestionPackData.CreateFromCorrect(null, words);
        }

    }
}