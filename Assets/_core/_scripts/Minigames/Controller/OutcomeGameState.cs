using Antura.Database;
using Antura.Helpers;

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
            if (outcomeStarted)
            {
                return;
            }

            outcomeStarted = true;

            int starsScore = game.StarsScore;
            if (starsScore > 3)
            {
                starsScore = 3;
            }

            game.Context.GetStarsWidget().Show(starsScore);

            LocalizationDataId locID = LocalizationDataId.Keeper_Result_0_Bones;
            switch (starsScore)
            {
                case 0:
                    locID = LocalizationDataId.Keeper_Result_0_Bones;
                    break;
                case 1:
                    locID = new[]
                    {
                        LocalizationDataId.Keeper_Result_1_Bones_1,
                        LocalizationDataId.Keeper_Result_1_Bones_2,
                        LocalizationDataId.Keeper_Result_1_Bones_3
                    }.GetRandom();
                    break;
                case 2:
                    locID = new[]
                    {
                        LocalizationDataId.Keeper_Result_2_Bones_1,
                        LocalizationDataId.Keeper_Result_2_Bones_2,
                        LocalizationDataId.Keeper_Result_2_Bones_3
                    }.GetRandom();
                    break;
                case 3:
                    locID = new[]
                    {
                        LocalizationDataId.Keeper_Result_3_Bones_1,
                        LocalizationDataId.Keeper_Result_3_Bones_2,
                        LocalizationDataId.Keeper_Result_3_Bones_3
                    }.GetRandom();
                    break;
            }

            game.Context.GetAudioManager().PlayDialogue(locID);

            game.Context.GetOverlayWidget().Initialize(false, false, false);
        }

        public void Update(float delta)
        {
            if (timer > 0)
            {
                timer -= delta;

                if (timer <= 0)
                {
                    Complete();
                }
            }
        }

        public void UpdatePhysics(float delta)
        {
        }
    }
}
