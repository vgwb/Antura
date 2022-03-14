using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Intro
{
    public class IntroStrollingLetterFallingState : IntroStrollingLetterState
    {
        LivingLetterController view;
        float fallSpeed = 0;

        public IntroStrollingLetterFallingState(IntroStrollingLetter letter) : base(letter)
        {
            view = letter.gameObject.GetComponent<LivingLetterController>();
        }

        public override void EnterState()
        {
            fallSpeed = 0;

            // set letter animation
            view.Falling = true;
        }

        public override void ExitState()
        {
            view.Falling = false;
        }

        public override void Update(float delta)
        {
            fallSpeed += Physics.gravity.y * delta;

            var currentPos = view.transform.position;

            currentPos.y += fallSpeed * delta;

            if (currentPos.y <= 0)
            {
                currentPos.y = 0;
                letter.SetCurrentState(letter.IdleState);
            }

            view.transform.position = currentPos;
        }

        public override void UpdatePhysics(float delta)
        {
        }
    }
}
