namespace Antura.Minigames.MakeFriends
{
    public class MakeFriendsIntroductionState : FSM.IState
    {
        MakeFriendsGame game;

        float timer = 1.5f;
        bool takenAction = false;

        public MakeFriendsIntroductionState(MakeFriendsGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
        }

        public void OnFinishedTutorial()
        {
            game.SetCurrentState(game.QuestionState);
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            if (takenAction)
            {
                return;
            }

            timer -= delta;

            if (timer < 0)
            {
                takenAction = true;

                if (game.TutorialEnabled)
                {
                    game.PlayTutorial();
                }
                else
                {
                    game.SetCurrentState(game.QuestionState);
                }
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
