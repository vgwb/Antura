using System;
using System.Collections;
using Antura.Database;
using Antura.Keeper;
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

            switch (ReadingGameConfiguration.Instance.CurrentGameType)
            {
                case ReadingGameConfiguration.GameType.FollowReading:

                    if (!TutorialMode)
                    {
                        gameTime.Start();
                    }
                    else
                    {
                        game.PlayTutorial();
                        drawTutorialLineTimer = 0;
                    }

                    break;

                case ReadingGameConfiguration.GameType.ReadAndListen:
                    break;

                case ReadingGameConfiguration.GameType.FollowSong:
                    break;
                case ReadingGameConfiguration.GameType.SimonSong:
                    game.PlayTutorialConditional(TutorialMode, StartSimonQuestionLoop);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var inputManager = game.Context.GetInputManager();

            inputManager.onPointerDown += OnPointerDown;
            inputManager.onPointerUp += OnPointerUp;

            game.blurredText.SetActive(true);

            switch (ReadingGameConfiguration.Instance.CurrentGameType)
            {
                case ReadingGameConfiguration.GameType.FollowReading:

                    break;
                case ReadingGameConfiguration.GameType.ReadAndListen:

                    // Read it
                    // Play the question word and go to the answer
                    game.PlayTutorialConditional(TutorialMode,
                        () =>
                            game.Context.GetAudioManager().PlayVocabularyData(game.CurrentQuestion.GetQuestion(),
                        keeperMode: KeeperMode.LearningAndSubtitles, autoClose: false, callback:
                        () =>
                        {
                            // Setup timer for the new state
                            game.AnswerState.ReadTime = gameTime.Time;
                            game.AnswerState.MaxTime = gameTime.Duration;
                            game.AnswerState.TutorialMode = TutorialMode;
                            game.SetCurrentState(game.AnswerState);

                        })
                        );

                    break;

                case ReadingGameConfiguration.GameType.FollowSong:
                    AudioClip songAudio = null;
                    switch (ReadingGameConfiguration.Instance.Variation)
                    {
                        case ReadingGameVariation.SongAlphabet:
                            songAudio = game.alphabetSongAudio;
                            break;
                        case ReadingGameVariation.SongDiacritics:
                            songAudio = game.diacriticSongAudio;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    game.barSet.PlaySong(game.Context.GetAudioManager().PlaySound(songAudio), OnSongEnded);
                    break;

                case ReadingGameConfiguration.GameType.SimonSong:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            completedDragging = false;

            if (firstRealRun)
            {
                bool showClock = ReadingGameConfiguration.Instance.ShowTimer;
                bool showLives = ReadingGameConfiguration.Instance.CurrentGameType == ReadingGameConfiguration.GameType.FollowReading;

                if (!TutorialMode)
                {
                    // Configure overlay
                    var overlay = game.Context.GetOverlayWidget();
                    overlay.Initialize(true, showClock, showLives);
                    overlay.SetMaxLives(game.Lives);
                    overlay.SetLives(game.Lives);
                    overlay.SetClockDuration(gameTime.Duration);
                    overlay.SetClockTime(gameTime.Time);
                    overlay.SetStarsThresholds(game.GetStarsThreshold(1), game.GetStarsThreshold(2), game.GetStarsThreshold(3));
                    firstRealRun = false;
                }
            }

            switch (ReadingGameConfiguration.Instance.CurrentGameType)
            {
                case ReadingGameConfiguration.GameType.FollowReading:
                    game.barSet.SwitchToNextBar();
                    game.barSet.active = true;
                    break;
                case ReadingGameConfiguration.GameType.FollowSong:
                    game.barSet.active = true;
                    break;
                case ReadingGameConfiguration.GameType.ReadAndListen:
                case ReadingGameConfiguration.GameType.SimonSong:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region Simon Song

        private void StartSimonQuestionLoop()
        {
            Debug.Log("StartSimonQuestionLoop");
            var songBPM = game.CurrentSongBPM;

            // Intro loop
            if (!game.IsLoopingSong) // first intro loop
            {
                SimonSongStartQuestion(songBPM);
            }
            else
            {
                game.onSongLoop += () =>
                {
                    Debug.Log("END INTRO LOOP");
                    SimonSongStartQuestion(songBPM);
                };
            }
        }

        private void SimonSongStartQuestion(ReadingGameGame.SimonSongBPM songBPM)
        {
            game.Context.GetAudioManager().PlayDialogue(LocalizationDataId.Song_Word_Question, keeperMode: KeeperMode.SubtitlesOnly, isKeeper: false);

            game.ChangeLoopingSong(songBPM.songFirstHalf, songBPM.voicePartLearning);
            game.StartCoroutine(ShowAnimationLetters(songBPM));
            game.StartCoroutine(SimonSongShowButtons(songBPM));

            // Question song
            game.onSongLoop += () => {

                // Second part
                game.ChangeLoopingSong(songBPM.songSecondHalf, songBPM.voicePartNative);
                game.onSongLoop = () =>
                {
                    game.StopLoopingSong();
                };
            };
        }

        #region Animation Letters

        private IEnumerator ShowAnimationLetters(ReadingGameGame.SimonSongBPM songBpm)
        {
            yield return game.StartCoroutine(game.gameLettersHandler.AnimateLettersCO(songBpm.periodRatio, TutorialMode));
        }

        #endregion

        private IEnumerator SimonSongShowButtons(ReadingGameGame.SimonSongBPM songBpm)
        {
            yield return game.WaitForPauseCO(songBpm.questionTime);

            // Vocabulary data
            game.Context.GetAudioManager()
                .PlayVocabularyData(game.CurrentQuestion.GetQuestion(), keeperMode: KeeperMode.LearningAndSubtitles,
                    autoClose: false);

            // Setup timer for the answer state
            game.AnswerState.ReadTime = gameTime.Time;
            game.AnswerState.MaxTime = gameTime.Duration;
            game.AnswerState.TutorialMode = TutorialMode;
            game.SetCurrentState(game.AnswerState);
        }

        #endregion

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
            if (!TutorialMode)
            {
                game.Context.GetOverlayWidget().SetClockTime(gameTime.Time);
            }
            else if (dragging == null)
            {
                drawTutorialLineTimer -= delta;

                if (drawTutorialLineTimer < 0)
                {
                    var activeBar = game.barSet.GetActiveBar();
                    if (activeBar != null)
                    {
                        drawTutorialLineTimer = 5;

                        var handleOffset = activeBar.glass.handleOffset.position - activeBar.glass.transform.position;
                        TutorialUI.DrawLine(activeBar.start.transform.position + handleOffset, activeBar.endCompleted.transform.position + handleOffset, TutorialUI.DrawLineMode.FingerAndArrow, false, true);
                    }
                }
            }

            gameTime.Update(delta);

            // Drag & Read
            if (dragging != null)
            {
                var inputManager = game.Context.GetInputManager();
                bool applyMagnet = ReadingGameConfiguration.Instance.CurrentGameType == ReadingGameConfiguration.GameType.FollowReading;
                completedDragging = dragging.SetGlassScreenPosition(inputManager.LastPointerPosition + draggingOffset, applyMagnet);
            }
            else
            {
                if (ReadingGameConfiguration.Instance.CurrentGameType == ReadingGameConfiguration.GameType.FollowReading)
                {
                    if (completedDragging)
                    {
                        var completedAllBars = game.barSet.SwitchToNextBar();

                        if (completedAllBars)
                        {
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

            // Antura reactions
            switch (ReadingGameConfiguration.Instance.CurrentGameType)
            {
                case ReadingGameConfiguration.GameType.FollowReading:

                    float perc = gameTime.Time / gameTime.Duration;

                    if (perc < 0.05f)
                    {
                        game.antura.Mood = ReadingGameAntura.AnturaMood.SAD;
                    }
                    else if (perc < 0.5f)
                    {
                        game.antura.Mood = ReadingGameAntura.AnturaMood.ANGRY;
                    }
                    else
                    {
                        game.antura.Mood = ReadingGameAntura.AnturaMood.HAPPY;
                    }
                    break;

                case ReadingGameConfiguration.GameType.FollowSong:

                    float distance;
                    if (game.barSet.GetFollowingDistance(out distance))
                    {
                        distance = Math.Abs(distance);

                        if (distance > 100)
                        {
                            timeFarFromTarget += delta;
                        }
                        else
                        {
                            timeFarFromTarget = 0;
                            //if (distance < 50) {
                            scoreAccumulator += 1.2f * delta;
                            //} else {
                            //    scoreAccumulator += 1 * delta;
                            // }
                            if (scoreAccumulator >= 1)
                            {
                                game.AddScore((int)scoreAccumulator);
                                scoreAccumulator = scoreAccumulator - (int)scoreAccumulator;
                                game.UpdateFollowDifficulty();
                            }
                        }

                        if (timeFarFromTarget > 1.0f)
                        {
                            game.antura.Mood = ReadingGameAntura.AnturaMood.ANGRY;
                        }
                        else
                        {
                            game.antura.Mood = ReadingGameAntura.AnturaMood.HAPPY;
                        }
                    }

                    break;
                case ReadingGameConfiguration.GameType.ReadAndListen:
                    break;
                case ReadingGameConfiguration.GameType.SimonSong:
                    break;
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

            if (game.RemoveLife())
            {
                return;
            }

            // show time's up and back
            game.Context.GetPopupWidget().ShowTimeUp(
                () =>
                {
                    game.SetCurrentState(game.QuestionState);
                    game.Context.GetPopupWidget().Hide();
                });
        }

        void OnPointerDown()
        {
            if (dragging)
            {
                return;
            }

            var inputManager = game.Context.GetInputManager();
            dragging = game.barSet.PickGlass(Camera.main, inputManager.LastPointerPosition);

            if (dragging != null)
            {
                draggingOffset = dragging.GetGlassScreenPosition() - inputManager.LastPointerPosition;
            }
        }

        void OnPointerUp()
        {
            dragging = null;
        }

        void OnSongEnded()
        {
            switch (ReadingGameConfiguration.Instance.CurrentGameType)
            {
                case ReadingGameConfiguration.GameType.FollowReading:
                case ReadingGameConfiguration.GameType.ReadAndListen:
                case ReadingGameConfiguration.GameType.SimonSong:
                    // Never here
                    break;
                case ReadingGameConfiguration.GameType.FollowSong:
                    game.EndGame(game.CurrentStars, game.CurrentScore);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
