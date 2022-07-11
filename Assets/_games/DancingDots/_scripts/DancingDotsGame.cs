using Antura.Dog;
using Antura.Audio;
using Antura.Keeper;
using Antura.LivingLetters;
using Antura.Minigames;
using Antura.Tutorial;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using System.Linq;
using Antura.Core;
using Antura.Language;

namespace Antura.Minigames.DancingDots
{
    // @todo: move these somewhere accessible by the games, but in the DATA
    public enum DiacriticEnum { None, Sokoun, Fatha, Dameh, Kasrah };

    public class DancingDotsGame : MiniGameController
    {
        public IntroductionGameState IntroductionState { get; private set; }
        public QuestionGameState QuestionState { get; private set; }
        public PlayGameState PlayState { get; private set; }
        public ResultGameState ResultState { get; private set; }

        public bool TutorialEnabled => GetConfiguration().TutorialEnabled;

        protected override void OnInitialize(IGameContext context)
        {
            IntroductionState = new IntroductionGameState(this);
            QuestionState = new QuestionGameState(this);
            PlayState = new PlayGameState(this);
            ResultState = new ResultGameState(this);
        }

        protected override FSM.IState GetInitialState()
        {
            return IntroductionState;
        }

        protected override IGameConfiguration GetConfiguration()
        {
            return DancingDotsConfiguration.Instance;
        }

        public const string DANCING_DOTS = "DancingDots_DotZone";
        public const string DANCING_DIACRITICS = "DancingDots_Diacritic";

        public bool disableInput;
        public float gameDuration = 60f;
        public DancingDotsLivingLetter dancingDotsLL;
        public GameObject antura;
        public float anturaMinDelay = 3f;
        public float anturaMaxDelay = 10f;
        public float anturaMinScreenTime = 1f;
        public float anturaMaxScreenTime = 2f;
        public GameObject[] diacritics;
        public SpriteRenderer[] DDBackgrounds;
        public DancingDotsDiacriticPosition activeDiacritic;

        [HideInInspector]
        public DancingDotsQuestionsManager questionsManager;

        [HideInInspector]
        public DiacriticEnum letterDiacritic;


        DancingDotsTutorial tutorial;

        public bool isTutRound
        {
            get
            {
                if (numberOfRoundsPlayed == 0 && GetConfiguration().TutorialEnabled)
                    return true;
                else
                    return false;
            }
        }

        public string currentLetter = "";
        private int _dotsCount;
        public int dotsCount
        {
            get
            {
                return _dotsCount;
            }
            set
            {
                _dotsCount = value;
                foreach (DancingDotsDraggableDot dd in dragableDots)
                {
                    dd.isCorrect = dd.dots == _dotsCount;
                }
            }
        }

        public GameObject splatPrefab;
        public Transform splatParent;

        [Range(0, 255)]
        public byte dotHintAlpha = 60;
        [Range(0, 255)]
        public byte diacriticHintAlpha = 60;

        public float hintDotDuration = 2.5f;
        public float hintDiacriticDuration = 3f;

        public GameObject poofPrefab;

        [Range(0, 1)]
        public float pedagogicalLevel = 0;

        public int numberOfRounds = 6;

        public int allowedFailedMoves = 3;

        public DancingDotsDraggableDot[] dragableDots;
        public DancingDotsDraggableDot[] dragableDiacritics;
        public DancingDotsDropZone[] dropZones;

        private bool isCorrectDot = false;
        private bool isCorrectDiacritic = false;

        private int numberOfRoundsPlayed = -1;
        private int numberOfFailedMoves = 0;

        #region Score

        public override int MaxScore => STARS_3_THRESHOLD;

        // Stars
        const int STARS_1_THRESHOLD = 2;
        const int STARS_2_THRESHOLD = 4;
        const int STARS_3_THRESHOLD = 6;

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

        enum Level { Level1, Level2, Level3, Level4, Level5, Level6 };

