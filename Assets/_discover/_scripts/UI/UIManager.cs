using System;
using System.Collections;
using Antura.Minigames.DiscoverCountry.Interaction;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class UIManager : MonoBehaviour
    {
        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] Canvas canvas;
        [DeEmptyAlert]
        [SerializeField] UIVirtualButton btAct;
        [DeEmptyAlert]
        [SerializeField] PlayerMapIcon playerMapIco;
        [DeEmptyAlert]
        [SerializeField] AnturaMapIcon anturaMapIco;
        [DeEmptyAlert]
        [SerializeField] TargetMarker targetMarker;
        [DeEmptyAlert]
        [SerializeField] TargetIndicator targetIndicator;
        [DeEmptyAlert, Tooltip("Objects to hide when a dialogue starts")]
        [SerializeField] GameObject[] hideDuringDialogue;

        #endregion

        public static UIManager I { get; private set; }
        public DialoguesUI dialogues { get; private set; }

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
            canvas.gameObject.SetActive(true);
            dialogues.gameObject.SetActive(true);
            btAct.gameObject.SetActive(false);
            playerMapIco.gameObject.SetActive(true);
            anturaMapIco.gameObject.SetActive(true);
            targetMarker.gameObject.SetActive(true);
        }

        void Start()
        {
            DiscoverNotifier.Game.OnAgentTriggerEnteredByPlayer.Subscribe(OnAgentTriggerEnter);
            DiscoverNotifier.Game.OnAgentTriggerExitedByPlayer.Subscribe(OnAgentTriggerExit);
            DiscoverNotifier.Game.OnStartDialogue.Subscribe(OnStartDialogue);
            DiscoverNotifier.Game.OnCloseDialogue.Subscribe(OnCloseDialogue);
            DiscoverNotifier.Game.OnMapCameraActivated.Subscribe(OnMapCameraActivated);
        }

        void OnDestroy()
        {
            if (I == this)
                I = null;
            this.StopAllCoroutines();
            DiscoverNotifier.Game.OnAgentTriggerEnteredByPlayer.Unsubscribe(OnAgentTriggerEnter);
            DiscoverNotifier.Game.OnAgentTriggerExitedByPlayer.Unsubscribe(OnAgentTriggerExit);
            DiscoverNotifier.Game.OnStartDialogue.Unsubscribe(OnStartDialogue);
            DiscoverNotifier.Game.OnCloseDialogue.Unsubscribe(OnCloseDialogue);
            DiscoverNotifier.Game.OnMapCameraActivated.Unsubscribe(OnMapCameraActivated);
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

        void OnAgentTriggerEnter(EdAgent agent)
        {
            btAct.gameObject.SetActive(true);
        }

        void OnAgentTriggerExit(EdAgent agent)
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
            if (InteractionManager.I.nearbyAgent == null)
                btAct.gameObject.SetActive(false);
            foreach (GameObject go in hideDuringDialogue)
                go.gameObject.SetActive(true);
        }

        void OnMapCameraActivated(bool activated)
        {
            if (activated)
            {
                playerMapIco.Show();
                anturaMapIco.Show();
            }
            else
            {
                playerMapIco.Hide();
                anturaMapIco.Hide();
            }
        }

        #endregion
    }
}
