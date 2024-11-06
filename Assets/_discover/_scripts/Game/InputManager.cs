using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class InputManager : MonoBehaviour
    {
        static InputManager I;
        public static Vector3 CurrMovementVector { get; private set; } // Set by EdPlayer
        public static Vector3 CurrWorldMovementVector { get; private set; } // Set by EdPlayer

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
            CurrMovementVector = CurrWorldMovementVector = Vector3.zero;
        }

        #endregion

        #region Public Methods

        public static void SetCurrMovementVector(Vector3 relativeVector)
        {
            CurrMovementVector = relativeVector;
            
            Quaternion camRot = CameraManager.I.CamController.transform.rotation;
            Vector3 camRotEuler = camRot.eulerAngles;
            camRotEuler.x = 0;
            camRot = Quaternion.Euler(camRotEuler);
            CurrWorldMovementVector = camRot * relativeVector;
        }

        #endregion
    }
}