using UnityEngine;
using UnityEngine.InputSystem;

namespace Antura.Discover
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool act;
        public bool map;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        private bool _inputsEnabled = true;

        public bool InputsEnabled => _inputsEnabled;

        public void SetInputsEnabled(bool enabled)
        {
            if (_inputsEnabled == enabled)
            {
                return;
            }

            _inputsEnabled = enabled;

            if (!enabled)
            {
                move = Vector2.zero;
                look = Vector2.zero;
                jump = false;
                sprint = false;
                act = false;
                map = false;
            }
        }

        public void OnMove(InputValue value)
        {
            if (!_inputsEnabled)
            {
                MoveInput(Vector2.zero);
                return;
            }

            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (!_inputsEnabled)
            {
                return;
            }

            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            if (!_inputsEnabled)
            {
                JumpInput(false);
                return;
            }

            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            if (!_inputsEnabled)
            {
                SprintInput(false);
                return;
            }

            SprintInput(value.isPressed);
        }

        public void OnAct(InputValue value)
        {
            if (!_inputsEnabled)
            {
                return;
            }

            ActInput();
        }

        public void OnMap(InputValue value)
        {
            if (!_inputsEnabled)
            {
                return;
            }

            MapInput(value.isPressed);
        }

        // public void OnActStarted(InputValue value)
        // {
        //     Debug.Log("ACT STARTED");
        // }

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = _inputsEnabled ? newMoveDirection : Vector2.zero;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = _inputsEnabled ? newLookDirection : Vector2.zero;
        }

        public void JumpInput(bool newJumpState)
        {
            if (DiscoverGameManager.I.State != GameplayState.Play3D || InteractionManager.I.HasValidNearbyInteractable)
                return;
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void ActInput()
        {
            DiscoverNotifier.Game.OnActClicked.Dispatch();
        }

        public void MapInput(bool justPressed)
        {
            if (justPressed)
                DiscoverNotifier.Game.OnMapButtonToggled.Dispatch();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

}
