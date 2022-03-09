using Antura.Core;
using Antura.Helpers;
using Antura.LivingLetters;
using Antura.Minigames;
using Antura.UI;
using Antura.Utilities;
using DG.Tweening;
using System;
using Antura.Database;
using TMPro;
using UnityEngine;
using Antura.Language;


namespace Antura.Assessment
{
    /// <summary>
    /// Alternative LL, no animation, more lightweight,
    /// and with some Assessments specific functionalities
    /// </summary>
    public class StillLetterBox : MonoBehaviour
    {
        /// <summary>
        /// Injected properties
        /// </summary>
        public TMP_Text Label;
        public TextRender LabelRender;
        public TextMeshPro Drawing;
        public ParticleSystem poofPrefab;

        public NineSlicedSprite questionSprite;
        public NineSlicedSprite hiddenQuestionSprite;
        public NineSlicedSprite answerSprite;
        public SpriteRenderer MegaphoneIcon;

        Vector2 startTextScale;

        public RectTransform textTransform;
        public RectTransform drawingTransform;
        public Transform backgroundTransform;

        ///################# ANIMATIONS #################

        private float megaphoneScale = 1.587492f;

        // Local Tween
        Tween tween = null;

        /// <summary>
        /// Flip the LetterBox updside down to reveal the letter
        /// </summary>
        internal void RevealHiddenQuestion()
        {
            KillTween();
            SetQuestionGreen();
        }

        private Color32 SpecialGreen = new Color32(45, 246, 38, 255);

        public void SetQuestionGreen()
        {
            answerSprite.enabled = true;
            answerSprite.Material.DOColor(SpecialGreen, 0.5f);
            hiddenQuestionSprite.Material.DOFade(0, 1);
            Label.alpha = 0;
            Label.DOFade(1, 0.6f);
            MegaphoneIcon.DOFade(0, 0.3f);
        }

        public void SetGreenLetter(ILivingLetterData word, ILivingLetterData letter)
        {
            var wordInner = word as LL_WordData;
            var letterInner = letter as LL_LetterData;

            var parts = LanguageSwitcher.LearningHelper.FindLetter(AppManager.I.DB, wordInner.Data, letterInner.Data, LetterEqualityStrictness.Letter);

            var partToRemove = parts[0];

            // .. and voil�! Thank you Davide! :)
            LabelRender.text = LanguageSwitcher.LearningHelper.GetWordWithMarkedLetterText(
                wordInner.Data, partToRemove, SpecialGreen, MarkType.SingleLetter
            );
        }

        /// <summary>
        /// Hides the letter
        /// </summary>
        internal void HideHiddenQuestion()
        {
            Label.alpha = 0;
            MegaphoneIcon.DOFade(1, 0);
            MegaphoneIcon.enabled = true;
            questionSprite.enabled = false;
            hiddenQuestionSprite.enabled = true;
            hiddenQuestionSprite.Material.color = new Color(1, 1, 1, 1);
            InstaShrink();
        }

        /// <summary>
        /// Magnify animation.
        /// </summary>
        internal void Magnify()
        {
            TweenScale(1);
        }

        internal void InstaShrink()
        {
            Scale = 0;
        }

        internal void Grabbed()
        {
            Scale = 1.1f;
        }

        internal void Dropped()
        {
            Scale = 1;
        }

        internal void TweenScale(float newScale)
        {
            KillTween();

            tween = DOTween.To(() => Scale, x => Scale = x, newScale, 0.4f);
        }

        private void KillTween()
        {
            if (tween != null)
            {
                tween.Kill(true);
            }
            tween = null;
        }


        ///############### IMPLEMENTATION ################

        public NineSlicedSprite slotSprite;

        /// <summary>
        /// Gets the data.
        /// </summary>
        ILivingLetterData data = null; // NOT SET ALWAYS. DEBUGGIN
        bool nullOnDemand = false;
        public ILivingLetterData Data
        {
            get
            {
                if (data == null && !nullOnDemand)
                {
                    throw new ArgumentNullException("Null on demand: " + nullOnDemand);
                }
                return data;
            }
            private set
            {
                data = value;

                OnModelChanged();

                if (data != null)
                {
                    if (data.Id == "with_article" || data.Id == "without_article")
                    {
                        Wideness = 2.3f;
                    }
                }
            }
        }

        private float Wideness;

