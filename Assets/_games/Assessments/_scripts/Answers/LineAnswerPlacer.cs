using Antura.Core;
using Antura.LivingLetters;
using DG.DeExtensions;
using DG.Tweening;
using Kore.Coroutines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Assessment
{
    /// <summary>
    /// Place answers in a line, ready to be sorted
    /// </summary>
    internal class LineAnswerPlacer : IAnswerPlacer
    {
        private AssessmentAudioManager audioManager;
        private float letterSize;

        public LineAnswerPlacer(AssessmentAudioManager audioManager, float letterSize)
        {
            this.audioManager = audioManager;
            this.letterSize = letterSize;
        }

        private bool isAnimating = false;
        public bool IsAnimating()
        {
            return isAnimating;
        }

        private Answer[] allAnswers;

        public void Place(Answer[] answer)
        {
            Answer[] original = new Answer[answer.Length];
            placeholders = new List<StillLetterBox>();

            for (int i = 0; i < answer.Length; i++)
            {
                original[i] = answer[i];
            }

            for (int i = 0; i < answer.Length; i++)
            {
                answer[i].AddTicket(i);
            }

            answer.Shuffle();

            // Avoid the case where all letters are already in correct order (automatic win)
            // In case we see there would be an automatic win, we just swap the first 2
            // different letters. This may introduce some bias on shorter words.. but that's
            // ok. This is not a gamblin game.
            ForceRandom(answer, original);

            allAnswers = answer;
            isAnimating = true;
            Koroutine.Run(PlaceCoroutine());
        }

        public void ForceRandom(Answer[] answer, Answer[] original)
        {
            for (int i = 0; i < answer.Length; i++)
            {
                // if there is one element out of place.. GOOD!
                if (answer[i].Equals(original[i]) == false)
                {
                    return;
                }
            }

            // Otherwise we swap the first 2 elements that are different
            // (we force at least 1 out of place to prevent automatic victory!)
            for (int i = 0; i < answer.Length; i++)
            {
                if (answer[i].Equals(answer[0]) == false)
                {
                    Answer first = answer[0];
                    answer[0] = answer[i];
                    answer[i] = first;
                }
            }
        }

        List<Answer> playbackAnswers = null;

        private IEnumerator PlaceCoroutine()
        {
            // Text justification "algorithm"
            float spaceIncrement = 0.5f + letterSize;
            float occupiedSpace = allAnswers.Length * spaceIncrement - 3.5f;

            var flow = AssessmentOptions.Instance.LocaleTextDirection;
            float sign;
            Vector3 currentPos = Vector3.zero;
            currentPos.y = -1;
            currentPos.z = 5;

            if (flow == TextDirection.RightToLeft)
            {
                currentPos.x = occupiedSpace / 2f;
                sign = -1;
            }
            else
            {
                currentPos.x = -occupiedSpace / 2f;
                sign = 1;
            }

            currentPos.y -= 1.5f;

            // Move answer so we can wrap them in bg.
            foreach (var a in allAnswers)
            {
                MoveAnswer(a, currentPos);
                currentPos.x += spaceIncrement * sign;
            }

            // Spawn BG
            SpawnLettersBG();

            // Spawn Answers.
            playbackAnswers = new List<Answer>();
            foreach (var a in allAnswers)
            {
                yield return Koroutine.Nested(PlaceAnswer(a));
            }

            yield return Koroutine.Nested(PlayBackCorrectAnswers());

            yield return Wait.For(0.65f);
            isAnimating = false;
        }

        private void MoveAnswer(Answer a, Vector3 currentPos)
        {
            var go = a.gameObject;
            go.transform.localPosition = currentPos;
            AddPlaceHolder(currentPos);
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

        private IEnumerator PlaceAnswer(Answer a)
        {
            var go = a.gameObject;
            go.GetComponent<StillLetterBox>().Poof();
            go.GetComponent<StillLetterBox>().Magnify();
            audioManager.PlayPoofSound();
            if (a.IsCorrect())
            {
                if (playbackAnswers.Count == 0 && AssessmentOptions.Instance.PlayCorrectAnswer)
                {
                    playbackAnswers.Add(a);
                }
                else if (AssessmentOptions.Instance.PlayAllCorrectAnswers)
                {
                    playbackAnswers.Add(a);
                }
            }

            yield return Wait.For(Random.Range(0.07f, 0.13f));
        }

        List<StillLetterBox> placeholders;
        QuestionBox bgBox;

        private void AddPlaceHolder(Vector3 currentPos)
        {
            currentPos.z = 5.5f;
            var placeholder = ItemFactory.Instance.SpawnPlaceholder(LivingLetterDataType.Letter);
            placeholder.transform.localPosition = currentPos;
            placeholder.InstaShrink();
            placeholders.Add(placeholder);
        }

        private void SpawnLettersBG()
        {
            bgBox = ItemFactory.Instance.SpawnQuestionBox(placeholders);
            bgBox.Show();

            foreach (var p in placeholders)
            {
                p.Magnify();
            }
        }

        public void RemoveAnswers()
        {
            isAnimating = true;
            Koroutine.Run(RemoveCoroutine());
        }

        private IEnumerator RemoveCoroutine()
        {
            foreach (var a in allAnswers)
                yield return Koroutine.Nested(RemoveAnswer(a.gameObject));

            yield return Wait.For(0.2f);

            bgBox.Hide();

            foreach (var p in placeholders)
            {
                RemovePlaceholder(p);
            }

            audioManager.PlayPoofSound();
            yield return Wait.For(0.3f);

            isAnimating = false;
        }

        private void RemovePlaceholder(StillLetterBox box)
        {
            box.transform.DOScale(0, 0.3f).OnComplete(() => GameObject.Destroy(box));
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
