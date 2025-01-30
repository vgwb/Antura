using System.Collections;
using System.Collections.Generic;
using Antura.Minigames.DiscoverCountry.Interaction;
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
        #region Serialized

        [Header("Interaction")]
        [Tooltip("Is it enabled for interaction?")]
        public bool IsInteractable = true;
        [Tooltip("Icon to be shown")]
        public InteractionType InteractionType = InteractionType.Look;
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
        
        #endregion
        
        public bool IsLL { get; private set; }
        public EdLivingLetter LL { get; private set; }

        void Awake()
        {
            // Store IconTransform if missing
            if (IconTransform == null) IconTransform = transform;
            
            // Store EdLivingLetter if present (so its methods like LookAt can be called on Act)
            EdLivingLetter ll = this.GetComponent<EdLivingLetter>();
            if (ll != null)
            {
                IsLL = true;
                LL = ll;
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

        public void OnTriggerEnter(Collider other)
        {
            if (IsInteractable)
            {
                if (other.gameObject == InteractionManager.I.player.gameObject)
                {
                    DiscoverNotifier.Game.OnInteractableEnteredByPlayer.Dispatch(this);
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == InteractionManager.I.player.gameObject)
            {
                DiscoverNotifier.Game.OnInteractableExitedByPlayer.Dispatch(this);
            }
        }
    }
}
