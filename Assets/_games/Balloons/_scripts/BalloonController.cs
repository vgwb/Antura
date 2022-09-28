using Antura.UI;
using UnityEngine;

namespace Antura.Minigames.Balloons
{
    public class BalloonController : MonoBehaviour
    {
        public FloatingLetterController parentFloatingLetter;
        public Collider balloonCollider;
        public Renderer balloonRenderer;
        public Animator animator;

        private int taps = 0;

        [HideInInspector]
        public bool isPopped;

        // Middle balloon adjustment for Triple Balloon Variation
        private bool adjustMiddleBalloon = false;
        private Vector3 adjustedLocalPosition = new Vector3(0f, 3.5f, 0f);
        private float adjustDuration = 7.5f;
        private float adjustProgress;
        private float adjustProgressPercentage;


        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            if (adjustMiddleBalloon)
            {
                if (adjustProgress < adjustDuration)
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, adjustedLocalPosition, adjustProgressPercentage);
                    adjustProgress += Time.deltaTime;
                    adjustProgressPercentage = adjustProgress / adjustDuration;
                }
                else
                {
                    adjustMiddleBalloon = false;
                }
            }
        }

        public void OnMouseDown()
        {
            if (GlobalUI.I.IsFingerOverUI()) return;
            TapAction();
        }

        void TapAction()
        {
            taps++;
            if (taps >= parentFloatingLetter.tapsNeeded)
            {
                Pop();
            }
            else
            {
                animator.SetTrigger("Tap");
            }
        }

        public void Pop()
        {
            isPopped = true;
            balloonCollider.enabled = false;
            parentFloatingLetter.Pop();
            BalloonsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.BalloonPop);
            animator.SetBool("Pop", true);
            GameObject poof = Instantiate(BalloonsGame.instance.FxParticlesPoof, transform.position, Quaternion.identity) as GameObject;
            Destroy(poof, 10);
        }

        public void AdjustMiddleBalloon()
        {
            adjustProgress = 0f;
            adjustProgressPercentage = 0f;
            adjustMiddleBalloon = true;
        }

        public void DisableCollider()
        {
            balloonCollider.enabled = false;
        }

        public void EnableCollider()
        {
            balloonCollider.enabled = true;
        }

        public void SetColor(Color color)
        {
            balloonRenderer.material.color = color;
        }
    }
}
