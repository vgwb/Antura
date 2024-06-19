using System.Collections;
using Antura.Homer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Antura.Minigames.DiscoverCountry.Interaction
{
    public class InteractionManager : MonoBehaviour
    {
        public static InteractionManager I { get; private set; }
        public InteractionLayer Layer { get; private set; }

        public EdPlayer player;
        public EdAgent nearbyAgent;
        Coroutine coStartDialogue;

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
            DiscoverNotifier.Game.OnActPressed.Subscribe(OnActPressed);
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
        }

        #endregion

        #region Update Methods


        public void OnAct(InputValue value)
        {
            Act();
        }

        void UpdateWorld()
        {
            // if (Input.GetKeyDown(KeyCode.E) && nearbyAgent != null)
            // {
            //     // Start dialogue with LL
            //     this.RestartCoroutine(ref coStartDialogue, CO_StartDialogue());
            // }
        }

        void UpdateDialogue()
        {
            // if (Input.GetKeyDown(KeyCode.Escape)) ExitDialogue();
        }

        #endregion

        #region Methods

        void Act()
        {
            if (Layer != InteractionLayer.World) return;
            if (nearbyAgent != null)
            {
                // Start dialogue with LL
                this.RestartCoroutine(ref coStartDialogue, CO_StartDialogue());
            }
        }

        void ChangeLayer(InteractionLayer newLayer)
        {
            if (newLayer == Layer)
                return;

            Layer = newLayer;
        }

        IEnumerator CO_StartDialogue()
        {
            ChangeLayer(InteractionLayer.Dialogue);
            nearbyAgent.LookAt(player.transform);
            CameraManager.I.ChangeCameraMode(CameraMode.Dialogue);
            CameraManager.I.FocusDialogueCamOn(nearbyAgent.transform);
            UIManager.I.dialogues.HideDialogueSignal();
            QuestNode questNode = QuestManager.I.GetQuestNode(nearbyAgent.ActorId);
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

        #endregion

        #region Callbacks

        void OnCloseDialogue()
        {
            ExitDialogue();
        }

        void OnActPressed()
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
