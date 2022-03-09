using Antura.LivingLetters;
using Antura.Minigames.FastCrowd;
using UnityEngine;

namespace Antura.Intro
{
    public class IntroStrollingLetterScaredState : IntroStrollingLetterState
    {
        public float ScaredDuration = 1.0f;
        public Vector3 ScareSource;

        const float SCARED_RUN_SPEED = 10.0f;

        float scaredTimer;

        LetterCharacterController movement;

        public IntroStrollingLetterScaredState(IntroStrollingLetter letter) : base(letter)
        {
            movement = letter.GetComponent<LetterCharacterController>();
        }

        public override void EnterState()
        {
            scaredTimer = ScaredDuration;

            // set letter animation
            letter.gameObject.GetComponent<LivingLetterController>().HasFear = true;
            letter.gameObject.GetComponent<LivingLetterController>().SetState(LLAnimationStates.LL_walking);
            letter.gameObject.GetComponent<LivingLetterController>().SetWalkingSpeed(LivingLetterController.RUN_SPEED);
        }

        public override void ExitState()
        {
            letter.gameObject.GetComponent<LivingLetterController>().SetState(LLAnimationStates.LL_idle);
            letter.gameObject.GetComponent<LivingLetterController>().HasFear = false;
        }

        public override void Update(float delta)
        {
            // Stay scared if danger is near
            if (Vector3.Distance(letter.transform.position, letter.antura.transform.position) < 20.0f)
            {
                ScaredDuration = 3;
                ScareSource = letter.antura.transform.position;
            }
            else if (Vector3.Distance(letter.transform.position, ScareSource) > 10.0f)
            {
                scaredTimer = Mathf.Min(0.5f, scaredTimer);
            }

            scaredTimer -= delta;

            if (scaredTimer <= 0)
            {
                letter.SetCurrentState(letter.WalkingState);
                return;
            }

            // Run-away from danger!
            Vector3 runDirection = letter.transform.position - ScareSource;
            runDirection.y = 0;
            runDirection.Normalize();

            movement.MoveAmount(runDirection * SCARED_RUN_SPEED * delta);
            movement.LerpLookAt(letter.transform.position + runDirection, 4 * delta);
        }

        public override void UpdatePhysics(float delta)
        {
        }
    }
}
