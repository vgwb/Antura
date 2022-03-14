using Antura.Core;
using Kore.Coroutines;
using System.Collections;
using UnityEngine;

namespace Antura.Assessment
{
    internal class CategoryQuestionPlacer : DefaultQuestionPlacer
    {
        public CategoryQuestionPlacer(AssessmentAudioManager audioManager, QuestionPlacerOptions options)
            : base(null, audioManager, options)
        {
        }

        public override IEnumerator GetPlaceCoroutine(bool playAudio)
        {
            ResetAnimationsQueue();

            if (playAudio)
                // warn our heirs
                Debug.LogWarning("playAudio, parameter not used for Categorization questions");

            // Count questions and answers
            int questionsNumber = 0;
            int placeHoldersNumber = 0;

            foreach (var q in allQuestions)
            {
                questionsNumber++;
                placeHoldersNumber += q.PlaceholdersCount();
            }

            float occupiedSpace = placeHoldersNumber * options.SlotSize;
            float blankSpace = options.RightX - options.LeftX - occupiedSpace;
            float spaceIncrement = blankSpace / (questionsNumber + 1);

            var flow = AssessmentOptions.Instance.LocaleTextDirection;
            float sign;
            Vector3 currentPos = new Vector3(0, options.QuestionY - 3.5f, options.DefaultZ);

            if (flow == TextDirection.RightToLeft)
            {
                currentPos.x = options.RightX;
                sign = -1;
            }
            else
            {
                currentPos.x = options.LeftX;
                sign = 1;
            }

            int questionIndex = 0;
            for (int i = 0; i < questionsNumber; i++)
            {
                currentPos.x += spaceIncrement * sign;
                float min = 1000, max = -1000;

                foreach (var p in allQuestions[questionIndex].GetPlaceholders())
                {
                    currentPos.x += (options.SlotSize / 2) * sign;

                    if (currentPos.x > max)
                    {
                        max = currentPos.x;
                    }

                    if (currentPos.x < min)
                    {
                        min = currentPos.x;
                    }

                    PlacePlaceholder(p, currentPos);
                    currentPos.x += (options.SlotSize / 2) * sign;
                }

                var questionPos = currentPos;
                questionPos.y = options.QuestionY;
                questionPos.x = (max + min) / 2f;

                // Category questions never read the category
                PlaceQuestion(allQuestions[questionIndex], questionPos, false);

                WrapQuestionInABox(allQuestions[questionIndex]);
                foreach (var anim in AnimationsQueue)
                {
                    yield return Koroutine.Nested(anim);
                }

                AnimationsQueue.Clear();

                questionIndex++;
            }

            // give time to finish animating elements
            yield return Wait.For(0.65f);
            isAnimating = false;
        }
    }
}
