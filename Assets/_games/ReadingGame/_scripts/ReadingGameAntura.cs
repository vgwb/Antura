using Antura.Dog;
using Antura.FSM;
using UnityEngine;

namespace Antura.Minigames.ReadingGame
{
    public class ReadingGameAntura : MonoBehaviour
    {
        public enum AnturaMood
        {
            HAPPY,
            ANGRY,
            SAD
        }

        [HideInInspector]
        public AnturaAnimationController animator;

        StateMachineManager stateManager = new StateMachineManager();

        public WalkAnturaState WalkingState { get; private set; }
        public IdleAnturaState IdleState { get; private set; }


        AnturaMood mood;
        public AnturaMood Mood
        {
            get
            {
                return mood;
            }

            set
            {
                if (this.mood == value)
                    return;

                mood = value;
                if (value == ReadingGameAntura.AnturaMood.ANGRY)
                {
                    if (animator.State == AnturaAnimationStates.sitting)
                        animator.State = AnturaAnimationStates.idle;

                    animator.DoShout(() => { ReadingGameConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.DogBarking); });
                }
            }
        }

        public bool AllowSitting = true;

        void Awake()
        {
            animator = GetComponent<AnturaAnimationController>();

            WalkingState = new WalkAnturaState(this);
            IdleState = new IdleAnturaState(this);

            SetCurrentState(IdleState);
        }

        void Update()
        {
            stateManager.Update(Time.deltaTime);
        }

        void FixedUpdate()
        {
            stateManager.UpdatePhysics(Time.fixedDeltaTime);
        }

        public void SetCurrentState(AnturaState state)
        {
            stateManager.CurrentState = state;
        }

        public AnturaState GetCurrentState()
        {
            return (AnturaState)stateManager.CurrentState;
        }
    }
}
