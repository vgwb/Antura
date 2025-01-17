using Antura.Audio;
using Antura.Core;
using Antura.Homer;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using Homer;
using System;
using System.Collections;
using Antura.Minigames.DiscoverCountry.Interaction;
using UnityEngine;

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

        QuestNode currNode;
        AbstractDialogueBalloon currBalloon;
        Coroutine coShowDialogue, coNext;
        bool SpeechCycle = false;

        #region Unity

        void Awake()
        {
            contentBox.SetActive(true);
            narratorBalloon.gameObject.SetActive(true);
            postcardFocusView.Hide();

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

        public void ShowSignalFor(EdAgent agent)
        {
            signal.ShowFor(agent);
        }
        public void ShowSignalFor(InfoPoint infoPoint)
        {
            signal.ShowFor(infoPoint);
        }

        public void HideSignal()
        {
            signal.Hide();
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
            while (InteractionManager.I.IsUsingFocusView)
                yield return null;
            Sprite image;
            if (node.Native)
            {
                SpeechCycle = false;
            }
            else
            {
                SpeechCycle = true;

            }
            switch (node.Type)
            {
                case HomerNode.NodeType.START:
                case HomerNode.NodeType.TEXT:
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
                case HomerNode.NodeType.CHOICE:
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
            // this.RestartCoroutine(ref coNext, CO_Next(choiceIndex));
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

            QuestNode next = currNode.NextNode(choiceIndex);
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
            // Debug.Log($"► Should play audio for main balloon");
            AudioManager.I.PlayDiscoverDialogue(
                 currNode.LocId,
                 SpeechCycle ? AppManager.I.AppSettings.NativeLanguage : AppManager.I.ContentEdition.LearningLanguage
            );
            SpeechCycle = !SpeechCycle;
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
