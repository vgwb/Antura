using Antura.LivingLetters;
using UnityEngine;
using TMPro;

namespace Antura.Minigames.Balloons
{
    public class LetterPromptController : MonoBehaviour
    {
        public TMP_Text LetterLabel;
        public LL_LetterData LetterData;
        public Animator animator;

        public enum PromptState
        {
            IDLE,
            CORRECT,
            WRONG
        }
        private PromptState _state;
        public PromptState State
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

        public void Init(LL_LetterData _letterData)
        {
            LetterData = _letterData;
            LetterLabel.text = LetterData.TextForLivingLetter;
        }

        void OnStateChanged()
        {
            switch (State)
            {
                case PromptState.IDLE:
                    //GetComponent<Image>().color = Color.white;
                    animator.SetBool("Idle", true);
                    animator.SetBool("Correct", false);
                    animator.SetBool("Wrong", false);
                    break;
                case PromptState.CORRECT:
                    //GetComponent<Image>().color = Color.green;
                    animator.SetBool("Idle", false);
                    animator.SetBool("Correct", true);
                    animator.SetBool("Wrong", false);
                    break;
                case PromptState.WRONG:
                    //GetComponent<Image>().color = Color.red;
                    animator.SetBool("Idle", false);
                    animator.SetBool("Correct", false);
                    animator.SetBool("Wrong", true);
                    break;
                default:
                    break;
            }
        }
    }
}
