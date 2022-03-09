namespace Antura.Minigames
{
    /// <summary>
    /// Concrete implementation of ITimer.
    /// Used by minigames.
    /// </summary>
    public class Timer : ITimer
    {
        private float time;
        public bool IsRunning { get; private set; }

        public float Time
        {
            get { return time; }
        }

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }


        public void Reset()
        {
            Stop();
            time = 0;
        }

        public void Update(float delta)
        {
            if (IsRunning)
            {
                time += delta;
            }
        }
    }
}
