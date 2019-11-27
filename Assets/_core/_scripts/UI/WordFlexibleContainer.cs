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

        public void Reset()
        {
            Label.gameObject.SetActive(true);
            NumbersLabel.gameObject.SetActive(false);
            Label.text = "";
            NumbersLabel.text = "";
        }

        public void SetTextUnfiltered(string text)
        {
            Label.gameObject.SetActive(true);
            NumbersLabel.gameObject.SetActive(false);
            Label.SetTextUnfiltered(text);
        }

        public void SetLetterData(ILivingLetterData data)
        {
            Label.gameObject.SetActive(true);
            NumbersLabel.gameObject.SetActive(false);
            Label.SetLetterData(data);
        }

        public void SetNumber(int numberValue)
        {
            Label.gameObject.SetActive(false);
            NumbersLabel.gameObject.SetActive(true);
            NumbersLabel.SetTextUnfiltered(numberValue.ToString());
        }

    }
}