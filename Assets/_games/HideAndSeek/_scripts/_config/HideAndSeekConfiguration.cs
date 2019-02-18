using System;
using Antura.Database;
using Antura.LivingLetters.Sample;
using Antura.Teacher;

namespace Antura.Minigames.HideAndSeek
{
    public enum HideAndSeekVariation
    {
        LetterPhoneme = MiniGameCode.HideSeek_letterphoneme,
        Image = MiniGameCode.HideSeek_image
    }

    public class HideAndSeekConfiguration : AbstractGameConfiguration
    {
        public HideAndSeekVariation Variation { get; private set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (HideAndSeekVariation)code;
        }

        // Singleton Pattern
        static HideAndSeekConfiguration instance;
        public static HideAndSeekConfiguration Instance
        {
            get
            {
                if (instance == null)
                    instance = new HideAndSeekConfiguration();
                return instance;
            }
        }

        private HideAndSeekConfiguration()
        {
            // Default values
            Context = new MinigamesGameContext(MiniGameCode.HideSeek_letterphoneme, System.DateTime.Now.Ticks.ToString());
            Questions = new SampleQuestionProvider();
            Difficulty = 0.5f;
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 10;
            int nCorrect = 1;
            int nWrong = 6;

            var builderParams = new QuestionBuilderParameters();
            switch (Variation)
            {
                case HideAndSeekVariation.LetterPhoneme:
                    var letterAlterationFilters = LetterAlterationFilters.FormsAndPhonemesOfMultipleLetters_OneForm;
                    builder = new RandomLetterAlterationsQuestionBuilder(nPacks, nCorrect, nWrong: nWrong, letterAlterationFilters: letterAlterationFilters, parameters: builderParams,
                            avoidWrongLettersWithSameSound: true);
                    break;
                case HideAndSeekVariation.Image:
                    builderParams.wordFilters.requireDrawings = true;
                    builder = new RandomWordsQuestionBuilder(nPacks, nCorrect, nWrong: nWrong, parameters: builderParams);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return builder;
        }

        public override MiniGameLearnRules SetupLearnRules()
        {
            var rules = new MiniGameLearnRules();
            // example: a.minigameVoteSkewOffset = 1f;
            return rules;
        }

        public override LocalizationDataId TitleLocalizationId
        {
            get
            {
                switch (Variation)
                {
                    case HideAndSeekVariation.LetterPhoneme:
                        return LocalizationDataId.HideSeek_letterphoneme_Title;
                    case HideAndSeekVariation.Image:
                        return LocalizationDataId.HideSeek_image_Title;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public LocalizationDataId TutorialLocalizationId
        {
            get
            {
                switch (Variation)
                {
                    case HideAndSeekVariation.LetterPhoneme:
                        return LocalizationDataId.HideSeek_letterphoneme_Tuto;
                    case HideAndSeekVariation.Image:
                        return LocalizationDataId.HideSeek_image_Tuto;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public LocalizationDataId IntroLocalizationId
        {
            get
            {
                switch (Variation)
                {
                    case HideAndSeekVariation.LetterPhoneme:
                        return LocalizationDataId.HideSeek_letterphoneme_Intro;
                    case HideAndSeekVariation.Image:
                        return LocalizationDataId.HideSeek_image_Intro;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

    }
}
