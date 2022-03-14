using UnityEngine;
using System.Collections;
using System.Linq;
using Antura.LivingLetters;

namespace Antura.Minigames.Balloons
{
    public class FloatingLetterController : MonoBehaviour
    {
        public Rigidbody body;
        public BalloonVariation[] variations;

        [Header("Floating Letter Parameters")]
        [Range(-1, 2)]
        [Tooltip("Index of default variation (-1 for none)")]
        public int defaultActiveVariation;
        [Range(0, 10)]
        [Tooltip("e.g.: 1")]
        public float floatSpeed;
        [Range(0, 10)]
        [Tooltip("e.g.: 1.5")]
        public float floatDistance;
        [Range(0, 1)]
        [Tooltip("e.g.: 0.25")]
        public float floatRandomnessFactor;
        [Range(0, 10)]
        [Tooltip("e.g.: 2.5")]
        public float distanceRandomnessMargin;
        [Range(0, 10)]
        [Tooltip("e.g.: 1")]
        public float waftSpeed;
        [Range(0, 1)]
        [Tooltip("e.g.: 0.75")]
        public float waftRandomnessFactor;
        [Range(-10, 10)]
        [Tooltip("e.g.: 2")]
        public float stageMargin;
        [Range(0, 500)]
        [Tooltip("e.g.: 50")]
        public float initialSwingPush;
        [Range(0, 1)]
        [Tooltip("e.g.: 0.5")]
        public float initialSwingRandomnessFactor;
        [Range(0, 10)]
        [Tooltip("e.g.: 2")]
        public float stageEntranceDuration;
        [Range(0, 10)]
        [Tooltip("e.g.: 2")]
        public float dragSpeed;
        [Range(1, 10)]
        [Tooltip("e.g.: 1")]
        public int tapsNeeded;
        [Range(0, 100f)]
        [Tooltip("e.g.: 9.5")]
        public float shockwaveRadius;
        [Range(0, 100f)]
        [Tooltip("e.g.: 1")]
        public float shockwavePower;
        [Range(0, -30f)]
        [Tooltip("e.g.: -15")]
        public float focusZ;

        [HideInInspector]
        public BalloonsLetterController Letter
        {
            get { return ActiveVariation.letter; }
        }

        [HideInInspector]
        public BalloonController[] Balloons
        {
            get { return ActiveVariation.balloons; }
        }

        private BalloonVariation _activeVariation;

        [HideInInspector]
        public BalloonVariation ActiveVariation
        {
            get
            { return _activeVariation; }
            private set
            {
                _activeVariation = value;
                activeBalloonCount = Balloons.Length;
            }
        }

        private Vector3 _mouseOffset;

        [HideInInspector]
        public Vector3 MouseOffset
        {
            get { return _mouseOffset; }
            set { _mouseOffset.Set(value.x, value.y, 0f); }
        }

        [HideInInspector]
        public int activeBalloonCount;

        private int floatDirection = 1;
        private int waftDirection = 1;
        private float randomOffset = 0f;
        private Vector3 basePosition;
        private Vector3 clampedPosition = new Vector3();
        private Vector3 floatVelocity = new Vector3();
        private Vector3 waftVelocity = new Vector3();
        private Vector3 focusedPosition = new Vector3();
        private Vector3 unfocusedPosition = new Vector3();
        private bool keepUnfocusing = true;
        private float focusDuration = 1f;
        private float focusProgress;
        private float focusProgressPercentage;


        void Awake()
        {
            SetActiveVariation(defaultActiveVariation);
        }

        void Start()
        {
            basePosition = transform.position;
            RandomizePosition();
            RandomizeFloating();
            RandomizeWafting();
            RandomizeSwing();
            EnterStage();
            waftVelocity.x = waftSpeed;
        }

        void Update()
        {
            Float();
            Waft();
            Unfocus();
        }

        public void SetActiveVariation(int index)
        {
            if (index < 0)
            {
                return;
            }

            for (int i = 0; i < variations.Length; i++)
            {
                variations[i].gameObject.SetActive(false);
            }
            ActiveVariation = variations[index];
            ActiveVariation.gameObject.SetActive(true);
        }

        void RandomizePosition()
        {
            basePosition = basePosition + Random.Range(-distanceRandomnessMargin, distanceRandomnessMargin) * Vector3.up;
        }

        void RandomizeFloating()
        {
            randomOffset = Random.Range(0, 2 * Mathf.PI);
            floatSpeed += Random.Range(-floatRandomnessFactor * floatSpeed, floatRandomnessFactor * floatSpeed);
            floatDistance += Random.Range(-floatRandomnessFactor * floatDistance, floatRandomnessFactor * floatDistance);
            floatDirection *= (Random.Range(0, 2) > 0 ? -1 : 1);
        }

