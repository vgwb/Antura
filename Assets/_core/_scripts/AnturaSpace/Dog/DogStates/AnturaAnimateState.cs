using System;
using Antura.Dog;

namespace Antura.AnturaSpace
{
    public class AnturaAnimateState : AnturaState
    {
        private float timer = 2.0f;
        private AnturaAnimationStates state;

        public AnturaAnimateState(AnturaSpaceScene controller) : base(controller)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            timer = 4.0f;

            if (controller.AnturaHappiness > 0.95f)
            {
                state = AnturaAnimationStates.dancing;
            }
            else
            {
                float p = UnityEngine.Random.value * controller.AnturaHappiness;

                if (p < 0.25f)
                {
                    state = AnturaAnimationStates.digging;
                }
                else if (p < 0.45f)
                {
                    state = AnturaAnimationStates.sheeping;
                }
                else if (p < 0.7f)
                {
                    state = AnturaAnimationStates.bellyUp;
                }
                else
                {
                    state = AnturaAnimationStates.dancing;
                }
            }
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            controller.Antura.AnimationController.State = state;

            timer -= delta;

            if (timer <= 0)
            {
                controller.CurrentState = controller.Idle;
            }
        }

        public override void ExitState()
        {
            controller.Antura.AnimationController.State = AnturaAnimationStates.idle;
            base.ExitState();
        }
    }
}
