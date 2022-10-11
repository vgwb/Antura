using System;
using System.Collections;
using System.Collections.Generic;
using Antura.UI;
using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.UI;
using Antura.Core;
using Antura.Database;
using Antura.Minigames;
using TMPro;

namespace Antura.Teacher.Test
{
    public enum QuestionBuilderType
    {
        Empty,

        RandomLetters,
        RandomLetterForms,
        Alphabet,
        LettersBySunMoon,
        LettersByType,

        RandomWords,
        OrderedWords,
        WordsByArticle,
        WordsByForm,
        WordsBySunMoon,

        LettersInWord,
        LetterFormsInWords,
        LetterAlterationsInWords,
        CommonLettersInWords,
        WordsWithLetter,

        WordsInPhrase,
        PhraseQuestions,

        MAX
    }

    /// <summary>
    /// Helper class to test Teacher functionality regardless of minigames.
    /// </summary>
    public class TeacherTester : MonoBehaviour
    {
        [DeBeginGroup]
        [Header("Reporting")]
        [DeToggleButton(DePosition.HHalfLeft)]
        public bool verboseQuestionPacks = false;
        [DeToggleButton(DePosition.HHalfRight)]
        public bool verboseDataSelection = false;
        [DeToggleButton(DePosition.HHalfLeft)]
        public bool verboseDataFiltering = false;
        [DeEndGroup]
        [DeToggleButton(DePosition.HHalfRight)]
        public bool verbosePlaySessionInitialisation = false;
        [DeToggleButton(DePosition.HHalfLeft)]
        public bool PrintReport = false;

        [DeBeginGroup]
        [Header("Simulation")]
        public int numberOfSimulations = 50;
        public int attemptsPerSimulation = 10;
        [DeEndGroup]

        // Current options
        [DeBeginGroup]
        [Header("Journey")]
        [DeToggleButton()]
        public bool simulateInsideJourney = true;
        [DeToggleButton()]
        public bool ignoreJourneyPlaySessionSelection = false;
        [Range(1, 6)]
        public int currentJourneyStage = 1;
        [Range(1, 15)]
        public int currentJourneyLB = 1;
        [DeToggleButton()]
        [DeEndGroup]
        public bool isAssessment = false;
        [DeToggleButton()]
        public bool isRecap = false;

        [DeToggleButton()]
        public bool testWholeJourneyAtButtonClick = false;

        [DeBeginGroup]
        [Header("Selection Parameters")]
        [Range(1, 10)]
        public int nPacks = 5;
        [Range(1, 10)]
        public int nPacksPerRound = 2;
        [DeToggleButton()]
        public bool sortPacksByDifficulty = true;

        [Range(1, 10)]
        public int nCorrectAnswers = 1;
        public SelectionSeverity correctSeverity = SelectionSeverity.MayRepeatIfNotEnough;
        public PackListHistory correctHistory = PackListHistory.RepeatWhenFull;
        [DeToggleButton()]
        public bool journeyEnabledForBase = true;

        [Range(0, 10)]
        public int nWrongAnswers = 1;
        public SelectionSeverity wrongSeverity = SelectionSeverity.MayRepeatIfNotEnough;
        public PackListHistory wrongHistory = PackListHistory.RepeatWhenFull;
        [DeEndGroup]
        [DeToggleButton()]
        public bool journeyEnabledForWrong = true;

        [HideInInspector]
        public InputField journey_stage_in;
        [HideInInspector]
        public InputField journey_learningblock_in;
        [HideInInspector]
        public InputField journey_playsession_in;
        [HideInInspector]
        public InputField npacks_in;
        [HideInInspector]
        public InputField ncorrect_in;
        [HideInInspector]
        public InputField nwrong_in;
        [HideInInspector]
        public Dropdown severity_in;
        [HideInInspector]
        public Dropdown severitywrong_in;
        [HideInInspector]
        public Dropdown history_in;
        [HideInInspector]
        public Dropdown historywrong_in;
        [HideInInspector]
        public Toggle journeybase_in;
        [HideInInspector]
        public Toggle journeywrong_in;

        [HideInInspector]
        public Dictionary<MiniGameCode, Button> minigamesButtonsDict = new Dictionary<MiniGameCode, Button>();
        [HideInInspector]
        public Dictionary<QuestionBuilderType, Button> qbButtonsDict = new Dictionary<QuestionBuilderType, Button>();

