using Antura.Teacher;
using System;
using System.Collections.Generic;
using Antura.Core;
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
            get
            {
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
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 10;
            int nWrong = 5;
            int nWords = 2;
            var letterEqualityStrictness = LetterEqualityStrictness.WithVisualForm;

            var builderParams = InitQuestionBuilderParamaters();
            switch (Variation)
            {
                case MakeFriendsVariation.LetterInWord:
                    builderParams.wordFilters.excludeDipthongs = true;
                    builderParams.wordFilters.excludeArticles = true;
                    builderParams.letterFilters.excludeDiphthongs = true;
                    builderParams.letterFilters.excludeDiacritics = AppManager.I.ContentEdition.DiacriticsOnlyOnIsolated ? LetterFilters.ExcludeDiacritics.All : LetterFilters.ExcludeDiacritics.None;
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

        public override LetterDataSoundType GetVocabularySoundType()
        {
            LetterDataSoundType soundType;
            switch (Variation)
            {
                case MakeFriendsVariation.LetterInWord:
                    soundType = AppManager.I.ContentEdition.PlayNameSoundWithForms ? LetterDataSoundType.Name : LetterDataSoundType.Phoneme;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return soundType;
        }
    }
}
