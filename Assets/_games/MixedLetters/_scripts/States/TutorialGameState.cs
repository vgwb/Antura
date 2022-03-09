using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace Antura.Minigames.MixedLetters
{
    public class TutorialGameState : FSM.IState
    {
        MixedLettersGame game;

        private IAudioManager audioManager;

        public TutorialGameState(MixedLettersGame game)
        {
            this.game = game;
            audioManager = game.Context.GetAudioManager();
        }

        public void EnterState()
        {
            if (game.TutorialEnabled)
            {
                game.DisableRepeatPromptButton();
                game.GenerateNewWord();

                VictimLLController.instance.HideVictoryRays();
                VictimLLController.instance.Reset();
                VictimLLController.instance.Enable();

                Vector3 victimLLPosition = VictimLLController.instance.transform.position;
                victimLLPosition.x = Random.Range(0, 40) % 2 == 0 ? 0.5f : -0.5f;
                VictimLLController.instance.SetPosition(victimLLPosition);
            }

            if (!game.TutorialEnabled)
            {
                game.SetCurrentState(game.IntroductionState);
                return;
            }

            game.StartCoroutine(OnTitleVoiceOverDoneCoroutine());
        }

        private IEnumerator OnTitleVoiceOverDoneCoroutine()
        {
            yield return new WaitForSeconds(0.75f);
            game.SayQuestion(OnQuestionOver);
        }

        private void OnQuestionOver()
        {
            game.StartCoroutine(OnQuestionOverCoroutine());
        }

        private IEnumerator OnQuestionOverCoroutine()
        {
            yield return new WaitForSeconds(MixedLettersConfiguration.Instance.Variation == MixedLettersVariation.Alphabet ? 1.5f : 3f);

            MixedLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.DogBarking);
            AnturaController.instance.PrepareToEnterScene();
            VictimLLController.instance.LookTowardsAntura();

            yield return new WaitForSeconds(0.25f);

            AnturaController.instance.Enable();
            AnturaController.instance.EnterScene(OnFightBegan, OnAnturaExitedScene);
        }

        private void OnFightBegan()
        {
            AnturaController.instance.SetPosition(VictimLLController.instance.transform.position);
            AnturaController.instance.Disable();
            VictimLLController.instance.Disable();
            ParticleSystemController.instance.Enable();
            ParticleSystemController.instance.SetPosition(VictimLLController.instance.transform.position);
            SeparateLettersSpawnerController.instance.SetPosition(VictimLLController.instance.transform.position);
            SeparateLettersSpawnerController.instance.SpawnLetters(game.PromptLettersInOrder, OnFightEnded);
        }

        private void OnFightEnded()
        {
            AnturaController.instance.Enable();
            AnturaController.instance.SetPositionWithOffset(VictimLLController.instance.transform.position, new Vector3(0, 0, 1f));
            ParticleSystemController.instance.Disable();
        }

        private void OnAnturaExitedScene()
        {
            game.PlayIntro(OnIntroVoiceOverDone);
        }

        private void OnIntroVoiceOverDone()
        {
            MixedLettersGame.instance.OnRoundStarted();
            game.EnableRepeatPromptButton();
            audioManager.PlayDialogue(MixedLettersConfiguration.Instance.TutorialLocalizationId);
        }

        public void ExitState()
        {

        }

        public void Update(float delta)
        {
            if (game.WasLastRoundWon)
            {
                game.SetCurrentState(game.ResultState);
            }
        }

        public void UpdatePhysics(float delta)
        {

        }
    }
}
