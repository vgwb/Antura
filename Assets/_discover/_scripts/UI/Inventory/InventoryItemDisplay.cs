using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Antura.Discover
{
    public class InventoryItemDisplay : MonoBehaviour
    {
        public GameObject CounterGO;
        public TMP_Text CounterText;
        public Image IconImage;
        [Header("Selection Highlight")]
        [Tooltip("Optional: assign a highlight GameObject (e.g., glow frame) to toggle and animate when selected.")]
        public GameObject SelectionHighlight;
        [Tooltip("Optional: border image to pulse alpha when selected.")]
        public Image SelectionBorderImage;
        public InventoryItem CurrentItemData;
        private int lastQuantity = 0;
        private Vector3 _iconInitialScale;
        private Vector3 _rootInitialScale;
        private Tween _selectionTween;

        void Start()
        {
            _iconInitialScale = IconImage != null ? IconImage.transform.localScale : Vector3.one;
            _rootInitialScale = transform.localScale;
            // Ensure highlight starts hidden
            if (SelectionHighlight != null)
                SelectionHighlight.SetActive(false);
        }
        public void SetItemData(InventoryItem itemData)
        {
            var prevQty = lastQuantity;
            CurrentItemData = itemData;
            lastQuantity = Mathf.Max(0, itemData.Quantity);

            if (itemData.Quantity > 0)
            {
                CounterGO.SetActive(true);
                CounterText.text = itemData.Quantity.ToString();
            }
            else
            {
                CounterGO.SetActive(false);
            }

            // Set icon
            Sprite sprite = null;
            if (itemData.Item != null)
            {
                sprite = itemData.Item.Icon;
            }
            else if (itemData.Card != null && itemData.Card.ImageAsset != null && itemData.Card.ImageAsset.Image != null)
            {
                sprite = itemData.Card.ImageAsset.Image;
            }
            if (IconImage != null)
            {
                IconImage.sprite = sprite;
                IconImage.enabled = sprite != null;
            }

            // Animations based on transition
            if (prevQty == 0 && lastQuantity >= 1)
            {
                PlayAppear();
            }
            else if (lastQuantity > prevQty)
            {
                PlayIncrement();
            }
            else if (lastQuantity < prevQty)
            {
                PlayDecrement();
            }
        }

        // public void OnCLick()
        // {
        //     if (CurrentItemData == null)
        //     {
        //         Debug.LogWarning("CurrentItemData is null, cannot display item.");
        //         return;
        //     }

        //     // Display the item details in the UI
        //     // Debug.Log($"Displaying item: {CurrentItemData.ItemName}");
        //     // // Here you would typically update the UI elements to show the item's details
        //     // CounterGO.SetActive(true);
        //     // CounterText.text = CurrentItemData.ItemCount.ToString();
        // }

        public void PlayAppear()
        {
            // if (IconImage != null)
            // {
            //     var t = IconImage.transform;
            //     t.DOKill();
            //     t.localScale = Vector3.zero;
            //     t.DOScale(_iconInitialScale, 0.35f).SetEase(Ease.OutBack);
            // }
            // // small pop on root as well
            // transform.DOKill();
            // transform.localScale = _rootInitialScale * 0.9f;
            // transform.DOScale(_rootInitialScale, 0.25f).SetEase(Ease.OutBack);
        }

        public void PlayIncrement()
        {
            // Punch icon and counter
            if (IconImage != null)
            {
                IconImage.transform.DOKill();
                IconImage.transform.DOPunchScale(Vector3.one * 0.15f, 0.2f, 8, 1f);
            }
            if (CounterGO != null)
            {
                CounterGO.transform.DOKill();
                CounterGO.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 8, 1f);
            }
        }

        public void PlayDecrement()
        {
            // Flash counter red then back
            if (CounterText != null)
            {
                var baseColor = CounterText.color;
                CounterText.DOKill();
                CounterText.DOColor(Color.red, 0.08f).OnComplete(() =>
                {
                    CounterText.DOColor(baseColor, 0.18f);
                });
            }
            // Slight inward pulse
            transform.DOKill();
            transform.DOScale(_rootInitialScale * 0.95f, 0.08f).OnComplete(() =>
            {
                transform.DOScale(_rootInitialScale, 0.12f);
            });
        }

        public void PlayRemoveAndDestroy(System.Action onComplete)
        {
            // Shrink and fade out, then callback for destruction
            // var seq = DOTween.Sequence();
            // var cg = GetComponent<CanvasGroup>();
            // if (cg == null)
            //     cg = gameObject.AddComponent<CanvasGroup>();
            // seq.Join(transform.DOScale(0.0f, 0.2f).SetEase(Ease.InBack));
            // seq.Join(cg.DOFade(0f, 0.2f));
            // seq.OnComplete(() => { onComplete?.Invoke(); });
            onComplete?.Invoke();
        }

        public void SetSelected(bool selected)
        {
            if (selected)
            {
                // Show highlight GO if assigned
                if (SelectionHighlight != null)
                    SelectionHighlight.SetActive(true);
            }
            else
            {
                // Hide highlight and restore defaults
                if (SelectionHighlight != null)
                    SelectionHighlight.SetActive(false);
            }
        }

    }
}
