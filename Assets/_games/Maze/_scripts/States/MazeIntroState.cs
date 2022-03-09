namespace Antura.Minigames.Maze
{
    public class MazeIntroState : FSM.IState
    {
        MazeGame game;

        public MazeIntroState(MazeGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.startGame();
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            game.timer.Update();
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
