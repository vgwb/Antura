using System;
using Antura.Core;
using Antura.Database;
using Antura.Teacher;

namespace Antura.Minigames.ThrowBalls
{
    public enum ThrowBallsVariation
    {
        LetterName = MiniGameCode.ThrowBalls_lettername,
        LetterAny = MiniGameCode.ThrowBalls_letterany,
        Word = MiniGameCode.ThrowBalls_word,
        BuildWord = MiniGameCode.ThrowBalls_buildword,
        Image = MiniGameCode.ThrowBalls_image,
        LetterForm = MiniGameCode.ThrowBalls_letterform
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
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 10;
            int nWrong = 4;

            var builderParams = InitQuestionBuilderParamaters();
            switch (Variation) {
                case ThrowBallsVariation.LetterName:
                    builderParams.letterFilters.excludeDiacritics = LetterFilters.ExcludeDiacritics.All;
                    builder = new RandomLettersQuestionBuilder(nPacks, 1, nWrong: nWrong, firstCorrectIsQuestion: true, parameters: builderParams);
                    break;
                case ThrowBallsVariation.LetterAny:
                {
                    var letterAlterationFilters = LetterAlterationFilters.FormsAndPhonemesOfMultipleLetters;
                    if (AppManager.I.ContentEdition.DiacriticsOnlyOnIsolated) letterAlterationFilters = LetterAlterationFilters.PhonemesOfMultipleLetters;
                    builder = new RandomLetterAlterationsQuestionBuilder(nPacks, 1, nWrong: nWrong, letterAlterationFilters: letterAlterationFilters, parameters: builderParams);
                }
                    break;
                case ThrowBallsVariation.Word:
                case ThrowBallsVariation.Image:
                    builderParams.wordFilters.requireDrawings = true;
                    builder = new RandomWordsQuestionBuilder(nPacks, 1, nWrong, firstCorrectIsQuestion: true, parameters: builderParams);
                    break;
                case ThrowBallsVariation.BuildWord:
                    builderParams.wordFilters.requireDrawings = true;
                    builderParams.wordFilters.excludeDipthongs = true;
                    builderParams.letterFilters.includeSpecialCharacters = true;
                    builder = new LettersInWordQuestionBuilder(7, maximumWordLength: 7, nWrong: nWrong, useAllCorrectLetters: true, parameters: builderParams);
                    break;
                case ThrowBallsVariation.LetterForm:
                {
                    var letterAlterationFilters = LetterAlterationFilters.FormsOfMultipleLetters;
                    builder = new RandomLetterAlterationsQuestionBuilder(nPacks, 1, nWrong: nWrong, letterAlterationFilters: letterAlterationFilters, parameters: builderParams);
                }
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

        public override bool AutoPlayIntro => false;

        public override LetterDataSoundType GetVocabularySoundType()
        {
            LetterDataSoundType soundType;
            switch (Variation) {

                case ThrowBallsVariation.LetterName:
                    soundType = LetterDataSoundType.Name;
                    break;
                case ThrowBallsVariation.LetterForm:
                    soundType = AppManager.I.ContentEdition.PlayNameSoundWithForms ? LetterDataSoundType.Name : LetterDataSoundType.Phoneme;
                    break;
                default:
                    soundType = LetterDataSoundType.Phoneme;
                    break;
            }
            return soundType;
        }
    }
}
