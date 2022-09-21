using Antura.Database;
using Antura.LivingLetters;
using Antura.Tutorial;
using Antura.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Language;
using TMPro;
using DG.Tweening;
using UnityEngine;

namespace Antura.Minigames.Balloons
{
    public class BalloonsGame : MiniGameController
    {
        [Header("References")]
        public WordPromptController wordPrompt;
        public WordFlexibleContainer wordFlexibleContainer;
        public Animator wordFlexibleContainerAnimator;
        public GameObject floatingLetterPrefab;
        public GameObject floatingLetterPrefab_LetterVariation;
        public Transform[] floatingLetterLocations;
        public AnimationClip balloonPopAnimation;
        public GameObject runningAntura;
        public Canvas uiCanvas;
        public Canvas endGameCanvas;
        public TextMeshProUGUI roundNumberText;
        public TimerManager timer;
        public Animator countdownAnimator;
        public GameObject FxParticlesPoof;
        public RoundResultAnimator roundResultAnimator;

        [Header("Stage")]
        public float minX;
        public float maxX;
        public float minY;
        public float maxY;

        //[Header("Images")]
        //public Sprite FailWrongBalloon;
        //public Sprite FailTime;

        [Header("Game Parameters")]
        [Tooltip("e.g.: 6")]
        public const int numberOfRounds = 6;
        public int lives;

        public float RoundTime
        {
            get
            {
                switch (BalloonsConfiguration.Instance.Variation)
                {
                    case BalloonsVariation.Counting:
                    case BalloonsVariation.LetterInWord:
                        return 60;
                    default:
                        return 30;
                }
            }

        }

        public Color[] balloonColors;

        [HideInInspector]
        public List<FloatingLetterController> floatingLetters;
        [HideInInspector]
        public float letterDropDelay;
        [HideInInspector]
        public float letterAnimationLength = 0.367f;

        public static BalloonsGame instance;

        public bool isTutorialRound;

        public IQuestionPack questionPack;
        public ILivingLetterData question;
        public List<ILivingLetterData> correctAnswers;
        public IEnumerable<ILivingLetterData> wrongAnswers;
        private ILivingLetterData lastDroppedLivingLetter;
        public int countingIndex;
        public int maxCountingIndex;
        //private LL_WordData wordData;
        //private string word;
        //private List<LL_LetterData> wordLetters = new List<LL_LetterData>();
        private int currentRound = 0;
        private int remainingLives;

        //        private int _tutorialState;
        //
        //        private int TutorialState
        //        {
        //            get { return _tutorialState; }
        //            set {
        //                _tutorialState = value;
        //                OnTutorialStateChanged();
        //            }
        //        }

        #region Score

        public override int MaxScore => STARS_3_THRESHOLD;

        private readonly int STARS_1_THRESHOLD = Mathf.CeilToInt(0.33f * numberOfRounds);
        private readonly int STARS_2_THRESHOLD = Mathf.CeilToInt(0.66f * numberOfRounds);
        private readonly int STARS_3_THRESHOLD = numberOfRounds;

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

        enum Result
        {
            PERFECT,
            GOOD,
            CLEAR,
            FAIL
        }

        enum How2Die
        {
            Null,
            TimeUp,
            WrongBalloon
        }

        How2Die howDied;

        private enum RoundStatus
        {
            Over, Started
        }

        private RoundStatus roundStatus;

        private IPopupWidget Popup { get { return GetConfiguration().Context.GetPopupWidget(); } }

        private IAudioManager AudioManager { get { return GetConfiguration().Context.GetAudioManager(); } }

        public BalloonsIntroductionState IntroductionState { get; private set; }
        public BalloonsQuestionState QuestionState { get; private set; }
        public BalloonsPlayState PlayState { get; private set; }
        public BalloonsResultState ResultState { get; private set; }

        protected override void OnInitialize(IGameContext context)
        {
            IntroductionState = new BalloonsIntroductionState(this, GetConfiguration().TutorialEnabled);
            QuestionState = new BalloonsQuestionState(this);
            PlayState = new BalloonsPlayState(this);
            ResultState = new BalloonsResultState(this);

        }

        public void InitializeMinigameUI()
        {
            Context.GetOverlayWidget().Initialize(true, true, false);
            Context.GetOverlayWidget().SetStarsThresholds(STARS_1_THRESHOLD, STARS_2_THRESHOLD, STARS_3_THRESHOLD);
        }

        protected override FSM.IState GetInitialState()
        {
            return IntroductionState;
        }

        protected override IGameConfiguration GetConfiguration()
        {
            return BalloonsConfiguration.Instance;
        }

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        protected override void Start()
        {
            base.Start();

            Random.InitState(System.DateTime.Now.GetHashCode());
            remainingLives = lives;
            letterDropDelay = balloonPopAnimation.length;

            ResetScene();

            roundStatus = RoundStatus.Over;
        }

        public void PlayActiveMusic()
        {
            GetConfiguration().Context.GetAudioManager().PlayMusic(Music.MainTheme);
        }

        public void PlayIdleMusic()
        {
            GetConfiguration().Context.GetAudioManager().PlayMusic(Music.Relax);
        }

        public void PlayTutorialVoiceOver(float delay = 2f)
        {
            StartCoroutine(PlayDialog_Coroutine(BalloonsConfiguration.Instance.TutorialLocalizationId, delay));
        }

