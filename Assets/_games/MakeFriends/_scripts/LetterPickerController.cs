using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Antura.LivingLetters;

namespace Antura.Minigames.MakeFriends
{
    public class LetterPickerController : MonoBehaviour
    {
        public LetterChoiceController[] letterChoices;
        public Animator animator;
        public GameObject letterPickerBlocker;

        [HideInInspector]
        public bool IsBlocked { get { return letterPickerBlocker.activeSelf; } }

        [HideInInspector]
        public LetterChoiceController letterChoiceBeingDragged;

        [HideInInspector]
        public List<LetterChoiceController> IdleLetterChoices
        {
            get { return new List<LetterChoiceController>(letterChoices).FindAll(choice => choice.isActiveAndEnabled && choice.State == LetterChoiceController.ChoiceState.IDLE); }
        }

        [HideInInspector]
        public List<LetterChoiceController> CorrectLetterChoices
        {
            get { return new List<LetterChoiceController>(letterChoices).FindAll(choice => choice.isActiveAndEnabled && choice.isCorrectChoice); }
        }


        public void DisplayLetters(List<ILivingLetterData> letters)
        {
            for (int i = 0; i < letters.Count; i++)
            {
                letterChoices[i].gameObject.SetActive(true);
                letterChoices[i].Init(letters[i] as LL_LetterData);
            }
        }

        public void SetCorrectChoices(List<ILivingLetterData> commonLetters)
        {
            for (int i = 0; i < letterChoices.Length; i++)
            {
                for (int j = 0; j < commonLetters.Count; j++)
                {
                    if (letterChoices[i].letterData.Id == commonLetters[j].Id)
                    {
                        letterChoices[i].isCorrectChoice = true;
                    }
                }
            }
        }

        public void Reset()
        {
            Block();

            foreach (var prompt in letterChoices)
            {
                prompt.State = LetterChoiceController.ChoiceState.IDLE;
                prompt.gameObject.SetActive(false);
            }
        }

        public void Show()
        {
            animator.SetTrigger("Entrance");
        }

        public void BlockForSeconds(float duration)
        {
            StopCoroutine("BlockForSeconds_Coroutine");
            StartCoroutine("BlockForSeconds_Coroutine", duration);
        }

        public IEnumerator BlockForSeconds_Coroutine(float duration)
        {
            Block();
            yield return new WaitForSeconds(duration);
            Unblock();
        }

        public void ShowAndUnblockDelayed(float delay)
        {
            StopCoroutine("ShowAndUnblockDelayed_Coroutine");
            StartCoroutine("ShowAndUnblockDelayed_Coroutine", delay);
        }

        private IEnumerator ShowAndUnblockDelayed_Coroutine(float delay)
        {
            while (MakeFriendsGame.Instance.SpokenWords < 2)
                yield return null;

            yield return new WaitForSeconds(delay);
            Show();
            Unblock();
        }

        public void Hide()
        {
            animator.SetTrigger("Exit");
        }

        public void Block()
        {
            letterPickerBlocker.SetActive(true);
        }

        public void Unblock()
        {
            letterPickerBlocker.SetActive(false);
        }
    }
}
