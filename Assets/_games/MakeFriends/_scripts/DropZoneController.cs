using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Antura.LivingLetters;
using TMPro;

namespace Antura.Minigames.MakeFriends
{
    public class DropZoneController : MonoBehaviour, IDropHandler, IPointerEnterHandler
    {
        public TMP_Text LetterText;
        public Animator animator;

        [HideInInspector]
        public ILivingLetterData letterData;


        public void OnDrop(PointerEventData eventData)
        {
            var draggedLetter = MakeFriendsGame.Instance.letterPicker.letterChoiceBeingDragged;

            if (draggedLetter != null)
            {
                draggedLetter.wasChosen = true;
                DisplayText(draggedLetter.letterData);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var draggedLetter = MakeFriendsGame.Instance.letterPicker.letterChoiceBeingDragged;

            if (draggedLetter != null)
            {
                animator.SetTrigger("Highlight");
            }
        }

        public void DisplayText(ILivingLetterData _letterData)
        {
            letterData = _letterData;
            LetterText.text = letterData.TextForLivingLetter;//ArabicAlphabetHelper.GetLetterFromUnicode(letterData.Data.Isolated_Unicode);
        }


        public void Appear()
        {
            StartCoroutine(Appear_Coroutine());
        }

        private IEnumerator Appear_Coroutine()
        {
            while (MakeFriendsGame.Instance.SpokenWords < 2)
                yield return null;

            animator.SetTrigger("Appear");
        }

        public void Disappear(float delay = 0f)
        {
            if (delay > 0f)
            {
                StartCoroutine(Disappear_Coroutine(delay));
            }
            else
            {
                animator.SetTrigger("Disappear");
            }
        }

        private IEnumerator Disappear_Coroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            Disappear();
        }

        public void AnimateCorrect()
        {
            animator.SetTrigger("Correct");
        }

        public void AnimateWrong()
        {
            animator.SetTrigger("Wrong");
        }

        public void ResetLetter(float delay = 0f)
        {
            if (delay > 0f)
            {
                StartCoroutine(ResetLetter_Coroutine(delay));
            }
            else
            {
                letterData = null;
                LetterText.text = string.Empty;
            }
        }

        private IEnumerator ResetLetter_Coroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            ResetLetter();
        }

        public void Reset()
        {
            ResetLetter();
            animator.SetTrigger("Hidden");
            transform.localScale = Vector3.forward;
        }

    }
}