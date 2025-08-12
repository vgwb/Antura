using Antura.Minigames;
using Antura.LivingLetters.Sample;
using Antura.Teacher;
using System;

namespace Antura.Discover
{
    public enum DiscoverCountryVariation
    {
        Discover_Country = MiniGameCode.Discover_Country,
    }

    public class DiscoverCountryConfiguration : AbstractGameConfiguration
    {
        public DiscoverCountryVariation Variation { get; private set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (DiscoverCountryVariation)code;
        }

        // Singleton Pattern
        static DiscoverCountryConfiguration instance;
        public static DiscoverCountryConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DiscoverCountryConfiguration();
                }
                return instance;
            }
        }

        private DiscoverCountryConfiguration()
        {
            // Default values
            Context = new MinigamesGameContext(MiniGameCode.Discover_Country, System.DateTime.Now.Ticks.ToString());
            Variation = DiscoverCountryVariation.Discover_Country;
            Questions = new SampleQuestionProvider();
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            var builderParams = InitQuestionBuilderParamaters();
            builderParams.correctSeverity = SelectionSeverity.AsManyAsPossible;

            switch (Variation)
            {
                case DiscoverCountryVariation.Discover_Country:
                    builder = new EmptyQuestionBuilder();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return builder;
        }

        public override MiniGameLearnRules SetupLearnRules()
        {
            var rules = new MiniGameLearnRules();
            return rules;
        }

    }
}
