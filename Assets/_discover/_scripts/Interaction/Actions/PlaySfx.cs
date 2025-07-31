using System.Collections;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry.Interaction
{
    public class PlaySfx : ActionAbstract
    {
        public AudioClip SoundFx;
        public AudioSource audioSource;

        void Start()
        {
            if (SoundFx != null)
            {
                audioSource.clip = SoundFx;
                audioSource.playOnAwake = false;
                audioSource.loop = false;
            }
        }

        public override void OnTrigger()
        {
            audioSource.Play();
            Debug.Log("Playing SFX: " + SoundFx.name);
        }
    }
}
