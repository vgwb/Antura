using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Antura.Discover.Activities
{
    /// <summary>
    /// UI view for a single money item (coin or note).
    /// </summary>
    public class MoneyItemView : MonoBehaviour
    {
        [Header("Wiring")]
        public Image image;
        public TextMeshProUGUI valueLabel;

        [Header("Runtime")]
        public MoneySet.MoneyItem Model;

        public void Setup(MoneySet.MoneyItem model, string currencySymbol = "€")
        {
            Model = model;
            if (image)
                image.sprite = model.Image;
            if (valueLabel)
                valueLabel.text = $"{currencySymbol} {Model.Value:0.00}";
        }
    }
}
