using System;
using Antura.Homer;
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

        #region Unity

        void Awake()
        {
            contentBox.SetActive(true);
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
                    Debug.LogError("DialoguesUI.ShowDialogueNode ► QuestNode is of invalid type");
                    break;
            }
        }

        public void CloseDialogue()
        {
            balloon.Hide();
            postcard.Hide();
            choices.Hide();
        }

        #endregion
    }
}