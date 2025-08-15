using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [Serializable]
    public struct MelodyEvent
    {
        public NoteName Note;
        public int Octave;
        public NoteDuration Duration;
        public bool IsRest;
    }

    [CreateAssetMenu(fileName = "PianoSettingsData", menuName = "Antura/Activity/Piano Settings")]
    public class PianoSettingsData : ActivitySettingsAbstract
    {
        public int tempoBPM = 100;
        public List<MelodyEvent> sequence = new List<MelodyEvent>();
    }
}
