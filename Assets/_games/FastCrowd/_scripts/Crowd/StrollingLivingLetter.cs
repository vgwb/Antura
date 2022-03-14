using Antura.FSM;
using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.FastCrowd
{
    [RequireComponent(typeof(LetterCharacterController))]
    public class StrollingLivingLetter : MonoBehaviour
    {
        public event System.Action onDestroy;
        public event System.Action<bool> onDropped;

        StateMachineManager stateManager = new StateMachineManager();

        public StrollingLetterWalkingState WalkingState { get; private set; }
        public StrollingLetterIdleState IdleState { get; private set; }
        public StrollingLetterFallingState FallingState { get; private set; }
        public StrollingLetterHangingState HangingState { get; private set; }
        public StrollingLetterTutorialState TutorialState { get; private set; }

        // Use Scare() method instead
        private StrollingLetterScaredState ScaredState { get; set; }

        Collider[] colliders;

        public LetterCrowd crowd;
        public LettersWalkableArea walkableArea { get { return crowd.walkableArea; } }
        public AnturaRunnerController antura { get { return crowd.antura; } }

        void Awake()
        {
            colliders = GetComponentsInChildren<Collider>();

            WalkingState = new StrollingLetterWalkingState(this);
            IdleState = new StrollingLetterIdleState(this);
            ScaredState = new StrollingLetterScaredState(this);
            FallingState = new StrollingLetterFallingState(this);
            HangingState = new StrollingLetterHangingState(this);
            TutorialState = new StrollingLetterTutorialState(this);

            SetCurrentState(FallingState);
        }

        void Start()
        {

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
                    oldPos.y = 0;
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

        public void SetCurrentState(StrollingLetterState letterState)
        {
            stateManager.CurrentState = letterState;
        }

        public StrollingLetterState GetCurrentState()
        {
            return (StrollingLetterState)stateManager.CurrentState;
        }

        /// <summary>
        /// Scare time is the duration of being in scared state
        /// </summary>
        public void Scare(Vector3 scareSource, float scareTime)
        {
            ScaredState.ScaredDuration = scareTime;
            ScaredState.ScareSource = scareSource;

            if (GetCurrentState() == IdleState ||
                GetCurrentState() == WalkingState)
                SetCurrentState(ScaredState);
        }

        public void Tutorial()
        {
            SetCurrentState(TutorialState);
        }

        void OnDestroy()
        {
            if (onDestroy != null)
                onDestroy();
        }

        public void DropOnArea(DropAreaWidget area)
        {
            var currentData = area.GetActiveData();

            if (currentData != null)
            {
                var myData = GetComponent<LivingLetterController>().Data;

                bool matching = FastCrowdConfiguration.Instance.IsDataMatching(currentData, myData);

                if (onDropped != null)
                    onDropped(matching);
            }
        }
    }
}
