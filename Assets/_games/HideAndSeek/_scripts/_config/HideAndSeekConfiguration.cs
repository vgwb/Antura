using System;
using Antura.Core;
using Antura.Database;
using Antura.LivingLetters.Sample;
using Antura.Teacher;

namespace Antura.Minigames.HideAndSeek
{
    public enum HideAndSeekVariation
    {
        LetterPhoneme = MiniGameCode.HideSeek_letterphoneme,
        LetterName = MiniGameCode.HideSeek_lettername,
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
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 10;
            int nCorrect = 1;
            int nWrong = 6;

            var builderParams = InitQuestionBuilderParamaters();
            switch (Variation)
            {
                case HideAndSeekVariation.LetterPhoneme:
                    var letterAlterationFilters = LetterAlterationFilters.FormsAndPhonemesOfMultipleLetters_OneForm;
                    if (AppManager.I.ContentEdition.DiacriticsOnlyOnIsolated)
                        letterAlterationFilters = LetterAlterationFilters.DiacriticsOfMultipleLetters;
                    builder = new RandomLetterAlterationsQuestionBuilder(nPacks, nCorrect, nWrong: nWrong, letterAlterationFilters: letterAlterationFilters, parameters: builderParams,
                            avoidWrongLettersWithSameSound: true);
                    break;
                case HideAndSeekVariation.LetterName:
                    builder = new RandomLettersQuestionBuilder(nPacks, nCorrect, nWrong: nWrong, parameters: builderParams);
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

    }
}
