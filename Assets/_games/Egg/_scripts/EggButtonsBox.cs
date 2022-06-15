using System;
using System.Collections;
using System.Collections.Generic;
using Antura.Language;
using Antura.LivingLetters;
using Antura.Minigames;
using UnityEngine;

namespace Antura.Minigames.Egg
{
    public class EggButtonsBox : MonoBehaviour
    {
        public Transform anturaOut;
        public Transform anturaIn;

        public AnimationCurve normalAnimationCurve;
        public AnimationCurve anturaInAnimationCurve;

        public Color[] buttonColors;
        List<Color> availableButtonColors = new List<Color>();

        GameObject eggButtonPrefab;

        List<EggButton> eggButtons = new List<EggButton>();

        float buttonDistance = 20f;
        bool useEnlargeAnimation;

        int buttonCount;

        IAudioManager audioManager;

        System.Random randomGenerator;

        public void Initialize(GameObject eggButtonPrefab, IAudioManager audioManager, float buttonDistance, bool useEnlargeAnimation)
        {
            this.eggButtonPrefab = eggButtonPrefab;
            this.audioManager = audioManager;
            this.buttonDistance = buttonDistance;
            this.useEnlargeAnimation = useEnlargeAnimation;

            randomGenerator = new System.Random((int)Time.realtimeSinceStartup);
        }

        public void AddButton(ILivingLetterData letterData)
        {
            EggButton eggButton = CreateButton();
            eggButton.useEnlargeAnimation = useEnlargeAnimation;

            eggButton.SetAnswer(letterData);

            eggButtons.Add(eggButton);
        }

        public bool RemoveButton(ILivingLetterData letterData)
        {
            bool removed = false;
            for (int i = 0; i < eggButtons.Count; i++)
            {
                EggButton eggButton = eggButtons[i];

                if (eggButton.livingLetterData == letterData)
                {
                    eggButton.ScaleTo(0f, 0.3f, 0f, delegate ()
                    { Destroy(eggButton.gameObject); });

                    eggButtons.RemoveAt(i--);
                    removed = true;
                }
            }
            return removed;
        }

        public void RemoveButtons(Predicate<ILivingLetterData> predicate)
        {
            for (int i = 0; i < eggButtons.Count; i++)
            {
                EggButton eggButton = eggButtons[i];

                if (predicate(eggButton.livingLetterData))
                {
                    if (RemoveButton(eggButton.livingLetterData))
                    {
                        --i;
                    }
                }
            }
        }

        public void RemoveButtons()
        {
            for (int i = 0; i < eggButtons.Count; i++)
            {
                EggButton eggButton = eggButtons[i];

                eggButton.ScaleTo(0f, 0.3f, 0f, delegate ()
                { Destroy(eggButton.gameObject); });
            }

            eggButtons.Clear();
        }

        EggButton CreateButton()
        {
            EggButton eggButton = Instantiate(eggButtonPrefab).GetComponent<EggButton>();
            eggButton.transform.SetParent(transform, false);
            eggButton.gameObject.SetActive(false);
            eggButton.Initialize(audioManager);
            eggButton.colorLightUp = GetButtonColor();
            eggButton.DisableInput();
            return eggButton;
        }

        public void ShowButtons()
        {
            for (int i = 0; i < eggButtons.Count; i++)
            {
                eggButtons[i].gameObject.SetActive(true);
            }
        }

        public void HideButtons()
        {
            for (int i = 0; i < eggButtons.Count; i++)
            {
                eggButtons[i].gameObject.SetActive(false);
            }
        }

        public void SetButtonsOnPressedColor()
        {
            for (int i = 0; i < eggButtons.Count; i++)
            {
                eggButtons[i].SetPressed();
            }
        }

        public void SetButtonsOnStandardColor(EggButton without = null, bool killTween = true)
        {
            for (int i = 0; i < eggButtons.Count; i++)
            {
                if (eggButtons[i] != without)
                {
                    eggButtons[i].SetNormal(killTween);
                }
            }
        }

        public void SetButtonsOnPosition()
        {
            buttonCount = eggButtons.Count;

            Vector3[] buttonsPosition = CalculateButtonPositions();

            List<int> buttonsIndex = new List<int>();

            for (int i = 0; i < buttonCount; i++)
            {
                buttonsIndex.Add(i);
            }

            for (int i = 0; i < buttonsPosition.Length; i++)
            {
                int index = randomGenerator.Next(0, buttonsIndex.Count);
                int currentIndex = buttonsIndex[index];
                buttonsIndex.RemoveAt(index);

                eggButtons[currentIndex].transform.localPosition = buttonsPosition[i];
                eggButtons[currentIndex].positionIndex = i;
            }
        }

