using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using Antura.LivingLetters;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Antura.Language;

namespace Antura.Minigames.Balloons
{
    public class BalloonsLetterController : MonoBehaviour
    {
        private const float FLASH_CYCLE_DURATION = 0.2f;
        private const int NUM_FLASH_CYCLES = 3;

        public LivingLetterController LLPrefab;
        public FloatingLetterController parentFloatingLetter;
        public Animator animator;
        public Collider letterCollider;
        public Rigidbody body;
        public int associatedPromptIndex;
        public bool isRequired;
        public ILivingLetterData letterData;
        public TMP_Text LetterView;

        [Header("Letter Parameters")]
        [Tooltip("e.g: true")]
        public bool spinEnabled;
        [Range(0, 5)]
        [Tooltip("e.g.: 1")]
        public float spinSpeed;
        [Range(0, 360)]
        [Tooltip("e.g.: 90")]
        public float spinAngle;
        [Range(0, 5)]
        [Tooltip("e.g.: 0.25")]
        public float spinRandomnessFactor;

        [HideInInspector]
        public bool keepFocusingLetter = false;

        private bool keepSpinning;
        private float spinDirection = 1f;
        private float randomOffset = 0f;
        private Vector3 baseRotation;
        private Vector3 mousePosition = new Vector3();
        private float cameraDistance;
        private bool drop;
        private float focusDuration = 1f;
        private float focusProgress;
        private float focusProgressPercentage;
        private float unfocusDuration = 1f;
        private float unfocusProgress;
        private float unfocusProgressPercentage;

        private LivingLetterController letterObjectView;
        private IEnumerator flashLetterInWordCoroutine;
        private List<SpringJoint> springJoints;

        private void Awake()
        {
            letterObjectView = GetComponent<LivingLetterController>();

            springJoints = new List<SpringJoint>();

            foreach (SpringJoint springJoint in GetComponents<SpringJoint>())
            {
                springJoints.Add(springJoint);
            }
        }

        public void Start()
        {
            cameraDistance = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
            baseRotation = transform.rotation.eulerAngles;
            keepSpinning = spinEnabled;
            LLPrefab.SetState(LLAnimationStates.LL_hanging);
            RandomizeSpin();
            //RandomizeAnimation();
        }

        void Update()
        {
            Spin();

            if (keepFocusingLetter)
            {
                FocusLetter();
            }

            if (transform.position.y < -10)
            {
                Destroy(gameObject);
            }
        }

        public void Init(ILivingLetterData _data)
        {
            letterData = _data;
            LLPrefab.Init(_data);
            ConfigureJointAnchors(_data);
        }

        private void ConfigureJointAnchors(ILivingLetterData _data)
        {
            Vector3 anchor;

            switch (_data.DataType)
            {
                case LivingLetterDataType.Letter:
                    anchor = new Vector3(0f, 5.5f, -0.2f);
                    break;
                case LivingLetterDataType.Word:
                    anchor = new Vector3(0.85f, 6.09f, -0.2f);
                    break;
                case LivingLetterDataType.Image:
                    anchor = new Vector3(0f, 5.5f, -0.2f);
                    break;
                default:
                    anchor = new Vector3(0f, 5.5f, -0.2f);
                    break;
            }

            foreach (SpringJoint springJoint in springJoints)
            {
                springJoint.anchor = anchor;
            }
        }

        void OnMouseDown()
        {
            SpeakLetter();
            FocusLetter();

            mousePosition = Input.mousePosition;
            mousePosition.z = cameraDistance;

            parentFloatingLetter.MouseOffset = parentFloatingLetter.transform.position - Camera.main.ScreenToWorldPoint(mousePosition);
        }

        void OnMouseDrag()
        {
            FocusLetter();

            mousePosition = Input.mousePosition;
            mousePosition.z = cameraDistance;

            parentFloatingLetter.Drag(Camera.main.ScreenToWorldPoint(mousePosition));
        }

        void OnMouseUp()
        {
            ResetLetterFocusingParameters();
            ResetLetterUnfocusingParameters();
            parentFloatingLetter.ResetFocusingParameters();
        }

        private void RandomizeSpin()
        {
            randomOffset = Random.Range(0, 2 * Mathf.PI);
            spinSpeed += Random.Range(-spinRandomnessFactor * spinSpeed, spinRandomnessFactor * spinSpeed);
            spinAngle += Random.Range(-spinRandomnessFactor * spinAngle, spinRandomnessFactor * spinAngle);
            spinDirection *= (Random.Range(0, 2) > 0 ? -1 : 1);
        }

        private void RandomizeAnimation()
        {
            animator.speed *= Random.Range(0.75f, 1.25f);
            animator.SetFloat("Offset", Random.Range(0f, BalloonsGame.instance.letterAnimationLength));
        }

        private void SpeakLetter()
        {
            if (letterData != null && letterData.Id != null)
            {
                BalloonsConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(letterData, soundType: BalloonsConfiguration.Instance.GetVocabularySoundType());
            }
        }

