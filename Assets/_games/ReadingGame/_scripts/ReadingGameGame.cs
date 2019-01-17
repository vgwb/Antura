using System.IO;
using System.Text;
using Antura.LivingLetters;
using Antura.Minigames;
using UnityEngine;

namespace Antura.Minigames.ReadingGame
{
    public class ReadingGameGame : MiniGameController // ReadingGameGameGameGameGame!
    {
        public ReadingBarSet barSet;
        public GameObject blurredText;
        public HiddenText hiddenText;
        public GameObject circleBox;
        public ReadingGameAntura antura;
        public ReadingRadialWidget radialWidget;
        public Camera uiCamera;

        public int CurrentScore { get; private set; }
        public int CurrentQuestionNumber { get; set; }
        public bool TutorialEnabled { get { return GetConfiguration().TutorialEnabled; } }

        [HideInInspector]
        public bool isTimesUp;

        int lives = 3;
        public int Lives { get { return lives; } }

        public Material magnifyingGlassMaterial;
        public Material blurredTextMaterial;

        [HideInInspector]
        public KaraokeSong songToPlay;

        public AudioClip alphabetSongAudio;
        public AudioClip diacriticSongAudio;
        public TextAsset alphabetSongSrt;
        public TextAsset diacriticSongSrt;

        public int TimeToAnswer
        {
            get
            {
                return Mathf.RoundToInt(30 - 20 * ReadingGameConfiguration.Instance.Difficulty);
            }
        }

        public const int MAX_QUESTIONS = 5;
        static readonly float[] READING_STARS_THRESHOLDS_RATIOS = new float[] { 0.4f, 0.55f, 0.7f };

        public int GetStarsThreshold(int stars)
        {
            if (ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.ReadAndAnswer)
            {
                return Mathf.FloorToInt(MAX_QUESTIONS * TimeToAnswer * READING_STARS_THRESHOLDS_RATIOS[stars - 1]);
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

            if (ReadingGameConfiguration.Instance.Variation != ReadingGameVariation.ReadAndAnswer)
            {
                ISongParser parser = new AkrSongParser();

                var textAsset = ReadingGameConfiguration.Instance.Variation == ReadingGameVariation.Alphabet ? alphabetSongSrt : diacriticSongSrt;

                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(textAsset.text)))
                {
                    songToPlay = new KaraokeSong(parser.Parse(stream));
                }
            }

            radialWidget.Hide();

            // Instantiating a runtime material
            magnifyingGlassMaterial = new Material(magnifyingGlassMaterial);
            magnifyingGlassMaterial.name = magnifyingGlassMaterial.name + "(INSTANCE)";
            blurredTextMaterial = new Material(blurredTextMaterial);
            blurredTextMaterial.name = blurredTextMaterial.name + "(INSTANCE)";
        }

        public void AddScore(int score)
        {
            CurrentScore += score;

            Context.GetOverlayWidget().SetStarsScore(CurrentScore);
        }

        public bool RemoveLife()
        {
            --lives;
            Context.GetOverlayWidget().SetLives(lives);

            if (lives == 0)
            {
                EndGame(CurrentStars, CurrentScore);
                return true;
            }
            return false;
        }
    }
}