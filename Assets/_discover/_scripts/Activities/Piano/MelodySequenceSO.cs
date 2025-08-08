using System.Collections.Generic;
using UnityEngine;
namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "MelodySequence", menuName = "PianoActivity/Melody Sequence", order = 0)]
    public class MelodySequenceSO : ScriptableObject
    {
        public int tempoBPM = 100;
        public List<MelodyEvent> sequence = new List<MelodyEvent>();
    }
}
