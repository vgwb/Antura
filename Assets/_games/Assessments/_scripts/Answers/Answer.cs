using Antura.LivingLetters;
using System.Collections;
using Antura.Core;
using Antura.UI;
using UnityEngine;

namespace Antura.Assessment
{
    /// <summary>
    /// This class is an answer to a question. It provide a simplified interface
    /// compared to LLs and few additionals utilities for checking correctness
    /// and equality with other answers.
    /// </summary>
    public class Answer : MonoBehaviour
    {
        private ILivingLetterData data;
        private AssessmentAudioManager dialogues;
        private bool isCorrect;

        public Answer Init(bool correct, AssessmentAudioManager dialogues, ILivingLetterData data)
        {
            this.data = data;
            isCorrect = correct;
            this.dialogues = dialogues;
            return this;
        }

        /// <summary>
        /// Is this a correct answer?
        /// </summary>
        public bool IsCorrect()
        {
            return isCorrect;
        }

        /// <summary>
        /// Compare content of the answer
        /// </summary>
        /// <param name="other"> other answer content</param>
        public bool Equals(Answer other)
        {
            return AssessmentConfiguration.Instance.IsDataMatching(Data(), other.Data());
        }

        /// <summary>
        /// Regular override of Equals/GetHashCode
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Answer)
            {
                return this.Equals(obj as Answer);
            }
            return false;
        }

        /// <summary>
        /// Regular override of Equals/GetHashCode
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// The data of the living letter
        /// </summary>
        public ILivingLetterData Data()
        {
            return data;
        }

        /// <summary>
        /// Play letter sound if global options require that.
        /// </summary>
        void OnMouseDown()
        {
            if (GlobalUI.PauseMenu.IsMenuOpen)
                return;
            if (AssessmentOptions.Instance.PronunceAnswerWhenClicked)
            {
                dialogues.PlayLetterData(Data());
            }
        }

        public IEnumerator PlayLetter()
        {
            return dialogues.PlayLetterDataCoroutine(Data());
        }
    }
}
