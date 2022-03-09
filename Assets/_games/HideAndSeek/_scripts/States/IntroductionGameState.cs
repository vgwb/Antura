namespace Antura.Minigames.HideAndSeek
{
    public class IntroductionGameState : FSM.IState
    {
        HideAndSeekGame game;

        float timer = 1.0f;
        float startIntroDelay = 0;
        bool dialogueEnded;

        public IntroductionGameState(HideAndSeekGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            OnTitleEnded();
        }

        void OnTitleEnded()
        {
            startIntroDelay = 0.25f;
        }

        public void ExitState() { }

        public void Update(float delta)
        {
            if (startIntroDelay > 0)
            {
                startIntroDelay -= delta;

                if (startIntroDelay <= 0)
                {
                    dialogueEnded = true;
                    // game.PlayIntro(() => dialogueEnded = true);
                }
            }

            timer -= delta;

            if (dialogueEnded && timer < 0)
            {
                if (game.TutorialEnabled)
                {
                    game.SetCurrentState(game.TutorialState);
                }
                else
                {
                    game.SetCurrentState(game.PlayState);
                }
            }
        }

        public void UpdatePhysics(float delta) { }
    }
}
