using System.Collections.Generic;
using Antura.Core;
using Antura.Helpers;

namespace Antura.Teacher
{
    /// <summary>
    /// Categorize words based on their article (with/without)
    /// * Question: Word to categorize
    /// * Correct answers: Correct article
    /// * Wrong answers: Wrong article
    /// </summary>
    public class WordsByArticleQuestionBuilder : IQuestionBuilder
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

        public WordsByArticleQuestionBuilder(int nPacks, QuestionBuilderParameters parameters = null)
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
            var choice1 = db.GetWordDataById("with_article");
            var choice2 = db.GetWordDataById("without_article");

            int nPerType = nPacks / 2;

            var list_choice1 = teacher.VocabularyAi.SelectData(
                () => vocabularyHelper.GetWordsByArticle(Database.WordDataArticle.Determinative, parameters.wordFilters),
                new SelectionParameters(parameters.correctSeverity, nPerType, useJourney: parameters.useJourneyForCorrect)
                );

            var list_choice2 = teacher.VocabularyAi.SelectData(
                () => vocabularyHelper.GetWordsByArticle(Database.WordDataArticle.None, parameters.wordFilters),
                new SelectionParameters(parameters.wrongSeverity, nPerType, useJourney: parameters.useJourneyForCorrect)
                );

            foreach (var wordWithArticle in list_choice1)
            {
                var correctWords = new List<Database.WordData>();
                var wrongWords = new List<Database.WordData>();
                correctWords.Add(choice1);
                wrongWords.Add(choice2);

                var pack = QuestionPackData.Create(wordWithArticle, correctWords, wrongWords);
                packs.Add(pack);
            }

            foreach (var wordWithoutArticle in list_choice2)
            {
                var correctWords = new List<Database.WordData>();
                var wrongWords = new List<Database.WordData>();
                correctWords.Add(choice2);
                wrongWords.Add(choice1);

                var pack = QuestionPackData.Create(wordWithoutArticle, correctWords, wrongWords);
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
                    foreach (var l in pack.correctAnswers)
                        debugString += " " + l;
                    debugString += "\nWrong Word: " + pack.wrongAnswers.Count;
                    foreach (var l in pack.wrongAnswers)
                        debugString += " " + l;
                    ConfigAI.AppendToTeacherReport(debugString);
                }
            }

            return packs;
        }

    }
}
