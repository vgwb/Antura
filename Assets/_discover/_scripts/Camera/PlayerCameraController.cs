using Antura.Core;
using System;
using DG.DeInspektor.Attributes;
using DG.DemiLib;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Unity.Cinemachine;

namespace Antura.Minigames.DiscoverCountry
{
    public class PlayerCameraController : AbstractCineCamera
    {
        enum InteractionLayer
        {
            Unset,
            Movement,
            Other
        }

        enum AutoRotateMode
        {
            None,
            YAxis
        }

        enum MouseRotationMode
        {
            None,
            LMB,
            RMB_or_MMB
        }

        #region Serialized

        [DeEmptyAlert]
        [SerializeField] Camera cam;
        
        [DeHeader("Startup Options")]
        [Range(-2, 0)]
        [SerializeField] float startupZoomLevel = -0.5f;
        [Range(0, 45)]
        [SerializeField] int startupInclination = 10;
        
        [DeHeader("Options")]
        [SerializeField] MouseRotationMode mouseRotationMode = MouseRotationMode.RMB_or_MMB;
        [Range(1, 20)]
        [SerializeField] int mouseRotationSpeed = 3;
        [Range(1, 20)]
        [SerializeField] int touchRotationSpeed = 3;
        [SerializeField] AutoRotateMode autoRotateMode = AutoRotateMode.YAxis;
        [SerializeField] bool resetRotationWhenStationary = false; // Only used if autoRotate is active
        [Range(0, 10)]
        [SerializeField] float resetRotationDelay = 2.5f; // Only used if autoRotate is active
        [SerializeField] bool invertYAxis = false;
        [DeRange(0, 360)]
        [SerializeField] IntRange minMaxVerticalRotation = new IntRange(65, 210);
        [DeRange(0, 10)]
        [SerializeField] float lookUpZoomFactor = 4;
        [DeRange(0, 10)]
        [SerializeField] float lookUpArmLengthFactor = 0.25f;
        [DeRange(0, -10)]
        [SerializeField] float lookDownZoomFactor = -4;
        [DeRange(0, 10)]
        [SerializeField] float lookDownArmLengthFactor = 0.25f;
        [DeRange(0, -3f)]
        [SerializeField] float lookDownShoulderZFactor = -1.9f;
        [SerializeField] Ease lookDownShoulderZFactorEase = Ease.InQuad;
        
        [Header("Debug")]
        [SerializeField] bool drawGizmos = false;

        #endregion

        public float ZoomLevel { get; private set; }

        bool isPlayerMoving { get { return InputManager.CurrWorldMovementVector != Vector3.zero; } }
        float baseCamDistance { get { return defCamDistance - currZoomLevel; } }

        InteractionLayer interactionLayer;
        CinemachineThirdPersonFollow cineMainFollow;
        Vector3 defShoulderOffset;
        float defCamDistance, defCamArmLength;
        float lastRotationTime;
        float currZoomLevel;
        float currLookZoomFactor;
        Transform camTarget;
        Transform camTargetOriginalParent;
        Vector3 camTargetOffset;
        Tween zoomLevelTween;

        #region Unity

        void Awake()
        {
            interactionLayer = InteractionLayer.Movement;
            camTarget = FindFirstObjectByType<PlayerCameraTarget>(FindObjectsInactive.Include).transform;
            cineMain.Target.TrackingTarget = camTarget;
            cineMainFollow = cineMain.GetComponent<CinemachineThirdPersonFollow>();
            camTargetOriginalParent = camTarget.parent;
            camTargetOffset = camTarget.localPosition;
            camTarget.SetParent(this.transform);
            RefreshCinemachineSetup();
            SetZoomLevel(startupZoomLevel, true);
            // UpdateManualRotation(Vector2.zero, 1);
            UpdateManualRotation(new Vector2(0, startupInclination), 1);
        }

        void OnDestroy()
        {
            zoomLevelTween.Kill();
        }

        bool mouseLookActive = false;
        Vector2 mouseOffset = Vector2.zero;
        Vector2 lookOffset = Vector2.zero;
        bool touchLookActive;
        bool manualRotate;
        void Update()
        {
            if (Active)
            {
                camTarget.position = camTargetOriginalParent.position + camTargetOffset;
                if (interactionLayer == InteractionLayer.Movement)
                {
                    mouseLookActive = false;
                    if (mouseRotationMode != MouseRotationMode.None && (Application.isEditor || AppConfig.IsDesktopPlatform()))
                    {
                        mouseOffset = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
                        bool hasValidLookInput = mouseRotationMode == MouseRotationMode.RMB_or_MMB
                            ? Input.GetMouseButton(1) || Input.GetMouseButton(2)
                            : Input.GetMouseButton(0);
                        mouseLookActive = hasValidLookInput && mouseOffset != Vector2.zero;
                    }
                    touchLookActive = false;
                    if (AppConfig.IsMobilePlatform())
                    {
                        touchLookActive = CameraManager.I.StarterInput.look != Vector2.zero;
                    }
                    manualRotate = mouseLookActive || touchLookActive;
                    if (manualRotate)
                    {
                        lookOffset = mouseLookActive ? mouseOffset : CameraManager.I.StarterInput.look * 0.003f;
                        UpdateManualRotation(lookOffset, mouseLookActive ? mouseRotationSpeed : touchRotationSpeed);
                    }
                    else if (autoRotateMode != AutoRotateMode.None && (isPlayerMoving || resetRotationWhenStationary && Time.time - lastRotationTime > resetRotationDelay))
                    {
                        UpdateAutoRotation();
                    }
                }
            }
        }

