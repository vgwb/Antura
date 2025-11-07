using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Antura.Discover;

namespace Antura.Discover.Activities
{
    public enum HighlightVisualState
    {
        Default,
        Hover,
        Correct,
        Wrong
    }

    public class QuestionItem : MonoBehaviour, IPointerClickHandler
    {
        [Header("References")]
        public Image CardImage;
        public TextMeshProUGUI Label;
        public GameObject Highlight;
        public Image highlightImage;

        public MatchDropSlot DropSlot;

        [Header("Data")]
        public CardData Data;
        public string ExpectedAnswerId;

        [Header("Interaction")]
        [Tooltip("Minimum seconds between voice playback when clicking the card.")]
        public float ClickCooldown = 0.5f;

        private float _lastClickTime = -999f;
        private Action<CardData> _onClicked;

        private HighlightVisualState _currentHighlightState = HighlightVisualState.Default;

        public void Init(string title, Sprite sprite, int slotIndex, ActivityMatch manager, CardData data, string expectedAnswerId = null, Action<CardData> onClicked = null)
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
            _onClicked = onClicked;

            if (DropSlot != null)
            {
                DropSlot.manager = manager;
                DropSlot.slotIndex = slotIndex;
                DropSlot.Owner = this;
            }

            SetHighlightState(HighlightVisualState.Default);
        }

        /// null -> default, true -> green, false -> red
        public void SetHighlight(bool? correct)
        {
            if (correct == null)
            {
                _currentHighlightState = HighlightVisualState.Default;

            }
            else
            {
                _currentHighlightState = correct.Value ? HighlightVisualState.Correct : HighlightVisualState.Wrong;
            }
            SetHighlightState(_currentHighlightState);
        }

        public void SetHighlightState(HighlightVisualState state)
        {
            Color color;
            if (state == HighlightVisualState.Hover)
            {
                color = new Color(1f, 1f, 1f, 0.5f);
            }
            else if (state == HighlightVisualState.Correct)
            {
                color = new Color(0.1f, 0.85f, 0.2f, 0.35f);
            }
            else if (state == HighlightVisualState.Wrong)
            {
                color = new Color(1f, 0.2f, 0.2f, 0.35f);
            }
            else
            {
                color = new Color(1f, 1f, 1f, 0.2f);
            }
            highlightImage.color = color;
        }

        public void SetHoverVisual(bool isHover)
        {
            SetHighlightState(isHover ? HighlightVisualState.Hover : _currentHighlightState);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Data == null)
                return;

            if (Time.unscaledTime - _lastClickTime < ClickCooldown)
                return;

            _lastClickTime = Time.unscaledTime;
            _onClicked?.Invoke(Data);
        }
    }
}
