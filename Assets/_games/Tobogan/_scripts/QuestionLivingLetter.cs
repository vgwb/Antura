using UnityEngine;
using DG.Tweening;
using System;
using Antura.Helpers;
using Antura.LivingLetters;
using Antura.UI;
using Antura.Core;
using Antura.Database;
using Antura.Language;

namespace Antura.Minigames.Tobogan
{
    public class QuestionLivingLetter : MonoBehaviour
    {
        [HideInInspector]
        public PipeAnswer NearTube;

        public bool playWhenDragged = true;
        public Transform livingLetterTransform;
        public BoxCollider boxCollider;

        public LivingLetterController letter;

        private Tweener moveTweener;
        private Tweener rotationTweener;

        private Vector3 holdPosition;
        private Vector3 normalPosition;

        private float cameraDistance;

        private Camera tubesCamera;
        private float minX;
        private float maxX;
        private float minY;
        private float maxY;

        public bool Sucked = false;

        private bool isFalling;
        private bool dragging = false;
        private Vector3 dragOffset = Vector3.zero;

        public event Action onMouseUpLetter;

        private Action endTransformToCallback;

        private Transform[] letterPositions;
        private int currentPosition;

        private Vector3 colliderStartScale;

        private Vector3 targetDragPosition;

        public Vector3? TargetContentDragPosition
        {
            get
            {
                if (dragging)
                {
                    return targetDragPosition + ContentOffset;
                }
                return null;
            }
        }

        Vector3 ContentOffset { get; set; }

        void Awake()
        {
            normalPosition = livingLetterTransform.localPosition;

            holdPosition.x = normalPosition.x;
            holdPosition.y = normalPosition.y;

            colliderStartScale = boxCollider.size;

            ContentOffset = letter.contentTransform.position - transform.position;
        }

        public void Initialize(Camera tubesCamera, Vector3 upRightMaxPosition, Vector3 downLeftMaxPosition, Transform[] letterPositions)
        {
            this.tubesCamera = tubesCamera;
            this.letterPositions = letterPositions;

            cameraDistance = Vector3.Distance(tubesCamera.transform.position, letterPositions[letterPositions.Length - 1].position);

            minX = downLeftMaxPosition.x;
            maxX = upRightMaxPosition.x;
            minY = downLeftMaxPosition.y;
            maxY = upRightMaxPosition.y;

            EnableCollider(true);
        }

        public void PlayIdleAnimation()
        {
            letter.SetState(LLAnimationStates.LL_idle);

            livingLetterTransform.localPosition = normalPosition;
        }

        public void PlayStillAnimation()
        {
            letter.SetState(LLAnimationStates.LL_still);

            livingLetterTransform.localPosition = normalPosition;
        }

        public void PlayWalkAnimation()
        {
            letter.SetState(LLAnimationStates.LL_walking);
            letter.SetWalkingSpeed(LivingLetterController.WALKING_SPEED);

            livingLetterTransform.localPosition = normalPosition;
        }

        public void PlayHoldAnimation()
        {
            letter.SetState(LLAnimationStates.LL_dragging);

            livingLetterTransform.localPosition = holdPosition;
        }

        public void SetQuestionText(ILivingLetterData livingLetterData)
        {
            letter.Init(livingLetterData);
        }

        public void SetQuestionText(LL_WordData word, LL_LetterData markedLetter, Color color)
        {
            string text = LanguageSwitcher.LearningHelper.ProcessString(word.Data.Text);
            var parts = LanguageSwitcher.LearningHelper.FindLetter(AppManager.I.DB, word.Data, markedLetter.Data, LetterEqualityStrictness.Letter);
            if (parts.Count > 0)
            {
                text = LanguageSwitcher.LearningHelper.GetWordWithMarkedLetterText(word.Data, parts[0], color, MarkType.SingleLetter);
            }
            letter.Init(word, text, 1.3f);
        }

        public void SetQuestionText(LL_WordData word, int letterToMark, Color color)
        {
            string text = LanguageSwitcher.LearningHelper.ProcessString(word.Data.Text);

            var parts = LanguageSwitcher.LearningHelper.SplitWord(AppManager.I.DB, word.Data);
            if (parts.Count > letterToMark)
            {
                text = LanguageSwitcher.LearningHelper.GetWordWithMarkedLetterText(word.Data, parts[letterToMark], color, MarkType.SingleLetter);
            }
            letter.Init(word, text, 1.3f);
        }

        public void ClearQuestionText()
        {
            letter.Init(null);
        }

        void MoveTo(Vector3 position, float duration)
        {
            PlayWalkAnimation();

            if (moveTweener != null)
            {
                moveTweener.Kill();
            }

            moveTweener = transform.DOLocalMove(position, duration).OnComplete(delegate ()
            {
                if (letter.Data == null)
                {
                    PlayStillAnimation();
                }
                else
                {
                    PlayIdleAnimation();
                }
                if (endTransformToCallback != null)
                {
                    endTransformToCallback();
                }
            });
        }

