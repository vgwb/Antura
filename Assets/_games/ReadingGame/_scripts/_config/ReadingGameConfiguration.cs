using System;
using Antura.Database;
using Antura.Teacher;

namespace Antura.Minigames.ReadingGame
{
    public enum ReadingGameVariation
    {
        ReadingGame_Words = MiniGameCode.ReadingGame_word,
        SongAlphabet = MiniGameCode.Song_alphabet,
        SongDiacritics = MiniGameCode.Song_diacritics,
        Song_Word_Animals = MiniGameCode.Song_word_animals,
        Song_Word_Nature = MiniGameCode.Song_word_nature,
        Song_Word_Home = MiniGameCode.Song_word_home,
        Song_Word_ObjectsClothes = MiniGameCode.Song_word_objectsclothes,
        Song_Word_City = MiniGameCode.Song_word_city,
        Song_Word_Family = MiniGameCode.Song_word_family,
        Song_Word_Food = MiniGameCode.Song_word_food,
        Song_Word_Body = MiniGameCode.Song_word_body,
    }

    public class ReadingGameConfiguration : AbstractGameConfiguration
    {
        public enum GameType
        {
            FollowReading,
            FollowSong,
            SimonSong,
            ReadAndListen,
        }

        public ReadingGameVariation Variation { get; private set; }

        public override void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (ReadingGameVariation)code;
        }

        // Singleton Pattern
        static ReadingGameConfiguration instance;

        public static ReadingGameConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReadingGameConfiguration();
                }

                return instance;
            }
        }

        private ReadingGameConfiguration()
        {
            // Default values
            Questions = new SampleReadingGameQuestionProvider();
            Variation = ReadingGameVariation.ReadingGame_Words;
            //Variation = ReadingGameVariation.AlphabetSong;

            Context = new MinigamesGameContext(MiniGameCode.ReadingGame_word, System.DateTime.Now.Ticks.ToString());
            TutorialEnabled = true;
        }

        public override IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            var builderParams = InitQuestionBuilderParamaters();
            switch (Variation)
            {
                case ReadingGameVariation.SongAlphabet:
                case ReadingGameVariation.SongDiacritics:
                    builder = new EmptyQuestionBuilder();
                    break;
                case ReadingGameVariation.ReadingGame_Words:
                    builderParams.wordFilters.requireDrawings = true;
                    builderParams.phraseFilters.requireAnswersOrWords = true;
                    builder = new WordsInPhraseQuestionBuilder(nPacks: 10, nCorrect: 1, nWrong: 6,
                        usePhraseAnswersIfFound: true, parameters: builderParams);
                    break;
                case ReadingGameVariation.Song_Word_Animals:
                    builder = CreateCategoryGameBuilder(builderParams, WordDataCategory.Animals);
                    break;
                case ReadingGameVariation.Song_Word_Nature:
                    builder = CreateCategoryGameBuilder(builderParams, WordDataCategory.Nature);
                    break;
                case ReadingGameVariation.Song_Word_Home:
                    builder = CreateCategoryGameBuilder(builderParams, WordDataCategory.Home);
                    break;
                case ReadingGameVariation.Song_Word_ObjectsClothes:
                    builder = CreateCategoryGameBuilder(builderParams, WordDataCategory.Objects, WordDataCategory.Clothes);
                    break;
                case ReadingGameVariation.Song_Word_City:
                    builder = CreateCategoryGameBuilder(builderParams, WordDataCategory.City);
                    break;
                case ReadingGameVariation.Song_Word_Family:
                    builder = CreateCategoryGameBuilder(builderParams, WordDataCategory.Family);
                    break;
                case ReadingGameVariation.Song_Word_Food:
                    builder = CreateCategoryGameBuilder(builderParams, WordDataCategory.Food);
                    break;
                case ReadingGameVariation.Song_Word_Body:
                    builder = CreateCategoryGameBuilder(builderParams, WordDataCategory.Body);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return builder;
        }

        private IQuestionBuilder CreateCategoryGameBuilder(QuestionBuilderParameters builderParams, params WordDataCategory[] categories)
        {
            builderParams.wordFilters.allowedCategories = categories;
            builderParams.wordFilters.requireDrawings = true;
            return new RandomWordsQuestionBuilder(nPacks: 10, nCorrect: 1, nWrong: 2,
                parameters: builderParams,
                firstCorrectIsQuestion: true);
        }

        public override MiniGameLearnRules SetupLearnRules()
        {
            var rules = new MiniGameLearnRules();
            // example: a.minigameVoteSkewOffset = 1f;
            return rules;
        }

        public bool ShowTimer => CurrentGameType == GameType.FollowReading || CurrentGameType == GameType.ReadAndListen;

        public GameType CurrentGameType
        {
            get
            {
                switch (Variation)
                {
                    case ReadingGameVariation.ReadingGame_Words:
                        return GameType.ReadAndListen;
                    case ReadingGameVariation.SongAlphabet:
                    case ReadingGameVariation.SongDiacritics:
                        return GameType.FollowSong;
                    case ReadingGameVariation.Song_Word_Animals:
                    case ReadingGameVariation.Song_Word_Nature:
                    case ReadingGameVariation.Song_Word_Home:
                    case ReadingGameVariation.Song_Word_ObjectsClothes:
                    case ReadingGameVariation.Song_Word_City:
                    case ReadingGameVariation.Song_Word_Family:
                    case ReadingGameVariation.Song_Word_Food:
                    case ReadingGameVariation.Song_Word_Body:
                        return GameType.SimonSong;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override bool AutoPlayIntro => false;

    }
}
