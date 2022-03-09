using System.Collections.Generic;

namespace Antura.LivingLetters
{
    /// <summary>
    /// Default IQuestionProvider that returns packs in a sequential order.
    /// </summary>
    /// <seealso cref="IQuestionProvider" />
    // refactor: this is used in all minigames as the core application reasons only in terms of question packs
    public class SequentialQuestionPackProvider : IQuestionProvider
    {
        #region properties

        private List<IQuestionPack> questions = new List<IQuestionPack>();
        private int currentQuestion;

        #endregion

        public SequentialQuestionPackProvider(List<IQuestionPack> _questionsPack)
        {
            currentQuestion = -1;

            questions.AddRange(_questionsPack);
        }

        /// <summary>
        /// Provide me another question.
        /// </summary>
        /// <returns></returns>
        IQuestionPack IQuestionProvider.GetNextQuestion()
        {
            currentQuestion++;

            if (currentQuestion >= questions.Count)
            {
                currentQuestion = 0;
            }

            return questions[currentQuestion];
        }

        public IQuestionPack PeekFirstQuestion()
        {
            return questions[0];
        }

        public IEnumerable<IQuestionPack> EnumerateAllPacks()
        {
            foreach (var questionPack in questions)
            {
                yield return questionPack;
            }
        }
    }
}
