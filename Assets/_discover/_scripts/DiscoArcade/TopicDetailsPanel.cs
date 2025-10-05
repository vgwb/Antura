using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Antura.Discover
{
    public class TopicDetailsPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text topicTitleLabel;
        [SerializeField] private TMP_Text countryLabel;
        [SerializeField] private TMP_Text coreCardLabel;
        [SerializeField] private RectTransform connectionsContent;
        [SerializeField] private TopicCardListItem cardItemPrefab;
        [SerializeField] private GameObject emptyState;

        private readonly List<TopicCardListItem> _items = new();

        public event Action<CardData> CardSelected;

        public void Show(TopicData topic)
        {
            if (topic == null)
            {
                if (topicTitleLabel != null)
                    topicTitleLabel.text = "";
                if (countryLabel != null)
                    countryLabel.text = "";
                if (coreCardLabel != null)
                    coreCardLabel.text = "";
                ToggleEmptyState(true);
                ClearItems();
                return;
            }

            if (topicTitleLabel != null)
                topicTitleLabel.text = topic.Name;

            if (countryLabel != null)
                countryLabel.text = topic.Country.ToString();

            if (coreCardLabel != null)
                coreCardLabel.text = GetCardTitle(topic.CoreCard);

            ToggleEmptyState(false);

            var entries = BuildEntries(topic);
            EnsureItemCount(entries.Count);
            for (int i = 0; i < _items.Count; i++)
            {
                if (i < entries.Count)
                {
                    var entry = entries[i];
                    _items[i].gameObject.SetActive(true);
                    _items[i].Bind(entry.card, entry.connectionLabel, entry.description, HandleCardSelected);
                }
                else
                {
                    _items[i].gameObject.SetActive(false);
                }
            }
        }

        private void HandleCardSelected(CardData card)
        {
            if (card == null)
                return;

            CardSelected?.Invoke(card);
        }

        private List<(CardData card, string connectionLabel, string description)> BuildEntries(TopicData topic)
        {
            var list = new List<(CardData, string, string)>();
            if (topic == null)
                return list;

            if (topic.CoreCard != null)
            {
                list.Add((topic.CoreCard, "Core", GetCardTitle(topic.CoreCard)));
            }

            if (topic.Connections != null)
            {
                foreach (var connection in topic.Connections)
                {
                    if (connection?.ConnectedCard == null)
                        continue;
                    string label = connection.ConnectionType.ToString();
                    string description = BuildConnectionDescription(connection);
                    list.Add((connection.ConnectedCard, label, description));
                }
            }

            return list;
        }

        private string BuildConnectionDescription(CardConnection connection)
        {
            var title = GetCardTitle(connection?.ConnectedCard);
            if (string.IsNullOrEmpty(connection?.ConnectionReason))
                return title;

            if (string.IsNullOrEmpty(title))
                return connection.ConnectionReason;

            return $"{title} — {connection.ConnectionReason}";
        }

        private void ToggleEmptyState(bool isEmpty)
        {
            if (emptyState != null)
            {
                emptyState.SetActive(isEmpty);
            }
        }

        private void EnsureItemCount(int desired)
        {
            if (cardItemPrefab == null || connectionsContent == null)
                return;

            while (_items.Count < desired)
            {
                var item = Instantiate(cardItemPrefab, connectionsContent);
                _items.Add(item);
            }
        }

        private void ClearItems()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] != null)
                    _items[i].gameObject.SetActive(false);
            }
        }

        private string GetCardTitle(CardData card)
        {
            if (card == null)
                return string.Empty;

            var data = DiscoverDataManager.I;
            if (data == null)
                return !string.IsNullOrEmpty(card.TitleEn) ? card.TitleEn : card.name;

            var localized = data.GetCardTitle(card);
            if (!string.IsNullOrEmpty(localized))
                return localized;

            return !string.IsNullOrEmpty(card.TitleEn) ? card.TitleEn : card.name;
        }
    }
}
