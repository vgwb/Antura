using System.Collections;
using Antura.Database;
using Antura.LivingLetters;
using Antura.Tutorial;
using UnityEngine;

namespace Antura.Minigames.Egg
{
    public class EggPlayState : FSM.IState
    {
        private EggGame game;

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

        public EggPlayState(EggGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            letterOnSequence = 0;

            isSequence = game.CurrentQuestion.IsSequence();

            questionProgress = 0;

            if (isSequence)
            {
                correctAnswers = game.CurrentQuestion.Answers.Count;
            }
            else
            {
                correctAnswers = 3;
            }

            game.HintButton.gameObject.SetActive(true);
            game.eggController.onEggPressedCallback = OnHintPressed;
            UnityEngine.UI.Button.ButtonClickedEvent clickEvent = game.HintButton.onClick;

            if (clickEvent == null)
            {
                game.HintButton.onClick = clickEvent = new UnityEngine.UI.Button.ButtonClickedEvent();
            }
            clickEvent.AddListener(OnHintPressed);

            EnableAllGameplayInput();

            nextStateTimer = 0f;
            toNextState = false;

            inputButtonTimer = 0f;
            inputButtonCount = 0;
            repeatInputHasProgressed = false;
            enteredRepeatMode = false;

            game.eggButtonBox.SetOnPressedCallback(OnEggButtonPressed);

            showTutorial = game.ShowTutorial;
            game.EndTutorial();
            tutorialCorrectActive = false;
            tutorialDelayTimer = tutorialDelayTime;
            tutorialStop = false;

            if (showTutorial)
            {
                ShowTutorialPressCorrect();
            }

            if (!showTutorial)
            {
                game.InitializeOverlayWidget();
            }

            EnableAllGameplayInput();
        }

        public void ExitState()
        {
            game.HintButton.gameObject.SetActive(false);
            UnityEngine.UI.Button.ButtonClickedEvent clickEvent = game.HintButton.onClick;

            if (clickEvent != null)
            {
                clickEvent.RemoveListener(OnHintPressed);
            }
            if (showTutorial)
            {
                TutorialUI.Clear(true);
            }

            game.eggButtonBox.SetOnPressedCallback(null);
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

                            if (EggConfiguration.Instance.Variation == EggVariation.BuildWord)
                            {
                                runLetterData = new LL_ImageData(game.CurrentQuestion.Question.Id);
                            }
                            else if (EggConfiguration.Instance.Variation == EggVariation.Image)
                            {
                                runLetterData = new LL_ImageData(game.CurrentQuestion.Answers[0].Id);
                            }
                            else
                            {
                                runLetterData = game.CurrentQuestion.Answers[0];
                            }

                            game.runLettersBox.AddRunLetter(runLetterData);
                        }

                        game.currentStage++;
                        game.antura.NextStage();
                    }

