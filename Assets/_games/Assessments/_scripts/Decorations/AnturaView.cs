using Antura.Dog;
using Antura.Minigames.ReadingGame;
using DG.Tweening;
using Kore.Coroutines;
using System.Collections;
using UnityEngine;

namespace Antura.Assessment
{
    public class AnturaView : MonoBehaviour
    {
        AnturaAnimationController controller = null;
        ReadingGameAntura antura = null;

        // States (Coroutine State Machine => easier animations)
        IEnumerator Entering, Idle, Exiting, Angry, Happy;
        StateEvent becomeHappy, becomeAngry, becomeExiting;
        StateCache state = null;

        private void Awake()
        {
            controller = GetComponent<AnturaAnimationController>();

            // Reuse a well made animation
            antura = gameObject.AddComponent<ReadingGameAntura>();
            antura.enabled = false;
            antura.AllowSitting = true;
            state = State.Cache();

            Entering = EnteringState();
            Idle = IdleState();
            Exiting = ExitingState();
            Angry = AngryState();
            Happy = HappyState();

            becomeExiting = state.Event();
            becomeHappy = state.Event();
            becomeAngry = state.Event();
        }

        private void Start()
        {
            var startPos = ItemFactory.Instance.AnturaStart;
            transform.localPosition = startPos.localPosition;
            transform.localRotation = startPos.localRotation;

            Koroutine.Run(Entering);
        }

        AssessmentAudioManager audioManager;

        public void WrongAssessment(AssessmentAudioManager audioManager)
        {
            this.audioManager = audioManager;
            becomeAngry.Trigger();
        }

        public void CorrectAssessment(AssessmentAudioManager audioManager)
        {
            this.audioManager = audioManager;
            becomeHappy.Trigger();
        }

        private float timer = 0;
        private void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0)
            {
                timer = 0;
            }
        }

        // ##################################################
        //                STATES IMPLEMENTATION
        // ##################################################

        IEnumerator EnteringState()
        {
            while (true)
            {
                yield return state.EnterState();

                controller.State = AnturaAnimationStates.walking;
                controller.IsExcited = true;

                yield return Wait.For(0.6f);

                var middlePos = ItemFactory.Instance.AnturaMiddle;
                transform.DOMove(middlePos.localPosition, 3.0f).SetEase(Ease.InOutSine);

                yield return Wait.For(2.4f);
                transform.DORotateQuaternion(middlePos.localRotation, 0.5f);

                yield return Wait.For(0.3f);
                yield return state.Change(Idle);
            }
        }

        private IEnumerator HappyState()
        {
            while (true)
            {
                yield return state.EnterState();
                controller.State = AnturaAnimationStates.bitingTail;
                yield return Wait.For(3.0f);

                controller.State = AnturaAnimationStates.sitting;

                yield return state.Change(Idle);
            }
        }

        private IEnumerator AngryState()
        {
            while (true)
            {
                yield return state.EnterState();
                controller.IsAngry = true;
                controller.DoShout(() => audioManager.AnturaAngrySound());
                yield return Wait.For(3.0f);
                controller.IsAngry = true;
                controller.State = AnturaAnimationStates.sitting;

                yield return state.Change(Idle);
            }
        }

        IEnumerator IdleState()
        {
            while (true)
            {
                yield return state.EnterState(() =>
                {
                    controller.State = AnturaAnimationStates.idle;
                    controller.IsAngry = false;
                    controller.State = AnturaAnimationStates.sitting;
                });

                if (becomeHappy)
                {
                    yield return state.Change(Happy);
                }

                if (becomeAngry)
                {
                    yield return state.Change(Angry);
                }
                if (becomeExiting)
                {
                    yield return state.Change(Exiting);
                }
            }
        }

        IEnumerator ExitingState()
        {
            yield return state.EnterState();

            // Exit
            yield return null;
        }
    }
}
