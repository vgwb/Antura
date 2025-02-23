using Antura.Audio;
using Antura.Core;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using Homer;
using System;
using System.Collections;
using System.Collections.Generic;
using Antura.Minigames.DiscoverCountry.Interaction;
using UnityEngine;
using Antura.Language;

namespace Antura.Minigames.DiscoverCountry
{
    public class DialoguesUI : MonoBehaviour
    {
        public enum DialogueType
        {
            None,
            Text,
            Choice
        }

        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] DialogueSignal signal;
        [DeEmptyAlert]
        [SerializeField] GameObject contentBox;
        [DeEmptyAlert]
        [SerializeField] NarratorBalloon narratorBalloon;
        [DeEmptyAlert]
        [SerializeField] DialogueChoices choices;
        [DeEmptyAlert]
        [SerializeField] DialoguePostcard postcard;
        [DeEmptyAlert]
        [SerializeField] DialoguePostcardFocusView postcardFocusView;

        #endregion

        public bool IsOpen { get; private set; }
        public bool IsPostcardOpen => IsOpen && postcardFocusView.IsOpen;
        public DialogueType CurrDialogueType { get; private set; }

        int currChoiceIndex;
        bool currPostcardWasZoomed;
        bool gotoNextWhenPostcardFocusViewCloses;
        bool UseLearningLanguage = true;
        QuestNode currNode;
        DialogueSignal previewSignalPrefab;
        readonly Dictionary<Interactable, DialogueSignal> previewSignalByInteractable = new();
        AbstractDialogueBalloon currBalloon;
        Coroutine coShowDialogue, coNext;

        #region Unity

        void Awake()
        {
            contentBox.SetActive(true);
            narratorBalloon.gameObject.SetActive(true);
            postcardFocusView.Hide(true);
            previewSignalPrefab = Instantiate(signal, signal.transform.parent, false);
            signal.Setup(false);

            narratorBalloon.OnBalloonClicked.Subscribe(OnBalloonClicked);
            narratorBalloon.OnBalloonContinueClicked.Subscribe(OnBalloonContinueClicked);
            choices.OnChoiceConfirmed.Subscribe(OnChoiceConfirmed);
            postcard.OnClicked.Subscribe(ShowPostcardFocusView);
            postcardFocusView.OnClicked.Subscribe(OnPostcardFocusViewClicked);
            DiscoverNotifier.Game.OnActClicked.Subscribe(OnActClicked);
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
            this.CancelCoroutine(ref coShowDialogue);
            this.CancelCoroutine(ref coNext);
            narratorBalloon.OnBalloonClicked.Unsubscribe(OnBalloonClicked);
            narratorBalloon.OnBalloonContinueClicked.Unsubscribe(OnBalloonContinueClicked);
            choices.OnChoiceConfirmed.Unsubscribe(OnChoiceConfirmed);
            postcard.OnClicked.Unsubscribe(ShowPostcardFocusView);
            postcardFocusView.OnClicked.Unsubscribe(OnPostcardFocusViewClicked);
            DiscoverNotifier.Game.OnActClicked.Unsubscribe(OnActClicked);
        }

        #endregion

        #region Public Methods

        public void ShowSignalFor(Interactable interactable)
        {
            if (previewSignalByInteractable.ContainsKey(interactable))
                previewSignalByInteractable[interactable].Hide(true);
            signal.ShowFor(interactable);
        }

        public void HideSignal(Interactable interactable, bool showPreviewIfExists)
        {
            bool immediate = false;
            if (showPreviewIfExists && interactable.IsInteractable && previewSignalByInteractable.ContainsKey(interactable))
            {
                previewSignalByInteractable[interactable].ShowFor(interactable, true);
                immediate = true;
            }
            signal.Hide(immediate);
        }

        public void ShowPreviewSignalFor(Interactable interactable, bool show)
        {
            if (interactable == null)
                return;

            if (show)
            {
                if (!previewSignalByInteractable.ContainsKey(interactable))
                {
                    DialogueSignal previewSignal = Instantiate(previewSignalPrefab);
                    previewSignal.transform.SetParent(interactable.transform); // Switch parent after instantiation so scale is kept consistent
                    previewSignal.Setup(true);
                    previewSignal.gameObject.SetActive(true);
                    previewSignal.ShowFor(interactable);
                    previewSignalByInteractable.Add(interactable, previewSignal);
                }
                else
                {
                    previewSignalByInteractable[interactable].ShowFor(interactable);
                }
            }
            else
            {
                if (previewSignalByInteractable.ContainsKey(interactable))
                {
                    previewSignalByInteractable[interactable].Hide();
                }
            }
        }

        public void StartDialogue(QuestNode node)
        {
            ShowDialogueFor(node);
        }

