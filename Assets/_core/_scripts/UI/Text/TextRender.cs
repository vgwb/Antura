using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using Antura.Language;
using Antura.LivingLetters;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/* this class is used as interface to text objects, to manage any type of renderer (UI text or TextMeshPro), LTR or RTL,
 * so we just need to reference the TextRender as controller
 */

namespace Antura.UI
{
    public class TextRender : MonoBehaviour
    {
        [SerializeField]
        protected string m_text;
        public bool isNumber;

        [FormerlySerializedAs("isEnglishSubtitle")]
        public bool isSubtitle;

        public LanguageUse languageUse;
        private Font2Use fontUse = Font2Use.Default;
        public Database.LocalizationDataId LocalizationId;

        private TextRender drawingLabelText;

        public Mesh mesh
        {
            get => TMPText.mesh;
        }

        public Material fontSharedMaterial
        {
            get => TMPText.fontSharedMaterial;
            set => TMPText.fontSharedMaterial = value;
        }

        public Color color
        {
            get => TMPText.color;
            set => TMPText.color = value;
        }

        public string text
        {
            get => m_text;
            set
            {
                if (m_text == value)
                    return;
                //m_text = SAppConfig.I.ForceALLCAPSTextRendering ? value.ToUpper() : value;
                m_text = value;
                UpdateText();
            }
        }

        public float Alpha
        {
            get => TMPText.alpha;
            set => TMPText.alpha = value;
        }

        public string RenderedText => TMPText.text;
        public TMP_Text TMPText => gameObject.GetComponent<TMP_Text>();

        void Awake()
        {
            var drawingLabelGO = transform.Find("drawingLabel");
            if (drawingLabelGO != null)
            {
                drawingLabelText = drawingLabelGO.GetComponent<TextRender>();
            }

            if (isSubtitle)
            {
                gameObject.SetActive(AppManager.I.ContentEdition.LearnMethod.ShowHelpText);
            }

            if (LocalizationId != Database.LocalizationDataId.None)
            {
                SetSentence(LocalizationId);
            }
            UpdateText();
        }

        public void SetOverridenLanguageText(LanguageCode code, LocalizationDataId locId)
        {
            OverridenLanguageCode = code;
            var loc = LocalizationManager.GetLocalizationData(locId);
            SetText(loc.GetLocalized(code).Text);
        }

        /// <summary>
        /// the main method to set the text of this field. doesn't change any setting
        /// </summary>
        /// <param name="_text">Text.</param>
        public void SetText(string _text, Font2Use _fontUse = Font2Use.Default)
        {
            fontUse = _fontUse;
            text = _text;
            CheckRTL();
        }

        public void SetText(string _text, LanguageUse _languageUse, Font2Use _fontUse = Font2Use.Default)
        {
            languageUse = _languageUse;
            fontUse = _fontUse;
            text = _text;
            CheckRTL();
        }

        public void SetTextUnfiltered(string _text, Font2Use _fontUse = Font2Use.Default, bool forceFont = false)
        {
            fontUse = _fontUse;
            TMPText.text = _text;
            m_text = _text;
            if (forceFont)
            {
                UpdateFont();
            }
            CheckRTL();
        }

        [HideInInspector]
        public LanguageCode OverridenLanguageCode = LanguageCode.NONE;

        private void UpdateFont()
        {
            var config = LanguageSwitcher.I.GetLangConfig(languageUse);
            if (OverridenLanguageCode != LanguageCode.NONE)
            {
                config = LanguageSwitcher.I.GetLangConfig(OverridenLanguageCode);
            }
            switch (fontUse)
            {
                case Font2Use.UI:
                    TMPText.font = config.UIFont;
                    break;
                case Font2Use.Learning:
                    TMPText.font = config.LanguageFont;
                    break;
                case Font2Use.Default:
                default:
                    TMPText.font = config.UIFont;
                    break;
            }
        }

        private void UpdateText()
        {
            if (LanguageSwitcher.I == null || !AppManager.I.Loaded)
                return;

            if (!isLetter && !isNumber)
            {
                UpdateFont();
            }

            if (!isNumber && !isLetter)
            {
                if (OverridenLanguageCode != LanguageCode.NONE)
                {
                    TMPText.text = LanguageSwitcher.I.GetHelper(OverridenLanguageCode).ProcessString(m_text);
                }
                else
                {
                    TMPText.text = LanguageSwitcher.I.GetHelper(languageUse).ProcessString(m_text);
                }
            }
            else if (isLetter)
            {
                // Processed already
                TMPText.text = m_text;
            }
            else
            {
                // Raw
                TMPText.text = m_text;
            }

            if (RenderedText == "")
                text = " "; // Avoid no text not getting update correctly
            CheckRTL();
        }

