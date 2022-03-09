using Antura.LivingLetters;
using Antura.LivingLetters.Sample;
using Antura.Core;
using System.Collections.Generic;

namespace Antura.Minigames.Egg
{
    public class SampleEggSequenceQuestionProvider : IQuestionProvider
    {
        public SampleEggSequenceQuestionProvider()
        {
        }

        public IQuestionPack GetNextQuestion()
        {
            ILivingLetterData questionSentence = null;

            var correctAnswers = new List<ILivingLetterData>();
            var wrongAnswers = new List<ILivingLetterData>();

            while (correctAnswers.Count < 8)
            {
                var letter = AppManager.I.Teacher.GetRandomTestLetterLL();

                if (!CheckIfContains(correctAnswers, letter))
                {
                    correctAnswers.Add(letter);
                }
            }

            return new SampleQuestionPack(questionSentence, wrongAnswers, correctAnswers);
        }

        static bool CheckIfContains(List<ILivingLetterData> list, ILivingLetterData letter)
        {
            for (int i = 0, count = list.Count; i < count; ++i)
            {
                if (list[i].Id == letter.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
