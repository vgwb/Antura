using Antura.Audio;
using Antura.Core;
using Antura.UI;
using DG.DeExtensions;
using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace Antura.Map
{
    /// <summary>
    /// Controls the new Map camera.
    /// Handles touch to scroll the map right and left.
    /// Makes sure to not conflict with CameraGameplayController (which is now deprecated)
    /// </summary>
    public class MapCameraController : MonoBehaviour
    {
        [Header("Options")]
        public bool CanDragBetweenStages = false;

        enum MovementType
        {
            AUTO,
            MANUAL
        }

        private MovementType movementType;

        // State
        private Transform followedTransform;

        // References
        private StageMapsManager _stageMapsManager;

        // Touch controls
        public bool useSwipe = true;
        public bool useDrag = true;

        private float xDown;
        private System.DateTime timeDown;
        public float sensibility = 0.1f;
        public float deceleration = 20.0f;
        public float minScreenPercentage = 0.3f;
        public float swipeMaxTime = 0.3f;

        private bool isFollowingFinger = false;
        private float startFingerX = 0.0f;
        private float startCameraX = 0.0f;
        public float dragSensibility = 0.1f;

        public bool dragWorld = true;   // If true, a finger movement will drag the WORLD and not the CAMERA
        public int DragWorldDir { get { return dragWorld ? -1 : 1; } }

        public bool IsFollowingFinger
        {
            get { return isFollowingFinger; }
        }

        public void Initialise(StageMapsManager _stageMapsManager)
        {
            this._stageMapsManager = _stageMapsManager;
        }

        #region Movements

        /// <summary>
        /// Follows the target transform.
        /// Mantains the X over the current map
        /// </summary>
        public void SetAutoFollowTransformCurrentMap(Transform targetTransform)
        {
            movementType = MovementType.AUTO;
            followedTransform = targetTransform;
        }

        /// <summary>
        /// Move to a new transform.
        /// Does NOT mantain X over the current map.
        /// Restores manual movement at the end.
        /// </summary>
        public void SetAutoMoveToTransformFree(Transform pivotTr, float duration)
        {
            movementType = MovementType.AUTO;
            followedTransform = null;

            AudioManager.I.PlaySound(Sfx.CameraMovementShort);
            TweenTo(pivotTr.position, pivotTr.rotation, duration, SetManualMovementCurrentMap);
        }

        /// <summary>
        /// Move to a new transform.
        /// Will match the x of the new map
        /// Restores manual movement at the end.
        /// </summary>
        public void SetAutoMoveToLookAtFree(Transform lookAtTr, Transform pivotTr, float duration, bool teleport = false)
        {
            //Debug.Log("LOOK AT " + lookAtTr.position);
            movementType = MovementType.AUTO;
            followedTransform = null;

            Vector3 pos = lookAtTr.position;
            pos.y = pivotTr.position.y;
            pos.z = pivotTr.position.z;

            if (teleport) {
                TeleportTo(pos, pivotTr.rotation);
            } else {
                AudioManager.I.PlaySound(Sfx.CameraMovementShort);
                TweenTo(pos, pivotTr.rotation, duration, SetManualMovementCurrentMap);
            }
        }

        public void TeleportToLookAtFree(Transform lookAtTr, Transform pivotTr)
        {
            SetAutoMoveToLookAtFree(lookAtTr, pivotTr, 0, true);
        }

        /// <summary>
        /// Restore manual movement.
        /// Forces the movement on the current map.
        /// </summary>
        public void SetManualMovementCurrentMap()
        {
            currentSpeed = 0.0f;
            movementType = MovementType.MANUAL;
            followedTransform = null;
        }

        #endregion

        private void LateUpdate()
        {
            if (AppManager.I.ModalWindowActivated) { return; }
            //if (GlobalUI.I.IsFingerOverUI()) { return; }

            if (movementType == MovementType.AUTO) {
                // Auto-follow
                if (followedTransform != null) {
                    // When slowed down enough, we may re-enable manual movement
                    if (Mathf.Abs(currentSpeed) < 10.0f && Input.GetMouseButtonDown(0)) {
                        SetManualMovementCurrentMap();
                        currentSpeed = 0.0f;
                    } else {
                        currentSpeed = (followedTransform.position.x - transform.position.x) * 10.0f;
                        CameraMoveUpdate();
                    }
                }
            }

            HandleMouseUp();
            if (movementType == MovementType.MANUAL) {
                // Manual control
                HandleMouseDown();
                CameraMoveUpdate();
            }

            CheckStageSwitching();
        }

        void CheckStageSwitching()
        {
            if (!_stageMapsManager || !_stageMapsManager.isLazyInitialised) { return; }

            // If dragging between stages, we may need to change stage as we move.
            if (CanDragBetweenStages) {
                float currentX = transform.position.x;

                foreach (var stageMap in _stageMapsManager.stageMaps) {
                    float startX = stageMap.cameraPivotStart.position.x;
                    float endX = stageMap.cameraPivotEnd.position.x;
                    float minX = startX < endX ? startX : endX;
                    float maxX = startX < endX ? endX : startX;

                    if (currentX > minX && currentX < maxX) {
                        _stageMapsManager.MoveToStageMap(stageMap.stageNumber, animateCamera: false);
                    }
                }
            }
        }

        private void HandleMouseUp()
        {
            // Swipe end
            if (Input.GetMouseButtonUp(0)) {
                if (useDrag) {
                    isFollowingFinger = false;
                }

                if (movementType == MovementType.MANUAL) {
                    if (useSwipe) {
                        var xUp = Input.mousePosition.x;
                        var xDelta = xUp - xDown;
                        xDown = 0;

                        var timeUp = System.DateTime.Now;
                        var timeDelta = timeUp - timeDown;
                        if ((float)timeDelta.TotalSeconds <= swipeMaxTime) {
                            float addedSpeed = DragWorldDir * xDelta / (float)timeDelta.TotalSeconds * sensibility;
                            //Debug.Log("x " + xDelta + " time " + (float)timeDelta.TotalSeconds);
                            currentSpeed = Mathf.Abs(xDelta) > Screen.width * minScreenPercentage ? (currentSpeed + addedSpeed) : 0.0f;
                        }
                    }
                }
            }

        }

        private void HandleMouseDown()
        {
            // Swipe start
            if (Input.GetMouseButtonDown(0)) {
                if (useDrag) {
                    startFingerX = Input.mousePosition.x;
                    startCameraX = Camera.main.transform.position.x;
                }

                if (useSwipe) {
                    xDown = Input.mousePosition.x;
                    timeDown = System.DateTime.Now;
                }
            }

            if (useDrag && Input.GetMouseButton(0)) {
                var xDrag = Input.mousePosition.x;
                var xDelta = xDrag - startFingerX;

                if (Mathf.Abs(xDelta) > 10) {
                    isFollowingFinger = true;
                }

                Vector3 nextCameraPosition = transform.position;
                nextCameraPosition.x = startCameraX + DragWorldDir * xDelta * dragSensibility;

                AssignCameraPosition(nextCameraPosition);
            }
        }

        private float currentSpeed = 0.0f;
        void CameraMoveUpdate()
        {
            if (isFollowingFinger) { currentSpeed = 0.0f; }
            if (Mathf.Abs(currentSpeed) > AppConfig.EPSILON) {
                int startDir = (int)Mathf.Sign(currentSpeed);
                currentSpeed -= Time.deltaTime * deceleration * startDir;

                if ((int)Mathf.Sign(currentSpeed) != startDir
                    || Mathf.Abs(currentSpeed) <= 10.0f) {
                    currentSpeed = 0.0f;
                }

                Vector3 nextCameraPosition = transform.position + Vector3.right * currentSpeed * Time.deltaTime;
                AssignCameraPosition(nextCameraPosition);
            }
        }

        private void AssignCameraPosition(Vector3 nextCameraPosition)
        {
            // Camera limits
            var startX = _stageMapsManager.CurrentShownStageMap.cameraPivotStart.position.x;
            var endX = _stageMapsManager.CurrentShownStageMap.cameraPivotEnd.position.x;

            if (CanDragBetweenStages) {
                startX = _stageMapsManager.stageMaps.First().cameraPivotStart.position.x;
                endX = _stageMapsManager.stageMaps.Last().cameraPivotEnd.position.x;
            }

            var minX = startX < endX ? startX : endX;
            var maxX = startX < endX ? endX : startX;

            if (nextCameraPosition.x > maxX || nextCameraPosition.x < minX) {
                currentSpeed = 0.0f;
            }
            nextCameraPosition.x = Mathf.Clamp(nextCameraPosition.x, minX, maxX);

            // Assign the position
            transform.position = nextCameraPosition;

        }

        private void TweenTo(Vector3 newPosition, Quaternion newRotation, float duration = 1, TweenCallback callback = null)
        {
            // @note: we just use the X and mantain the original camera's transform now
            Vector3 targetPos = transform.position;
            targetPos.x = newPosition.x;
            Quaternion targetRotation = transform.rotation;

            DOTween.Sequence()
                .Append(transform.DOLocalMove(targetPos, duration))
                .Insert(0, transform.DOLocalRotate(targetRotation.eulerAngles, duration))
                .OnComplete(callback);
        }

        public void TeleportTo(Vector3 pos, Quaternion rot)
        {
            // @note: we just use the X and mantain the original camera's transform now
            transform.SetX(pos.x);
            //transform.position = pos;
            //transform.rotation = rot;
        }

        public void TeleportTo(Transform pivot)
        {
            TeleportTo(pivot.position, pivot.rotation);
        }
    }
}