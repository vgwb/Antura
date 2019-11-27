using System.Linq;
using Antura.LivingLetters;

namespace Antura.Minigames.DancingDots
{
    public class DancingDotsQuestionsManager
    {
        private IQuestionProvider provider;

        public DancingDotsQuestionsManager()
        {
            provider = DancingDotsConfiguration.Instance.Questions;
        }

        public ILivingLetterData getNewLetter()
        {
            var question = provider.GetNextQuestion();
            return question.GetCorrectAnswers().First();
        }
    }
}
