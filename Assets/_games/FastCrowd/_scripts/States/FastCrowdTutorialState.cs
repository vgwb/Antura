using Antura.Core;
using Antura.Keeper;
using Antura.LivingLetters;
using Antura.Tutorial;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.FastCrowd
{
    public class FastCrowdTutorialState : FSM.IState
    {
        FastCrowdGame game;

        float tutorialStartTimer;
        int answerCounter;

        bool tutorialStarted;

        bool MustTrunk
        {
            get
            {
                return FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Alphabet ||
                        FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Counting ||
                        FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Word ||
                        FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.LetterName
                        || FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Image
                        || FastCrowdConfiguration.Instance.IsOrderingVariation;
            }
        }

        public FastCrowdTutorialState(FastCrowdGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.QuestionManager.OnCompleted += OnQuestionCompleted;
            game.QuestionManager.OnDropped += OnAnswerDropped;

            if (game.CurrentChallenge != null)
            {
                if (MustTrunk)
                {
                    game.CurrentChallenge.RemoveRange(2, game.CurrentChallenge.Count - 2);
                }
                game.QuestionManager.StartQuestion(game.CurrentChallenge, game.NoiseData);
            }
            else
            {
                game.QuestionManager.Clean();
            }
            tutorialStarted = false;

            game.Context.GetAudioManager().PlayDialogue(FastCrowdConfiguration.Instance.TutorialLocalizationId, StartTutorial);

            game.QuestionManager.wordComposer.gameObject.SetActive(FastCrowdConfiguration.Instance.NeedsWordComposer);
        }

        public void ExitState()
        {
            game.QuestionManager.OnCompleted -= OnQuestionCompleted;
            game.QuestionManager.OnDropped -= OnAnswerDropped;
            game.QuestionManager.Clean();

            game.showTutorial = false;
            game.QuestionManager.wordComposer.gameObject.SetActive(false);
        }

        void StartTutorial()
        {
            DrawTutorial();

            tutorialStartTimer = 3f;

            tutorialStarted = true;
        }

        void OnQuestionCompleted()
        {
            game.SetCurrentState(game.ResultState);
        }

        void OnAnswerDropped(ILivingLetterData data, bool result)
        {
            tutorialStartTimer = 3f;
            game.Context.GetCheckmarkWidget().Show(result);
            game.Context.GetAudioManager().PlaySound(result ? Sfx.OK : Sfx.KO);

            if (result && (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Counting || FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Word
                                                                                                    || FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Image
                                                                                                    || FastCrowdConfiguration.Instance.IsOrderingVariation))
            {
                game.Context.GetAudioManager().PlayVocabularyData(data);
            }

        }

        public void Update(float delta)
        {
            if (tutorialStarted)
            {
                tutorialStartTimer += -delta;

                if (tutorialStartTimer <= 0f)
                {
                    tutorialStartTimer = 3f;

                    DrawTutorial();
                }
            }
        }

        void DrawTutorial()
        {
            if (game.QuestionManager.crowd.GetLetter(game.QuestionManager.dropContainer.GetActiveData()) == null)
            {
                return;
            }

            StrollingLivingLetter tutorialLetter = game.QuestionManager.crowd.GetLetter(game.QuestionManager.dropContainer.GetActiveData());

            Vector3 startLine = tutorialLetter.gameObject.GetComponent<LivingLetterController>().contentTransform.position;
            Vector3 endLine = game.QuestionManager.dropContainer.transform.position;

            List<StrollingLivingLetter> nearLetters = new List<StrollingLivingLetter>();

            game.QuestionManager.crowd.GetNearLetters(nearLetters, startLine, 10f);

            for (int i = 0; i < nearLetters.Count; i++)
            {
                if (nearLetters[i] != tutorialLetter)
                {
                    nearLetters[i].Scare(startLine, 3f);
                }
            }

            tutorialLetter.Tutorial();

            TutorialUI.DrawLine(startLine, endLine, TutorialUI.DrawLineMode.Finger);
        }

        public void UpdatePhysics(float delta) { }
    }
}
