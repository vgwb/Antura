using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    public class TopicListItem : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text topicLabel;
        [SerializeField] private TMP_Text coreCardLabel;
        [SerializeField] private GameObject selectedHighlight;

        private Action<TopicData> _onSelected;
        private string _coreCardTitle;

        public TopicData Topic { get; private set; }

        public void Bind(TopicData topic, bool isSelected, Action<TopicData> onSelected)
        {
            Topic = topic;
            _onSelected = onSelected;
            RefreshLabels();
            RefreshSelection(isSelected);

            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(HandleClicked);
            }
        }

        public void RefreshSelection(bool isSelected)
        {
            if (selectedHighlight != null)
            {
                selectedHighlight.SetActive(isSelected);
            }
        }

        private void RefreshLabels()
        {
            if (topicLabel != null)
            {
                topicLabel.text = Topic != null ? Topic.Name : "";
            }

            if (coreCardLabel != null)
            {
                _coreCardTitle = GetCoreCardTitle();
                coreCardLabel.text = _coreCardTitle;
            }
        }

        private string GetCoreCardTitle()
        {
            if (Topic?.CoreCard == null)
                return string.Empty;

            var data = DiscoverDataManager.I;
            if (data == null)
            {
                return Topic.CoreCard != null ? Topic.CoreCard.name : string.Empty;
            }

            return data.GetCardTitle(Topic.CoreCard);
        }

        private void HandleClicked()
        {
            if (Topic == null)
                return;
            _onSelected?.Invoke(Topic);
        }
    }
}
