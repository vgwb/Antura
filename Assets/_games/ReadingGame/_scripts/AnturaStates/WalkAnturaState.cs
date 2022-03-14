using Antura.Dog;
using UnityEngine;

namespace Antura.Minigames.ReadingGame
{
    public class WalkAnturaState : AnturaState
    {
        float circlePercentage = 0;

        const float radius = 20.0f;
        Vector3 right;
        Vector3 forward;
        Vector3 center;

        public WalkAnturaState(ReadingGameAntura antura)
            : base(antura)
        {
            this.antura = antura;
            var startPosition = antura.transform.position;
            center = startPosition + radius * (new Vector3(1, 0, 1)).normalized;

            right = (startPosition - center).normalized;
            forward = Vector3.Cross(right, Vector3.up);
        }

        public override void EnterState()
        {
            antura.animator.State = AnturaAnimationStates.walking;
            circlePercentage = 0;
        }

        public override void ExitState()
        {
            antura.animator.State = AnturaAnimationStates.idle;
        }
        public override void Update(float delta)
        {
            float speed;

            if (antura.animator.IsAnimationActuallyWalking)
                speed = 0.5f;
            else
                speed = 0;

            if (antura.Mood == ReadingGameAntura.AnturaMood.HAPPY)
            {
                antura.animator.SetWalkingSpeed(0);
                antura.animator.IsExcited = true;
                antura.animator.IsAngry = false;
                antura.animator.IsSad = false;

                circlePercentage += speed * delta;
            }
            else //if (antura.Mood == ReadingGameAntura.AnturaMood.ANGRY)
            {
                antura.animator.SetWalkingSpeed(1);
                antura.animator.IsExcited = false;
                antura.animator.IsAngry = true;
                antura.animator.IsSad = false;

                circlePercentage += 2f * speed * delta;
            }

            bool completed = false;
            if (circlePercentage > 2)
            {
                circlePercentage = 2;
                completed = true;
            }

            var pos = center + radius * (right * Mathf.Cos(circlePercentage * Mathf.PI) + forward * Mathf.Sin(circlePercentage * Mathf.PI));
            antura.transform.position = pos;
            antura.transform.forward = Vector3.Cross(center - pos, Vector3.up).normalized;

            if (completed)
                antura.SetCurrentState(antura.IdleState);
        }
        public override void UpdatePhysics(float delta)
        {

        }
    }
}
