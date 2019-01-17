using System.Collections.Generic;
using Antura.LivingLetters;
using Antura.LivingLetters.Sample;
using Antura.Core;

namespace Antura.Minigames.ThrowBalls
{
    public class ThrowBallsQuestionProvider : IQuestionProvider
    {
        List<SampleQuestionPack> questions = new List<SampleQuestionPack>();

        int currentQuestion;

        public ThrowBallsQuestionProvider()
        {
            currentQuestion = -1;

            // 10 QuestionPacks
            for (int i = 0; i < 10; i++)
            {
                List<ILivingLetterData> correctAnswers = new List<ILivingLetterData>();
                List<ILivingLetterData> wrongAnswers = new List<ILivingLetterData>();

                LL_LetterData newLetterData = AppManager.I.Teacher.GetRandomTestLetterLL();

                if (newLetterData == null)
                    return;

                correctAnswers.Add(newLetterData);

                // At least 4 wrong letters
                while (wrongAnswers.Count < 4)
                {
                    var letter = AppManager.I.Teacher.GetRandomTestLetterLL();

                    if (!CheckIfContains(correctAnswers, letter) && !CheckIfContains(wrongAnswers, letter))
                    {
                        wrongAnswers.Add(letter);
                    }
                }

                var currentPack = new SampleQuestionPack(newLetterData, wrongAnswers, correctAnswers);
                questions.Add(currentPack);
            }
        }

        static bool CheckIfContains(List<ILivingLetterData> list, ILivingLetterData letter)
        {
            for (int i = 0, count = list.Count; i < count; ++i)
                if (list[i].Id == letter.Id)
                    return true;
            return false;
        }

        IQuestionPack IQuestionProvider.GetNextQuestion()
        {
            currentQuestion++;

            if (currentQuestion >= questions.Count)
                currentQuestion = 0;

            return questions[currentQuestion];
        }
    }
}
