using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public class TopicListPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private TopicListItem itemPrefab;

        private readonly List<TopicListItem> _items = new();

        public event Action<TopicData> SelectionRequested;

        /// <summary>
        /// Populate the list with the given topics.
        /// </summary>
        public void SetTopics(IReadOnlyList<TopicData> topics, TopicData selectedTopic)
        {
            int count = topics != null ? topics.Count : 0;
            EnsureItemCount(count);

            for (int i = 0; i < _items.Count; i++)
            {
                if (i < count)
                {
                    var topic = topics[i];
                    _items[i].gameObject.SetActive(true);
                    _items[i].Bind(topic, topic == selectedTopic, HandleItemSelected);
                }
                else
                {
                    _items[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Update selection highlight without rebuilding the list.
        /// </summary>
        public void Highlight(TopicData selectedTopic)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].RefreshSelection(_items[i].Topic == selectedTopic);
            }
        }

        private void HandleItemSelected(TopicData topic)
        {
            SelectionRequested?.Invoke(topic);
        }

        private void EnsureItemCount(int desired)
        {
            if (itemPrefab == null || content == null)
            {
                Debug.LogWarning("TopicListView: missing prefab or content.");
                return;
            }

            while (_items.Count < desired)
            {
                var item = Instantiate(itemPrefab, content);
                _items.Add(item);
            }
        }
    }
}
