using Antura.Tutorial;
using Kore.Coroutines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Assessment
{
    public class AnswerChecker
    {
        private AssessmentAudioManager audioManager;
        private AssessmentEvents events = null;

        public AnswerChecker(AssessmentAudioManager audioManager, AssessmentEvents events = null)
        {
            this.audioManager = audioManager;
            this.events = events;
        }

        private bool isAnimating = false;
        private bool allCorrect = false;

        // When all answers are correct return true
        public bool AllCorrect()
        {
            if (coroutineEnded)  // Needed to see All Correct only when animation ended
            {
                coroutineEnded = false;
                isAnimating = false;
                return allCorrect; // Value setted by CheckCoroutine
            }

            return false;
        }

        // When need to check validity of answers return true
        public bool AreAllAnswered(List<PlaceholderBehaviour> placeholders)
        {
            var count = AnswerSet.GetCorrectCount();
            int linkedDroppables = 0;
            foreach (var p in placeholders)
            {
                if (p.LinkedDroppable != null)
                {
                    linkedDroppables++;
                }
            }

            return linkedDroppables >= count;
        }

        public void Check(List<PlaceholderBehaviour> placeholders,
                            List<IQuestion> questions,
                            IDragManager dragManager)
        {
            isAnimating = true;
            coroutineEnded = false;
            allCorrect = false;
            Koroutine.Run(CheckCoroutine(placeholders, questions, dragManager));
        }

        private bool AreQuestionsCorrect(List<IQuestion> questions)
        {
            foreach (var q in questions)
            {
                if (q.GetAnswerSet().AllCorrect() == false)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CorrectSoundPlayed = false;
        private bool WrongSoundPlayed = false;

        private void PlayCorrectSound()
        {
            if (CorrectSoundPlayed == false)
            {
                audioManager.PlayStampSound();
                CorrectSoundPlayed = true;
            }
        }

        private void MarkYes(Vector3 position)
        {
            PlayCorrectSound();
            Koroutine.Run(MarkYesCoroutine(position));
        }

        /// <summary>
        /// Add some delay to the sound because it is not synchronized with TutorialUI.
        /// </summary>
        IEnumerator MarkYesCoroutine(Vector3 position)
        {
            yield return Wait.For(0.3f);
            TutorialUI.MarkYes(position, TutorialUI.MarkSize.Normal);
        }

        private void PlayWrongSound()
        {
            if (WrongSoundPlayed == false)
            {
                ItemFactory.Instance.GetAntura().WrongAssessment(audioManager);
                audioManager.PlayKOSound();
                WrongSoundPlayed = true;
            }
        }

        private bool coroutineEnded = false;
        private IEnumerator CheckCoroutine(List<PlaceholderBehaviour> placeholders,
                                            List<IQuestion> questions,
                                            IDragManager dragManager)
        {
            WrongSoundPlayed = false;
            CorrectSoundPlayed = false;
            dragManager.DisableInput();

            bool areAllCorrect = AreQuestionsCorrect(questions);
            if (areAllCorrect)
            {

                // Log learning progress
                foreach (var p in placeholders)
                    if (p.LinkedDroppable != null)
                    {
                        var set = p.Placeholder.GetQuestion().GetAnswerSet();
                        var answ = p.LinkedDroppable.GetAnswer();
                        if (set.IsCorrect(answ))
                        {
                            AssessmentConfiguration.Instance.Context.GetLogManager().OnAnswered(answ.Data(), true);
                        }
                        var pos = p.gameObject.transform.localPosition;
                        pos.y -= 3.5f;
                        MarkYes(pos);
                    }

                ItemFactory.Instance.GetAntura().CorrectAssessment(audioManager);

                // Just trigger OnQuestionAnswered events if all are correct
                foreach (var q in questions)
                {
                    yield return Wait.For(q.QuestionBehaviour.TimeToWait());
                    q.QuestionBehaviour.OnQuestionAnswered();
                    yield return Wait.For(q.QuestionBehaviour.TimeToWait());
                }

            }
            else
            {
                foreach (var p in placeholders)
                {
                    if (p.LinkedDroppable != null)
                    {
                        var set = p.Placeholder.GetQuestion().GetAnswerSet();
                        var answ = p.LinkedDroppable.GetAnswer();
                        if (set.IsCorrect(answ) == false)
                        {
                            AssessmentConfiguration.Instance.Context.GetLogManager().OnAnswered(answ.Data(), false);
                            PlayWrongSound();
                            p.LinkedDroppable.Detach(true);
                            var pos = p.gameObject.transform.localPosition;
                            pos.y -= 3.5f;
                            TutorialUI.MarkNo(pos, TutorialUI.MarkSize.Normal);
                        }
                        else
                        {
                            var pos = p.gameObject.transform.localPosition;
                            pos.y -= 3.5f;
                            MarkYes(pos);
                        }
                    }
                }
            }

            allCorrect = areAllCorrect;
            while (wrongAnswerAnimationPlaying)
            {
                yield return null;
            }

            if (allCorrect)
            {
                if (events != null && events.OnPreQuestionsAnswered != null)
                {
                    yield return new WaitCoroutine(events.OnPreQuestionsAnswered());
                }
                yield return Wait.For(1.0f);
            }
            else
            {
                wrongAnswerAnimationPlaying = true;
                Koroutine.Run(WrongAnswerCoroutine());
            }

            coroutineEnded = true;
            dragManager.EnableInput();
        }

        private bool wrongAnswerAnimationPlaying = false;

        private IEnumerator WrongAnswerCoroutine()
        {
            yield return Wait.For(0.51f);
            wrongAnswerAnimationPlaying = false;
        }

        IYieldable PlayAnswerWrong()
        {
            return audioManager.PlayAnswerWrong();
        }

        public bool IsAnimating()
        {
            return isAnimating;
        }
    }
}
