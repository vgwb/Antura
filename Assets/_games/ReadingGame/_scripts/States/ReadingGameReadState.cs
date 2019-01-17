using System;
using Antura.Tutorial;
using UnityEngine;

namespace Antura.Minigames.ReadingGame
{
    public class ReadingGameReadState : FSM.IState
    {
        public bool TutorialMode = false;

        CountdownTimer gameTime = new CountdownTimer(90.0f);
        ReadingGameGame game;

        bool completedDragging = false;
        ReadingBar dragging;
        Vector2 draggingOffset;

        // song related
        float timeFarFromTarget = 0;
        float scoreAccumulator = 0;

        // tutorial
        float drawTutorialLineTimer = 0;
        bool firstRealRun = true;

        public ReadingGameReadState(ReadingGameGame game)
        {
            this.game = game;

            gameTime.onTimesUp += OnTimesUp;
        }

        public void EnterState()
        {
            game.antura.AllowSitting = true;
            game.isTimesUp = false;

            // Reset game timer
            gameTime.Reset(game.TimeToAnswer);

            if (ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.ReadAndAnswer) {
                if (!TutorialMode) {
                    gameTime.Start();
                } else {
                    game.Context.GetAudioManager().PlayDialogue(Database.LocalizationDataId.ReadingGame_Tuto);
                    drawTutorialLineTimer = 0;
                }
            }

            var inputManager = game.Context.GetInputManager();

            inputManager.onPointerDown += OnPointerDown;
            inputManager.onPointerUp += OnPointerUp;

            game.blurredText.SetActive(true);
            //game.circleBox.SetActive(false);

            if (ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.Alphabet) {
                game.barSet.PlaySong(game.Context.GetAudioManager().PlaySound(game.alphabetSongAudio), OnSongEnded);
            } else if (ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.DiacriticSong) {
                game.barSet.PlaySong(game.Context.GetAudioManager().PlaySound(game.diacriticSongAudio), OnSongEnded);
            }

            completedDragging = false;

            if (firstRealRun) {
                bool isSong = (ReadingGameConfiguration.Instance.Variation != ReadingGameVariation.ReadAndAnswer);

                if (!TutorialMode) {
                    // Configure overlay
                    var overlay = game.Context.GetOverlayWidget();
                    overlay.Initialize(true, !isSong, !isSong);
                    overlay.SetMaxLives(game.Lives);
                    overlay.SetLives(game.Lives);
                    overlay.SetClockDuration(gameTime.Duration);
                    overlay.SetClockTime(gameTime.Time);
                    overlay.SetStarsThresholds(game.GetStarsThreshold(1), game.GetStarsThreshold(2), game.GetStarsThreshold(3));

                    firstRealRun = false;
                }
            }

            if (ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.ReadAndAnswer) {
                game.barSet.SwitchToNextBar();
            }

            game.barSet.active = true;
        }


        public void ExitState()
        {
            TutorialMode = false;

            var inputManager = game.Context.GetInputManager();

            inputManager.onPointerDown -= OnPointerDown;
            inputManager.onPointerUp -= OnPointerUp;

            gameTime.Stop();

            game.barSet.active = false;
            game.barSet.Clear();
            game.blurredText.SetActive(false);
            game.hiddenText.Clear();
        }

        public void Update(float delta)
        {
            if (!TutorialMode) {
                game.Context.GetOverlayWidget().SetClockTime(gameTime.Time);
            } else if (dragging == null) {
                drawTutorialLineTimer -= delta;

                if (drawTutorialLineTimer < 0) {
                    var activeBar = game.barSet.GetActiveBar();

                    if (activeBar != null) {
                        drawTutorialLineTimer = 5;

                        var handleOffset = activeBar.glass.handleOffset.position - activeBar.glass.transform.position;
                        TutorialUI.DrawLine(activeBar.start.transform.position + handleOffset, activeBar.endCompleted.transform.position + handleOffset, TutorialUI.DrawLineMode.FingerAndArrow, false, true);
                    }
                }
            }

            gameTime.Update(delta);

            if (dragging != null) {
                var inputManager = game.Context.GetInputManager();
                completedDragging = dragging.SetGlassScreenPosition(inputManager.LastPointerPosition + draggingOffset,
                    ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.ReadAndAnswer);
            } else {
                if (ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.ReadAndAnswer) {

                    if (completedDragging) {
                        var completedAllBars = game.barSet.SwitchToNextBar();

                        if (completedAllBars) {
                            // go to Buttons State
                            game.AnswerState.ReadTime = gameTime.Time;
                            game.AnswerState.MaxTime = gameTime.Duration;
                            game.AnswerState.TutorialMode = TutorialMode;
                            game.SetCurrentState(game.AnswerState);
                            return;
                        }
                    }

                    completedDragging = false;
                }
            }


            if (ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.ReadAndAnswer) {
                float perc = gameTime.Time / gameTime.Duration;

                if (perc < 0.05f) {
                    game.antura.Mood = ReadingGameAntura.AnturaMood.SAD;
                } else if (perc < 0.5f) {
                    game.antura.Mood = ReadingGameAntura.AnturaMood.ANGRY;
                } else {
                    game.antura.Mood = ReadingGameAntura.AnturaMood.HAPPY;
                }
            } else // Alphabet Song
              {
                float distance;
                if (game.barSet.GetFollowingDistance(out distance)) {
                    distance = Math.Abs(distance);

                    if (distance > 100) {
                        timeFarFromTarget += delta;
                    } else {
                        timeFarFromTarget = 0;
                        //if (distance < 50) {
                            scoreAccumulator += 1.2f * delta;
                        //} else {
                        //    scoreAccumulator += 1 * delta;
                        // }
                        if (scoreAccumulator >= 1) {
                            game.AddScore((int)scoreAccumulator);
                            scoreAccumulator = scoreAccumulator - (int)scoreAccumulator;
                        }
                    }

                    if (timeFarFromTarget > 1.0f) {
                        game.antura.Mood = ReadingGameAntura.AnturaMood.ANGRY;
                    } else {
                        game.antura.Mood = ReadingGameAntura.AnturaMood.HAPPY;
                    }
                }
            }
        }

        public void UpdatePhysics(float delta)
        {

        }

        void OnTimesUp()
        {
            // Time's up!
            game.barSet.active = false;
            game.isTimesUp = true;
            game.Context.GetOverlayWidget().OnClockCompleted();

            if (game.RemoveLife()) {
                return;
            }

            // show time's up and back
            game.Context.GetPopupWidget().ShowTimeUp(
                () => {
                    game.SetCurrentState(game.QuestionState);
                    game.Context.GetPopupWidget().Hide();
                });
        }

        void OnPointerDown()
        {
            if (dragging) {
                return;
            }

            var inputManager = game.Context.GetInputManager();
            dragging = game.barSet.PickGlass(Camera.main, inputManager.LastPointerPosition);

            if (dragging != null) {
                draggingOffset = dragging.GetGlassScreenPosition() - inputManager.LastPointerPosition;
            }
        }

        void OnPointerUp()
        {
            dragging = null;
        }

        void OnSongEnded()
        {
            game.EndGame(game.CurrentStars, game.CurrentScore);
        }
    }
}