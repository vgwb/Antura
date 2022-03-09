using System.Collections.Generic;
using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.FastCrowd
{
    public class QuestionManager : MonoBehaviour
    {
        public event System.Action OnCompleted;
        public event System.Action<ILivingLetterData, bool> OnDropped;

        public DropAreaWidget dropContainer;
        public FastCrowdLetterCrowd crowd;
        public WordComposer wordComposer;

        void Start()
        {
            dropContainer.OnComplete += OnContainerComplete;
            crowd.onDropped += OnLetterDropped;

            //crowd.MaxConcurrentLetters = Mathf.RoundToInt(4 + game.Difficulty * 4);
        }

        void OnContainerComplete()
        {
            if (OnCompleted != null)
                OnCompleted();
        }

        void OnLetterDropped(ILivingLetterData data, bool result)
        {
            if (result)
            {
                dropContainer.AdvanceArea();

                if (data is LL_LetterData)
                    wordComposer.AddLetter(data);
            }

            if (OnDropped != null)
                OnDropped(data, result);
        }

        public void StartQuestion(List<ILivingLetterData> nextChallenge, List<ILivingLetterData> wrongAnswers)
        {
            Clean();

            for (int i = 0; i < nextChallenge.Count; ++i)
            {
                var correctAnswer = nextChallenge[i];

                // Add drop areas
                if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Counting)
                    dropContainer.AddDropData(correctAnswer, true);
                else if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Word
                         || FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Image
                         || FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.CategoryForm
                         || FastCrowdConfiguration.Instance.IsOrderingVariation)
                {
                    dropContainer.AddDropData(correctAnswer, true);
                }
                else
                {
                    dropContainer.AddDropData(correctAnswer, false);
                }

                // Add living letters
                if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Image
                    || FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.CategoryForm
                    || FastCrowdConfiguration.Instance.IsOrderingVariation
                    )
                    correctAnswer = new LL_ImageData(correctAnswer.Id);

                crowd.AddLivingLetter(correctAnswer);
            }

            foreach (var wrongAnswer in wrongAnswers)
            {
                var answer = wrongAnswer;
                if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Image
                    || FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.CategoryForm
                    || FastCrowdConfiguration.Instance.IsOrderingVariation)
                    answer = new LL_ImageData(answer.Id);

                // Add living letters
                crowd.AddLivingLetter(answer);
            }
        }

        /*
        public void StartQuestion(IQuestionPack nextQuestion)
        {
            Clean();

            foreach (var correctAnswer in nextQuestion.GetCorrectAnswers())
            {
                // Add drop areas
                dropContainer.AddDropArea(correctAnswer);

                // Add living letters
                crowd.AddLivingLetter(correctAnswer);
            }

            foreach (var wrongAnswers in nextQuestion.GetWrongAnswers())
            {
                // Add living letters
                crowd.AddLivingLetter(wrongAnswers);
            }
        }
        */

        public void Clean()
        {
            wordComposer.Clean();
            dropContainer.Clean();
            crowd.Clean();
        }
    }
}
