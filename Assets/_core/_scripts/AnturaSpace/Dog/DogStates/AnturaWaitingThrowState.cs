﻿using Antura.Dog;
using UnityEngine;

namespace Antura.AnturaSpace
{
    public class AnturaWaitingThrowState : AnturaState
    {
        float shoutTimer;
        float timeInThisState;

        GameObject waitForLaunchPoint;

        public AnturaWaitingThrowState(AnturaSpaceScene controller) : base(controller)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            shoutTimer = Random.Range(1, 3);
            timeInThisState = 0;
            controller.AnturaMain.AnimController.State = AnturaAnimationStates.idle;

            waitForLaunchPoint = new GameObject("WaitForLaunch");
            controller.AnturaMain.SetTarget(waitForLaunchPoint.transform, true);
            controller.AnturaMain.Excited = true;
        }

        public override void ExitState()
        {
            controller.AnturaMain.Excited = false;
            GameObject.Destroy(waitForLaunchPoint);
            base.ExitState();
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            if (controller.DraggedTransform == null)
            {
                controller.CurrentState = controller.Idle;
                return;
            }

            waitForLaunchPoint.transform.position = controller.DraggedTransform.position + Camera.main.transform.forward * 6;
            waitForLaunchPoint.transform.forward = (controller.DraggedTransform.position - waitForLaunchPoint.transform.position).normalized;

            if (shoutTimer > 0 & controller.AnturaMain.HasReachedTarget)
            {
                timeInThisState += delta;
                shoutTimer -= delta;

                if (shoutTimer <= 0)
                {
                    shoutTimer = Random.Range(1.5f, 4);

                    if (Random.value < 0.3f)
                    {
                        controller.AnturaMain.AnimController.DoSniff();
                        Audio.AudioManager.I.PlaySound(Sfx.DogSnorting);
                    }
                    else
                    {
                        controller.AnturaMain.AnimController.DoShout(() => { Audio.AudioManager.I.PlaySound(Sfx.DogBarking); });
                    }
                }
            }

            if (timeInThisState > 10)
            {
                controller.CurrentState = controller.Idle;
            }
        }
    }
}
