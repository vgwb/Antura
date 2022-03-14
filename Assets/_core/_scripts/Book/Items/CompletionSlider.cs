using Antura.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Book
{
    /// <summary>
    /// Displays the completion state of a variable
    /// </summary>
    public class CompletionSlider : MonoBehaviour
    {
        public TextRender InfoText;
        public Slider Slider;

        public void SetValue(float current, float max)
        {
            float ratio = current / max;
            Slider.value = ratio;
            InfoText.text = Mathf.RoundToInt(current).ToString() + " / " + Mathf.RoundToInt(max).ToString();
        }
    }
}