        void RandomizeWafting()
        {
            waftSpeed += waftSpeed * Random.Range(-waftRandomnessFactor, waftRandomnessFactor);
            waftDirection *= (Random.Range(0, 2) > 0 ? -1 : 1);
        }

        void RandomizeSwing()
        {
            var direction = waftDirection *= (Random.Range(0, 2) > 0 ? -1 : 1);
            initialSwingPush += initialSwingPush * Random.Range(-initialSwingRandomnessFactor, initialSwingRandomnessFactor);
            Vector3 force = direction * initialSwingPush * Vector3.right;
            Letter.body.AddForce(force);
        }

        void EnterStage()
        {
            if (basePosition.x > 0)
            {
                transform.position = new Vector3(BalloonsGame.instance.maxX + stageMargin, basePosition.y, basePosition.z);
            }
            else
            {
                transform.position = new Vector3(BalloonsGame.instance.minX - stageMargin, basePosition.y, basePosition.z);
            }

            StartCoroutine(EnterStage_Coroutine());
        }

        private IEnumerator EnterStage_Coroutine()
        {
            float progress = 0f;
            float percentage = 0f;

            var initialPosition = transform.position;
            var finalPosition = basePosition;
            var lerpedPosition = new Vector3();

            while (progress < stageEntranceDuration)
            {
                lerpedPosition.Set(Vector3.Lerp(initialPosition, finalPosition, percentage).x, transform.position.y, transform.position.z);
                transform.position = lerpedPosition;
                progress += Time.deltaTime;
                percentage = progress / stageEntranceDuration;
                percentage = Mathf.Sin(percentage * Mathf.PI * 0.5f);

                yield return null;
            }
        }

        void Float()
        {
            // Float using Rigidbody velocity
            //floatVelocity = floatDirection * floatDistance * Mathf.Sin(floatSpeed * Time.time + randomOffset) * Vector3.up;
            floatVelocity.Set(body.velocity.x, floatDirection * floatDistance * Mathf.Sin(floatSpeed * Time.time + randomOffset), body.velocity.z);
            body.velocity = floatVelocity;

            // Float using Transform position
            //transform.position = basePosition + floatDirection * floatDistance * Mathf.Sin(floatSpeed * Time.time + randomOffset) * Vector3.up;
        }

        public void Waft()
        {
            waftVelocity.Set(waftDirection * waftSpeed, body.velocity.y, body.velocity.z);

            body.velocity = waftVelocity;
            if (body.transform.position.x > BalloonsGame.instance.maxX - stageMargin)
            {
                waftDirection = -1;
                //body.transform.position = new Vector3(BalloonsGameManager.instance.minX - waftWrappingMargin, body.transform.position.y, body.transform.position.z);
            }
            else if (body.transform.position.x < BalloonsGame.instance.minX + stageMargin)
            {
                waftDirection = 1;
            }
        }

        public void Drag(Vector3 dragPosition)
        {
            dragPosition.z = transform.position.z;

            // Drag using Transform Position
            transform.position = ClampPositionToStage(dragPosition + MouseOffset);

            // Drag using Rigidbody Position
            //body.MovePosition(ClampPositionToStage(dragPosition + MouseOffset));
        }

        public void Focus()
        {
            keepUnfocusing = false;
            focusedPosition.Set(transform.position.x, transform.position.y, transform.position.z + focusZ);

            if (focusProgress < focusDuration)
            {
                focusProgress += Time.deltaTime;
                focusProgressPercentage = focusProgress / focusDuration;
            }

            transform.position = Vector3.Lerp(transform.position, focusedPosition, focusProgressPercentage); //transform.position = focusedPosition;
        }

        public void Unfocus()
        {
            if (keepUnfocusing)
            {
                if (!Mathf.Approximately(transform.position.z, basePosition.z))
                {
                    unfocusedPosition.Set(transform.position.x, transform.position.y, basePosition.z);
                    transform.position = unfocusedPosition;
                }
            }
            else
            {
                keepUnfocusing = true;
            }
        }

        public void ResetFocusingParameters()
        {
            focusProgress = 0f;
            focusProgressPercentage = 0f;
        }

