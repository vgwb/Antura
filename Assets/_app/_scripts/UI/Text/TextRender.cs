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

        public bool isTMPro = true;
        public bool isUI;
        public bool isEnglishSubtitle;

        private LanguageUse languageUse;
        public Database.LocalizationDataId LocalizationId;

        public string text
        {
            get { return m_text; }
            set
            {
                if (m_text == value) return;
                if (SAppConfig.I.ForceALLCAPSTextRendering) {
                    m_text = value.ToUpper();
                } else {
                    m_text = value;
                }
                updateText();
            }
        }

        public float Alpha
        {
            get {
                if (isTMPro) {
                    if (isUI) {
                        return gameObject.GetComponent<TextMeshProUGUI>().alpha;
                    } else {
                        return gameObject.GetComponent<TextMeshPro>().alpha;
                    }
                }
                return 0;
            }
            set {
                if (isTMPro) {
                    if (isUI) {
                        gameObject.GetComponent<TextMeshProUGUI>().alpha = value;
                    } else {
                        gameObject.GetComponent<TextMeshPro>().alpha = value;
                    }
                }
            }
        }

        public string RenderedText
        {
            get {
                if (isTMPro) {
                    if (isUI) {
                        return gameObject.GetComponent<TextMeshProUGUI>().text;
                    } else {
                        return gameObject.GetComponent<TextMeshPro>().text;
                    }
                } else {
                    if (isUI) {
                        return gameObject.GetComponent<Text>().text;
                    } else {
                        return gameObject.GetComponent<TextMesh>().text;
                    }
                }
            }
        }

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
            if (isTMPro) {
                if (isUI) {
                    gameObject.GetComponent<TextMeshProUGUI>().text = text;
                } else {
                    gameObject.GetComponent<TextMeshPro>().text = text;
                }
            } else {
                if (isUI) {
                    gameObject.GetComponent<Text>().text = text;
                } else {
                    gameObject.GetComponent<TextMesh>().text = text;
                }
            }
            CheckRTL();
        }

        private void updateText()
        {
            if (isTMPro) {
                if (isUI) {
                    gameObject.GetComponent<TextMeshProUGUI>().text = LanguageSwitcher.I.GetHelper(languageUse).ProcessString(m_text);
                } else {
                    gameObject.GetComponent<TextMeshPro>().text = LanguageSwitcher.I.GetHelper(languageUse).ProcessString(m_text);
                }
            } else {
                if (isUI) {
                    gameObject.GetComponent<Text>().text = m_text;
                } else {
                    gameObject.GetComponent<TextMesh>().text = m_text;
                }
            }

            if (RenderedText == "") text = " "; // Avoid no text not getting update correctly
            CheckRTL();
        }

        public void SetColor(Color color)
        {
            if (isTMPro) {
                if (isUI) {
                    gameObject.GetComponent<TextMeshProUGUI>().color = color;
                } else {
                    gameObject.GetComponent<TextMeshPro>().color = color;
                }
            }
        }

        void CheckRTL()
        {
            bool isRTL = LanguageSwitcher.I.GetLangConfig(languageUse).IsRightToLeft();
            if (isUI)
            {
                gameObject.GetComponent<TextMeshProUGUI>().isRightToLeftText = isRTL;
            }
            else
            {
                gameObject.GetComponent<TextMeshPro>().isRightToLeftText = isRTL;
            }
        }

        public void SetLetterData(ILivingLetterData livingLetterData)
        {
            languageUse =LanguageUse.Learning;
            CheckRTL();

            if (livingLetterData.DataType == LivingLetterDataType.Letter) {
                text = livingLetterData.TextForLivingLetter;
            } else if (livingLetterData.DataType == LivingLetterDataType.Word) {
                text = livingLetterData.TextForLivingLetter;
            }
        }

        public void SetSentence(Database.LocalizationDataId sentenceId)
        {
            text = LocalizationManager.GetTranslation(sentenceId);
        }
    }
}