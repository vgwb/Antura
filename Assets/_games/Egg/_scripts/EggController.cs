using Antura.LivingLetters;
using DG.Tweening;
using System;
using System.Collections.Generic;
using Antura.Language;
using UnityEngine;

namespace Antura.Minigames.Egg
{
    public class EggController : MonoBehaviour
    {
        private List<EggLivingLetter> eggLivingLetters = new List<EggLivingLetter>();
        private GameObject letterObjectViewPrefab;
        private GameObject shadowPrefab;
        private Vector3[] lettersMaxPositions;

        public GameObject emoticonPrefab;
        public EggEmoticonsMaterials eggEmoticonsMaterials;

        public GameObject egg;

        public EggPiece[] eggPieces;

        public EggControllerCollider eggCollider;

        public Transform notRotatedObjects;
        private Vector3 notRotation = new Vector3(0f, 0f, 0f);
        public Transform emoticonsScale;

        public GameObject eggShadow;

        public Action onEggCrackComplete;
        public Action onEggExitComplete;

        public Action onEggPressedCallback
        {
            get { return eggCollider.pressedCallback; }
            set { eggCollider.pressedCallback = value; }
        }

        public GameObject eggParticleWin;
        public GameObject eggParticleCorrect;

        private Sequence particleCorrectSequence;

        private Tween moveEggParentTween;
        private Tween moveEggTeewn;
        private Tween rotationEggTween;

        private Action endTransformToCallback;
        private Action endAudioQuestion;

        private int currentPosition;
        private Vector3 currentRotation;

        private Vector3[] eggPositions;

        private IAudioManager audioManager;
        private IAudioSource audioSource;
        private List<ILivingLetterData> lLDAudioQuestion = new List<ILivingLetterData>();

        private ILivingLetterData questionData;
        private List<ILivingLetterData> answersData = new List<ILivingLetterData>();

        private int piecePoofCompleteCount = 0;
        private bool eggEggCrackCompleteSent = false;

        private EggEmoticonsController emoticonsController;

        public void Initialize(GameObject letterObjectViewPrefab, GameObject shadowPrefab, Vector3[] eggPositions, Vector3[] lettersMaxPositions, IAudioManager audioManager)
        {
            this.letterObjectViewPrefab = letterObjectViewPrefab;
            this.shadowPrefab = shadowPrefab;
            this.lettersMaxPositions = lettersMaxPositions;

            this.eggPositions = eggPositions;
            eggCollider.DisableCollider();

            EggShow(false);
            eggParticleWin.SetActive(false);
            eggParticleCorrect.SetActive(false);

            this.audioManager = audioManager;

            piecePoofCompleteCount = 0;
            eggEggCrackCompleteSent = false;

            InitializeEggPieces();

            currentRotation = new Vector3(0f, 0f, -90f);
            GoToPosition(0, currentRotation);

            emoticonsController = new EggEmoticonsController(emoticonsScale, emoticonPrefab, eggEmoticonsMaterials);
        }

        public void Reset()
        {
            ResetCrack();

            currentRotation = new Vector3(0f, 0f, -90f);
            GoToPosition(0, currentRotation);

            DestroyQuestionLetters();
            EggShow(true);

            eggParticleWin.SetActive(true);
            eggParticleCorrect.SetActive(true);

            ParticleWinDisabled();
            ParticleCorrectDisabled();

            audioSource = null;

            piecePoofCompleteCount = 0;
            eggEggCrackCompleteSent = false;
        }

        public void MoveNext(float duration, Action callback)
        {
            if (moveEggParentTween != null)
            { moveEggParentTween.Kill(); }
            if (rotationEggTween != null)
            { rotationEggTween.Kill(); }

            currentPosition++;

            if (currentPosition >= eggPositions.Length)
            {
                currentPosition = 0;
            }

            currentRotation.z += 90f;

            bool inOutRotation = currentPosition == 1 || currentPosition == eggPositions.Length - 1;

            TransformTo(eggPositions[currentPosition], inOutRotation, currentRotation, duration, callback);
        }

        public bool isNextToExit
        {
            get
            {
                if (currentPosition == eggPositions.Length - 2)
                    return true;

                return false;
            }
        }

        public void InitializeEggPieces()
        {
            for (int i = 0; i < eggPieces.Length; i++)
            {
                eggPieces[i].onPoofEnd = OnPiecePoofComplete;
            }
        }

        public void ResetCrack()
        {
            for (int i = 0; i < eggPieces.Length; i++)
            {
                eggPieces[i].Reset();
            }
        }

        public void Cracking(float progress)
        {
            if (progress == 1f)
            {
                for (int i = 0; i < eggPieces.Length; i++)
                {
                    bool poofDirRight = (i % 2 == 0);

                    eggPieces[i].Poof(poofDirRight);
                }

                ShowEndLetters();
            }
            else
            {
                for (int i = 0; i < eggPieces.Length; i++)
                {
                    eggPieces[i].Shake();
                }
            }
        }

