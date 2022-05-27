using Antura.Dog;
using UnityEngine;

namespace Antura.Test
{
    public class AnturaAnimationTester : MonoBehaviour
    {
        public AnturaAnimationController antura;
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
                antura = GetComponent<AnturaAnimationController>();
        }
	
        void Update ()
        {
            antura.IsAngry = angry;

            antura.SetWalkingSpeed(walkSpeed);

            if (doTransition || UsingInAnimationUI)
            {
                if (!UsingInAnimationUI)
                    doTransition = false;
                antura.State = targetState;
            }

            if (doBurp)
            {
                if (!UsingInAnimationUI)
                    doBurp = false;
                antura.DoBurp();
            }

            if (doBite)
            {
                if (!UsingInAnimationUI)
                    doBite = false;
                antura.DoBite();
            }

            if (doShout)
            {
                if (!UsingInAnimationUI)
                    doShout = false;
                antura.DoShout();
            }


            if (doSniff)
            {
                if (!UsingInAnimationUI)
                    doSniff = false;
                antura.DoSniff();
            }


            if (onJumpStart)
            {
                if (!UsingInAnimationUI)
                    onJumpStart = false;
                antura.OnJumpStart();
            }


            if (onJumpMiddle)
            {
                if (!UsingInAnimationUI)
                    onJumpMiddle = false;
                antura.OnJumpMaximumHeightReached();
            }

            if (onJumpGrab)
            {
                if (!UsingInAnimationUI)
                    onJumpGrab = false;
                antura.OnJumpGrab();
            }

            if (onJumpEnd)
            {
                if (!UsingInAnimationUI)
                    onJumpEnd = false;
                antura.OnJumpEnded();
            }

            if (doCharge)
            {
                if (!UsingInAnimationUI)
                    doCharge = false;
                antura.DoCharge(null);
            }

            if (doSpitOpen)
            {
                if (!UsingInAnimationUI)
                    doSpitOpen = false;
                antura.DoSpit(true);
            }

            if (doSpitClosed)
            {
                if (!UsingInAnimationUI)
                    doSpitClosed = false;
                antura.DoSpit(false);
            }

            if (onSlipStart)
            {
                if (!UsingInAnimationUI)
                    onSlipStart = false;
                antura.OnSlipStarted();
            }

            if (onSlipEnd)
            {
                if (!UsingInAnimationUI)
                    onSlipEnd = false;
                antura.OnSlipEnded();
            }
        }
    }
}

// refactor: add to the Test namespace
