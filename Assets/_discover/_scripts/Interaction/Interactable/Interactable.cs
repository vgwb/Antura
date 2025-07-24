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
        [Tooltip("Autorun when player gets nerby")]
        public bool AutoActivate;
        [Tooltip("Icon to be shown")]
        public InteractionType InteractionType = InteractionType.Look;
        [Tooltip("Where does the icon appear and camera focus?")]
        public Transform IconTransform;
        [Tooltip("Show on map (if active)")]
        public bool ShowOnMap;
        [Tooltip("Should the icon be always shown?")]
        public bool ShowIconAlways;
        [Tooltip("Camera focus on icon on interaction?")]
        public bool FocusCameraOnInteract;

        [Header("Unity Actions")]
        public bool ActivateUnityAction;
        [SerializeField] bool disableAfterAction;
        [SerializeField] UnityEvent unityAction;

        [Header("Quest Actions")]
        [SerializeField] QuestActionData QuestAction;

        [Header("Quest Node")]
        public bool ActivateNode;
        public string NodePermalink;
        public string NodeCommand;

        #endregion

        public bool IsLL { get; private set; }
        public EdLivingLetter LL { get; private set; }
        public Transform LookAtTransform { get; private set; }
        Coroutine coDisableAfterAction;

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

        public void OnTriggerEnter(Collider other)
        {
            if (IsInteractable)
            {
                if (other.gameObject == InteractionManager.I.player.gameObject)
                {
                    DiscoverNotifier.Game.OnInteractableEnteredByPlayer.Dispatch(this);
                    if (AutoActivate)
                    {
                        AutoActivate = false;
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

        /// <summary>
        /// Returns a <see cref="QuestNode"/> or NULL if there was no node to activate
        /// </summary>
        public QuestNode Activate()
        {
            QuestNode node = null;
            if (ActivateNode)
                node = QuestManager.I.GetQuestNode(NodePermalink, NodeCommand);
            if (ActivateUnityAction)
                LaunchUnityAction();
            if (disableAfterAction)
                this.RestartCoroutine(ref coDisableAfterAction, CO_DisableAfterAction());
            return node;
        }

        #endregion

        #region Methods

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void LaunchUnityAction()
        {
            if (unityAction != null)
                unityAction.Invoke();
        }

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