        void LateUpdate()
        {
            camTarget.position = camTargetOriginalParent.position + camTargetOffset;
        }

        #endregion

        #region Public Methods

        public void SetZoomLevel(float value, bool immediate = false)
        {
            ZoomLevel = value;
            if (immediate)
            {
                currZoomLevel = value;
                SetZoomLevel_Update();
            }
            else
            {
                zoomLevelTween.Kill();
                zoomLevelTween = DOTween.To(() => currZoomLevel, x => currZoomLevel = x, value, 0.25f).SetEase(Ease.OutQuad)
                    .OnUpdate(SetZoomLevel_Update);
            }
        }

        void SetZoomLevel_Update()
        {
            cineMainFollow.CameraDistance = baseCamDistance - currLookZoomFactor;
        }

        #endregion

        #region Methods

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void RefreshCinemachineSetup()
        {
            defShoulderOffset = cineMainFollow.ShoulderOffset;
            defCamDistance = cineMainFollow.CameraDistance;
            defCamArmLength = cineMainFollow.VerticalArmLength;
        }

        void UpdateManualRotation(Vector2 inputOffset, float speed)
        {
            if (invertYAxis)
                inputOffset.y = -inputOffset.y;
            Quaternion camRot = camTarget.rotation;
            // Left/right rotation
            camRot *= Quaternion.AngleAxis(inputOffset.x * speed, Vector3.up);
            // Up/down rotation
            camRot *= Quaternion.AngleAxis(inputOffset.y * speed, Vector3.right);
            // Clamp
            Vector3 camAngle = camRot.eulerAngles;
            camAngle.z = 0;
            if (camAngle.x > 180 && camAngle.x < minMaxVerticalRotation.max)
                camAngle.x = minMaxVerticalRotation.max;
            else if (camAngle.x < 180 && camAngle.x > minMaxVerticalRotation.min)
                camAngle.x = minMaxVerticalRotation.min;

            GetDataForCamAngle(camAngle, out bool isLookingDown, out float currLookUpPerc, out float currLookDownPerc, out float lookZoomFactor, out float currArmLengthFactor);
            currLookZoomFactor = lookZoomFactor;

            // float currArmLengthFactor = isLookingDown ? lookDownArmLengthFactor * currLookDownPerc : lookUpArmLengthFactor * currLookUpPerc;
            cineMainFollow.CameraDistance = baseCamDistance - currLookZoomFactor;
            cineMainFollow.VerticalArmLength = defCamArmLength + currArmLengthFactor;
            Vector3 currShoulderOffset = defShoulderOffset;
            if (isLookingDown)
            {
                float currShoulderZFactor = lookDownShoulderZFactor * DOVirtual.EasedValue(0, 1, currLookDownPerc, lookDownShoulderZFactorEase);
                currShoulderOffset.z += currShoulderZFactor;
            }
            cineMainFollow.ShoulderOffset = currShoulderOffset;
            // Assign
            camTarget.rotation = Quaternion.Euler(camAngle);

            lastRotationTime = Time.time;
        }

        void UpdateAutoRotation()
        {
            float speed = 0.5f;
            Vector3 currPivotEuler = camTarget.eulerAngles;
            if (isPlayerMoving)
            {
                float turnAngle = Vector2.Angle(new Vector2(0, 1), new Vector2(InputManager.CurrMovementVector.x, InputManager.CurrMovementVector.z));
                if (turnAngle > 135)
                    return; // Don't re-adapt camera if moving backwards
                speed = 0.25f + (1 - turnAngle / 180) * 2f;
            }
            currPivotEuler.y = camTargetOriginalParent.eulerAngles.y;
            Quaternion targetRot = Quaternion.Euler(currPivotEuler);
            float time = Time.deltaTime * speed;
            camTarget.rotation = Quaternion.Slerp(camTarget.rotation, targetRot, time);
        }

        // void UpdateMovementVector()
        // {
        //     Vector3 movementFactor = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //     InputManager.SetCurrMovementVector(movementFactor);
        // }

        void GetDataForCamAngle(Vector3 camAngle, out bool isLookingDown, out float lookUpPerc, out float lookDownPerc, out float lookZoomFactor, out float armLengthFactor)
        {
            isLookingDown = camAngle.x < 180;
            lookUpPerc = isLookingDown ? 0 : Mathf.Clamp((360 - camAngle.x) / (360 - minMaxVerticalRotation.max), 0, 1);
            lookDownPerc = !isLookingDown ? 0 : Mathf.Clamp(camAngle.x / minMaxVerticalRotation.min, 0, 1);
            lookZoomFactor = isLookingDown ? lookDownZoomFactor * lookDownPerc : lookUpZoomFactor * lookUpPerc;
            armLengthFactor = isLookingDown ? lookDownArmLengthFactor * lookDownPerc : lookUpArmLengthFactor * lookUpPerc;
        }

        void OnDrawGizmos()
        {
            if (!drawGizmos || !Application.isPlaying)
                return;

            Vector3 p = camTarget.position;
            p.y = 0;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(p, p + InputManager.CurrWorldMovementVector * 10);
        }

        #endregion
    }
}
