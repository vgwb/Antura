using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Antura.Discover.UI
{
    public class CardDetailsPanel : MonoBehaviour
    {
        [Header("UI")]
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        public Image image;
        public Image soundIcon;
        public GameObject lockedBadge;

        public Button nextCardButton;
        public Button prevCardButton;

        [Header("Audio")]
        public AudioSource audioSource;

        [Header("Locked Greyscale")]
        public Material greyscaleMaterial;

        // Cache of current async handles so we can release if needed
        private AsyncOperationHandle<string>? titleHandle;
        private AsyncOperationHandle<string>? descHandle;

        private CardData def;
        private CardState state;
        private QuestCardsUI owner; // source list owner for navigation
        private System.Collections.Generic.IReadOnlyList<CardData> orderedCards;
        private Func<CardData, CardState> stateResolver;

        private void Awake()
        {
            if (nextCardButton)
            {
                nextCardButton.onClick.RemoveListener(OnNextCard);
                nextCardButton.onClick.AddListener(OnNextCard);
            }
            if (prevCardButton)
            {
                prevCardButton.onClick.RemoveListener(OnPrevCard);
                prevCardButton.onClick.AddListener(OnPrevCard);
            }
        }

        public void SetOwner(QuestCardsUI questCardsUI)
        {
            owner = questCardsUI;
            UpdateNavButtons();
        }

        public void SetNavData(System.Collections.Generic.IReadOnlyList<CardData> order, Func<CardData, CardState> resolver)
        {
            orderedCards = order;
            stateResolver = resolver;
            UpdateNavButtons();
        }

        public void Show(CardData card, CardState state)
        {
            this.def = card;
            this.state = state;
            var c = card.Title.GetLocalizedString();
            gameObject.SetActive(true);

            bool isLocked = state == null || !state.unlocked;

            SetLocalized(titleText, card.Title, fallback: card.Id);
            if (isLocked)
            {
                SetLocalized(descriptionText, card.Description, fallback: "Locked. Play quests to discover this content.");
            }
            else
            {
                SetLocalized(descriptionText, card.Description, fallback: string.Empty);
            }

            if (card.ImageAsset != null && card.ImageAsset.Image != null && image != null)
                image.sprite = card.ImageAsset.Image;
            if (image && greyscaleMaterial != null)
                image.material = isLocked ? greyscaleMaterial : null;

            // if (lockedBadge)
            //     lockedBadge.SetActive(isLocked);
            // if (soundIcon)
            //     soundIcon.enabled = def.AudioAsset != null && def.AudioAsset.Audio != null;

            UpdateNavButtons();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnPlayAudio()
        {
            if (def?.AudioAsset == null || audioSource == null)
                return;
            audioSource.PlayOneShot(def.AudioAsset.Audio);
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

        private void OnNextCard()
        {
            if (orderedCards == null || def == null)
                return;
            int idx = IndexOfCurrent();
            if (idx >= 0 && idx + 1 < orderedCards.Count)
            {
                var next = orderedCards[idx + 1];
                var nextState = stateResolver != null ? stateResolver(next) : null;
                Show(next, nextState);
            }
            UpdateNavButtons();
        }

        private void OnPrevCard()
        {
            if (orderedCards == null || def == null)
                return;
            int idx = IndexOfCurrent();
            if (idx > 0)
            {
                var prev = orderedCards[idx - 1];
                var prevState = stateResolver != null ? stateResolver(prev) : null;
                Show(prev, prevState);
            }
            UpdateNavButtons();
        }

        private void UpdateNavButtons()
        {
            bool canPrev = false, canNext = false;
            if (orderedCards != null && def != null)
            {
                int idx = IndexOfCurrent();
                canPrev = idx > 0;
                canNext = idx >= 0 && (idx + 1) < orderedCards.Count;
            }
            if (prevCardButton)
                prevCardButton.interactable = canPrev;
            if (nextCardButton)
                nextCardButton.interactable = canNext;
        }

        private int IndexOfCurrent()
        {
            if (orderedCards == null || def == null)
                return -1;
            for (int i = 0; i < orderedCards.Count; i++)
            {
                var cd = orderedCards[i];
                if (cd == def)
                    return i;
                if (cd != null && def != null && !string.IsNullOrEmpty(cd.Id) && cd.Id == def.Id)
                    return i;
            }
            return -1;
        }
    }
}