        void Start()
        {
            // Setup for testing
            Application.runInBackground = true;
            DebugConfig.I.VerboseTeacher = true;
            ConfigAI.ForceJourneyIgnore = false;

            /*
            journey_stage_in.onValueChanged.AddListener(x => { currentJourneyStage = int.Parse(x); });
            journey_learningblock_in.onValueChanged.AddListener(x => { currentJourneyLB = int.Parse(x); });
            journey_playsession_in.onValueChanged.AddListener(x => { currentJourneyPS = int.Parse(x); });

            npacks_in.onValueChanged.AddListener(x => { nPacks = int.Parse(x); });
            ncorrect_in.onValueChanged.AddListener(x => { nCorrectAnswers = int.Parse(x); });
            nwrong_in.onValueChanged.AddListener(x => { nWrongAnswers = int.Parse(x); });

            severity_in.onValueChanged.AddListener(x => { correctSeverity = (SelectionSeverity)x; });
            severitywrong_in.onValueChanged.AddListener(x => { wrongSeverity = (SelectionSeverity)x; });

            history_in.onValueChanged.AddListener(x => { correctHistory = (PackListHistory)x; });
            historywrong_in.onValueChanged.AddListener(x => { wrongHistory = (PackListHistory)x; });

            journeybase_in.onValueChanged.AddListener(x => { journeyEnabledForBase = x; });
            journeywrong_in.onValueChanged.AddListener(x => { journeyEnabledForWrong = x; });
            */

            GlobalUI.ShowPauseMenu(false);
        }

        private void InitialisePlaySession(JourneyPosition jp = null)
        {
            if (jp == null) {
                jp = new JourneyPosition(currentJourneyStage, currentJourneyLB, 1);
                if (isAssessment) jp.PlaySession = JourneyPosition.ASSESSMENT_PLAY_SESSION_INDEX;
                if (isRecap) jp.PlaySession = JourneyPosition.RECAP_PLAY_SESSION_INDEX;
            }
            AppManager.I.Player.CurrentJourneyPosition.SetPosition(jp.Stage, jp.LearningBlock, jp.PlaySession);
            AppManager.I.Teacher.SetPlayerProfile(AppManager.I.Player);
            AppManager.I.Teacher.InitNewPlaySession();
        }

        void SetVerboseAI(bool choice)
        {
            DebugConfig.I.VerboseTeacher = choice;
        }

        #region Testing API

        void ApplyParameters()
        {
            DebugConfig.I.VerboseQuestionPacks = verboseQuestionPacks;
            DebugConfig.I.VerboseDataFiltering = verboseDataFiltering;
            DebugConfig.I.VerboseDataSelection = verboseDataSelection;
            DebugConfig.I.VerbosePlaySessionInitialisation = verbosePlaySessionInitialisation;
        }

        private bool IsCodeValid(MiniGameCode code)
        {
            bool isValid = true;
            switch (code) {
                case MiniGameCode.Invalid:
                case MiniGameCode.Assessment_VowelOrConsonant:
                    isValid = false;
                    break;
            }
            return isValid;
        }

        private bool IsCodeValid(QuestionBuilderType code)
        {
            bool isValid = true;
            switch (code) {
                case QuestionBuilderType.Empty:
                case QuestionBuilderType.MAX:
                    isValid = false;
                    break;
            }
            return isValid;
        }

        [DeMethodButton("Cleanup")]
        public void DoCleanup()
        {
            foreach (var code in Helpers.GenericHelper.SortEnums<QuestionBuilderType>()) {
                if (!IsCodeValid(code)) continue;
                SetButtonStatus(qbButtonsDict[code], Color.white);
            }

            foreach (var code in Helpers.GenericHelper.SortEnums<MiniGameCode>()) {
                if (!IsCodeValid(code)) continue;
                SetButtonStatus(minigamesButtonsDict[code], Color.white);
            }
        }

        [DeMethodButton("Test Minimum Journey")]
        public void DoTestMinimumJourney()
        {
            StartCoroutine(DoTest(() => DoTestMinimumJourneyCO()));
        }
        private IEnumerator DoTestMinimumJourneyCO()
        {
            // Test all minigames at their minimum journey
            foreach (var code in Helpers.GenericHelper.SortEnums<MiniGameCode>()) {
                if (!IsCodeValid(code)) continue;
                var jp = AppManager.I.JourneyHelper.GetMinimumJourneyPositionForMiniGame(code);
                if (jp == null) jp = AppManager.I.JourneyHelper.GetFinalJourneyPosition();
                InitialisePlaySession(jp);
                yield return DoTestMinigameCO(code);
            }
        }

        [DeMethodButton("Test Complete Journey")]
        public void DoTestCompleteJourney()
        {
            StartCoroutine(DoTest(() => DoTestCompleteJourneyCO()));
        }

