namespace Antura.Core
{
    /// <summary>
    /// we refer to app scenes by these enums, and not by scene names, that could change anytime
    /// </summary>
    public enum AppScene
    {
        Bootstrap = 15,
        Home = 1,
        AnturaSpace = 2,
        Book = 3,
        ReservedArea = 4,
        GameSelector = 5,
        Intro = 6,
        MiniGame = 7,
        Map = 8,
        Mood = 9,
        PlaySessionResult = 10,
        PlayerCreation = 11,
        Rewards = 12,
        Ending = 13,
        DailyReward = 14,
        Kiosk = 16,

        NONE = 100
    }
}
