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

        void OnDestroy()
        {
            if (I == this) I = null;
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
    }
}