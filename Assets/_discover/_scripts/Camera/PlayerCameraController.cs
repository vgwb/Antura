using System;
using Unity.Cinemachine;
using DG.DeInspektor.Attributes;
using DG.DemiLib;
using DG.Tweening;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class PlayerCameraController : MonoBehaviour
    {
        enum Mode
        {
            Unset,
            Desktop,
            Mobile
        }

        enum InteractionLayer
        {
            Unset,
            Movement,
            Other
        }

        #region Serialized

        [SerializeField] bool clickAndRotate = true;
        [Range(1, 20)]
        [SerializeField] int rotationSpeed = 10;
        [SerializeField] bool resetRotationAfterAWhile = false;
        [Range(0, 10)]
        [SerializeField] float resetRotationDelay = 2.5f;
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
        [Header("References - Prefab")]
        [DeEmptyAlert]
        [SerializeField] Camera cam;
        [Header("References - Scene")]
        [DeEmptyAlert]
        [SerializeField] Transform camPivot;
        [DeEmptyAlert]
        [SerializeField] CinemachineCamera cineMain;
        [Header("Debug")]
        [SerializeField] bool drawGizmos = false;

        #endregion

        [NonSerialized] public Vector3 CurrMovementVector;

        Mode mode;
        InteractionLayer interactionLayer;
        CinemachineThirdPersonFollow cineMainFollow;
        Vector3 defShoulderOffset;
        float defCamDistance, defCamArmLength;
        float lastRotationTime;
        Transform camPivotOriginalParent;
        Vector3 camPivotOffset;
        bool isMoving { get { return CurrMovementVector != Vector3.zero; } }

        #region Unity

        void Awake()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            SetMode(Mode.Desktop);
#else
            SetMode(Mode.Mobile);
#endif

            interactionLayer = InteractionLayer.Movement;
            cineMainFollow = cineMain.GetComponent<CinemachineThirdPersonFollow>();
            camPivotOriginalParent = camPivot.parent;
            camPivotOffset = camPivot.localPosition;
            camPivot.SetParent(this.transform);
            RefreshCinemachineSetup();
        }

        void Update()
        {
            camPivot.position = camPivotOriginalParent.position + camPivotOffset;
            if (interactionLayer == InteractionLayer.Movement)
            {
                switch (mode)
                {
                    case Mode.Desktop:
                        Vector2 mouseOffset = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
                        bool manualRotate = (!clickAndRotate || Input.GetMouseButton(1)) && mouseOffset != Vector2.zero;
                        if (manualRotate)
                            UpdateMouseRotation(mouseOffset);
                        else if (resetRotationAfterAWhile && (isMoving || Time.time - lastRotationTime > resetRotationDelay))
                            UpdateResetRotation(isMoving);
                        UpdateMovementVector();
                        break;
                }
            }
        }

        void LateUpdate()
        {
            camPivot.position = camPivotOriginalParent.position + camPivotOffset;
        }

        #endregion

        #region Methods

        [DeMethodButton]
        void RefreshCinemachineSetup()
        {
            defShoulderOffset = cineMainFollow.ShoulderOffset;
            defCamDistance = cineMainFollow.CameraDistance;
            defCamArmLength = cineMainFollow.VerticalArmLength;
        }

        void SetMode(Mode newMode)
        {
            mode = newMode;
        }

        void UpdateMouseRotation(Vector2 mouseOffset)
        {
            if (invertYAxis)
                mouseOffset.y = -mouseOffset.y;
            Quaternion camRot = camPivot.rotation;
            // Left/right rotation
            camRot *= Quaternion.AngleAxis(mouseOffset.x * rotationSpeed, Vector3.up);
            // Up/down rotation
            camRot *= Quaternion.AngleAxis(mouseOffset.y * rotationSpeed, Vector3.right);
            // Clamp
            Vector3 camAngle = camRot.eulerAngles;
            camAngle.z = 0;
            if (camAngle.x > 180 && camAngle.x < minMaxVerticalRotation.max)
                camAngle.x = minMaxVerticalRotation.max;
            else if (camAngle.x < 180 && camAngle.x > minMaxVerticalRotation.min)
                camAngle.x = minMaxVerticalRotation.min;
            // LookUp/Down modifiers
            bool isLookingDown = camAngle.x < 180;
            float currLookUpPerc = isLookingDown ? 0 : Mathf.Clamp((360 - camAngle.x) / (360 - minMaxVerticalRotation.max), 0, 1);
            float currLookDownPerc = !isLookingDown ? 0 : Mathf.Clamp(camAngle.x / minMaxVerticalRotation.min, 0, 1);
            float currZoomFactor = isLookingDown ? lookDownZoomFactor * currLookDownPerc : lookUpZoomFactor * currLookUpPerc;
            float currArmLengthFactor = isLookingDown ? lookDownArmLengthFactor * currLookDownPerc : lookUpArmLengthFactor * currLookUpPerc;
            cineMainFollow.CameraDistance = defCamDistance - currZoomFactor;
            cineMainFollow.VerticalArmLength = defCamArmLength + currArmLengthFactor;
            Vector3 currShoulderOffset = defShoulderOffset;
            if (isLookingDown)
            {
                float currShoulderZFactor = lookDownShoulderZFactor * DOVirtual.EasedValue(0, 1, currLookDownPerc, lookDownShoulderZFactorEase);
                currShoulderOffset.z += currShoulderZFactor;
            }
            cineMainFollow.ShoulderOffset = currShoulderOffset;
            // Assign
            camPivot.rotation = Quaternion.Euler(camAngle);

            lastRotationTime = Time.time;
        }

        void UpdateResetRotation(bool fast)
        {
            Vector3 currPivotEuler = camPivot.eulerAngles;
            currPivotEuler.y = camPivotOriginalParent.eulerAngles.y;
            Quaternion targetRot = Quaternion.Euler(currPivotEuler);
            camPivot.rotation = Quaternion.Lerp(camPivot.rotation, targetRot, Time.deltaTime * (fast ? 5 : 0.75f));
        }

        void UpdateMovementVector()
        {
            Vector3 movementFactor = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Quaternion camRot = cam.transform.rotation;
            Vector3 camRotEuler = camRot.eulerAngles;
            camRotEuler.x = 0;
            camRot = Quaternion.Euler(camRotEuler);
            CurrMovementVector = camRot * movementFactor;
        }

        void OnDrawGizmos()
        {
            if (!drawGizmos || !Application.isPlaying)
                return;

            Vector3 p = camPivot.position;
            p.y = 0;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(p, p + CurrMovementVector * 10);
        }

        #endregion
    }
}
