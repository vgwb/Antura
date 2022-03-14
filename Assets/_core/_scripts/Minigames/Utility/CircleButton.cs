using DG.Tweening;
using System;
using Antura.LivingLetters;
using Antura.UI;
using UnityEngine;

namespace Antura.Minigames
{
    /// <summary>
    /// A circle-shaped button.
    /// </summary>
    // refactor: should be merged with other UI elements
    public class CircleButton : MonoBehaviour
    {
        bool active = true;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public UnityEngine.UI.Image button;
        public TextRender text;

        public Action<CircleButton> onClicked;

        bool isDestroying;
        float destroyTimer;
        System.Action onAnimationCompleted;

        ILivingLetterData answer;

        public ILivingLetterData Answer
        {
            get { return answer; }
            set
            {
                if (isDestroying)
                    return;

                answer = value;
                text.SetLetterData(answer);
            }
        }

        public void SetColor(Color color)
        {
            button.color = color;
        }

        public void Destroy(float delay = 0, System.Action onAnimationCompleted = null)
        {
            destroyTimer = delay;
            isDestroying = true;
            this.onAnimationCompleted = onAnimationCompleted;
        }

        public void OnClicked()
        {
            if (!Active)
                return;

            if (isDestroying)
                return;

            if (onClicked != null)
                onClicked(this);
        }

        Tween enterScaleTweener;
        Tween exitScaleTweener;

        public void DoEnterAnimation(float delay)
        {
            if (enterScaleTweener != null)
            {
                enterScaleTweener.Kill();
            }

            var oldScale = transform.localScale;
            transform.localScale = Vector3.one * 0.001f;
            enterScaleTweener = DOTween.Sequence().Append(
                transform.DOScale(oldScale, 0.2f).SetDelay(delay)
            ).Append(
                transform.DOPunchRotation(Vector3.forward * 20, 0.3f, 10, 1)
            );
        }

        public bool IsReady()
        {
            return !enterScaleTweener.IsPlaying();
        }

        void ScaleTo(float scale, float duration, Action endCallback = null)
        {
            var endScaleCallback = endCallback;

            if (exitScaleTweener != null)
            {
                exitScaleTweener.Kill();
            }

            exitScaleTweener = transform.DOScale(scale, duration).OnComplete(delegate ()
            {
                if (endScaleCallback != null)
                    endScaleCallback();
            });
        }


        void Disappear()
        {
            ScaleTo(0.01f, 0.1f, () =>
            {
                if (onAnimationCompleted != null)
                    onAnimationCompleted();

                Destroy(gameObject);
            });
        }

        void Update()
        {
            if (isDestroying)
            {
                if (destroyTimer >= 0)
                {
                    destroyTimer -= Time.deltaTime;

                    if (destroyTimer < 0)
                    {
                        Disappear();
                    }
                }
            }
        }
    }
}