        public void SetOnPressedCallback(Action<ILivingLetterData> callback)
        {
            for (int i = 0; i < eggButtons.Count; i++)
            {
                eggButtons[i].SetOnPressedCallback(callback);
            }
        }

        public void AnturaButtonOut(float duration, float delay, Action endCallback = null)
        {
            List<EggButton> buttons = GetButtons(true);

            float shakeTime = duration * 0.3f;
            float moveTime = duration - shakeTime;
            float delayBetweenButton = 0.1f;
            Action callback;

            for (int i = 0; i < buttons.Count; i++)
            {
                callback = null;

                if (i == buttons.Count - 1)
                {
                    callback = endCallback;
                }

                buttons[i].ShakePosition(shakeTime + (i * shakeTime), delay);
                buttons[i].MoveTo(anturaOut.localPosition, moveTime, normalAnimationCurve, (i * delayBetweenButton) + (delay + shakeTime), false, 0f, null, callback);
                buttons[i].ScaleTo(0f, moveTime, (i * delayBetweenButton) + (delay + shakeTime));
            }
        }

        public void AnturaButtonIn(float duration, float delay, float delayBetweenButton = 0.5f, float anturaSpitDelay = 0.3f, Action anturaSpit = null, Action endCallback = null)
        {
            buttonCount = eggButtons.Count;

            for (int i = 0; i < eggButtons.Count; i++)
            {
                eggButtons[i].transform.localPosition = anturaIn.localPosition;
            }

            Vector3[] buttonsPosition = CalculateButtonPositions();

            List<int> buttonsIndex = new List<int>();

            for (int i = 0; i < buttonCount; i++)
            {
                buttonsIndex.Add(i);
            }

            float fromStartDelay;
            bool jump;
            Action onStartCallBack;
            Action onEndCallback;

            for (int i = 0; i < buttonsPosition.Length; i++)
            {
                int index = randomGenerator.Next(0, buttonsIndex.Count);
                int currentIndex = buttonsIndex[index];
                buttonsIndex.RemoveAt(index);

                fromStartDelay = 0f;
                onStartCallBack = null;
                onEndCallback = null;
                jump = true;

                fromStartDelay = anturaSpitDelay;

                if (i == 0)
                {
                    fromStartDelay = anturaSpitDelay;
                    onStartCallBack = anturaSpit;
                    jump = false;
                }

                if (i == buttonsPosition.Length - 1)
                {
                    onEndCallback = endCallback;
                }


                eggButtons[currentIndex].MoveTo(buttonsPosition[i], duration, anturaInAnimationCurve, (i * delayBetweenButton) + delay, jump, fromStartDelay, onStartCallBack, onEndCallback);

                eggButtons[currentIndex].positionIndex = i;
                eggButtons[currentIndex].ScaleTo(1f, duration, (i * delayBetweenButton) + delay + fromStartDelay);
                eggButtons[currentIndex].transform.SetAsFirstSibling();
            }
        }

        Vector3[] CalculateButtonPositions()
        {
            Vector3[] buttonsPosition = new Vector3[buttonCount];

            Vector2 eggSizeDelta = ((RectTransform)eggButtons[0].transform).sizeDelta;

            Vector3 currentPosition = Vector3.zero;

            float positionUp = (eggSizeDelta.y + buttonDistance) / 2f;
            float positionDown = -positionUp;

            int upLineLength = 0;
            int downLineLength = 0;

            if (buttonCount <= 4)
            {
                upLineLength = buttonCount;
                downLineLength = 0;
            }
            else
            {
                upLineLength = (buttonCount == 5 || buttonCount == 6) ? 3 : 4;
                downLineLength = (buttonCount % 2) == 0 ? upLineLength : upLineLength - 1;
            }

            int lineIndex = 0;
            bool goDown = false;

            for (int i = 0; i < buttonCount; i++)
            {
                if (buttonCount <= 4)
                {
                    currentPosition.y = 0f;
                }
                else
                {
                    currentPosition.y = goDown ? positionDown : positionUp;
                }

                currentPosition.x = GetHorizontalPositions(eggSizeDelta.x, goDown ? downLineLength : upLineLength)[lineIndex];

                lineIndex++;
                if (lineIndex >= upLineLength)
                {
                    goDown = true;
                    lineIndex = 0;
                }

                buttonsPosition[i] = currentPosition;
            }

            return buttonsPosition;
        }

