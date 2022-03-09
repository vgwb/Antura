using Antura.Minigames;
using UnityEngine;

namespace Antura.Minigames.Tobogan
{
    public class TubeAudio
    {
        public float basePitch = 1;
        public float baseVolume = 1;
        public float turnOnSpeed = 4;
        public float turnOffSpeed = 3;

        public bool enable = false;
        IAudioSource source;

        public TubeAudio(IAudioSource source)
        {
            this.source = source;
        }

        public void Stop()
        {
            source.Stop();
        }

        public void Update(float deltaTime)
        {
            var volume = enable ? baseVolume : 0;
            var pitch = enable ? basePitch * 1.5f : basePitch * 1;

            float volumeSpeed = enable ? turnOnSpeed : turnOffSpeed;
            float pitchSpeed = enable ? turnOnSpeed : turnOffSpeed;

            source.Volume = Mathf.Lerp(source.Volume, volume, volumeSpeed * deltaTime);
            source.Pitch = Mathf.Lerp(source.Pitch, pitch, pitchSpeed * deltaTime);

            if (source.Volume < 0.05f && source.IsPlaying)
            {
                source.Stop();
            }
            else if (source.Volume > 0.075f && !source.IsPlaying)
            {
                source.Play();
            }
        }
    }
}
