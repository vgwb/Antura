using System;
using System.Collections.Generic;

namespace Antura.Assessment
{
    /// <summary>
    /// This class collect some answers, those become "a answer set". The Answer
    /// set is linked to a question to let indentify correct answers easily.
    /// </summary>
    public class AnswerSet
    {
        Answer[] correctAnswers;
        private static int totalCorrectAnswers = 0;

        public static void ResetTotalCount()
        {
            totalCorrectAnswers = 0;
        }

        public static int GetCorrectCount()
        {
            return totalCorrectAnswers;
        }

        public AnswerSet(Answer[] answers)
        {
            //Should filter only correct answers
            int count = 0;
            foreach (var answ in answers)
            {
                if (answ.IsCorrect())
                {
                    count++;
                }
            }

            totalCorrectAnswers += count;

            correctAnswers = new Answer[count];
            int index = 0;
            foreach (var answ in answers)
            {
                if (answ.IsCorrect())
                {
                    correctAnswers[index++] = answ;
                }
            }
        }

        List<Answer> currentAnswers = new List<Answer>();

        public void OnDroppedAnswer(Answer answer)
        {
            currentAnswers.Add(answer);
        }

        public void OnRemovedAnswer(Answer answer)
        {
            if (currentAnswers.Remove(answer) == false)
            {
                throw new InvalidOperationException("Cannot remove something that was not added");
            }
        }

        public bool AllCorrect()
        {
            foreach (var correct in correctAnswers)
            {
                bool found = false;

                foreach (var ci in currentAnswers)
                {
                    if (correct.Equals(ci))
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }
            return true;
        }

        internal bool IsCorrect(Answer answ)
        {
            foreach (var c in correctAnswers)
            {
                if (c == answ)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
