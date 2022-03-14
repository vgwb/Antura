namespace Antura.LivingLetters
{
    /// <summary>
    /// Interface for a provider of ILivingLetterData.
    /// Used by minigames to access dictionary content with no specific structure.
    /// </summary>
    public interface ILivingLetterDataProvider
    {
        ILivingLetterData GetNextData();
    }
}