                    game.SetCurrentState(game.ResultState);
                }
            }

            inputButtonTimer -= delta;

            if (repeatInputHasProgressed)
            {
                PlayPositiveAudioFeedback();

                game.eggController.EmoticonPositive();
                game.eggController.StartShake();
                game.eggController.ParticleCorrectEnabled();

                repeatInputHasProgressed = false;
                if (inputButtonTimer >= 0)
                {
                    inputButtonCount++;
                }
                else
                {
                    inputButtonCount = 0;
                }

                if (inputButtonCount >= inputButtonMax)
                {
                    inputButtonCount = 0;
                    PositiveFeedback();
                }

                tutorialDelayTimer = 0.5f;

                inputButtonTimer = inputButtonTime;
            }

            if (showTutorial && !tutorialStop)
            {
                if (tutorialCorrectActive)
                {
                    tutorialCorrectTimer -= delta;
                    if (tutorialCorrectTimer <= 0f)
                    {
                        if (isSequence)
                        {
                            tutorialSequenceIndex++;
                            if (tutorialSequenceIndex < correctAnswers)
                            {
                                tutorialCorrectTimer = 1f;

                                Vector3 clickPosition = game.eggButtonBox.GetButtons(false)[tutorialSequenceIndex].transform.position;
                                TutorialUI.Click(clickPosition);
                            }
                            else
                            {
                                tutorialCorrectActive = false;
                                tutorialDelayTimer = tutorialDelayTime;
                            }
                        }
                        else
                        {
                            tutorialCorrectActive = false;
                            tutorialDelayTimer = tutorialDelayTime;
                        }
                    }
                }
                else
                {
                    tutorialDelayTimer -= delta;

                    if (tutorialDelayTimer <= 0f)
                    {
                        ShowTutorialPressCorrect();
                    }
                }
            }
        }

        public void UpdatePhysics(float delta) { }

        public void OnEggButtonPressed(ILivingLetterData letterData)
        {
            if (isSequence && game.eggButtonBox.GetEggButton(letterData).IsPressed())
            {
                return;
            }

            if (!enteredRepeatMode)
            {
                game.Context.GetAudioManager().PlayVocabularyData(letterData, false, EggConfiguration.Instance.GetVocabularySoundType());
            }
            else
            {
                game.Context.GetAudioManager().PlaySound(Sfx.UIButtonClick);
            }

            if (showTutorial)
            {
                if (!enteredRepeatMode)
                {
                    TutorialUI.Clear(false);
                }
                tutorialDelayTimer = tutorialDelayTime;
                tutorialCorrectActive = false;
            }

            if (EggConfiguration.Instance.IsDataMatching(letterData, game.CurrentQuestion.Answers[letterOnSequence]))
            {
                if (isSequence)
                {
                    game.eggButtonBox.GetEggButton(letterData).SetPressed();
                    PositiveFeedback();
                    game.Context.GetLogManager().OnAnswered(letterData, true);
                }
                else
                {
                    repeatInputHasProgressed = true;

                    if (!enteredRepeatMode)
                    {
                        EggConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.OK);
                        enteredRepeatMode = true;
                        game.HintButton.gameObject.SetActive(false);
                        Vector3 clickPosition = game.eggButtonBox.GetButtons(false)[0].transform.position;
                        TutorialUI.ClickRepeat(clickPosition, 4);
                        game.eggButtonBox.RemoveButtons((a) => { return a != letterData; });

                        if (!showTutorial)
                        {
                            game.Context.GetLogManager().OnAnswered(letterData, true);
                        }
                    }
                }
            }
            else
            {
                game.eggButtonBox.SetButtonsOnStandardColor(game.eggButtonBox.GetEggButton(letterData));

                if (showTutorial)
                {
                    ShowTutorialPressedWrong(letterData);
                }
                else
                {
                    NegativeFeedback();
                    game.Context.GetLogManager().OnAnswered(letterData, false);
                }
            }
        }

        void OnHintPressed()
        {
            if (!isPlayingQuestion)
            {
                game.StartCoroutine(OnEggPressed());
            }
        }

        IEnumerator OnEggPressed()
        {
            DisableAllGameplayInput();

            game.eggController.EmoticonInterrogative();
            isPlayingQuestion = true;
            if (isSequence)
            {
                yield return game.eggButtonBox.PlayButtonsAudio(game.CurrentQuestion.Question, null, true, false, 0f,
                    () =>
                    {
                        isPlayingQuestion = false;
                        game.eggController.EmoticonClose();
                        EnableAllGameplayInput();
                        ResetSequenceProgress();
                        game.eggButtonBox.SetButtonsOnStandardColor();
                    }
                );
            }
            else
            {
                game.eggController.PlayAudioQuestion(delegate ()
                { isPlayingQuestion = false; game.eggController.EmoticonClose(); EnableAllGameplayInput(); });
            }
        }

        void ResetSequenceProgress()
        {
            letterOnSequence = 0;
            questionProgress = 0;
        }

        void PositiveFeedback()
        {
            if (isSequence)
            {
                letterOnSequence++;
            }

            questionProgress++;

            float crackingProgress = (float)questionProgress / (float)correctAnswers;

            game.eggController.Cracking(crackingProgress);

            game.eggController.ParticleCorrectEnabled();

            if (crackingProgress == 1f)
            {
                game.HintButton.gameObject.SetActive(false);
                game.Context.GetAudioManager().PlaySound(Sfx.EggBreak);
                game.eggController.EmoticonHappy();
                game.eggController.ParticleWinEnabled();
                DisableAllGameplayInput();
                tutorialStop = true;
                TutorialUI.Clear(false);

                game.StartCoroutine(OnEggCrackComplete());
            }
            else
            {
                PlayPositiveAudioFeedback();
                game.eggController.EmoticonPositive();
            }
        }

        void NegativeFeedback()
        {
            DisableAllGameplayInput();

            bool goAntura = false;

            if (!game.eggController.isNextToExit)
            {
                if (game.antura.IsAnturaIn())
                {
                    goAntura = true;
                }

                game.Context.GetAudioManager().PlaySound(Sfx.ScaleUp);
                tutorialStop = true;
            }
            else
            {
                game.Context.GetAudioManager().PlaySound(Sfx.ScaleDown);
                OnEggExitComplete();
            }

            game.eggController.EmoticonNegative();
            EggConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.KO);

            letterOnSequence = 0;
            questionProgress = 0;
            game.eggController.ResetCrack();

            if (goAntura)
            {
                AnturaEnter();
                game.eggController.MoveNext(1f, null);
            }
            else
            {
                game.eggController.MoveNext(1f, EnableAllGameplayInput);
            }
        }

        void AnturaExit()
        {
            game.antura.Exit(EnableAllGameplayInput);
        }

        void AnturaEnter()
        {
            game.antura.Enter(AnturaButtonsOut);
        }

        void AnturaButtonsOut()
        {
            game.eggButtonBox.AnturaButtonOut(0.5f, 1f, AnturaSetOnSpitPostion);
        }

        void AnturaButtonsIn()
        {
            game.eggButtonBox.AnturaButtonIn(0.5f, 0.5f, 0.05f, 0.25f, game.antura.DoSpit, AnturaExit);
        }

        void AnturaSetOnSpitPostion()
        {
            game.antura.SetOnSpitPosition(AnturaButtonsIn);
        }

        void OnEggExitComplete()
        {
            tutorialStop = true;
            DisableAllGameplayInput();
            game.stagePositiveResult = false;
            toNextState = true;
            //game.eggButtonBox.SetButtonsOnStandardColor();
            game.eggButtonBox.DisableButtonsInput();
        }

        IEnumerator OnEggCrackComplete()
        {
            tutorialStop = true;
            DisableAllGameplayInput();
            game.stagePositiveResult = true;

            if (isSequence)
            {
                yield return game.eggButtonBox.PlayButtonsAudio(null, game.CurrentQuestion.Question, true, false, 0.5f, OnLightUpButtonsComplete, () => { game.eggButtonBox.SetButtonsOnStandardColor(null, false); }, playButtonsNames:false);
            }
            else
            {
                yield return game.eggButtonBox.GetButtons(false)[0].PlayButtonAudio(true, 0.5f, OnLightUpButtonsComplete, () => { game.eggButtonBox.SetButtonsOnStandardColor(null, false); });
            }
        }

        void OnLightUpButtonsComplete()
        {
            if (isSequence)
            {
                game.eggButtonBox.SetButtonsOnPressedColor();
                game.eggButtonBox.HideButtons();
            }
            else
            {
                //game.eggButtonBox.GetEggButton(game.CurrentQuestion.Letters[0]).SetPressed();
            }

            game.eggController.ParticleWinDisabled();
            toNextState = true;
        }

        void EnableAllGameplayInput()
        {
            game.eggButtonBox.EnableButtonsInput();
            game.eggController.EnableInput();
        }

        void DisableAllGameplayInput()
        {
            game.eggButtonBox.DisableButtonsInput();
            game.eggController.DisableInput();
        }

        void PlayPositiveAudioFeedback()
        {
            if (positiveAudioSource != null && positiveAudioSource.IsPlaying)
            {
                return;
            }

            positiveAudioSource = game.Context.GetAudioManager().PlaySound(Sfx.EggMove);
        }

        void ShowTutorialPressCorrect()
        {
            tutorialCorrectActive = true;
            tutorialSequenceIndex = letterOnSequence;

            if (isSequence)
            {
                tutorialCorrectTimer = 1f;

                Vector3 clickPosition = game.eggButtonBox.GetButtons(false)[tutorialSequenceIndex].transform.position;
                TutorialUI.Click(clickPosition);
            }
            else
            {
                tutorialCorrectTimer = 2f;

                Vector3 clickPosition = game.eggButtonBox.GetButtons(false)[tutorialSequenceIndex].transform.position;
                TutorialUI.ClickRepeat(clickPosition, tutorialCorrectTimer);
            }
        }

        void ShowTutorialPressedWrong(ILivingLetterData letterData)
        {
            letterOnSequence = 0;
            questionProgress = 0;

            Vector3 markPosition = game.eggButtonBox.GetEggButton(letterData).transform.position;

            TutorialUI.MarkNo(markPosition, TutorialUI.MarkSize.Normal);
        }
    }
}
