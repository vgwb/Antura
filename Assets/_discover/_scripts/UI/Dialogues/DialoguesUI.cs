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
        public DialogueType CurrDialogueType { get; private set; }

        DialogueSignal previewSignalPrefab;
        readonly Dictionary<Interactable, DialogueSignal> previewSignalByInteractable = new();
        QuestNode currNode;
        AbstractDialogueBalloon currBalloon;
        Coroutine coShowDialogue, coNext;
        bool UseLearningLanguage = true;

        #region Unity

        void Awake()
        {
            contentBox.SetActive(true);
            narratorBalloon.gameObject.SetActive(true);
            postcardFocusView.Hide();
            previewSignalPrefab = Instantiate(signal, signal.transform.parent, false);
            signal.Setup(false);

            narratorBalloon.OnBalloonClicked.Subscribe(OnBalloonClicked);
            narratorBalloon.OnBalloonContinueClicked.Subscribe(OnBalloonContinueClicked);
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
            choices.OnChoiceConfirmed.Unsubscribe(OnChoiceConfirmed);
            postcard.OnClicked.Unsubscribe(OnPostcardClicked);
            postcardFocusView.OnClicked.Unsubscribe(OnPostcardFocusViewClicked);
            DiscoverNotifier.Game.OnActClicked.Unsubscribe(OnActClicked);
        }

        #endregion

        #region Public Methods

        public void ShowSignalFor(Interactable interactable)
        {
            if (previewSignalByInteractable.ContainsKey(interactable)) previewSignalByInteractable[interactable].Hide(true);
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
            if (interactable == null) return;
            
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
            //Debug.Log("TOTAL ECONS " + HomerVars.TOTAL_COINS);
            // this.RestartCoroutine(ref coShowDialogue, CO_ShowDialogueFor(node));
            CoroutineRunner.RestartCoroutine(ref coShowDialogue, CO_ShowDialogueFor(node));
        }

        IEnumerator CO_ShowDialogueFor(QuestNode node)
        {
            IsOpen = true;
            currNode = node;
            currBalloon = narratorBalloon; // TODO : Assign correct balloon
            while (InteractionManager.I.IsUsingFocusView) yield return null;
            
            Sprite image;
            UseLearningLanguage = !node.Native;
            switch (node.Type)
            {
                case NodeType.TEXT:
                    CurrDialogueType = DialogueType.Text;
                    currBalloon.Show(node);
                    // yield return new WaitForSeconds(0.2f);
                    image = node.GetImage();
                    if (image != null)
                    {
                        postcard.Show(image);
                    }
                    else
                    {
                        postcard.Hide();
                    }
                    break;
                case NodeType.CHOICE:
                case NodeType.QUIZ:
                    CurrDialogueType = DialogueType.Choice;
                    if (!string.IsNullOrEmpty(node.Content))
                    {
                        currBalloon.Show(node);
                        // yield return new WaitForSeconds(0.2f);
                    }
                    image = node.GetImage();
                    if (image != null)
                    {
                        postcard.Show(image);
                    }
                    else
                    {
                        postcard.Hide();
                    }
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
            if (currBalloon != null && currBalloon.IsOpen)
                currBalloon.Hide();

            if (choices.IsOpen)
            {
                choices.Hide(choiceIndex);
                while (choices.IsHiding)
                    yield return null;
                // yield return new WaitForSeconds(0.35f);
            }

            QuestNode next = QuestManager.I.GetNextNode(choiceIndex);
            if (next == null)
                CloseDialogue(choiceIndex);
            else
                ShowDialogueFor(next);

            coNext = null;
        }

        #endregion

        #region Callbacks

        void OnActClicked()
        {
            if (CurrDialogueType == DialogueType.Text && !InteractionManager.I.IsUsingFocusView)
            {
                Next();
            }
        }

        void OnBalloonClicked()
        {
            // Play/repeat alternate audio here
            UseLearningLanguage = !UseLearningLanguage;
            AudioManager.I.PlayDiscoverDialogue(
                 currNode.AudioId,
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

        void OnPostcardClicked(Sprite sprite)
        {
            postcardFocusView.Show(sprite);
        }

        void OnPostcardFocusViewClicked()
        {
            postcardFocusView.Hide();
        }

        #endregion
    }
}
