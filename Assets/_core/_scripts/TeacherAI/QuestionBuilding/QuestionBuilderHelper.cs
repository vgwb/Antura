using System.Collections.Generic;
using Antura.Database;

namespace Antura.Teacher
{
    /// <summary>
    /// Defines rules on how question packs can be generated for a specific mini game.
    /// </summary>
    public static class QuestionBuilderHelper
    {
        public static void SortPacksByDifficulty(List<QuestionPackData> packs)
        {
            packs.Sort((x, y) => (int)(getAverageIntrinsicDifficulty(x) * 100f - getAverageIntrinsicDifficulty(y) * 100f));
        }

        private static float getAverageIntrinsicDifficulty(QuestionPackData pack)
        {
            float qDiff = 0;
            float cDiff = 0;

            float qWeight = 0.5f;
            float cWeight = 0.5f;

            if (pack.questions != null && pack.questions.Count > 0)
            {
                foreach (var q in pack.questions)
                    qDiff += ((IVocabularyData)q).GetIntrinsicDifficulty();
                qDiff /= pack.questions.Count;
            }

            if (pack.question != null)
            {
                qDiff += ((IVocabularyData)pack.question).GetIntrinsicDifficulty();
            }
            else
            {
                qWeight = 0;
            }

            if (pack.correctAnswers.Count > 0)
            {
                foreach (var c in pack.correctAnswers)
                {
                    cDiff += ((IVocabularyData)c).GetIntrinsicDifficulty();
                }
                cDiff /= pack.correctAnswers.Count;
            }
            else
            {
                cWeight = 0;
            }

            float diff = (qWeight * qDiff + cWeight * cDiff) / (qWeight + cWeight);
            //UnityEngine.Debug.Log("Pack " + pack.question + " diff: " + diff);
            return diff;
        }
    }
}
