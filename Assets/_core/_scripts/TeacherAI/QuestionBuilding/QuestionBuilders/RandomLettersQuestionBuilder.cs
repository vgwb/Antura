using System.Collections.Generic;
using Antura.Core;
using Antura.Database;

namespace Antura.Teacher
{
    /// <summary>
    /// Selects letters at random
    /// * Question: The letter to find
    /// * Correct answers: The correct letter
    /// * Wrong answers: Wrong letters
    /// </summary>
    public class RandomLettersQuestionBuilder : IQuestionBuilder
    {
        // focus: Letters
        // pack history filter: parameterized
        // journey: enabled

        private int nPacks;
        private int nCorrect;
        private int nWrong;
        private bool firstCorrectIsQuestion;
        private QuestionBuilderParameters parameters;

        public QuestionBuilderParameters Parameters => parameters;

        public RandomLettersQuestionBuilder(int nPacks, int nCorrect = 1, int nWrong = 0, bool firstCorrectIsQuestion = false, QuestionBuilderParameters parameters = null)
        {
            if (parameters == null)
            {
                parameters = new QuestionBuilderParameters();
            }

            this.nPacks = nPacks;
            this.nCorrect = nCorrect;
            this.nWrong = nWrong;
            this.firstCorrectIsQuestion = firstCorrectIsQuestion;
            this.parameters = parameters;

            // Forced filters
            this.parameters.letterFilters.excludeDiphthongs = true;
        }

        private List<string> previousPacksIDs = new List<string>();

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            previousPacksIDs.Clear();

            var packs = new List<QuestionPackData>();
            for (int pack_i = 0; pack_i < nPacks; pack_i++)
            {
                var pack = CreateSingleQuestionPackData();
                packs.Add(pack);
            }
            return packs;
        }

        private QuestionPackData CreateSingleQuestionPackData()
        {
            var teacher = AppManager.I.Teacher;
            var vocabularyHelper = AppManager.I.VocabularyHelper;

            var selectionParams1 = new SelectionParameters(parameters.correctSeverity, nCorrect,
                useJourney: parameters.useJourneyForCorrect,
                packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs);
            selectionParams1.AssignJourney(parameters.insideJourney);
            var correctLetters = teacher.VocabularyAi.SelectData(
                () => vocabularyHelper.GetAllLetters(parameters.letterFilters), selectionParams1);

            var selectionParams2 = new SelectionParameters(parameters.wrongSeverity, nWrong,
                useJourney: parameters.useJourneyForWrong,
                packListHistory: PackListHistory.NoFilter);
            selectionParams2.AssignJourney(parameters.insideJourney);
            var wrongLetters = teacher.VocabularyAi.SelectData(
                () => vocabularyHelper.GetLettersNotIn(LetterEqualityStrictness.Letter, parameters.letterFilters, correctLetters.ToArray()),
                selectionParams2
                );

            var question = firstCorrectIsQuestion ? correctLetters[0] : null;

            if (ConfigAI.VerboseQuestionPacks)
            {
                string debugString = "--------- TEACHER: question pack result ---------";
                debugString += "\nCorrect Letters: " + correctLetters.Count;
                foreach (var l in correctLetters)
                    debugString += " " + l;
                debugString += "\nWrong Letters: " + wrongLetters.Count;
                foreach (var l in wrongLetters)
                    debugString += " " + l;
                ConfigAI.AppendToTeacherReport(debugString);
            }

            return QuestionPackData.Create(question, correctLetters, wrongLetters);
        }

    }
}
