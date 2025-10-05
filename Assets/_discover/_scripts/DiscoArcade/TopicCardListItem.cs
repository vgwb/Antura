using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    public class TopicCardListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text relationLabel;
        [SerializeField] private TMP_Text descriptionLabel;
        [SerializeField] private Button openButton;

        private CardData _card;
        private Action<CardData> _onSelected;

        public void Bind(CardData card, string relationText, string descriptionText, Action<CardData> onSelected)
        {
            _card = card;
            _onSelected = onSelected;

            if (relationLabel != null)
                relationLabel.text = relationText ?? string.Empty;

            if (descriptionLabel != null)
                descriptionLabel.text = descriptionText ?? string.Empty;

            if (openButton != null)
            {
                openButton.onClick.RemoveAllListeners();
                openButton.onClick.AddListener(HandleClick);
            }
        }

        private void HandleClick()
        {
            _onSelected?.Invoke(_card);
        }
    }
}
