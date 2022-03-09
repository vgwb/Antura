using Antura.LivingLetters;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Antura.Minigames.DancingDots
{
    public class DancingDotsQuestionsPack : MonoBehaviour, IQuestionPack
    {
        LL_LetterData correctAnswers, question;

        public DancingDotsQuestionsPack(LL_LetterData LLData)
        {
            question = correctAnswers = LLData;
        }

        public LL_LetterData GetAnswer()
        {
            return correctAnswers;
        }

        public IEnumerable<ILivingLetterData> GetCorrectAnswers()
        {
            throw new NotImplementedException();
        }

        public ILivingLetterData GetQuestion()
        {
            return question;
        }

        public IEnumerable<ILivingLetterData> GetQuestions()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ILivingLetterData> GetWrongAnswers()
        {
            throw new NotImplementedException();
        }

    }
}
