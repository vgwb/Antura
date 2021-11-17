using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using Antura.Language;
using Antura.LivingLetters;
using UnityEngine;
using TMPro;
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
        public Database.LocalizationDataId LocalizationId;

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
            set {
                if (m_text == value) return;
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
            if (isSubtitle) {
                gameObject.SetActive(AppManager.I.AppEdition.ShowHelpText);
            }

            if (LocalizationId != Database.LocalizationDataId.None) {
                SetSentence(LocalizationId);
            }
            UpdateText();
        }

        /// <summary>
        /// the main method to set the text of this field. doesn't change any setting
        /// </summary>
        /// <param name="_text">Text.</param>
        public void SetText(string _text)
        {
            text = _text;
            CheckRTL();
        }

        public void SetText(string _text, LanguageUse _languageUse)
        {
            languageUse = _languageUse;
            text = _text;
            CheckRTL();
        }

        public void SetTextUnfiltered(string text)
        {
            TMPText.text = text;
            m_text = text;
            CheckRTL();
        }

        private void UpdateText()
        {
            if (LanguageSwitcher.I == null || !AppManager.I.Loaded) return;

            var config = LanguageSwitcher.I.GetLangConfig(languageUse);
            if (!isLetter && !isNumber && config.OverrideTextFonts) {
                TMPText.font = config.TextFont;
            }

            if (!isNumber && !isLetter) {
                TMPText.text = LanguageSwitcher.I.GetHelper(languageUse).ProcessString(m_text);
            } else if (isLetter) {
                // Processed already
                TMPText.text = m_text;
            } else {
                // Raw
                TMPText.text = m_text;
            }

            if (RenderedText == "") text = " "; // Avoid no text not getting update correctly
            CheckRTL();
        }

        public void SetColor(Color color)
        {
            TMPText.color = color;
        }

        void CheckRTL()
        {
            TMPText.isRightToLeftText = !isNumber && LanguageSwitcher.I.GetLangConfig(languageUse).IsRightToLeft();
        }

        public void SetTextAlign(bool alignRight)
        {
            if (alignRight) {
                TMPText.horizontalAlignment = HorizontalAlignmentOptions.Right;
            } else {
                TMPText.horizontalAlignment = HorizontalAlignmentOptions.Left;
            }
        }

        private bool isLetter = false;
        public int drawingFontSize = 40;
        public void SetLetterData(ILivingLetterData livingLetterData, bool outlined = false)
        {
            isLetter = true;
            if (livingLetterData.DataType == LivingLetterDataType.Image) {
                TMPText.enableAutoSizing = false;
                TMPText.fontSize = drawingFontSize;
                text = livingLetterData.DrawingCharForLivingLetter;

                LL_ImageData imageData = (LL_ImageData)livingLetterData;
                color = imageData.Data.Category == Database.WordDataCategory.Colors ? GenericHelper.GetColorFromString(imageData.Data.Value) : Color.black;

                TMPText.font = LanguageSwitcher.I.GetLangConfig(languageUse).DrawingsFont;
                if (outlined) TMPText.fontSharedMaterial = LanguageSwitcher.LearningConfig.OutlineDrawingFontMaterial;
            } else {
                TMPText.enableAutoSizing = true;
                languageUse = LanguageUse.Learning;
                CheckRTL();
                color = Color.black;

                text = livingLetterData.TextForLivingLetter;

                TMPText.font = LanguageSwitcher.I.GetLangConfig(languageUse).LetterFont;
                if (outlined) TMPText.fontSharedMaterial = LanguageSwitcher.LearningConfig.OutlineLetterFontMaterial;
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

            if (flashingTextCoroutine != null) StopCoroutine(flashingTextCoroutine);
            flashingTextCoroutine = LanguageSwitcher.LearningHelper.GetWordWithFlashingText(word, letterPartToFlash.fromCharacterIndex, toCharIndex, Color.green, FLASHING_TEXT_CYCLE_DURATION, int.MaxValue,
                s => { text = s; }, markPrecedingLetters);
            StartCoroutine(flashingTextCoroutine);
        }

        public void StopFlashing()
        {
            if (flashingTextCoroutine != null) StopCoroutine(flashingTextCoroutine);
            flashingTextCoroutine = null;
        }

        #endregion

    }
}