        private float Scale
        {
            get
            {
                return textTransform.sizeDelta.x / (startTextScale.x * Wideness);
            }
            set
            {
                float widthScale = value * Wideness;

                foreach (NineSlicedSprite sprite in backgroundTransform.GetComponentsInChildren<NineSlicedSprite>(true))
                {
                    sprite.Width = sprite.initialWidth * widthScale;
                    sprite.Height = sprite.initialHeight * value;
                }

                //transform the parent too because border sprites have offset even with 0 scale
                transform.localScale = new Vector3(value, value, value);

                //Allow space for diacritics.
                textTransform.sizeDelta = new Vector2(startTextScale.x * widthScale, startTextScale.y * value);


                drawingTransform.sizeDelta = new Vector2(startTextScale.x * widthScale, startTextScale.y * value);
                MegaphoneIcon.transform.localScale =
                    new Vector3(megaphoneScale * value, megaphoneScale * value, 1);

                if (extendedBoxCollider == false)
                {
                    GetComponent<BoxCollider>().size = textTransform.sizeDelta;
                }
            }
        }

        /// <summary>
        /// Called when [model changed].
        /// </summary>
        private void OnModelChanged()
        {
            DisableSlots();
            if (data == null)
            {
                Wideness = 1.0f;
                Drawing.enabled = false;
                Label.enabled = false;
            }
            else
            {
                Drawing.enabled = false;
                Label.enabled = true;
                LabelRender.SetLetterData(Data);
                SetWidness(data.DataType);
            }
        }

        private Tween colorTween = null;
        private bool stoppedColor = true;

        public void NearbySlot()
        {
            if (stoppedColor)
            {
                StopColorTween();
                colorTween = slotSprite.Material.DOColor(new Color32(180, 180, 180, 255), 0.3f);
                stoppedColor = false;
            }
        }

        private void StopColorTween()
        {
            if (colorTween != null)
            {
                colorTween.Kill(false);
                colorTween = null;
            }
        }

        public void FarSlot()
        {
            if (stoppedColor == false)
            {
                StopColorTween();
                colorTween = slotSprite.Material.DOColor(new Color32(255, 255, 255, 255), 0.3f);
                stoppedColor = true;
            }
        }

        private void SetWidness(LivingLetterDataType dataType)
        {
            Wideness = ElementsSize.Get(dataType);
        }

        private void DisableSlots()
        {
            questionSprite.enabled = false;
            answerSprite.enabled = false;
            slotSprite.enabled = false;
            hiddenQuestionSprite.enabled = false;
            MegaphoneIcon.enabled = false;
        }

        /// <summary>
        /// Return half width of LL, usefull for determining layout offsets
        /// </summary>
        public float GetHalfWidth()
        {
            return Wideness * 3.0f / 2.0f;
        }

        /// <summary>
        /// Return half height of LL, usefull for determining layout offsets
        /// </summary>
        public float GetHalfHeight()
        {
            return 3.0f / 2.0f;
        }

        /// <summary>
        /// Initializes  object with the specified data.
        /// </summary>
        /// <param name="_data">The data.</param>
        public void Init(ILivingLetterData _data, bool answer)
        {
            if (_data == null)
            {
                throw new ArgumentNullException("Cannot init with null data");
            }
            nullOnDemand = false;

            Data = _data;
            answerSprite.enabled = answer;
            questionSprite.enabled = !answer;
        }

        /// <summary>
        /// Initializes  object with the specified data.
        /// </summary>
        /// <param name="_data">The data.</param>
        public void InitAsSlot(LivingLetterDataType dataType)
        {
            nullOnDemand = true;
            Data = null;
            SetWidness(dataType);
            slotSprite.enabled = true;
        }

        public void Poof()
        {
            var rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            var puffGo = Instantiate(poofPrefab, transform.position, rotation) as ParticleSystem;

            puffGo.gameObject.AddComponent<AutoDestroy>().duration = 2;
            puffGo.gameObject.SetActive(true);
        }

        void Awake()
        {
            startTextScale = textTransform.sizeDelta;
        }

        bool extendedBoxCollider = false;

        internal void SetExtendedBoxCollider()
        {
            extendedBoxCollider = true;
            GetComponent<BoxCollider>().size = new Vector3(6.8f, 2.6f, 1);
            GetComponent<BoxCollider>().center = new Vector3(1.6f, 0, 0);
        }
    }
}
