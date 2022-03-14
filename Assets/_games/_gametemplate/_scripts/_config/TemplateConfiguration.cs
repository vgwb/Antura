using System;
using Antura.LivingLetters;
using Antura.Teacher;

namespace Antura.Minigames.Template
{
    public enum TemplateVariation
    {
        Example = 0
    }

    /// <summary>
    /// Template configuration for a minigame.
    /// Use this as a starting point.
    /// </summary>
    public class TemplateConfiguration : AbstractGameConfiguration
    {
        private TemplateVariation Variation { get; set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (TemplateVariation)code;
        }

        // Singleton Pattern
        static TemplateConfiguration instance;
        public static TemplateConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TemplateConfiguration();
                }
                return instance;
            }
        }

        private TemplateConfiguration()
        {
            // Default values
            // THESE SETTINGS ARE FOR SAMPLE PURPOSES, THESE VALUES MUST BE SET BY GAME CORE
            Context = new MinigamesGameContext(MiniGameCode.Invalid, System.DateTime.Now.Ticks.ToString());
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            // CONFIGURE HERE WHAT BUILDER THE MINIGAME IS EXPECTING BASED ON ITS VARIATION
            switch (Variation)
            {
                case TemplateVariation.Example:
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
