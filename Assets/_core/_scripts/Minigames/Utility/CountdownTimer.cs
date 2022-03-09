namespace Antura.Minigames
{
    /// <summary>
    /// Concrete implementation of ITimer that counts down and triggers an event when the time is up.
    /// Used by minigames.
    /// </summary>
    public class CountdownTimer : ITimer
    {
        public event System.Action onTimesUp;

        private float time;
        public bool IsRunning { get; private set; }
        public float Duration { get; private set; }

        public float CurrentPercentage
        {
            get { return Time / Duration; }
        }

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
            time = Duration;
        }

        public void Reset(float newDuration)
        {
            Duration = newDuration;
            Stop();
            time = Duration;
        }

        public CountdownTimer(float duration)
        {
            Duration = duration;
            Reset();
        }

        public void Update(float delta)
        {
            if (IsRunning)
            {
                time -= delta;

                if (time <= 0)
                {
                    time = 0;
                    Stop();

                    if (onTimesUp != null)
                    {
                        onTimesUp();
                    }
                }
            }
        }
    }
}
