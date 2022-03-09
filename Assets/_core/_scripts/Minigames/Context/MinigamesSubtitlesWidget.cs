using Antura.UI;

namespace Antura.Minigames
{
    /// <summary>
    /// Concrete implementation of ISubtitlesWidget. Accessible to minigames.
    /// </summary>
    public class MinigamesSubtitlesWidget : ISubtitlesWidget
    {
        public void DisplaySentence(Database.LocalizationDataId text, float enterDuration, bool showSpeaker,
            System.Action onSentenceCompleted)
        {
            WidgetSubtitles.I.DisplayDialogue(text, enterDuration, showSpeaker, onSentenceCompleted);
        }

        public void Clear()
        {
            WidgetSubtitles.I.Close();
        }
    }
}
