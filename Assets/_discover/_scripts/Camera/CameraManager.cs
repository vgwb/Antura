using Unity.Cinemachine;
using DG.DeInspektor.Attributes;
using DG.DemiLib;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    [RequireComponent(typeof(PlayerCameraController), typeof(DialogueCamera), typeof(MapCamera))]
    public class CameraManager : MonoBehaviour
    {
        #region Serialized

        /// <summary>After reaching min zoom level it switches to map view</summary>
        [DeRange(-50, 10)]
        [SerializeField] IntRange minMaxPlayerZoomLevel = new IntRange(-10, 0);
        [Range(0.1f, 10)]
        [SerializeField] float zoomTick = 2;
        [Range(0, 0.5f)]
        [SerializeField] float minIntervalBetweenZoomTicks = 0.25f;

        #endregion
        
        public static CameraManager I;
        public Camera MainCam { get; private set; }
        public Transform MainCamTrans { get; private set; }
        public PlayerCameraController CamController { get; private set; }

        float lastZoomTickTime;
        float leaveMapTime;
        CameraMode cameraMode;
        DialogueCamera dialogueCam;
        MapCamera mapCam;


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
            MainCam = Camera.main;
            MainCamTrans = MainCam.transform;
        }

        void Start()
        {
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
            // Zoom via mouse scroll
            if (!Mathf.Approximately(Input.mouseScrollDelta.y, 0) && Time.time - leaveMapTime > 0.5f && Time.time - lastZoomTickTime >= minIntervalBetweenZoomTicks)
            {
                lastZoomTickTime = Time.time;
                switch (cameraMode)
                {
                    case CameraMode.Player:
                        if (Input.mouseScrollDelta.y > 0)
                        {
                            CamController.SetZoomLevel(Mathf.Clamp(CamController.ZoomLevel + zoomTick, minMaxPlayerZoomLevel.min, minMaxPlayerZoomLevel.max));
                        }
                        else if (Input.mouseScrollDelta.y < 0)
                        {
                            if (CamController.ZoomLevel > minMaxPlayerZoomLevel.min)
                            {
                                CamController.SetZoomLevel(Mathf.Clamp(CamController.ZoomLevel - zoomTick, minMaxPlayerZoomLevel.min, minMaxPlayerZoomLevel.max));
                            }
                            else
                            {
                                ChangeCameraMode(CameraMode.Map);
                            }
                        }
                        break;
                    case CameraMode.Map:
                        if (Input.mouseScrollDelta.y > 0)
                        {
                            leaveMapTime = Time.time;
                            ChangeCameraMode(CameraMode.Player);
                            CamController.SetZoomLevel(minMaxPlayerZoomLevel.min);
                        }
                        break;
                }
            }
        }

        #endregion

        #region Public Methods

        public void ChangeCameraMode(CameraMode newMode)
        {
            if (newMode == cameraMode) return;

            cameraMode = newMode;
            
            CamController.Activate(cameraMode == CameraMode.Player);
            dialogueCam.Activate(cameraMode == CameraMode.Dialogue);
            mapCam.Activate(cameraMode == CameraMode.Map);
        }

        public void FocusDialogueCamOn(Transform target)
        {
            dialogueCam.SetTarget(target);
        }

        #endregion
    }
}
