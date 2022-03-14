using UnityEngine;
using System.Collections.Generic;
using Antura.LivingLetters;
using Antura.LivingLetters.Sample;
using Antura.Core;

namespace Antura.Minigames.SickLetters
{
    /*
    This is a Dummy Provider, used only when the game is started from the Editor,
    will be overridden when the game is called from the debugger/Map.
    */

    public class SickLettersQuestionProvider : IQuestionProvider
    {
        string dotlessLetters = "ﻻ لأ ﺉ آ ٶ ى ر س ل ص ع ه ح د م ك ط ئ ء ؤ و �ة - ", prevLetter = "", newLetterString = "X";

        public IQuestionPack GetNextQuestion()
        {
            LL_LetterData newLetter;
            List<ILivingLetterData> correctAnswers = new List<ILivingLetterData>();
            List<ILivingLetterData> wrongAnswers = new List<ILivingLetterData>();

            prevLetter = newLetterString;
            do
            {
                newLetter = AppManager.I.Teacher.GetRandomTestLetterLL();
                newLetterString = newLetter.TextForLivingLetter;
            }
            while (newLetterString == "" || dotlessLetters.Contains(newLetterString) || newLetterString == prevLetter);

            //Debug.Log(newLetterString);

            correctAnswers.Add(newLetter);
            return new SampleQuestionPack(newLetter, wrongAnswers, correctAnswers);

        }

    }
}