        public bool IsExecuting { get; set; }

        private IEnumerator DoTestCompleteJourneyCO()
        {
            IsExecuting = true;
            // Test all minigames at all their available journeys. Stop when we find a wrong one.
            foreach (var code in Helpers.GenericHelper.SortEnums<MiniGameCode>()) {
                if (!IsCodeValid(code)) continue;
                yield return DoTestMinigameWholeJourneyCO(code);
            }
            IsExecuting = false;
        }

        [DeMethodButton("Test Everything (current PS)")]
        public void DoTestEverything()
        {
            StartCoroutine(DoTest(() => DoTestEverythingCO()));
        }
        private IEnumerator DoTestEverythingCO()
        {
            yield return DoTestAllMiniGamesCO();
            yield return DoTestAllQuestionBuildersCO();
        }

        [DeMethodButton("Test Minigames (current PS)")]
        public void DoTestAllMiniGames()
        {
            StartCoroutine(DoTest(() => DoTestAllMiniGamesCO()));
        }
        private IEnumerator DoTestAllMiniGamesCO()
        {
            foreach (var code in Helpers.GenericHelper.SortEnums<MiniGameCode>()) {
                if (!IsCodeValid(code)) continue;
                yield return DoTestMinigameCO(code);
            }
        }

        [DeMethodButton("Test QuestionBuilders (current PS)")]
        public void DoTestAllQuestionBuilders()
        {
            StartCoroutine(DoTest(() => DoTestAllQuestionBuildersCO()));
        }
        private IEnumerator DoTestAllQuestionBuildersCO()
        {
            foreach (var type in Helpers.GenericHelper.SortEnums<QuestionBuilderType>()) {
                if (!IsCodeValid(type)) continue;
                yield return DoTestQuestionBuilderCO(type);
            }
        }

        public void DoTestQuestionBuilder(QuestionBuilderType type)
        {
            StartCoroutine(DoTest(() => DoTestQuestionBuilderCO(type)));
        }
        private IEnumerator DoTestQuestionBuilderCO(QuestionBuilderType type)
        {
            SetButtonStatus(qbButtonsDict[type], Color.yellow);
            yield return new WaitForSeconds(0.1f);
            var statusColor = Color.green;
            try {
                SimulateQuestionBuilder(type);
            } catch (Exception e) {
                Debug.LogError($"!! {type}\n {e.Message}");
                statusColor = Color.red;
            }
            SetButtonStatus(qbButtonsDict[type], statusColor);
            yield return null;
        }

        public void DoTestMinigame(MiniGameCode code)
        {
            if (testWholeJourneyAtButtonClick) {
                StartCoroutine(DoTest(() => DoTestMinigameWholeJourneyCO(code)));
            } else {
                StartCoroutine(DoTest(() => DoTestMinigameCO(code)));
            }
        }

        private IEnumerator DoTestMinigameWholeJourneyCO(MiniGameCode code)
        {
            SetButtonStatus(minigamesButtonsDict[code], Color.yellow);
            int lastStage = 0;
            bool isCorrect = true;
            bool atLeastOneFound = false;
            foreach (var psData in AppManager.I.DB.GetAllPlaySessionData())
            {
                if (!AppManager.I.Teacher.CanMiniGameBePlayedAtPlaySession(psData.GetJourneyPosition(), code)) continue;
                atLeastOneFound = true;

                InitialisePlaySession(psData.GetJourneyPosition());

                // Log
                Debug.Log($"Testing {code} at ps {psData.GetJourneyPosition()}");
                if (psData.Stage != lastStage) {
                    lastStage = psData.Stage;
                }

                // Skip minigames that found errors
                var result = new Ref<bool>();
                yield return DoTestMinigameCO(code, 0.01f, result);
                if (result.v == false) {
                    Debug.LogError($"Minigame {code} first wrong at ps {psData.GetJourneyPosition()}");
                    isCorrect = false;
                    break;
                }

                yield return null;
            }

            if (!atLeastOneFound)
            {
                Debug.Log($"Minigame {code} is never found in journey position");
                SetButtonStatus(minigamesButtonsDict[code], Color.magenta);
            }
            else if (isCorrect)
            {
                Debug.Log($"Minigame {code} is always fine");
                SetButtonStatus(minigamesButtonsDict[code], Color.green);
            }
            else
            {
                SetButtonStatus(minigamesButtonsDict[code], Color.red);
            }
        }

        private class Ref<T>
        {
            public T v;
        }

