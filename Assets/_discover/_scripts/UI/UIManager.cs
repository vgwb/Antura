using System;
using System.Collections;
using Antura.Minigames.DiscoverCountry.Interaction;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    [RequireComponent(typeof(MapIconsManager))]
    public class UIManager : MonoBehaviour
    {
        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] Canvas canvas;
        [DeEmptyAlert]
        [SerializeField] UIVirtualButton btAct;
        [DeEmptyAlert]
        [SerializeField] TargetMarker targetMarker;
        [DeEmptyAlert]
        [SerializeField] TargetIndicator targetIndicator;
        [DeEmptyAlert, Tooltip("Objects to hide when a dialogue starts")]
        [SerializeField] GameObject[] hideDuringDialogue;

        #endregion

        public static UIManager I { get; private set; }
        public DialoguesUI dialogues { get; private set; }
        public MapIconsManager MapIcons { get; private set; }

        #region Unity

        void Awake()
        {
            if (I != null)
            {
                Debug.LogError("UIManager already exists, deleting duplicate");
                Destroy(this);
                return;
            }

            I = this;
            dialogues = this.GetComponentInChildren<DialoguesUI>(true);
            MapIcons = this.GetComponent<MapIconsManager>();
            canvas.gameObject.SetActive(true);
            dialogues.gameObject.SetActive(true);
            btAct.gameObject.SetActive(false);
            targetMarker.gameObject.SetActive(true);
            targetIndicator.gameObject.SetActive(true);
        }

        void Start()
        {
            DiscoverNotifier.Game.OnInteractableEnteredByPlayer.Subscribe(OnInteractableEnteredByPlayer);
            DiscoverNotifier.Game.OnInteractableExitedByPlayer.Subscribe(OnInteractableExitedByPlayer);
            DiscoverNotifier.Game.OnStartDialogue.Subscribe(OnStartDialogue);
            DiscoverNotifier.Game.OnCloseDialogue.Subscribe(OnCloseDialogue);
        }

        void OnDestroy()
        {
            if (I == this)
                I = null;
            this.StopAllCoroutines();
            DiscoverNotifier.Game.OnInteractableEnteredByPlayer.Unsubscribe(OnInteractableEnteredByPlayer);
            DiscoverNotifier.Game.OnInteractableExitedByPlayer.Unsubscribe(OnInteractableExitedByPlayer);
            DiscoverNotifier.Game.OnStartDialogue.Unsubscribe(OnStartDialogue);
            DiscoverNotifier.Game.OnCloseDialogue.Unsubscribe(OnCloseDialogue);
        }

        void LateUpdate()
        {
            if (targetMarker.IsShown) targetIndicator.SetIndicators(targetMarker.OutHor, targetMarker.OutVert);
        }

        #endregion

        #region Public Methods

        public void ActivateWorldTargetMarker(bool activate, Transform target = null)
        {
            if (activate && target == null)
            {
                Debug.LogError("UIManager: you can't call ActivateWorldTargetIcon(TRUE) without passing a Transform");
                return;
            }
            
            if (activate) targetMarker.Show(target);
            else targetMarker.Hide();
        }

        #endregion

        #region Callbacks

        void OnInteractableEnteredByPlayer(Interactable interactable)
        {
            btAct.gameObject.SetActive(true);
        }

        void OnInteractableExitedByPlayer(Interactable interactable)
        {
            btAct.gameObject.SetActive(false);
        }

        void OnStartDialogue()
        {
            btAct.gameObject.SetActive(true);
            foreach (GameObject go in hideDuringDialogue)
                go.gameObject.SetActive(false);
        }

        void OnCloseDialogue()
        {
            if (InteractionManager.I.nearbyInteractable == null)
                btAct.gameObject.SetActive(false);
            foreach (GameObject go in hideDuringDialogue)
                go.gameObject.SetActive(true);
        }

        #endregion
    }
}
