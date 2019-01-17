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

            if (game.CurrentQuestionNumber >= ReadingGameGame.MAX_QUESTIONS) {
                game.EndGame(game.CurrentStars, game.CurrentScore);
                return;
            }

            if (ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.ReadAndAnswer) {
                game.Context.GetOverlayWidget().SetClockDuration(game.TimeToAnswer);
                game.Context.GetOverlayWidget().SetClockTime(game.TimeToAnswer);
            }

            game.blurredText.SetActive(true);
            //game.circleBox.SetActive(false);

            if (ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.ReadAndAnswer) {
                // Pick a question
                var pack = ReadingGameConfiguration.Instance.Questions.GetNextQuestion();
                game.CurrentQuestion = pack;
                if (pack != null)
                    game.barSet.SetData(pack.GetQuestion());
                else
                    game.EndGame(game.CurrentStars, game.CurrentScore);
            } else {
                game.barSet.SetShowTargets(ReadingGameConfiguration.Instance.Difficulty < 0.5f);
                game.barSet.SetShowArrows(ReadingGameConfiguration.Instance.Difficulty < 0.8f);

                game.barSet.SetData(game.songToPlay);
            }

            game.barSet.active = false;

            if (firstRun) {
                if (ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.ReadAndAnswer) {
                    game.Context.GetAudioManager().PlayDialogue(Database.LocalizationDataId.ReadingGame_Intro, () => { completed = true; });
                    game.ReadState.TutorialMode = true;
                } else {
                    var introDialogue = ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.Alphabet ?
                        Database.LocalizationDataId.Song_alphabet_Intro : Database.LocalizationDataId.AlphabetSong_letters_Intro;

                    game.Context.GetAudioManager().PlayDialogue(introDialogue, () => {
                        var firstBar = game.barSet.GetNextBar();
                        var handleOffset = firstBar.glass.handleOffset.position - firstBar.glass.transform.position;

                        if (game.TutorialEnabled) {
                            TutorialUI.DrawLine(firstBar.start.transform.position + handleOffset, firstBar.endCompleted.transform.position + handleOffset, TutorialUI.DrawLineMode.FingerAndArrow, false, true);
                        }
                        game.barSet.SwitchToNextBar();

                        if (game.TutorialEnabled) {
                            game.Context.GetAudioManager()
                                .PlayDialogue(Database.LocalizationDataId.Song_alphabet_Tuto, () => {
                                    completed = true;
                                });
                        } else {
                            completed = true;
                        }
                    });
                }
            } else {
                ++game.CurrentQuestionNumber;
                completed = true;
                game.ReadState.TutorialMode = false;
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

