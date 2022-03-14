using Antura.LivingLetters;
using Antura.LivingLetters.Sample;
using Antura.Core;
using System.Collections.Generic;

namespace Antura.Minigames.Egg
{
    public class SampleEggSingleQuestionProvider : IQuestionProvider
    {
        public SampleEggSingleQuestionProvider()
        {
        }

        public IQuestionPack GetNextQuestion()
        {
            ILivingLetterData questionSentence = null;

            var correctAnswers = new List<ILivingLetterData>();
            var wrongAnswers = new List<ILivingLetterData>();

            correctAnswers.Add(AppManager.I.Teacher.GetRandomTestLetterLL());

            while (wrongAnswers.Count < 8)
            {
                var letter = AppManager.I.Teacher.GetRandomTestLetterLL();

                if (!CheckIfContains(correctAnswers, letter) && !CheckIfContains(wrongAnswers, letter))
                {
                    wrongAnswers.Add(letter);
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