        void MoveTo(Vector3 position, float duration)
        {
            if (moveEggParentTween != null)
            {
                moveEggParentTween.Kill();
            }

            moveEggParentTween = transform.DOLocalMove(position, duration).OnComplete(delegate ()
            {
                if (endTransformToCallback != null)
                    endTransformToCallback();

                if (onEggExitComplete != null && (currentPosition == eggPositions.Length - 1))
                {
                    onEggExitComplete();
                }

            });
        }

        void InOutRotation(Vector3 rotation, float duration)
        {
            if (rotationEggTween != null)
            {
                rotationEggTween.Kill();
            }

            rotationEggTween = DOTween.To(() => egg.transform.eulerAngles.z, z => egg.transform.eulerAngles = new Vector3(egg.transform.eulerAngles.x, egg.transform.eulerAngles.y, z), rotation.z + 720f, duration * 0.95f)
                .OnComplete(delegate ()
                {
                    BouncingRotation(0.5f);
                });
        }

        void RoteteTo(Vector3 rotation, float duration)
        {
            if (rotationEggTween != null)
            {
                rotationEggTween.Kill();
            }

            rotationEggTween = egg.transform.DORotate(rotation, duration * 0.93f).OnComplete(delegate ()
            {
                BouncingRotation();
            });
        }

        void BouncingRotation(float duration = 0.8f)
        {
            float firstStepValue = 5f;
            float secondStepValue = -2.5f;

            Vector3 rotationFirstStep = Vector3.zero;
            rotationFirstStep.z += firstStepValue;
            Vector3 rotationSecondStep = Vector3.zero;
            rotationSecondStep.z += secondStepValue;

            rotationEggTween = transform.DORotate(rotationFirstStep, (duration / 10f) * 5f).OnComplete(delegate ()
            {
                rotationEggTween = transform.DORotate(rotationSecondStep, (duration / 10f) * 4f).OnComplete(delegate ()
                {
                    rotationEggTween = transform.DORotate(Vector3.zero, (duration / 10f) * 2f);
                });
            });
        }

        void TransformTo(Vector3 localPosition, bool inOutRotation, Vector3 rotation, float duration, Action callback)
        {
            MoveTo(localPosition, duration);

            if (inOutRotation)
            {
                InOutRotation(rotation, duration);
            }
            else
            {
                RoteteTo(rotation, duration);
            }

            endTransformToCallback = callback;
        }

        void GoToPosition(int positionNumber, Vector3 rotation)
        {
            if (moveEggParentTween != null)
            { moveEggParentTween.Kill(); }
            if (rotationEggTween != null)
            { rotationEggTween.Kill(); }

            currentPosition = positionNumber;

            transform.localPosition = eggPositions[currentPosition];
            egg.transform.eulerAngles = rotation;
            transform.eulerAngles = Vector3.zero;
        }

        public void EnableInput()
        {
            eggCollider.EnableCollider();
        }

        public void DisableInput()
        {
            eggCollider.DisableCollider();
        }

        void Update()
        {
            if (lLDAudioQuestion.Count > 0)
            {
                if (audioSource != null)
                {
                    if (audioSource.IsLoaded && !audioSource.IsPlaying)
                    {
                        audioSource = null;
                    }
                }
                else
                {
                    ILivingLetterData letterData = lLDAudioQuestion[0];

                    audioSource = audioManager.PlayVocabularyData(letterData, false, EggConfiguration.Instance.GetVocabularySoundType());

                    lLDAudioQuestion.RemoveAt(0);
                }
            }
            else
            {
                if (audioSource != null)
                {
                    if (audioSource.IsLoaded && !audioSource.IsPlaying)
                    {
                        audioSource = null;

                        if (endAudioQuestion != null)
                        {
                            endAudioQuestion();
                        }
                    }
                }
            }

            if (!eggEggCrackCompleteSent)
            {
                if (piecePoofCompleteCount >= eggPieces.Length)
                {
                    eggEggCrackCompleteSent = true;

                    EggShow(false);

                    if (onEggCrackComplete != null)
                    {
                        onEggCrackComplete();
                    }
                }
            }

            if (emoticonsController != null)
            {
                emoticonsController.Update(Time.deltaTime);
            }

            for (int i = 0; i < eggLivingLetters.Count; i++)
            {
                eggLivingLetters[i].Update(Time.deltaTime);
            }
        }

        public void LateUpdate()
        {
            float minY = 2.5f;
            float maxY = 4.1f;

            float maxDelta = maxY - minY;

            float zRotation = egg.transform.eulerAngles.z % 360f;

            float newYPosition = minY;

            if (zRotation <= 180)
            {
                if (zRotation < 90f)
                {
                    zRotation = 0f;
                }
                else
                {
                    zRotation += -90f;
                }
            }
            else
            {
                zRotation += -180f;

                if (zRotation <= 90f)
                {
                    zRotation = 90f - zRotation;
                }
                else
                {
                    zRotation = 0f;
                }
            }

            newYPosition += maxDelta * (zRotation / 90f);

            Vector3 newPosition = egg.transform.localPosition;
            newPosition.y = newYPosition;
            egg.transform.localPosition = newPosition;
            notRotatedObjects.eulerAngles = notRotation;
        }


