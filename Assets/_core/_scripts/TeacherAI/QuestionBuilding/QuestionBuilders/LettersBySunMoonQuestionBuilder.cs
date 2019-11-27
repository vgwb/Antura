using System.Collections.Generic;
using Antura.Core;
using Antura.Helpers;

namespace Antura.Teacher
{
    /// <summary>
    /// Categorize letters and shows words that are either Sun or Moon.
    /// * Question: Letter to categorize
    /// * Correct answers: Sun or Moon
    /// * Wrong answers: Sun or Moon
    /// </summary>
    // TODO refactor: this question builder is tied to the arabic language and should thus be separated from the others
    public class LettersBySunMoonQuestionBuilder : IQuestionBuilder
    {
        // focus: Letters
        // pack history filter: by default, all different
        // journey: enabled

        private int nPacks;
        private QuestionBuilderParameters parameters;

        public QuestionBuilderParameters Parameters
        {
            get { return this.parameters; }
        }

        public LettersBySunMoonQuestionBuilder(int nPacks, QuestionBuilderParameters parameters = null)
        {
            if (parameters == null) { parameters = new QuestionBuilderParameters(); }

            this.nPacks = nPacks;
            this.parameters = parameters;

            // Forced filters
            this.parameters.letterFilters.excludeDiphthongs = true;
        }

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            var packs = new List<QuestionPackData>();
            var teacher = AppManager.I.Teacher;
            var vocabularyHelper = AppManager.I.VocabularyHelper;

            var db = AppManager.I.DB;
            var choice1 = db.GetWordDataById("the_sun");
            var choice2 = db.GetWordDataById("the_moon");

            int nPerType = nPacks / 2;

            var list_choice1 = teacher.VocabularyAi.SelectData(
                () => vocabularyHelper.GetLettersBySunMoon(Database.LetterDataSunMoon.Sun, parameters.letterFilters),
                new SelectionParameters(parameters.correctSeverity, nPerType, useJourney: parameters.useJourneyForCorrect)
                );

            var list_choice2 = teacher.VocabularyAi.SelectData(
                () => vocabularyHelper.GetLettersBySunMoon(Database.LetterDataSunMoon.Moon, parameters.letterFilters),
                new SelectionParameters(parameters.correctSeverity, nPerType, useJourney: parameters.useJourneyForCorrect)
                );

            foreach (var data in list_choice1)
            {
                var correctWords = new List<Database.WordData>();
                var wrongWords = new List<Database.WordData>();
                correctWords.Add(choice1);
                wrongWords.Add(choice2);

                var pack = QuestionPackData.Create(data, correctWords, wrongWords);
                packs.Add(pack);
            }

            foreach (var data in list_choice2)
            {
                var correctWords = new List<Database.WordData>();
                var wrongWords = new List<Database.WordData>();
                correctWords.Add(choice2);
                wrongWords.Add(choice1);

                var pack = QuestionPackData.Create(data, correctWords, wrongWords);
                packs.Add(pack);
            }


            // Shuffle the packs at the end
            packs.Shuffle();

            if (ConfigAI.VerboseQuestionPacks)
            {
                foreach (var pack in packs)
                {
                    string debugString = "--------- TEACHER: question pack result ---------";
                    debugString += "\nQuestion: " + pack.question;
                    debugString += "\nCorrect Word: " + pack.correctAnswers.Count;
                    foreach (var l in pack.correctAnswers) { debugString += " " + l; }
                    debugString += "\nWrong Word: " + pack.wrongAnswers.Count;
                    foreach (var l in pack.wrongAnswers) { debugString += " " + l; }
                    ConfigAI.AppendToTeacherReport(debugString);
                }
            }

            return packs;
        }

    }
}