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

        public EdAgent nearbyAgent { get; private set; }
        Coroutine coChangeLayer, coStartDialogue, coFocusCam;

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
            DiscoverNotifier.Game.OnAgentTriggerEnter.Subscribe(OnAgentTriggerEnter);
            DiscoverNotifier.Game.OnAgentTriggerExit.Subscribe(OnLivingLetterTriggerExit);
            DiscoverNotifier.Game.OnActClicked.Subscribe(OnActClicked);
        }

        void OnDestroy()
        {
            if (I == this)
                I = null;
            this.StopAllCoroutines();
            DiscoverNotifier.Game.OnCloseDialogue.Unsubscribe(OnCloseDialogue);
            DiscoverNotifier.Game.OnAgentTriggerEnter.Unsubscribe(OnAgentTriggerEnter);
            DiscoverNotifier.Game.OnAgentTriggerExit.Unsubscribe(OnLivingLetterTriggerExit);
        }

        #endregion

        #region Update

        void Update()
        {
            switch (Layer)
            {
                case InteractionLayer.Dialogue:
                    UpdateDialogue();
                    break;
            }
            
            // // DEBUG
            // if (Input.GetKeyDown(KeyCode.Q)) FocusCameraOn(debugFocusTarget);
        }
        
        void UpdateDialogue()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) ExitDialogue();
        }

        #endregion

        #region Public Methods


        public void OnAct(InputValue value)
        {
            Act();
        }

        public void StartInfoPointDialogue(InfoPoint infoPoint, QuestNode questNode)
        {
            this.RestartCoroutine(ref coStartDialogue, CO_StartDialogue(questNode, infoPoint));
        }

        public void FocusCameraOn(Transform target)
        {
            this.RestartCoroutine(ref coFocusCam, CO_FocusCamOn(target));
        }

        #endregion

        #region Methods

        void Act()
        {
            if (Layer != InteractionLayer.World) return;
            if (nearbyAgent != null)
            {
                // Start dialogue with LL
                QuestNode questNode = QuestManager.I.GetQuestNode(nearbyAgent.ActorId);
                this.RestartCoroutine(ref coStartDialogue, CO_StartDialogue(questNode));
            }
        }

        void ChangeLayer(InteractionLayer newLayer)
        {
            if (newLayer == Layer) return;
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
            if (!isInfoPointMode) nearbyAgent.LookAt(player.transform);
            CameraManager.I.ChangeCameraMode(CameraMode.Dialogue);
            CameraManager.I.FocusDialogueCamOn(isInfoPointMode ? infoPoint.transform : nearbyAgent.transform);
            UIManager.I.dialogues.HideDialogueSignal();
            if (questNode == null)
                Debug.LogError("QuestNode is NULL, shouldn't happen");
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
            if (nearbyAgent != null)
                UIManager.I.dialogues.ShowDialogueSignalFor(nearbyAgent);
            this.CancelCoroutine(ref coStartDialogue);
            UIManager.I.dialogues.CloseDialogue();
        }

        IEnumerator CO_FocusCamOn(Transform target)
        {
            IsUsingFocusView = true;
            CameraManager.I.FocusCamOn(target);
            CameraManager.I.ChangeCameraMode(CameraMode.Focus);
            UIManager.I.gameObject.SetActive(false);
            
            while (!Input.GetMouseButtonDown(0)) yield return null;
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
            nearbyAgent = ll;
            UIManager.I.dialogues.ShowDialogueSignalFor(nearbyAgent);
        }

        void OnLivingLetterTriggerExit(EdAgent ll)
        {
            if (nearbyAgent == ll)
            {
                nearbyAgent = null;
                UIManager.I.dialogues.HideDialogueSignal();
            }
        }

        #endregion
    }
}
