using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Antura.LivingLetters;
using Antura.Minigames;

namespace Antura.Minigames.DiscoverCountry
{
    public class DiscoverCountryQuestionState : FSM.IState
    {
        DiscoverCountryGame game;

        bool firstQuestion;

        public DiscoverCountryQuestionState(DiscoverCountryGame game)
        {
            this.game = game;

            firstQuestion = true;
        }

        public void EnterState()
        {
        }

        public void ExitState()
        {
            firstQuestion = false;
        }

        public void Update(float delta)
        {
        }
        public void UpdatePhysics(float delta) { }

    }
}
