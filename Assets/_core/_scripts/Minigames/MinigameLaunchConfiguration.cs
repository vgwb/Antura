namespace Antura.Minigames
{
    /// <summary>
    /// Data passed to a minigame to configure it.
    /// </summary>
    [System.Serializable]
    public class MinigameLaunchConfiguration
    {
        public float Difficulty;
        public int NumberOfRounds;
        public bool TutorialEnabled;
        public bool InsideJourney;   // Current journey: use data up to the current reached play session
        public bool DirectGame;
        public bool IgnoreJourney;   // Use all data, regardless of journey

        public MinigameLaunchConfiguration(float difficulty = 0, int numberOfRounds = 1, bool tutorialEnabled = true, bool insideJourney = true, bool directGame = false, bool ignoreJourney = false)
        {
            Difficulty = difficulty;
            NumberOfRounds = numberOfRounds;
            TutorialEnabled = tutorialEnabled;
            InsideJourney = insideJourney;
            DirectGame = directGame;
            IgnoreJourney = ignoreJourney;
        }

        public override string ToString()
        {
            return "Launch Config: "
                   + "\nDifficulty: " + Difficulty
                   + "\nInside Current Journey: " + InsideJourney
                   + "\nDirect Game: " + DirectGame
                   + "\nIgnore Journey Data: " + IgnoreJourney;
        }
    }
}
