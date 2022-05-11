using Antura.LivingLetters;
using Antura.Tutorial;
using DG.DeExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Database;
using Antura.Keeper;
using UnityEngine;

namespace Antura.Minigames.ReadingGame
{
    public class ReadingGameAnswerState : FSM.IState
    {
        public bool TutorialMode = false;
        ReadingGameGame game;

        ILivingLetterData correctLLData;

        public float ReadTime;
        public float MaxTime;
        CircleButton correctButton;
        CircleButtonBox box;

        CountdownTimer gameTime = new CountdownTimer(90.0f);

        private bool showOldTimer => ReadingGameConfiguration.Instance.CurrentGameType == ReadingGameConfiguration.GameType.FollowReading;

        float rightButtonTimer = 0;

        public ReadingGameAnswerState(ReadingGameGame game)
        {
            this.game = game;

            gameTime.onTimesUp += OnTimesUp;
        }

        public void EnterState()
        {
            Finished = false;
            game.antura.AllowSitting = false;
            game.isTimesUp = false;
            gameTime.Reset(game.TimeToAnswer);
            if (!TutorialMode)
                countdownCo = game.StartCoroutine(CountdownSFXCO());

            game.circleBox.SetActive(true);
            box = game.circleBox.GetComponent<CircleButtonBox>();
            box.Clear();

            correctLLData = game.CurrentQuestion.GetCorrectAnswers().First();
            var wrongs = game.CurrentQuestion.GetWrongAnswers();

            var choices = new List<ILivingLetterData>();

            // Difficulty for game type ReadAndListen
            int maxWrongs = Mathf.RoundToInt(2 + 3 * game.Difficulty);
            choices.AddRange(wrongs);
            for (int i = maxWrongs, count = choices.Count; i < count; ++i)
            {
                choices.RemoveAt(choices.Count - 1);
            }

            choices.Add(correctLLData);
            choices.Shuffle();

            float delay = 0;
            foreach (var c in choices)
            {
                var imageData = new LL_ImageData(c.Id);
                var button = box.AddButton(imageData, OnAnswered, delay);
                delay += 0.2f;

                if (c == correctLLData)
                {
                    correctButton = button;
                }
            }

            box.Active = true;

            if (!TutorialMode)
            {
                if (showOldTimer)
                {
                    game.radialWidget.Show();
                    game.radialWidget.Reset(ReadTime / MaxTime);
                    game.radialWidget.inFront = true;
                    game.radialWidget.pulsing = true;
                }

                gameTime.Start();
            }

            if (ReadingGameConfiguration.Instance.CurrentGameType == ReadingGameConfiguration.GameType.ReadAndListen)
                game.EnableRepeatPromptButton();
        }

        private Coroutine countdownCo;
        private IEnumerator CountdownSFXCO()
        {
            while (true)
            {
                yield return new WaitForSeconds(game.CurrentSongBPM.periodRatio);
                game.Context.GetAudioManager().PlaySound(Sfx.WheelTick);
                if (Finished)
                    break;
            }
        }

        public void ExitState()
        {
            game.circleBox.GetComponent<CircleButtonBox>().Clear();
            game.radialWidget.inFront = false;

            gameTime.Stop();
            countdownCo = null;

            game.runLettersBox.RunAll();
        }

        public void Update(float delta)
        {
            rightButtonTimer -= delta;

            if (correctButton != null && TutorialMode && rightButtonTimer < 0 && box.IsReady() && !Finished)
            {
                rightButtonTimer = 3;
                var uicamera = game.uiCamera;
                TutorialUI.SetCamera(uicamera);
                TutorialUI.Click(correctButton.transform.position);
            }

            if (!TutorialMode && showOldTimer)
            {
                game.radialWidget.percentage = gameTime.Time / gameTime.Duration;
            }
            if (!TutorialMode && ReadingGameConfiguration.Instance.ShowTimer && !Finished)
                game.Context.GetOverlayWidget().SetClockTime(gameTime.Time);
            gameTime.Update(delta);
        }

        public void UpdatePhysics(float delta)
        {

        }

