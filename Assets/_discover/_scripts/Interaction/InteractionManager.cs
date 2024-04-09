using System.Collections;
using Antura.Homer;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry.Interaction
{
    public class InteractionManager : MonoBehaviour
    {
        public static InteractionManager I { get; private set; }
        public InteractionLayer Layer { get; private set; }

        EdLivingLetter nearbyLetter;
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
            
            DiscoverNotifier.Game.OnCloseDialogue.Subscribe(OnCloseDialogue);
            DiscoverNotifier.Game.OnLivingLetterTriggerEnter.Subscribe(OnLivingLetterTriggerEnter);
            DiscoverNotifier.Game.OnLivingLetterTriggerExit.Subscribe(OnLivingLetterTriggerExit);
        }

        void OnDestroy()
        {
            if (I == this) I = null;
            this.StopAllCoroutines();
            DiscoverNotifier.Game.OnCloseDialogue.Unsubscribe(OnCloseDialogue);
            DiscoverNotifier.Game.OnLivingLetterTriggerEnter.Unsubscribe(OnLivingLetterTriggerEnter);
            DiscoverNotifier.Game.OnLivingLetterTriggerExit.Unsubscribe(OnLivingLetterTriggerExit);
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

        void UpdateWorld()
        {
            if (Input.GetKeyDown(KeyCode.E) && nearbyLetter != null)
            {
                // Start dialogue with LL
                if (coStartDialogue != null) this.StopCoroutine(coStartDialogue);
                coStartDialogue = this.StartCoroutine(CO_StartDialogue());
            }
        }

        void UpdateDialogue()
        {
            // if (Input.GetKeyDown(KeyCode.Escape)) ExitDialogue();
        }

        #endregion

        #region Methods

        void ChangeLayer(InteractionLayer newLayer)
        {
            if (newLayer == Layer) return;

            Layer = newLayer;
        }

        IEnumerator CO_StartDialogue()
        {
            ChangeLayer(InteractionLayer.Dialogue);
            CameraManager.I.ChangeCameraMode(CameraMode.Dialogue);
            CameraManager.I.FocusDialogueCamOn(nearbyLetter.transform);
            UIManager.I.dialogues.HideDialogueSignal();
            QuestNode questNode = QuestManager.I.GetQuestNode(nearbyLetter.ActorId);
            if (questNode == null) Debug.LogError("QuestNode is NULL, shouldn't happen");
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
            if (nearbyLetter != null) UIManager.I.dialogues.ShowDialogueSignalFor(nearbyLetter);
            if (coStartDialogue != null)
            {
                this.StopCoroutine(coStartDialogue);
                coStartDialogue = null;
            }
            UIManager.I.dialogues.CloseDialogue();
        }

        #endregion

        #region Callbacks

        void OnCloseDialogue()
        {
            ExitDialogue();
        }

        void OnLivingLetterTriggerEnter(EdLivingLetter ll)
        {
            nearbyLetter = ll;
            UIManager.I.dialogues.ShowDialogueSignalFor(nearbyLetter);
        }
        
        void OnLivingLetterTriggerExit(EdLivingLetter ll)
        {
            if (nearbyLetter == ll)
            {
                nearbyLetter = null;
                UIManager.I.dialogues.HideDialogueSignal();
            }
        }

        #endregion
    }
}