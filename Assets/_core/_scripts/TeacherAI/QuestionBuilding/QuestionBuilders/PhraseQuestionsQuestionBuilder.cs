using System.Collections.Generic;
using Antura.Core;

namespace Antura.Teacher
{
    /// <summary>
    /// Selects phrases based on a question/answer relationship
    /// * Question: The question phrase
    /// * Correct answers: The correct answer
    /// * Wrong answers: Wrong answers
    /// </summary>
    public class PhraseQuestionsQuestionBuilder : IQuestionBuilder
    {
        // Focus: Phrases
        // pack history filter: enabled
        // journey: enabled

        private int nPacks;
        private int nWrong;
        private QuestionBuilderParameters parameters;

        public QuestionBuilderParameters Parameters
        {
            get { return this.parameters; }
        }

        public PhraseQuestionsQuestionBuilder(int nPacks, int nWrong = 0, QuestionBuilderParameters parameters = null)
        {
            if (parameters == null)
            {
                parameters = new QuestionBuilderParameters();
            }

            this.nPacks = nPacks;
            this.nWrong = nWrong;
            this.parameters = parameters;
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

            // Get a question phrase at random
            int nToUse = 1;
            var selectionParams1 = new SelectionParameters(parameters.correctSeverity, nToUse, useJourney: parameters.useJourneyForCorrect,
                packListHistory: parameters.correctChoicesHistory, filteringIds: previousPacksIDs);
            selectionParams1.AssignJourney(parameters.insideJourney);
            var usablePhrases = teacher.VocabularyAi.SelectData(
                () => vocabularyHelper.GetPhrasesByCategory(Database.PhraseDataCategory.Question, parameters.wordFilters, parameters.phraseFilters), selectionParams1);
            var question = usablePhrases[0];

            // Get the linked reply phrase
            var reply = vocabularyHelper.GetLinkedPhraseOf(question);

            var correctAnswers = new List<Database.PhraseData>();
            correctAnswers.Add(reply);

            // Get random wrong phrases
            var selectionParams2 = new SelectionParameters(parameters.correctSeverity, nWrong,
                useJourney: parameters.useJourneyForWrong,
                packListHistory: PackListHistory.NoFilter,
                journeyFilter: SelectionParameters.JourneyFilter.CurrentJourney);
            selectionParams2.AssignJourney(parameters.insideJourney);

            var wrongPhrases = teacher.VocabularyAi.SelectData(
                () => vocabularyHelper.GetPhrasesNotIn(parameters.wordFilters, parameters.phraseFilters, question, reply), selectionParams2);

            if (ConfigAI.VerboseQuestionPacks)
            {
                string debugString = "--------- TEACHER: question pack result ---------";
                debugString += "\nQuestion: " + question;
                debugString += "\nCorrect Answers: " + correctAnswers.Count;
                foreach (var l in correctAnswers)
                    debugString += " " + l;
                debugString += "\nWrong Answers: " + wrongPhrases.Count;
                foreach (var l in wrongPhrases)
                    debugString += " " + l;
                ConfigAI.AppendToTeacherReport(debugString);
            }

            return QuestionPackData.Create(question, correctAnswers, wrongPhrases);
        }


    }
}
