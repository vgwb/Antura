using System.Collections.Generic;
using Antura.LivingLetters;

namespace Antura.Minigames.Egg
{
    public class EggChallenge
    {
        public List<ILivingLetterData> Letters { get; private set; }
        public ILivingLetterData Question { get; private set; }
        bool sequence;

        public EggChallenge(float difficulty, bool onlyLetter)
        {
            Letters = new List<ILivingLetterData>();
            sequence = false;

            IQuestionPack questionPack = EggConfiguration.Instance.Questions.GetNextQuestion();

            List<ILivingLetterData> correctAnswers = new List<ILivingLetterData>();
            List<ILivingLetterData> wrongAnswers = new List<ILivingLetterData>();

            Question = questionPack.GetQuestion();
            correctAnswers.AddRange(questionPack.GetCorrectAnswers());
            wrongAnswers.AddRange(questionPack.GetWrongAnswers());

            sequence = EggConfiguration.Instance.IsSequence();

            int numberOfLetters = 3;

            if (EggConfiguration.Instance.Variation == EggVariation.BuildWord)
                numberOfLetters = 8;
            else
            {
                if (sequence)
                {
                    if (difficulty < 0.5f)
                    {
                        numberOfLetters += UnityEngine.Mathf.RoundToInt(difficulty * 6);
                    }
                    else
                    {
                        numberOfLetters += UnityEngine.Mathf.RoundToInt((difficulty - 0.5f) * 4f);
                    }
                    if (numberOfLetters > 6)
                    {
                        numberOfLetters = 6;
                    }
                }
                else
                {
                    numberOfLetters += (int)(difficulty * 5);

                    if (numberOfLetters > 8)
                    {
                        numberOfLetters = 8;
                    }
                }
            }

            if (!sequence)
            {
                Letters.Add(correctAnswers[0]);

                numberOfLetters -= 1;

                if (numberOfLetters > wrongAnswers.Count)
                {
                    numberOfLetters = wrongAnswers.Count;
                }

                for (int i = 0; i < numberOfLetters; i++)
                {
                    Letters.Add(wrongAnswers[i]);
                }
            }
            else
            {
                if (numberOfLetters > correctAnswers.Count)
                {
                    numberOfLetters = correctAnswers.Count;
                }

                for (int i = 0; i < numberOfLetters; i++)
                {
                    Letters.Add(correctAnswers[i]);
                }
            }
        }

        public bool IsSequence()
        {
            return sequence;
        }
    }
}