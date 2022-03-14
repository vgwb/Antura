using System;
using Antura.Dog;
using UnityEngine;

namespace Antura.AnturaSpace
{
    public class AnturaIdleState : AnturaState
    {
        private float sitTimer;
        private float animateTimer;
        private float timeToStayInThisState;

        public AnturaIdleState(AnturaSpaceScene controller) : base(controller)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            controller.Antura.AnimationController.State = AnturaAnimationStates.idle;
            sitTimer = 0.5f;
            timeToStayInThisState = 4 + UnityEngine.Random.value * 2;
            animateTimer = UnityEngine.Random.Range(4, 8) - 2 * controller.AnturaHappiness;
            controller.Antura.SetTarget(controller.SceneCenter, true);
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            if (controller.DraggedTransform != null)
            {
                controller.CurrentState = controller.WaitingThrow;
            }
            else if (controller.NextObjectToCatch != null)
            {
                controller.CurrentState = controller.Catching;
                return;
            }
            else if (controller.InCustomizationMode)
            {
                controller.CurrentState = controller.Customization;
                return;
            }

            if (controller.Antura.HasReachedTarget)
            {
                timeToStayInThisState -= delta;

                sitTimer -= delta;

                if (sitTimer <= 0)
                {
                    if (Time.realtimeSinceStartup - controller.LastTimeCatching > 30.0f)
                    {
                        controller.CurrentState = controller.Sleeping;
                        return;
                    }

                    controller.Antura.AnimationController.State = AnturaAnimationStates.sitting;
                }
            }

            if (timeToStayInThisState <= 0 && controller.HasPlayerBones && controller.AnturaHappiness < 0.1f)
            {
                controller.CurrentState = controller.DrawingAttention;
            }
            else if (controller.AnturaHappiness > 0.2f)
            {
                animateTimer -= delta;

                if (animateTimer <= 0 && Vector3.Distance(controller.Antura.transform.position, controller.SceneCenter.position) < 6)
                {
                    controller.CurrentState = controller.Animate;
                }
            }
        }

        public override void ExitState()
        {
            controller.Antura.SetTarget(null, false);
            base.ExitState();
        }

        public override void OnTouched()
        {
            base.OnTouched();

            if (controller.Antura.HasReachedTarget)
            {
                controller.Antura.AnimationController.DoShout(() => { Audio.AudioManager.I.PlaySound(Sfx.DogBarking); });
            }
        }
    }
}
