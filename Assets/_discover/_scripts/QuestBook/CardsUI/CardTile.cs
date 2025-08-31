using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Antura.Discover.UI
{
    public class CardTile : MonoBehaviour
    {
        [Header("UI")]
        public Image image;
        public Image lockOverlay;
        public Image soundIcon;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI categoryText;

        [Header("Locked Greyscale")]
        public Material greyscaleMaterial;

        [Header("Image Cover-Fit")]
        [Tooltip("When enabled, the card image will fill the viewport height/width")]
        public bool imageCoverFit = true;
        [Tooltip("Viewport that defines the clipping area for the image")]
        public RectTransform imageViewport;
        [Tooltip("Auto-add a RectMask2D to the viewport if missing")]
        public bool autoAddRectMask = true;

        private CardData def;
        private CardState state;
        private Action<CardData> onClick;

        public CardData Data => def;
        public CardState State => state;

        public void Init(CardData card, CardState state, Action<CardData> onClick)
        {
            this.def = card;
            this.state = state;
            this.onClick = onClick;

            titleText.text = card.Title.GetLocalizedString();
            categoryText.text = card.Type.ToString();

            if (image)
            {
                image.sprite = card.ImageAsset.Image;
                if (imageCoverFit)
                    ApplyCoverFit(image.sprite);
            }

            bool isLocked = state == null || !state.unlocked;

            if (lockOverlay)
                lockOverlay.enabled = isLocked;
            if (soundIcon)
                soundIcon.enabled = card.AudioAsset != null && card.AudioAsset.Audio != null;

            if (greyscaleMaterial != null && image != null)
                image.material = isLocked ? greyscaleMaterial : null;

        }

        private void OnRectTransformDimensionsChange()
        {
            if (imageCoverFit && image != null && image.sprite != null && gameObject.activeInHierarchy)
            {
                ApplyCoverFit(image.sprite);
            }
        }

        private void ApplyCoverFit(Sprite sprite)
        {
            if (image == null || sprite == null)
                return;

            // Determine viewport (area to cover)
            var view = imageViewport != null ? imageViewport : (image.rectTransform.parent as RectTransform);
            if (view == null)
                return;

            if (autoAddRectMask)
            {
                var mask = view.GetComponent<RectMask2D>();
                if (mask == null)
                    view.gameObject.AddComponent<RectMask2D>();
            }

            // Compute cover size: scale so that image fully covers the viewport and crop the overflow
            var viewSize = view.rect.size;
            if (viewSize.x <= 0 || viewSize.y <= 0)
                return;
            var spSize = sprite.rect.size;
            if (spSize.x <= 0 || spSize.y <= 0)
                return;

            float viewAspect = viewSize.x / viewSize.y;
            float spriteAspect = spSize.x / spSize.y;

            Vector2 targetSize;
            if (spriteAspect > viewAspect)
            {
                // Sprite is wider than viewport: match height, crop left/right
                targetSize = new Vector2(viewSize.y * spriteAspect, viewSize.y);
            }
            else
            {
                // Sprite is taller/narrower: match width, crop top/bottom
                targetSize = new Vector2(viewSize.x, viewSize.x / spriteAspect);
            }

            var rt = image.rectTransform;
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = targetSize;
            rt.anchoredPosition = Vector2.zero;
            // Since we are managing size by hand, preserveAspect is not needed
            image.preserveAspect = false;
        }

        public void OnTileClicked()
        {
            onClick?.Invoke(def);
        }
    }
}
