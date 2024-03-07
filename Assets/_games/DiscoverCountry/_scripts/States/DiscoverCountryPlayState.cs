using System.Collections;
using Antura.Database;
using Antura.LivingLetters;
using Antura.Tutorial;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class DiscoverCountryPlayState : FSM.IState
    {
        private DiscoverCountryGame game;

        private int letterOnSequence;
        private bool isSequence;

        private int questionProgress;
        private int correctAnswers;

        private float nextStateTimer;
        private bool toNextState;

        private float inputButtonTime = 0.3f;
        private float inputButtonTimer;
        private int inputButtonCount;
        private int inputButtonMax = 4;
        private bool repeatInputHasProgressed;
        private bool enteredRepeatMode;

        private IAudioSource positiveAudioSource;

        private bool showTutorial;
        private bool tutorialCorrectActive;
        private int tutorialSequenceIndex;
        private float tutorialCorrectTimer;

        private float tutorialDelayTimer;
        private float tutorialDelayTime = 3f;
        private bool tutorialStop;

        bool isPlayingQuestion = false;

        public DiscoverCountryPlayState(DiscoverCountryGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            letterOnSequence = 0;

            questionProgress = 0;

            nextStateTimer = 0f;
            toNextState = false;

            inputButtonTimer = 0f;
            inputButtonCount = 0;
            repeatInputHasProgressed = false;
            enteredRepeatMode = false;

            showTutorial = game.ShowTutorial;
            game.EndTutorial();
            tutorialCorrectActive = false;
            tutorialDelayTimer = tutorialDelayTime;
            tutorialStop = false;

            if (!showTutorial)
            {
                game.InitializeOverlayWidget();
            }
        }

        public void ExitState()
        {
            if (showTutorial)
            {
                TutorialUI.Clear(true);
            }
        }

        public void Update(float delta)
        {
            if (toNextState)
            {
                nextStateTimer -= delta;

                if (nextStateTimer <= 0f)
                {
                    toNextState = false;

                    if (!showTutorial)
                    {
                        if (game.stagePositiveResult)
                        {
                            game.CurrentScore++;

                            ILivingLetterData runLetterData;
                        }

                    }

                    game.SetCurrentState(game.ResultState);
                }
            }

            inputButtonTimer -= delta;
        }

        public void UpdatePhysics(float delta) {}
    }
}
