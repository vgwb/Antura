using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Antura.LivingLetters;
using Antura.Minigames;

namespace Antura.Minigames.Egg
{
    public class EggQuestionState : FSM.IState
    {
        EggGame game;

        bool firstQuestion;

        public EggQuestionState(EggGame game)
        {
            this.game = game;

            firstQuestion = true;
        }

        public void EnterState()
        {
            passed = false;
            game.eggButtonBox.RemoveButtons();

            game.CurrentQuestion = new EggChallenge(game.Difficulty);
            game.eggController.Reset();

            EggEnter();
        }

        public void ExitState()
        {
            firstQuestion = false;

            game.eggButtonBox.SetOnPressedCallback(null);
        }

        public void Update(float delta)
        {
        }
        public void UpdatePhysics(float delta) { }

        void EggEnter()
        {
            game.Context.GetAudioManager().PlaySound(Sfx.TickAndWin);
            game.eggController.MoveNext(1.3f, OnEggEnterComplete);
        }

        void OnEggEnterComplete()
        {
            if (firstQuestion)
                game.PlayIntro(PlayTutorial);
            else
                PlayTutorial();
        }

        void PlayTutorial()
        {
            if (game.ShowTutorial)
                game.PlayTutorial(SetAndShowEggButtons);
            else
                SetAndShowEggButtons();
        }

        void SetAndShowEggButtons()
        {
            List<ILivingLetterData> lLetterDataSequence = game.CurrentQuestion.Answers;

            for (int i = 0; i < lLetterDataSequence.Count; i++)
            {
                var data = lLetterDataSequence[i];
                if (EggConfiguration.Instance.Variation == EggVariation.Image)
                    data = new LL_ImageData(data.Id);

                game.eggButtonBox.AddButton(data);
            }

            game.eggButtonBox.SetButtonsOnPosition();
            game.eggButtonBox.ShowButtons();
            game.eggButtonBox.SetOnPressedCallback(OnEggButtonPressed);

            game.eggController.EmoticonInterrogative();

            game.StartCoroutine(ShowQuestionSequence());
        }

        IEnumerator ShowQuestionSequence()
        {
            bool isSequence = game.CurrentQuestion.IsSequence();

            if (isSequence)
            {
                game.eggController.SetQuestion(game.CurrentQuestion.Question);
                game.eggController.SetAnswers(game.CurrentQuestion.Answers);
                yield return game.eggButtonBox.PlayButtonsAudio(game.CurrentQuestion.Question, null, true, false, 0f, OnQuestionAudioComplete, yieldDuration: true);
            }
            else
            {
                game.eggController.SetQuestion(null);
                game.eggController.SetAnswers(game.CurrentQuestion.Answers[0]);
                yield return game.eggButtonBox.PlayButtonsAudio(game.CurrentQuestion.Question, null, true, true, 0.5f, OnQuestionAudioComplete);
                EnableEggButtonsInput();
            }
        }

        void OnEggButtonPressed(ILivingLetterData letterData)
        {
            if (!game.CurrentQuestion.IsSequence())
            {
                game.eggButtonBox.StopButtonsAudio();
            }

            game.PlayState.OnEggButtonPressed(letterData);
        }

        private bool passed = false;
        void OnQuestionAudioComplete()
        {
            if (passed)
                return;
            passed = true;

            DisableEggButtonsInput();

            game.eggController.EmoticonClose();

            game.SetCurrentState(game.PlayState);
        }

        void EnableEggButtonsInput()
        {
            game.eggButtonBox.EnableButtonsInput();
        }

        void DisableEggButtonsInput()
        {
            game.eggButtonBox.DisableButtonsInput();
        }
    }
}
