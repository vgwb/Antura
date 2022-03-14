namespace Antura.Minigames
{
    /// <summary>
    /// Provides access to an Audio Source for minigames.
    /// <seealso cref="MinigamesAudioSource"/>
    /// </summary>
    public interface IAudioSource
    {
        bool Loop { get; set; }
        float Pitch { get; set; }
        float Volume { get; set; }
        float Duration { get; }
        float Position { get; set; }

        bool IsPlaying { get; }

        bool IsLoaded { get; }

        void Play();
        void Pause();
        void Stop();
    }
}
