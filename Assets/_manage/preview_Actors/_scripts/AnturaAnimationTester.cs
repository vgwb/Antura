using Antura.Dog;
using UnityEngine;

namespace Antura.Test
{
    public class AnturaAnimationTester : MonoBehaviour
    {
        public AnturaPetSwitcher petSwitcher;
        public AnturaAnimationController animController => petSwitcher.AnimController;
        public bool UsingInAnimationUI;

        public AnturaAnimationStates targetState;
        public bool doTransition;

        [Range(0,1)]
        public float walkSpeed;

        public bool angry;

        public bool onJumpStart;
        public bool onJumpGrab;
        public bool onJumpMiddle;
        public bool onJumpEnd;
        public bool doCharge;

        public bool doBurp;
        public bool doBite;
        public bool doShout;
        public bool doSniff;

        public bool doSpitOpen;
        public bool doSpitClosed;

        public bool onSlipStart;
        public bool onSlipEnd;

        void Start ()
        {
            if(!UsingInAnimationUI)
                petSwitcher = GetComponentInChildren<AnturaPetSwitcher>();
        }

        void Update ()
        {
            animController.IsAngry = angry;

            animController.SetWalkingSpeed(walkSpeed);

            if (doTransition || UsingInAnimationUI)
            {
                if (!UsingInAnimationUI)
                    doTransition = false;
                animController.State = targetState;
            }

            if (doBurp)
            {
                if (!UsingInAnimationUI)
                    doBurp = false;
                animController.DoBurp();
            }

            if (doBite)
            {
                if (!UsingInAnimationUI)
                    doBite = false;
                animController.DoBite();
            }

            if (doShout)
            {
                if (!UsingInAnimationUI)
                    doShout = false;
                animController.DoShout();
            }


            if (doSniff)
            {
                if (!UsingInAnimationUI)
                    doSniff = false;
                animController.DoSniff();
            }


            if (onJumpStart)
            {
                if (!UsingInAnimationUI)
                    onJumpStart = false;
                animController.OnJumpStart();
            }


            if (onJumpMiddle)
            {
                if (!UsingInAnimationUI)
                    onJumpMiddle = false;
                animController.OnJumpMaximumHeightReached();
            }

            if (onJumpGrab)
            {
                if (!UsingInAnimationUI)
                    onJumpGrab = false;
                animController.OnJumpGrab();
            }

            if (onJumpEnd)
            {
                if (!UsingInAnimationUI)
                    onJumpEnd = false;
                animController.OnJumpEnded();
            }

            if (doCharge)
            {
                if (!UsingInAnimationUI)
                    doCharge = false;
                animController.DoCharge(null);
            }

            if (doSpitOpen)
            {
                if (!UsingInAnimationUI)
                    doSpitOpen = false;
                animController.DoSpit(true);
            }

            if (doSpitClosed)
            {
                if (!UsingInAnimationUI)
                    doSpitClosed = false;
                animController.DoSpit(false);
            }

            if (onSlipStart)
            {
                if (!UsingInAnimationUI)
                    onSlipStart = false;
                animController.OnSlipStarted();
            }

            if (onSlipEnd)
            {
                if (!UsingInAnimationUI)
                    onSlipEnd = false;
                animController.OnSlipEnded();
            }
        }
    }
}

// refactor: add to the Test namespace
