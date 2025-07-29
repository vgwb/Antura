using System;
using System.Collections;
using Antura.Minigames.DiscoverCountry.Interaction;
using DG.DeInspektor.Attributes;
using UnityEngine;
using Antura.UI;

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
        [SerializeField] NavigatorMarker navigatorMarker;
        [DeEmptyAlert]
        [SerializeField] Navigator navigator;
        [DeEmptyAlert, Tooltip("Objects to hide when a dialogue starts")]
        [SerializeField] GameObject[] hideDuringDialogue;

        [Header("UI Elements")]
        public BonesCounter bonesCounter;
        public BonesCounter coinsCounter;
        public ItemsCounter itemsCounter;
        public ProgressCounter progressCounter;
        public GameObject QuestObjective;
        public GameObject InventoryItem;
        public GameObject ActivityContainer;

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

            // init widgets
            if (coinsCounter == null)
            {
                coinsCounter = GameObject.Find("CoinsCounter").GetComponent<BonesCounter>();
            }
            if (bonesCounter == null)
            {
                bonesCounter = GameObject.Find("BonesCounter").GetComponent<BonesCounter>();
            }
            if (itemsCounter == null)
            {
                itemsCounter = GameObject.Find("ItemsCounter").GetComponent<ItemsCounter>();
                itemsCounter.gameObject.SetActive(false);
            }

            if (progressCounter == null)
            {
                progressCounter = GameObject.Find("ProgressCounter").GetComponent<ProgressCounter>();
            }

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
            if (InteractionManager.I.nearbyInteractable == null)
                btAct.gameObject.SetActive(false);
            foreach (GameObject go in hideDuringDialogue)
                go.gameObject.SetActive(true);
        }

        #endregion
    }
}