        public void SetQuestion(ILivingLetterData _questionData)
        {
            questionData = _questionData;
        }

        public void SetAnswers(ILivingLetterData _answersData)
        {
            answersData.Clear();
            answersData.Add(_answersData);
        }

        public void SetAnswers(IEnumerable<ILivingLetterData> _answersData)
        {
            answersData.Clear();
            foreach (ILivingLetterData answerData in _answersData)
            {
                answersData.Add(answerData);
            }
        }

        public void PlayAudioQuestion(Action endCallback)
        {
            audioSource = null;

            endAudioQuestion = endCallback;

            lLDAudioQuestion.Clear();

            for (int i = 0; i < answersData.Count; i++)
            {
                lLDAudioQuestion.Add(answersData[i]);
            }
        }

        public void ParticleWinEnabled()
        {
            foreach (var particles in eggParticleWin.GetComponentsInChildren<ParticleSystem>(true))
            {
                particles.Play();
            }
        }

        public void ParticleWinDisabled()
        {
            foreach (var particles in eggParticleWin.GetComponentsInChildren<ParticleSystem>(true))
            {
                particles.Stop();
            }
        }

        public void ParticleCorrectEnabled()
        {
            if (particleCorrectSequence != null && particleCorrectSequence.IsPlaying())
                return;

            foreach (var particles in eggParticleCorrect.GetComponentsInChildren<ParticleSystem>(true))
            {
                particles.Play();
            }

            particleCorrectSequence = DOTween.Sequence();

            particleCorrectSequence.AppendInterval(0.5f);
            particleCorrectSequence.AppendCallback(delegate ()
            { ParticleCorrectDisabled(); });

            particleCorrectSequence.Play();
        }

        public void ParticleCorrectDisabled()
        {
            if (particleCorrectSequence != null)
                particleCorrectSequence.Kill();

            foreach (var particles in eggParticleCorrect.GetComponentsInChildren<ParticleSystem>(true))
            {
                particles.Stop();
            }
        }

        public void StartShake()
        {
            for (int i = 0; i < eggPieces.Length; i++)
            {
                eggPieces[i].Shake();
            }
        }

        void OnPiecePoofComplete()
        {
            piecePoofCompleteCount++;
        }

        void EggShow(bool show)
        {
            eggShadow.SetActive(show);
            egg.SetActive(show);
        }

        void ShowEndLetters()
        {
            EggLivingLetter letter;
            float startDelay = 0f;
            float jumpDelay = 0.5f;

            Vector3[] lettersEndPositions = GetLettersEndPositions();

            Vector3 lLetterPosition = new Vector3(transform.localPosition.x, egg.transform.localPosition.y, transform.localPosition.z);
            for (int i = 0; i < answersData.Count; i++)
            {
                letter = new EggLivingLetter(transform.parent, letterObjectViewPrefab, shadowPrefab, answersData[i], lLetterPosition, transform.localPosition, lettersEndPositions[i], (jumpDelay * i) + startDelay, null);

                eggLivingLetters.Add(letter);
            }

            if (questionData != null)
            {
                Vector3 finalLivingLetterEndPosition = new Vector3(0, -1f, -17);
                var questionImageData = new LL_ImageData(questionData.Id);
                var finalLivingLetter = new EggLivingLetter(transform.parent, letterObjectViewPrefab, shadowPrefab,
                    questionImageData, lLetterPosition, transform.localPosition, finalLivingLetterEndPosition, (jumpDelay * answersData.Count) + startDelay, null);

                eggLivingLetters.Add(finalLivingLetter);
            }
        }

        void DestroyQuestionLetters()
        {
            for (int i = 0; i < eggLivingLetters.Count; i++)
            {
                eggLivingLetters[i].DestroyLetter();
            }

            eggLivingLetters.Clear();
        }

        Vector3[] GetLettersEndPositions()
        {
            int questionDataCount = answersData.Count;

            Vector3[] lettersEndPositions = new Vector3[questionDataCount];

            Vector3 maxLeft = lettersMaxPositions[0];
            Vector3 maxRight = lettersMaxPositions[1];

            float positionLerp = 1f / (questionDataCount + 1);

            for (int i = 0; i < questionDataCount; i++)
            {
                float l = 1f - (positionLerp * (i + 1));
                if (!LanguageSwitcher.LearningRTL)
                    l = 1 - l;

                lettersEndPositions[i] = Vector3.Lerp(maxLeft, maxRight, l);
            }

            return lettersEndPositions;
        }

        public void EmoticonHappy()
        {
            emoticonsController.EmoticonHappy();
        }

        public void EmoticonPositive()
        {
            emoticonsController.EmoticonPositive();
        }

        public void EmoticonNegative()
        {
            emoticonsController.EmoticonNegative();
        }

        public void EmoticonInterrogative()
        {
            emoticonsController.EmoticonInterrogative();
        }

        public void EmoticonClose()
        {
            emoticonsController.CloseEmoticons();
        }
    }
}
