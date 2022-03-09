using System.Collections.Generic;
using Antura.LivingLetters;
using Antura.Core;

namespace Antura.Teacher
{
    /// <summary>
    /// Given a minigame, handles the generation of question packs.
    /// This is also used to convert data-only question packs to LivingLetter-related question packs.
    /// </summary>
    public class QuestionPacksGenerator
    {
        public List<IQuestionPack> GenerateQuestionPacks(IQuestionBuilder questionBuilder, int safetyCounter = 10)
        {

            // Safety fallback, used for release to avoid crashing the application.
            // @note: This WILL block the game if an error happens EVERYTIME, so make sure that never happens!
            List<QuestionPackData> questionPackDataList = null;
            while (true)
            {
                try
                {
                    // Generate packs
                    questionPackDataList = questionBuilder.CreateAllQuestionPacks();
                    break;
                }
                catch (System.Exception e)
                {
                    if (!ConfigAI.TeacherSafetyFallbackEnabled)
                    {
                        throw e;
                    }
                    else
                    {
                        safetyCounter--;
                        UnityEngine.Debug.LogError("Teacher fallback triggered (" + safetyCounter + "): " + e.ToString());
                        ConfigAI.PrintTeacherReport(logOnly: true);

                        if (safetyCounter <= 0)
                        {
                            break;
                        }
                    }
                }
            }

            // Apply ordering
            if (questionBuilder.Parameters != null && questionBuilder.Parameters.sortPacksByDifficulty)
            {
                QuestionBuilderHelper.SortPacksByDifficulty(questionPackDataList);
            }

            // Fix blatant repetitions
            if (questionPackDataList.Count > 2)
            {
                FixRepetitions(questionPackDataList);
            }

            ConfigAI.ReportPacks(questionPackDataList);

            var questionPackList = ConvertQuestionPackDataToQuestionPack(questionPackDataList);
            return questionPackList;
        }

        private void FixRepetitions(List<QuestionPackData> packs)
        {
            //ConfigAI.ReportPacks(packs);

            // Remove repeated packs
            var repeatedPacks = new List<QuestionPackData>();
            for (int i = packs.Count - 2; i >= 0; i--)
            {
                if (IsSamePack(packs[i], packs[i + 1]))
                {
                    repeatedPacks.Add(packs[i]);
                    packs.RemoveAt(i);
                }
            }

            //UnityEngine.Debug.LogError("FOUND " + repeatedPacks.Count + " REP");

            // Reinsert them
            repeatedPacks.Reverse();
            for (int ri = repeatedPacks.Count - 1; ri >= 0; ri--)
            {
                bool inserted = false;

                if (!IsSamePack(repeatedPacks[ri], packs[packs.Count - 1]))
                {
                    packs.Add(repeatedPacks[ri]);
                    inserted = true;
                    continue;
                }

                for (int i = packs.Count - 2; i >= 0; i--)
                {
                    if (!IsSamePack(repeatedPacks[ri], packs[i + 1]) && !IsSamePack(repeatedPacks[ri], packs[i]))
                    {
                        //UnityEngine.Debug.LogError("Reinserting " + repeatedPacks[ri] + " at " + (i + 1)   + "\n between " + packs[i] + " and " + packs[i+1]);
                        packs.Insert(i + 1, repeatedPacks[ri]);
                        inserted = true;
                        break;
                    }
                }

                if (!inserted)
                {
                    packs.Insert(0, repeatedPacks[ri]);
                }
            }
        }

        private bool IsSamePack(QuestionPackData pack1, QuestionPackData pack2)
        {
            bool isSame = (pack1.question == null || pack1.question == pack2.question)
                          && (pack1.questions == null || (pack1.questions[0] == pack2.questions[0]))
                          && (pack1.correctAnswers == null || (pack1.correctAnswers[0] == pack2.correctAnswers[0]));
            return isSame;
        }

        private List<IQuestionPack> ConvertQuestionPackDataToQuestionPack(List<QuestionPackData> questionPackDataList)
        {
            var questionPackList = new List<IQuestionPack>();
            foreach (var questionPackData in questionPackDataList)
            {
                ILivingLetterData ll_question =
                    questionPackData.question != null ? questionPackData.question.ConvertToLivingLetterData() : null;
                List<ILivingLetterData> ll_questions = questionPackData.questions != null
                    ? questionPackData.questions.ConvertAll(x => x.ConvertToLivingLetterData())
                    : null;
                List<ILivingLetterData> ll_wrongAnswers = questionPackData.wrongAnswers != null
                    ? questionPackData.wrongAnswers.ConvertAll(x => x.ConvertToLivingLetterData())
                    : null;
                List<ILivingLetterData> ll_correctAnswers = questionPackData.correctAnswers != null
                    ? questionPackData.correctAnswers.ConvertAll(x => x.ConvertToLivingLetterData())
                    : null;
                IQuestionPack pack;

                // Conversion based on what kind of question we have
                // @todo: at least on the teacher's side, this could be simplified by always using a LIST of questions
                if (ll_questions != null && ll_questions.Count > 0)
                {
                    pack = new FindRightDataQuestionPack(ll_questions, ll_wrongAnswers, ll_correctAnswers);
                }
                else
                {
                    pack = new FindRightDataQuestionPack(ll_question, ll_wrongAnswers, ll_correctAnswers);
                }
                questionPackList.Add(pack);
            }
            return questionPackList;
        }
    }
}
