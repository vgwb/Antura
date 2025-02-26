using UnityEngine;

namespace Antura.AnturaSpace
{
    public class AnturaCatchingState : AnturaState
    {
        private ThrowableObject objectToCatch;
        private bool objectInteracted;
        private Rigidbody objectRigidBody;

        public AnturaCatchingState(AnturaSpaceScene controller) : base(controller)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            var newObjectToCatch = controller.NextObjectToCatch;
            if (newObjectToCatch == null)
            {
                controller.CurrentState = controller.Idle;
                return;
            }

            objectToCatch = newObjectToCatch;
            objectInteracted = false;

            controller.AnturaMain.SetTarget(objectToCatch.transform, false);
            objectRigidBody = objectToCatch.GetComponent<Rigidbody>();
            controller.AnturaMain.Excited = true;
        }

        public override void ExitState()
        {
            base.ExitState();

            controller.AnturaMain.Excited = false;
            controller.LastTimeCatching = Time.realtimeSinceStartup;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            if (!objectInteracted && !controller.AnturaMain.IsJumping &&
                (controller.AnturaMain.HasReachedTarget || controller.AnturaMain.PlanarDistanceFromTarget < 5))
            {
                if ((controller.AnturaMain.TargetHeight >= 2 && objectRigidBody != null && objectRigidBody.linearVelocity.y > 10))
                {
                    objectInteracted = true;

                    // Jump & Interact
                    controller.AnturaMain.AnimController.DoSmallJumpAndGrab(InteractWithCurrentObject);

                }
                else if (controller.AnturaMain.TargetHeight <= 4.5f)
                {
                    objectInteracted = true;

                    // Interact from the ground
                    controller.AnturaMain.AnimController.DoBite(InteractWithCurrentObject);
                }
            }
        }

        private void InteractWithCurrentObject()
        {
            if (objectToCatch != null && objectToCatch.Edible)
            {
                controller.EatObject(objectToCatch);
            }
            else
            {
                controller.HitObject(objectToCatch);
            }

            objectToCatch = null;
            objectInteracted = false;
            controller.CurrentState = controller.Idle;

        }
    }
}
