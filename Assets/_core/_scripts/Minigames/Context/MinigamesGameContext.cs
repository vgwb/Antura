namespace Antura.Minigames
{
    /// <summary>
    /// Concrete implementation of IGameContext. Accessible to minigames.
    /// </summary>
    public class MinigamesGameContext : IGameContext
    {
        public MiniGameCode Code { get; private set; }

        private IAudioManager audioManager = new MinigamesAudioManager();
        private IInputManager inputManager = new MinigamesInputManager();

        private ISubtitlesWidget subtitleWidget = new MinigamesSubtitlesWidget();
        private IStarsWidget starsWidget = new MinigamesStarsWidget();
        private IPopupWidget questionWidget = new MinigamesPopupWidget();
        private ICheckmarkWidget checkmarkWidget = new MinigamesCheckmarkWidget();
        private IOverlayWidget overlayWidget = new MinigamesOverlayWidget();
        private ILogManager logManager;

        public MinigamesGameContext(MiniGameCode code, string sessionName)
        {
            Code = code;
            logManager = new MinigamesLogManager(code, sessionName);
        }

        public IAudioManager GetAudioManager()
        {
            return audioManager;
        }

        public IInputManager GetInputManager()
        {
            return inputManager;
        }

        public ILogManager GetLogManager()
        {
            return logManager;
        }

        public IStarsWidget GetStarsWidget()
        {
            return starsWidget;
        }

        public ISubtitlesWidget GetSubtitleWidget()
        {
            return subtitleWidget;
        }

        public IPopupWidget GetPopupWidget()
        {
            return questionWidget;
        }

        public void Reset()
        {
            overlayWidget.Reset();
            audioManager.Reset();
            inputManager.Reset();
        }

        public ICheckmarkWidget GetCheckmarkWidget()
        {
            return checkmarkWidget;
        }

        public IOverlayWidget GetOverlayWidget()
        {
            return overlayWidget;
        }
    }
}
