using System.Collections.Generic;
using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.Egg
{
    public class EggChallenge
    {
        public List<ILivingLetterData> Answers { get; private set; }
        public ILivingLetterData Question { get; private set; }
        bool sequence;

        public EggChallenge(float difficulty)
        {
            Answers = new List<ILivingLetterData>();
            sequence = false;

            IQuestionPack questionPack = EggConfiguration.Instance.Questions.GetNextQuestion();

            List<ILivingLetterData> correctAnswers = new List<ILivingLetterData>();
            List<ILivingLetterData> wrongAnswers = new List<ILivingLetterData>();

            Question = questionPack.GetQuestion();
            correctAnswers.AddRange(questionPack.GetCorrectAnswers());
            wrongAnswers.AddRange(questionPack.GetWrongAnswers());

            sequence = EggConfiguration.Instance.IsSequence();

            int numberOfAnswers = 3;

            if (EggConfiguration.Instance.Variation == EggVariation.BuildWord)
                numberOfAnswers = 8;
            else
            {
                if (sequence)
                {
                    if (difficulty < 0.5f)
                    {
                        numberOfAnswers += UnityEngine.Mathf.RoundToInt(difficulty * 6);
                    }
                    else
                    {
                        numberOfAnswers += UnityEngine.Mathf.RoundToInt((difficulty - 0.5f) * 4f);
                    }
                    if (numberOfAnswers > 6)
                    {
                        numberOfAnswers = 6;
                    }
                }
                else
                {
                    numberOfAnswers += Mathf.CeilToInt(difficulty * 3);

                    if (numberOfAnswers > 8)
                    {
                        numberOfAnswers = 8;
                    }
                }
            }

            if (!sequence)
            {
                Answers.Add(correctAnswers[0]);

                numberOfAnswers -= 1;

                if (numberOfAnswers > wrongAnswers.Count)
                {
                    numberOfAnswers = wrongAnswers.Count;
                }

                for (int i = 0; i < numberOfAnswers; i++)
                {
                    Answers.Add(wrongAnswers[i]);
                }
            }
            else
            {
                if (numberOfAnswers > correctAnswers.Count)
                {
                    numberOfAnswers = correctAnswers.Count;
                }

                for (int i = 0; i < numberOfAnswers; i++)
                {
                    Answers.Add(correctAnswers[i]);
                }
            }
        }

        public bool IsSequence()
        {
            return sequence;
        }
    }
}