        private Level currentLevel = Level.Level4;
        private List<DancingDotsSplat> splats;
        private bool isPlaying = false;
        //private bool wonLastRound = false;

        protected override void Start()
        {
            base.Start();
            tutorial = GetComponent<DancingDotsTutorial>();

            var source = AudioManager.I.PlayMusic(Music.MainTheme);
            //AudioManager.I.transform.FindChild("Music").gameObject.AddComponent<AudioProcessor>();
            var beatDetection = gameObject.AddComponent<DancingDotsBeatDetection>();
            beatDetection.Initialize(source as AudioSourceWrapper);

            //StartCoroutine(beat());

            questionsManager = new DancingDotsQuestionsManager();

            splats = new List<DancingDotsSplat>();

            if (AppManager.I.ContentEdition.LearningLanguage == LanguageCode.persian_dari)
            {
                dragableDiacritics.First(x => x.diacritic == DiacriticEnum.Sokoun).gameObject.SetActive(false);
                dragableDiacritics = dragableDiacritics.ToList().Where(x => x.diacritic != DiacriticEnum.Sokoun).ToArray();
                diacritics.First(x => x.GetComponent<DancingDotsDiacriticPosition>().diacritic == DiacriticEnum.Sokoun).SetActive(false);
                diacritics = diacritics.ToList().Where(x => x.GetComponent<DancingDotsDiacriticPosition>().diacritic != DiacriticEnum.Sokoun).ToArray();
            }

            foreach (DancingDotsDraggableDot dDots in dragableDots)
                dDots.gameObject.SetActive(false);
            foreach (DancingDotsDraggableDot dDiacritic in dragableDiacritics)
                dDiacritic.gameObject.SetActive(false);

            //StartRound();

            isPlaying = true;


            //			StartCoroutine(AnimateAntura());


        }

        public Color32 SetAlpha(Color32 color, byte alpha)
        {
            if (alpha >= 0 && alpha <= 255)
            {
                return new Color32(color.r, color.g, color.b, alpha);
            }
            else
            {
                return color;
            }
        }

