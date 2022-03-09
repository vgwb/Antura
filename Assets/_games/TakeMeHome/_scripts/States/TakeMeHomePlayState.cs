using Antura.Minigames;
using Antura.UI;

namespace Antura.Minigames.TakeMeHome
{
    public class TakeMeHomePlayState : FSM.IState
    {

        TakeMeHomeGame game;

        public CountdownTimer anturaTimer;
        public TakeMeHomePlayState(TakeMeHomeGame game)
        {
            this.game = game;

        }

        public void EnterState()
        {
            /*game.gameTime.Start();

			game.timerText.gameObject.SetActive(true);
			game.roundText.gameObject.SetActive(true);*/

            MinigamesUI.Timer.Play();


            anturaTimer = new CountdownTimer(10);
            anturaTimer.onTimesUp += OnTimesUp;

            anturaTimer.Reset();
            anturaTimer.Start();
        }

        public void ExitState()
        {
            //game.timerText.gameObject.SetActive(false);
            //game.roundText.gameObject.SetActive(false);
            anturaTimer.Stop();
            //game.gameTime.Stop();

            MinigamesUI.Timer.Pause();
        }

        public void Update(float delta)
        {
            if (MinigamesUI.Timer.Duration == MinigamesUI.Timer.Elapsed)
            {
                MinigamesUI.Timer.Pause();
                game.OnTimesUp();
            }

            //game.gameTime.Update(delta);
            anturaTimer.Update(delta);
            //game.timerText.text = ((int)game.gameTime.Time).ToString();

            if (game.currentLetter.dragging)
            {
                anturaTimer.Reset();
                anturaTimer.Start();
            }



            //check if we placed the CurrentLetter:
            if (!game.currentLetter.dragging && game.currentLetter.collidedTubes.Count > 0)
            {


                game.SetCurrentState(game.ResetState);


            }
        }

        public void UpdatePhysics(float delta)
        {
        }

        void OnTimesUp()
        {
            game.SetCurrentState(game.AntureState);
        }

    }
}
