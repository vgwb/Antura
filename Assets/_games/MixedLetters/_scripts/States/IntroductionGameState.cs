using Antura.UI;
using UnityEngine;

namespace Antura.Minigames.MixedLetters
{
    public class IntroductionGameState : FSM.IState
    {
        MixedLettersGame game;

        private bool uiInitialised = false;

        private float anturaEnterTimer;
        private bool anturaEntered = false;
        private bool anturaBarked = false;
        //private float anturaExitTimer;
        //private bool anturaExited = false;

        private bool isAnturaEnterTimerActivated = false;

        private float timePerRound
        {
            get
            {
                float difficulty = game.Difficulty;

                float t;
                if (difficulty <= 0.25f)
                {
                    t = 60f;
                }
                else if (difficulty <= 0.5f)
                {
                    t = 50f;
                }
                else if (difficulty <= 0.75f)
                {
                    t = 40f;
                }
                else
                {
                    t = 30f;
                }

                return t;
            }
        }

        public IntroductionGameState(MixedLettersGame game)
        {
            this.game = game;
        }

        private void OnQuestionOver()
        {
            isAnturaEnterTimerActivated = true;
        }

        public void EnterState()
        {
            if (!game.WasLastRoundWon)
            {
                game.GenerateNewWord();
            }

            anturaEnterTimer = MixedLettersConfiguration.Instance.Variation == MixedLettersVariation.BuildWord ? 3.25f : 1.5f;
            anturaEntered = false;
            anturaBarked = false;
            //anturaExitTimer = Random.Range(0.75f, 1.5f);
            //anturaExited = false;

            isAnturaEnterTimerActivated = false;

            game.DisableRepeatPromptButton();

            //game.GenerateNewWord();
            game.SayQuestion(OnQuestionOver);

            VictimLLController.instance.Reset();
            VictimLLController.instance.Enable();

            /*Vector3 victimLLPosition = VictimLLController.instance.transform.position;
            victimLLPosition.x = Random.Range(0, 40) % 2 == 0 ? 0.5f : -0.5f;
            VictimLLController.instance.SetPosition(victimLLPosition);*/

            if (!uiInitialised)
            {
                uiInitialised = true;

                game.Context.GetOverlayWidget().Initialize(true, true, false);
                game.Context.GetOverlayWidget().SetStarsThresholds(game.STARS_1_THRESHOLD, game.STARS_2_THRESHOLD, game.STARS_3_THRESHOLD);
            }

            MinigamesUI.Timer.Setup(timePerRound);
            MinigamesUI.Timer.Rewind();
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            if (isAnturaEnterTimerActivated)
            {
                anturaEnterTimer -= delta;
            }

            if (anturaEnterTimer < 0.25f && !anturaBarked)
            {
                MixedLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.DogBarking);
                //AnturaController.instance.Enable();
                AnturaController.instance.PrepareToEnterScene();
                VictimLLController.instance.LookTowardsAntura();
                anturaBarked = true;
            }

            if (anturaEnterTimer < 0 && !anturaEntered)
            {
                AnturaController.instance.Enable();
                AnturaController.instance.EnterScene(OnFightBegan, OnAnturaExitedScene);
                anturaEntered = true;
            }
        }

        private void OnAnturaExitedScene()
        {
            game.SetCurrentState(game.PlayState);
        }

        public void UpdatePhysics(float delta)
        {
        }

        public void OnFightBegan()
        {
            AnturaController.instance.SetPosition(VictimLLController.instance.transform.position);
            AnturaController.instance.Disable();
            VictimLLController.instance.Disable();
            ParticleSystemController.instance.Enable();
            ParticleSystemController.instance.SetPosition(VictimLLController.instance.transform.position);
            SeparateLettersSpawnerController.instance.SetPosition(VictimLLController.instance.transform.position);
            SeparateLettersSpawnerController.instance.SpawnLetters(game.PromptLettersInOrder, OnFightEnded);
        }

        public void OnFightEnded()
        {
            AnturaController.instance.Enable();
            AnturaController.instance.SetPositionWithOffset(VictimLLController.instance.transform.position, new Vector3(0, 0, 1f));
            ParticleSystemController.instance.Disable();
            //anturaExited = true;
        }
    }
}
