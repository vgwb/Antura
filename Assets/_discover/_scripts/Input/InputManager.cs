using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class InputManager : MonoBehaviour
    {
        static InputManager I;
        public static Vector3 CurrMovementVector { get; private set; } // Set by PlayerCameraController

        #region Unity

        void Awake()
        {
            if (I != null)
            {
                Debug.LogError("DiscoverInputManager already exists, deleting duplicate");
                Destroy(this);
                return;
            }

            I = this;
        }

        void OnDestroy()
        {
            if (I != this) return;

            I = null;
            CurrMovementVector = Vector3.zero;
        }

        #endregion

        #region Public Methods

        public static void SetCurrMovementVector(Vector3 vector)
        {
            CurrMovementVector = vector;
        }

        #endregion
    }
}