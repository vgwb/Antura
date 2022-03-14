using System;
using System.Collections.Generic;
using System.Linq;
using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.FastCrowd
{
    public class FastCrowdGame : MiniGameController
    {
        public QuestionManager QuestionManager;

        public AnturaRunnerController antura;

        public List<ILivingLetterData> CurrentChallenge = new List<ILivingLetterData>();
        public List<ILivingLetterData> NoiseData = new List<ILivingLetterData>();

        public IQuestionPack CurrentQuestion = null;

        #region Score

        public override int MaxScore => stars3Threshold;

        int stars1Threshold
        {
            get
            {
                switch (FastCrowdConfiguration.Instance.Variation)
                {
                    case FastCrowdVariation.Word:
                    case FastCrowdVariation.Image:
                        return 8;
                    default:
                        return (int)(stars3Threshold * 0.25f);
                }
            }
        }

        private int stars2Threshold
        {
            get
            {
                return (int)(stars3Threshold * 0.5f);
            }
        }

        private int stars3Threshold
        {
            get
            {
                switch (FastCrowdConfiguration.Instance.Variation)
                {
                    case FastCrowdVariation.Alphabet:
                        return CurrentChallenge.Count;
                    case FastCrowdVariation.Counting:
                    case FastCrowdVariation.OrderedImage_Numbers:
                    case FastCrowdVariation.OrderedImage_Colors:
                    case FastCrowdVariation.OrderedImage_Months:
                    case FastCrowdVariation.OrderedImage_Days_Seasons:
                    {
                        var provider = (FastCrowdConfiguration.Instance.Questions as SequentialQuestionPackProvider);
                        return provider?.EnumerateAllPacks().Sum(pack => pack.GetCorrectAnswers().Count()) * 2 ?? 15;
                    }
                    case FastCrowdVariation.BuildWord:
                    {
                        var provider = (FastCrowdConfiguration.Instance.Questions as SequentialQuestionPackProvider);
                        var firstPack = provider.PeekFirstQuestion();
                        int totLetters = (int)(provider?.EnumerateAllPacks().Sum(pack => pack.GetCorrectAnswers().Count()) - firstPack.GetCorrectAnswers().Count());
                        return totLetters;
                    }
                    default:
                        return 15;
                }
            }
        }

        public int CurrentStars
        {
            get
            {
                if (CurrentScore < stars1Threshold)
                    return 0;
                if (CurrentScore < stars2Threshold)
                    return 1;
                if (CurrentScore < stars3Threshold)
                    return 2;
                return 3;
            }
        }

        #endregion

        public bool showTutorial { get; set; }

        public FastCrowdIntroductionState IntroductionState { get; private set; }
        public FastCrowdQuestionState QuestionState { get; private set; }
        public FastCrowdPlayState PlayState { get; private set; }
        public FastCrowdResultState ResultState { get; private set; }
        public FastCrowdEndState EndState { get; private set; }
        public FastCrowdTutorialState TutorialState { get; private set; }

        public void ResetScore()
        {
            CurrentScore = 0;
        }

        public void IncrementScore()
        {
            ++CurrentScore;
        }

        protected override IGameConfiguration GetConfiguration()
        {
            return FastCrowdConfiguration.Instance;
        }

        protected override FSM.IState GetInitialState()
        {
            return IntroductionState;
        }

        protected override void OnInitialize(IGameContext context)
        {
            showTutorial = GetConfiguration().TutorialEnabled;

            IntroductionState = new FastCrowdIntroductionState(this);
            QuestionState = new FastCrowdQuestionState(this);
            PlayState = new FastCrowdPlayState(this);
            ResultState = new FastCrowdResultState(this);
            EndState = new FastCrowdEndState(this);
            TutorialState = new FastCrowdTutorialState(this);

            QuestionManager.wordComposer.gameObject.SetActive(false);
            QuestionManager.wordComposer.splitMode = FastCrowdConfiguration.Instance.WordComposerInSplitMode;
        }

        public bool ShowChallengePopupWidget(bool showAsGoodAnswer, Action callback)
        {
            if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.BuildWord)
            {
                var popupWidget = Context.GetPopupWidget();
                popupWidget.Show();
                popupWidget.SetButtonCallback(callback);

                if (showAsGoodAnswer)
                {
                    //popupWidget.SetTitle(Database.LocalizationDataId.Keeper_Good_5);
                    popupWidget.SetTitle("");
                    popupWidget.SetMark(true, true);
                }
                //else
                //    popupWidget.SetTitle("" + QuestionNumber);

                var question = CurrentQuestion.GetQuestion();
                popupWidget.SetLetterData(question);
                Context.GetAudioManager().PlayVocabularyData(question, soundType: FastCrowdConfiguration.Instance.GetVocabularySoundType());
                return true;
            }
            return false;
        }

        public void InitializeOverlayWidget()
        {
            Context.GetOverlayWidget().Initialize(true, true, false);
            Context.GetOverlayWidget().SetStarsThresholds(stars1Threshold, stars2Threshold, stars3Threshold);
        }

        protected override Vector3 GetGravity()
        {
            return Vector3.up * (-40);
        }
    }
}
