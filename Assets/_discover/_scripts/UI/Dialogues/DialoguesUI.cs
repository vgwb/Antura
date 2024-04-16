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
            Text,
            Choice,
            Quiz
        }
        
        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] DialogueSignal signal;
        [DeEmptyAlert]
        [SerializeField] GameObject contentBox;
        [DeEmptyAlert]
        [SerializeField] DialogueBalloon balloon;
        [DeEmptyAlert]
        [SerializeField] DialogueChoices choices;
        [DeEmptyAlert]
        [SerializeField] DialoguePostcard postcard;

        #endregion
        
        public bool IsOpen { get; private set; }

        QuestNode currNode;
        Coroutine coShowDialogue, coNext;

        #region Unity

        void Awake()
        {
            contentBox.SetActive(true);
            
            balloon.OnBalloonClicked.Subscribe(OnBalloonClicked);
            choices.OnChoiceSelected.Subscribe(OnChoiceSelected);
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
            balloon.OnBalloonClicked.Unsubscribe(OnBalloonClicked);
            choices.OnChoiceSelected.Unsubscribe(OnChoiceSelected);
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
            if (!IsOpen) return;
            
            IsOpen = false;
            balloon.Hide();
            postcard.Hide();
            if (choices.IsOpen) choices.Hide(choiceIndex);
            DiscoverNotifier.Game.OnCloseDialogue.Dispatch();
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
            switch (node.Type)
            {
                case HomerNode.NodeType.TEXT:
                    balloon.Show(node);
                    yield return new WaitForSeconds(0.2f);
                    postcard.Show();
                    break;
                case HomerNode.NodeType.CHOICE:
                    if (!string.IsNullOrEmpty(node.Content))
                    {
                        balloon.Show(node);
                        yield return new WaitForSeconds(0.2f);
                    }
                    postcard.Show();
                    yield return new WaitForSeconds(0.3f);
                    choices.Show(node.Choices);
                    break;
                default:
                    IsOpen = false;
                    Debug.LogError("DialoguesUI.ShowDialogueNode ► QuestNode is of invalid type");
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
            if (balloon.IsOpen) balloon.Hide();
            if (choices.IsOpen)
            {
                choices.Hide(choiceIndex);
                yield return new WaitForSeconds(0.35f);
            }
            
            QuestNode next = currNode.NextNode(choiceIndex);
            if (next == null) CloseDialogue(choiceIndex);
            else ShowDialogueFor(next);
            coNext = null;
        }

        #endregion

        #region Callbacks
        
        void OnBalloonClicked()
        {
            Next();
        }

        void OnChoiceSelected(int choiceIndex)
        {
            Next(choiceIndex);
        }

        #endregion
    }
}