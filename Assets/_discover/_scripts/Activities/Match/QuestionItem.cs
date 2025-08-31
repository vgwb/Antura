using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Antura.Discover;

namespace Antura.Discover.Activities
{
    [DisallowMultipleComponent]
    public class QuestionItem : MonoBehaviour
    {
        [Header("References")]
        public Image CardImage;
        public TextMeshProUGUI Label;
        public GameObject Highlight;
        [HideInInspector] public MatchDropSlot DropSlot;

        [Header("Data")]
        public CardData Data;
        public string ExpectedAnswerId;

        private Image _highlightImage;

        public void Init(string title, Sprite sprite, int slotIndex, ActivityMatch manager, CardData data, string expectedAnswerId = null)
        {
            if (CardImage == null)
            {
                var tr = transform.Find("CardImage");
                if (tr)
                    CardImage = tr.GetComponent<Image>();
            }
            if (CardImage != null)
                CardImage.sprite = sprite;
            if (Label == null)
                Label = GetComponentInChildren<TextMeshProUGUI>();
            if (Label != null)
                Label.text = title;

            Data = data;
            ExpectedAnswerId = expectedAnswerId;

            // Drop overlay (needs a Graphic to receive UI events)
            var dropGO = new GameObject("DropOverlay", typeof(RectTransform));
            dropGO.transform.SetParent(transform, false);
            var img = dropGO.AddComponent<Image>();
            img.color = new Color(1, 1, 1, 0f); // invisible, but raycastable
            img.raycastTarget = true;
            DropSlot = dropGO.AddComponent<MatchDropSlot>();
            DropSlot.manager = manager;
            DropSlot.slotIndex = slotIndex;

            // Cache highlight image
            if (Highlight != null)
                _highlightImage = Highlight.GetComponent<Image>();
            if (_highlightImage == null && Highlight != null)
                _highlightImage = Highlight.AddComponent<Image>();
            // Default normal
            SetHighlight(null);
        }

        /// null -> default, true -> green, false -> red
        public void SetHighlight(bool? correct)
        {
            if (Highlight == null)
                return;
            if (_highlightImage == null)
                _highlightImage = Highlight.GetComponent<Image>();
            if (correct == null)
            {
                var color = Color.white;
                color.a = .2f;
                _highlightImage.color = color;
                // Highlight.SetActive(false);
            }
            else
            {
                var color = correct.Value ? Color.green : Color.red;
                color.a = 0.35f;
                _highlightImage.color = color;
                // Highlight.SetActive(true);
            }
        }
    }
}
