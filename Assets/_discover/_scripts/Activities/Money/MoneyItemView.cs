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

        /// <summary>
        /// Setup the view with the given model. The sprite will be assigned and sized using
        /// model.Width/model.Height when available.
        /// </summary>
        public void Setup(MoneySet.MoneyItem model, string currencySymbol = "€", float globalScale = 1.0f)
        {
            Model = model;
            if (image)
            {
                image.sprite = model.Image;

                // Determine target size from model or fallback to sprite rect
                if (image.rectTransform)
                {
                    float targetW = model.Width > 0 ? model.Width : (image.sprite ? image.sprite.rect.width : image.rectTransform.sizeDelta.x);
                    float targetH = model.Height > 0 ? model.Height : (image.sprite ? image.sprite.rect.height : image.rectTransform.sizeDelta.y);

                    // Apply base size (unscaled). The overall token scaling is handled via transform scale below.
                    var rt = image.rectTransform;
                    rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);
                    rt.sizeDelta = new Vector2(targetW, targetH);
                }
            }
            if (valueLabel)
                valueLabel.text = $"{currencySymbol} {Model.Value:0.00}";
            valueLabel.gameObject.SetActive(false); // TODO enable this only in tutorial

            transform.localScale = new Vector3(globalScale, globalScale, 1f);
        }
    }
}