        private IEnumerator PlayDialog_Coroutine(LocalizationDataId dialog, float delay)
        {
            yield return new WaitForSeconds(delay);
            GetConfiguration().Context.GetAudioManager().PlayDialogue(dialog);
        }

        public void OnRoundStartPressed()
        {
            Popup.Hide();
            BeginGameplay();
        }

        public void OnRoundResultPressed()
        {
            AudioManager.PlaySound(Sfx.UIButtonClick);
            Popup.Hide();
            Play(true);
        }

        public void Play(bool advanceRound)
        {
            if (advanceRound)
                currentRound++;
            if (currentRound <= numberOfRounds)
            {
                StartNewRound();
            }
            else
            {
                EndGame();
            }
        }

        public void PlayTutorial()
        {
            Debug.Log("Playing Tutorial");

            isTutorialRound = true;
            PlayIdleMusic();

            StartNewRound();

            PlayTutorialVoiceOver();
            StartCoroutine("ShowTutorialUI_Coroutine");
        }

        private IEnumerator ShowTutorialUI_Coroutine()
        {
            yield return new WaitForSeconds(1f);

            while (isTutorialRound)
            {
                yield return new WaitForSeconds(1f);

                if (BalloonsConfiguration.Instance.Variation == BalloonsVariation.Counting)
                {
                    var floatingLetter = floatingLetterLocations[countingIndex].GetComponentInChildren<FloatingLetterController>();
                    if (floatingLetter == null)
                    {
                        continue;
                    }

                    foreach (var balloon in floatingLetter.ActiveVariation.balloons)
                    {
                        if (balloon.balloonCollider.enabled)
                        {
                            TutorialUI.Click(balloon.transform.position + 2.75f * Vector3.up + 2f * Vector3.back);
                        }
                    }
                }
                else
                {
                    foreach (var floatingLetter in floatingLetters)
                    {
                        if (!floatingLetter.Letter.isRequired && floatingLetter.enabled)
                        {
                            foreach (var balloon in floatingLetter.ActiveVariation.balloons)
                            {
                                if (balloon.balloonCollider.enabled)
                                {
                                    TutorialUI.Click(balloon.transform.position + 2.75f * Vector3.up + 2f * Vector3.back);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void HideTutorialUI()
        {
            StopCoroutine("ShowTutorialUI_Coroutine");
            TutorialUI.Clear(false);
        }

        public void StartNewRound()
        {
            alreadyClicked = false;
            if (roundStatus != RoundStatus.Started)
            {
                roundStatus = RoundStatus.Started;

                ResetScene();

                // Get next question
                questionPack = GetConfiguration().Questions.GetNextQuestion();
                question = questionPack.GetQuestion();
                correctAnswers = questionPack.GetCorrectAnswers().ToList();
                wrongAnswers = questionPack.GetWrongAnswers();

                switch (BalloonsConfiguration.Instance.Variation)
                {
                    case BalloonsVariation.Spelling:
                        var spellingWordData = question as LL_WordData;
                        var spellingWord = ArabicFixer.Fix(spellingWordData.Data.Text);

                        // Display
                        wordPrompt.DisplayWord(correctAnswers.Cast<LL_LetterData>().ToList());

                        // Debug
                        Debug.Log("[New Round] Spelling Word: " + spellingWord);
                        Debug.Log("Letters (" + correctAnswers.Count() + "): " + string.Join(" / ", correctAnswers.Cast<LL_LetterData>().ToList().Select(x => x.TextForLivingLetter).Reverse().ToArray()));
                        Debug.Log("Random Letters (" + wrongAnswers.Count() + "): " + string.Join(" / ", wrongAnswers.Cast<LL_LetterData>().ToList().Select(x => x.TextForLivingLetter).Reverse().ToArray()));

                        // Start round
                        StartCoroutine(StartNewRound_Coroutine());
                        break;

                    case BalloonsVariation.Words:
                    case BalloonsVariation.Image:
                        correctAnswers = new List<ILivingLetterData>() { question };
                        ILivingLetterData wordToKeepData = question as LL_WordData;
                        if (BalloonsConfiguration.Instance.Variation == BalloonsVariation.Image)
                            wordToKeepData = new LL_ImageData(wordToKeepData.Id);

                        // Display
                        wordFlexibleContainer.gameObject.SetActive(true);
                        wordFlexibleContainer.SetLetterData(wordToKeepData);

                        // Debug
                        //Debug.Log("[New Round] Word To Keep: " + wordToKeepData.TextForLivingLetter);
                        //Debug.Log(" Matching word: " + ArabicFixer.Fix(correctAnswers.Cast<LL_WordData>().ToList()[0].Data.Text));
                        // Debug.Log(" Random words (" + wrongAnswers.Count() + "): " + string.Join(" / ", wrongAnswers.Cast<LL_WordData>().ToList().Select(x => ArabicFixer.Fix(x.Data.Text)).ToArray()));

                        // Start round
                        StartCoroutine(StartNewRound_Coroutine());
                        break;

                    case BalloonsVariation.LetterInWord:
                        var letterToKeepData = question as LL_LetterData;
                        var letterToKeep = letterToKeepData.TextForLivingLetter;

                        // Display
                        wordFlexibleContainer.gameObject.SetActive(true);

                        string text = "";
                        text = "<size=130%>" + letterToKeepData.Data.GetStringForDisplay(LetterForm.Isolated) + "</size>";
                        text += "\n" + letterToKeepData.Data.GetStringForDisplay(LetterForm.Initial);
                        text += " " + letterToKeepData.Data.GetStringForDisplay(LetterForm.Medial);
                        text += " " + letterToKeepData.Data.GetStringForDisplay(LetterForm.Final);

                        wordFlexibleContainer.SetTextUnfiltered(text);

                        // Debug
                        Debug.Log("[New Round] Letter To Keep: " + letterToKeep);
                        Debug.Log(" Words with letter (" + correctAnswers.Count() + "): " + string.Join(" / ", correctAnswers.Cast<LL_WordData>().ToList().Select(x => x.TextForLivingLetter).ToArray()));
                        Debug.Log(" Random words without letter (" + wrongAnswers.Count() + "): " + string.Join(" / ", wrongAnswers.Cast<LL_WordData>().ToList().Select(x => x.TextForLivingLetter).ToArray()));

                        // Start round
                        StartCoroutine(StartNewRound_Coroutine());
                        break;

                    case BalloonsVariation.Counting:
                        countingIndex = 0;

                        // Display
                        DisplayWordFlexibleContainer_Counting();

                        // Debug
                        Debug.Log("[New Round] Correct Answers (" + correctAnswers.Count() + "): " + string.Join(" / ", correctAnswers.ToList().Select(x => x.TextForLivingLetter).ToArray()));

                        // Start round
                        StartCoroutine(StartNewRound_Coroutine());
                        break;

                    default:
                        Debug.LogError("Invalid Balloons Game Variation!");
                        break;
                }
            }
        }

        private void DisplayWordFlexibleContainer_Counting()
        {
            if (countingIndex < 0 || countingIndex > maxCountingIndex)
            {
                return;
            }

            wordFlexibleContainer.gameObject.SetActive(true);
            var imageData = new LL_ImageData(correctAnswers[countingIndex].Id);
            wordFlexibleContainer.SetLetterData(imageData);
        }

        private IEnumerator StartNewRound_Coroutine()
        {
            float delay = 0.25f;
            yield return new WaitForSeconds(delay);

            switch (BalloonsConfiguration.Instance.Variation)
            {
                case BalloonsVariation.Spelling:
                    SayQuestion();
                    //Popup.Show();
                    //Popup.SetButtonCallback(OnRoundStartPressed);
                    //if (question.DataType == LivingLetterDataType.Word)
                    //{
                    //    Popup.SetLetterData(question as LL_WordData);
                    //}
                    OnRoundStartPressed();
                    uiCanvas.gameObject.SetActive(true);
                    break;

                case BalloonsVariation.Words:
                case BalloonsVariation.Image:
                    SayQuestion();
                    //Popup.Show();
                    //Popup.SetButtonCallback(OnRoundStartPressed);
                    //if (question.DataType == LivingLetterDataType.Word)
                    //{
                    //    Popup.SetLetterData(question as LL_WordData);
                    //}
                    OnRoundStartPressed();
                    uiCanvas.gameObject.SetActive(true);
                    break;

                case BalloonsVariation.LetterInWord:
                    SayQuestion();
                    //Popup.Show();
                    //Popup.SetButtonCallback(OnRoundStartPressed);
                    //Popup.SetLetterData(question);
                    OnRoundStartPressed();
                    uiCanvas.gameObject.SetActive(true);
                    break;

                case BalloonsVariation.Counting:
                    SayQuestion();
                    //AudioManager.PlayVocabularyData(correctAnswers.Last());
                    //Popup.Show();
                    //Popup.SetButtonCallback(OnRoundStartPressed);
                    //Popup.SetLetterData(question);
                    OnRoundStartPressed();
                    uiCanvas.gameObject.SetActive(true);
                    DisplayWordFlexibleContainer_Counting();
                    break;

                default:
                    Debug.LogError("Invalid Balloons Game Variation!");
                    break;
            }
        }

        private void EndRound(Result result)
        {
            PlayIdleMusic();
            DisableFloatingLetters();
            timer.StopTimer();
            ProcessRoundResult(result);

            roundStatus = RoundStatus.Over;
        }

        private void EndGame()
        {
            StartCoroutine(EndGame_Coroutine());
        }

        private IEnumerator EndGame_Coroutine()
        {
            var delay = 1f;
            yield return new WaitForSeconds(delay);

            ResetScene();
            uiCanvas.gameObject.SetActive(false);

            PlayState.OnResult();
        }

        private void ResetScene()
        {
            timer.ResetTimer();
            timer.DisplayTime();
            roundNumberText.text = "#" + currentRound.ToString();
            wordPrompt.Reset();
            wordFlexibleContainer.Reset();
            wordFlexibleContainer.gameObject.SetActive(false);
            uiCanvas.gameObject.SetActive(false);
            DestroyAllFloatingLetters();
            howDied = How2Die.Null;
            questionPack = null;
            roundResultAnimator.Hide();
            countingIndex = 0;
            lastDroppedLivingLetter = null;
        }

        private void BeginGameplay()
        {
            switch (BalloonsConfiguration.Instance.Variation)
            {
                case BalloonsVariation.Spelling:
                    timer.DisplayTime();
                    CreateFloatingLetters_Spelling();
                    if (isTutorialRound)
                    {
                        FreezeFloatingLetters();
                        DisableRequiredFloatingLetters();
                    }
                    else
                    {
                        runningAntura.SetActive(true);
                        timer.StartTimer();
                        PlayActiveMusic();
                    }
                    break;

                case BalloonsVariation.Words:
                case BalloonsVariation.Image:
                    timer.DisplayTime();
                    CreateFloatingLetters_Words();
                    if (isTutorialRound)
                    {
                        FreezeFloatingLetters();
                        DisableRequiredFloatingLetters();
                    }
                    else
                    {
                        runningAntura.SetActive(true);
                        timer.StartTimer();
                        PlayActiveMusic();
                    }
                    break;

                case BalloonsVariation.LetterInWord:
                    timer.DisplayTime();
                    CreateFloatingLetters_Letter();
                    if (isTutorialRound)
                    {
                        FreezeFloatingLetters();
                        DisableRequiredFloatingLetters();
                    }
                    else
                    {
                        runningAntura.SetActive(true);
                        timer.StartTimer();
                        PlayActiveMusic();
                    }
                    break;

                case BalloonsVariation.Counting:
                    timer.DisplayTime();
                    CreateFloatingLetters_Counting();
                    if (isTutorialRound)
                    {
                        FreezeFloatingLetters();
                        DisableRequiredFloatingLetters();
                    }
                    else
                    {
                        runningAntura.SetActive(true);
                        timer.StartTimer();
                        PlayActiveMusic();
                    }
                    break;

                default:
                    Debug.LogError("Invalid Balloons Game Variation!");
                    break;
            }
        }

        private void AnimateCountdown(string text)
        {
            countdownAnimator.gameObject.GetComponent<TextMeshProUGUI>().text = text;
            countdownAnimator.SetTrigger("Count");
        }

        private int ExtraAnswers => (int)Mathf.Lerp(1, 10, Difficulty);

        private void CreateFloatingLetters_Spelling()
        {
            var wordLetters = correctAnswers.Cast<LL_LetterData>().ToList();
            var randomLetters = wrongAnswers.Cast<LL_LetterData>().GetEnumerator();

            var numberOfLetters = Mathf.Clamp(wordLetters.Count + ExtraAnswers, 0, floatingLetterLocations.Length);

            // Determine indices of required letters
            List<int> requiredLetterIndices = new List<int>();
            for (int i = 0; i < wordLetters.Count; i++)
            {
                var index = Random.Range(0, numberOfLetters);

                if (!requiredLetterIndices.Contains(index))
                {
                    requiredLetterIndices.Add(index);
                }
                else
                {
                    i--;
                }
            }

            // Create floating letters
            for (int i = 0; i < numberOfLetters; i++)
            {
                var instance = Instantiate(floatingLetterPrefab);
                instance.SetActive(true);
                instance.transform.SetParent(floatingLetterLocations[i]);
                instance.transform.localPosition = Vector3.zero;

                int requiredLetterIndex = requiredLetterIndices.IndexOf(i);
                bool isRequiredLetter = requiredLetterIndex > -1 ? true : false;

                var floatingLetter = instance.GetComponent<FloatingLetterController>();

                // Set variation, set at least 2 balloons for required letters
                //if (isRequiredLetter) {
                floatingLetter.SetActiveVariation(Random.Range(1, floatingLetter.variations.Length));
                //} else {
                //    floatingLetter.SetActiveVariation(Random.Range(0, floatingLetter.variations.Length));
                //}

                var balloons = floatingLetter.Balloons;
                var letter = floatingLetter.Letter;

                // Set random balloon colors without repetition if possible
                var usedColorIndexes = new List<int>();
                for (int j = 0; j < balloons.Length; j++)
                {
                    int randomColorIndex;

                    if (balloons.Length <= balloonColors.Length)
                    {
                        do
                        {
                            randomColorIndex = Random.Range(0, balloonColors.Length);
                        } while (usedColorIndexes.Contains(randomColorIndex));
                    }
                    else
                    {
                        randomColorIndex = Random.Range(0, balloonColors.Length);
                    }

                    usedColorIndexes.Add(randomColorIndex);
                    balloons[j].SetColor(balloonColors[randomColorIndex]);
                }

                // Set letters
                if (isRequiredLetter)
                {
                    // Set required letter
                    letter.isRequired = true;
                    letter.associatedPromptIndex = requiredLetterIndex;
                    letter.Init(wordLetters[requiredLetterIndex]);
                    //Debug.Log("Create word balloon with: " + wordLetters[requiredLetterIndex].TextForLivingLetter);
                }
                else
                {
                    // Set a random letter that is not a required letter
                    LL_LetterData randomLetter;
                    bool invalid = false;
                    do
                    {
                        randomLetter = randomLetters.Current;
                        invalid = randomLetter == null || wordLetters.Exists(x => x.Id == randomLetter.Id);
                    } while (randomLetters.MoveNext() && invalid);

                    if (invalid)
                    {
                        Debug.LogError("Error getting valid random letter for balloon!");
                    }
                    else
                    {
                        letter.Init(randomLetter);
                        Debug.Log("Create random balloon with: " + randomLetter.TextForLivingLetter);
                    }
                }

                floatingLetters.Add(floatingLetter);
            }
        }

        private void CreateFloatingLetters_Words()
        {
            var numberOfImages = Mathf.Clamp(correctAnswers.Count() + ExtraAnswers, 0, floatingLetterLocations.Length);
            var correctWord = correctAnswers.Cast<LL_WordData>().ToList()[0];
            var wrongWords = wrongAnswers.Cast<LL_WordData>().GetEnumerator();

            // Determine index of required word
            int requiredWordIndex = Random.Range(0, numberOfImages);

            // Create floating letters
            for (int i = 0; i < numberOfImages; i++)
            {
                var instance = Instantiate(floatingLetterPrefab);
                instance.SetActive(true);
                instance.transform.SetParent(floatingLetterLocations[i]);
                instance.transform.localPosition = Vector3.zero;

                bool isRequiredWord = (i == requiredWordIndex ? true : false);

                var floatingLetter = instance.GetComponent<FloatingLetterController>();

                // Set variation, set at least 2 balloons for required word
                //if (isRequiredWord) {
                floatingLetter.SetActiveVariation(Random.Range(1, floatingLetter.variations.Length));
                //} else {
                //    floatingLetter.SetActiveVariation(Random.Range(0, floatingLetter.variations.Length));
                //}

                var balloons = floatingLetter.Balloons;
                var letter = floatingLetter.Letter;

                // Set random balloon colors without repetition if possible
                var usedColorIndexes = new List<int>();
                for (int j = 0; j < balloons.Length; j++)
                {
                    int randomColorIndex;

                    if (balloons.Length <= balloonColors.Length)
                    {
                        do
                        {
                            randomColorIndex = Random.Range(0, balloonColors.Length);
                        } while (usedColorIndexes.Contains(randomColorIndex));
                    }
                    else
                    {
                        randomColorIndex = Random.Range(0, balloonColors.Length);
                    }

                    usedColorIndexes.Add(randomColorIndex);
                    balloons[j].SetColor(balloonColors[randomColorIndex]);
                }

                // Set images
                if (isRequiredWord)
                {
                    // Set correct word
                    LL_WordData word = correctWord;
                    bool invalid = (word == null);

                    if (invalid)
                    {
                        Debug.LogError("Error getting valid word (correct answer) for balloon!");
                    }
                    letter.isRequired = !HACK_INVERTED_BALLOONS_LOGIC ? true : false;
                    letter.associatedPromptIndex = -1;
                    //letter.letterData.DataType = LivingLetterDataType.Image;
                    letter.Init(new LL_ImageData(word.Id));
                    Debug.Log("Create word balloon with: " + letter.LLPrefab.Data.TextForLivingLetter);
                }
                else
                {
                    // Set a random image
                    LL_WordData randomWord;
                    bool invalid = false;
                    do
                    {
                        randomWord = wrongWords.Current;
                        invalid = (randomWord == null);
                    } while (wrongWords.MoveNext() && invalid);

                    if (invalid)
                    {
                        Debug.LogError("Error getting valid random word (wrong answer) for balloon!");
                    }
                    letter.Init(new LL_ImageData(randomWord.Id));
                    letter.isRequired = !HACK_INVERTED_BALLOONS_LOGIC ? false : true;

                    if (string.IsNullOrEmpty(letter.letterData.DrawingCharForLivingLetter))
                    {
                        Debug.Log("EMPTY DRAWING!!!");
                        letter.Init(new LL_ImageData("dog"));
                    }

                    Debug.Log("Create random balloon with: " + letter.LLPrefab.Data.TextForLivingLetter);
                }

                floatingLetters.Add(floatingLetter);
            }
        }

        public static bool HACK_INVERTED_BALLOONS_LOGIC = true;

        private void CreateFloatingLetters_Letter()
        {
            var numberOfWords = Mathf.Clamp(correctAnswers.Count() + ExtraAnswers, 0, floatingLetterLocations.Length);
            var correctWords = correctAnswers.Cast<LL_WordData>().GetEnumerator();
            var wrongWords = wrongAnswers.Cast<LL_WordData>().GetEnumerator();

            // Determine indices of required words
            var requiredWordIndices = new List<int>();
            for (int i = 0; i < correctAnswers.Count(); i++)
            {
                var index = Random.Range(0, numberOfWords);

                if (!requiredWordIndices.Contains(index))
                {
                    requiredWordIndices.Add(index);
                }
                else
                {
                    i--;
                }
            }

            // Create floating letters
            for (int i = 0; i < numberOfWords; i++)
            {
                var instance = Instantiate(i % 2 == 0 ? floatingLetterPrefab_LetterVariation : floatingLetterPrefab);
                instance.SetActive(true);
                instance.transform.SetParent(floatingLetterLocations[i]);
                instance.transform.localPosition = Vector3.zero;

                int requiredWordIndex = requiredWordIndices.IndexOf(i);
                bool isRequiredWord = requiredWordIndex > -1 ? true : false;

                var floatingLetter = instance.GetComponent<FloatingLetterController>();

                // Set variation, set at least 2 balloons for required words
                //if (isRequiredWord) {
                floatingLetter.SetActiveVariation(Random.Range(1, floatingLetter.variations.Length));
                //} else {
                //    floatingLetter.SetActiveVariation(Random.Range(0, floatingLetter.variations.Length));
                //}

                var balloons = floatingLetter.Balloons;
                var letter = floatingLetter.Letter;

                // Set random balloon colors without repetition if possible
                var usedColorIndexes = new List<int>();
                for (int j = 0; j < balloons.Length; j++)
                {
                    int randomColorIndex;

                    if (balloons.Length <= balloonColors.Length)
                    {
                        do
                        {
                            randomColorIndex = Random.Range(0, balloonColors.Length);
                        } while (usedColorIndexes.Contains(randomColorIndex));
                    }
                    else
                    {
                        randomColorIndex = Random.Range(0, balloonColors.Length);
                    }

                    usedColorIndexes.Add(randomColorIndex);
                    balloons[j].SetColor(balloonColors[randomColorIndex]);
                }

                // Set words
                if (isRequiredWord)
                {
                    // Set correct word
                    LL_WordData word;
                    bool invalid = false;
                    do
                    {
                        word = correctWords.Current;
                        invalid = (word == null);
                    } while (correctWords.MoveNext() && invalid);

                    if (invalid)
                    {
                        Debug.LogError("Error getting valid word (correct answer) for balloon!");
                    }
                    letter.isRequired = !HACK_INVERTED_BALLOONS_LOGIC ? true : false;
                    letter.associatedPromptIndex = -1;
                    letter.Init(word);
                    //Debug.Log("Create word balloon with: " + letter.LLPrefab.Data.TextForLivingLetter);
                    Debug.Log("Create correct balloon with: " + letter.LLPrefab.Data);
                }
                else
                {
                    // Set a random letter that is not a required letter
                    LL_WordData randomWord;
                    bool invalid = false;
                    do
                    {
                        randomWord = wrongWords.Current;
                        invalid = (randomWord == null);
                    } while (wrongWords.MoveNext() && invalid);

                    if (invalid)
                    {
                        Debug.LogError("Error getting valid random word (wrong answer) for balloon!");
                    }
                    letter.isRequired = !HACK_INVERTED_BALLOONS_LOGIC ? false : true;
                    letter.Init(randomWord);
                    //Debug.Log("Create random balloon with: " + letter.LLPrefab.Data.TextForLivingLetter);
                    Debug.Log("Create wrong balloon with: " + letter.LLPrefab.Data);
                }

                floatingLetters.Add(floatingLetter);
            }
        }

        private void CreateFloatingLetters_Counting()
        {
            var numberOfWords = Mathf.Clamp(3 + ExtraAnswers, 0, (floatingLetterLocations.Length <= correctAnswers.Count() ? floatingLetterLocations.Length : correctAnswers.Count()));
            var correctWords = correctAnswers.Cast<LL_WordData>().GetEnumerator();
            maxCountingIndex = numberOfWords - 1;

            // Determine indices of locations to use
            var countingLocationIndices = new List<int>();
            if (isTutorialRound)
            {
                numberOfWords = 3;
                maxCountingIndex = numberOfWords - 1;
                countingLocationIndices = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            }
            else
            {
                for (int i = 0; i < numberOfWords; i++)
                {
                    int index;
                    int attempts = 100;
                    do
                    {
                        index = Random.Range(0, numberOfWords);
                        attempts--;
                        if (attempts <= 0)
                        {
                            Debug.Log("Something wrong happened when choosing locations...");
                        }
                    } while (countingLocationIndices.Contains(index) && attempts > 0);
                    countingLocationIndices.Add(index);
                }
            }

            // Create floating letters
            for (int i = 0; i < numberOfWords; i++)
            {
                var instance = Instantiate(floatingLetterPrefab);
                instance.SetActive(true);
                instance.transform.SetParent(floatingLetterLocations[countingLocationIndices[i]]);
                instance.transform.localPosition = Vector3.zero;

                var floatingLetter = instance.GetComponent<FloatingLetterController>();
                floatingLetter.SetActiveVariation(Random.Range(1, floatingLetter.variations.Length));
                var balloons = floatingLetter.Balloons;
                var letter = floatingLetter.Letter;

                // Set random balloon colors without repetition if possible
                var usedColorIndexes = new List<int>();
                for (int j = 0; j < balloons.Length; j++)
                {
                    int randomColorIndex;

                    if (balloons.Length <= balloonColors.Length)
                    {
                        do
                        {
                            randomColorIndex = Random.Range(0, balloonColors.Length);
                        } while (usedColorIndexes.Contains(randomColorIndex));
                    }
                    else
                    {
                        randomColorIndex = Random.Range(0, balloonColors.Length);
                    }

                    usedColorIndexes.Add(randomColorIndex);
                    balloons[j].SetColor(balloonColors[randomColorIndex]);
                }

                // Set correct word
                LL_WordData word;
                bool invalid = false;
                do
                {
                    word = correctWords.Current;
                    invalid = (word == null);
                } while (correctWords.MoveNext() && invalid);

                if (invalid)
                {
                    Debug.LogError("Error getting valid word (correct answer) for balloon!");
                }
                letter.isRequired = false;
                letter.associatedPromptIndex = -1;
                letter.Init(word);
                //Debug.Log("Create word balloon with: " + letter.LLPrefab.Data.ToString());
                Debug.Log("Create word balloon with: " + (letter.LLPrefab.Data as LL_WordData).Data.ToString());

                //floatingLetter.Disable();
                floatingLetters.Add(floatingLetter);
            }

            //floatingLetters[0].Enable();
        }

        public void OnDroppedLetter(BalloonsLetterController letter = null)
        {
            bool isRequired = false;
            int promptIndex = -1;
            string letterKey = "";

            if (letter != null)
            {
                isRequired = letter.isRequired;
                promptIndex = letter.associatedPromptIndex;
                if (letter.letterData != null && !string.IsNullOrEmpty(letter.letterData.Id))
                {
                    letterKey = letter.letterData.Id;
                }
                lastDroppedLivingLetter = letter.letterData;
            }

            if (isRequired)
            {
                OnDroppedRequiredLetter(promptIndex);
            }
            else
            {
                //
            }

            if (BalloonsConfiguration.Instance.Variation == BalloonsVariation.Counting)
            {
                if (letter.letterData.Id != correctAnswers.ToList()[countingIndex].Id)
                {
                    OnDroppedRequiredLetter(promptIndex);
                }
                else
                {
                    countingIndex++;

                    if (countingIndex <= maxCountingIndex)
                    {
                        SayQuestion();
                        WobbleLetterHint();
                    }
                }
                DisplayWordFlexibleContainer_Counting();
            }

            CheckRemainingBalloons();
        }

        public void OnDroppedRequiredLetter(int promptIndex)
        {
            remainingLives--;
            AudioManager.PlaySound(Sfx.LetterSad);
            if (promptIndex > -1)
            {
                wordPrompt.letterPrompts[promptIndex].State = LetterPromptController.PromptState.WRONG;
            }
            if (wordFlexibleContainer.enabled == true)
            {
                wordFlexibleContainerAnimator.SetBool("Idle", false);
                wordFlexibleContainerAnimator.SetBool("Correct", false);
                wordFlexibleContainerAnimator.SetBool("Wrong", true);
            }

            if (remainingLives <= 0)
            {
                howDied = How2Die.WrongBalloon;
                EndRound(Result.FAIL);
            }
        }

        public void OnPoppedRequiredBalloon(int promptIndex)
        {
            AudioManager.PlaySound(Sfx.KO);
            if (promptIndex > -1)
            {
                wordPrompt.letterPrompts[promptIndex].animator.SetTrigger("Flash");
            }
            if (wordFlexibleContainer.enabled == true)
            {
                wordFlexibleContainerAnimator.SetTrigger("Flash");
            }
        }

        public void OnPoppedNonRequiredBalloon()
        {
            if (wordFlexibleContainer.enabled == true)
            {
                wordFlexibleContainerAnimator.SetTrigger("FlashCorrect");
            }
        }

        private void CheckRemainingBalloons()
        {
            int idlePromptsCount = wordPrompt.IdleLetterPrompts.Count;
            bool randomBalloonsExist = floatingLetters.Exists(balloon => balloon.Letter.isRequired == false);
            bool requiredBalloonsExist = floatingLetters.Exists(balloon => balloon.Letter.isRequired == true);

            switch (BalloonsConfiguration.Instance.Variation)
            {
                case BalloonsVariation.Spelling:
                    if (!requiredBalloonsExist)
                    {
                        EndRound(Result.FAIL);
                    }
                    else if (!randomBalloonsExist)
                    {
                        Result result;
                        if (idlePromptsCount == wordPrompt.activePromptsCount)
                        {
                            result = Result.PERFECT;
                        }
                        else if (idlePromptsCount > 1)
                        {
                            result = Result.GOOD;
                        }
                        else
                        {
                            result = Result.CLEAR;
                        }
                        EndRound(result);
                    }
                    break;

                case BalloonsVariation.LetterInWord:
                case BalloonsVariation.Words:
                case BalloonsVariation.Image:
                    if (!requiredBalloonsExist)
                    {
                        EndRound(Result.FAIL);
                    }
                    else if (!randomBalloonsExist)
                    {
                        EndRound(Result.PERFECT);
                    }
                    break;

                case BalloonsVariation.Counting:
                    if (!randomBalloonsExist)
                    {
                        EndRound(Result.PERFECT);
                    }
                    break;

                default:
                    Debug.LogError("Invalid Balloons Game Variation!");
                    break;
            }
        }

        public void FreezeFloatingLetters()
        {
            for (int i = 0; i < floatingLetters.Count; i++)
            {
                floatingLetters[i].Freeze();
            }
        }

        public void DisableRequiredFloatingLetters()
        {
            for (int i = 0; i < floatingLetters.Count; i++)
            {
                if (floatingLetters[i].Letter.isRequired)
                {
                    floatingLetters[i].Disable();
                }
            }
        }

        private void DisableFloatingLetters()
        {
            for (int i = 0; i < floatingLetters.Count; i++)
            {
                floatingLetters[i].Disable();
                floatingLetters[i].Letter.keepFocusingLetter = true;
            }
        }

        private void DestroyAllFloatingLetters()
        {
            for (int i = 0; i < floatingLetters.Count; i++)
            {
                Destroy(floatingLetters[i].gameObject);
            }
            floatingLetters.Clear();
        }

        private void DestroyRandomFloatingLetters()
        {
            for (int i = 0; i < floatingLetters.Count; i++)
            {
                if (!floatingLetters[i].Letter.isRequired)
                {
                    Destroy(floatingLetters[i]);
                }
            }
        }

        private void MakeWordPromptGreen()
        {
            for (int i = 0; i < wordPrompt.letterPrompts.Length; i++)
            {
                wordPrompt.letterPrompts[i].State = LetterPromptController.PromptState.CORRECT;
                wordFlexibleContainerAnimator.SetBool("Idle", false);
                wordFlexibleContainerAnimator.SetBool("Correct", true);
                wordFlexibleContainerAnimator.SetBool("Wrong", false);
            }
        }

        public void OnTimeUp()
        {
            bool randomBalloonsExist = floatingLetters.Exists(balloon => balloon.Letter.isRequired == false);
            howDied = How2Die.TimeUp;

            if (randomBalloonsExist)
            {
                EndRound(Result.FAIL);
            }
            else
            {
                OnDroppedLetter();
            }
        }

        private bool alreadyClicked = false;
        private void ProcessRoundResult(Result result)
        {
            if (alreadyClicked)
                return;
            alreadyClicked = true;
            bool win = false;

            switch (result)
            {
                case Result.PERFECT:
                case Result.GOOD:
                case Result.CLEAR:
                    if (!isTutorialRound)
                    {
                        CurrentScore++;
                    }
                    win = true;
                    AudioManager.PlaySound(Sfx.Win);
                    break;
                case Result.FAIL:
                    win = false;
                    AudioManager.PlaySound(Sfx.Lose);
                    break;
            }

            DisplayRoundResult(win);
        }

        private void DisplayRoundResult(bool win)
        {
            StartCoroutine(DisplayRoundResult_Coroutine(win));
        }

        private IEnumerator DisplayRoundResult_Coroutine(bool win)
        {
            if (win)
            {
                MakeWordPromptGreen();

                if (isTutorialRound)
                {
                    HideTutorialUI();
                }

                var winInitialDelay = 1f;
                yield return new WaitForSeconds(winInitialDelay);

                ILivingLetterData roundResultData;
                if (BalloonsConfiguration.Instance.Variation == BalloonsVariation.Counting)
                {
                    roundResultData = correctAnswers.ToList()[maxCountingIndex];
                }
                else if (BalloonsConfiguration.Instance.Variation == BalloonsVariation.Words
                         || BalloonsConfiguration.Instance.Variation == BalloonsVariation.Image)
                {
                    roundResultData = new LL_ImageData(question.Id);
                }
                else
                {
                    roundResultData = question;
                }
                roundResultAnimator.ShowWin(roundResultData);

                Context.GetLogManager().OnAnswered(roundResultData, true);

                //var winSpeakWordDelay = 0.75f;
                //yield return new WaitForSeconds(winSpeakWordDelay);
                if (question != null)
                {
                    AudioManager.PlayVocabularyData(question, soundType: BalloonsConfiguration.Instance.GetVocabularySoundType());
                }

                var resumePlayingDelay = 1.5f;
                yield return new WaitForSeconds(resumePlayingDelay);
                if (isTutorialRound)
                {
                    isTutorialRound = false;
                    IntroductionState.OnFinishedTutorial();
                }
                else
                {
                    Play(true);
                }
            }
            else
            {
                var failDelay = 1f;
                yield return new WaitForSeconds(failDelay);

                ILivingLetterData roundResultData;
                if (BalloonsConfiguration.Instance.Variation == BalloonsVariation.Counting)
                {
                    roundResultData = correctAnswers.ToList()[maxCountingIndex];
                }
                else
                {
                    roundResultData = question;
                }

                Context.GetLogManager().OnAnswered(roundResultData, false);

                switch (howDied)
                {
                    case How2Die.TimeUp:
                        //sentence = TextID.TIMES_UP.ToString();
                        //WidgetPopupWindow.I.ShowSentenceWithMark(OnRoundResultPressed, sentence, false, FailTime);
                        //Popup.ShowTimeUp(OnRoundResultPressed);
                        roundResultAnimator.ShowLose(lastDroppedLivingLetter == null ? roundResultData : lastDroppedLivingLetter);
                        break;
                    case How2Die.WrongBalloon:
                        //var sentenceOptions = new[]{ "game_balloons_commentA", "game_balloons_commentB" };
                        //sentence = sentenceOptions[Random.Range(0, sentenceOptions.Length)];
                        //WidgetPopupWindow.I.ShowSentenceWithMark(OnRoundResultPressed, sentence, false, FailWrongBalloon);
                        //Popup.Show();
                        //Popup.SetButtonCallback(OnRoundResultPressed);
                        //Popup.SetMark(true, false);
                        //Popup.SetImage(FailWrongBalloon);
                        //Popup.SetTitle(TextID.GAME_RESULT_RETRY);
                        roundResultAnimator.ShowLose(lastDroppedLivingLetter == null ? roundResultData : lastDroppedLivingLetter);
                        break;
                }

                var resumePlayingDelay = 2f;
                yield return new WaitForSeconds(resumePlayingDelay);
                Play(true);
            }
        }

        public void OnLetterHintClicked()
        {
            if (roundStatus == RoundStatus.Started)
            {
                SayQuestion();
                WobbleLetterHint();
            }
        }

        private void SayQuestion()
        {
            if (BalloonsConfiguration.Instance.Variation == BalloonsVariation.Counting)
            {
                BalloonsConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(correctAnswers[countingIndex], soundType: BalloonsConfiguration.Instance.GetVocabularySoundType());
            }
            else
            {
                BalloonsConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(question, soundType: BalloonsConfiguration.Instance.GetVocabularySoundType());
            }
        }

        private void WobbleLetterHint()
        {
            wordFlexibleContainer.gameObject.transform.DOKill(true);
            wordFlexibleContainer.gameObject.transform.DOShakeScale(0.5f);
        }
    }
}
