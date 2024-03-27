using System.Collections;
using Antura.Database;
using Antura.LivingLetters;
using Antura.Tutorial;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class DiscoverCountryPlayState : FSM.IState
    {
        private DiscoverCountryGame game;


        public DiscoverCountryPlayState(DiscoverCountryGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            game.SetCurrentState(game.ResultState);
        }

        public void UpdatePhysics(float delta) {}
    }
}
