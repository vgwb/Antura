using Antura.Teacher;
using System;
using System.Collections.Generic;
using Antura.Database;

namespace Antura.Minigames.MakeFriends
{
    public enum MakeFriendsDifficulty
    {
        EASY,
        MEDIUM,
        HARD
    }

    public enum MakeFriendsVariation
    {
        LetterInWord = MiniGameCode.MakeFriends_letterinword
    }

    public class MakeFriendsConfiguration : AbstractGameConfiguration
    {
        private MakeFriendsVariation Variation { get; set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (MakeFriendsVariation)code;
        }

        // Singleton Pattern
        static MakeFriendsConfiguration instance;
        public static MakeFriendsConfiguration Instance
        {
            get {
                if (instance == null)
                    instance = new MakeFriendsConfiguration();
                return instance;
            }
        }

        private MakeFriendsConfiguration()
        {
            // Default values
            Questions = new MakeFriendsQuestionProvider();
            Context = new MinigamesGameContext(MiniGameCode.MakeFriends_letterinword, System.DateTime.Now.Ticks.ToString());
            Difficulty = 0f;
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 10;
            int nWrong = 5;
            int nWords = 2;
            var letterEqualityStrictness = LetterEqualityStrictness.WithVisualForm;


            var builderParams = new QuestionBuilderParameters();
            switch (Variation) {
                case MakeFriendsVariation.LetterInWord:
                    builderParams.letterFilters.excludeDiphthongs = true;
                    builderParams.wordFilters.excludeDipthongs = true;
                    builderParams.wordFilters.excludeArticles = true;
                    builder = new CommonLetterInWordQuestionBuilder(nPacks, nWrong, nWords, parameters: builderParams, letterEqualityStrictness: letterEqualityStrictness);
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


        #region Difficulty Choice

        private const float EASY_THRESHOLD = 0f;
        private const float MEDIUM_THRESHOLD = 0.3f;
        private const float HARD_THRESHOLD = 0.7f;

        public MakeFriendsDifficulty DifficultyChoice
        {
            get {
                // GameManager Override
                if (MakeFriendsGame.Instance.overrideDifficulty) {
                    switch (MakeFriendsGame.Instance.difficultySetting) {
                        case MakeFriendsDifficulty.EASY:
                            Difficulty = EASY_THRESHOLD;
                            break;

                        case MakeFriendsDifficulty.MEDIUM:
                            Difficulty = MEDIUM_THRESHOLD;
                            break;

                        case MakeFriendsDifficulty.HARD:
                            Difficulty = HARD_THRESHOLD;
                            break;
                    }
                }

                // Get Variation based on Difficulty
                MakeFriendsDifficulty variation;
                if (Difficulty < MEDIUM_THRESHOLD) {
                    variation = MakeFriendsDifficulty.EASY;
                } else if (Difficulty < HARD_THRESHOLD) {
                    variation = MakeFriendsDifficulty.MEDIUM;
                } else {
                    variation = MakeFriendsDifficulty.HARD;
                }

                return variation;
            }
        }

        #endregion
    }
}