        public void CloseDialogue(int choiceIndex = 0)
        {
            if (!IsOpen)
                return;

            IsOpen = false;
            if (currBalloon != null)
                currBalloon.Hide();
            postcard.Hide();
            if (choices.IsOpen)
                choices.Hide(choiceIndex);
            DiscoverNotifier.Game.OnCloseDialogue.Dispatch();
            CurrDialogueType = DialogueType.None;
        }

        #endregion

        #region Methods

        void ShowDialogueFor(QuestNode node)
        {
            currChoiceIndex = 0;
            currPostcardWasZoomed = gotoNextWhenPostcardFocusViewCloses = false;
            CoroutineRunner.RestartCoroutine(ref coShowDialogue, CO_ShowDialogueFor(node));
        }

        IEnumerator CO_ShowDialogueFor(QuestNode node)
        {
            IsOpen = true;
            currNode = node;
            currBalloon = narratorBalloon; // TODO : Assign correct balloon
            while (InteractionManager.I.IsUsingFocusView)
                yield return null;

            Sprite image;
            UseLearningLanguage = !node.Native;
            switch (node.Type)
            {
                case NodeType.TEXT:
                    CurrDialogueType = DialogueType.Text;
                    currBalloon.Show(node);
                    image = node.GetImage();
                    if (image != null)
                        postcard.Show(image);
                    else
                        postcard.Hide();
                    break;
                case NodeType.CHOICE:
                case NodeType.QUIZ:
                    CurrDialogueType = DialogueType.Choice;
                    if (!string.IsNullOrEmpty(node.Content))
                        currBalloon.Show(node);
                    image = node.GetImage();
                    if (image != null)
                        postcard.Show(image);
                    else
                        postcard.Hide();
                    yield return new WaitForSeconds(0.3f);
                    choices.Show(node.Choices);
                    break;
                default:
                    IsOpen = false;
                    CurrDialogueType = DialogueType.None;
                    Debug.LogError($"DialoguesUI.ShowDialogueNode ► QuestNode is of invalid type ({node.Type})");
                    break;
            }
            coShowDialogue = null;
        }

        void Next(int choiceIndex = 0)
        {
            CoroutineRunner.RestartCoroutine(ref coNext, CO_Next(choiceIndex));
        }

        IEnumerator CO_Next(int choiceIndex)
        {
            currChoiceIndex = choiceIndex;
            if (gotoNextWhenPostcardFocusViewCloses)
            {
                // Close postcard zoom and move onward
                if (postcardFocusView.IsOpen)
                {
                    HidePostcardFocusView();
                    yield return new WaitForSeconds(0.15f);
                }
            }
            else
            {
                if (currNode.ImageAutoOpen && postcard.IsActive && !currPostcardWasZoomed)
                {
                    // Zoom into postcard and wait for next action
                    ShowPostcardFocusView(postcard.CurrSprite);
                    gotoNextWhenPostcardFocusViewCloses = true;
                    yield break;
                }
            }

            if (currBalloon != null && currBalloon.IsOpen)
                currBalloon.Hide();

            if (choices.IsOpen)
            {
                choices.Hide(choiceIndex);
                while (choices.IsHiding)
                    yield return null;
            }

            QuestNode next = QuestManager.I.GetNextNode(choiceIndex);
            if (next == null)
                CloseDialogue(choiceIndex);
            else
                ShowDialogueFor(next);

            coNext = null;
        }

        void ShowPostcardFocusView(Sprite sprite)
        {
            currPostcardWasZoomed = true;
            postcardFocusView.Show(sprite);
        }

        void HidePostcardFocusView()
        {
            postcardFocusView.Hide();
        }

        #endregion

        #region Callbacks

        void OnActClicked()
        {
            if (postcardFocusView.IsOpen && !gotoNextWhenPostcardFocusViewCloses)
                postcardFocusView.Hide();
            else if (CurrDialogueType == DialogueType.Text && !InteractionManager.I.IsUsingFocusView)
            {
                Next(currChoiceIndex);
            }
        }

        void OnBalloonClicked()
        {
            // Play/repeat alternate audio here
            UseLearningLanguage = !UseLearningLanguage;
            AudioManager.I.PlayDiscoverDialogue(
                 currNode.AudioId,
                 QuestManager.I.CurrentQuest.assetsFolder,
                 UseLearningLanguage ? AppManager.I.ContentEdition.LearningLanguage : AppManager.I.AppSettings.NativeLanguage
            );

            // Localize text to correspond to audio language
            currBalloon.LocalizeText(UseLearningLanguage ? LanguageUse.Learning : LanguageUse.Native);
        }

        void OnBalloonContinueClicked()
        {
            Next();
        }

        void OnChoiceConfirmed(int choiceIndex)
        {
            Next(choiceIndex);
        }

        void OnPostcardFocusViewClicked()
        {
            if (gotoNextWhenPostcardFocusViewCloses)
                Next(currChoiceIndex);
            else
                HidePostcardFocusView();
        }

        #endregion
    }
}
