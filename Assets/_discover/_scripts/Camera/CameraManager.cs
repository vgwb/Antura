using System;
using System.Collections;
using Unity.Cinemachine;
using DG.DeInspektor.Attributes;
using DG.DemiLib;
using UnityEngine;

namespace Antura.Discover
{
    [RequireComponent(typeof(PlayerCameraController), typeof(DialogueCamera), typeof(MapCamera))]
    public class CameraManager : MonoBehaviour
    {
        #region Serialized

        /// <summary>After reaching min zoom level it switches to map view</summary>
        [DeRange(-5, 0)]
        [SerializeField] DG.DemiLib.Range minMaxPlayerZoomLevel = new DG.DemiLib.Range(-2, 0);
        [Range(0.1f, 10)]
        [SerializeField] float zoomTick = 2;
        [Range(0, 0.5f)]
        [SerializeField] float minIntervalBetweenZoomTicks = 0.25f;

        #endregion

        public static CameraManager I;
        public CameraMode Mode { get; private set; }
        public Camera MainCam { get; private set; }
        public Transform MainCamTrans { get; private set; }
        public PlayerCameraController CamController { get; private set; }
        public StarterAssetsInputs StarterInput { get; private set; }

        float lastZoomTickTime;
        float leaveMapTime;
        DialogueCamera dialogueCam;
        FocusCamera focusCam;
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
            StarterInput = FindFirstObjectByType<StarterAssetsInputs>();
            dialogueCam = this.GetComponent<DialogueCamera>();
            mapCam = this.GetComponent<MapCamera>();
            focusCam = this.GetComponent<FocusCamera>();
            ChangeCameraMode(CameraMode.Player);

            DiscoverNotifier.Game.OnMapButtonToggled.Subscribe(OnMapButtonToggled);
        }

        void OnDestroy()
        {
            if (I == this)
                I = null;
            DiscoverNotifier.Game.OnMapButtonToggled.Unsubscribe(OnMapButtonToggled);
        }

        void Update()
        {
            // Zoom via mouse scroll
            if (!Mathf.Approximately(Input.mouseScrollDelta.y, 0) && Time.time - leaveMapTime > 0.5f && Time.time - lastZoomTickTime >= minIntervalBetweenZoomTicks)
            {
                lastZoomTickTime = Time.time;
                switch (Mode)
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
                                // Go-to map via scroll wheel is not allowed anymore
                                // ChangeCameraMode(CameraMode.Map);
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
            if (newMode == Mode)
                return;

            Mode = newMode;

            CamController.Activate(Mode == CameraMode.Player);
            dialogueCam.Activate(Mode == CameraMode.Dialogue);
            mapCam.Activate(Mode == CameraMode.Map);
            focusCam.Activate(Mode == CameraMode.Focus);
        }

        /// <summary>
        /// Sets the target (LL, chest, etc.) of the current dialogue, which the camera should focus on
        /// </summary>
        public void SetDialogueModeTarget(Transform target)
        {
            dialogueCam.SetTarget(target);
        }

        /// <summary>
        /// Focuses the camera to look at a specific target and with an optional specific origin.
        /// Only call this during dialogues
        /// </summary>
        public IEnumerator FocusOn(Transform lookAtTarget, Transform origin = null)
        {
            Debug.Log("CameraManager: FocusOn: " + (lookAtTarget != null ? lookAtTarget.name : "null") + ", origin: " + (origin != null ? origin.name : "null"));
            Vector3 toCamPos = dialogueCam.CineMain.transform.position;
            if (origin != null)
                toCamPos = origin.position;
            else
                toCamPos.y += focusCam.YOffset;
            focusCam.CineMain.transform.position = toCamPos;
            focusCam.SetTarget(lookAtTarget);
            ChangeCameraMode(CameraMode.Focus);
            yield break;
        }

        /// <summary>
        /// Focuses the camera to look at a specific target and with an optional specific origin.
        /// Only call this during dialogues
        /// </summary>
        public IEnumerator FocusOnFocusData(CameraFocusData focusData)
        {
            // Debug.Log("CameraManager: FocusOn: " + (lookAtTarget != null ? lookAtTarget.name : "null") + ", origin: " + (origin != null ? origin.name : "null"));
            // Vector3 toCamPos = dialogueCam.CineMain.transform.position;
            // if (origin != null)
            //     toCamPos = origin.position;
            // else
            //     toCamPos.y += focusCam.YOffset;
            // focusCam.CineMain.transform.position = toCamPos;
            // focusCam.SetTarget(lookAtTarget);
            // ChangeCameraMode(CameraMode.Focus);
            yield break;
        }

        /// <summary>
        /// Resets the eventually active camera focus
        /// </summary>
        public void ResetFocus()
        {
            if (Mode != CameraMode.Focus)
                return;

            switch (DiscoverGameManager.I.State)
            {
                case GameplayState.Dialogue:
                    ChangeCameraMode(CameraMode.Dialogue);
                    break;
                case GameplayState.Play3D:
                    ChangeCameraMode(CameraMode.Player);
                    break;
                case GameplayState.Map:
                    ChangeCameraMode(CameraMode.Map);
                    break;
                default:
                    ChangeCameraMode(CameraMode.Unset);
                    break;
            }
        }

        #endregion

        #region Callbacks

        void OnMapButtonToggled()
        {
            switch (Mode)
            {
                case CameraMode.Player:
                    ChangeCameraMode(CameraMode.Map);
                    break;
                case CameraMode.Map:
                    ChangeCameraMode(CameraMode.Player);
                    break;
            }
        }

        #endregion
    }
}
