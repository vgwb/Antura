using Antura.Core;
using DG.Tweening;
using Kore.Coroutines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Assessment
{
    public class DefaultQuestionPlacer : IQuestionPlacer
    {
        protected AssessmentAudioManager audioManager;
        protected QuestionPlacerOptions options;
        protected AssessmentEvents events;

        public DefaultQuestionPlacer(AssessmentEvents events,
            AssessmentAudioManager audioManager, QuestionPlacerOptions options)
        {
            this.events = events;
            this.audioManager = audioManager;
            this.options = options;

            if (events != null)
                this.events.OnAllQuestionsAnsweredPlacer = ColorImageByGreen;
        }

        private IEnumerator ColorImageByGreen()
        {
            yield return null;
            cacheImage.Poof();
            cacheImage.SetQuestionGreen();
            yield return Wait.For(0.41f);
        }

        protected bool isAnimating = false;

        public bool IsAnimating()
        {
            return isAnimating;
        }

        protected IQuestion[] allQuestions;
        protected List<IEnumerator> questionSounds;
        private List<StillLetterBox> images;
        private List<QuestionBox> boxesList;

        public void Place(IQuestion[] question, bool playSound)
        {
            allQuestions = question;
            isAnimating = true;
            images = new List<StillLetterBox>();
            questionSounds = new List<IEnumerator>();
            boxesList = new List<QuestionBox>();
            Koroutine.Run(PlaceCoroutine(playSound));
        }


        IEnumerator PlaceCoroutine(bool playAudio)
        {
            return GetPlaceCoroutine(playAudio);
        }

        public virtual IEnumerator GetPlaceCoroutine(bool playAudio)
        {
            ResetAnimationsQueue();

            // Count questions and answers
            int questionsNumber = 0;
            int placeHoldersNumber = 0;

            foreach (var q in allQuestions)
            {
                questionsNumber++;
                placeHoldersNumber += q.PlaceholdersCount();
            }

            float questionSize = options.QuestionSize;
            float occupiedSpace =
                    placeHoldersNumber * options.SlotSize
                + questionsNumber * options.QuestionSize;

            if (options.SpawnImageWithQuestion)
            {
                occupiedSpace += options.ImageSize;
            }

            float blankSpace = options.RightX - options.LeftX - occupiedSpace;
            float spaceIncrement = blankSpace / (questionsNumber + 1);

            var flow = AssessmentOptions.Instance.LocaleTextDirection;
            float sign;
            Vector3 currentPos = new Vector3(0, options.QuestionY, options.DefaultZ);

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
                currentPos.x += (spaceIncrement + questionSize / 2) * sign;
                PlaceQuestion(allQuestions[questionIndex], currentPos, playAudio);
                currentPos.x += (questionSize * sign) / 2;

                if (options.SpawnImageWithQuestion)
                {
                    currentPos.x += (sign * options.ImageSize) / 1.8f;
                    PlaceImage(allQuestions[questionIndex], currentPos);
                    currentPos.x += (sign * options.ImageSize) / 1.8f;
                }

                foreach (var p in allQuestions[questionIndex].GetPlaceholders())
                {
                    currentPos.x += sign * options.SlotSize / 2;
                    PlacePlaceholder(p, currentPos);
                    currentPos.x += sign * options.SlotSize / 2;
                }

                WrapQuestionInABox(allQuestions[questionIndex]);
                foreach (var anim in AnimationsQueue)
                    yield return Koroutine.Nested(anim);

                AnimationsQueue.Clear();
                questionIndex++;
            }

            // give time to finish animating elements
            yield return Wait.For(0.65f);
            isAnimating = false;
        }

        protected List<IEnumerator> AnimationsQueue { get; private set; }

        protected void ResetAnimationsQueue()
        {
            AnimationsQueue = new List<IEnumerator>();
        }

        protected void WrapQuestionInABox(IQuestion q)
        {
            var ll = q.gameObject.GetComponent<StillLetterBox>();
            int placeholdersCount = 0;

            foreach (var p in q.GetPlaceholders())
            {
                placeholdersCount++;
            }

            StillLetterBox[] boxes = new StillLetterBox[placeholdersCount + 1];

            placeholdersCount = 0;
            foreach (var p in q.GetPlaceholders())
            {
                boxes[placeholdersCount++] = p.GetComponent<StillLetterBox>();
            }
            boxes[boxes.Length - 1] = ll;

            var box = ItemFactory.Instance.SpawnQuestionBox(boxes);
            box.Show();
            audioManager.PlayPoofSound();

            boxesList.Add(box);
        }

        protected StillLetterBox cacheImage = null;

        protected void PlaceImage(IQuestion q, Vector3 imagePos)
        {
            var ll = ItemFactory.Instance.SpawnQuestion(q.Image());
            cacheImage = ll;

            images.Add(ll);
            ll.transform.position = imagePos;
            ll.InstaShrink();

            AnimationsQueue.Add(ImageShowAnimation(ll));
        }

        private IEnumerator ImageShowAnimation(StillLetterBox letter)
        {
            letter.Poof();
            audioManager.PlayPoofSound();
            letter.Magnify();
            yield return Wait.For(0.9f);
        }

        IQuestion lastPlacedQuestion = null;

        protected void PlaceQuestion(IQuestion q, Vector3 position, bool playAudio)
        {
            lastPlacedQuestion = q;

            var ll = q.gameObject.GetComponent<StillLetterBox>();
            ll.transform.localPosition = position;
            ll.InstaShrink();

            AnimationsQueue.Add(QuestionShowAnimation(q, playAudio));
        }

        private IEnumerator QuestionShowAnimation(IQuestion q, bool playAudio)
        {
            var letter = q.gameObject.GetComponent<StillLetterBox>();
            letter.Poof();
            audioManager.PlayPoofSound();
            letter.transform.GetComponent<StillLetterBox>().Magnify();

            if (playAudio)
            {
                q.QuestionBehaviour.ReadMeSound();
            }
            yield return Wait.For(1.6f);
        }

        protected void PlacePlaceholder(GameObject placeholder, Vector3 position)
        {
            var tr = placeholder.transform;
            tr.localPosition = position;
            var box = tr.GetComponent<StillLetterBox>();
            box.InstaShrink();

            AnimationsQueue.Add(PlaceholderShowAnimation(box));
        }

        private IEnumerator PlaceholderShowAnimation(StillLetterBox box)
        {
            audioManager.PlayPlaceSlot();
            box.Magnify();
            yield return Wait.For(0.2f);
        }

        public void RemoveQuestions()
        {
            isAnimating = true;
            Koroutine.Run(RemoveCoroutine());
        }

        IEnumerator RemoveCoroutine()
        {
            foreach (var q in allQuestions)
            {
                foreach (var p in q.GetPlaceholders())
                {
                    yield return Koroutine.Nested(FadeOutPlaceholder(p));
                }
                foreach (var img in images)
                {
                    yield return Koroutine.Nested(FadeOutImage(img));
                }
                yield return Koroutine.Nested(FadeOutQuestion(q));
            }

            foreach (var box in boxesList)
            {
                box.Hide();
            }

            // give time to finish animating elements
            yield return Wait.For(0.45f);
            isAnimating = false;
        }

        IEnumerator FadeOutImage(StillLetterBox image)
        {
            audioManager.PlayPoofSound();
            image.Poof();

            image.transform.DOScale(0, 0.4f).OnComplete(() => GameObject.Destroy(image.gameObject));
            yield return Wait.For(0.1f);
        }

        IEnumerator FadeOutQuestion(IQuestion q)
        {
            audioManager.PlayPoofSound();
            q.gameObject.GetComponent<StillLetterBox>().Poof();
            q.gameObject.transform.DOScale(0, 0.4f).OnComplete(() => GameObject.Destroy(q.gameObject));
            yield return Wait.For(0.1f);
        }

        IEnumerator FadeOutPlaceholder(GameObject go)
        {
            audioManager.PlayRemoveSlot();

            go.transform.DOScale(0, 0.23f).OnComplete(() => GameObject.Destroy(go));
            yield return Wait.For(0.06f);
        }

        /// <summary>
        ///  Should highlight 1 QUESTION and play their audio. This is called
        ///  only if we have to pronunce question, and only if we should pronunce it
        ///  after the tutorial brief. Actually this is always called in the first round
        ///  (I find it more natural) but requisites may change later so, it is still
        ///  possible that we have to play this Before the tutorial brief.
        ///  (that is the reason I still have a
        ///  AssessmentOptions.Instance.PlayQuestionAudioAfterTutorial
        ///  flag
        /// </summary>
        public IYieldable PlayQuestionSound()
        {
            audioManager.PlayQuestionBlip();

            var sequence = DOTween.Sequence();
            lastPlacedQuestion.QuestionBehaviour.ReadMeSound();
            lastPlacedQuestion.gameObject.GetComponent<StillLetterBox>().Poof();

            sequence
                .Append(lastPlacedQuestion.gameObject.transform.DOScale(0.5f, 0.15f))
                .Append(lastPlacedQuestion.gameObject.transform.DOScale(1.0f, 0.15f));

            return Wait.For(1.0f);
        }
    }
}
