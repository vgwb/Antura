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

            controller.Antura.SetTarget(objectToCatch.transform, false);
            objectRigidBody = objectToCatch.GetComponent<Rigidbody>();
            controller.Antura.Excited = true;
        }

        public override void ExitState()
        {
            base.ExitState();

            controller.Antura.Excited = false;
            controller.LastTimeCatching = Time.realtimeSinceStartup;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            if (!objectInteracted && !controller.Antura.IsJumping &&
                (controller.Antura.HasReachedTarget || controller.Antura.PlanarDistanceFromTarget < 5))
            {
                if ((controller.Antura.TargetHeight >= 2 && objectRigidBody != null && objectRigidBody.velocity.y > 10))
                {
                    objectInteracted = true;

                    // Jump & Interact
                    controller.Antura.AnimationController.DoSmallJumpAndGrab(InteractWithCurrentObject);

                }
                else if (controller.Antura.TargetHeight <= 4.5f)
                {
                    objectInteracted = true;

                    // Interact from the ground
                    controller.Antura.AnimationController.DoBite(InteractWithCurrentObject);
                }
            }
        }

        private void InteractWithCurrentObject()
        {
            if (objectToCatch.Edible)
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
