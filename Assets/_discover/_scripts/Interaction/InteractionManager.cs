using UnityEngine;

namespace Antura.Minigames.DiscoverCountry.Interaction
{
    public class InteractionManager : MonoBehaviour
    {
        public static InteractionManager I { get; private set; }
        public InteractionLayer Layer { get; private set; }

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
            
            DiscoverNotifier.Game.OnLivingLetterTriggered.Subscribe(OnLivingLetterTriggered);
        }

        void OnDestroy()
        {
            if (I == this) I = null;
            DiscoverNotifier.Game.OnLivingLetterTriggered.Unsubscribe(OnLivingLetterTriggered);
        }

        void Update()
        {
            switch (Layer)
            {
                case InteractionLayer.World:
                    UpdateWorld();
                    break;
            }
        }

        #endregion

        #region Update Methods

        void UpdateWorld()
        {
            
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

        void OnLivingLetterTriggered(EdLivingLetter ll)
        {
            Debug.Log("HERE " + Layer);
            switch (Layer)
            {
                case InteractionLayer.World:
                    ChangeLayer(InteractionLayer.Dialogue);
                    CameraManager.I.ChangeCameraMode(CameraMode.Dialogue);
                    CameraManager.I.FocusDialogueCamOn(ll.transform);
                    break;
            }
        }

        #endregion
    }
}