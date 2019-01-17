using Antura.Dog;
using UnityEngine;

namespace Antura.Test
{
    public class AnturaAnimationTester : MonoBehaviour
    {
        AnturaAnimationController antura;

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
            antura = GetComponent<AnturaAnimationController>();
        }
	
        void Update ()
        {
            antura.IsAngry = angry;

            antura.SetWalkingSpeed(walkSpeed);

            if (doTransition)
            {
                doTransition = false;
                antura.State = targetState;
            }

            if (doBurp)
            {
                doBurp = false;
                antura.DoBurp();
            }

            if (doBite)
            {
                doBite = false;
                antura.DoBite();
            }

            if (doShout)
            {
                doShout = false;
                antura.DoShout();
            }


            if (doSniff)
            {
                doSniff = false;
                antura.DoSniff();
            }


            if (onJumpStart)
            {
                onJumpStart = false;
                antura.OnJumpStart();
            }


            if (onJumpMiddle)
            {
                onJumpMiddle = false;
                antura.OnJumpMaximumHeightReached();
            }

            if (onJumpGrab)
            {
                onJumpGrab = false;
                antura.OnJumpGrab();
            }

            if (onJumpEnd)
            {
                onJumpEnd = false;
                antura.OnJumpEnded();
            }

            if (doCharge)
            {
                doCharge = false;
                antura.DoCharge(null);
            }

            if (doSpitOpen)
            {
                doSpitOpen = false;
                antura.DoSpit(true);
            }

            if (doSpitClosed)
            {
                doSpitClosed = false;
                antura.DoSpit(false);
            }

            if (onSlipStart)
            {
                onSlipStart = false;
                antura.OnSlipStarted();
            }

            if (onSlipEnd)
            {
                onSlipEnd = false;
                antura.OnSlipEnded();
            }
        }
    }
}

// refactor: add to the Test namespace