using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class ActivityPiano : ActivityBase
    {

        public enum PlayMode
        {
            Free = 0,
            Easy = 1, // Normal (ear and play)
            Normal = 2, // Difficult (just sound)
            Difficult = 3, // Normal (ear and play)
            Expert = 4, // Difficult (just sound)
        }

        [Header("Activity Piano Settings")]
        public PlayMode Mode = PlayMode.Free;


        void Start()
        {

        }

    }
}
