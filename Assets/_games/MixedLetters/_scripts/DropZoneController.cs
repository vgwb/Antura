using System;
using UnityEngine;
using System.Collections;
using Antura.Tutorial;

namespace Antura.Minigames.MixedLetters
{
    public class DropZoneController : MonoBehaviour
    {
        public static DropZoneController chosenDropZone;

        public const float DropZoneZ = -41.5f;

        public SpriteRenderer spriteRenderer;

        // The equilibrium scale of the throbbing. It is automatically set to how it is configured in the scene:
        private float THROB_INIT_SCALE;

        // The factor by which the drop zone expands when throbbing:
        private const float THROB_SCALE_MULTIPLIER = 1.2f;

        // The period of the throb, in seconds:
        private const float THROB_PERIOD = 0.33f;

        // If the drop zone already has a letter and it is swapped, translate the letter by this amount on the Y-axis
        // before dropping it:
        private const float LETTER_SWAP_DROP_OFFSET = -1f;

        private IEnumerator throbAnimation;
        private bool isChosen = false;
        public SeparateLetterController droppedLetter;

        [HideInInspector]
        public SeparateLetterController correctLetter;

        public RotateButtonController rotateButtonController;

        public BoxCollider boxCollider;

        void Start()
        {
            THROB_INIT_SCALE = transform.localScale.x;
        }

        void FixedUpdate()
        {
            if (isChosen && chosenDropZone != this)
            {
                isChosen = false;
                Unhighlight();
            }
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;

            Vector3 rotateButtonPosition = transform.position;
            rotateButtonPosition.y += 2.2f;
            rotateButtonPosition.z += 0.5f;
            rotateButtonController.SetPosition(rotateButtonPosition);
        }

        public void Highlight()
        {
            spriteRenderer.color = Color.yellow;
        }

        public void Unhighlight()
        {
            spriteRenderer.color = Color.white;
        }

        public void SetDroppedLetter(SeparateLetterController letter)
        {
            if (letter != null)
            {
                if (droppedLetter != null)
                {
                    Vector3 position = transform.position;
                    position.y += LETTER_SWAP_DROP_OFFSET;
                    droppedLetter.SetPosition(position, false);
                    droppedLetter.SetIsKinematic(false);
                    droppedLetter.droppedZone = null;
                }

                rotateButtonController.Enable();
            }

            else
            {
                rotateButtonController.Disable();
            }

            droppedLetter = letter;
            Unhighlight();
        }

        public void OnTriggerEnter(Collider collider)
        {
            Throb();
            isChosen = true;
            chosenDropZone = this;
            Highlight();
        }

        public void OnTriggerExit(Collider collider)
        {
            if (isChosen)
            {
                isChosen = false;
                chosenDropZone = null;
            }

            Unhighlight();
        }

        public void HideRotationButton()
        {
            rotateButtonController.Disable();
        }

        public void ShowGreenTick()
        {
            TutorialUI.MarkYes(rotateButtonController.transform.position);
        }

        private void Throb()
        {
            if (throbAnimation != null)
            {
                StopCoroutine(throbAnimation);
            }

            throbAnimation = ThrobCoroutine();
            StartCoroutine(throbAnimation);
        }

        private void OnEnable()
        {
            if (THROB_INIT_SCALE > 0f)
            {
                transform.localScale = new Vector3(THROB_INIT_SCALE, THROB_INIT_SCALE, 1);
            }
        }

        private IEnumerator ThrobCoroutine()
        {
            float throbFinalScale = THROB_INIT_SCALE * THROB_SCALE_MULTIPLIER;
            float throbScaleIncrementPerFixedFrame = ((throbFinalScale - THROB_INIT_SCALE) * Time.fixedDeltaTime) / (THROB_PERIOD * 0.5f);

            Vector3 scale = new Vector3(THROB_INIT_SCALE, THROB_INIT_SCALE, 1);

            transform.localScale = scale;

            while (true)
            {
                scale.x += throbScaleIncrementPerFixedFrame;
                scale.y += throbScaleIncrementPerFixedFrame;
                if (scale.x > throbFinalScale)
                {
                    throbScaleIncrementPerFixedFrame *= -1;
                }
                else if (scale.x < THROB_INIT_SCALE)
                {
                    transform.localScale = new Vector3(THROB_INIT_SCALE, THROB_INIT_SCALE, 1);
                    break;
                }
                transform.localScale = scale;
                yield return new WaitForFixedUpdate();
            }
        }

        public void OnRotateLetter()
        {
            if (droppedLetter != null && !droppedLetter.IsRotating())
            {
                droppedLetter.RotateCCW();
                MixedLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.WheelTick);
            }
        }

        public void Reset()
        {
            droppedLetter = null;
            correctLetter = null;

            Unhighlight();
            isChosen = false;

            rotateButtonController.Disable();

            boxCollider.enabled = true;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            rotateButtonController.Disable();
        }

        public void DisableCollider()
        {
            boxCollider.enabled = false;
        }
    }
}
