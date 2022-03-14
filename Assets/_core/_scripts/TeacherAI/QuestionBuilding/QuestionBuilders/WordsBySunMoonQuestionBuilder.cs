using System.Collections.Generic;
using Antura.Core;

namespace Antura.Teacher
{
    /// <summary>
    /// Categorize words based on their sun/moon status
    /// * Question: Word to categorize
    /// * Correct answers: Correct category
    /// * Wrong answers: Wrong category
    /// </summary>
    public class WordsBySunMoonQuestionBuilder : IQuestionBuilder
    {
        // focus: Words
        // pack history filter: by default, all different
        // journey: enabled

        private int nPacks;
        private QuestionBuilderParameters parameters;

        public QuestionBuilderParameters Parameters
        {
            get { return this.parameters; }
        }

        public WordsBySunMoonQuestionBuilder(int nPacks, QuestionBuilderParameters parameters = null)
        {
            if (parameters == null)
            {
                parameters = new QuestionBuilderParameters();
            }

            this.nPacks = nPacks;
            this.parameters = parameters;

            // Forced parameters
            this.parameters.wordFilters.excludeArticles = false;
        }

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            var packs = new List<QuestionPackData>();
            var teacher = AppManager.I.Teacher;
            var vocabularyHelper = AppManager.I.VocabularyHelper;

            var db = AppManager.I.DB;
            var choice1 = db.GetWordDataById("the_sun");
            var choice2 = db.GetWordDataById("the_moon");

            var wordsWithArticle = teacher.VocabularyAi.SelectData(
                () => vocabularyHelper.GetWordsByArticle(Database.WordDataArticle.Determinative, parameters.wordFilters),
                new SelectionParameters(parameters.correctSeverity, nPacks, useJourney: parameters.useJourneyForCorrect)
                );

            foreach (var wordWithArticle in wordsWithArticle)
            {
                int articleLength = 2;
                var letterAfterArticle = vocabularyHelper.GetLettersInWord(wordWithArticle)[articleLength];
                var correctWords = new List<Database.WordData>();
                var wrongWords = new List<Database.WordData>();

                switch (letterAfterArticle.SunMoon)
                {
                    case Database.LetterDataSunMoon.Sun:
                        correctWords.Add(choice1);
                        wrongWords.Add(choice2);
                        break;

                    case Database.LetterDataSunMoon.Moon:
                        correctWords.Add(choice2);
                        wrongWords.Add(choice1);
                        break;
                }

                // Debug
                if (ConfigAI.VerboseQuestionPacks)
                {
                    string debugString = "--------- TEACHER: question pack result ---------";
                    debugString += "\nQuestion: " + wordWithArticle;
                    debugString += "\nCorrect Word: " + correctWords.Count;
                    foreach (var l in correctWords)
                        debugString += " " + l;
                    debugString += "\nWrong Word: " + wrongWords.Count;
                    foreach (var l in wrongWords)
                        debugString += " " + l;
                    ConfigAI.AppendToTeacherReport(debugString);
                }

                var pack = QuestionPackData.Create(wordWithArticle, correctWords, wrongWords);
                packs.Add(pack);
            }

            return packs;
        }

    }
}
