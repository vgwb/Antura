using System.Collections;
using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Antura.Minigames.DiscoverCountry
{
    public enum InteractionType
    {
        None = 0,
        Look = 1,
        Talk = 2,
        Use = 4
    }

    public class Interactable : MonoBehaviour
    {
        [Header("Interaction")]
        [Tooltip("Is it enaabled for interaction?")]
        public bool IsInteractable;
        [Tooltip("Icon to be shown")]
        public InteractionType InteractionType;
        [Tooltip("Where does the icon appear and camera focus?")]
        public Transform IconTransform;
        [Tooltip("Should the icon be always shown?")]
        public bool ShowIconAlways;
        [Tooltip("Camera focus on icon on interaction?")]
        public bool FocusCameraOnInteract;

        [Header("Quest Node")]
        public bool ActivateNode;
        [DeConditional("ActivateNode", true, behaviour:ConditionalBehaviour.Hide)]
        public string NodePermalink;
        [DeConditional("ActivateNode", true, behaviour:ConditionalBehaviour.Hide)]
        public string NodeCommand;

        [Header("Unity Action")]
        public bool ActivateUnityAction;
        [DeConditional("ActivateUnityAction", true, behaviour:ConditionalBehaviour.Hide)]
        [SerializeField] bool disableAfterAction;
        [DeConditional("ActivateUnityAction", true, behaviour:ConditionalBehaviour.Hide)]
        [SerializeField] UnityEvent unityAction;

        void Start()
        {
            if (IconTransform == null)
            {
                IconTransform = transform;
            }
        }

        /// <summary>
        /// Returns a <see cref="QuestNode"/> or NULL if there was no node to activate 
        /// </summary>
        public QuestNode Activate()
        {
            QuestNode node = null;
            if (ActivateNode) node = QuestManager.I.GetQuestNode(NodePermalink, NodeCommand);
            if (ActivateUnityAction) LaunchUnityAction();
            return node;
        }

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void LaunchUnityAction()
        {
            if (unityAction != null)
                unityAction.Invoke();
        }

    }
}
