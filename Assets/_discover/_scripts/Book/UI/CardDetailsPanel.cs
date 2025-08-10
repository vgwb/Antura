using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Antura.Discover.Achievements.UI
{
    public class CardDetailsPanel : MonoBehaviour
    {
        [Header("UI")]
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        public Image image;
        public Image soundIcon;
        public GameObject lockedBadge;

        [Header("Audio")]
        public AudioSource audioSource;

        [Header("Locked Greyscale")]
        public Material greyscaleMaterial;

        // Cache of current async handles so we can release if needed
        private AsyncOperationHandle<string>? titleHandle;
        private AsyncOperationHandle<string>? descHandle;

        private CardDefinition def;
        private CardState state;

        public void Show(CardDefinition def, CardState state)
        {
            this.def = def;
            this.state = state;

            gameObject.SetActive(true);

            bool isLocked = state == null || !state.Unlocked;

            SetLocalized(titleText, def.Title, fallback: def.Id);
            if (isLocked)
            {
                SetLocalized(descriptionText, def.Description, fallback: "Locked. Play quests to discover this content.");
            }
            else
            {
                SetLocalized(descriptionText, def.Description, fallback: string.Empty);
            }

            if (image)
                image.sprite = def.Image;
            if (image && greyscaleMaterial != null)
                image.material = isLocked ? greyscaleMaterial : null;

            if (lockedBadge)
                lockedBadge.SetActive(isLocked);
            if (soundIcon)
                soundIcon.enabled = def.Audio != null;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnPlayAudio()
        {
            if (def?.Audio == null || audioSource == null)
                return;
            audioSource.PlayOneShot(def.Audio);
        }

        private void SetLocalized(TMP_Text label, LocalizedString localized, string fallback)
        {
            if (!label)
                return;
            if (localized == null)
            {
                label.text = fallback;
                return;
            }

            // Fetch localized text asynchronously
            var handle = localized.GetLocalizedStringAsync();
            handle.Completed += (op) => label.text = string.IsNullOrEmpty(op.Result) ? fallback : op.Result;

            // Track handles
            if (ReferenceEquals(label, titleText))
                titleHandle = handle;
            if (ReferenceEquals(label, descriptionText))
                descHandle = handle;
        }
    }
}
