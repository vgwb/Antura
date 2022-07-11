using System;
using System.Collections;
using System.IO;
using System.Text;
using Antura.Core;
using Antura.Dog;
using Antura.Language;
using Antura.LivingLetters;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.ReadingGame
{
    public class ReadingGameGame : MiniGameController // ReadingGameGameGameGameGame!
    {
        public ReadingBarSet barSet;
        public GameObject blurredText;
        public HiddenText hiddenText;
        public GameObject circleBox;
        public ReadingGameAntura antura;
        public AnturaAnimationController dancingAntura;
        public ReadingRadialWidget radialWidget;
        public Camera uiCamera;

        public int CurrentQuestionNumber { get; set; }
        public bool TutorialEnabled => GetConfiguration().TutorialEnabled;

        [HideInInspector]
        public bool isTimesUp;

        int lives = 3;
        public int Lives => lives;

        public Material magnifyingGlassMaterial;
        public Material blurredTextMaterial;

        [Header("Parameters - Follow Song")]
        [HideInInspector]
        public KaraokeSong songToPlay;

        public AudioClip alphabetSongAudio => AppManager.I.AssetManager.GetSongClip("AlphabetSong");
        public AudioClip diacriticSongAudio => AppManager.I.AssetManager.GetSongClip("DiacriticSong");

        public TextAsset alphabetSongSrt => AppManager.I.AssetManager.GetSongSrt("AlphabetSong.akr");
        public TextAsset diacriticSongSrt => AppManager.I.AssetManager.GetSongSrt("DiacriticSong.akr");

        [Header("Parameters - Simon Song")]
        public SimonSongBPM simonSong120;
        public SimonSongBPM simonSong140;
        public SimonSongBPM simonSong160;

        public SimonSongBPM CurrentSongBPM
        {
            get
            {
                // Difficulty for SimonSong
                if (Difficulty <= 0.33f)
                    return simonSong120;
                else if (Difficulty <= 0.66f)
                    return simonSong140;
                else
                    return simonSong160;
            }
        }

        public void UpdateFollowDifficulty()
        {
            barSet.SetShowTargets(Difficulty < DifficultyForStars(2));
            barSet.SetShowArrows(true);
        }

        #region Score

        public override int MaxScore => GetStarsThreshold(3);

        public int GetStarsThreshold(int stars)
        {
            if (ReadingGameConfiguration.Instance.CurrentGameType == ReadingGameConfiguration.GameType.SimonSong)
            {
                return Mathf.RoundToInt((1 + MAX_QUESTIONS_SIMON_SONG) / 3f * stars);
            }

            if (ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.ReadingGame_Words)
            {
                return Mathf.Clamp(stars * 2 - 1, 0, MAX_QUESTIONS);
            }

            if (songToPlay == null)
                return int.MaxValue;

            var t = (int)(songToPlay.GetSegmentsLength() / (4 - stars));
            return t;
        }

        public int CurrentStars
        {
            get
            {
                if (CurrentScore < GetStarsThreshold(1))
                    return 0;
                if (CurrentScore < GetStarsThreshold(2))
                    return 1;
                if (CurrentScore < GetStarsThreshold(3))
                    return 2;
                return 3;
            }
        }

        public float DifficultyForStars(int stars)
        {
            return DifficultyForScore(GetStarsThreshold(stars));
        }

        #endregion

        [Header("Animations")]
        public GameObject letterObjectPrefab;
        public GameObject shadowPrefab;
        public RunLettersBox runLettersBox;
        public GameLettersHandler gameLettersHandler;

        [Serializable]
        public struct SimonSongBPM
        {
            public float questionTime;
            public int bpm;

            public AudioClip voicePartNative => AppManager.I.AssetManager.GetNativeSongClip($"SimonSong_Voice_{bpm}");
            public AudioClip voicePartLearning => AppManager.I.AssetManager.GetSongClip($"SimonSong_Voice_{bpm}");
            public AudioClip songFirstHalf => AppManager.I.AssetManager.GetSongClip($"SimonSong_MusicFirstHalf_{bpm}");
            public AudioClip songSecondHalf => AppManager.I.AssetManager.GetSongClip($"SimonSong_MusicSecondHalf_{bpm}");

            public AudioClip intro => AppManager.I.AssetManager.GetSongClip($"SimonSong_Intro_{bpm}");

            public float periodRatio
            {
                get
                {
                    float bps = bpm / 60f;
                    return 2f / bps;
                }
            }
        }

        public float TimeToAnswer
        {
            get
            {
                switch (ReadingGameConfiguration.Instance.CurrentGameType)
                {
                    case ReadingGameConfiguration.GameType.FollowReading:
                    case ReadingGameConfiguration.GameType.ReadAndListen:
                        return Mathf.RoundToInt(30 - 20 * Difficulty);
                    case ReadingGameConfiguration.GameType.FollowSong:
                        // no time
                        return 0;
                    case ReadingGameConfiguration.GameType.SimonSong:
                        return 8 * CurrentSongBPM.periodRatio;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public const int MAX_QUESTIONS = 5;
        public const int MAX_QUESTIONS_SIMON_SONG = 8;

        public ReadingGameInitialState InitialState { get; private set; }
        public ReadingGameQuestionState QuestionState { get; private set; }
        public ReadingGameReadState ReadState { get; private set; }
        public ReadingGameAnswerState AnswerState { get; private set; }
        public IQuestionPack CurrentQuestion { get; set; }

        protected override IGameConfiguration GetConfiguration()
        {
            return ReadingGameConfiguration.Instance;
        }

        protected override FSM.IState GetInitialState()
        {
            return InitialState;
        }

        protected override void OnInitialize(IGameContext context)
        {
            InitialState = new ReadingGameInitialState(this);
            QuestionState = new ReadingGameQuestionState(this);
            ReadState = new ReadingGameReadState(this);
            AnswerState = new ReadingGameAnswerState(this);

            radialWidget.Hide();

            // Instantiating a runtime material
            magnifyingGlassMaterial = new Material(magnifyingGlassMaterial);
            magnifyingGlassMaterial.name = magnifyingGlassMaterial.name + "(INSTANCE)";
            blurredTextMaterial = new Material(blurredTextMaterial);
            blurredTextMaterial.name = blurredTextMaterial.name + "(INSTANCE)";

            runLettersBox.Initialize(letterObjectPrefab, shadowPrefab);
            gameLettersHandler.Initialize(letterObjectPrefab, shadowPrefab);

            DisableRepeatPromptButton();

            antura.gameObject.SetActive(ReadingGameConfiguration.Instance.CurrentGameType != ReadingGameConfiguration.GameType.SimonSong);
            dancingAntura.gameObject.SetActive(ReadingGameConfiguration.Instance.CurrentGameType == ReadingGameConfiguration.GameType.SimonSong);
            dancingAntura.State = AnturaAnimationStates.dancing;
        }

        public void LoadSongAkr()
        {
            if (ReadingGameConfiguration.Instance.CurrentGameType == ReadingGameConfiguration.GameType.FollowSong)
            {
                ISongParser parser = new AkrSongParser();

                var textAsset = ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.SongAlphabet ? alphabetSongSrt : diacriticSongSrt;

                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(textAsset.text)))
                {
                    songToPlay = new KaraokeSong(parser.Parse(stream));
                }
            }
        }

        public void AddScore(int score)
        {
            CurrentScore += score;
        }

        public bool RemoveLife()
        {
            --lives;
            Context.GetOverlayWidget().SetLives(lives);

            if (lives == 0)
            {
                runLettersBox.RemoveAllRunLetters();
                EndGame(CurrentStars, CurrentScore);
                return true;
            }
            return false;
        }

        #region Looping Song

        private Coroutine loopSongCO;
        public void StartLoopingSong(AudioClip song, AudioClip voice = null)
        {
            loopSongCO = StartCoroutine(SongLoopCO(song, voice));
        }
        public bool IsLoopingSong => loopSongCO != null;

        public void StopLoopingSong()
        {
            onSongLoop = null;
            if (loopSongCO != null)
            {
                StopCoroutine(loopSongCO);
                loopSongCO = null;
            }
        }

        public void ChangeLoopingSong(AudioClip song, AudioClip voice = null)
        {
            StopLoopingSong();
            StartLoopingSong(song, voice);
        }

        public Action onSongLoop;
        private IEnumerator SongLoopCO(AudioClip song, AudioClip voice = null)
        {
            while (true)
            {
                if (voice != null)
                {
                    loopingVoiceSource = Context.GetAudioManager().PlaySound(voice);
                }
                else
                {
                    loopingVoiceSource = null;
                }
                loopingSongSource = Context.GetAudioManager().PlaySound(song);
                yield return WaitForPauseCO(song.length, true);
                onSongLoop?.Invoke();
                yield return null;
            }
        }

        public IEnumerator WaitForPauseCO(float waitTime, bool checkPause = false)
        {
            float t = 0.0f;
            while (t < waitTime)
            {
                if (!songPaused)
                    t += Time.deltaTime;
                if (t >= waitTime)
                    break;
                if (checkPause)
                    CheckSongPause();
                yield return null;
            }
        }

        private IAudioSource loopingSongSource;
        private IAudioSource loopingVoiceSource;
        private float lastSongTime;
        private bool songPaused;
        private void CheckSongPause()
        {
            bool isMenuOpen = UI.PauseMenu.I.IsMenuOpen;
            if (loopingSongSource != null && loopingSongSource.IsPlaying && isMenuOpen)
            {
                float currentTime = loopingSongSource.Position;
                lastSongTime = currentTime;
                loopingSongSource.Pause();
                if (loopingVoiceSource != null)
                    loopingVoiceSource.Pause();
                songPaused = true;
                Debug.Log("SONG PAUSING");
            }
            else if (loopingSongSource != null && !loopingSongSource.IsPlaying && !isMenuOpen && songPaused)
            {
                loopingSongSource.Stop();
                loopingSongSource.Play();
                loopingSongSource.Position = lastSongTime;
                if (loopingVoiceSource != null)
                {
                    loopingVoiceSource.Stop();
                    loopingVoiceSource.Play();
                    loopingVoiceSource.Position = lastSongTime;
                }
                songPaused = false;
                Debug.Log("SONG RESUMING");
            }
        }


        #endregion

        #region Repeat Button

        public Button repeatPromptButton;

        public void EnableRepeatPromptButton()
        {
            repeatPromptButton.gameObject.SetActive(true);
        }

        public void DisableRepeatPromptButton()
        {
            repeatPromptButton.gameObject.SetActive(false);
        }

        public void SayQuestion()
        {
            if (AnswerState.Finished)
                return;
            var question = CurrentQuestion.GetQuestion();
            ReadingGameConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(question);
        }

        #endregion

    }
}
