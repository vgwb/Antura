using System;

namespace Antura.Minigames.ReadingGame
{
    public class ReadingGameInitialState : FSM.IState
    {
        ReadingGameGame game;

        float timer = 2;

        public ReadingGameInitialState(ReadingGameGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            timer = 2;

            switch (ReadingGameConfiguration.Instance.CurrentGameType)
            {
                case ReadingGameConfiguration.GameType.FollowReading:
                case ReadingGameConfiguration.GameType.ReadAndListen:
                    game.Context.GetAudioManager().PlayMusic(Music.Theme8);
                    break;
                case ReadingGameConfiguration.GameType.FollowSong:
                    game.Context.GetAudioManager().StopMusic();
                    break;
                case ReadingGameConfiguration.GameType.SimonSong:
                    game.Context.GetAudioManager().StopMusic();
                    // We'll loop the intro instead!
                    var song = game.CurrentSongBPM.intro;
                    game.StartLoopingSong(song);
                    timer = song.length;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public void ExitState()
        {

        }

        public void Update(float delta)
        {
            timer -= delta;

            if (timer < 0)
                game.SetCurrentState(game.QuestionState);
        }

        public void UpdatePhysics(float delta)
        {

        }
    }
}