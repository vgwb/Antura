using Unity.Cinemachine;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    [RequireComponent(typeof(PlayerCameraController), typeof(DialogueCamera))]
    public class CameraManager : MonoBehaviour
    {
        public PlayerCameraController CamController { get; private set; }
        DialogueCamera dialogueCam;

        public static CameraManager I;

        #region Unity

        void Awake()
        {
            if (I != null)
            {
                Debug.LogError("CameraManager already exists in scene, destroying duplicate");
                Destroy(this.gameObject);
                return;
            }

            I = this;
            CamController = this.GetComponent<PlayerCameraController>();
            dialogueCam = this.GetComponent<DialogueCamera>();
        }

        void OnDestroy()
        {
            if (I == this)
                I = null;
        }

        #endregion
    }
}
