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

            bool onlyLetter = Random.Range(0, 2) == 0;

            game.CurrentQuestion = new EggChallenge(game.GameDifficulty, onlyLetter);
            game.eggController.Reset();

            if (firstQuestion)
            {
                game.Context.GetAudioManager().PlayDialogue(EggConfiguration.Instance.TitleLocalizationId);
            }

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
            {
                game.Context.GetAudioManager().PlayDialogue(Database.LocalizationDataId.Egg_Intro, delegate () { SetAndShowEggButtons(); });
            }
            else
            {
                SetAndShowEggButtons();
            }
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

            ShowQuestionSequence();
        }

        void ShowQuestionSequence()
        {
            bool lightUpButtons = game.GameDifficulty < 0.5f;

            bool isSequence = game.CurrentQuestion.IsSequence();

            if (isSequence)
            {
                game.eggController.SetQuestion(game.CurrentQuestion.Question);
                game.eggController.SetAnswers(game.CurrentQuestion.Answers);
                game.eggButtonBox.PlayButtonsAudio(game.CurrentQuestion.Question, null, lightUpButtons, false, 0f, OnQuestionAudioComplete);
            }
            else
            {
                game.eggController.SetQuestion(null);
                game.eggController.SetAnswers(game.CurrentQuestion.Answers[0]);

                if (lightUpButtons)
                {
                    game.eggController.PlayAudioQuestion(delegate ()
                    {
                        EnableEggButtonsInput();
                        game.eggButtonBox.PlayButtonsAudio(null, null, true, true, 0.5f, OnQuestionAudioComplete);
                    });
                }
                else
                {
                    game.eggController.PlayAudioQuestion(OnQuestionAudioComplete);
                }
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
            if (passed) return;
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