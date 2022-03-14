using UnityEngine;
using System.Collections;
using Antura.LivingLetters;
using Antura.Minigames;
using Antura.Tutorial;

namespace Antura.Minigames.MixedLetters
{
    public class SeparateLetterController : MonoBehaviour
    {
        // How fast the letter rotates when the rotate button is pressed, in degrees per second:
        private const float ROTATION_SPEED = 720f;

        private const float TUTORIAL_UI_PERIOD = 3f;

        public Rigidbody rigidBody;
        public BoxCollider boxCollider;

        //private float cameraDistance;
        private LL_LetterData letterData;

        [HideInInspector]
        public DropZoneController droppedZone;
        private DropZoneController correctDropZone;

        public LivingLetterController letterObjectView;

        private enum State
        {
            NonInteractive, Draggable, Dragging, Rotating, Dropped
        }

        private State state;
        private float stateTime;

        private bool isSubjectOfTutorial;

        void Awake()
        {
            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }

            boxCollider.enabled = true;
        }

        void Start()
        {
            IInputManager inputManager = MixedLettersConfiguration.Instance.Context.GetInputManager();
            inputManager.onPointerDown += OnPointerDown;
            inputManager.onPointerDrag += OnPointerDrag;
            inputManager.onPointerUp += OnPointerUp;

            //cameraDistance = Vector3.Distance(Camera.main.transform.position, transform.position);

            letterObjectView.SetState(LLAnimationStates.LL_still);
            letterObjectView.SetState(LLAnimationStates.LL_limbless);

        }

        private void SetState(State state)
        {
            this.state = state;
            stateTime = 0f;

            if (state == State.Draggable && isSubjectOfTutorial)
            {
                ShowTutorial();
            }
        }

        private void OnPointerDown()
        {
            if (state == State.Draggable || state == State.Dropped)
            {
                Ray ray = Camera.main.ScreenPointToRay(MixedLettersConfiguration.Instance.Context.GetInputManager().LastPointerPosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider == boxCollider)
                {
                    SetState(State.Dragging);
                    SetIsKinematic(true);

                    if (transform.position.z != DropZoneController.DropZoneZ)
                    {
                        Vector3 position = transform.position;
                        position.z = DropZoneController.DropZoneZ;
                        transform.position = position;
                    }

                    if (droppedZone != null)
                    {
                        droppedZone.SetDroppedLetter(null);
                        droppedZone = null;
                    }

                    MixedLettersConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(letterData, true, soundType: MixedLettersConfiguration.Instance.GetVocabularySoundType());
                }
            }
        }

        private void OnPointerDrag()
        {
            if (state == State.Dragging)
            {
                Vector2 lastPointerPosition = MixedLettersConfiguration.Instance.Context.GetInputManager().LastPointerPosition;
                Vector3 pointerPosInWorldUnits = Camera.main.ScreenToWorldPoint(new Vector3(lastPointerPosition.x, lastPointerPosition.y, Mathf.Abs(transform.position.z - Camera.main.transform.position.z)));

                transform.position = pointerPosInWorldUnits;
            }
        }

        private void OnPointerUp()
        {
            if (state == State.Dragging)
            {
                if (DropZoneController.chosenDropZone != null)
                {
                    droppedZone = DropZoneController.chosenDropZone;
                    droppedZone.SetDroppedLetter(this);
                    transform.position = droppedZone.transform.position;
                    DropZoneController.chosenDropZone = null;

                    SetState(State.Dropped);
                }

                else
                {
                    SetIsKinematic(false);
                    SetState(State.Draggable);
                }

                MixedLettersGame.instance.VerifyLetters();
            }
        }

        public bool IsRotating()
        {
            return state == State.Rotating;
        }

        void FixedUpdate()
        {
            rigidBody.AddForce(Constants.GRAVITY, ForceMode.Acceleration);
        }

        void Update()
        {
            stateTime += Time.deltaTime;

            if (isSubjectOfTutorial && stateTime >= TUTORIAL_UI_PERIOD)
            {
                ShowTutorial();
                stateTime = 0;
            }
        }

        public void SetIsSubjectOfTutorial(bool isSubjectOfTutorial)
        {
            this.isSubjectOfTutorial = isSubjectOfTutorial;

            if (isSubjectOfTutorial)
            {
                ShowTutorial();
            }
        }

        public void SetCorrectDropZone(DropZoneController dropZoneController)
        {
            correctDropZone = dropZoneController;
        }

        public void ShowTutorial()
        {
            if (state != State.NonInteractive)
            {
                if (droppedZone == null || droppedZone != correctDropZone)
                {
                    TutorialUI.DrawLine(transform.position, correctDropZone.transform.position, TutorialUI.DrawLineMode.FingerAndArrow);
                }

                else if (Mathf.Abs(transform.rotation.z) > 0.1f)
                {
                    Vector3 rotateButtonPosition = droppedZone.rotateButtonController.transform.position;
                    TutorialUI.Click(new Vector3(rotateButtonPosition.x, rotateButtonPosition.y, rotateButtonPosition.z - 1f));
                }
            }
        }

        public void SetIsKinematic(bool isKinematic)
        {
            rigidBody.isKinematic = isKinematic;
        }

        public void Vanish()
        {
            letterObjectView.Poof();
            Disable();
        }

        public void SetRotation(Vector3 eulerAngles)
        {
            transform.localRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
        }

        public void SetDraggable()
        {
            SetState(State.Draggable);
        }

        public void SetNonInteractive()
        {
            SetState(State.NonInteractive);
        }

        public void AddForce(Vector3 force, ForceMode forceMode)
        {
            rigidBody.AddForce(force, forceMode);
        }

        public void RotateCCW()
        {
            StartCoroutine(RotateCCWCoroutine());
        }

        private IEnumerator RotateCCWCoroutine()
        {
            SetState(State.Rotating);

            float currentZRotation = transform.localEulerAngles.z;
            float targetZRotation = currentZRotation + 90;

            float increment = Time.fixedDeltaTime * ROTATION_SPEED;

            while (true)
            {
                currentZRotation += increment;

                if (currentZRotation >= targetZRotation)
                {
                    SetRotation(new Vector3(0, 0, targetZRotation % 360));
                    break;
                }

                else
                {
                    SetRotation(new Vector3(0, 0, currentZRotation));
                }

                yield return new WaitForFixedUpdate();
            }

            SetState(State.Dropped);

            if (isSubjectOfTutorial)
            {
                ShowTutorial();
            }

            MixedLettersGame.instance.VerifyLetters();
        }

        public void Reset()
        {
            SetState(State.NonInteractive);
            SetIsKinematic(true);
            SetRotation(new Vector3(0, 180, 0));
            droppedZone = null;
            correctDropZone = null;
            isSubjectOfTutorial = false;
        }

        public void SetPosition(Vector3 position, bool offsetOnZ)
        {
            if (offsetOnZ)
            {
                position.z -= 1f;
            }

            transform.position = position;
        }

        public void SetPosition(Vector3 position)
        {

            transform.position = position;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void EnableCollider()
        {
            boxCollider.enabled = true;
        }

        public void DisableCollider()
        {
            boxCollider.enabled = false;
        }

        public void SetLetter(LL_LetterData letterData)
        {
            this.letterData = letterData;
            letterObjectView.Init(letterData);
        }

        public LL_LetterData GetLetter()
        {
            return letterData;
        }
    }
}