        IEnumerator AnimateAntura()
        {
            Vector3 pos = antura.transform.position;
            // Move antura off screen because SetActive is reseting the animation to running
            antura.transform.position = new Vector3(-50, pos.y, pos.z);
            do
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(anturaMinDelay, anturaMaxDelay));
                CreatePoof(pos, 2f, false);
                yield return new WaitForSeconds(0.4f);
                antura.transform.position = pos;
                antura.GetComponent<AnturaAnimationController>().DoSniff();
                yield return new WaitForSeconds(UnityEngine.Random.Range(anturaMinScreenTime, anturaMaxScreenTime));
                CreatePoof(pos, 2f, false);
                antura.transform.position = new Vector3(-50, pos.y, pos.z);
            } while (isPlaying);

        }


        private void SetLevel(Level level)
        {
            // Hide all dots
            foreach (DancingDotsDraggableDot dDots in dragableDots)
                dDots.gameObject.SetActive(false);
            foreach (DancingDotsDraggableDot dDiacritic in dragableDiacritics)
                dDiacritic.gameObject.SetActive(false);
            foreach (GameObject go in diacritics)
                go.SetActive(false);
            isCorrectDiacritic = true;
            isCorrectDot = true;

            // HACK: removed dots gameplay
            //foreach (DancingDotsDraggableDot dDots in dragableDots) dDots.Reset();
            //isCorrectDot = false;

            switch (level)
            {
                case Level.Level1: // Dots alone with visual aid
                    gameDuration = 120;
                    StartCoroutine(RemoveHintDot());
                    break;

                case Level.Level2: // Diacritics alone with visual aid
                    gameDuration = 110;
                    //StartCoroutine(RemoveHintDot());  // HACK: removed hint removal!
                    break;

                case Level.Level3: // Dots and diacritics with visual aid
                    gameDuration = 100;

                    StartCoroutine(RemoveHintDot());
                    break;

                case Level.Level4: // Dots alone without visual aid
                    gameDuration = 90;
                    dancingDotsLL.HideText(dancingDotsLL.hintText);
                    break;

                case Level.Level5: // Diacritics alone without visual aid
                    gameDuration = 90;
                    dancingDotsLL.HideText(dancingDotsLL.hintText);
                    break;

                case Level.Level6: // Dots and diacritics without visual aid
                    gameDuration = 80;
                    dancingDotsLL.HideText(dancingDotsLL.hintText);
                    break;

                default:
                    SetLevel(Level.Level1);
                    break;

            }
        }

        public void StartRound()
        {

            numberOfRoundsPlayed++;

            dancingDotsLL.letterObjectView.SetDancingSpeed(StartingRoundDancingSpeed);

            if (splats != null)
                splats.Clear();

            dancingDotsLL.HideRainbow();

            Debug.Log("[Dancing Dots] Round: " + numberOfRoundsPlayed);
            numberOfFailedMoves = 0;

            if (pedagogicalLevel == 0f) // TODO for testing only each round increment Level. Remove later!
            {
                switch (numberOfRoundsPlayed)
                {
                    case 1:
                    case 2:
                        currentLevel = Level.Level1;
                        break;
                    case 3:
                        currentLevel = Level.Level4;
                        break;
                    case 4:
                        currentLevel = Level.Level2;
                        break;
                    case 5:
                    case 6:
                        currentLevel = Level.Level3;
                        break;
                    default:
                        currentLevel = Level.Level3;
                        break;
                }
            }
            else
            {
                // TODO Move later to Start method
                var numberOfLevels = Enum.GetNames(typeof(Level)).Length;
                pedagogicalLevel = 0;
                currentLevel = (Level)Mathf.Clamp((int)Mathf.Floor(pedagogicalLevel * numberOfLevels), 0, numberOfLevels - 1);
            }

            // HACK: FORCED LEVEL TO GET ONLY DIACRITICS
            pedagogicalLevel = 0.0f;
            currentLevel = Level.Level2;

            Debug.Log("[Dancing Dots] pedagogicalLevel: " + pedagogicalLevel + " Game Level: " + currentLevel);
            SetLevel(currentLevel);

            startUI();

            dancingDotsLL.Reset();

            StartCoroutine(tutorial.DoTutorial());

        }

        private void CreatePoof(Vector3 position, float duration, bool withSound)
        {
            if (withSound)
                Context.GetAudioManager().PlaySound(Sfx.BalloonPop);
            GameObject poof = Instantiate(poofPrefab, position, Quaternion.identity) as GameObject;
            Destroy(poof, duration);
        }


        /// <summary>
        /// Remove the hint for a dot
        /// </summary>
        IEnumerator RemoveHintDot()
        {
            yield return new WaitForSeconds(hintDotDuration);
            if (!isCorrectDot)
            {
                // find dot postions
                Vector3 poofPosition = Vector3.zero;
                foreach (DancingDotsDropZone dz in dropZones)
                {
                    if (dz.letters.Contains(currentLetter))
                    {
                        poofPosition = new Vector3(dz.transform.position.x, dz.transform.position.y, -8);
                        break;
                    }
                }
                CreatePoof(poofPosition, 2f, true);
                dancingDotsLL.HideText(dancingDotsLL.hintText);
            }
        }

        /// <summary>
        /// Remove the hint for a diacritic
        /// </summary>
        IEnumerator RemoveHintDiacritic()
        {
            if (letterDiacritic != DiacriticEnum.None)
            {
                yield return new WaitForSeconds(hintDiacriticDuration);
                if (!isCorrectDiacritic)
                {
                    CreatePoof(activeDiacritic.transform.position, 2f, true);
                    activeDiacritic.Hide();
                }
            }
        }

        public IEnumerator SetupDiacritic()
        {
            if (letterDiacritic != DiacriticEnum.None)
            {
                foreach (DancingDotsDraggableDot dDots in dragableDiacritics)
                {
                    dDots.Reset();
                }
                isCorrectDiacritic = false;

                foreach (GameObject go in diacritics)
                {
                    go.SetActive(true);
                    if (go.GetComponent<DancingDotsDiacriticPosition>().diacritic == letterDiacritic)
                    {
                        activeDiacritic = go.GetComponent<DancingDotsDiacriticPosition>();
                    }
                    go.SetActive(false);
                    go.GetComponent<DancingDotsDiacriticPosition>().Hide();
                }

                //            int random = UnityEngine.Random.Range(0, diacritics.Length);
                //            activeDiacritic = diacritics[random].GetComponent<DancingDotsDiacriticPosition>();

                activeDiacritic.gameObject.SetActive(true);

                foreach (DancingDotsDraggableDot dd in dragableDiacritics)
                {
                    dd.isCorrect = activeDiacritic.diacritic == dd.diacritic;
                }

                // wait for end of frame to get correct values for meshes
                yield return new WaitForEndOfFrame();
                activeDiacritic.CheckPosition();

                // Level checked in SetDiacritic instead of SetLevel due to frame delay
                // HACK: removed hint removal
                activeDiacritic.Show();
                /*
                if (currentLevel != Level.Level5 && currentLevel != Level.Level6) {
                    activeDiacritic.Show();
                    StartCoroutine(RemoveHintDiacritic());
                }*/
            }
        }

        public float StartingRoundDancingSpeed => Mathf.Lerp(0.75f, 1.2f, Difficulty);

        IEnumerator CorrectMove(bool roundWon)
        {
            //AudioManager.I.PlayDialog("comment_welldone");
            DancingDotsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.OK);
            TutorialUI.MarkYes(dancingDotsLL.transform.position + Vector3.up * 19 - Vector3.forward * 4, TutorialUI.MarkSize.Big);
            KeeperManager.I.PlayDialogue("Keeper_Good_" + UnityEngine.Random.Range(1, 13), keeperMode:KeeperMode.LearningNoSubtitles);

            dancingDotsLL.ShowRainbow();
            dancingDotsLL.letterObjectView.SetDancingSpeed(StartingRoundDancingSpeed);


            if (roundWon)
            {
                StartCoroutine(RoundWon());
            }
            else
            {
                dancingDotsLL.letterObjectView.DoHorray(); // ("Jump");
                yield return new WaitForSeconds(1f);
                dancingDotsLL.HideRainbow();
                dancingDotsLL.letterObjectView.ToggleDance();
                //                yield return new WaitForSeconds(1f);
                StartCoroutine(tutorial.DoTutorial());
                //startUI();

            }
        }

        IEnumerator PoofOthers(DancingDotsDraggableDot[] draggables)
        {
            foreach (DancingDotsDraggableDot dd in draggables)
            {
                if (dd.gameObject.activeSelf)
                {
                    yield return new WaitForSeconds(0.25f);
                    dd.gameObject.SetActive(false);
                    CreatePoof(dd.transform.position, 2f, true);
                }

            }
        }

        public void CorrectDot()
        {
            // Change font or show correct character
            isCorrectDot = true;
            dancingDotsLL.fullTextGO.SetActive(true);
            StartCoroutine(PoofOthers(dragableDots));
            StartCoroutine(CorrectMove(isCorrectDiacritic));
        }

        public void CorrectDiacritic()
        {
            isCorrectDiacritic = true;
            activeDiacritic.GetComponent<TextMeshPro>().color = new Color32(0, 0, 0, 255);
            StartCoroutine(PoofOthers(dragableDiacritics));
            StartCoroutine(CorrectMove(isCorrectDot));
        }

        public void WrongMove(Vector3 pos)
        {
            //DancingDotsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.KO);

            //DancingDotsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.StampOK);
            TutorialUI.MarkNo(dancingDotsLL.transform.position + Vector3.up * 19 - Vector3.forward * 4, TutorialUI.MarkSize.Big);

            KeeperManager.I.PlayDialogue("Keeper_Bad_" + UnityEngine.Random.Range(1, 6), keeperMode:KeeperMode.LearningNoSubtitles);
            numberOfFailedMoves++;
            dancingDotsLL.letterObjectView.SetDancingSpeed(StartingRoundDancingSpeed - numberOfFailedMoves * 0.25f);
            GameObject splat = (GameObject)Instantiate(splatPrefab);
            splat.transform.parent = splatParent;
            splat.transform.localScale = new Vector3(1f, 1f, 1f);
            splat.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            splat.transform.position = pos;
            splat.transform.localPosition = new Vector3(splat.transform.localPosition.x, splat.transform.localPosition.y, 0f);

            splats.Add(splat.GetComponent<DancingDotsSplat>());

            if (numberOfFailedMoves >= allowedFailedMoves)
            {
                StartCoroutine(RoundLost());
            }

        }

        IEnumerator CheckNewRound()
        {
            dancingDotsLL.letterObjectView.SetDancingSpeed(StartingRoundDancingSpeed);

            if (numberOfRoundsPlayed >= numberOfRounds)
            {
                DancingDotsEndGame();
            }
            else
            {

                dancingDotsLL.letterObjectView.DoTwirl(null);
                foreach (DancingDotsSplat splat in splats)
                    splat.CleanSplat();
                yield return new WaitForSeconds(1f);
                StartRound();
                dancingDotsLL.letterObjectView.ToggleDance();

            }
        }


        IEnumerator RoundLost()
        {
            //wonLastRound = false;
            Context.GetLogManager().OnAnswered(dancingDotsLL.letterData, false);
            yield return new WaitForSeconds(0.5f);
            Context.GetAudioManager().PlaySound(Sfx.Lose);

            StartCoroutine(PoofOthers(dragableDots));
            StartCoroutine(PoofOthers(dragableDiacritics));
            dancingDotsLL.letterObjectView.SetDancingSpeed(StartingRoundDancingSpeed);
            dancingDotsLL.letterObjectView.DoDancingLose();
            yield return new WaitForSeconds(1.5f);
            dancingDotsLL.letterObjectView.DoSmallJump();
            dancingDotsLL.letterObjectView.ToggleDance();
            StartCoroutine(CheckNewRound());
        }

        IEnumerator RoundWon()
        {
            if (!isTutRound)
            {
                Context.GetLogManager().OnAnswered(dancingDotsLL.letterData, true);
                CurrentScore++;
            }

            //wonLastRound = true;
            yield return new WaitForSeconds(0.25f);
            Context.GetAudioManager().PlaySound(Sfx.Win);
            yield return new WaitForSeconds(1f);

            StartCoroutine(CheckNewRound());
        }

        public void DancingDotsEndGame()
        {
            //dancingDotsLL.letterObjectView.DoDancingWin();
            isPlaying = false;
            //dancingDotsLL.letterObjectView.SetState(LLAnimationStates.LL_idle);

            dancingDotsLL.letterObjectView.DoDancingWin();

            // Stop danger clock if rounds finish and it is running
            //Context.GetAudioManager().StopSounds();
            //this.SetCurrentState(this.ResultState);
            StartCoroutine(EndGame_Coroutine());
        }

        IEnumerator EndGame_Coroutine()
        {
            yield return new WaitForSeconds(2f);
            this.SetCurrentState(this.ResultState);
        }
        IEnumerator setIdle()
        {
            yield return new WaitForSeconds(2.5f);
            dancingDotsLL.letterObjectView.SetState(LLAnimationStates.LL_still);
        }

        void startUI()
        {
            if (numberOfRoundsPlayed != 1)
            {
                return;
            }
            Debug.Log("UI Started");
            Context.GetOverlayWidget().Initialize(true, true, false);
            Context.GetOverlayWidget().SetClockDuration(gameDuration);
            Context.GetOverlayWidget().SetStarsThresholds(STARS_1_THRESHOLD, STARS_2_THRESHOLD, STARS_3_THRESHOLD);
        }


        public string removeDiacritics(string letter)
        {
            //nasb
            if (letter.Contains("ً"))
            {
                return letter.Replace("ً", string.Empty);
            }
            //jarr
            else if (letter.Contains("ٍ"))
            {
                return letter.Replace("ٍ", string.Empty);
            }
            //damm
            else if (letter.Contains("ٌ"))
            {
                return letter.Replace("ٌ", string.Empty);
            }
            //kasra
            else if (letter.Contains("ِ"))
            {
                return letter.Replace("ِ", string.Empty);
            }
            //fatha
            else if (letter.Contains("َ"))
            {
                return letter.Replace("َ", string.Empty);
            }
            //damma
            else if (letter.Contains("ُ"))
            {
                return letter.Replace("ُ", string.Empty);
            }
            //shadda
            else if (letter.Contains("ّ"))
            {
                return letter.Replace("ّ", string.Empty);

            }
            //sukon
            else if (letter.Contains("ْ"))
            {
                return letter.Replace("ْ", string.Empty);
            }
            else
            {
                return letter;
            }
        }

        /*
        DancingDotsQuadManager disco;
        double time = 0;
        public float add;
        IEnumerator doBeatAt(float t)
        {

            while (AudioSettings.dspTime < t + time+ add)
                yield return null;
            Debug.LogError(AudioSettings.dspTime);
            disco.swap();
            disco.swap();
            disco.swap();
            disco.swap();
            //time = AudioSettings.dspTime;
        }
        IEnumerator beat() {
            time = AudioSettings.dspTime;

            StartCoroutine(doBeatAt(0.02f));
            StartCoroutine(doBeatAt(0.11f));
            StartCoroutine(doBeatAt(0.16f));
            StartCoroutine(doBeatAt(0.20f));
            StartCoroutine(doBeatAt(0.29f));
            StartCoroutine(doBeatAt(1.08f));
            StartCoroutine(doBeatAt(1.17f));
            StartCoroutine(doBeatAt(1.21f));
            StartCoroutine(doBeatAt(1.26f));
            StartCoroutine(doBeatAt(2.05f));
            StartCoroutine(doBeatAt(2.14f));
            StartCoroutine(doBeatAt(2.23f));
            StartCoroutine(doBeatAt(2.27f));
            StartCoroutine(doBeatAt(3.02f));
            StartCoroutine(doBeatAt(3.11f));
            StartCoroutine(doBeatAt(3.20f));
            StartCoroutine(doBeatAt(3.28f));
            StartCoroutine(doBeatAt(4.03f));
            StartCoroutine(doBeatAt(4.08f));
            StartCoroutine(doBeatAt(4.17f));
            StartCoroutine(doBeatAt(4.26f));
            StartCoroutine(doBeatAt(5.05f));
            StartCoroutine(doBeatAt(5.09f));
            StartCoroutine(doBeatAt(5.13f));
            StartCoroutine(doBeatAt(5.23f));
            StartCoroutine(doBeatAt(6.02f));
            StartCoroutine(doBeatAt(6.10f));
            StartCoroutine(doBeatAt(6.16f));
            StartCoroutine(doBeatAt(6.20f));
            StartCoroutine(doBeatAt(6.28f));
            StartCoroutine(doBeatAt(7.07f));
            StartCoroutine(doBeatAt(7.16f));
            StartCoroutine(doBeatAt(7.21f));
            StartCoroutine(doBeatAt(7.26f));
            StartCoroutine(doBeatAt(8.04f));
            StartCoroutine(doBeatAt(8.14f));
            StartCoroutine(doBeatAt(8.22f));
            StartCoroutine(doBeatAt(8.24f));
            StartCoroutine(doBeatAt(8.27f));
            StartCoroutine(doBeatAt(9.01f));
            StartCoroutine(doBeatAt(9.10f));

            yield return null;
        }*/

        /*IEnumerator doBeatAt(float s)
        {
            yield return new WaitForSeconds(s);
            floor.swap();
        }
        */
    }


}
