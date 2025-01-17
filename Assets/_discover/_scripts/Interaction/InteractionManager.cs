using System.Collections;
using Antura.Homer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Antura.Minigames.DiscoverCountry.Interaction
{
    public class InteractionManager : MonoBehaviour
    {
        #region Serialized

        public EdPlayer player;
        // [Header("Debug References")]
        // [SerializeField] Transform debugFocusTarget;

        #endregion

        public static InteractionManager I { get; private set; }
        public InteractionLayer Layer { get; private set; }
        public bool IsUsingFocusView { get; private set; }
        public bool HasValidNearbyAgent => nearbyAgent != null && nearbyAgent.gameObject.activeInHierarchy;
        public bool HasValidNearbyInfoPoint => nearbyInfoPoint != null && nearbyInfoPoint.gameObject.activeInHierarchy;

        public EdAgent nearbyAgent { get; private set; }
        public InfoPoint nearbyInfoPoint { get; private set; }
        string nearbyInfoPointNodeId;
        string nearbyInfoPointNodeCommand;
        int focusViewEnterFrame;
        Coroutine coChangeLayer, coStartDialogue;

        #region Unity

        void Awake()
        {
            if (I != null)
            {
                Debug.LogError("InteractionManager already exists, deleting duplicate");
                Destroy(this);
                return;
            }

            I = this;
        }

        void Start()
        {
            Layer = InteractionLayer.World;
            //            player = FindObjectOfType<EdAntura>(true);

            DiscoverNotifier.Game.OnCloseDialogue.Subscribe(OnCloseDialogue);
            DiscoverNotifier.Game.OnAgentTriggerEnteredByPlayer.Subscribe(OnAgentTriggerEnter);
            DiscoverNotifier.Game.OnAgentTriggerExitedByPlayer.Subscribe(OnAgentTriggerExit);
            DiscoverNotifier.Game.OnInfoPointTriggerEnteredByPlayer.Subscribe(OnInfoPointTriggerEnter);
            DiscoverNotifier.Game.OnInfoPointTriggerExitedByPlayer.Subscribe(OnInfoPointTriggerExit);
            DiscoverNotifier.Game.OnActClicked.Subscribe(OnActClicked);
        }

        void OnDestroy()
        {
            if (I == this)
                I = null;
            this.StopAllCoroutines();
            DiscoverNotifier.Game.OnCloseDialogue.Unsubscribe(OnCloseDialogue);
            DiscoverNotifier.Game.OnAgentTriggerEnteredByPlayer.Unsubscribe(OnAgentTriggerEnter);
            DiscoverNotifier.Game.OnAgentTriggerExitedByPlayer.Unsubscribe(OnAgentTriggerExit);
            DiscoverNotifier.Game.OnInfoPointTriggerEnteredByPlayer.Unsubscribe(OnInfoPointTriggerEnter);
            DiscoverNotifier.Game.OnInfoPointTriggerExitedByPlayer.Unsubscribe(OnInfoPointTriggerExit);
            DiscoverNotifier.Game.OnActClicked.Unsubscribe(OnActClicked);
        }

        #endregion

        #region Update

        void Update()
        {
            switch (Layer)
            {
                case InteractionLayer.World:
                    UpdateWorld();
                    break;
                case InteractionLayer.Dialogue:
                    UpdateDialogue();
                    break;
            }

            // // DEBUG
            // if (Input.GetKeyDown(KeyCode.Q)) FocusCameraOn(debugFocusTarget);
        }

        void UpdateWorld()
        {
            if (nearbyAgent != null && !HasValidNearbyAgent)
            {
                DiscoverNotifier.Game.OnAgentTriggerExitedByPlayer.Dispatch(nearbyAgent);
            }
            if (nearbyInfoPoint != null && !HasValidNearbyInfoPoint)
            {
                DiscoverNotifier.Game.OnInfoPointTriggerExitedByPlayer.Dispatch(nearbyInfoPoint);
            }
        }

        void UpdateDialogue()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                ExitDialogue();
            if (IsUsingFocusView && Input.GetMouseButtonDown(0))
                UnfocusCam();
        }

        #endregion

        #region Public Methods

        public void FocusCameraOn(Transform target)
        {
            IsUsingFocusView = true;
            focusViewEnterFrame = Time.frameCount;
            CameraManager.I.FocusCamOn(target);
            CameraManager.I.ChangeCameraMode(CameraMode.Focus);
            UIManager.I.gameObject.SetActive(false);
        }

        /// <summary>
        /// Activates the world icon for the light beam
        /// </summary>
        /// <param name="activate">TRUE to activate, FALSE otherwise</param>
        /// <param name="target">Required only if activating, the target to follow</param>
        public void ActivateWorldTargetIcon(bool activate, Transform target = null)
        {
            UIManager.I.ActivateWorldTargetMarker(activate, target);
        }

        #endregion

        #region Methods

        void Act()
        {
            if (IsUsingFocusView && focusViewEnterFrame != Time.frameCount)
            {
                UnfocusCam();
            }
            if (Layer != InteractionLayer.World)
            {
                return;
            }
            if (HasValidNearbyAgent)
            {
                // Start dialogue with LL
                string command = "TALK_" + nearbyAgent.ActorId.ToString();
                if (nearbyAgent.SubCommand != "")
                {
                    command += "_" + nearbyAgent.SubCommand;
                }
                Debug.Log("command: " + command);
                QuestNode questNode = QuestManager.I.GetQuestNodeByCommand(command);
                this.RestartCoroutine(ref coStartDialogue, CO_StartDialogue(questNode));
            }
            else if (HasValidNearbyInfoPoint)
            {
                // Start info dialogue
                //Debug.Log("nearbyInfoPointNodeId: " + nearbyInfoPointNodeId);
                // QuestNode questNode = QuestManager.I.GetQuestNodeByPermalink(nearbyInfoPointNodeId);
                QuestNode questNode = QuestManager.I.GetQuestNodeByCommand(nearbyInfoPointNodeCommand);
                this.RestartCoroutine(ref coStartDialogue, CO_StartDialogue(questNode, nearbyInfoPoint));
            }
        }

        void ChangeLayer(InteractionLayer newLayer)
        {
            if (newLayer == Layer)
                return;
            this.RestartCoroutine(ref coChangeLayer, CO_ChangeLayer(newLayer));
        }
        IEnumerator CO_ChangeLayer(InteractionLayer newLayer)
        {
            Layer = InteractionLayer.Changing;
            yield return null;
            Layer = newLayer;
        }

        IEnumerator CO_StartDialogue(QuestNode questNode, InfoPoint infoPoint = null)
        {
            bool isInfoPointMode = infoPoint != null;
            ChangeLayer(InteractionLayer.Dialogue);
            DiscoverNotifier.Game.OnStartDialogue.Dispatch();

            if (!isInfoPointMode)
            {
                nearbyAgent.LookAt(player.transform);
            }

            CameraManager.I.ChangeCameraMode(CameraMode.Dialogue);
            CameraManager.I.FocusDialogueCamOn(isInfoPointMode ? infoPoint.transform : nearbyAgent.transform);
            UIManager.I.dialogues.HideSignal();

            if (questNode == null)
            {
                Debug.LogError("QuestNode is NULL, shouldn't happen");
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                UIManager.I.dialogues.StartDialogue(questNode);
            }
            coStartDialogue = null;
        }

        void ExitDialogue()
        {
            ChangeLayer(InteractionLayer.World);
            CameraManager.I.ChangeCameraMode(CameraMode.Player);
            if (HasValidNearbyAgent)
            {
                UIManager.I.dialogues.ShowSignalFor(nearbyAgent);
            }
            else if (HasValidNearbyInfoPoint)
            {
                UIManager.I.dialogues.ShowSignalFor(nearbyInfoPoint);
            }
            this.CancelCoroutine(ref coStartDialogue);
            UIManager.I.dialogues.CloseDialogue();
        }

        void UnfocusCam()
        {
            IsUsingFocusView = false;
            CameraManager.I.ChangeCameraMode(CameraMode.Dialogue);
            UIManager.I.gameObject.SetActive(true);
        }

        #endregion

        #region Callbacks

        void OnCloseDialogue()
        {
            ExitDialogue();
        }

        void OnActClicked()
        {
            Act();
        }

        void OnAgentTriggerEnter(EdAgent ll)
        {
            //            Debug.Log($"Enter {ll}", ll);
            nearbyAgent = ll;
            UIManager.I.dialogues.ShowSignalFor(nearbyAgent);
        }

        void OnAgentTriggerExit(EdAgent ll)
        {
            //            Debug.Log($"Exit {ll} ({ll == nearbyAgent})", ll);
            if (nearbyAgent == ll)
            {
                nearbyAgent = null;
                UIManager.I.dialogues.HideSignal();
            }
        }

        void OnInfoPointTriggerEnter(InfoPoint infoPoint, string nodeId, string command)
        {
            //            Debug.Log($"Enter {infoPoint}", infoPoint);
            nearbyInfoPoint = infoPoint;
            nearbyInfoPointNodeId = nodeId;
            nearbyInfoPointNodeCommand = command;
            UIManager.I.dialogues.ShowSignalFor(infoPoint);
        }

        void OnInfoPointTriggerExit(InfoPoint infoPoint)
        {
            //            Debug.Log($"Exit {infoPoint} ({infoPoint == nearbyInfoPoint})", infoPoint);
            if (nearbyInfoPoint == infoPoint)
            {
                nearbyInfoPoint = null;
                nearbyInfoPointNodeId = null;
                nearbyInfoPointNodeCommand = null;
                UIManager.I.dialogues.HideSignal();
            }
        }

        #endregion
    }
}
