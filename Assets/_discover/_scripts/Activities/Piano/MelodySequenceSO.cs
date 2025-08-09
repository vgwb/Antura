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

    [CreateAssetMenu(fileName = "MelodySequence", menuName = "Antura/Activity/Piano Melody Sequence", order = 0)]
    public class MelodySequenceSO : ScriptableObject
    {
        public int tempoBPM = 100;
        public List<MelodyEvent> sequence = new List<MelodyEvent>();
    }
}
