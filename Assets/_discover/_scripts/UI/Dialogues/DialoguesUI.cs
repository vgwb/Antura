#nullable enable
using DG.DeInspektor.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Antura.Discover
{
    public class DialoguesUI : MonoBehaviour
    {

        public YarnTaskCompletionSource<DialogueOption?>? OnOptionSelected;

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
        [DeEmptyAlert]
        [SerializeField] StartEndPanel startEndPanel;

        #endregion

        public bool IsOpen { get; private set; }
        public bool IsPostcardOpen => IsOpen && postcardFocusView.IsOpen;
        public DialogueType CurrDialogueType { get; private set; }

        int currChoiceIndex;
        bool currPostcardWasZoomedOnce; // TRUE when current postcard was zoomed in at least once
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
            startEndPanel.gameObject.SetActive(true);
            postcardFocusView.Hide(true);
            previewSignalPrefab = Instantiate(signal, signal.transform.parent, false);
            signal.Setup(false);

            narratorBalloon.OnBalloonClicked.Subscribe(OnBalloonClicked);
            narratorBalloon.OnBalloonContinueClicked.Subscribe(OnBalloonContinueClicked);
            startEndPanel.OnBalloonClicked.Subscribe(OnBalloonClicked);
            startEndPanel.OnBalloonContinueClicked.Subscribe(OnBalloonContinueClicked);
            choices.OnChoiceConfirmed.Subscribe(OnChoiceConfirmed);
            postcard.OnClicked.Subscribe(OnPostcardClicked);
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
            startEndPanel.OnBalloonClicked.Unsubscribe(OnBalloonClicked);
            startEndPanel.OnBalloonContinueClicked.Unsubscribe(OnBalloonContinueClicked);
            choices.OnChoiceConfirmed.Unsubscribe(OnChoiceConfirmed);
            postcard.OnClicked.Unsubscribe(OnPostcardClicked);
            postcardFocusView.OnClicked.Unsubscribe(OnPostcardFocusViewClicked);
            DiscoverNotifier.Game.OnActClicked.Unsubscribe(OnActClicked);
        }

        #endregion

        #region Public Methods

        public void ShowStartPanel(QuestNode node)
        {
            UseLearningLanguage = !node.Native;
            startEndPanel.Show(node, UseLearningLanguage, true, 0);
        }

        public void ShowEndPanel(QuestNode node, int totStarsAchieved)
        {
            UseLearningLanguage = !node.Native;
            startEndPanel.Show(node, UseLearningLanguage, false, totStarsAchieved);
        }

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

        public void ShowPostcard(Sprite sprite, bool zoom = false)
        {
            if (zoom)
            {
                currPostcardWasZoomedOnce = true;
                postcardFocusView.Show(sprite);
            }
            else
            {
                currPostcardWasZoomedOnce = false;
                postcard.Show(sprite);
            }
        }

        public void HidePostcard()
        {
            gotoNextWhenPostcardFocusViewCloses = false;
            postcard.Hide();
            postcardFocusView.Hide();
        }

        #endregion

        #region Methods

        void ShowDialogueFor(QuestNode node)
        {
            currChoiceIndex = 0;
            currPostcardWasZoomedOnce = gotoNextWhenPostcardFocusViewCloses = false;
            CoroutineRunner.RestartCoroutine(ref coShowDialogue, CO_ShowDialogueFor(node));
        }

        IEnumerator CO_ShowDialogueFor(QuestNode node)
        {
            Debug.Log("IS THIS DEPRECATED??");
            IsOpen = true;
            currNode = node;
            currBalloon = narratorBalloon; // Can be changed by switch below
            while (InteractionManager.I.IsUsingFocusView)
                yield return null;

            Sprite image;
            UseLearningLanguage = !node.Native;
            switch (node.Type)
            {
                case NodeType.TEXT:
                    CurrDialogueType = DialogueType.Text;
                    currBalloon.Show(node, UseLearningLanguage);
                    image = node.GetImage();
                    if (image != null)
                        postcard.Show(image);
                    else
                        postcard.Hide();
                    break;
                case NodeType.PANEL:
                    currBalloon = startEndPanel;
                    CurrDialogueType = DialogueType.Text;
                    ShowStartPanel(node);
                    break;
                case NodeType.CHOICE:
                case NodeType.QUIZ:
                    CurrDialogueType = DialogueType.Choice;
                    if (!string.IsNullOrEmpty(node.Content))
                        currBalloon.Show(node, UseLearningLanguage);
                    image = node.GetImage();
                    if (image != null)
                        postcard.Show(image);
                    else
                        postcard.Hide();
                    yield return new WaitForSeconds(0.3f);
                    choices.Show(node.Choices, UseLearningLanguage);
                    break;
                default:
                    IsOpen = false;
                    CurrDialogueType = DialogueType.None;
                    Debug.LogError($"DialoguesUI.ShowDialogueNode ► QuestNode is of invalid type ({node.Type})");
                    break;
            }
            coShowDialogue = null;
        }

        public void ShowDialogueLine(QuestNode node)
        {
            Debug.Log("DialoguesUI.ShowDialogueLine " + node.Content + " / " + node.ContentNative);

            IsOpen = true;
            currNode = node;
            currBalloon = narratorBalloon;

            UseLearningLanguage = !node.Native;
            switch (node.Type)
            {
                case NodeType.TEXT:
                    CurrDialogueType = DialogueType.Text;
                    currBalloon.Show(node, UseLearningLanguage);
                    // image = node.GetImage();
                    // if (image != null)
                    //     postcard.Show(image);
                    // else
                    //     postcard.Hide();
                    break;
                case NodeType.PANEL:
                    currBalloon = startEndPanel;
                    CurrDialogueType = DialogueType.Text;
                    ShowStartPanel(node);
                    break;
                case NodeType.PANEL_ENDGAME:
                    currBalloon = startEndPanel;
                    CurrDialogueType = DialogueType.Text;
                    ShowEndPanel(node, QuestManager.I.Progress.GetCurrentStarsAchieved());
                    break;
                case NodeType.CHOICE:
                case NodeType.QUIZ:
                    CurrDialogueType = DialogueType.Choice;
                    if (!string.IsNullOrEmpty(node.Content))
                        currBalloon.Show(node, UseLearningLanguage);
                    // image = node.GetImage();
                    // if (image != null)
                    //     postcard.Show(image);
                    // else
                    //     postcard.Hide();
                    //yield return new WaitForSeconds(0.3f);
                    choices.Show(node.Choices, UseLearningLanguage);
                    break;
                default:
                    IsOpen = false;
                    CurrDialogueType = DialogueType.None;
                    Debug.LogError($"DialoguesUI.ShowDialogueNode ► QuestNode is of invalid type ({node.Type})");
                    break;
            }
            //coShowDialogue = null;
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
                if (currNode.ImageAutoOpen && postcard.IsActive && !currPostcardWasZoomedOnce)
                {
                    // Zoom into postcard and wait for next action
                    ShowPostcard(postcard.CurrSprite, true);
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

            if (currNode.Type == NodeType.CHOICE || currNode.Type == NodeType.QUIZ)
            {
                // Resolve choice to Yarn; DialogueRunner will proceed
                currNode.Choices[choiceIndex].OnOptionSelected?.TrySetResult(currNode.Choices[choiceIndex].YarnOption);
            }
            else
            {
                // For text lines, request next line from Yarn
                YarnAnturaManager.I?.Runner?.RequestNextLine();
            }

            coNext = null;
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
            {
                postcardFocusView.Hide();
            }
            else if (CurrDialogueType == DialogueType.Text && (!InteractionManager.I.IsUsingFocusView || this.gameObject.activeSelf))
            {
                // Previously, with Homer, the condition was only checking if the camera focus view was active,
                // and if so would return FALSE because SPACE/E would have to exit focus view and show the dialogue UI again.
                // Since we moved to Yarn, the logic has changed in that the dialogue UI is not hidden anymore when focusing the camera,
                // so now it returns FALSE only if we're in focus mode AND the UI is hidden
                Next(currChoiceIndex);
            }
        }

        void OnBalloonClicked()
        {
            // Debug.Log("OnBalloonClicked");
            UseLearningLanguage = !UseLearningLanguage;
            currBalloon.DisplayText(UseLearningLanguage);
        }

        void OnBalloonContinueClicked()
        {
            Next();
        }

        void OnChoiceConfirmed(int choiceIndex)
        {
            Next(choiceIndex);
        }

        void OnPostcardClicked(Sprite sprite)
        {
            ShowPostcard(sprite, true);
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
