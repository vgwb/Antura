using UnityEngine;

namespace Antura.Minigames.DiscoverCountry.Interaction
{
    public class InteractionManager : MonoBehaviour
    {
        public static InteractionManager I { get; private set; }
        public InteractionLayer Layer { get; private set; }

        EdLivingLetter nearbyLetter;

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
            
            DiscoverNotifier.Game.OnLivingLetterTriggerEnter.Subscribe(OnLivingLetterTriggerEnter);
            DiscoverNotifier.Game.OnLivingLetterTriggerExit.Subscribe(OnLivingLetterTriggerExit);
        }

        void OnDestroy()
        {
            if (I == this) I = null;
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
                ChangeLayer(InteractionLayer.Dialogue);
                CameraManager.I.ChangeCameraMode(CameraMode.Dialogue);
                CameraManager.I.FocusDialogueCamOn(nearbyLetter.transform);
                UIManager.I.dialogues.HideDialogueSignal();
            }
        }

        void UpdateDialogue()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Exit dialogue
                ChangeLayer(InteractionLayer.World);
                CameraManager.I.ChangeCameraMode(CameraMode.Player);
                if (nearbyLetter != null) UIManager.I.dialogues.ShowDialogueSignalFor(nearbyLetter);
            }
        }

        #endregion

        #region Methods

        void ChangeLayer(InteractionLayer newLayer)
        {
            if (newLayer == Layer) return;

            Layer = newLayer;
        }

        #endregion

        #region Callbacks

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