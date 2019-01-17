using System;
using Antura.Database;
using Antura.Teacher;

namespace Antura.Minigames.ThrowBalls
{
    public enum ThrowBallsVariation
    {
        LetterName = MiniGameCode.ThrowBalls_lettername,
        LetterAny = MiniGameCode.ThrowBalls_letterany,
        Word = MiniGameCode.ThrowBalls_word,
        BuildWord = MiniGameCode.ThrowBalls_buildword
    }

    public class ThrowBallsConfiguration : AbstractGameConfiguration
    {
        public ThrowBallsVariation Variation { get; private set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (ThrowBallsVariation)code;
        }

        // Singleton Pattern
        static ThrowBallsConfiguration instance;
        public static ThrowBallsConfiguration Instance
        {
            get {
                if (instance == null) {
                    instance = new ThrowBallsConfiguration();
                }
                return instance;
            }
        }

        private ThrowBallsConfiguration()
        {
            // Default values
            Questions = new ThrowBallsQuestionProvider();
            Variation = ThrowBallsVariation.LetterName;
            Context = new MinigamesGameContext(MiniGameCode.ThrowBalls_lettername, DateTime.Now.Ticks.ToString());
            Difficulty = 0.7f;
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 10;
            int nWrong = 4;

            var builderParams = new QuestionBuilderParameters();
            switch (Variation) {
                case ThrowBallsVariation.LetterName:
                    builderParams.letterFilters.excludeDiacritics = LetterFilters.ExcludeDiacritics.All;
                    builder = new RandomLettersQuestionBuilder(nPacks, 1, nWrong: nWrong, firstCorrectIsQuestion: true, parameters: builderParams);
                    break;
                case ThrowBallsVariation.LetterAny:
                    var letterAlterationFilters = LetterAlterationFilters.FormsAndPhonemesOfMultipleLetters;
                    builder = new RandomLetterAlterationsQuestionBuilder(nPacks, 1, nWrong: nWrong, letterAlterationFilters: letterAlterationFilters, parameters: builderParams);
                    break;
                case ThrowBallsVariation.Word:
                    builderParams.wordFilters.requireDrawings = true;
                    builder = new RandomWordsQuestionBuilder(nPacks, 1, nWrong, firstCorrectIsQuestion: true, parameters: builderParams);
                    break;
                case ThrowBallsVariation.BuildWord:
                    builderParams.wordFilters.excludeDipthongs = true;
                    builder = new LettersInWordQuestionBuilder(nPacks, maximumWordLength: 7, nWrong: nWrong, useAllCorrectLetters: true, parameters: builderParams);
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
            get {
                switch (Instance.Variation) {
                    case ThrowBallsVariation.LetterName:
                        return LocalizationDataId.ThrowBalls_lettername_Title;
                    case ThrowBallsVariation.LetterAny:
                        return LocalizationDataId.ThrowBalls_lettername_Title; // TODO: get the correct title here
                    case ThrowBallsVariation.Word:
                        return LocalizationDataId.ThrowBalls_word_Title;
                    case ThrowBallsVariation.BuildWord:
                        return LocalizationDataId.ThrowBalls_buildword_Title;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override LetterDataSoundType GetVocabularySoundType()
        {
            LetterDataSoundType soundType;
            switch (Variation) {
                case ThrowBallsVariation.LetterName:
                    soundType = LetterDataSoundType.Name;
                    break;
                default:
                    soundType = LetterDataSoundType.Phoneme;
                    break;
            }
            return soundType;
        }
    }
}
