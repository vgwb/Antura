using Antura.LivingLetters;
using Antura.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Antura.Minigames.MakeFriends
{
    public class LetterChoiceController : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public TextRender LetterText;
        public Animator animator;
        public Image image;
        public Button button;
        public CanvasGroup canvasGroup;
        public float canvasDistance;

        [HideInInspector]
        public ILivingLetterData letterData;
        [HideInInspector]
        public bool isCorrectChoice;
        [HideInInspector]
        public bool wasChosen;

        [HideInInspector]
        public bool IsDisabled { get { return disabled; } }

        public enum ChoiceState
        {
            IDLE,
            CORRECT,
            WRONG
        }

        public ChoiceState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    OnStateChanged();
                }
            }
        }

        private ChoiceState _state;
        private bool disabled;
        private Vector3 initialPosition = new Vector3();
        private Vector3 dragPosition = new Vector3();


        public void Init(ILivingLetterData _letterData)
        {
            Reset();
            letterData = _letterData;
            LetterText.SetLetterData(_letterData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (disabled)
            {
                return;
            }

            //Disable();
            SpeakLetter();
            //MakeFriendsGameManager.Instance.OnClickedLetterChoice(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (disabled)
            {
                return;
            }

            initialPosition = transform.position;
            MakeFriendsGame.Instance.letterPicker.letterChoiceBeingDragged = this;
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (disabled)
            {
                return;
            }
            dragPosition.Set(eventData.position.x, eventData.position.y, canvasDistance);
            dragPosition = Camera.main.ScreenToWorldPoint(dragPosition);
            dragPosition.z = initialPosition.z;
            transform.position = dragPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (disabled)
            {
                return;
            }

            if (wasChosen)
            {
                Disable();
                MakeFriendsGame.Instance.OnLetterChoiceSelected(this);
            }
            else
            {
                transform.position = initialPosition;
            }

            MakeFriendsGame.Instance.letterPicker.letterChoiceBeingDragged = null;
            canvasGroup.blocksRaycasts = true;
        }

        public void SpeakLetter()
        {
            if (letterData != null && letterData.Id != null)
            {
                MakeFriendsConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(letterData, soundType: MakeFriendsConfiguration.Instance.GetVocabularySoundType());
            }
        }

        public void FlashWrong()
        {
            animator.SetTrigger("FlashWrong");
        }

        public void SpawnBalloon(bool correctChoice)
        {
            var balloon = Instantiate(MakeFriendsGame.Instance.letterBalloonPrefab, MakeFriendsGame.Instance.letterBalloonContainer.transform.position, Quaternion.identity, MakeFriendsGame.Instance.letterBalloonContainer.transform) as GameObject;
            var balloonController = balloon.GetComponent<LetterBalloonController>();
            balloonController.Init(letterData);
            balloonController.EnterScene(correctChoice);
        }

        private void Disable()
        {
            disabled = true;
            image.enabled = false;
            button.enabled = false;
            LetterText.enabled = false;
            LetterText.gameObject.SetActive(false);
        }

        private void Reset()
        {
            disabled = false;
            wasChosen = false;
            image.enabled = true;
            button.enabled = true;
            LetterText.enabled = true;
            State = ChoiceState.IDLE;
            LetterText.gameObject.SetActive(true);
        }

        private void OnStateChanged()
        {
            switch (State)
            {
                case ChoiceState.IDLE:
                    animator.SetTrigger("Idle");
                    break;
                case ChoiceState.CORRECT:
                    animator.SetTrigger("Correct");
                    break;
                case ChoiceState.WRONG:
                    animator.SetTrigger("Wrong");
                    break;
                default:
                    break;
            }
        }
    }
}
