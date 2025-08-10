using UnityEngine;

namespace Antura.Discover.Activities
{
    public class ActivitySettingsAbstract : ScriptableObject
    {
        public string Id;
        public Difficulty Difficulty = Difficulty.Normal;
    }
}
