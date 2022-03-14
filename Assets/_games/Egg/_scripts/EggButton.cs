using Antura.LivingLetters;
using Antura.UI;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Antura.Minigames.Egg
{
    public class EggButton : MonoBehaviour
    {
        public bool useEnlargeAnimation = true;

        public TextRender buttonText;
        public Image buttonImage;
        public Button button;

        public Color colorStandard;
        public Color colorLightUp;
        float sizeStandard = 1;
        float sizeLightUp = 1.5f;

        Tween animationTweener;

        public ILivingLetterData livingLetterData { get; private set; }

        Action<ILivingLetterData> onButtonPressed;

        public int positionIndex { get; set; }

        Action startButtonAudioCallback;
        Action playButtonAudioCallback;
        IAudioManager audioManager;
        IAudioSource sourceWrapper;

        bool inputEnabled = false;

        Action endMoveCallback;
        Action startMoveCallback;
        Action endScaleCallback;
        Action endShakeCallback;

        Tween scaleTweener;
        Sequence moveSequence;

        Tween shakeTwenner;

        bool holdPressed;

        public void Initialize(IAudioManager audioManager)
        {
            button.onClick.AddListener(OnButtonPressed);

            this.audioManager = audioManager;
        }

        public void SetAnswer(ILivingLetterData livingLetterData)
        {
            this.livingLetterData = livingLetterData;

            buttonText.gameObject.SetActive(true);

            buttonText.SetLetterData(livingLetterData);
        }

        public IEnumerator PlayButtonAudio(bool lightUp, float delay = 0f, Action callback = null, Action startCallback = null, Ref<float> outDelay = null)
        {
            startButtonAudioCallback = startCallback;
            playButtonAudioCallback = callback;

            sourceWrapper = audioManager.PlayVocabularyData(livingLetterData, false, EggConfiguration.Instance.GetVocabularySoundType());
            if (sourceWrapper != null)
            {
                while (!sourceWrapper.IsLoaded)
                    yield return null;
                sourceWrapper.Stop();
            }

            float duration = sourceWrapper?.Duration ?? 0;

            if (animationTweener != null)
                animationTweener.Kill();

            Color newColor = lightUp ? colorLightUp : colorStandard;
            float newSize = lightUp ? sizeLightUp : sizeStandard;

            if (useEnlargeAnimation)
            {
                animationTweener = buttonImage.rectTransform.DOScale(Vector3.one * newSize, duration / 2f).OnComplete(delegate ()
                {
                    animationTweener = buttonImage.rectTransform.DOScale(Vector3.one * sizeStandard, duration / 2f).OnComplete(delegate ()
                    {
                        playButtonAudioCallback?.Invoke();
                    });
                }).OnStart(delegate ()
                {
                    sourceWrapper = audioManager.PlayVocabularyData(livingLetterData, false, EggConfiguration.Instance.GetVocabularySoundType());
                    startButtonAudioCallback?.Invoke();
                }).SetDelay(delay);
            }
            else
            {
                animationTweener = DOTween.To(() => buttonImage.color, x => buttonImage.color = x, newColor, duration / 2f).OnComplete(delegate ()
                {
                    animationTweener = DOTween.To(() => buttonImage.color, x => buttonImage.color = x, colorStandard, duration / 2f).OnComplete(delegate ()
                    {
                        playButtonAudioCallback?.Invoke();
                    });
                }).OnStart(delegate ()
                {
                    sourceWrapper = audioManager.PlayVocabularyData(livingLetterData, false, EggConfiguration.Instance.GetVocabularySoundType());
                    startButtonAudioCallback?.Invoke();

                }).SetDelay(delay);
            }

            if (outDelay != null)
                outDelay.v = duration;
        }

        public void StopButtonAudio()
        {
            SetNormal();

            if (sourceWrapper != null)
            {
                sourceWrapper.Stop();
            }

            playButtonAudioCallback?.Invoke();
        }

        public void SetOnPressedCallback(Action<ILivingLetterData> callback)
        {
            onButtonPressed = callback;
        }

        void OnButtonPressed()
        {
            if (inputEnabled)
            {
                if (!holdPressed)
                    ChangeColorOnButtonPressed();

                onButtonPressed?.Invoke(livingLetterData);
            }
        }

        void ChangeColorOnButtonPressed()
        {
            if (animationTweener != null)
                animationTweener.Kill();

            if (useEnlargeAnimation)
            {
                var currentEnlargeValue = buttonImage.rectTransform.localScale.x;

                float enlargeTarget = Mathf.Min(sizeLightUp * 1.25f, currentEnlargeValue * 1.2f);

                buttonImage.rectTransform.localScale = Vector3.one * enlargeTarget;
                animationTweener = buttonImage.rectTransform.DOScale(Vector3.one * sizeStandard, 0.75f);
            }
            else
            {
                buttonImage.color = colorLightUp;
                animationTweener = DOTween.To(() => buttonImage.color, x => buttonImage.color = x, colorStandard, 1f);
            }
        }

        public bool IsPressed()
        {
            return holdPressed;
        }

        public void SetPressed()
        {
            holdPressed = true;

            if (animationTweener != null)
            {
                animationTweener.Kill();
            }

            if (useEnlargeAnimation)
            {
                buttonImage.rectTransform.localScale = sizeLightUp * Vector3.one;
            }
            else
            {
                buttonImage.color = colorLightUp;
            }
        }

        public void SetNormal(bool killTween = true)
        {
            holdPressed = false;

            if (animationTweener != null && killTween)
            {
                animationTweener.Kill();
            }

            if (useEnlargeAnimation)
            {
                buttonImage.rectTransform.localScale = sizeStandard * Vector3.one;
            }
            else
            {
                buttonImage.color = colorStandard;
            }
        }

        public void EnableInput()
        {
            inputEnabled = true;
        }

        public void DisableInput()
        {
            inputEnabled = false;
        }

        public void ScaleTo(float scale, float duration, float delay = 0f, Action endCallback = null)
        {
            endScaleCallback = endCallback;

            if (scaleTweener != null)
            {
                scaleTweener.Kill();
            }

            scaleTweener = transform.DOScale(scale, duration).SetDelay(delay).OnComplete(delegate ()
            { if (endScaleCallback != null) endScaleCallback(); });
        }

        public void ShakePosition(float duration, float delay = 0f, Action endCallback = null)
        {
            endShakeCallback = endCallback;

            if (shakeTwenner != null)
            {
                shakeTwenner.Kill();
            }

            shakeTwenner = transform.DOShakePosition(duration, 4f, 20, 90f, false, false).SetDelay(delay).OnComplete(delegate ()
            { if (endShakeCallback != null) endShakeCallback(); });
        }

        public void MoveTo(Vector3 position, float duration, AnimationCurve animationCurve, float delay = 0f, bool doJump = false, float delayFromStart = 0f, Action startCallback = null, Action endCallback = null)
        {
            endMoveCallback = endCallback;
            startMoveCallback = startCallback;

            if (moveSequence != null)
            {
                moveSequence.Kill();
            }

            moveSequence = DOTween.Sequence();

            moveSequence.AppendInterval(delay);

            moveSequence.AppendCallback(delegate ()
            { if (startMoveCallback != null) startMoveCallback(); });
            moveSequence.AppendInterval(delayFromStart);

            if (doJump)
            {
                moveSequence.Append(transform.DOLocalJump(position, 100f, 1, duration));
            }
            else
            {
                moveSequence.Append(transform.DOLocalMove(position, duration).SetEase(animationCurve));
            }

            moveSequence.OnComplete(delegate ()
            { if (endMoveCallback != null) endMoveCallback(); });

            moveSequence.Play();
        }

        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }
    }
}
