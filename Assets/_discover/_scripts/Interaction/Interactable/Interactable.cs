using System.Collections;
using System.Collections.Generic;
using Antura.Discover.Interaction;
using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.Events;
using Yarn;
using Yarn.Unity;
using Yarn.Unity.Attributes;

namespace Antura.Discover
{
    public enum InteractionType
    {
        None = 0,
        Look = 1,
        Talk = 2,
        Use = 4
    }

    public class Interactable : ActableAbstract
    {
        #region Serialized

        [Header("Interaction")]
        [Tooltip("Is it enabled for interaction?")]
        public bool IsInteractable = true;
        [Tooltip("Autorun when player gets nerby")]
        public bool NearbyAutoActivate;
        [Tooltip("Icon to be shown")]
        public InteractionType IconType = InteractionType.Look;
        [Tooltip("Where does the icon appear and camera focus?")]
        public Transform IconTransform;
        [Tooltip("Show on map (if active)")]
        public bool ShowOnMap;
        [Tooltip("Should the icon be always shown?")]
        public bool ShowIconAlways;
        [Tooltip("Camera focus on icon on interaction?")]
        public bool FocusCameraOnInteract;

        [Header("Execute Quest Node")]

        [Tooltip("Dialogue node to start")]
        [SerializeField] DialogueReference DialogueNode = new();
        [Tooltip("If DialogueNode is not set, you can this string to start a dialogue node")]
        public string NodePermalink;

        [Header("Execute Quest Actions")]
        [SerializeField] bool disableAfterAction;
        [Tooltip("Execute a quest action of ActionManager")]
        [SerializeField] string QuestAction;
        [Tooltip("Executes these commands")]
        [SerializeField] List<CommandData> Commands;
        #endregion

        public bool IsLL { get; private set; }
        public EdLivingLetter LL { get; private set; }
        public Transform LookAtTransform { get; private set; }
        Coroutine coDisableAfterAction;

        // Expose node name for debug visualizations
        public string DialogueNodeName
        {
            get
            {
                try
                { return DialogueNode != null ? DialogueNode.nodeName ?? string.Empty : string.Empty; }
                catch { return string.Empty; }
            }
        }

        #region Unity

        void Awake()
        {
            // Store IconTransform and LookAtTransform
            if (IconTransform == null)
                IconTransform = transform;
            LookAtTransform = IconTransform;

            // Store EdLivingLetter if present (so its methods like LookAt can be called on Act)
            EdLivingLetter ll = this.GetComponent<EdLivingLetter>();
            if (ll != null)
            {
                IsLL = true;
                LL = ll;
            }
        }

        void Start()
        {
            if (ShowIconAlways)
                InteractionManager.I.ShowPreviewSignalFor(this, true);
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
            if (InteractionManager.I != null)
                InteractionManager.I.ShowPreviewSignalFor(this, false);
        }

        // from ActableAbstract
        public override void OnTrigger()
        {
        }

        public void OnTriggerEnter(Collider other)
        {
            if (IsInteractable)
            {
                if (other.gameObject == InteractionManager.I.player.gameObject)
                {
                    DiscoverNotifier.Game.OnInteractableEnteredByPlayer.Dispatch(this);
                    if (NearbyAutoActivate)
                    {
                        NearbyAutoActivate = false;
                        DiscoverNotifier.Game.OnActClicked.Dispatch();
                    }
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == InteractionManager.I.player.gameObject)
                OnTriggerExitPlayer();
        }

        void OnTriggerExitPlayer()
        {
            DiscoverNotifier.Game.OnInteractableExitedByPlayer.Dispatch(this);
        }

        public void OnCollected()
        {
            IsInteractable = false;
            //Debug.Log("ITEM " + NodePermalink + " COLLECTED");
            // if (permalink == NodePermalink)
            // {
            //     Debug.Log("ITEM " + permalink + " COLLECTED");

            // }
        }

        #endregion

        #region Public Methods

        public void SetActivated(bool status)
        {
            IsInteractable = status;
            if (status)
            {
                if (ShowIconAlways)
                    InteractionManager.I.ShowPreviewSignalFor(this, true);
                DiscoverNotifier.Game.OnInteractableEnteredByPlayer.Dispatch(this);
            }
            else
            {
                InteractionManager.I.ShowPreviewSignalFor(this, false);
                DiscoverNotifier.Game.OnInteractableExitedByPlayer.Dispatch(this);
            }
        }

        /// <summary>
        /// Returns a <see cref="QuestNode"/> or NULL if there was no node to activate
        /// </summary>
        public QuestNode Execute()
        {
            QuestNode node = null;

            if (DialogueNode != null && DialogueNode.nodeName != "")
            {
                // node = DialogueNode.GetNode();
                // if (node == null)
                // {
                //     Debug.LogError($"Interactable: Execute - Node {DialogueNode.NodeName} not found in project {DialogueNode.Project.name}");
                //     return null;
                // }
                // Debug.Log($"Interactable: Execute - Node: {DialogueNode.NodeName}");
                QuestManager.I.StartDialogue(DialogueNode.nodeName);
            }

            if (NodePermalink != "")
            {
                Debug.Log($"Interactable: Execute - NodePermalink: {NodePermalink}");
                QuestManager.I.StartDialogue(NodePermalink);

            }

            if (QuestAction != "")
            {
                ActionManager.I.ResolveQuestAction(QuestAction);
            }

            if (Commands != null && Commands.Count > 0)
            {
                ActionManager.I.ResolveCommands(Commands);
            }

            if (disableAfterAction)
                this.RestartCoroutine(ref coDisableAfterAction, CO_DisableAfterAction());
            return node;
        }

        #endregion

        #region Methods

        // Coroutine to disable interactable after one frame, so it doesn't interfere with multiple actions being called in the same frame
        IEnumerator CO_DisableAfterAction()
        {
            yield return null;
            IsInteractable = false;
            InteractionManager.I.ShowPreviewSignalFor(this, false);
            OnTriggerExitPlayer();
        }

        #endregion
    }
}
