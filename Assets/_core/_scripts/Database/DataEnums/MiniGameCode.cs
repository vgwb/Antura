namespace Antura
{
    /// <summary>
    /// Enumerator specifying a minigame (or minigame variation) that is supported by the core application.
    /// </summary>
    // TODO refactor: this enum depends on the specific implemented minigames and should be grouped with them
    // last is 39 and 115
    public enum MiniGameCode
    {
        Invalid = 0,
        Balloons_counting = 6,
        Balloons_image = 42,
        Balloons_letterinword = 7,
        Balloons_spelling = 3, // DISABLED
        Balloons_word = 8,

        ColorTickle_image = 45,
        ColorTickle_lettername = 9,

        DancingDots_lettername = 10, // DISABLED
        DancingDots_letterany = 36, // NEW
        Egg_image = 40,
        Egg_lettername = 11,
        Egg_letterphoneme = 37, // NEW
        Egg_buildword = 34,

        FastCrowd_alphabet = 12, // DISABLED
        FastCrowd_counting = 13,
        FastCrowd_image = 43,
        FastCrowd_lettername = 14,
        FastCrowd_letterform = 38, // NEW
        FastCrowd_buildword = 1,
        FastCrowd_word = 4,
        FastCrowd_categoryform = 60,
        FastCrowd_orderedimage_numbers = 61,
        FastCrowd_orderedimage_colors = 62,
        FastCrowd_orderedimage_months = 63,
        FastCrowd_orderedimage_days_seasons = 64,

        HideSeek_image = 41,
        HideSeek_lettername = 48,
        HideSeek_letterphoneme = 16,
        MakeFriends_letterinword = 17,
        Maze_lettername = 18,
        MissingLetter_image = 49,
        MissingLetter_letter = 19, // DISABLED
        MissingLetter_letterform = 39, // NEW
        MissingLetter_phrase = 20, // DISABLED
        MissingLetter_letterinword = 35,
        MixedLetters_alphabet = 21, // DISABLED
        MixedLetters_buildword = 22,
        ReadingGame_word = 24, // DISABLED
        Scanner_word = 25, // DISABLED
        Scanner_phrase = 26, // DISABLED
        SickLetters_lettername = 23,

        Song_alphabet = 5,
        Song_diacritics = 33, // DISABLED
        Song_word_animals = 46,
        Song_word_nature = 47,
        Song_word_home = 50,
        Song_word_objectsclothes = 51,
        Song_word_city = 52,
        Song_word_family = 53,
        Song_word_food = 54,
        Song_word_body = 55,

        TakeMeHome_lettername = 31, // DISABLED
        ThrowBalls_image = 44,
        ThrowBalls_lettername = 27,
        ThrowBalls_letterany = 15, // NEW
        ThrowBalls_buildword = 32,
        ThrowBalls_word = 28,
        ThrowBalls_multiletterform = 65,
        Tobogan_letterinword = 29, // DISABLED
        Tobogan_sunmoon = 30, // DISABLED

        Assessment_LetterName = 100,
        Assessment_LetterAny = 115, // NEW
        Assessment_WordsWithLetter = 101,
        Assessment_MatchLettersToWord = 102,
        Assessment_MatchLettersToWord_Form = 114,
        Assessment_CompleteWord = 103,
        Assessment_CompleteWord_Form = 113,
        Assessment_OrderLettersOfWord = 104,
        Assessment_VowelOrConsonant = 105, // DISABLED
        Assessment_SelectPronouncedWord = 106,
        Assessment_MatchWordToImage = 107,
        Assessment_WordArticle = 108,
        Assessment_SingularDualPlural = 109,
        Assessment_SunMoonWord = 110,
        Assessment_SunMoonLetter = 111,
        Assessment_QuestionAndReply = 112,
        Assessment_SelectPronouncedWordByImage = 116
    }
}
