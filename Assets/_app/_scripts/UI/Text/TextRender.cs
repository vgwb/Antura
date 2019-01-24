using Antura.Core;
using Antura.Helpers;
using Antura.LivingLetters;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Antura.Language;

/* this class is used as interface to text objects, to manage any type of renderer (UI text or TextMeshPro), LTR or RTL,
 * so we just need to reference the TextRender as controller
 */

// TODO refactor: remove reference to Arabic
namespace Antura.UI
{
    public class TextRender : MonoBehaviour
    {
        public string text
        {
            get { return m_text; }
            set {
                if (m_text == value) return;
                m_text = value;
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

        [SerializeField]
        protected string m_text;

        public bool isTMPro = true;
        public bool isUI;
        public bool isArabic;
        public bool isEnglishSubtitle;

        public Database.LocalizationDataId LocalizationId;

        void Awake()
        {
            if (isEnglishSubtitle) {
                gameObject.SetActive(AppManager.I.AppSettings.EnglishSubtitles);
            }

            checkConfiguration();

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
        }

        /// <summary>
        /// here we can force the arabic setup of this text field (in case needs to be changed by code)
        /// </summary>
        /// <param name="_text">Text.</param>
        /// <param name="arabic">forces the arabic parsing ON/OFF</param>
        public void SetText(string _text, bool arabic)
        {
            isArabic = arabic;
            text = _text;
        }

        void checkConfiguration()
        {
            if (isTMPro && isUI && isArabic) {
                if (!gameObject.GetComponent<TextMeshProUGUI>().isRightToLeftText) {
                    Debug.LogWarning("TextMeshPro on component " + gameObject.name + " isn't RTL");
                }
            }
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
        }

        private void updateText()
        {
            if (isTMPro) {
                if (isArabic) {
                    if (isUI) {
                        gameObject.GetComponent<TextMeshProUGUI>().text = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessArabicString(m_text);
                    } else {
                        gameObject.GetComponent<TextMeshPro>().text = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessArabicString(m_text);
                    }
                } else {
                    if (isUI) {
                        gameObject.GetComponent<TextMeshProUGUI>().text = m_text;
                    } else {
                        gameObject.GetComponent<TextMeshPro>().text = m_text;
                    }
                }
            } else {
                if (isArabic) {
                    if (isUI) {
                        gameObject.GetComponent<Text>().text = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessArabicString(m_text);
                    } else {
                        gameObject.GetComponent<TextMesh>().text = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessArabicString(m_text);
                    }
                } else {
                    if (isUI) {
                        gameObject.GetComponent<Text>().text = m_text;
                    } else {
                        gameObject.GetComponent<TextMesh>().text = m_text;
                    }
                }
            }
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

        public void SetLetterData(ILivingLetterData livingLetterData)
        {
            isArabic = false;

            if (isUI) {
                gameObject.GetComponent<TextMeshProUGUI>().isRightToLeftText = true;
            } else {
                gameObject.GetComponent<TextMeshPro>().isRightToLeftText = true;
            }
            if (livingLetterData.DataType == LivingLetterDataType.Letter) {
                text = livingLetterData.TextForLivingLetter;
            } else if (livingLetterData.DataType == LivingLetterDataType.Word) {
                text = livingLetterData.TextForLivingLetter;
            }
        }

        public void SetSentence(Database.LocalizationDataId sentenceId)
        {
            // Debug.Log("SetSentence " + sentenceId);
            isArabic = true;
            text = LocalizationManager.GetTranslation(sentenceId);
        }
    }
}