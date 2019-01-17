namespace Antura.Minigames
{
    /// <summary>
    /// Data passed to a minigame to configure it. 
    /// </summary>
    public class MinigameLaunchConfiguration
    {
        public float Difficulty;
        public int NumberOfRounds;
        public bool TutorialEnabled;

        public MinigameLaunchConfiguration(float _Difficulty = 0, int _NumberOfRounds = 1, bool tutorialEnabled = true)
        {
            Difficulty = _Difficulty;
            NumberOfRounds = _NumberOfRounds;
            TutorialEnabled = tutorialEnabled;
        }
    }
}