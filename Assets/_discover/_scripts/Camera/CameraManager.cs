using Unity.Cinemachine;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    [RequireComponent(typeof(PlayerCameraController), typeof(DialogueCamera), typeof(MapCamera))]
    public class CameraManager : MonoBehaviour
    {
        enum CameraMode
        {
            Unset,
            Player,
            Dialogue,
            Map
        }
        
        public PlayerCameraController CamController { get; private set; }

        CameraMode cameraMode;
        DialogueCamera dialogueCam;
        MapCamera mapCam;

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
        }

        void Start()
        {
            I = this;
            CamController = this.GetComponent<PlayerCameraController>();
            dialogueCam = this.GetComponent<DialogueCamera>();
            mapCam = this.GetComponent<MapCamera>();
            ChangeCameraMode(CameraMode.Player);
        }

        void OnDestroy()
        {
            if (I == this)
                I = null;
        }

        void Update()
        {
            // Zoom to/from map mode via mouse scroll
            if (!Mathf.Approximately(Input.mouseScrollDelta.y, 0))
            {
                switch (cameraMode)
                {
                    case CameraMode.Player:
                        if (Input.mouseScrollDelta.y < 0) ChangeCameraMode(CameraMode.Map);
                        break;
                    case CameraMode.Map:
                        if (Input.mouseScrollDelta.y > 0) ChangeCameraMode(CameraMode.Player);
                        break;
                }
            }
        }

        #endregion

        #region Methods

        void ChangeCameraMode(CameraMode newMode)
        {
            if (newMode == cameraMode) return;

            cameraMode = newMode;
            
            CamController.Activate(cameraMode == CameraMode.Player);
            dialogueCam.Activate(cameraMode == CameraMode.Dialogue);
            mapCam.Activate(cameraMode == CameraMode.Map);
        }

        #endregion
    }
}
