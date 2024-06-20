using System;
using System.Collections;
using Antura.Homer;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using Homer;
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
        [SerializeField] SpeechBalloon speechBalloon;
        [DeEmptyAlert]
        [SerializeField] DialogueChoices choices;
        [DeEmptyAlert]
        [SerializeField] DialoguePostcard postcard;

        #endregion

        public bool IsOpen { get; private set; }
        public DialogueType CurrDialogueType { get; private set; }

        QuestNode currNode;
        AbstractDialogueBalloon currBalloon;
        Coroutine coShowDialogue, coNext;

        #region Unity

        void Awake()
        {
            contentBox.SetActive(true);
            narratorBalloon.gameObject.SetActive(true);
            speechBalloon.gameObject.SetActive(true);

            narratorBalloon.OnBalloonClicked.Subscribe(OnBalloonClicked);
            speechBalloon.OnBalloonClicked.Subscribe(OnBalloonClicked);
            choices.OnChoiceConfirmed.Subscribe(OnChoiceConfirmed);
            DiscoverNotifier.Game.OnActClicked.Subscribe(OnActClicked);
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
            narratorBalloon.OnBalloonClicked.Unsubscribe(OnBalloonClicked);
            speechBalloon.OnBalloonClicked.Unsubscribe(OnBalloonClicked);
            choices.OnChoiceConfirmed.Unsubscribe(OnChoiceConfirmed);
            DiscoverNotifier.Game.OnActClicked.Unsubscribe(OnActClicked);
        }

        #endregion

        #region Public Methods

        public void ShowDialogueSignalFor(EdAgent agent)
        {
            signal.ShowFor(agent);
        }

        public void HideDialogueSignal()
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
            this.RestartCoroutine(ref coShowDialogue, CO_ShowDialogueFor(node));
        }

        IEnumerator CO_ShowDialogueFor(QuestNode node)
        {
            IsOpen = true;
            currNode = node;
            currBalloon = narratorBalloon; // TODO : Assign correct balloon
            Sprite image;
            switch (node.Type)
            {
                case HomerNode.NodeType.START:
                case HomerNode.NodeType.TEXT:
                    CurrDialogueType = DialogueType.Text;
                    currBalloon.Show(node);
                    // yield return new WaitForSeconds(0.2f);
                    image = node.GetImage();
                    if (image != null) postcard.Show(image);
                    break;
                case HomerNode.NodeType.CHOICE:
                    CurrDialogueType = DialogueType.Choice;
                    if (!string.IsNullOrEmpty(node.Content))
                    {
                        currBalloon.Show(node);
                        // yield return new WaitForSeconds(0.2f);
                    }
                    image = node.GetImage();
                    if (image != null) postcard.Show(image);
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
            this.RestartCoroutine(ref coNext, CO_Next(choiceIndex));
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
            if (CurrDialogueType == DialogueType.Text) Next();
        }
        
        void OnBalloonClicked()
        {
            // Play/repeat alternate audio here
            Debug.Log($"► Should play audio for main balloon");
        }

        void OnChoiceConfirmed(int choiceIndex)
        {
            Next(choiceIndex);
        }

        #endregion
    }
}
