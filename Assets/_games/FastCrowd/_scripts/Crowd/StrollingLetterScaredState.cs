using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.FastCrowd
{
    public class StrollingLetterScaredState : StrollingLetterState
    {
        public float ScaredDuration = 1.0f;
        public Vector3 ScareSource;

        const float SCARED_RUN_SPEED = 10.0f;

        float scaredTimer;

        LetterCharacterController movement;

        public StrollingLetterScaredState(StrollingLivingLetter letter) : base(letter)
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
            if (Vector3.Distance(letter.transform.position, letter.antura.transform.position) < 15.0f)
            {
                ScaredDuration = 3;
                ScareSource = letter.antura.transform.position;
            }
            else if (Vector3.Distance(letter.transform.position, ScareSource) > 7.0f)
            {
                scaredTimer = Mathf.Min(0.5f, scaredTimer);
            }

            scaredTimer -= delta;

            Vector3 dropPosition = letter.walkableArea.focusPosition.position;
            Vector3 dropDistance = dropPosition - letter.transform.position;
            bool nearDropContainer = dropDistance.magnitude < 10;

            bool isOutsideWalkingPath = Vector3.Distance(
                letter.transform.position,
                letter.walkableArea.GetNearestPoint(letter.transform.position, true)
                ) > 6.0f;

            if (isOutsideWalkingPath && !nearDropContainer)
                scaredTimer = Mathf.Min(0.1f, scaredTimer);

            if (scaredTimer <= 0)
            {
                letter.SetCurrentState(letter.WalkingState);
                return;
            }

            // Run-away from danger!
            Vector3 runDirection = letter.transform.position - ScareSource;
            runDirection.y = 0;
            runDirection.Normalize();

            if (nearDropContainer)
            {
                runDirection = Vector3.Cross(letter.walkableArea.focusPosition.forward, Vector3.up).normalized;
            }

            movement.MoveAmount(runDirection * SCARED_RUN_SPEED * delta);
            movement.LerpLookAt(letter.transform.position + runDirection, 4 * delta);
        }

        public override void UpdatePhysics(float delta)
        {
        }
    }
}
