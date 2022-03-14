using Antura.LivingLetters;
using UnityEngine;
using DG.Tweening;

namespace Antura.Minigames.HideAndSeek
{
    public enum MovementType
    {
        Normal,
        OnlyRight,
        OnlyLeft
    }

    public class HideAndSeekLetterController : MonoBehaviour
    {
        public HideAndSeekGame game;

        public delegate void TouchAction(int i);
        public event TouchAction onLetterTouched;
        public event TouchAction onLetterReturned;

        bool lockMovement = false;

        void Start()
        {
            view = GetComponent<LivingLetterController>();
        }

        public void PlayResultAnimation(bool win)
        {
            if (moveTweener != null)
            {
                moveTweener.Kill();
            }

            lockMovement = true;
            if (win)
            {
                view.SetState(LLAnimationStates.LL_dancing);
                view.DoDancingWin();
            }
            else
            {
                view.SetState(LLAnimationStates.LL_dancing);
                view.DoDancingLose();
            }
        }

        void MoveTo(Vector3 position, float duration)
        {
            if (lockMovement)
                return;

            view.SetState(LLAnimationStates.LL_walking);
            if (position == positionStart)
                view.HasFear = true;

            view.SetWalkingSpeed(1f);
            if (moveTweener != null)
            {
                moveTweener.Kill();
            }

            moveTweener = transform.DOLocalMove(position, duration).OnComplete(
                delegate ()
                {
                    view.SetState(LLAnimationStates.LL_idle);
                    if (position == positionUncovered)
                    {
                        startTime = Time.time;
                        isArrived = true;
                    }
                    else if (position == positionStart)
                    {
                        isMoving = false;
                        isClickable = false;
                        view.HasFear = false;
                    }
                });
        }

        public void ResetLetter()
        {
            GetComponent<Animator>().SetTrigger("doReset");
            view.SetState(LLAnimationStates.LL_idle);
            isArrived = false;
            isMoving = false;
            isClickable = false;
            lockMovement = false;
        }

        void Update()
        {
            if (!lockMovement && isArrived && Time.time > startTime + idleTime)
            {
                MoveTo(positionStart, walkDuration / 2);
                isArrived = false;
                if (onLetterReturned != null)
                    onLetterReturned(id);
            }
        }

        public void SetStartPosition(Vector3 pos)
        {
            positionStart = pos;
        }

        public void MoveTutorial()
        {
            Vector3 uncoveredPosition = new Vector3(transform.position.x - 4.0f, transform.position.y, transform.position.z);
            isClickable = true;
            MoveTo(uncoveredPosition, walkDuration);
        }


        public bool Move()
        {
            if (lockMovement)
                return false;

            if (!isMoving)
            {
                int direction;

                if (movement == MovementType.OnlyLeft)
                {
                    direction = -1;
                }
                else if (movement == MovementType.OnlyRight)
                {
                    direction = 1;
                }
                else
                    direction = Random.Range(0, 2) * 2 - 1; // -1 or 1

                float moveOffset = direction * minMove;
                positionUncovered = positionStart + new Vector3(moveOffset, 0, 0);

                isMoving = true;
                isClickable = true;

                MoveTo(positionUncovered, walkDuration);
                return true;
            }
            return false;
        }

        void OnMouseDown()
        {
            if (isClickable && onLetterTouched != null)
            {
                HideAndSeekConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Blip);
                HideAndSeekConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(view.Data);
                isClickable = false;
                onLetterTouched(id);
            }
        }

        public void SetMovement(MovementType mov)
        {
            movement = mov;
        }


        #region VARIABLES
        public int id;
        float idleTime => 0.5f + 4f * (1 - game.Difficulty);
        public float ray = 5f;
        float minMove = 3f;

        private bool isClickable = false;

        private bool isMoving = false;
        private bool isArrived = false;

        private float startTime;
        private Vector3 positionStart;
        private Vector3 positionUncovered;

        private Animator anim;

        [HideInInspector]
        public LivingLetterController view;

        Tweener moveTweener;

        public float walkDuration = 2f;

        private MovementType movement = MovementType.Normal;
        #endregion
    }
}
