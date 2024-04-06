using System;
using Antura.Homer;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using Homer;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class DialoguesUI : MonoBehaviour
    {
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

        #region Unity

        void Awake()
        {
            contentBox.SetActive(true);
            
            choices.OnChoiceSelected.Subscribe(OnChoiceSelected);
        }

        void OnDestroy()
        {
            choices.OnChoiceSelected.Unsubscribe(OnChoiceSelected);
        }

        #endregion
        
        #region Public Methods

        public void ShowDialogueSignalFor(EdLivingLetter ll)
        {
            signal.ShowFor(ll);
        }

        public void HideDialogueSignal()
        {
            signal.Hide();
        }

        public void StartDialogue(QuestNode node)
        {
            IsOpen = true;
            switch (node.Type)
            {
                case HomerNode.NodeType.TEXT:
                    balloon.Show(node.Content);
                    postcard.Show();
                    break;
                case HomerNode.NodeType.CHOICE:
                    if (!string.IsNullOrEmpty(node.Content)) balloon.Show(node.Content);
                    postcard.Show();
                    choices.Show(node.Choices);
                    break;
                default:
                    IsOpen = false;
                    Debug.LogError("DialoguesUI.ShowDialogueNode ► QuestNode is of invalid type");
                    break;
            }
        }

        public void CloseDialogue(int choiceIndex = -1)
        {
            if (!IsOpen) return;
            
            IsOpen = false;
            balloon.Hide();
            postcard.Hide();
            choices.Hide(choiceIndex);
            DiscoverNotifier.Game.OnCloseDialogue.Dispatch();
        }

        #endregion

        #region Callbacks

        void OnChoiceSelected(int choiceIndex)
        {
            CloseDialogue(choiceIndex);
        }

        #endregion
    }
}