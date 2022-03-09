using Antura.LivingLetters;
using Antura.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Antura.Minigames.ThrowBalls
{
    public class UIController : MonoBehaviour
    {
        public static UIController instance;

        private const float CRACK_FADE_DELAY = 0.5f;
        private const float CRACK_FADE_DURATION = 1.5f;

        public GameObject letterHint;
        public WordFlexibleContainer wordFlexibleContainer;
        private ILivingLetterData livingLetterData;

        public GameObject crack;

        private Image crackImage;
        private Color crackImageColor;

        void Awake()
        {
            instance = this;

            crackImage = crack.GetComponent<Image>();
            crackImageColor = crackImage.color;
        }

        public void SetLivingLetterData(ILivingLetterData _data)
        {
            livingLetterData = _data;
            wordFlexibleContainer.SetLetterData(_data);
        }

        public void SetText(string text)
        {
            wordFlexibleContainer.Label.SetText(text);
        }

        public TextRender LabelRender => wordFlexibleContainer.Label;

        public void WobbleLetterHint()
        {
            wordFlexibleContainer.gameObject.transform.DOKill(true);
            wordFlexibleContainer.gameObject.transform.DOShakeScale(0.5f);
        }

        public void OnLetterHintClicked()
        {
            if (livingLetterData != null && ThrowBallsGame.instance.GameState.isRoundOngoing)
            {
                ThrowBallsConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(
                    livingLetterData, true, soundType: ThrowBallsConfiguration.Instance.GetVocabularySoundType()
                );
                WobbleLetterHint();
            }
        }

        public void EnableLetterHint()
        {
            wordFlexibleContainer.gameObject.SetActive(true);
        }

        public void DisableLetterHint()
        {
            wordFlexibleContainer.gameObject.SetActive(false);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void OnScreenCracked()
        {
            Camera.main.transform.DOShakePosition(CRACK_FADE_DELAY);
            StartCoroutine(CrackAnimationCoroutine());
        }

        private IEnumerator CrackAnimationCoroutine()
        {
            ThrowBallsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.ScreenHit);

            crackImageColor.a = 1;
            crackImage.color = crackImageColor;

            yield return new WaitForSeconds(CRACK_FADE_DELAY);

            float crackFadeStartTime = Time.time;
            float sinFactor = 2 * Mathf.PI * Mathf.Pow(CRACK_FADE_DURATION, -1);

            while (crackImageColor.a > 0)
            {
                crackImageColor.a = Mathf.Cos(sinFactor * (Time.time - crackFadeStartTime));
                crackImage.color = crackImageColor;

                yield return new WaitForFixedUpdate();
            }

            crackImageColor.a = 0;
            crackImage.color = crackImageColor;
        }

        public void Reset()
        {
            crackImageColor.a = 0;
            crackImage.color = crackImageColor;
        }
    }
}
