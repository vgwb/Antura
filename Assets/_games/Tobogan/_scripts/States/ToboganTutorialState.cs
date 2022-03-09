using Antura.LivingLetters;
using Antura.Tutorial;
using Antura.Minigames;
using UnityEngine;

namespace Antura.Minigames.Tobogan
{
    public class ToboganTutorialState : FSM.IState
    {
        ToboganGame game;

        float delayStartTutorial;
        bool tutorialStarted;

        bool toPlayState;
        float toPlayStateTimer;

        bool pointerUp;

        bool sunMoonGameVariation;
        int questionCount;

        float nextQuestionTimer;
        float nextQuestiontime = 1f;
        bool requestNextQueston;

        public ToboganTutorialState(ToboganGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.pipesAnswerController.SetSignHidingProbability(0);
            game.questionsManager.onAnswered += OnAnswered;
            game.questionsManager.playerInputPointerUp += OnPointerUp;

            game.questionsManager.StartNewQuestion();
            game.questionsManager.Enabled = true;

            tutorialStarted = false;
            delayStartTutorial = 1f;

            toPlayState = false;
            toPlayStateTimer = 1f;

            pointerUp = true;

            questionCount = 0;

            sunMoonGameVariation = ToboganConfiguration.Instance.Variation == ToboganVariation.SunMoon;

            game.Context.GetAudioManager().PlayDialogue(ToboganConfiguration.Instance.TutorialLocalizationId);

            nextQuestionTimer = 0f;
            requestNextQueston = false;

            game.questionsManager.SetEnteringAudio(false);
        }

        public void ExitState()
        {
            game.questionsManager.onAnswered -= OnAnswered;
            game.questionsManager.playerInputPointerUp -= OnPointerUp;

            TutorialUI.Clear(true);

            game.questionsManager.SetEnteringAudio(true);
            game.pipesAnswerController.SetSignHidingProbability(2 * Mathf.Clamp01(ToboganGame.I.Difficulty - 0.5f));
        }

        public void Update(float delta)
        {
            if (!tutorialStarted)
            {
                delayStartTutorial += -delta;

                if (delayStartTutorial <= 0f)
                {
                    tutorialStarted = true;
                    TutorialDrawLine();
                }
            }

            if (toPlayState)
            {
                toPlayStateTimer -= delta;

                if (toPlayStateTimer <= 0f)
                {
                    toPlayState = false;
                    game.SetCurrentState(game.PlayState);
                }
            }

            if (pointerUp && tutorialStarted)
            {
                tutorialStarted = false;
                delayStartTutorial = 3f;
            }

            if (requestNextQueston)
            {
                nextQuestionTimer -= delta;

                if (nextQuestionTimer <= 0f)
                {
                    game.questionsManager.StartNewQuestion();
                    requestNextQueston = false;
                }
            }
        }

        public void UpdatePhysics(float delta) { }

        void OnAnswered(IQuestionPack pack, bool result)
        {
            if (result)
            {
                questionCount++;

                game.questionsManager.OnQuestionEnd(0);

                if (!sunMoonGameVariation || (sunMoonGameVariation && questionCount == 2))
                {
                    toPlayState = true;
                }
                else
                {
                    tutorialStarted = false;
                    delayStartTutorial = 2f;

                    requestNextQueston = true;
                    nextQuestionTimer = nextQuestiontime;
                }
            }
            else
            {
                TutorialMarkNo();
            }
        }

        void OnPointerUp(bool pointerUp)
        {
            if (!pointerUp)
            {
                tutorialStarted = true;

                TutorialUI.Clear(false);
            }

            this.pointerUp = pointerUp;
        }

        void TutorialDrawLine()
        {
            Vector3 lineFrom = game.questionsManager.GetQuestionLivingLetter().letter.contentTransform.position;
            Vector3 lineTo = game.pipesAnswerController.GetCorrectPipeAnswer().tutorialPoint.position;

            TutorialUI.DrawLine(lineFrom, lineTo, TutorialUI.DrawLineMode.FingerAndArrow);
        }

        void TutorialMarkNo()
        {
            Vector3 markNoPosition = game.pipesAnswerController.GetCurrentPipeAnswer().tutorialPoint.position;

            TutorialUI.MarkNo(markNoPosition);
        }
    }
}
