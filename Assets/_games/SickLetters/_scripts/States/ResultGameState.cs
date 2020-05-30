using Antura.Keeper;

namespace Antura.Minigames.SickLetters
{
    public class ResultGameState : FSM.IState
    {
        SickLettersGame game;

        float timer = 0;
        public ResultGameState(SickLettersGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.LLPrefab.jumpOut(0, true);

            if (game.scale.counter < game.targetScale) {
                game.manager.failure();
                timer = 6;
            }

            if (game.scale.counter >= game.targetScale) {
                timer = 4;
            }
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            timer -= delta;

            if (timer < 0) {
                game.EndGame(game.CurrentStars, game.maxReachedCounter);
                game.buttonRepeater.SetActive(false);

                if (game.CurrentStars == 0) {
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Reward_0Star);
                } else {
                    Database.LocalizationDataId data = randomRewardData();
                    KeeperManager.I.PlayDialogue(data);
                }
            }
        }

        public void UpdatePhysics(float delta)
        {
        }

        Database.LocalizationDataId randomRewardData()
        {
            if (game.CurrentStars == 1) {
                return (Database.LocalizationDataId)(UnityEngine.Random.Range(262, 265));
            } else if (game.CurrentStars == 2) {
                return (Database.LocalizationDataId)(UnityEngine.Random.Range(265, 268));
            } else {
                return (Database.LocalizationDataId)(UnityEngine.Random.Range(268, 271));
            }
        }
    }
}
