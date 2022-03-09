namespace Antura.Minigames
{
    /// <summary>
    /// Provides access to the SubtitlesWidget UI element for minigames.
    /// <seealso cref="WidgetSubtitles"/>
    /// </summary>
    public interface ISubtitlesWidget
    {
        void DisplaySentence(Database.LocalizationDataId text, float enterDuration = 2, bool showSpeaker = false,
            System.Action onSentenceCompleted = null);

        void Clear();
    }
}