        void OnAnswered(CircleButton clickedButton)
        {
            if (Finished)
                return;
            Finished = true;
            game.DisableRepeatPromptButton();

            if (ReadingGameConfiguration.Instance.ShowTimer && !TutorialMode)
                UI.MinigamesUI.Timer.Pause();

            bool isCorrect = clickedButton != null && DataMatchingHelper.IsDataMatching(clickedButton.Answer, correctLLData, LetterEqualityStrictness.Letter);

            game.Context.GetAudioManager().PlaySound(isCorrect ? Sfx.OK : Sfx.KO);

            if (isCorrect)
            {
                clickedButton?.SetColor(Color.green);
            }
            else
            {
                if (clickedButton != null)
                    TutorialUI.MarkNo(clickedButton.transform.position);
            }

            if (ReadingGameConfiguration.Instance.CurrentGameType == ReadingGameConfiguration.GameType.ReadAndListen)
            {
                if (isCorrect)
                {
                    correctLLData = new LL_ImageData(correctLLData.Id);
                    var runLetter = game.runLettersBox.AddRunLetter(correctLLData, Vector3.one * 4);
                    runLetter.Stop();
                    runLetter.PlayAnimation(LLAnimationStates.LL_dancing);
                }
                else
                {
                    game.runLettersBox.AnimateAll(LLAnimationStates.LL_tickling);
                }

                // First read the answer you clicked
                if (clickedButton != null)
                {
                    game.Context.GetAudioManager().PlayVocabularyData(clickedButton.Answer, autoClose: false, callback: () =>
                     {
                         // Then read the one that is correct, if not already correct, and highlight it
                         if (!isCorrect)
                         {
                             if (TutorialMode)
                             {
                                 Finished = false;
                             }
                             else
                             {
                                 correctButton.SetColor(Color.green);
                                 game.Context.GetAudioManager().PlayVocabularyData(correctButton.Answer,
                                     callback: () =>
                                     {
                                         // Then translate the sentence
                                         game.Context.GetAudioManager().PlayVocabularyData(
                                              game.CurrentQuestion.GetQuestion(),
                                              autoClose: false,
                                              keeperMode: KeeperMode.NativeNoSubtitles,
                                              callback: () => { Next(isCorrect, clickedButton); });
                                     });
                             }
                         }
                         else
                         {
                             // Just translate the sentence
                             game.Context.GetAudioManager().PlayVocabularyData(game.CurrentQuestion.GetQuestion(),
                                  autoClose: false,
                                  keeperMode: KeeperMode.NativeNoSubtitles, callback: () =>
                                  {
                                      Next(isCorrect, clickedButton);
                                  });
                         }

                     });
                }
                else
                {
                    // Time out. Just as if you failed.
                    if (TutorialMode)
                    {
                        Finished = false;
                    }
                    else
                    {
                        correctButton.SetColor(Color.green);
                        game.Context.GetAudioManager().PlayVocabularyData(correctButton.Answer,
                            callback: () =>
                            {
                                // Then translate the sentence
                                game.Context.GetAudioManager().PlayVocabularyData(
                                    game.CurrentQuestion.GetQuestion(),
                                    autoClose: false,
                                    keeperMode: KeeperMode.NativeNoSubtitles,
                                    callback: () => { Next(isCorrect, clickedButton); });
                            });
                    }
                }
            }
            else
            {
                Next(isCorrect, clickedButton);
            }
        }

        void Next(bool correct, CircleButton clickedButton)
        {
            if (ReadingGameConfiguration.Instance.CurrentGameType != ReadingGameConfiguration.GameType.SimonSong)
                KeeperManager.I.CloseSubtitles();

            if (!TutorialMode)
            {
                game.Context.GetLogManager().OnAnswered(correctLLData, correct);
            }
            if (correct)
            {
                // Assign score
                if (!TutorialMode && showOldTimer)
                {
                    game.AddScore((int)(ReadTime) + 1);
                    game.radialWidget.percentage = 0;
                    game.radialWidget.pulsing = false;
                }

                game.StartCoroutine(DoEndAnimation(true, correctButton, clickedButton));

                game.antura.Mood = ReadingGameAntura.AnturaMood.HAPPY;
            }
            else
            {
                if (TutorialMode)
                {
                    Finished = false;
                }
                else
                {
                    if (!TutorialMode && showOldTimer)
                    {
                        game.radialWidget.PoofAndHide();
                    }
                    game.StartCoroutine(DoEndAnimation(false, correctButton, clickedButton));

                    if (game.antura.isActiveAndEnabled)
                        game.antura.animator.DoShout(() => { ReadingGameConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.DogBarking); });
                }
            }
        }

