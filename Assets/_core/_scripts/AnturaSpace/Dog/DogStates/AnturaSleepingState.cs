using System;
using Antura.Dog;
using UnityEngine;

namespace Antura.AnturaSpace
{
    public class AnturaSleepingState : AnturaState
    {
        private float timer = 0.5f;

        public AnturaSleepingState(AnturaSpaceScene controller) : base(controller)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            controller.Antura.SetTarget(controller.SceneCenter, true);
            timer = 0.5f;
        }

        public override void OnTouched()
        {
            base.OnTouched();
            controller.CurrentState = controller.Idle;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            if (controller.Antura.HasReachedTarget)
            {
                timer -= delta;

                if (timer <= 0)
                {
                    controller.Antura.AnimationController.State = AnturaAnimationStates.sleeping;
                }
            }
            else if (controller.Antura.AnimationController.State == AnturaAnimationStates.sleeping)
            {
                controller.Antura.AnimationController.State = AnturaAnimationStates.idle;
            }
        }

        public override void ExitState()
        {
            base.ExitState();
            controller.LastTimeCatching = Time.realtimeSinceStartup;
            controller.Antura.SetTarget(null, false);
        }
    }
}
