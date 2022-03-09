using Antura.LivingLetters;
using Antura.LivingLetters.Sample;
using Antura.Core;
using System.Collections.Generic;

namespace Antura.Minigames.DancingDots
{
    public class DancingDotsQuestionProvider : IQuestionProvider
    {

        public IQuestionPack GetNextQuestion()
        {
            LL_LetterData newLetter;
            List<ILivingLetterData> correctAnswers = new List<ILivingLetterData>();
            List<ILivingLetterData> wrongAnswers = new List<ILivingLetterData>();

            newLetter = AppManager.I.Teacher.GetRandomTestLetterLL();

            correctAnswers.Add(newLetter);
            return new SampleQuestionPack(newLetter, wrongAnswers, correctAnswers);
        }

        //		public DancingDotsQuestionsPack DancingDotsGetNextQuestion()
        //		{
        //			LL_LetterData newLetter;
        //
        //			newLetter = AppManager.I.Teacher.GetRandomTestLetterLL();
        //
        //			DancingDotsQuestionsPack dataPack = new DancingDotsQuestionsPack(newLetter);
        //			return dataPack;
        //		}
        //
        //		public ILivingLetterData GetNextQuestion_Temp()
        //		{
        //			LL_LetterData newLetter;
        //
        //			newLetter = AppManager.I.Teacher.GetRandomTestLetterLL();
        //
        //			return newLetter;
        //		}

    }
}
