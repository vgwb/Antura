// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2024/03/27

using System;
using Cinemachine;
using DG.DeInspektor.Attributes;
using DG.DemiLib;
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

        [Range(1, 20)]
        [SerializeField] int rotationSpeed = 10;
        [Range(0, 10)]
        [SerializeField] float resetRotationDelay = 2.5f;
        [SerializeField] bool invertYAxis = false;
        [DeRange(0, 360)]
        [SerializeField] IntRange minMaxVerticalRotation = new IntRange(65, 210);
        [DeRange(0, 10)]
        [SerializeField] int lookUpZoomFactor = 5;
        [DeRange(0, 10)]
        [SerializeField] int lookUpArmLengthFactor = 3;
        [DeEmptyAlert]
        [SerializeField] Camera cam;
        [DeEmptyAlert]
        [SerializeField] CinemachineVirtualCamera virtualCam;
        [DeEmptyAlert]
        [SerializeField] Transform camPivot;

        #endregion

        public Vector3 CurrMovementVector;
        
        Mode mode;
        InteractionLayer interactionLayer;
        Cinemachine3rdPersonFollow camFollow;
        float defCamDistance, defCamArmLength;
        float lastRotationTime;
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
            camFollow = virtualCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            defCamDistance = camFollow.CameraDistance;
            defCamArmLength = camFollow.VerticalArmLength;
        }

        void Update()
        {
            if (interactionLayer == InteractionLayer.Movement)
            {
                switch (mode)
                {
                    case Mode.Desktop:
                        Vector2 mouseOffset = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
                        if (mouseOffset != Vector2.zero) UpdateMouseRotation(mouseOffset);
                        else if (isMoving || Time.time - lastRotationTime > resetRotationDelay) UpdateResetRotation(isMoving);
                        UpdateMovementVector();
                        break;
                }
            }
        }

        #endregion

        #region Methods

        void SetMode(Mode newMode)
        {
            mode = newMode;
        }

        void UpdateMouseRotation(Vector2 mouseOffset)
        {
            if (invertYAxis) mouseOffset.y = -mouseOffset.y;
            Quaternion camRot = camPivot.rotation;
            // Left/right rotation
            camRot *= Quaternion.AngleAxis(mouseOffset.x * rotationSpeed, Vector3.up);
            // Up/down rotation
            camRot *= Quaternion.AngleAxis(mouseOffset.y * rotationSpeed, Vector3.right);
            // Clamp
            Vector3 camAngle = camRot.eulerAngles;
            camAngle.z = 0;
            if (camAngle.x > 180 && camAngle.x < minMaxVerticalRotation.max) camAngle.x = minMaxVerticalRotation.max;
            else if (camAngle.x < 180 && camAngle.x > minMaxVerticalRotation.min) camAngle.x = minMaxVerticalRotation.min;
            // LookUp factor
            float currLookUpPerc = camAngle.x < 180 ? 0 : Mathf.Clamp((360 - camAngle.x) / (360 - minMaxVerticalRotation.max), 0, 1);
            float currZoomFactor = lookUpZoomFactor * currLookUpPerc;
            float currArmLengthFactor = lookUpArmLengthFactor * currLookUpPerc;
            camFollow.CameraDistance = defCamDistance - currZoomFactor;
            camFollow.VerticalArmLength = defCamArmLength + currArmLengthFactor;
            // Assign
            camPivot.rotation = Quaternion.Euler(camAngle);

            lastRotationTime = Time.time;
        }

        void UpdateResetRotation(bool fast)
        {
            Vector3 currPivotEuler = camPivot.localEulerAngles;
            currPivotEuler.y = 0;
            Quaternion targetRot = Quaternion.Euler(currPivotEuler);
            camPivot.localRotation = Quaternion.Lerp(camPivot.localRotation, targetRot, Time.deltaTime * (fast ? 5 : 0.75f));
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
            if (!Application.isPlaying) return;
            
            Vector3 p = camPivot.position;
            p.y = 0;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(p, p + CurrMovementVector * 10);
        }

        #endregion
    }
}