using Antura.Core;
using Antura.Keeper;

namespace Antura.Minigames.FastCrowd
{
    public class FastCrowdIntroductionState : FSM.IState
    {
        FastCrowdGame game;

        bool nextState;
        bool playIntro;

        public FastCrowdIntroductionState(FastCrowdGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            nextState = false;
            playIntro = false;
            if (AppManager.I.AppSettings.EnglishSubtitles) {
                KeeperManager.I.PlayDialog(FastCrowdConfiguration.Instance.TitleLocalizationId, true, true, () => { playIntro = true; });
            } else {
                game.Context.GetAudioManager().PlayDialogue(FastCrowdConfiguration.Instance.TitleLocalizationId, () => { playIntro = true; });
            }

            game.Context.GetAudioManager().PlayMusic(Music.Theme10);
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            if (nextState) {
                nextState = false;
                game.SetCurrentState(game.QuestionState);
            }

            if (playIntro) {
                playIntro = false;

                if (AppManager.I.AppSettings.EnglishSubtitles) {
                    KeeperManager.I.PlayDialog(FastCrowdConfiguration.Instance.IntroLocalizationId, true, true, () => { nextState = true; });
                } else {
                    game.Context.GetAudioManager().PlayDialogue(FastCrowdConfiguration.Instance.IntroLocalizationId, () => { nextState = true; });
                }
            }
        }

        public void UpdatePhysics(float delta) { }

    }
}