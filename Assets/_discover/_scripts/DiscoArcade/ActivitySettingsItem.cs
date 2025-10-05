using System;
using Antura.Discover.Activities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    /// <summary>
    /// Button representing a concrete ActivitySettings asset.
    /// </summary>
    public class ActivitySettingsItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private Button button;

        private ActivitySettingsAbstract _settings;
        private Action<ActivitySettingsAbstract> _onClick;

        public void Bind(ActivitySettingsAbstract settings, Action<ActivitySettingsAbstract> onClick)
        {
            _settings = settings;
            _onClick = onClick;

            if (label != null)
            {
                string text = settings != null ? (!string.IsNullOrEmpty(settings.Id) ? settings.Id : settings.name) : string.Empty;
                label.text = text;
            }

            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.interactable = settings != null;
                if (settings != null)
                {
                    button.onClick.AddListener(HandleClick);
                }
            }
        }

        private void HandleClick()
        {
            if (_settings == null)
                return;
            _onClick?.Invoke(_settings);
        }
    }
}
