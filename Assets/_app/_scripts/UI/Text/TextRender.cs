using Antura.Core;
using Antura.Language;
using Antura.LivingLetters;
using UnityEngine;
using UnityEngine.UI;
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

        public bool isEnglishSubtitle;

        private LanguageUse languageUse;
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
            set
            {
                if (m_text == value) return;
                m_text = SAppConfig.I.ForceALLCAPSTextRendering ? value.ToUpper() : value;
                updateText();
            }
        }

        public float Alpha
        {
            get => TMPText.alpha;
            set => TMPText.alpha = value;
        }

        public string RenderedText => TMPText.text;
        private TMP_Text TMPText => gameObject.GetComponent<TMP_Text>();

        void Awake()
        {
            if (isEnglishSubtitle) {
                gameObject.SetActive(AppManager.I.AppSettings.EnglishSubtitles);
            }

            if (LocalizationId != Database.LocalizationDataId.None) {
                SetSentence(LocalizationId);
            }
            updateText();
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

        private void updateText()
        {
            TMPText.text = LanguageSwitcher.I.GetHelper(languageUse).ProcessString(m_text);

            if (RenderedText == "") text = " "; // Avoid no text not getting update correctly
            CheckRTL();
        }

        public void SetColor(Color color)
        {
            TMPText.color = color;
        }

        void CheckRTL()
        {
            TMPText.isRightToLeftText = LanguageSwitcher.I.GetLangConfig(languageUse).IsRightToLeft();
        }

        public int drawingFontSize = 40;
        public void SetLetterData(ILivingLetterData livingLetterData)
        {
            if (livingLetterData.DataType == LivingLetterDataType.Image)
            {
                TMPText.enableAutoSizing = false;
                TMPText.fontSize = drawingFontSize;
                text = livingLetterData.DrawingCharForLivingLetter;
                TMPText.font = Resources.Load<TMP_FontAsset>("EA4S_WordDrawings SDF");
                color = Color.black; 
            }
            else
            {
                TMPText.enableAutoSizing = true;
                languageUse = LanguageUse.Learning;
                TMPText.font = LanguageSwitcher.I.GetLangConfig(languageUse).Font;
                CheckRTL();
                text = livingLetterData.TextForLivingLetter;
                color = Color.black;
            }
        }

        public void SetSentence(Database.LocalizationDataId sentenceId)
        {
            text = LocalizationManager.GetTranslation(sentenceId);
        }
    }
}