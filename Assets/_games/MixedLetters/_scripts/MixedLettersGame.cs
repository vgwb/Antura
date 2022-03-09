using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Language;
using Antura.LivingLetters;
using Antura.Minigames;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.MixedLetters
{
    public class MixedLettersGame : MiniGameController
    {
        public static MixedLettersGame instance;

        #region Score

        public override int MaxScore => STARS_3_THRESHOLD;

        // Stars
        public int STARS_1_THRESHOLD { get { return Mathf.CeilToInt(0.33f * TotalNumRounds); } }
        public int STARS_2_THRESHOLD { get { return Mathf.CeilToInt(0.66f * TotalNumRounds); } }
        public int STARS_3_THRESHOLD { get { return TotalNumRounds; } }

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

        private readonly int[] ALPHABET_PICKING_ORDER = new int[] { 4, 3, 4, 4, 4, 4, 2, 3 };

        public IntroductionGameState IntroductionState { get; private set; }
        public PlayGameState PlayState { get; private set; }
        public ResultGameState ResultState { get; private set; }
        public TutorialGameState TutorialState { get; private set; }

        public DropZoneController[] dropZoneControllers;

        private IQuestionPack spellingQuestionPack;
        private ILivingLetterData question;

        public ILivingLetterData Question
        {
            get
            {
                return question;
            }
        }

        public List<ILivingLetterData> PromptLettersInOrder
        {
            get
            {
                List<ILivingLetterData> _promptLettersInOrder;

                if (isSpelling)
                {
                    _promptLettersInOrder = spellingQuestionPack.GetCorrectAnswers().ToList();
                }
                else
                {
                    int startIndex = 0;

                    for (int i = 0; i < roundNumber; i++)
                    {
                        startIndex += ALPHABET_PICKING_ORDER[i];
                    }

                    // @note: there is a bug here when starting the game with no content, sometimes the picking order will be larger then entireAlphabet.Count
                    // this is because the sample pack provides a pack for the Spelling version but we need to work with the Alphabet. Ignore errors.
                    if (ALPHABET_PICKING_ORDER[roundNumber] > entireAlphabet.Count)
                        Debug.LogError("Error with the Alphabet pack. This appears only when testing the game directly, so retry. See the code for further information.", this);
                    _promptLettersInOrder = entireAlphabet.GetRange(startIndex, ALPHABET_PICKING_ORDER[roundNumber]);
                }

                return _promptLettersInOrder;
            }
        }

        private bool _wasLastRoundWon;

        public bool WasLastRoundWon
        {
            get
            {
                return _wasLastRoundWon;
            }
        }

        public bool IsGameOver
        {
            get
            {
                return roundNumber >= TotalNumRounds;
            }
        }

        public int TotalNumRounds
        {
            get
            {
                if (isSpelling)
                {
                    return 6;
                }

                else
                {
                    return ALPHABET_PICKING_ORDER.Length;
                }
            }
        }

        public List<ILivingLetterData> entireAlphabet;

        public int roundNumber = 0;

        private bool isSpelling = true;
        public bool IsSpelling
        {
            get
            {
                return isSpelling;
            }
        }

        public bool TutorialEnabled
        {
            get { return GetConfiguration().TutorialEnabled; }
        }

        public Button repeatPromptButton;

        protected override void OnInitialize(IGameContext context)
        {
            instance = this;

            IntroductionState = new IntroductionGameState(this);
            PlayState = new PlayGameState(this);
            ResultState = new ResultGameState(this);
            TutorialState = new TutorialGameState(this);

            entireAlphabet = new List<ILivingLetterData>();

            isSpelling = MixedLettersConfiguration.Instance.Variation == MixedLettersVariation.BuildWord;

            if (!isSpelling)
            {
                entireAlphabet = MixedLettersConfiguration.Instance.Questions.GetNextQuestion().GetCorrectAnswers().ToList();
                VictimLLController.instance.SetBigScale();
            }

            Physics.IgnoreLayerCollision(0, 5);
            Physics.IgnoreLayerCollision(12, 11);
            Physics.IgnoreLayerCollision(10, 12);

            ResetScene();

            MixedLettersConfiguration.Instance.Context.GetAudioManager().PlayMusic(Music.Theme9);

            DisableRepeatPromptButton();
        }

        protected override FSM.IState GetInitialState()
        {
            return TutorialState;
        }

        protected override IGameConfiguration GetConfiguration()
        {
            return MixedLettersConfiguration.Instance;
        }

        public void ShowDropZones()
        {
            int numLetters = PromptLettersInOrder.Count;
            bool isEven = numLetters % 2 == 0;
            float dropZoneWidthWithSpace = Constants.DROP_ZONE_WIDTH + 0.6f;
            float dropZoneXStart = isEven ? numLetters / 2 - 0.5f : Mathf.Floor(numLetters / 2);
            dropZoneXStart *= dropZoneWidthWithSpace;

            for (int i = 0; i < numLetters; i++)
            {
                DropZoneController dropZoneController = dropZoneControllers[i];
                dropZoneController.Enable();

                Vector3 dropZonePosition = dropZoneController.transform.position;
                dropZonePosition.x = dropZoneXStart - i * dropZoneWidthWithSpace;
                if (!LanguageSwitcher.LearningRTL)
                    dropZonePosition.x *= -1;
                dropZoneController.SetPosition(dropZonePosition);
            }

            for (int i = numLetters; i < dropZoneControllers.Length; i++)
            {
                dropZoneControllers[i].Disable();
            }
        }

        public void OnRoundStarted()
        {
            _wasLastRoundWon = false;
            ShowDropZones();
            SeparateLettersSpawnerController.instance.SetLettersDraggable();
        }

        public void HideDropZones()
        {
            foreach (DropZoneController dropZoneController in dropZoneControllers)
            {
                dropZoneController.Disable();
            }
        }

        public void HideRotationButtons()
        {
            foreach (DropZoneController dropZoneController in dropZoneControllers)
            {
                dropZoneController.HideRotationButton();
            }
        }

        public void ShowGreenTicks()
        {
            for (int i = 0; i < PromptLettersInOrder.Count; i++)
            {
                dropZoneControllers[i].ShowGreenTick();
            }
        }

        private void ResetDropZones()
        {
            foreach (DropZoneController dropZoneController in dropZoneControllers)
            {
                dropZoneController.Reset();
            }
        }

        public void ResetScene()
        {
            ResetDropZones();
            HideDropZones();
            DropZoneController.chosenDropZone = null;
            SeparateLettersSpawnerController.instance.ResetLetters();
            SeparateLettersSpawnerController.instance.DisableLetters();

            ParticleSystemController.instance.Reset();
            ParticleSystemController.instance.Disable();
            AnturaController.instance.Disable();
        }

        public void GenerateNewWord()
        {
            if (isSpelling)
            {
                IQuestionPack newQuestionPack = MixedLettersConfiguration.Instance.Questions.GetNextQuestion();
                spellingQuestionPack = newQuestionPack;
                question = newQuestionPack.GetQuestion();

                VictimLLController.instance.letterObjectView.Init(question);
            }

            else
            {
                VictimLLController.instance.letterObjectView.Init(null);

                string victimLLWord = "";

                for (int i = 0; i < PromptLettersInOrder.Count; i++)
                {
                    victimLLWord += ((LL_LetterData)PromptLettersInOrder[i]).Data.GetStringForDisplay();

                    if (i != PromptLettersInOrder.Count - 1)
                    {
                        victimLLWord += " ";
                    }
                }

                VictimLLController.instance.SetCustomText(victimLLWord);
            }
        }

        public void SayQuestion()
        {
            SayQuestion(null);
        }

        public void SayQuestion(Action onQuestionOver)
        {
            if (MixedLettersConfiguration.Instance.Variation == MixedLettersVariation.BuildWord)
            {
                MixedLettersConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(question, soundType: MixedLettersConfiguration.Instance.GetVocabularySoundType());

                if (onQuestionOver != null)
                {
                    onQuestionOver.Invoke();
                }
            }

            else
            {
                StartCoroutine(AlphabetPronounciationCoroutine(onQuestionOver));
            }
        }

        private IEnumerator AlphabetPronounciationCoroutine(Action onQuestionOver)
        {
            IAudioManager audioManager = MixedLettersConfiguration.Instance.Context.GetAudioManager();

            foreach (ILivingLetterData letterData in PromptLettersInOrder)
            {
                audioManager.PlayVocabularyData(letterData, soundType: MixedLettersConfiguration.Instance.GetVocabularySoundType());

                yield return new WaitForSeconds(0.75f);
            }

            if (onQuestionOver != null)
            {
                onQuestionOver.Invoke();
            }
        }



        public void VerifyLetters()
        {
            bool isValid = true;

            for (int i = 0; i < PromptLettersInOrder.Count; i++)
            {
                DropZoneController dropZone = dropZoneControllers[i];

                if (dropZone.droppedLetter == null
                    || !MixedLettersConfiguration.Instance.IsDataMatching(dropZone.droppedLetter.GetLetter(), PromptLettersInOrder[i])
                      || Mathf.Abs(dropZone.droppedLetter.transform.rotation.z) > 0.1f)
                {
                    if (isValid)
                        for (int j = 0; j < PromptLettersInOrder.Count; j++)
                        {
                            SeparateLetterController letter = SeparateLettersSpawnerController.instance.separateLetterControllers[j];
                            letter.SetIsSubjectOfTutorial(
                                roundNumber == 0 && TutorialEnabled
                                && letter == dropZone.correctLetter);
                        }

                    isValid = false;
                }
                else
                {
                    if (dropZone.gameObject.activeInHierarchy)
                    {
                        dropZone.droppedLetter.DisableCollider();
                        dropZone.Disable();
                        dropZone.DisableCollider();
                        dropZone.ShowGreenTick();
                    }
                }
            }

            if (isValid)
            {
                for (int i = 0; i < PromptLettersInOrder.Count; i++)
                    dropZoneControllers[i].droppedLetter.EnableCollider();

                OnRoundWon();
            }
        }

        private void OnRoundWon()
        {
            _wasLastRoundWon = true;

            CurrentScore++;

            HideRotationButtons();
            ShowGreenTicks();
        }

        public void EnableRepeatPromptButton()
        {
            repeatPromptButton.gameObject.SetActive(true);
        }

        public void DisableRepeatPromptButton()
        {
            repeatPromptButton.gameObject.SetActive(false);
        }
    }
}