        void RoteteTo(Vector3 rotation, float duration)
        {
            if (rotationTweener != null)
            {
                rotationTweener.Kill();
            }

            rotationTweener = transform.DORotate(rotation, duration);
        }

        void TransformTo(Transform transformTo, float duration, Action callback)
        {
            MoveTo(transformTo.localPosition, duration);
            RoteteTo(transformTo.eulerAngles, duration);

            endTransformToCallback = callback;
        }

        public void GoToFirstPostion()
        {
            GoToPosition(0);
        }

        public void GoToPosition(int positionNumber)
        {
            Sucked = false;
            isFalling = false;

            if (moveTweener != null)
            { moveTweener.Kill(); }
            if (rotationTweener != null)
            { rotationTweener.Kill(); }

            currentPosition = positionNumber;

            transform.localPosition = letterPositions[currentPosition].localPosition;
            transform.rotation = letterPositions[currentPosition].rotation;
        }

        public void MoveToNextPosition(float duration, Action callback)
        {
            isFalling = false;

            if (moveTweener != null)
            { moveTweener.Kill(); }
            if (rotationTweener != null)
            { rotationTweener.Kill(); }

            currentPosition++;

            if (currentPosition >= letterPositions.Length)
            {
                currentPosition = 0;
            }

            TransformTo(letterPositions[currentPosition], duration, callback);
        }

        public void OnPointerDown(Vector2 pointerPosition)
        {
            if (!dragging)
            {
                dragging = true;

                var data = letter.Data;

                if (playWhenDragged)
                {
                    ToboganConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(data, true, soundType: ToboganConfiguration.Instance.GetVocabularySoundType());
                }
                Vector3 mousePosition = new Vector3(pointerPosition.x, pointerPosition.y, cameraDistance);
                Vector3 world = tubesCamera.ScreenToWorldPoint(mousePosition);
                dragOffset = world - transform.position;

                OnPointerDrag(pointerPosition);

                PlayHoldAnimation();
            }
        }

        public void OnPointerDrag(Vector2 pointerPosition)
        {
            if (dragging)
            {
                isFalling = false;

                Vector3 mousePosition = new Vector3(pointerPosition.x, pointerPosition.y, cameraDistance);

                targetDragPosition = tubesCamera.ScreenToWorldPoint(mousePosition);
                targetDragPosition = ClampPositionToStage(targetDragPosition - dragOffset);
            }
        }

        public void OnPointerUp()
        {
            if (dragging)
            {
                dragging = false;
                isFalling = true;

                PlayIdleAnimation();

                if (onMouseUpLetter != null)
                {
                    onMouseUpLetter();
                }
            }
        }

        void Update()
        {
            if (!dragging)
            {
                NearTube = null;
            }
            Vector3 targetScale;
            Vector3 targetPosition = transform.position;

            if (Sucked)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 10.0f * Time.deltaTime);
                transform.position += Vector3.up * Time.deltaTime * 20;
            }
            else
            {
                if (dragging)
                {
                    targetPosition = targetDragPosition;
                }
                if (NearTube != null)
                {
                    float yScale = 1.3f * (1 + 0.1f * Mathf.Cos(Time.realtimeSinceStartup * 3.14f * 6));

                    targetScale = 0.75f * new Vector3(1 / yScale, yScale, 1);

                    targetPosition = NearTube.tutorialPoint.position - ContentOffset;

                    targetPosition.y = Mathf.Lerp(targetPosition.y, maxY, 2.0f * Mathf.Abs(targetPosition.x - transform.position.x));
                }
                else
                {
                    targetScale = Vector3.one;
                }

                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 15.0f * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, targetPosition, 25.0f * Time.deltaTime);

                if (isFalling)
                {
                    // Linear fall
                    transform.position = ClampPositionToStage(transform.position + Vector3.down * 20 * Time.deltaTime);
                }
            }

            boxCollider.size = new Vector3(colliderStartScale.x * letter.Scale, colliderStartScale.y, colliderStartScale.z);
        }

        Vector3 ClampPositionToStage(Vector3 unclampedPosition)
        {
            Vector3 clampedPosition = unclampedPosition;

            clampedPosition.x = clampedPosition.x < minX ? minX : clampedPosition.x;
            clampedPosition.x = clampedPosition.x > maxX ? maxX : clampedPosition.x;
            clampedPosition.y = clampedPosition.y < minY ? minY : clampedPosition.y;
            clampedPosition.y = clampedPosition.y > maxY ? maxY : clampedPosition.y;

            return clampedPosition;
        }

        public void EnableCollider(bool enable)
        {
            boxCollider.enabled = enable;
        }
    }
}
