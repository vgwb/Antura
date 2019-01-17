using Antura.Audio;
using Antura.Core;
using Antura.Helpers;
using Antura.LivingLetters;
using Antura.Tutorial;
using Antura.UI;
using Random = UnityEngine.Random;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Antura.Minigames.ThrowBalls
{
    public class GameState : FSM.IState
    {
        public const int MAX_NUM_ROUNDS = 5;
        public const int NUM_LETTERS_IN_POOL = 7;
        public readonly int MAX_NUM_BALLS;
        public const float TUTORIAL_UI_PERIOD = 4;
        private const float FLASHING_TEXT_CYCLE_DURATION = 1f;

        private const float SHOW_BALL_START_DELAY = 0.33f;
        private const float SHOW_BALL_END_DELAY = 0.67f;

        public bool isRoundOngoing;

        // Round number is 1-based. (Round 1, round 2,...)
        // Round 0 is the tutorial round.
        private int roundNumber = 0;
        private int numBalls;

        private int numRoundsWon = 0;

        private float timeLeftToShowTutorialUI = TUTORIAL_UI_PERIOD;
        private bool isIdle = true;

        private ILivingLetterData question;
        private List<ILivingLetterData> currentLettersForLettersInWord;
        private int numLettersRemaining;

        private LetterSpawner letterSpawner;
        public GameObject[] letterPool;
        private LetterController[] letterControllers;

        private ThrowBallsGame game;

        public static GameState instance;

        private bool isVoiceOverDone = false;
        private IAudioManager audioManager;
        private IInputManager inputManager;

        private GameObject tutorialTarget;

        private IEnumerator flashingTextCoroutine;

        private List<LL_LetterData> flashedLettersInLiWVariation;

        private int NumLettersInCurrentRound
        {
            get {
                if (ThrowBallsConfiguration.Instance.Variation == ThrowBallsVariation.BuildWord) {
                    return currentLettersForLettersInWord.Count;
                } else {
                    return 3;
                }
            }
        }

        public GameState(ThrowBallsGame game)
        {
            this.game = game;

            instance = this;

            inputManager = ThrowBallsConfiguration.Instance.Context.GetInputManager();
            audioManager = game.Context.GetAudioManager();

            inputManager.Enabled = false;

            currentLettersForLettersInWord = new List<ILivingLetterData>();

            // Configure num balls:
            if (ThrowBallsConfiguration.Instance.Variation == ThrowBallsVariation.BuildWord) {
                MAX_NUM_BALLS = 10;
            } else {
                var difficulty = game.Difficulty;

                if (difficulty <= ThrowBallsGame.ThrowBallsDifficulty.Normal) {
                    MAX_NUM_BALLS = 5;
                } else if (difficulty == ThrowBallsGame.ThrowBallsDifficulty.Hard) {
                    MAX_NUM_BALLS = 4;
                } else {
                    MAX_NUM_BALLS = 3;
                }
            }

            flashedLettersInLiWVariation = new List<LL_LetterData>();
        }
        public void EnterState()
        {
            Random.InitState(DateTime.Now.GetHashCode());

            // Layer 8 = Terrain. Layer 12 = Ball.
            Physics.IgnoreLayerCollision(8, 10);

            // Layer 16 = Slingshot; Layer 10 = Player (Antura).
            Physics.IgnoreLayerCollision(16, 10);

            // Layer 16 = Slingshot; Layer 12 = Ball.
            Physics.IgnoreLayerCollision(16, 12);

            letterSpawner = new LetterSpawner();

            foreach (Collider collider in ThrowBallsGame.instance.environment.GetComponentsInChildren<Collider>()) {
                collider.enabled = false;
            }

            letterPool = new GameObject[NUM_LETTERS_IN_POOL];
            letterControllers = new LetterController[NUM_LETTERS_IN_POOL];

            for (int i = 0; i < letterPool.Length; i++) {
                GameObject letter = ThrowBallsGame.Instantiate(game.letterWithPropsPrefab).GetComponent<LetterWithPropsController>().letter;
                LetterController letterController = letter.GetComponent<LetterController>();

                letterPool[i] = letter;
                letterControllers[i] = letterController;

                letter.SetActive(false);
            }

            ThrowBallsGame.instance.letterWithPropsPrefab.SetActive(false);

            audioManager.PlayDialogue(ThrowBallsConfiguration.Instance.TitleLocalizationId, OnTitleVoiceOverDone);

            AudioManager.I.PlayMusic(Music.Theme10);

            BallController.instance.Reset();
            Catapult.instance.DisableCollider();
            BallController.instance.Disable();
            SlingshotController.instance.Disable();
            AnturaController.instance.Disable();
            ArrowHeadController.instance.Disable();
            ArrowBodyController.instance.Disable();
        }

        private void OnTitleVoiceOverDone()
        {
            SlingshotController.instance.Enable();

            GameObject poof = UnityEngine.Object.Instantiate(ThrowBallsGame.instance.poofPrefab, SlingshotController.instance.transform.position + new Vector3(0f, -5f, -2f), Quaternion.identity);
            UnityEngine.Object.Destroy(poof, 10);
            ThrowBallsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Poof);

            audioManager.PlayDialogue(Database.LocalizationDataId.ThrowBalls_Intro, OnIntroVoiceOverDone);
        }

        private void OnIntroVoiceOverDone()
        {
            AnturaController.instance.DoneChasing();
            AnturaController.instance.Disable();

            ThrowBallsGame.instance.StartCoroutine(ShowBallCoroutine());
        }

        private IEnumerator ShowBallCoroutine()
        {
            yield return new WaitForSeconds(SHOW_BALL_START_DELAY);

            BallController.instance.Enable();
            Catapult.instance.EnableCollider();

            GameObject poof = UnityEngine.Object.Instantiate(ThrowBallsGame.instance.cratePoofPrefab, BallController.instance.transform.position + Vector3.back * 1.75f, Quaternion.identity);
            UnityEngine.Object.Destroy(poof, 10);
            ThrowBallsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Poof);

            yield return new WaitForSeconds(SHOW_BALL_END_DELAY);

            switch (ThrowBallsConfiguration.Instance.Variation) {
                case ThrowBallsVariation.LetterName:
                    game.StartCoroutine(StartNewRound());
                    break;
                case ThrowBallsVariation.LetterAny:
                    game.StartCoroutine(StartNewRound());
                    break;
                case ThrowBallsVariation.Word:
                    game.StartCoroutine(StartNewRound());
                    break;
                case ThrowBallsVariation.BuildWord:
                    game.StartCoroutine(StartNewRound_LettersInWord());
                    break;
            }
        }

        private bool uiInitialised = false;
        public IEnumerator StartNewRound()
        {
            ResetScene();

            if (!uiInitialised && !IsTutorialRound()) {
                uiInitialised = true;

                
                game.Context.GetOverlayWidget().Initialize(true, false, true);
                game.Context.GetOverlayWidget().SetStarsThresholds(1, 3, 5);
                game.Context.GetOverlayWidget().SetMaxLives(MAX_NUM_BALLS);
            }

            IQuestionPack newQuestionPack = ThrowBallsConfiguration.Instance.Questions.GetNextQuestion();

            question = newQuestionPack.GetQuestion();
            ILivingLetterData correctDatum = newQuestionPack.GetCorrectAnswers().ToList()[0];
            List<ILivingLetterData> wrongData = newQuestionPack.GetWrongAnswers().ToList();

            if (ThrowBallsConfiguration.Instance.Variation == ThrowBallsVariation.Word) {
                correctDatum = new LL_ImageData(correctDatum.Id);

                for (int i = 0; i < wrongData.Count; i++) {
                    wrongData[i] = new LL_ImageData(wrongData[i].Id);
                }
            }
            else
                SayQuestion();

            yield return new WaitForSeconds(1f);

            int indexOfCorrectLetter = 0;

            if (game.Difficulty <= ThrowBallsGame.ThrowBallsDifficulty.Easy || IsTutorialRound()) {
                for (int i = 0; i < NumLettersInCurrentRound; i++) {
                    letterPool[i].SetActive(true);
                }

                int indexOfUnobstructedLetter = 0;

                while (letterControllers[indexOfUnobstructedLetter].IsObstructedByOtherLetter()) {
                    indexOfUnobstructedLetter++;
                }

                indexOfCorrectLetter = indexOfUnobstructedLetter;
            }

            for (int i = 0; i < NumLettersInCurrentRound; i++) {
                GameObject letterObj = letterPool[i];

                letterObj.SetActive(true);

                ConfigureLetterPropAndMotionVariation(letterControllers[i]);

                if (i == indexOfCorrectLetter) {
                    letterObj.tag = Constants.CORRECT_LETTER_TAG;
                    letterControllers[i].SetLetter(correctDatum);
                    tutorialTarget = letterObj;
                } else {
                    letterObj.tag = Constants.WRONG_LETTER_TAG;
                    letterControllers[i].SetLetter(wrongData[0]);
                    wrongData.RemoveAt(0);
                }
            }

            isRoundOngoing = true;

            BallController.instance.Enable();

            UIController.instance.Enable();
            UIController.instance.EnableLetterHint();
            UIController.instance.SetLivingLetterData(question);

            if (IsTutorialRound()) {
                switch (ThrowBallsConfiguration.Instance.Variation) {
                    case ThrowBallsVariation.LetterName:
                        audioManager.PlayDialogue(Database.LocalizationDataId.ThrowBalls_lettername_Tuto);
                        break;
                    case ThrowBallsVariation.LetterAny:
                        audioManager.PlayDialogue(Database.LocalizationDataId.ThrowBalls_lettername_Tuto);
                        break;
                    case ThrowBallsVariation.Word:
                        audioManager.PlayDialogue(Database.LocalizationDataId.ThrowBalls_word_Tuto);
                        break;
                    case ThrowBallsVariation.BuildWord:
                        audioManager.PlayDialogue(Database.LocalizationDataId.ThrowBalls_buildword_Tuto);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                inputManager.Enabled = true;
                isVoiceOverDone = true;
                ShowTutorialUI();
            }
        }

        public IEnumerator StartNewRound_LettersInWord()
        {
            IQuestionPack newQuestionPack = ThrowBallsConfiguration.Instance.Questions.GetNextQuestion();
            currentLettersForLettersInWord = newQuestionPack.GetCorrectAnswers().ToList();

            numLettersRemaining = currentLettersForLettersInWord.Count;

            ResetScene();

            List<int> sortedIndices = SortLettersByZIndex(currentLettersForLettersInWord.Count);

            if (!uiInitialised && !IsTutorialRound()) {
                uiInitialised = true;
                game.Context.GetOverlayWidget().Initialize(true, false, true);
                game.Context.GetOverlayWidget().SetStarsThresholds(1, 3, 5);
                game.Context.GetOverlayWidget().SetMaxLives(MAX_NUM_BALLS);
            }

            question = newQuestionPack.GetQuestion();
            SayQuestion();

            yield return new WaitForSeconds(1f);

            UIController.instance.Enable();
            UIController.instance.EnableLetterHint();
            UIController.instance.SetLivingLetterData(question);

            var letterToFlash = (LL_LetterData)currentLettersForLettersInWord[0];

            var letterDataToFlash = ArabicAlphabetHelper.FindLetter(AppManager.I.DB, ((LL_WordData)question).Data, letterToFlash.Data, false)[0];

            flashingTextCoroutine = ArabicTextUtilities.GetWordWithFlashingText(((LL_WordData)question).Data, letterDataToFlash.fromCharacterIndex, letterDataToFlash.toCharacterIndex, Color.green, FLASHING_TEXT_CYCLE_DURATION, int.MaxValue,
                    (string text) => {
                        UIController.instance.SetText(text);
                    }, false);

            flashedLettersInLiWVariation.Add(letterToFlash);

            ThrowBallsGame.instance.StartCoroutine(flashingTextCoroutine);

            for (int i = 0; i < currentLettersForLettersInWord.Count; i++) {
                int letterObjectIndex = game.Difficulty <= ThrowBallsGame.ThrowBallsDifficulty.Easy ? sortedIndices[i] : i;
                GameObject letterObj = letterPool[letterObjectIndex];

                letterObj.SetActive(true);

                ConfigureLetterPropAndMotionVariation(letterControllers[letterObjectIndex]);

                letterControllers[letterObjectIndex].SetLetter(currentLettersForLettersInWord[i]);
                letterObj.tag = currentLettersForLettersInWord[i].Id == currentLettersForLettersInWord[0].Id ? Constants.CORRECT_LETTER_TAG : Constants.WRONG_LETTER_TAG;

                if (i == 0) {
                    tutorialTarget = letterObj;
                }
            }

            isRoundOngoing = true;

            BallController.instance.Enable();

            if (IsTutorialRound()) {
                switch (ThrowBallsConfiguration.Instance.Variation) {
                    case ThrowBallsVariation.LetterName:
                        audioManager.PlayDialogue(Database.LocalizationDataId.ThrowBalls_lettername_Tuto);
                        break;
                    case ThrowBallsVariation.LetterAny:
                        audioManager.PlayDialogue(Database.LocalizationDataId.ThrowBalls_lettername_Tuto);
                        break;
                    case ThrowBallsVariation.Word:
                        audioManager.PlayDialogue(Database.LocalizationDataId.ThrowBalls_word_Tuto);
                        break;
                    case ThrowBallsVariation.BuildWord:
                        audioManager.PlayDialogue(Database.LocalizationDataId.ThrowBalls_buildword_Tuto);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                inputManager.Enabled = true;
                isVoiceOverDone = true;
                ShowTutorialUI();
            }
        }

        private List<int> SortLettersByZIndex(int numLetters)
        {
            List<float> zIndices = new List<float>();
            List<int> sortedIndices = new List<int>();

            for (int i = 0; i < numLetters; i++) {
                sortedIndices.Add(i);
                zIndices.Add(letterPool[i].transform.position.z);
            }

            for (int i = 0; i < numLetters - 1; i++) {
                int j = i + 1;

                while (j > 0) {
                    if (zIndices[j - 1] > zIndices[j]) {
                        float temp = zIndices[j - 1];
                        zIndices[j - 1] = zIndices[j];
                        zIndices[j] = temp;

                        int tempIndex = sortedIndices[j - 1];
                        sortedIndices[j - 1] = sortedIndices[j];
                        sortedIndices[j] = tempIndex;

                    }

                    j--;
                }
            }

            return sortedIndices;
        }

        private void SayQuestion()
        {
            game.Context.GetAudioManager().PlayVocabularyData(question, soundType: ThrowBallsConfiguration.Instance.GetVocabularySoundType());
        }

        private void ShowTutorialUI()
        {
            TutorialUI.Clear(false);

            Vector3 worldToScreen = Camera.main.WorldToScreenPoint(new Vector3(0, 8, -20));
            Vector3 fromPoint = Camera.main.ScreenToWorldPoint(new Vector3(worldToScreen.x, worldToScreen.y, 20f));
            Vector3 toPoint = LetterSpawner.instance.BiLerpForTutorialUI(tutorialTarget.transform.position);
            TutorialUI.DrawLine(fromPoint, toPoint, TutorialUI.DrawLineMode.FingerAndArrow);
            timeLeftToShowTutorialUI = TUTORIAL_UI_PERIOD;
        }

        private void UpdateLettersForLettersInWord(LetterController correctLetterCntrl)
        {
            correctLetterCntrl.Vanish();
            correctLetterCntrl.Reset();

            ILivingLetterData newCorrectLetter = currentLettersForLettersInWord[currentLettersForLettersInWord.Count - numLettersRemaining];

            for (int i = currentLettersForLettersInWord.Count - 1; i >= 0; i--) {
                if (letterControllers[i].GetLetter().Id == newCorrectLetter.Id && letterPool[i].activeSelf) {
                    letterPool[i].tag = Constants.CORRECT_LETTER_TAG;
                    tutorialTarget = letterPool[i];
                } else {
                    letterPool[i].tag = Constants.WRONG_LETTER_TAG;
                }
            }
        }

        public void OnBallLost()
        {
            if (isRoundOngoing && !IsTutorialRound()) {
                numBalls--;
                
                game.Context.GetOverlayWidget().SetLives(numBalls);

                if (numBalls == 0) {
                    BallController.instance.Disable();
                    OnRoundLost();
                }
            } else if (IsTutorialRound()) {
                ShowTutorialUI();
            }
        }

        public void OnRoundConcluded()
        {
            UIController.instance.DisableLetterHint();

            roundNumber++;

            if (roundNumber > MAX_NUM_ROUNDS) {
                EndGame();
            } else {
                if (ThrowBallsConfiguration.Instance.Variation == ThrowBallsVariation.BuildWord) {
                    game.StartCoroutine(StartNewRound_LettersInWord());
                } else {
                    game.StartCoroutine(StartNewRound());
                }
            }
        }

        private void DisableLetters(bool disablePropsToo)
        {
            foreach (LetterController letterController in letterControllers) {
                letterController.Disable();
                if (disablePropsToo) {
                    letterController.DisableProps();
                }
            }
        }

        public void OnCorrectLetterHit(LetterController correctLetterCntrl)
        {
            if (ThrowBallsConfiguration.Instance.Variation == ThrowBallsVariation.BuildWord) {
                numLettersRemaining--;
                var word = ((LL_WordData)question).Data;

                if (flashingTextCoroutine != null) {
                    ThrowBallsGame.instance.StopCoroutine(flashingTextCoroutine);
                }

                if (numLettersRemaining == 0) {
                    string markedText = ArabicTextUtilities.GetWordWithMarkedText(word, Color.green);
                    UIController.instance.SetText(markedText);
                } else {
                    var letterToFlash = (LL_LetterData)currentLettersForLettersInWord[currentLettersForLettersInWord.Count - numLettersRemaining];
                    int numTimesLetterHasBeenFlashed = flashedLettersInLiWVariation.Count(x => x.Id == letterToFlash.Id);
                    var letterDataToFlash = ArabicAlphabetHelper.FindLetter(AppManager.I.DB, word, letterToFlash.Data, false)[numTimesLetterHasBeenFlashed];
                    flashedLettersInLiWVariation.Add(letterToFlash);

                    flashingTextCoroutine = ArabicTextUtilities.GetWordWithFlashingText(word, letterDataToFlash.fromCharacterIndex, letterDataToFlash.toCharacterIndex, Color.green, FLASHING_TEXT_CYCLE_DURATION, int.MaxValue,
                        (string text) => {
                            UIController.instance.SetText(text);
                        }, true);

                    ThrowBallsGame.instance.StartCoroutine(flashingTextCoroutine);
                }

                UIController.instance.WobbleLetterHint();

                if (numLettersRemaining != 0) {
                    UpdateLettersForLettersInWord(correctLetterCntrl);
                    BallController.instance.DampenVelocity();
                } else {
                    OnRoundWon(correctLetterCntrl);
                }
            } else {
                OnRoundWon(correctLetterCntrl);
            }
        }

        private void OnRoundWon(LetterController correctLetterCntrl)
        {
            if (isRoundOngoing) {
                if (!IsTutorialRound()) {
                    numRoundsWon++;

                    game.Context.GetOverlayWidget().SetStarsScore(numRoundsWon);

                } else {
                    TutorialUI.Clear(true);
                }

                game.StartCoroutine(ShowWinSequence(correctLetterCntrl));
                BallController.instance.DampenVelocity();

                isRoundOngoing = false;

                game.Context.GetLogManager().OnAnswered(question, true);
            }
        }

        public void OnRoundLost()
        {
            if (isRoundOngoing) {
                BallController.instance.Disable();
                UIController.instance.DisableLetterHint();
                isRoundOngoing = false;
                DisableLetters(true);

                game.StartCoroutine(OnRoundLostCoroutine());

                game.Context.GetLogManager().OnAnswered(question, false);
            }
        }

        private IEnumerator OnRoundLostCoroutine()
        {
            game.Context.GetAudioManager().PlaySound(Sfx.Lose);
            yield return new WaitForSeconds(3f);
            OnRoundConcluded();
        }

        private IEnumerator ShowWinSequence(LetterController correctLetterCntrl)
        {
            correctLetterCntrl.ShowVictoryRays();

            yield return new WaitForSeconds(0.33f);

            correctLetterCntrl.Vanish();
            correctLetterCntrl.Reset();

            yield return new WaitForSeconds(0.7f);

            SayQuestion();

            correctLetterCntrl.SetMotionVariation(LetterController.MotionVariation.Idle);
            correctLetterCntrl.SetPropVariation(LetterController.PropVariation.Nothing);
            correctLetterCntrl.MoveTo(0, 13.5f, -33f);
            correctLetterCntrl.transform.rotation = Quaternion.Euler(-Camera.main.transform.rotation.eulerAngles.x, 180, 0);
            correctLetterCntrl.shadow.SetActive(false);

            if (ThrowBallsConfiguration.Instance.Variation == ThrowBallsVariation.BuildWord) {
                correctLetterCntrl.SetLetter(question);
            }

            correctLetterCntrl.Show();
            correctLetterCntrl.letterObjectView.DoHorray();

            game.Context.GetAudioManager().PlaySound(Sfx.Win);

            yield return new WaitForSeconds(3f);

            correctLetterCntrl.HideVictoryRays();

            OnRoundConcluded();
        }

        private Vector3 GetCratePosition(Vector3 relativeToLetterPosition)
        {
            return new Vector3(relativeToLetterPosition.x, relativeToLetterPosition.y - 2.1f, relativeToLetterPosition.z);
        }

        private void EndGame()
        {
            game.StartCoroutine(EndGame_Coroutine());
        }

        private IEnumerator EndGame_Coroutine()
        {
            ResetScene();

            UIController.instance.Disable();

            yield return new WaitForSeconds(1f);

            int numberOfStars = 2;

            if (numRoundsWon == 0) {
                numberOfStars = 0;
            } else if (numRoundsWon == 1 || numRoundsWon == 2) {
                numberOfStars = 1;
            } else if (numRoundsWon == 3 || numRoundsWon == 4) {
                numberOfStars = 2;
            } else {
                numberOfStars = 3;
            }

            game.EndGame(numberOfStars, 0);
        }

        private void ConfigureLetterPropAndMotionVariation(LetterController letterController)
        {
            if (IsTutorialRound()) {
                letterController.SetMotionVariation(LetterController.MotionVariation.Idle);
                letterController.SetPropVariation(LetterController.PropVariation.Nothing);
                return;
            }

            switch (game.Difficulty) {
                case ThrowBallsGame.ThrowBallsDifficulty.VeryEasy:
                    letterController.SetMotionVariation(LetterController.MotionVariation.Idle);
                    letterController.SetPropVariation(LetterController.PropVariation.Nothing);
                    break;
                case ThrowBallsGame.ThrowBallsDifficulty.Easy:
                    if (numRoundsWon < 2) {
                        letterController.SetMotionVariation(LetterController.MotionVariation.Idle);
                        letterController.SetPropVariation(LetterController.PropVariation.Nothing);
                    } else {
                        letterController.SetMotionVariation(LetterController.MotionVariation.Jumping);
                        letterController.SetPropVariation(LetterController.PropVariation.Nothing);
                    }
                    break;
                case ThrowBallsGame.ThrowBallsDifficulty.Normal:
                    if (numRoundsWon < 1) {
                        letterController.SetMotionVariation(LetterController.MotionVariation.Jumping);
                        letterController.SetPropVariation(LetterController.PropVariation.Nothing);
                    } else if (numRoundsWon < 3) {
                        letterController.SetMotionVariation(LetterController.MotionVariation.Idle);
                        letterController.SetPropVariation(LetterController.PropVariation.StaticPileOfCrates);
                    } else {
                        letterController.SetMotionVariation(LetterController.MotionVariation.Jumping);
                        letterController.SetPropVariation(LetterController.PropVariation.StaticPileOfCrates);
                    }
                    break;
                case ThrowBallsGame.ThrowBallsDifficulty.Hard:
                    if (roundNumber < 4) {
                        if (Random.value <= 0.5f) {
                            letterController.SetMotionVariation(LetterController.MotionVariation.Jumping);
                            letterController.SetPropVariation(LetterController.PropVariation.StaticPileOfCrates);
                        } else {
                            letterController.SetMotionVariation(LetterController.MotionVariation.Idle);
                            letterController.SetPropVariation(LetterController.PropVariation.SwervingPileOfCrates);
                        }
                    } else {
                        if (Random.value <= 0.6f) {
                            letterController.SetMotionVariation(LetterController.MotionVariation.Idle);
                            letterController.SetPropVariation(LetterController.PropVariation.SwervingPileOfCrates);
                        } else {
                            letterController.SetMotionVariation(LetterController.MotionVariation.Popping);
                            letterController.SetPropVariation(LetterController.PropVariation.Bush);
                        }
                    }
                    break;
                case ThrowBallsGame.ThrowBallsDifficulty.VeryHard:
                    if (Random.value <= 0.4f) {
                        letterController.SetMotionVariation(LetterController.MotionVariation.Idle);
                        letterController.SetPropVariation(LetterController.PropVariation.SwervingPileOfCrates);
                    } else {
                        letterController.SetMotionVariation(LetterController.MotionVariation.Popping);
                        letterController.SetPropVariation(LetterController.PropVariation.Bush);
                    }
                    break;
            }
        }

        public Vector3 GetPositionOfLetter(int index)
        {
            return letterControllers[index].gameObject.transform.position;
        }

        public void ResetScene()
        {
            UIController.instance.Reset();
            UIController.instance.Disable();

            foreach (LetterController letterController in letterControllers) {
                letterController.Reset();
                letterController.DisableProps();
            }

            for (int i = 0; i < letterPool.Length; i++) {
                GameObject letter = letterPool[i];
                letter.tag = Constants.WRONG_LETTER_TAG;
                letter.SetActive(false);
            }

            Vector3[] randomPositions = letterSpawner.GenerateRandomPositions(NumLettersInCurrentRound, IsTutorialRound());

            for (int i = 0; i < NumLettersInCurrentRound; i++) {
                GameObject letter = letterPool[i];
                letter.transform.position = randomPositions[i];
            }

            BallController.instance.Reset();

            numBalls = MAX_NUM_BALLS;

            if (roundNumber > 1 || !game.TutorialEnabled && roundNumber > 0) {
                
                game.Context.GetOverlayWidget().SetLives(MAX_NUM_BALLS);
            }

            isRoundOngoing = false;

            flashedLettersInLiWVariation.Clear();
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            if (IsTutorialRound()) {
                if (Input.touchCount > 0) {
                    Touch touch = Input.GetTouch(0);

                    switch (touch.phase) {
                        case TouchPhase.Began:
                            Touched();
                            break;
                        case TouchPhase.Ended:
                            OnMouseUp();
                            break;
                    }
                } else if (Input.GetMouseButtonDown(0)) {
                    Touched();
                } else if (Input.GetMouseButtonUp(0)) {
                    OnMouseUp();
                }
            }
        }

        public void UpdatePhysics(float delta)
        {
            if (isVoiceOverDone && IsTutorialRound() && isIdle && !BallController.instance.IsLaunched()) {
                timeLeftToShowTutorialUI -= Time.fixedDeltaTime;

                if (timeLeftToShowTutorialUI <= 0) {
                    ShowTutorialUI();
                }
            }
        }

        void Touched()
        {
            isIdle = false;
            TutorialUI.Clear(false);
        }

        void OnMouseUp()
        {
            isIdle = true;
            timeLeftToShowTutorialUI = TUTORIAL_UI_PERIOD;
        }

        public bool IsTutorialRound()
        {
            return roundNumber == 0 && game.TutorialEnabled;
        }
    }
}