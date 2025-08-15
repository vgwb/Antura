using System;
using System.Collections;
using Antura.Discover.Interaction;
using DG.DeInspektor.Attributes;
using UnityEngine;
using Antura.UI;

namespace Antura.Discover
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
        [SerializeField] NavigatorMarker navigatorMarker;
        [DeEmptyAlert]
        [SerializeField] Navigator navigator;
        [DeEmptyAlert, Tooltip("Objects to hide when a dialogue starts")]
        [SerializeField] GameObject[] hideDuringDialogue;

        [Header("UI Elements")]
        [DeEmptyAlert]
        public ItemsCounter BonesCounter;
        [DeEmptyAlert]
        public ItemsCounter CoinsCounter;
        [DeEmptyAlert]
        public TaskDisplay TaskDisplay;
        [DeEmptyAlert]
        public ProgressDisplay ProgressDisplay;
        [DeEmptyAlert]
        public InventoryDisplay InventoryDisplay;
        [DeEmptyAlert]
        public ToastDisplay ToastDisplay;

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
            navigatorMarker.gameObject.SetActive(true);
            navigator.gameObject.SetActive(true);
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
            if (navigatorMarker.IsShown)
                navigator.SetIndicators(navigatorMarker.OutHor, navigatorMarker.OutVert);
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

            if (activate)
                navigatorMarker.Show(target);
            else
                navigatorMarker.Hide();
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
            if (InteractionManager.I.NearbyInteractable == null)
                btAct.gameObject.SetActive(false);
            foreach (GameObject go in hideDuringDialogue)
                go.gameObject.SetActive(true);
        }

        #endregion
    }
}
