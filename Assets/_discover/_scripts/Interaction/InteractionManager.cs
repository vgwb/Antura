using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Antura.Discover
{
    public class InteractionManager : MonoBehaviour
    {
        #region Serialized

        public EdPlayer player;

        #endregion

        public static InteractionManager I { get; private set; }
        public int LastActionFrame { get; private set; }
        public bool IsUsingFocusView { get; private set; }
        public bool HasValidNearbyInteractable => nearbyInteractable != null && nearbyInteractable.gameObject.activeInHierarchy;

        public Interactable nearbyInteractable { get; private set; }
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
            if (nearbyInteractable != null && !HasValidNearbyInteractable)
            {
                DiscoverNotifier.Game.OnInteractableExitedByPlayer.Dispatch(nearbyInteractable);
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
            if (nearbyInteractable != interactable)
                SetNearbyInteractableTo(interactable);
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

            Debug.Log("InteractionManager: Act() called + " + HasValidNearbyInteractable);

            if (HasValidNearbyInteractable)
            {
                QuestNode questNode = nearbyInteractable.Execute();
                if (questNode != null)
                    this.RestartCoroutine(ref coStartDialogue, CO_StartDialogue(questNode, nearbyInteractable));
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

            if (nearbyInteractable.IsLL)
                nearbyInteractable.LL.LookAt(player.transform);

            if (nearbyInteractable.FocusCameraOnInteract)
            {
                CameraManager.I.ChangeCameraMode(CameraMode.Dialogue);
                CameraManager.I.FocusDialogueCamOn(nearbyInteractable.LookAtTransform);
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
                UIManager.I.dialogues.ShowSignalFor(nearbyInteractable);
            this.CancelCoroutine(ref coStartDialogue);
            UIManager.I.dialogues.CloseDialogue();
        }

        void UnfocusCam()
        {
            IsUsingFocusView = false;
            CameraManager.I.ChangeCameraMode(CameraMode.Dialogue);
            UIManager.I.gameObject.SetActive(true);
        }

        void SetNearbyInteractableTo(Interactable interactable)
        {
            nearbyInteractable = interactable;
            UIManager.I.dialogues.ShowSignalFor(nearbyInteractable);
        }

        #endregion

        #region Callbacks

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
            SetNearbyInteractableTo(interactable);
        }

        void OnInteractableExitedByPlayer(Interactable interactable)
        {
            if (nearbyInteractable == interactable)
            {
                nearbyInteractable = null;
                UIManager.I.dialogues.HideSignal(interactable, true);
            }
        }

        #endregion
    }
}
