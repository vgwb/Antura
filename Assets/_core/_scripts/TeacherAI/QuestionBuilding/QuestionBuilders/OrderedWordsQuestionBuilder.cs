using System;
using System.Collections.Generic;
using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using UnityEngine;
using static System.String;

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

        private QuestionBuilderParameters parameters;
        private bool skipWordZero;
        private WordDataCategory[] originalCategories;
        private int maxAnswers;

        public QuestionBuilderParameters Parameters => parameters;

        public OrderedWordsQuestionBuilder(QuestionBuilderParameters parameters = null, bool skipWordZero = false, int maxAnswers = 0)
        {
            if (parameters == null)
            {
                parameters = new QuestionBuilderParameters();
            }

            this.parameters = parameters;
            this.skipWordZero = skipWordZero;
            this.originalCategories = parameters.wordFilters.allowedCategories;
            this.maxAnswers = maxAnswers;
        }

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            var packs = new List<QuestionPackData>();

            foreach (var originalCategory in originalCategories)
            {
                packs.Add(CreateSingleQuestionPackData(originalCategory));
            }

            return packs;
        }

        private QuestionPackData CreateSingleQuestionPackData(WordDataCategory dataCategory)
        {
            var teacher = AppManager.I.Teacher;
            var vocabularyHelper = AppManager.I.VocabularyHelper;

            // Ordered words
            var selectionParams1 = new SelectionParameters(parameters.correctSeverity, getMaxData: true,
                useJourney: parameters.useJourneyForCorrect);
            selectionParams1.AssignJourney(parameters.insideJourney);
            parameters.wordFilters.allowedCategories = new[] { dataCategory };
            var words = teacher.VocabularyAi.SelectData(
                 () => vocabularyHelper.GetAllWords(parameters.wordFilters),
                 selectionParams1
               );

            // sort by id
            words.Sort((x, y) => int.Parse(x.SortValue) - int.Parse(y.SortValue));
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

            if (maxAnswers > 0)
                words = words.GetRange(0, Mathf.Min(words.Count, maxAnswers));

            return QuestionPackData.CreateFromCorrect(null, words);
        }

    }
}
