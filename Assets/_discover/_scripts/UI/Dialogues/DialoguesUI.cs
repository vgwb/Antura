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
        [SerializeField] StartEndPanel startEndPanel;

        #endregion

        public bool IsOpen { get; private set; }
        public bool IsPostcardOpen => IsOpen && postcard.IsActive;
        public DialogueType CurrDialogueType { get; private set; }

        int currChoiceIndex;
        bool gotoNextWhenPostcardFocusViewCloses;
        bool learningLanguageFirst = true;
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
            previewSignalPrefab = Instantiate(signal, signal.transform.parent, false);
            signal.Setup(false);

            narratorBalloon.OnBalloonClicked.Subscribe(OnBalloonClicked);
            narratorBalloon.OnBalloonContinueClicked.Subscribe(OnBalloonContinueClicked);
            startEndPanel.OnBalloonClicked.Subscribe(OnBalloonClicked);
            startEndPanel.OnBalloonContinueClicked.Subscribe(OnBalloonContinueClicked);
            choices.OnChoiceConfirmed.Subscribe(OnChoiceConfirmed);
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
            DiscoverNotifier.Game.OnActClicked.Unsubscribe(OnActClicked);
        }

        #endregion

        #region Public Methods

        public void ShowStartPanel(QuestNode node, bool useLearningLanguage)
        {
            startEndPanel.Show(node, useLearningLanguage, true, 0);
        }

        public void ShowEndPanel(QuestNode node, bool useLearningLanguage, int totStarsAchieved)
        {
            startEndPanel.Show(node, useLearningLanguage, false, totStarsAchieved);
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

        public void ShowPostcard(CardData card, bool zoom = false, bool silent = false)
        {
            postcard.Show(card, zoom, silent);
        }

        public void ShowPostcard(AssetData assetData, bool zoom = false)
        {
            postcard.Show(assetData, zoom);
        }

        public void ShowPostcard(Sprite sprite)
        {
            postcard.OpenZoomView(sprite, null);
        }

        public void HidePostcard()
        {
            gotoNextWhenPostcardFocusViewCloses = false;
            postcard.Hide();
        }

        public void TogglePostcard(CardData card)
        {
            if (postcard.IsActive)
                HidePostcard();
            else
                ShowPostcard(card, true, false);
        }

        #endregion

        #region Methods

        public void ShowDialogueLine(QuestNode node)
        {
            //Debug.Log("DialoguesUI.ShowDialogueLine " + node.Content + " / " + node.ContentNative);

            IsOpen = true;
            currNode = node;
            currBalloon = narratorBalloon;

            if (QuestManager.I.LearningLangFirst)
            {
                learningLanguageFirst = true;
            }
            else
            {
                learningLanguageFirst = false;
            }
            if (node.Native)
            {
                learningLanguageFirst = false;
            }
            // Debug.Log("###### UseLearningLanguage: " + learningLanguageFirst);

            switch (node.Type)
            {
                case NodeType.TEXT:
                    CurrDialogueType = DialogueType.Text;
                    currBalloon.Show(node, learningLanguageFirst);
                    // image = node.GetImage();
                    if (node.Image != null)
                    {
                        DatabaseProvider.TryGet<CardData>(node.Image, out var cardData);
                        postcard.Show(cardData);
                    }
                    // else
                    //     postcard.Hide();
                    break;
                case NodeType.PANEL:
                    currBalloon = startEndPanel;
                    CurrDialogueType = DialogueType.Text;
                    ShowStartPanel(node, learningLanguageFirst);
                    break;
                case NodeType.PANEL_ENDGAME:
                    currBalloon = startEndPanel;
                    CurrDialogueType = DialogueType.Text;
                    ShowEndPanel(node, learningLanguageFirst, QuestManager.I.Progress.GetCurrentStarsAchieved());
                    break;
                case NodeType.CHOICE:
                case NodeType.QUIZ:
                    CurrDialogueType = DialogueType.Choice;
                    if (!string.IsNullOrEmpty(node.Content))
                        currBalloon.Show(node, learningLanguageFirst, false);
                    choices.Show(node.Choices, node.Type == NodeType.QUIZ, learningLanguageFirst);
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
            // if (gotoNextWhenPostcardFocusViewCloses)
            // {
            //     // Close postcard zoom and move onward
            //     if (postcard.IsMagnified)
            //     {
            //         postcard.CloseZoomView();
            //         yield return new WaitForSeconds(0.15f);
            //     }
            // }
            // else
            // {
            //     if (currNode.ImageAutoOpen && postcard.IsActive && !postcard.CurrSpriteWasMagnifiedOnce)
            //     {
            //         // Zoom into postcard and wait for next action
            //         postcard.OpenZoomView();
            //         gotoNextWhenPostcardFocusViewCloses = true;
            //         yield break;
            //     }
            // }

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

        #endregion

        #region Callbacks

        void OnActClicked()
        {
            // if (postcard.IsMagnified && !gotoNextWhenPostcardFocusViewCloses)
            if (postcard.IsMagnified)
            {
                postcard.CloseZoomView();
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
            // Debug.Log("OnBalloonClicked with " + UseLearningLanguage);
            currBalloon.RepeatText();
        }

        void OnBalloonContinueClicked()
        {
            Next();
        }

        void OnChoiceConfirmed(int choiceIndex)
        {
            Next(choiceIndex);
        }

        #endregion
    }
}
