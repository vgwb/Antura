using Antura.Tutorial;
using Antura.Minigames;

namespace Antura.Minigames.TakeMeHome
{
    public class TakeMeHomeResetState : FSM.IState
    {

        TakeMeHomeGame game;
        bool win;

        public TakeMeHomeResetState(TakeMeHomeGame game)
        {
            this.game = game;
            win = false;
        }

        public void EnterState()
        {
            if (game.currentLetter.collidedTubes.Count == 0)
            {
                win = false;
                game.currentLetter.respawn = true;
                return;
            }

            int tubeIndex = int.Parse(game.currentLetter.collidedTubes[game.currentLetter.collidedTubes.Count - 1].name.Substring(5));
            UnityEngine.Vector3 markPosition = game.currentLetter.collidedTubes[game.currentLetter.collidedTubes.Count - 1].cubeInfo.transform.position + new UnityEngine.Vector3(0, 0, -3);
            win = false;
            if (tubeIndex == game.currentTube)
            {
                TakeMeHomeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.StampOK);
                //AudioManager.I.PlaySound (Sfx.Win);
                win = true;
                game.IncrementScore();


                TutorialUI.MarkYes(markPosition, TutorialUI.MarkSize.Big);
            }
            else
            {
                TakeMeHomeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.KO);
                TutorialUI.MarkNo(markPosition, TutorialUI.MarkSize.Big);
                game.spawnTube.show(game.currentLetter.lastCollidedTube.color);
            }

            game.Context.GetLogManager().OnAnswered(game.currentLetter.letter.Data, win);
            game.currentLetter.followTube(win);
        }

        public void ExitState()
        {
            game.currentLetter.isDraggable = true;
        }

        public void Update(float delta)
        {
            if (!game.currentLetter.isMoving)
            {
                if (!win)
                    game.SetCurrentState(game.PlayState);
                else
                    game.SetCurrentState(game.IntroductionState);
            }
        }

        public void UpdatePhysics(float delta)
        {
        }


    }
}