        public void SetColor(Color color)
        {
            TMPText.color = color;
        }

        void CheckRTL()
        {
            var config = LanguageSwitcher.I.GetLangConfig(languageUse);
            if (OverridenLanguageCode != LanguageCode.NONE)
                config = LanguageSwitcher.I.GetLangConfig(OverridenLanguageCode);
            TMPText.isRightToLeftText = !isNumber && config.IsRightToLeft();
        }

        public void SetTextAlign(bool alignRight)
        {
            if (alignRight)
            {
                TMPText.horizontalAlignment = HorizontalAlignmentOptions.Right;
            }
            else
            {
                TMPText.horizontalAlignment = HorizontalAlignmentOptions.Left;
            }
        }

        private bool isLetter = false;
        public int drawingFontSize = 40;
        public void SetLetterData(ILivingLetterData livingLetterData, bool outlined = false)
        {
            isLetter = true;
            if (livingLetterData.DataType == LivingLetterDataType.Image)
            {
                TMPText.enableAutoSizing = false;
                TMPText.fontSize = drawingFontSize;
                text = livingLetterData.DrawingCharForLivingLetter;

                LL_ImageData imageData = (LL_ImageData)livingLetterData;
                color = imageData.Data.Category == Database.WordDataCategory.Colors ? GenericHelper.GetColorFromString(imageData.Data.GetDrawingColor()) : Color.black;

                TMPText.font = LanguageSwitcher.I.GetLangConfig(languageUse).DrawingsFont;
                if (outlined)
                {
                    TMPText.fontSharedMaterial = LanguageSwitcher.LearningConfig.OutlineDrawingFontMaterial;
                }

                if (drawingLabelText != null)
                {
                    if (imageData.Data.DrawingLabel != "")
                    {
                        drawingLabelText.gameObject.SetActive(true);
                        drawingLabelText.SetText(imageData.Data.DrawingLabel, Font2Use.Learning);
                    }
                    else
                    {
                        drawingLabelText.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                TMPText.enableAutoSizing = true;
                languageUse = LanguageUse.Learning;
                CheckRTL();
                color = Color.black;

                text = livingLetterData.TextForLivingLetter;

                TMPText.font = LanguageSwitcher.I.GetLangConfig(languageUse).LanguageFont;
                if (outlined)
                    TMPText.fontSharedMaterial = LanguageSwitcher.LearningConfig.OutlineLanguageFontMaterial;
            }
        }

        public void SetSentence(Database.LocalizationDataId sentenceId)
        {
            text = LocalizationManager.GetLocalizationData(sentenceId).GetText(languageUse);
        }

        #region Flashing

        private const float FLASHING_TEXT_CYCLE_DURATION = 1f;
        private IEnumerator flashingTextCoroutine;
        public void SetFlashingText(WordData word, LL_LetterData letterToFlash, bool markPrecedingLetters, int sequentialIndex = 0)
        {
            var letterPartToFlash = LanguageSwitcher.LearningHelper.FindLetter(AppManager.I.DB, word, letterToFlash.Data, LetterEqualityStrictness.Letter)[sequentialIndex];

            int toCharIndex = letterPartToFlash.toCharacterIndex;
            if (letterPartToFlash.fromCharacterIndex != letterPartToFlash.toCharacterIndex)
            {
                var hexCode = LanguageSwitcher.LearningHelper.GetHexUnicodeFromChar(word.Text[letterPartToFlash.toCharacterIndex]);
                if (hexCode == "0651")   // Shaddah
                {
                    toCharIndex -= 1;
                }
            }

            if (flashingTextCoroutine != null)
            {
                StopCoroutine(flashingTextCoroutine);
            }
            flashingTextCoroutine = LanguageSwitcher.LearningHelper.GetWordWithFlashingText(word, letterPartToFlash.fromCharacterIndex, toCharIndex, Color.green, FLASHING_TEXT_CYCLE_DURATION, int.MaxValue,
                s => { text = s; }, markPrecedingLetters);
            StartCoroutine(flashingTextCoroutine);
        }

        public void StopFlashing()
        {
            if (flashingTextCoroutine != null)
            {
                StopCoroutine(flashingTextCoroutine);
            }
            flashingTextCoroutine = null;
        }

        public void HideDrawingLabel()
        {
            if (drawingLabelText != null)
            {
                drawingLabelText.gameObject.SetActive(false);
            }
        }

        #endregion

    }
}
