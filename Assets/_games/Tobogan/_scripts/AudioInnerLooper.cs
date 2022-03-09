using Antura.Minigames;

namespace Antura.Minigames.Tobogan
{
    public class AudioInnerLooper
    {
        float startPerc;
        float endPerc;

        IAudioSource source;

        public bool shouldPlay = false;

        const int START = 1;
        const int LOOP = 2;
        const int STOP = 3;

        int phase = START;

        public AudioInnerLooper(IAudioSource source, float startPerc, float endPerc)
        {
            this.source = source;
            this.startPerc = startPerc;
            this.endPerc = endPerc;
        }

        public void Stop()
        {
            if (source.IsPlaying)
            {
                source.Stop();
            }
        }

        public void Update()
        {
            if (!shouldPlay)
                phase = STOP;
            else
            {
                if (!source.IsPlaying)
                {
                    phase = START;
                    source.Position = 0.0f;
                    source.Loop = false;
                    source.Play();
                }
            }

            if (!source.IsPlaying)
                return;


            var length = source.Duration;

            if (phase == LOOP)
            {
                source.Loop = true;

                if (source.Position >= endPerc * length)
                {
                    source.Position = startPerc * length;
                }
            }
            else
            {
                source.Loop = false;

                if (phase == START)
                {
                    if (source.Position >= startPerc * length)
                    {
                        phase = LOOP;
                        source.Loop = true;
                    }
                }
                else // STOP
                {
                    if (source.IsPlaying &&
                        source.Position < 0.75f * (endPerc * length) &&
                        source.Position >= startPerc * length)
                    {
                        source.Position = endPerc * length;
                    }
                }
            }
        }
    }
}