        private void OnTimesUp()
        {
            if (Finished)
                return;

            // Time's up!
            game.isTimesUp = true;
            if (ReadingGameConfiguration.Instance.ShowTimer)
                game.Context.GetOverlayWidget().SetClockTime(gameTime.Time);

            if (ReadingGameConfiguration.Instance.CurrentGameType == ReadingGameConfiguration.GameType.FollowReading)
            {
                // show time's up and back
                game.Context.GetPopupWidget().ShowTimeUp(() => { });
            }
            OnAnswered(null);
        }


        public bool Finished = false;
        IEnumerator DoEndAnimation(bool correct, CircleButton correctButton, CircleButton clickedButton = null)
        {
            Finished = true;
            box.Active = false;
            game.StartCoroutine(game.gameLettersHandler.CleanupCO(correct));
            if (countdownCo != null)
                game.StopCoroutine(countdownCo);

            if (ReadingGameConfiguration.Instance.CurrentGameType == ReadingGameConfiguration.GameType.ReadAndListen)
            {
                if (!TutorialMode)
                    if (correct)
                        game.AddScore(1);
            }

            if (ReadingGameConfiguration.Instance.CurrentGameType != ReadingGameConfiguration.GameType.SimonSong)
            {
                yield return new WaitForSeconds(1.0f);
            }

            if (ReadingGameConfiguration.Instance.CurrentGameType != ReadingGameConfiguration.GameType.ReadAndListen)
            {
                if (!correct && game.RemoveLife())
                {
                    yield break;
                }
            }

            if (ReadingGameConfiguration.Instance.CurrentGameType == ReadingGameConfiguration.GameType.SimonSong)
            {
                // Time up
                if (!correct && !clickedButton)
                {
                    game.circleBox.GetComponent<CircleButtonBox>().ClearButtonsApartFrom(correctButton);

                }
                else
                {
                    if (correct)
                    {
                        game.AddScore(1);
                    }
                    else
                    {
                        clickedButton?.SetColor(Color.red);
                        yield return new WaitForSeconds(0.5f);
                    }
                    correctButton.SetColor(Color.green);
                    game.circleBox.GetComponent<CircleButtonBox>().Clear(startDelay: 0.5f);
                }

                game.Context.GetAudioManager().PlayVocabularyData(correctButton.Answer);
                game.ChangeLoopingSong(game.CurrentSongBPM.intro);
                //Debug.Log("POST ANSWER INTRO LOOP");
                game.onSongLoop += () =>
                {
                    KeeperManager.I.CloseSubtitles();
                    //Debug.Log("END - POST ANSWER INTRO LOOP");
                    game.StopLoopingSong();
                    game.SetCurrentState(game.QuestionState);
                };
            }
            else if (ReadingGameConfiguration.Instance.CurrentGameType == ReadingGameConfiguration.GameType.SimonSong)
            {
                game.circleBox.GetComponent<CircleButtonBox>().ClearButtonsApartFrom(correctButton);
                game.circleBox.GetComponent<CircleButtonBox>().Clear(() =>
                {
                    game.SetCurrentState(game.QuestionState);
                }, 0.5f);
            }
            else
            {
                //if (!correct) clickedButton?.SetColor(Color.red);
                correctButton.SetColor(Color.green);
                game.circleBox.GetComponent<CircleButtonBox>().Clear(() =>
                {
                    game.SetCurrentState(game.QuestionState);
                }, 0.5f);
            }
        }

    }
}