        public void Pop()
        {
            activeBalloonCount--;
            CreateShockwave();

            if (Letter.isRequired)
            {
                BalloonsGame.instance.OnPoppedRequiredBalloon(Letter.associatedPromptIndex);
                if (BalloonsConfiguration.Instance.Variation == BalloonsVariation.LetterInWord)
                {
                    Letter.FlashLetterInWord(((LL_LetterData)BalloonsGame.instance.question).Data, Color.red);
                }
            }
            else if (BalloonsConfiguration.Instance.Variation != BalloonsVariation.Counting)
            {
                BalloonsGame.instance.OnPoppedNonRequiredBalloon();
            }
            else
            {
                if (Letter.letterData.Id != BalloonsGame.instance.correctAnswers.ToList()[BalloonsGame.instance.countingIndex].Id)
                {
                    BalloonsGame.instance.OnPoppedRequiredBalloon(-1);
                }
                else
                {
                    BalloonsGame.instance.OnPoppedNonRequiredBalloon();
                }
            }

            if (Balloons.Length == 3)
            {
                if (Balloons[0] != null && Balloons[0].balloonCollider.enabled == true)
                {
                    Balloons[0].AdjustMiddleBalloon();
                }
            }

            if (activeBalloonCount <= 0)
            {
                Letter.transform.SetParent(null);
                Letter.Drop();
                BalloonsGame.instance.floatingLetters.Remove(this);
                BalloonsGame.instance.OnDroppedLetter(Letter);
                Destroy(gameObject, 3f);
            }
        }

        private void CreateShockwave()
        {
            if (BalloonsGame.instance.isTutorialRound)
            {
                return;
            }

            Vector3 shockwavePosition = transform.position;
            var affectedObjects = BalloonsGame.instance.floatingLetters.FindAll(floatingLetter => floatingLetter.transform.position.x > transform.position.x - shockwaveRadius && floatingLetter.transform.position.x < transform.position.x + shockwaveRadius);

            for (int i = 0; i < affectedObjects.Count; i++)
            {
                var hit = affectedObjects[i];

                if (hit != null && hit.transform != null)
                {
                    var distance = (hit.transform.position - shockwavePosition).magnitude;
                    if (hit == this)
                    {
                        continue;
                    }

                    var direction = (hit.transform.position - shockwavePosition).normalized;
                    var displacement = (direction * shockwavePower) * (shockwaveRadius - distance);
                    displacement.z = 0f;

                    Knockback(hit.transform, displacement);
                }
            }
        }

        private void Knockback(Transform hitTransform, Vector3 displacement)
        {
            StartCoroutine(Knockback_Coroutine(hitTransform, displacement));
        }

        private IEnumerator Knockback_Coroutine(Transform hitTransform, Vector3 displacement)
        {
            float duration = 0.5f;
            float progress = 0f;
            float percentage = 0f;

            var initialPosition = hitTransform.position;
            var finalPosition = hitTransform.position + displacement;

            while (progress < duration && hitTransform != null && hitTransform.transform != null)
            {
                hitTransform.position = ClampPositionToStage(Vector3.Lerp(initialPosition, finalPosition, percentage));
                progress += Time.deltaTime;
                percentage = progress / duration;
                percentage = Mathf.Sin(percentage * Mathf.PI * 0.5f);

                yield return null;
            }
        }

        public Vector3 ClampPositionToStage(Vector3 unclampedPosition)
        {
            clampedPosition = unclampedPosition;

            var minX = BalloonsGame.instance.minX;
            var maxX = BalloonsGame.instance.maxX;
            var minY = BalloonsGame.instance.minY;
            var maxY = BalloonsGame.instance.maxY;

            clampedPosition.x = clampedPosition.x < minX ? minX : clampedPosition.x;
            clampedPosition.x = clampedPosition.x > maxX ? maxX : clampedPosition.x;
            clampedPosition.y = clampedPosition.y < minY ? minY : clampedPosition.y;
            clampedPosition.y = clampedPosition.y > maxY ? maxY : clampedPosition.y;

            return clampedPosition;
        }

        public void Disable()
        {
            for (int i = 0; i < Balloons.Length; i++)
            {
                Balloons[i].DisableCollider();
            }
            Letter.DisableCollider();
        }

        public void Enable()
        {
            for (int i = 0; i < Balloons.Length; i++)
            {
                Balloons[i].EnableCollider();
            }
            Letter.EnableCollider();
        }

        public void Freeze()
        {
            floatDistance = 0f;
            floatSpeed = 0f;
            floatRandomnessFactor = 0f;
            waftSpeed = 0f;
            waftRandomnessFactor = 0f;
            Letter.keepFocusingLetter = true;
            Letter.DisableCollider();
        }
    }
}