        private IEnumerator DoTestMinigameCO(MiniGameCode code, float delay = 0.1f, Ref<bool> result = default)
        {
            var originalText = minigamesButtonsDict[code].GetComponentInChildren<Text>().text;
            SetButtonStatus(minigamesButtonsDict[code], Color.yellow);
            minigamesButtonsDict[code].GetComponentInChildren<Text>().text += "\n" + AppManager.I.Player.CurrentJourneyPosition.ToString();
            yield return new WaitForSeconds(delay);
            var statusColor = Color.green;
            if (result != null) result.v = true;


            if (!ignoreJourneyPlaySessionSelection && !AppManager.I.Teacher.CanMiniGameBePlayedAtAnyPlaySession(code)) {
                Debug.LogError($"Cannot select {code} for any journey position!");
                statusColor = Color.magenta;
            } else {
                if (ignoreJourneyPlaySessionSelection || AppManager.I.Teacher.CanMiniGameBePlayedAfterMinPlaySession(AppManager.I.Player.CurrentJourneyPosition, code)) {
                    try {
                        SimulateMiniGame(code);
                    } catch (Exception e) {
                        Debug.LogError($"!! {code} at PS({AppManager.I.Player.CurrentJourneyPosition})\n {e.Message}");
                        statusColor = Color.red;
                        if (result != null) result.v = false;
                    }
                } else {
                    Debug.LogError($"Cannot select {code} for position {AppManager.I.Player.CurrentJourneyPosition}");
                    statusColor = Color.gray;
                }
            }

            SetButtonStatus(minigamesButtonsDict[code], statusColor);
            minigamesButtonsDict[code].GetComponentInChildren<Text>().text = originalText;
            yield return null;
        }

        private void SetButtonStatus(Button button, Color statusColor)
        {
            button.image.color = statusColor;
        }

        private IEnumerator DoTest(Func<IEnumerator> CoroutineFunc)
        {
            if (PrintReport) ConfigAI.StartTeacherReport();
            ApplyParameters();
            InitialisePlaySession();
            for (int i = 1; i <= numberOfSimulations; i++) {
                Debug.Log($"************ Simulation {i} ************");
                if (PrintReport) ConfigAI.AppendToTeacherReport($"************ Simulation {i} ************");
                yield return CoroutineFunc();
                yield return null;
            }
            if (PrintReport) ConfigAI.PrintTeacherReport();
        }

        #endregion


        #region Minigames Simulation

        MinigameLaunchConfiguration minigameLaunchConfig = new MinigameLaunchConfiguration();
        private void SimulateMiniGame(MiniGameCode code)
        {
            minigameLaunchConfig.NumberOfRounds = nPacks;
            minigameLaunchConfig.InsideJourney = simulateInsideJourney;
            minigameLaunchConfig.DirectGame = false;

            var config = AppManager.I.GameLauncher.ConfigureMiniGameScene(code, System.DateTime.Now.Ticks.ToString(), minigameLaunchConfig);
            var builder = config.SetupBuilder();
            if (PrintReport) ConfigAI.AppendToTeacherReport($"** Minigame {code} - {builder.GetType().Name} PS: {AppManager.I.Player.CurrentJourneyPosition}");

            var questionPacksGenerator = new QuestionPacksGenerator();
            questionPacksGenerator.GenerateQuestionPacks(builder, attemptsPerSimulation);
        }

        #endregion

        #region  QuestionBuilder Simulation

        public int lettersVariationChoice = 0;

