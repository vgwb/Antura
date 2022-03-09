namespace Antura
{
    /// <summary>
    /// Defines a type of app-wide event that may happen.
    /// Used for logging.
    /// </summary>
    public enum InfoEvent
    {
        ProfileCreated = 1,
        EnterScene = 10,

        AppSessionStart = 20,
        AppSessionEnd = 21,
        AppPlay = 22,
        AppSuspend = 23,
        AppResume = 24,
        Book = 30,
        GameStart = 40,
        GameEnd = 41,
        Reward = 50,
        AnturaCustomization = 51,
        AnturaSpace = 60,

        DailyRewardReceived = 70,
    }

    /// <summary>
    /// Defines a type of play-related skill that may be measured.
    /// Used for logging.
    /// </summary>
    public enum PlaySkill
    {
        None = 0,
        Timing = 1,
        Precision = 2,
        Memory = 3,
        Logic = 4,
        Observation = 5,
        Listening = 6
    }

    public enum PlayEvent
    {
        Skill = 0
    }
}
