using Antura.Language;
using Antura.LivingLetters;
using UnityEngine;

namespace Antura.UI
{
    /// <summary>
    /// A flexible container for text.
    /// </summary>
    public class WordFlexibleContainer : MonoBehaviour
    {
        public TextRender Label;
        public TextRender NumbersLabel;

        public void SetText(string text, LanguageUse languageUse)
        {
            Label.gameObject.SetActive(true);
            NumbersLabel.gameObject.SetActive(false);
            Label.SetText(text, languageUse);
        }

        public void SetText(ILivingLetterData data)
        {
            Label.gameObject.SetActive(true);
            NumbersLabel.gameObject.SetActive(false);
            Label.SetLetterData(data);
        }

        public void Reset()
        {
            Label.gameObject.SetActive(true);
            NumbersLabel.gameObject.SetActive(false);
            Label.text = "";
            NumbersLabel.text = "";
        }

        public void SetNumber(int numberValue)
        {
            Label.gameObject.SetActive(false);
            NumbersLabel.gameObject.SetActive(true);
            NumbersLabel.SetTextUnfiltered(numberValue.ToString());
        }
    }
}