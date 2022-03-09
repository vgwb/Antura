namespace Antura.Minigames
{
    /// <summary>
    /// Interface for a generic timer.
    /// </summary>
    // refactor: should be grouped with other utilities
    public interface ITimer
    {
        bool IsRunning { get; }
        float Time { get; }

        void Start();
        void Stop();
        void Reset();

        void Update(float delta);
    }
}
