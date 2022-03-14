using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Antura.Audio;
using Antura.LivingLetters;
using Antura.Minigames;

namespace Antura.Minigames.Scanner
{

    public class ScannerGame : MiniGameController
    {
        public bool disableInput;
        public bool gameActive = true;
        public float beltSpeed = 1f;
        public bool facingCamera = true;

        public const string TAG_BELT = "Scanner_Belt";
        public const string TAG_SCAN_START = "Scanner_ScanStart";
        public const string TAG_SCAN_END = "Scanner_ScanEnd";

        public GameObject antura;
        public float anturaMinDelay = 3f;
        public float anturaMaxDelay = 10f;
        public float anturaMinScreenTime = 1f;
        public float anturaMaxScreenTime = 2f;

        public GameObject poofPrefab;

        public ScannerDevice scannerDevice;

        public string currentWord = "";

        [Range(0, 1)]
        public float pedagogicalLevel = 0;

        public int numberOfRounds = 6;

        public int allowedFailedMoves = 3;

        public float maxPlaySpeed;
        public float minPlaySpeed;

        public GameObject LLPrefab;

        [HideInInspector]
        public List<ScannerLivingLetter> scannerLL;

        public List<ScannerSuitcase> suitcases;

        [HideInInspector]
        public List<ILivingLetterData> wordData;

        [HideInInspector]
        public ScannerRoundsManager roundsManager;

        [HideInInspector]
        public int LLCount;

        public int CurrentScoreRecord;

        public Animator trapDoor;

        public ScannerTutorial tut;
        public bool TutorialEnabled
        {
            get { return GetConfiguration().TutorialEnabled; }
        }

        public int STARS_1_THRESHOLD, STARS_2_THRESHOLD, STARS_3_THRESHOLD;

        public int CurrentStars
        {
            get
            {
                return (int)Mathf.Ceil(roundsManager.numberOfRoundsWon / 2f);
            }
        }

        public ScannerIntroductionState IntroductionState { get; private set; }
        public ScannerPlayState PlayState { get; private set; }
        public ScannerResultState ResultState { get; private set; }

        public void ResetScore()
        {
            CurrentScoreRecord = 0;
        }

        protected override FSM.IState GetInitialState()
        {
            return IntroductionState;
        }

        protected override IGameConfiguration GetConfiguration()
        {
            return ScannerConfiguration.Instance;
        }

        void SetupVariables()
        {
            gameActive = true;
            beltSpeed = 1f;
            facingCamera = true;

            if (Difficulty <= 0.4f)
            {
                beltSpeed = 1f;
            }
            else if (Difficulty > 0.4f && Difficulty <= 0.6f)
            {
                beltSpeed = 2f;
            }
            else if (Difficulty > 0.6f && Difficulty <= 0.8f)
            {
                beltSpeed = 3f;
            }
            else if (Difficulty > 0.8f && Difficulty < 1f)
            {
                beltSpeed = 3.5f;
            }
            else if (Difficulty == 1f)
            {
                beltSpeed = 4.5f;
            }

            if (Difficulty <= 0.25f)
            {
                facingCamera = true;
            }
            else if (Difficulty > 0.25f && Difficulty <= 0.5f)
            {
                facingCamera = true;
            }
            else if (Difficulty > 0.5f && Difficulty <= 0.75f)
            {
                facingCamera = true;
            }
            else if (Difficulty > 0.75f && Difficulty < 1f)
            {
                facingCamera = false;
            }
            else if (Difficulty == 1f)
            {
                facingCamera = false;
            }
        }

        protected override void OnInitialize(IGameContext context)
        {
            STARS_1_THRESHOLD = numberOfRounds / 3;
            STARS_2_THRESHOLD = numberOfRounds / 2;
            STARS_3_THRESHOLD = numberOfRounds;

            SetupVariables();

            LLCount = ScannerConfiguration.Instance.nCorrect;

            if (LLCount == 3)
            {
                suitcases.First().gameObject.SetActive(false);
                suitcases.Last().gameObject.SetActive(false);

                suitcases.Remove(suitcases.First());
                suitcases.Remove(suitcases.Last());

                var leftSS = suitcases.First().transform.localPosition;
                suitcases.First().transform.localPosition = new Vector3(-7, leftSS.y, leftSS.z);

                var rightSS = suitcases.Last().transform.localPosition;
                suitcases.Last().transform.localPosition = new Vector3(7, rightSS.y, rightSS.z);
            }

            IntroductionState = new ScannerIntroductionState(this);
            PlayState = new ScannerPlayState(this);
            ResultState = new ScannerResultState(this);

            roundsManager = new ScannerRoundsManager(this);

            tut = GetComponent<ScannerTutorial>();

            Context.GetOverlayWidget().SetStarsThresholds(STARS_1_THRESHOLD, STARS_2_THRESHOLD, STARS_3_THRESHOLD);
        }

        public float min = 0.03f, max = 0.6f;

        public void PlayWord(float deltaTime, ScannerLivingLetter LL)
        {
            Debug.Log("Play word: " + deltaTime);
            IAudioSource wordSound = Context.GetAudioManager().PlayVocabularyData(LL.LLController.Data, true);
            //float scaledDelta = (maxPlaySpeed - minPlaySpeed) / (max - min) * (deltaTime - max) + maxPlaySpeed;
            wordSound.Pitch = Mathf.Clamp(scannerDevice.smoothedDraggingSpeed * 4f, minPlaySpeed, maxPlaySpeed);
        }

        public void CreatePoof(Vector3 position, float duration, bool withSound)
        {
            if (withSound)
                AudioManager.I.PlaySound(Sfx.BalloonPop);
            GameObject poof = Instantiate(poofPrefab, position, Quaternion.identity) as GameObject;
            poof.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Destroy(poof, duration);
        }

        public void LogAnswer(ILivingLetterData data, bool isCorrect)
        {
            Context.GetLogManager().OnAnswered(data, isCorrect);
            //Debug.Log(data.TextForLivingLetter);
        }
    }
}
