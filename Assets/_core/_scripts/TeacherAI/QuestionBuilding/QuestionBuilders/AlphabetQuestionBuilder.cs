using System.Collections.Generic;
using Antura.Core;

namespace Antura.Teacher
{
    /// <summary>
    /// Selects letters from the alphabet.
    /// Correct answers: all alphabet letters.
    /// </summary>
    public class AlphabetQuestionBuilder : IQuestionBuilder
    {
        // focus: Letters
        // pack history filter: forced - only 1 pack
        // journey: enabled

        private QuestionBuilderParameters parameters;

        public QuestionBuilderParameters Parameters
        {
            get { return this.parameters; }
        }

        public AlphabetQuestionBuilder(QuestionBuilderParameters parameters = null)
        {
            if (parameters == null)
            {
                parameters = new QuestionBuilderParameters();
            }

            this.parameters = parameters;

            // Forced filters
            this.parameters.letterFilters.excludeLetterVariations = LetterFilters.ExcludeLetterVariations.All;
            this.parameters.letterFilters.excludeDiacritics = LetterFilters.ExcludeDiacritics.All;
            this.parameters.letterFilters.excludeDiphthongs = true;
        }

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            var packs = new List<QuestionPackData>();
            packs.Add(CreateAlphabetQuestionPackData());
            return packs;
        }

        public QuestionPackData CreateAlphabetQuestionPackData()
        {
            var teacher = AppManager.I.Teacher;
            var vocabularyHelper = AppManager.I.VocabularyHelper;

            ConfigAI.AppendToTeacherReport("New Question Pack");

            // Fully ordered alphabet, only 1 pack
            var alphabetLetters = teacher.VocabularyAi.SelectData(
                () => vocabularyHelper.GetAllLetters(parameters.letterFilters),
                new SelectionParameters(parameters.correctSeverity, getMaxData: true, useJourney: parameters.useJourneyForCorrect)
                );

            alphabetLetters.Sort((x, y) =>
                {
                    return x.Number - y.Number;
                }
            );

            if (ConfigAI.VerboseQuestionPacks)
            {
                string debugString = "Letters: " + alphabetLetters.Count;
                foreach (var l in alphabetLetters)
                {
                    debugString += " " + l;
                }
                ConfigAI.AppendToTeacherReport(debugString);
            }

            return QuestionPackData.CreateFromCorrect(null, alphabetLetters);
        }

    }
}
