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
            Label.SetTextUnfiltered(text, Font2Use.Learning, forceFont: true);
            Label.TMPText.enabled = !(string.IsNullOrEmpty(text)); // Fix to avoid the wordcomposer showing the last text sometimes (seems a TMPro bug, see https://github.com/vgwb-private/Antura/issues/298)
        }

        public void SetLetterData(ILivingLetterData data)
        {
            Label.gameObject.SetActive(true);
            NumbersLabel.gameObject.SetActive(false);
            Label.SetLetterData(data);
            Label.TMPText.enabled = true;
        }

        public void SetNumber(int numberValue)
        {
            Label.gameObject.SetActive(false);
            NumbersLabel.gameObject.SetActive(true);
            NumbersLabel.SetTextUnfiltered(numberValue.ToString());
            Label.TMPText.enabled = true;
        }

    }
}
