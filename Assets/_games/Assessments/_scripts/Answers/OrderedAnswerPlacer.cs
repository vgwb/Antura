using Antura.Core;
using Antura.Extensions;
using Antura.Helpers;
using DG.Tweening;
using Kore.Coroutines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Assessment
{
    /// <summary>
    /// Place answers to random places over a grid with small displacement
    /// (answers are not overlapped and start in different positions,
    /// without overlapping the question area).
    /// </summary>
    public class OrderedAnswerPlacer : IAnswerPlacer
    {
        public OrderedAnswerPlacer(AssessmentAudioManager audioManager, QuestionPlacerOptions placerOptions)
        {
            this.audioManager = audioManager;
            this.placerOptions = placerOptions;
        }

        private bool isAnimating = false;
        public bool IsAnimating()
        {
            return isAnimating;
        }

        private Answer[] allAnswers;
        private AssessmentAudioManager audioManager;
        private QuestionPlacerOptions placerOptions;

        public void Place(Answer[] answer)
        {
            allAnswers = answer;
            isAnimating = true;
            Koroutine.Run(PlaceCoroutine());
        }

        public void RemoveAnswers()
        {
            isAnimating = true;
            Koroutine.Run(RemoveCoroutine());
        }

        private IEnumerator PlaceCoroutine()
        {
            allAnswers.Shuffle();

            List<Vector3> positions = new List<Vector3>();
            float xMin = placerOptions.LeftX + 2.0f;
            float xMax = placerOptions.RightX - 2.0f;
            float yMin = placerOptions.BottomY + 2.9f;
            float z = placerOptions.DefaultZ;

            float deltaX = xMax - xMin;

            int answerCount = 0;
            foreach (var a in allAnswers)
            {
                answerCount++;
            }

            float answerGap = placerOptions.AnswerSize + 0.5f;
            var flow = AssessmentOptions.Instance.LocaleTextDirection;
            while (answerCount > 0)
            {
                int answersInThisLine = Mathf.FloorToInt(deltaX / answerGap);
                if (answersInThisLine > answerCount)
                {
                    answersInThisLine = answerCount;
                }

                // space occupied by this line
                float lineSpace = (answersInThisLine - 1) * 0.5f + answersInThisLine * placerOptions.AnswerSize;
                // startin X position for an answer
                float startX = (lineSpace / 2.0f) + (placerOptions.AnswerSize / 2.0f);
                float direction = -1;

                if (flow == TextDirection.LeftToRight) // Not Arabic
                {
                    startX = -startX;
                    direction = 1;
                }

                for (int i = 0; i < answersInThisLine && answerCount > 0; i++, answerCount--)
                {
                    startX += direction * (0.5f + placerOptions.AnswerSize);
                    var position = new Vector3(startX, yMin, z);
                    positions.Add(position);
                }

                yMin += 3.5f;
            }

            positions.Shuffle();

            playbackAnswers = new List<Answer>();
            foreach (var a in allAnswers)
            {
                yield return Koroutine.Nested(PlaceAnswer(a, positions));
            }
            yield return Koroutine.Nested(PlayBackCorrectAnswers());

            yield return Wait.For(0.65f);
            isAnimating = false;
        }

        List<Answer> playbackAnswers = null;

        private IEnumerator PlaceAnswer(Answer answer, List<Vector3> positions)
        {
            var go = answer.gameObject;
            go.transform.localPosition = positions.Pull();
            go.GetComponent<StillLetterBox>().InstaShrink();
            go.GetComponent<StillLetterBox>().Poof();
            go.GetComponent<StillLetterBox>().Magnify();
            audioManager.PlayPoofSound();
            if (answer.IsCorrect())
            {
                if (playbackAnswers.Count == 0 && AssessmentOptions.Instance.PlayCorrectAnswer)
                {
                    playbackAnswers.Add(answer);
                }
                else if (AssessmentOptions.Instance.PlayAllCorrectAnswers)
                {
                    playbackAnswers.Add(answer);
                }
            }

            yield return Wait.For(Random.Range(0.07f, 0.13f));
        }

        private IEnumerator PlayBackCorrectAnswers()
        {
            playbackAnswers.Shuffle();
            foreach (var a in playbackAnswers)
            {
                yield return Koroutine.Nested(a.PlayLetter());
                yield return Wait.For(0.3f);
            }
            playbackAnswers = null;
        }

        private IEnumerator RemoveCoroutine()
        {
            foreach (var a in allAnswers)
            {
                yield return Koroutine.Nested(RemoveAnswer(a.gameObject));
            }
            yield return Wait.For(0.65f);
            isAnimating = false;
        }

        private IEnumerator RemoveAnswer(GameObject answ)
        {
            audioManager.PlayPoofSound();

            answ.GetComponent<StillLetterBox>().Poof();
            answ.transform.DOScale(0, 0.3f).OnComplete(() => GameObject.Destroy(answ));

            yield return Wait.For(0.1f);
        }
    }
}
