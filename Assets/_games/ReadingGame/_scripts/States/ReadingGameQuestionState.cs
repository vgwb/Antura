using System;
using System.Diagnostics;
using Antura.Tutorial;

namespace Antura.Minigames.ReadingGame
{
    public class ReadingGameQuestionState : FSM.IState
    {
        ReadingGameGame game;
        bool firstRun = true;
        bool completed = false;

        public ReadingGameQuestionState(ReadingGameGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.antura.AllowSitting = true;
            game.isTimesUp = false;

            int maxQuestions = ReadingGameGame.MAX_QUESTIONS;

            switch (ReadingGameConfiguration.Instance.CurrentGameType)
            {
                case ReadingGameConfiguration.GameType.SimonSong:
                    maxQuestions = ReadingGameGame.MAX_QUESTIONS_SIMON_SONG;
                    break;
            }

            if (game.CurrentQuestionNumber >= maxQuestions)
            {
                game.EndGame(game.CurrentStars, game.CurrentScore);
                return;
            }

            game.blurredText.SetActive(true);

            switch (ReadingGameConfiguration.Instance.CurrentGameType)
            {
                case ReadingGameConfiguration.GameType.FollowReading:
                {
                    // Pick a question and show it
                    var pack = ReadingGameConfiguration.Instance.Questions.GetNextQuestion();
                    game.CurrentQuestion = pack;

                    if (pack != null)
                    {
                        // Show the bar
                        game.barSet.SetData(pack.GetQuestion());
                    }
                    else
                        game.EndGame(game.CurrentStars, game.CurrentScore);

                    break;
                }

                case ReadingGameConfiguration.GameType.ReadAndListen:
                {
                    // Pick a question pack
                    var pack = ReadingGameConfiguration.Instance.Questions.GetNextQuestion();
                    game.CurrentQuestion = pack;
                    break;
                }

                case ReadingGameConfiguration.GameType.FollowSong:

                    // Just follow the bars
                    game.UpdateFollowDifficulty();
                    game.barSet.SetData(game.songToPlay);
                    break;

                case ReadingGameConfiguration.GameType.SimonSong:
                {
                    // Pick a question pack
                    var pack = ReadingGameConfiguration.Instance.Questions.GetNextQuestion();
                    game.CurrentQuestion = pack;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            game.barSet.active = false;

            if (firstRun)
            {
                switch (ReadingGameConfiguration.Instance.CurrentGameType)
                {
                    case ReadingGameConfiguration.GameType.FollowReading:
                    case ReadingGameConfiguration.GameType.ReadAndListen:
                        game.PlayIntro(() => { completed = true; });
                        game.ReadState.TutorialMode = game.TutorialEnabled;
                        break;

                    case ReadingGameConfiguration.GameType.SimonSong:
                        game.PlayIntro(() => { completed = true; });
                        game.ReadState.TutorialMode = game.TutorialEnabled;
                        break;

                    case ReadingGameConfiguration.GameType.FollowSong:

                        game.PlayIntro(() =>
                        {
                            var firstBar = game.barSet.GetNextBar();
                            var handleOffset = firstBar.glass.handleOffset.position - firstBar.glass.transform.position;

                            if (game.TutorialEnabled)
                            {
                                TutorialUI.DrawLine(firstBar.start.transform.position + handleOffset, firstBar.endCompleted.transform.position + handleOffset, TutorialUI.DrawLineMode.FingerAndArrow, false, true);
                            }
                            game.barSet.SwitchToNextBar();

                            if (game.TutorialEnabled)
                            {
                                //UnityEngine.Debug.LogError("TUTORIAL ON");
                                game.Context.GetAudioManager()
                                    .PlayDialogue(ReadingGameConfiguration.Instance.TutorialLocalizationId, () =>
                                    {
                                        completed = true;
                                    });
                            }
                            else
                            {
                                completed = true;
                            }
                        });
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
            else
            {
                ++game.CurrentQuestionNumber;
                completed = true;
                game.ReadState.TutorialMode = false;
            }

            if (ReadingGameConfiguration.Instance.ShowTimer && !game.ReadState.TutorialMode)
            {
                game.Context.GetOverlayWidget().SetClockDuration(game.TimeToAnswer);
                game.Context.GetOverlayWidget().SetClockTime(game.TimeToAnswer);
            }

            firstRun = false;
        }

        public void ExitState()
        {

        }

        public void Update(float delta)
        {
            if (completed)
                game.SetCurrentState(game.ReadState);
        }

        public void UpdatePhysics(float delta)
        {

        }
    }
}

