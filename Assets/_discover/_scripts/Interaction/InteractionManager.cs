using System.Collections;
using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Antura.Discover
{
    public class InteractionManager : MonoBehaviour
    {
        #region Serialized

        public PlayerController player;

        #endregion

        public static InteractionManager I { get; private set; }
        public int LastActionFrame { get; private set; }
        public bool IsUsingFocusView { get; private set; }
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
            switch (DiscoverGameManager.I.State)
            {
                case GameplayState.Play3D:
                    UpdateWorld();
                    break;
                case GameplayState.Dialogue:
                    UpdateDialogue();
                    break;
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
                UnfocusCam();
        }

        #endregion

        #region Public Methods

        public void FocusCameraOn(Transform target)
        {
            IsUsingFocusView = true;
            focusViewEnterFrame = Time.frameCount;
            CameraManager.I.FocusCamOn(target);
            CameraManager.I.ChangeCameraMode(CameraMode.Focus);
            UIManager.I.gameObject.SetActive(false);
        }

        /// <summary>
        /// Activates the world icon for the light beam
        /// </summary>
        /// <param name="activate">TRUE to activate, FALSE otherwise</param>
        /// <param name="target">Required only if activating, the target to follow</param>
        public void ActivateWorldTargetIcon(bool activate, Transform target = null)
        {
            UIManager.I.ActivateWorldTargetMarker(activate, target);
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
            if (IsUsingFocusView && focusViewEnterFrame != Time.frameCount)
                UnfocusCam();
            if (DiscoverGameManager.I.State != GameplayState.Play3D)
                return;

            // Debug.Log("InteractionManager: Act() called + " + HasValidNearbyInteractable);

            if (HasValidNearbyInteractable)
            {
                QuestNode questNode = NearbyInteractable.Execute();
                if (questNode != null)
                    this.RestartCoroutine(ref coStartDialogue, CO_StartDialogue(questNode, NearbyInteractable));
            }
        }

        public void DisplayNode(QuestNode node)
        {
            if (node == null)
            {
                Debug.LogError("QuestNode is NULL, shouldn't happen");
                return;
            }

            DiscoverNotifier.Game.OnStartDialogue.Dispatch();
            UIManager.I.dialogues.StartDialogue(node);

        }

        IEnumerator CO_StartDialogue(QuestNode questNode, Interactable interactable)
        {
            DiscoverGameManager.I.ChangeState(GameplayState.Dialogue);
            DiscoverNotifier.Game.OnStartDialogue.Dispatch();

            if (NearbyInteractable.IsLL)
                NearbyInteractable.LL.LookAt(player.transform);

            if (NearbyInteractable.FocusCameraOnInteract)
            {
                CameraManager.I.ChangeCameraMode(CameraMode.Dialogue);
                CameraManager.I.FocusDialogueCamOn(NearbyInteractable.LookAtTransform);
            }
            UIManager.I.dialogues.HideSignal(interactable, false);

            if (questNode == null)
            {
                Debug.LogError("QuestNode is NULL, shouldn't happen");
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                UIManager.I.dialogues.StartDialogue(questNode);
            }
            coStartDialogue = null;
        }

        void ExitDialogue()
        {
            DiscoverGameManager.I.ChangeState(DiscoverGameManager.I.LastPlayState);
            CameraManager.I.ChangeCameraMode(CameraMode.Player);
            if (HasValidNearbyInteractable)
                UIManager.I.dialogues.ShowSignalFor(NearbyInteractable);
            this.CancelCoroutine(ref coStartDialogue);
            UIManager.I.dialogues.CloseDialogue();
        }

        void UnfocusCam()
        {
            IsUsingFocusView = false;
            CameraManager.I.ChangeCameraMode(CameraMode.Dialogue);
            UIManager.I.gameObject.SetActive(true);
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
                    UIManager.I.dialogues.HideSignal(prev, true);
                if (NearbyInteractable != null && HasValidNearbyInteractable)
                    UIManager.I.dialogues.ShowSignalFor(NearbyInteractable);
            }
        }

        void SetNearbyInteractableTo(Interactable interactable)
        {
            NearbyInteractable = interactable;
            UIManager.I.dialogues.ShowSignalFor(NearbyInteractable);
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
