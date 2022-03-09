using System;
using Antura.Core;
using Antura.UI;
using UnityEngine;

namespace Antura.Assessment
{
    /// <summary>
    /// Keeps linked IQuestion and LL Gameobject
    /// </summary>
    public class QuestionBehaviour : MonoBehaviour
    {
        private IQuestion question = null;

        public void ReadMeSound()
        {
            dialogues.PlayLetterData(GetComponent<StillLetterBox>().Data);
        }

        bool triggered = false;
        public void TurnFaceUp()
        {
            if (triggered)
            {
                return;
            }

            triggered = true;
            GetComponent<StillLetterBox>().RevealHiddenQuestion();
        }

        public void GreenyTintQuestion()
        {
            GetComponent<StillLetterBox>().SetQuestionGreen();
        }

        AssessmentAudioManager dialogues;
        public void SetQuestion(IQuestion qst, AssessmentAudioManager dialogues)
        {
            this.dialogues = dialogues;

            if (qst == null)
            {
                throw new ArgumentException("Null questions");
            }
            if (question == null)
            {
                question = qst;
            }
            else
            {
                throw new ArgumentException("Answer already added");
            }
        }

        public IQuestion GetQuestion()
        {
            return question;
        }

        void OnMouseDown()
        {
            if (GlobalUI.PauseMenu.IsMenuOpen)
                return;
            if (AssessmentOptions.Instance.PronunceQuestionWhenClicked)
            {
                ReadMeSound();
            }
        }

        public IQuestionDecoration questionAnswered;

        internal void OnQuestionAnswered()
        {
            if (AssessmentOptions.Instance.QuestionAnsweredPlaySound)
            {
                ReadMeSound();
            }

            if (AssessmentOptions.Instance.QuestionAnsweredFlip)
            {
                TurnFaceUp();
            }
            else
            {
                GreenyTintQuestion();
            }
        }

        internal void OnSpawned()
        {
            if (AssessmentOptions.Instance.QuestionSpawnedPlaySound)
            {
                ReadMeSound();
            }
        }

        internal float TimeToWait()
        {
            if (AssessmentOptions.Instance.QuestionAnsweredFlip || AssessmentOptions.Instance.QuestionAnsweredPlaySound)
            {
                return 1.0f;
            }
            else
            {
                return 0.05f;
            }
        }
    }
}
