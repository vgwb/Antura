using UnityEngine;
using UnityEngine.InputSystem;

namespace Antura.Minigames.DiscoverCountry
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

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void OnAct(InputValue value)
        {
            ActInput();
        }

        public void OnMap(InputValue value)
        {
            MapInput(value.isPressed);
        }

        // public void OnActStarted(InputValue value)
        // {
        //     Debug.Log("ACT STARTED");
        // }

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            if (DiscoverGameManager.I.State != GameplayState.Play3D || InteractionManager.I.HasValidNearbyInteractable) return;
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
            if (justPressed) DiscoverNotifier.Game.OnMapButtonToggled.Dispatch();
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
