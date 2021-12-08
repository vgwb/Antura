using System.Collections;
using System.Collections.Generic;
using Antura.Core;
using Antura.FSM;
using Antura.LivingLetters;
using Antura.Tutorial;
using Antura.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Antura.Minigames.Maze
{
    public class MazeGame : MiniGameController
    {
        public static MazeGame instance;

        private const int MAX_NUM_ROUNDS = 6;

        public GameObject characterPrefab;
        public GameObject arrowTargetPrefab;
        public GameObject dotPrefab;

        public MazeCharacter currentCharacter;
        public HandTutorial currentTutorial;

        public List<GameObject> prefabs;

        #region Score

        public override int MaxScore => STARS_3_THRESHOLD;

        // Stars
        const int STARS_1_THRESHOLD = 1;
        const int STARS_2_THRESHOLD = 3;
        const int STARS_3_THRESHOLD = 5;

        public int CurrentStars
        {
            get
            {
                if (CurrentScore < STARS_1_THRESHOLD)
                    return 0;
                if (CurrentScore < STARS_2_THRESHOLD)
                    return 1;
                if (CurrentScore < STARS_3_THRESHOLD)
                    return 2;
                return 3;
            }
        }

        #endregion

        public TextMeshProUGUI roundNumberText;

        private int roundNumber;

        public GameObject currentPrefab;
        public int health = 4;
        public GameObject cracks;
        List<GameObject> _cracks;
        public List<Vector3> pointsList;

        public List<LineRenderer> lines;

        [HideInInspector]
        public float gameTime;
        public MazeTimer timer;
        public GameObject antura;
        public GameObject fleePositionObject;

        private List<Vector3> fleePositions;

        public bool isTutorialMode;
        public Dictionary<string, int> allLetters;

        private MazeLetter currentMazeLetter;
        private IInputManager inputManager;

        public Color drawingColor;
        public Color incorrectLineColor;
        public float durationToTweenLineColors;

        public GameObject drawingTool;

        void setupIndices() // TODO: REMOVE NOT NEEDED ANYMORE
        {
            allLetters = new Dictionary<string, int>();
            for (int i = 0; i < prefabs.Count; ++i) {
                allLetters.Add(prefabs[i].name, i);
            }
        }

        private void OnPointerDown()
        {
            if (currentMazeLetter != null && !gameEnded) {
                currentMazeLetter.OnPointerDown();
            }
        }

        private void OnPointerUp()
        {
            if (currentMazeLetter != null && !gameEnded) {
                currentMazeLetter.OnPointerUp();
            }
        }

        public Vector2 GetLastPointerPosition()
        {
            return inputManager.LastPointerPosition;
        }

        protected override void Awake()
        {
            base.Awake();
            instance = this;

            ConfigureDotPrefab();

            Physics.IgnoreLayerCollision(10, 12);
        }

        private void ConfigureDotPrefab()
        {
            dotPrefab = Instantiate(dotPrefab);
            dotPrefab.GetComponent<CapsuleCollider>().enabled = false;
        }

        public void ColorCurrentLinesAsIncorrect()
        {
            lines[lines.Count - 1].material.DOColor(incorrectLineColor, durationToTweenLineColors);
        }

        public void startGame()
        {
            isTutorialMode = GetConfiguration().TutorialEnabled;

            setupIndices();

            fleePositions = new List<Vector3>();
            foreach (Transform child in fleePositionObject.transform) {
                fleePositions.Add(child.position);
            }

            antura.AddComponent<MazeAntura>();
            //cracks to display:
            _cracks = new List<GameObject>();
            cracks.SetActive(true);
            foreach (Transform child in cracks.transform) {
                child.gameObject.SetActive(false);
                _cracks.Add(child.gameObject);
            }

            lines = new List<LineRenderer>();

            roundNumber = 0;
            roundNumberText.text = "#" + (roundNumber + 1);

            ConfigureTimer();

            //init first letter
            initCurrentLetter();
            if (!isTutorialMode) initUI();

            Context.GetAudioManager().PlayMusic(Music.Theme8);
        }

        private void ConfigureTimer()
        {
            gameTime = 90f;
        }

        public void OnFruitGotDrawnOver(MazeArrow mazeArrow)
        {
            currentMazeLetter.NotifyFruitGotMouseOver(mazeArrow);
        }

        public void OnDrawnLetterWrongly()
        {
            currentMazeLetter.NotifyDrawnLetterWrongly();
        }

        private bool uiInitialized;
        private void initUI()
        {
            if (uiInitialized) return;
            uiInitialized = true;

            Context.GetOverlayWidget().Initialize(true, true, false);
            Context.GetOverlayWidget().SetStarsThresholds(STARS_1_THRESHOLD, STARS_2_THRESHOLD, STARS_3_THRESHOLD);

            timer.initTimer();
        }

        public void addLine()
        {
            pointsList = new List<Vector3>();
            GameObject go = new GameObject();
            go.transform.position = new Vector3(0, 0, -0.2f);
            go.transform.Rotate(new Vector3(90, 0, 0));
            LineRenderer line = go.AddComponent<LineRenderer>();
            line.positionCount = 0;
            line.startWidth = 0.6f;
            line.endWidth = 0.6f;
            line.material = new Material(Shader.Find("Antura/Transparent"));
            line.material.color = drawingColor;

            lines.Add(line);

        }
        public bool tutorialForLetterisComplete()
        {
            return currentTutorial.isCurrentTutorialDone();
        }

        public bool isCurrentLetterComplete()
        {
            return currentTutorial.isComplete();
        }

        public void showAllCracks()
        {
            if (!currentCharacter || currentCharacter.isAppearing || !currentCharacter.gameObject.activeSelf) return;
            if (health == 0)
                return;

            for (int i = 0; i < _cracks.Count; ++i)
                _cracks[i].SetActive(true);

            MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.ScreenHit);

        }

        public void wasHit()
        {
            if (!currentCharacter || currentCharacter.isAppearing || !currentCharacter.gameObject.activeSelf) return;
            _cracks[_cracks.Count - health].SetActive(true);
            health--;
        }

        IEnumerator waitAndPerformCallback(float seconds, VoidDelegate init, VoidDelegate callback)
        {
            init();

            yield return new WaitForSeconds(seconds);

            callback();
        }

        public void moveToNext(bool won = false)
        {
            if (!currentCharacter || currentCharacter.isAppearing || !currentCharacter.gameObject.activeSelf) return;

            isShowingAntura = false;
            //check if current letter is complete:
            if (currentCharacter.isComplete()) {
                if (!isTutorialMode)
                {
                    CurrentScore++;
                    roundNumber++;
                    MazeConfiguration.Instance.Context.GetLogManager().OnAnswered(currentLL, true);
                }

                // Show message:
                MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Win);

                // Hide checkpoints of last path:
                currentTutorial.HideCheckpointsAndLineOfCurrentPath();

                currentCharacter.Celebrate(() => {
                    if (roundNumber == MAX_NUM_ROUNDS || CurrentStars == 3) {
                        endGame();
                    } else {
                        if (isTutorialMode) {
                            isTutorialMode = false;
                            initUI();
                        }

                        roundNumberText.text = "#" + (roundNumber + 1);
                        restartCurrentLetter(won);
                    }
                },
                OnAnswerValidated);
            } else {
                addLine();
                currentCharacter.nextPath();
                currentTutorial.moveToNextPath();
            }
        }

        public void lostCurrentLetter()
        {
            if (!currentCharacter || currentCharacter.isAppearing || !currentCharacter.gameObject.activeSelf) return;

            if (isTutorialMode) {
                hideCracks();

                //remove last line
                if (lines.Count > 0) {
                    lines[lines.Count - 1].positionCount = 0;
                    lines.RemoveAt(lines.Count - 1);
                }

                pointsList.RemoveRange(0, pointsList.Count);

                //removeLines();

                TutorialUI.Clear(false);
                addLine();

                currentCharacter.resetToCurrent();
                showCurrentTutorial();
                return;
            }

            roundNumber++;

            MazeConfiguration.Instance.Context.GetLogManager().OnAnswered(currentLL, false);

            if (roundNumber == MAX_NUM_ROUNDS || CurrentStars == 3) {
                endGame();
            } else {
                roundNumberText.text = "#" + (roundNumber + 1);
                restartCurrentLetter();
            }
        }

        public void restartCurrentLetter(bool won = false)
        {
            currentPrefab.SendMessage("moveOut", won);

            hideCracks();
            removeLines();

            initCurrentLetter();
        }

        void OnAnswerValidated()
        {
        }

        void removeLines()
        {
            foreach (LineRenderer line in lines)
                line.positionCount = 0;
            lines = new List<LineRenderer>();
            pointsList.RemoveRange(0, pointsList.Count);

        }

        void hideCracks()
        {
            health = 4;
            //hide cracks:
            foreach (Transform child in cracks.transform) {
                child.gameObject.SetActive(false);
            }
        }
        public LL_LetterData currentLL;

        public NewMazeLetter newMazeLetter;

        void initCurrentLetter()
        {
            if (gameEnded) {
                return;
            }

            currentCharacter = null;
            currentTutorial = null;

            TutorialUI.Clear(false);
            addLine();

            //get a new letter:
            IQuestionPack newQuestionPack = MazeConfiguration.Instance.Questions.GetNextQuestion();
            List<ILivingLetterData> ldList = (List<ILivingLetterData>)newQuestionPack.GetCorrectAnswers();
            LL_LetterData ld = (LL_LetterData)ldList[0];
            int index = -1;

            var id = "be";
            var _ld = AppManager.I.DB.GetLetterDataById(id);
            ld = new LL_LetterData(_ld);

            if (allLetters.ContainsKey(ld.Id))
                index = allLetters[ld.Id];
            if (index == -1) {
                Debug.Log("Letter got from Teacher is: " + ld.Id + " - does not match 11 models we have, we will play sound of the returned data");
                index = Random.Range(0, prefabs.Count);
            }

            // START NEW GAME
            newMazeLetter.SetupLetter(ld);
            // END NEW GAME

            currentLL = ld;
            //currentPrefab = Instantiate(prefabs[index]);
            currentPrefab = Instantiate(newMazeLetter.gameObject);
            currentPrefab.GetComponent<NewMazeLetter>().SetupLetter(ld);
            currentPrefab.GetComponent<NewMazeLetterBuilder>().build(() => {

                if (!isTutorialMode) {
                    MazeConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(
                        ld,
                        soundType: MazeConfiguration.Instance.GetVocabularySoundType()
                    );
                }

                foreach (Transform child in currentPrefab.transform) {
                    if (child.name == "Mazecharacter") {
                        currentCharacter = child.GetComponent<MazeCharacter>();
                    } else if (child.name == "HandTutorial") {
                        currentTutorial = child.GetComponent<HandTutorial>();
                    }
                }

                currentCharacter.gameObject.SetActive(false);

                currentMazeLetter = currentPrefab.GetComponentInChildren<MazeLetter>();
            });
            currentPrefab.GetComponent<NewMazeLetterBuilder>().Build();

        }

        public void showCharacterMovingIn()
        {
            if (isTutorialMode)
            {
                PlayTutorial(
                    () =>
                    {
                        currentCharacter.initialPosition = currentCharacter.transform.position;
                        currentCharacter.initialRotation = currentCharacter.transform.rotation;
                        currentCharacter.gameObject.SetActive(true);
                        currentCharacter.Appear();

                        MazeConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(
                            currentLL,
                            soundType: MazeConfiguration.Instance.GetVocabularySoundType());
                    });
                return;
            }
            currentCharacter.initialPosition = currentCharacter.transform.position;
            currentCharacter.initialRotation = currentCharacter.transform.rotation;
            currentCharacter.gameObject.SetActive(true);
            currentCharacter.Appear();
        }

        public void showCurrentTutorial()
        {
            isShowingAntura = false;

            if (currentTutorial != null) {
                currentTutorial.showCurrentTutorial();
            }

            if (currentCharacter != null) {
                currentCharacter.initialize();
            }
        }

        IEnumerator shakeCamera(float duration, float magnitude)
        {

            float elapsed = 0.0f;
            Vector3 originalCamPos = Camera.main.transform.position;

            while (elapsed < duration) {

                elapsed += Time.deltaTime;

                float percentComplete = elapsed / duration;
                float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

                // map value to [-1, 1]
                float x = Random.value * 2.0f - 1.0f;
                float y = Random.value * 2.0f - 1.0f;
                x *= magnitude * damper;
                y *= magnitude * damper;

                Camera.main.transform.position = new Vector3(x, y, originalCamPos.z);

                yield return null;
            }

            Camera.main.transform.position = originalCamPos;
        }

        public void appendToLine(Vector3 mousePos)
        {
            if (!pointsList.Contains(mousePos)) {
                //mousePos.z = -0.1071415f;
                pointsList.Add(mousePos);
                lines[lines.Count - 1].positionCount = pointsList.Count;
                lines[lines.Count - 1].SetPosition(pointsList.Count - 1, pointsList[pointsList.Count - 1]);
            }
        }

        public void AdjustLastPointOfLine(Vector3 adjustedPoint)
        {
            lines[lines.Count - 1].SetPosition(pointsList.Count - 1, adjustedPoint);
        }

        public bool gameEnded;
        private void endGame()
        {
            if (gameEnded) {
                return;
            }

            gameEnded = true;

            MinigamesUI.Timer.Pause();
            TutorialUI.Clear(false);

            // Reset physics collisions:
            Physics.IgnoreLayerCollision(10, 12, false);

            EndGame(CurrentStars, CurrentScore);
        }

        public void onTimeUp()
        {
            endGame();

            if (currentTutorial != null) {
                currentTutorial.HideAllCheckpointsAndLines();
            }

            if (currentCharacter != null) {
                currentCharacter.gameObject.SetActive(false);
            }

            foreach (var line in lines) {
                line.enabled = false;
            }
        }

        public bool isShowingAntura;
        public void onIdleTime()
        {
            if (isShowingAntura || gameEnded) { return; }
            isShowingAntura = true;

            timer.StopTimer();

            antura.SetActive(true);
            antura.GetComponent<MazeAntura>().SetAnturaTime(true, currentCharacter.transform.position);

            currentCharacter.Flee();
        }

        public Vector3 getRandFleePosition()
        {
            int randIndex = Random.Range(0, fleePositions.Count);
            return (fleePositions[randIndex]);
        }

        //states
        public MazeIntroState IntroductionState { get; private set; }

        protected override IGameConfiguration GetConfiguration()
        {
            return MazeConfiguration.Instance;
        }

        protected override IState GetInitialState()
        {
            return IntroductionState;
        }

        protected override void OnInitialize(IGameContext context)
        {
            IntroductionState = new MazeIntroState(this);

            inputManager = context.GetInputManager();
            inputManager.onPointerDown += OnPointerDown;
            inputManager.onPointerUp += OnPointerUp;
        }
    }
}