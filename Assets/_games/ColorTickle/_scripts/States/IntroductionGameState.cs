using System;
using UnityEngine;
using System.Linq;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using Antura.LivingLetters;

namespace Antura.Minigames.ColorTickle
{
    public class IntroductionGameState : FSM.IState
    {
        ColorTickleGame game;

        float timer = 1f;
        private bool PerformTutorial;

        public IntroductionGameState(ColorTickleGame game, bool PerformTutorial)
        {
            this.game = game;
            this.PerformTutorial = PerformTutorial;
        }

        public void EnterState()
        {
            game.colorsCanvas.gameObject.SetActive(false);

            game.Context.GetAudioManager().PlayMusic(game.backgroundMusic);

            BuildLetters();

            BuildTutorialLetter();

            game.tutorialUIManager = game.gameObject.GetComponent<TutorialUIManager>();
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            timer -= delta;

            if (timer < 0)
            {
                for (int i = 0; i < game.rounds; ++i)
                {
                    game.myLetters[i].gameObject.SetActive(false);
                }
                game.tutorialLetter.gameObject.SetActive(false);

                if (PerformTutorial)
                {
                    game.SetCurrentState(game.TutorialState);
                }
                else
                {
                    game.SetCurrentState(game.PlayState);
                }
            }
        }

        public void UpdatePhysics(float delta)
        {
        }

        void BuildLetters()
        {
            game.myLetters = new GameObject[game.rounds];

            IQuestionPack _qp;
            ILivingLetterData _lldata;

            for (int i = 0; i < game.rounds; ++i)
            {
                game.myLetters[i] = GameObject.Instantiate(game.letterPrefab);
                game.myLetters[i].SetActive(true);

                _qp = ColorTickleConfiguration.Instance.Questions.GetNextQuestion();
                _lldata = _qp.GetCorrectAnswers().ToList()[0];

                game.myLetters[i].GetComponent<LivingLetterController>().Init(_lldata, _outline: true);
                game.myLetters[i].GetComponent<LivingLetterController>().LabelRender.color = Color.white;
                game.myLetters[i].GetComponent<ColorTickle_LLController>().movingToDestination = false;

            }
        }

        void BuildTutorialLetter()
        {
            var tutorialLetterId = LanguageSwitcher.I.GetLangConfig(LanguageUse.Learning).TutorialLetterId;
            var data = AppManager.I.DB.GetLetterDataById(tutorialLetterId);

            LL_LetterData LLdata = new LL_LetterData(data);
            game.tutorialLetter = GameObject.Instantiate(game.letterPrefab);
            game.tutorialLetter.SetActive(true);
            game.tutorialLetter.GetComponent<LivingLetterController>().Init(LLdata, _outline: true);
            game.tutorialLetter.GetComponent<LivingLetterController>().LabelRender.color = Color.white;
            game.tutorialLetter.GetComponent<ColorTickle_LLController>().movingToDestination = false;
        }

    }

}
