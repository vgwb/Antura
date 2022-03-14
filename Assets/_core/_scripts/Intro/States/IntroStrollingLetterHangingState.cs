using System.Collections.Generic;
using Antura.LivingLetters;
using Antura.Minigames.FastCrowd;
using UnityEngine;

namespace Antura.Intro
{
    public class IntroStrollingLetterHangingState : IntroStrollingLetterState
    {
        List<IntroStrollingLetter> near = new List<IntroStrollingLetter>();

        LetterCharacterController movement;

        public IntroStrollingLetterHangingState(IntroStrollingLetter letter) : base(letter)
        {
            movement = letter.GetComponent<LetterCharacterController>();
        }

        public override void EnterState()
        {
            letter.gameObject.GetComponent<LivingLetterController>().SetState(LLAnimationStates.LL_dragging);
        }

        public override void ExitState()
        {
            letter.gameObject.GetComponent<LivingLetterController>().SetState(LLAnimationStates.LL_idle);
        }

        public override void Update(float delta)
        {
            // Scare neighbourhood
            near.Clear();
            letter.factory.GetNearLetters(near, letter.transform.position, 5.0f);
            for (int i = 0, count = near.Count; i < count; ++i)
                near[i].Scare(letter.transform.position, 2);

            // Face Camera!
            movement.LerpLookAt(Camera.main.transform.position, 8 * delta);
        }

        public override void UpdatePhysics(float delta)
        {
        }
    }
}
