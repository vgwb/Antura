namespace Antura.Minigames
{
    /// <summary>
    /// The game state reached when the minigame ends.
    /// This state is present in all minigames and is always accessed last.
    /// </summary>
    public class OutcomeGameState : FSM.IState
    {
        private MiniGameController game;

        private bool outcomeStarted;
        private float timer;

        public OutcomeGameState(MiniGameController game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            timer = 2.0f;
            game.Context.GetAudioManager().PlayMusic(Music.Relax);
        }

        public void ExitState()
        {
            game.Context.GetStarsWidget().Hide();
        }

        void Complete()
        {
            if (outcomeStarted) {
                return;
            }

            outcomeStarted = true;

            int starsScore = game.StarsScore;
            if (starsScore > 3) {
                starsScore = 3;
            }

            game.Context.GetStarsWidget().Show(starsScore);

            Database.LocalizationDataId text;

            if (starsScore < 1) {
                text = Database.LocalizationDataId.Keeper_Bad_2;
            } else if (starsScore < 2) {
                text = Database.LocalizationDataId.Keeper_Good_5;
            } else if (starsScore < 3) {
                text = Database.LocalizationDataId.Keeper_Good_2;
            } else {
                text = Database.LocalizationDataId.Keeper_Good_1;
            }

            game.Context.GetAudioManager().PlayDialogue(text);

            game.Context.GetOverlayWidget().Initialize(false, false, false);
        }

        public void Update(float delta)
        {
            if (timer > 0) {
                timer -= delta;

                if (timer <= 0) {
                    Complete();
                }
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}