        private void FocusLetter()
        {
            keepSpinning = false;
            //transform.rotation = Quaternion.Euler(baseRotation);
            parentFloatingLetter.Focus();

            if (focusProgress < focusDuration)
            {
                focusProgress += Time.deltaTime;
                focusProgressPercentage = focusProgress / focusDuration;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(baseRotation), focusProgressPercentage);
        }

        public void ResetLetterFocusingParameters()
        {
            focusProgress = 0f;
            focusProgressPercentage = 0f;
        }

        public void ResetLetterUnfocusingParameters()
        {
            unfocusProgress = 0f;
            unfocusProgressPercentage = 0f;
        }

        private void Spin()
        {
            if (keepSpinning)
            {
                if (unfocusProgress < unfocusDuration)
                {
                    unfocusProgress += Time.deltaTime;
                    unfocusProgressPercentage = unfocusProgress / unfocusDuration;
                }
                var spinRotation = Quaternion.Euler(baseRotation.x, baseRotation.y + spinDirection * spinAngle * Mathf.Sin(spinSpeed * Time.time + randomOffset), baseRotation.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, spinRotation, unfocusProgressPercentage);
            }
            else
            {
                keepSpinning = spinEnabled;
            }
        }

        public void Drop()
        {
            StartCoroutine(Drop_Coroutine(BalloonsGame.instance.letterDropDelay));
        }

        private IEnumerator Drop_Coroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            BalloonsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.LetterAngry);
            //drop = true;
            var dropSpeed = 2500f;
            body.AddForce(Vector3.down * dropSpeed);
        }

        public void DisableCollider()
        {
            letterCollider.enabled = false;
        }

        public void EnableCollider()
        {
            letterCollider.enabled = true;
        }

        public void FlashLetterInWord(LetterData letterToFlash, Color color)
        {
            if (flashLetterInWordCoroutine != null)
            {
                StopCoroutine(flashLetterInWordCoroutine);
            }

            flashLetterInWordCoroutine = FlashLetterInWordCoroutine(letterToFlash, color);
            StartCoroutine(flashLetterInWordCoroutine);
        }

        private IEnumerator FlashLetterInWordCoroutine(LetterData letterToFlash, Color color)
        {
            if (letterData is LL_WordData)
            {
                var splitLetters = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).SplitWord(AppManager.I.DB, ((LL_WordData)letterData).Data);

                int charPosition = 0;
                List<int> foundLetterIndices = new List<int>();

                for (int index = 0; index < splitLetters.Count; ++index)
                {
                    if (splitLetters[index].letter.Id == letterToFlash.Id)
                    {
                        foundLetterIndices.Add(charPosition);
                    }

                    charPosition += splitLetters[index].letter.GetStringForDisplay().Length;
                }

                if (foundLetterIndices.Count != 0)
                {
                    string originalText = ((LL_WordData)letterData).TextForLivingLetter;

                    letterObjectView.LabelRender.SetText(originalText);

                    float timeElapsed = 0f;
                    int numCompletedCycles = 0;

                    float halfDuration = FLASH_CYCLE_DURATION * 0.5f;

                    string preparedText = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessString(originalText);
                    preparedText = originalText;

                    while (numCompletedCycles < NUM_FLASH_CYCLES)
                    {
                        float interpolant = timeElapsed < halfDuration
                            ? timeElapsed / halfDuration
                            : 1 - ((timeElapsed - halfDuration) / halfDuration);
                        string tagStart = "<color=#" + GenericHelper.ColorToHex(Color.Lerp(Color.black, color, interpolant)) + ">";
                        string tagEnd = "</color>";

                        string composedString = "";

                        for (int i = 0; i < foundLetterIndices.Count; i++)
                        {
                            int startIdx = i == 0 ? 0 : foundLetterIndices[i - 1] + letterToFlash.GetStringForDisplay().Length;
                            int endIdx = foundLetterIndices[i] - 1;

                            composedString += preparedText.Substring(startIdx, endIdx - startIdx + 1);

                            composedString += tagStart;
                            composedString += preparedText.Substring(foundLetterIndices[i], letterToFlash.GetStringForDisplay().Length);
                            composedString += tagEnd;
                        }

                        composedString += preparedText.Substring(foundLetterIndices[foundLetterIndices.Count - 1] + letterToFlash.GetStringForDisplay().Length);

                        letterObjectView.LabelRender.SetText(composedString);

                        timeElapsed += Time.fixedDeltaTime;
                        if (timeElapsed >= FLASH_CYCLE_DURATION)
                        {
                            numCompletedCycles++;
                            timeElapsed = 0f;
                        }

                        yield return new WaitForFixedUpdate();
                    }

                    letterObjectView.LabelRender.SetText(originalText);
                }
            }

            flashLetterInWordCoroutine = null;
        }

        private Transform FindDescendant(Transform parent, string name)
        {
            if (parent.name.Equals(name))
            { return parent; }

            foreach (Transform child in parent)
            {
                Transform result = FindDescendant(child, name);
                if (result != null)
                { return result; }
            }

            return null;
        }
    }
}
