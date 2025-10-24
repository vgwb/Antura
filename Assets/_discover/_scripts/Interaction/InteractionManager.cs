using System.Collections;
using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Antura.Discover
{
    public class InteractionManager : MonoBehaviour
    {
        #region Serialized

        public PlayerController player;

        #endregion

        public static InteractionManager I { get; private set; }
        public int LastActionFrame { get; private set; }
        public bool IsUsingFocusView => CameraManager.I.Mode == CameraMode.Focus;
        public bool HasValidNearbyInteractable => NearbyInteractable != null && NearbyInteractable.gameObject.activeInHierarchy;
        public Interactable NearbyInteractable { get; private set; } // Closest interactable, if any

        readonly List<Interactable> allNearbyInteractables = new();
        int focusViewEnterFrame;
        Coroutine coChangeLayer, coStartDialogue;

        #region Unity

        void Awake()
        {
            if (I != null)
            {
                Debug.LogError("InteractionManager already exists, deleting duplicate");
                Destroy(this);
                return;
            }

            I = this;
        }

        void Start()
        {
            DiscoverNotifier.Game.OnPlayerMoved.Subscribe(OnPlayerMoved);
            DiscoverNotifier.Game.OnCloseDialogue.Subscribe(OnCloseDialogue);
            DiscoverNotifier.Game.OnInteractableEnteredByPlayer.Subscribe(OnInteractableEnteredByPlayer);
            DiscoverNotifier.Game.OnInteractableExitedByPlayer.Subscribe(OnInteractableExitedByPlayer);
            DiscoverNotifier.Game.OnActClicked.Subscribe(OnActClicked);
        }

        void OnDestroy()
        {
            if (I == this)
                I = null;
            this.StopAllCoroutines();
            DiscoverNotifier.Game.OnPlayerMoved.Unsubscribe(OnPlayerMoved);
            DiscoverNotifier.Game.OnCloseDialogue.Unsubscribe(OnCloseDialogue);
            DiscoverNotifier.Game.OnInteractableEnteredByPlayer.Unsubscribe(OnInteractableEnteredByPlayer);
            DiscoverNotifier.Game.OnInteractableExitedByPlayer.Unsubscribe(OnInteractableExitedByPlayer);
            DiscoverNotifier.Game.OnActClicked.Unsubscribe(OnActClicked);
        }

        #endregion

        #region Update

        void Update()
        {
            if (DiscoverGameManager.I.State == GameplayState.Play3D)
            {
                UpdateWorld();
            }
            else if (DiscoverGameManager.I.State == GameplayState.Dialogue)
            {
                UpdateDialogue();
            }
        }

        void UpdateWorld()
        {
            if (NearbyInteractable != null && !HasValidNearbyInteractable)
            {
                DiscoverNotifier.Game.OnInteractableExitedByPlayer.Dispatch(NearbyInteractable);
            }
        }

        void UpdateDialogue()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                ExitDialogue();
            if (IsUsingFocusView && Input.GetMouseButtonDown(0))
                ResetCameraFocus();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Hides the UI and focuses the camera to look at a specific target and with an optional specific origin
        /// </summary>
        public void FocusCameraOn(Transform target, Transform origin = null)
        {
            focusViewEnterFrame = Time.frameCount;
            CoroutineRunner.FireCoroutine(CameraManager.I.FocusOn(target, origin));
            UIManager.I.gameObject.SetActive(false);
        }

        /// <summary>
        /// Shows the UI and resets the eventually active camera focus
        /// </summary>
        public void ResetCameraFocus()
        {
            if (!IsUsingFocusView)
                return;

            CameraManager.I.ResetFocus();
            UIManager.I.gameObject.SetActive(true);
        }


        private Transform currentTarget;
        /// <summary>
        /// Activates the world icon for a target Transform
        /// </summary>
        /// <param name="activate">TRUE to activate, FALSE otherwise</param>
        /// <param name="target">Required only if activating, the target to follow</param>
        public void ActivateWorldTargetIcon(bool activate, Transform target = null)
        {
            if (activate)
                currentTarget = target;
            else
                currentTarget = null;

            UIManager.I.ActivateWorldTargetMarker(activate, currentTarget);
        }

        public void CheckDeactivateTarget(Transform target)
        {
            if (currentTarget == target)
            {
                ActivateWorldTargetIcon(false);
            }
        }

        /// <summary>
        /// Shows the preview icon signal (active even when far from the target) for the given interactable
        /// </summary>
        public void ShowPreviewSignalFor(Interactable interactable, bool show)
        {
            if (show || UIManager.I != null)
                UIManager.I.dialogues.ShowPreviewSignalFor(interactable, show);
        }

        public void ForceNearbyInteractableTo(Interactable interactable)
        {
            if (NearbyInteractable != interactable)
                SetNearbyInteractableTo(interactable);
            if (!allNearbyInteractables.Contains(interactable))
                allNearbyInteractables.Add(interactable);
        }

        #endregion

        #region Methods

        void Act()
        {
            LastActionFrame = Time.frameCount;
            // The bottom lines were for the previous version of camera focus, before Yarn and focus logic changes,
            // when pressing Act during a camera focus would automatically reset the focus.
            // Since we passed to Yarn, a focus reset needs to be called directly by the dialogue
            // and is not automatic anymore, so the bottom lines are now commented out
            // (but not deleted, in case things change again)
            // if (IsUsingFocusView && focusViewEnterFrame != Time.frameCount)
            //     ResetCameraFocus();
            if (DiscoverGameManager.I.State != GameplayState.Play3D)
                return;

            // Debug.Log("InteractionManager: Act() called + " + HasValidNearbyInteractable);

            if (HasValidNearbyInteractable)
            {
                var interacted = NearbyInteractable;
                CheckDeactivateTarget(interacted.transform);

                QuestNode questNode = interacted.Execute();
                // Notify task manager for Interact-type tasks
                QuestManager.I?.TaskManager?.OnInteractableUsed(interacted);
                // if (questNode != null)
                //     this.RestartCoroutine(ref coStartDialogue, CO_StartDialogue(questNode, interacted));
            }
        }

        // public void DisplayNode(QuestNode node)
        // {
        //     if (node == null)
        //     {
        //         Debug.LogError("QuestNode is NULL, shouldn't happen");
        //         return;
        //     }

        //     DiscoverNotifier.Game.OnStartDialogue.Dispatch();
        //     UIManager.I.dialogues.StartDialogue(node);
        // }

        public void StartDialogue(Interactable interactable)
        {
            player?.SetMovementLock(true);

            DiscoverGameManager.I.ChangeState(GameplayState.Dialogue);
            DiscoverNotifier.Game.OnStartDialogue.Dispatch();

            Interactable activeInteractable = interactable != null ? interactable : NearbyInteractable;

            if (activeInteractable != null)
            {
                if (activeInteractable.IsLL && player != null)
                {
                    activeInteractable.LL.LookAt(player.transform);
                }

                if (activeInteractable.FocusCameraOnInteract)
                {
                    CameraManager.I.ChangeCameraMode(CameraMode.Dialogue);
                    CameraManager.I.SetDialogueModeTarget(activeInteractable.LookAtTransform);
                }

                UIManager.I.dialogues.HideSignal(activeInteractable, false);
            }

            // if (questNode == null)
            // {
            //     Debug.LogError("QuestNode is NULL, shouldn't happen");
            // }
            // else
            // {
            //     yield return new WaitForSeconds(0.5f);
            //     UIManager.I.dialogues.StartDialogue(questNode);
            // }
            // coStartDialogue = null;
        }

        void ExitDialogue()
        {
            DiscoverGameManager.I.ChangeState(GameplayState.Play3D);
            player?.SetMovementLock(false);
            CameraManager.I.ChangeCameraMode(CameraMode.Player);
            if (HasValidNearbyInteractable)
                UIManager.I.dialogues.ShowSignalFor(NearbyInteractable);
            this.CancelCoroutine(ref coStartDialogue);
            UIManager.I.dialogues.CloseDialogue();
        }

        void RefreshNearbyInteractable()
        {
            Interactable prev = NearbyInteractable;
            NearbyInteractable = null;
            float lastDistSqr = float.MaxValue;
            foreach (Interactable interactable in allNearbyInteractables)
            {
                if (!interactable.gameObject.activeInHierarchy)
                    continue;
                float distSqr = (interactable.transform.position - player.transform.position).sqrMagnitude;
                if (distSqr >= lastDistSqr)
                    continue;
                lastDistSqr = distSqr;
                NearbyInteractable = interactable;
            }
            if (NearbyInteractable != prev)
            {
                if (prev != null)
                {
                    UIManager.I.dialogues.HideSignal(prev, true);
                }
                if (NearbyInteractable != null && HasValidNearbyInteractable)
                {
                    UIManager.I.dialogues.ShowSignalFor(NearbyInteractable);
                }
                DiscoverNotifier.Game.OnNearbyInteractableChanged.Dispatch(NearbyInteractable);
            }
        }

        void SetNearbyInteractableTo(Interactable interactable)
        {
            Interactable prev = NearbyInteractable;
            NearbyInteractable = interactable;
            UIManager.I.dialogues.ShowSignalFor(NearbyInteractable);
            if (NearbyInteractable != prev)
                DiscoverNotifier.Game.OnNearbyInteractableChanged.Dispatch(NearbyInteractable);
        }

        #endregion

        #region Callbacks

        void OnPlayerMoved()
        {
            RefreshNearbyInteractable();
        }

        void OnCloseDialogue()
        {
            ExitDialogue();
        }

        void OnActClicked()
        {
            Act();
        }

        void OnInteractableEnteredByPlayer(Interactable interactable)
        {
            if (!allNearbyInteractables.Contains(interactable))
                allNearbyInteractables.Add(interactable);
            RefreshNearbyInteractable();
        }

        void OnInteractableExitedByPlayer(Interactable interactable)
        {
            if (NearbyInteractable == interactable)
            {
                NearbyInteractable = null;
                UIManager.I.dialogues.HideSignal(interactable, true);
            }
            if (allNearbyInteractables.Contains(interactable))
                allNearbyInteractables.Remove(interactable);
            RefreshNearbyInteractable();
        }

        #endregion
    }
}
