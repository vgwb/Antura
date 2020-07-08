using Antura.Core;
using Antura.Helpers;
using Antura.Language;
using Antura.LivingLetters;
using UnityEngine;
using TMPro;

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
        public bool isEnglishSubtitle;

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
            if (isEnglishSubtitle) {
                gameObject.SetActive(AppManager.I.AppSettings.EnglishSubtitles);
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
            CheckRTL();
        }

        private void UpdateText()
        {
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
            if (languageUse == LanguageUse.Native) {
                text = LocalizationManager.GetNative(sentenceId);
            } else {
                text = LocalizationManager.GetTranslation(sentenceId);
            }
        }

    }
}