        private void SimulateQuestionBuilder(QuestionBuilderType builderType)
        {

            LetterAlterationFilters letterAlterationFilters = null;
            switch (lettersVariationChoice) {
                case 0:
                    letterAlterationFilters = LetterAlterationFilters.FormsOfSingleLetter;
                    break;
                case 1:
                    letterAlterationFilters = LetterAlterationFilters.FormsOfMultipleLetters;
                    break;
                case 2:
                    letterAlterationFilters = LetterAlterationFilters.MultipleLetters;
                    break;
                case 3:
                    letterAlterationFilters = LetterAlterationFilters.PhonemesOfSingleLetter;
                    break;
                case 4:
                    letterAlterationFilters = LetterAlterationFilters.PhonemesOfMultipleLetters;
                    break;
                case 5:
                    letterAlterationFilters = LetterAlterationFilters.FormsAndPhonemesOfMultipleLetters;
                    break;
            }


            var builderParams = SetupBuilderParameters();
            IQuestionBuilder builder = null;
            switch (builderType) {
                case QuestionBuilderType.RandomLetters:
                    builder = new RandomLettersQuestionBuilder(nPacks: nPacks, nCorrect: nCorrectAnswers, nWrong: nWrongAnswers, firstCorrectIsQuestion: true, parameters: builderParams);
                    break;
                case QuestionBuilderType.RandomLetterForms:
                    builder = new RandomLetterAlterationsQuestionBuilder(nPacks: nPacks, nCorrect: nCorrectAnswers, nWrong: nWrongAnswers, letterAlterationFilters: letterAlterationFilters, parameters: builderParams);
                    break;
                case QuestionBuilderType.Alphabet:
                    builder = new AlphabetQuestionBuilder(parameters: builderParams);
                    break;
                case QuestionBuilderType.LettersBySunMoon:
                    builder = new LettersBySunMoonQuestionBuilder(nPacks: nPacks, parameters: builderParams);
                    break;
                case QuestionBuilderType.LettersByType:
                    builder = new LettersByTypeQuestionBuilder(nPacks: nPacks, parameters: builderParams);
                    break;
                case QuestionBuilderType.LettersInWord:
                    builder = new LettersInWordQuestionBuilder(nRounds: nPacks, nPacksPerRound: nPacksPerRound, nCorrect: nCorrectAnswers, nWrong: nWrongAnswers, useAllCorrectLetters: true, parameters: builderParams);
                    break;
                case QuestionBuilderType.LetterFormsInWords:
                    builder = new LetterFormsInWordsQuestionBuilder(nPacks, nPacksPerRound, parameters: builderParams);
                    break;
                case QuestionBuilderType.LetterAlterationsInWords:
                    builder = new LetterAlterationsInWordsQuestionBuilder(nPacks, nPacksPerRound, parameters: builderParams, letterAlterationFilters: letterAlterationFilters);
                    break;
                case QuestionBuilderType.CommonLettersInWords:
                    builder = new CommonLetterInWordQuestionBuilder(nPacks: nPacks, nWrong: nWrongAnswers, parameters: builderParams);
                    break;
                case QuestionBuilderType.RandomWords:
                    builder = new RandomWordsQuestionBuilder(nPacks: nPacks, nCorrect: nCorrectAnswers, nWrong: nWrongAnswers, firstCorrectIsQuestion: true, parameters: builderParams);
                    break;
                case QuestionBuilderType.OrderedWords:
                    builderParams.wordFilters.allowedCategories = new[] { WordDataCategory.Numbers };
                    builder = new OrderedWordsQuestionBuilder(parameters: builderParams);
                    break;
                case QuestionBuilderType.WordsWithLetter:
                    builder = new WordsWithLetterQuestionBuilder(nRounds: nPacks, nPacksPerRound: nPacksPerRound, nCorrect: nCorrectAnswers, nWrong: nWrongAnswers, parameters: builderParams);
                    break;
                case QuestionBuilderType.WordsByForm:
                    builder = new WordsByFormQuestionBuilder(nPacks: nPacks, parameters: builderParams);
                    break;
                case QuestionBuilderType.WordsByArticle:
                    builder = new WordsByArticleQuestionBuilder(nPacks: nPacks, parameters: builderParams);
                    break;
                case QuestionBuilderType.WordsBySunMoon:
                    builder = new WordsBySunMoonQuestionBuilder(nPacks: nPacks, parameters: builderParams);
                    break;
                case QuestionBuilderType.WordsInPhrase:
                    builder = new WordsInPhraseQuestionBuilder(nPacks: nPacks, nCorrect: nCorrectAnswers, nWrong: nWrongAnswers, useAllCorrectWords: false, usePhraseAnswersIfFound: true, parameters: builderParams);
                    break;
                case QuestionBuilderType.PhraseQuestions:
                    builder = new PhraseQuestionsQuestionBuilder(nPacks: nPacks, nWrong: nWrongAnswers, parameters: builderParams);
                    break;
            }

            var questionPacksGenerator = new QuestionPacksGenerator();
            questionPacksGenerator.GenerateQuestionPacks(builder);
        }

        QuestionBuilderParameters SetupBuilderParameters()
        {
            var builderParams = new QuestionBuilderParameters();
            builderParams.correctChoicesHistory = correctHistory;
            builderParams.wrongChoicesHistory = wrongHistory;
            builderParams.correctSeverity = correctSeverity;
            builderParams.wrongSeverity = wrongSeverity;
            builderParams.useJourneyForCorrect = journeyEnabledForBase;
            builderParams.useJourneyForWrong = journeyEnabledForWrong;
            builderParams.sortPacksByDifficulty = sortPacksByDifficulty;
            return builderParams;
        }

        #endregion

    }

}
