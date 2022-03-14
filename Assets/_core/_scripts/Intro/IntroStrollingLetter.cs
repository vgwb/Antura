using Antura.FSM;
using Antura.LivingLetters;
using Antura.Minigames.FastCrowd;
using UnityEngine;

namespace Antura.Intro
{
    /// <summary>
    /// Behaviour of the LivingLetters in the Introduction
    /// </summary>
    public class IntroStrollingLetter : MonoBehaviour
    {
        public event System.Action onDestroy;
        public event System.Action<bool> onDropped;

        StateMachineManager stateManager = new StateMachineManager();

        // TODO refactor: the use of FSMs is not standardized across the codebase
        public IntroStrollingLetterWalkingState WalkingState { get; private set; }

        public IntroStrollingLetterIdleState IdleState { get; private set; }
        public IntroStrollingLetterFallingState FallingState { get; private set; }
        public IntroStrollingLetterHangingState HangingState { get; private set; }

        // Use Scare() method instead
        private IntroStrollingLetterScaredState ScaredState { get; set; }

        Collider[] colliders;

        public IntroFactory factory;

        public LettersWalkableArea walkableArea
        {
            get { return factory.walkableArea; }
        }

        public AnturaRunnerController antura
        {
            get { return factory.antura; }
        }

        void Awake()
        {
            colliders = GetComponentsInChildren<Collider>();

            WalkingState = new IntroStrollingLetterWalkingState(this);
            IdleState = new IntroStrollingLetterIdleState(this);
            ScaredState = new IntroStrollingLetterScaredState(this);
            FallingState = new IntroStrollingLetterFallingState(this);
            HangingState = new IntroStrollingLetterHangingState(this);

            SetCurrentState(FallingState);
        }

        void Update()
        {
            stateManager.Update(Time.deltaTime);

            // Just to be safe
            var currentState = GetCurrentState();
            if (currentState != HangingState && currentState != FallingState)
            {
                var oldPos = transform.position;

                if (oldPos.y != 0)
                {
                    oldPos.y = 0;
                }
                transform.position = oldPos;
            }

            if (Vector3.Distance(transform.position, antura.transform.position) < 15.0f)
            {
                Scare(antura.transform.position, 5);
                return;
            }
        }

        void FixedUpdate()
        {
            stateManager.UpdatePhysics(Time.fixedDeltaTime);
        }

        public bool Raycast(out float distance, out Vector3 position, Ray ray, float maxDistance)
        {
            for (int i = 0, count = colliders.Length; i < count; ++i)
            {
                RaycastHit info;
                if (colliders[i].Raycast(ray, out info, maxDistance))
                {
                    position = info.point;
                    distance = info.distance;
                    return true;
                }
            }
            position = Vector3.zero;
            distance = 0;
            return false;
        }

        public void SetCurrentState(IntroStrollingLetterState letterState)
        {
            stateManager.CurrentState = letterState;
        }

        public IntroStrollingLetterState GetCurrentState()
        {
            return (IntroStrollingLetterState)stateManager.CurrentState;
        }

        /// <summary>
        /// Scare time is the duration of being in scared state
        /// </summary>
        public void Scare(Vector3 scareSource, float scareTime)
        {
            ScaredState.ScaredDuration = scareTime;
            ScaredState.ScareSource = scareSource;

            if (GetCurrentState() == IdleState || GetCurrentState() == WalkingState)
            {
                SetCurrentState(ScaredState);
            }
        }

        void OnDestroy()
        {
            if (onDestroy != null)
            {
                onDestroy();
            }
        }

        public void DropOnArea(DropAreaWidget area)
        {
            var currentData = area.GetActiveData();

            if (currentData != null)
            {
                bool matching = GetComponent<LivingLetterController>().Data == currentData;

                if (onDropped != null)
                {
                    onDropped(matching);
                }
            }
        }
    }
}