        float[] GetHorizontalPositions(float size, int number)
        {
            float[] horizontalPositions = new float[number];

            if (number == 1)
            {
                horizontalPositions[0] = 0f;
            }
            else
            {
                float currentHorizontal = (((size + buttonDistance) * (number - 1)) / 2f);

                for (int i = 0; i < number; i++)
                {
                    if (i != 0)
                    {
                        currentHorizontal -= size + buttonDistance;
                    }

                    horizontalPositions[i] = currentHorizontal;
                }
            }

            return horizontalPositions;
        }

        public List<EggButton> GetButtons(bool inPositionOrder)
        {
            if (inPositionOrder)
            {
                List<EggButton> buttons = new List<EggButton>();

                while (buttons.Count < eggButtons.Count)
                {
                    EggButton eB = null;

                    for (int i = 0; i < eggButtons.Count; i++)
                    {
                        if (eggButtons[i].positionIndex == buttons.Count)
                        {
                            eB = eggButtons[i];
                            break;
                        }
                    }

                    buttons.Add(eB);
                }

                return buttons;
            }
            else
            {
                return eggButtons;
            }
        }

        public EggButton GetEggButton(ILivingLetterData letterData)
        {
            for (int i = 0; i < eggButtons.Count; i++)
            {
                if (eggButtons[i].livingLetterData == letterData)
                {
                    return eggButtons[i];
                }
            }

            return null;
        }

        public void EnableButtonsInput()
        {
            for (int i = 0; i < eggButtons.Count; i++)
            {
                eggButtons[i].EnableInput();
            }
        }

        public void DisableButtonsInput()
        {
            for (int i = 0; i < eggButtons.Count; i++)
            {
                eggButtons[i].DisableInput();
            }
        }

        public IEnumerator PlayButtonsAudio(ILivingLetterData playBefore, ILivingLetterData playAfter, bool lightUp, bool inPositionOrder, float delay, Action endCallback, Action startCallback = null, bool yieldDuration = false, bool playButtonsNames = true)
        {
            List<EggButton> buttons = GetButtons(inPositionOrder);

            Action sCallback = null;
            Action eCallback = null;

            if (playBefore != null)
            {
                var sourceWrapper = audioManager.PlayVocabularyData(playBefore, soundType: EggConfiguration.Instance.GetVocabularySoundType());
                while (!sourceWrapper.IsLoaded)
                    yield return null;
                delay += 0.5f + sourceWrapper.Duration;
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                int iRTL = LanguageSwitcher.I.IsLearningLanguageRTL() ? i : buttons.Count - 1 - i;
                if (EggConfiguration.Instance.IsSequence())
                    iRTL = i;

                if (iRTL == buttons.Count - 1)
                {
                    eCallback = () =>
                    {
                        if (playAfter != null)
                            audioManager.PlayVocabularyData(playAfter, soundType: EggConfiguration.Instance.GetVocabularySoundType());

                        if (endCallback != null)
                            endCallback();

                    };
                }

                if (iRTL == 0)
                {
                    sCallback = startCallback;
                }

                var delayRef = new Ref<float>();
                if (playButtonsNames)
                {
                    yield return buttons[iRTL].PlayButtonAudio(lightUp, delay, eCallback, sCallback, delayRef);
                }
                else
                {
                    var s = audioManager.PlaySound(Sfx.LL_Jump);
                    yield return new WaitForSeconds(0.5f);
                    sCallback?.Invoke();
                    eCallback?.Invoke();
                }
                delay += delayRef.v;
            }
        }


        public void StopButtonsAudio()
        {
            for (int i = 0; i < eggButtons.Count; i++)
            {
                eggButtons[i].StopButtonAudio();
            }
        }

        Color GetButtonColor()
        {
            Color newColor;

            if (availableButtonColors.Count <= 0)
            {
                for (int i = 0; i < buttonColors.Length; i++)
                {
                    availableButtonColors.Add(buttonColors[i]);
                }
            }

            newColor = availableButtonColors[0];

            availableButtonColors.RemoveAt(0);

            return newColor;
        }
    }

    public class Ref<T>
    {
        public T v;
    }
}
