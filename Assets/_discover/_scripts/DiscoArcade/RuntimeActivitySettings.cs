using Antura.Discover.Activities;
using UnityEngine;

namespace Antura.Discover
{
    /// <summary>
    /// runtime-only ActivitySettings used for quick-play in DiscoArcade.
    /// </summary>
    public class RuntimeActivitySettings : ActivitySettingsAbstract
    {
        public void InitializeDefaults(ActivityCode code)
        {
            ActivityCode = code;
            Difficulty = Activities.Difficulty.Normal;
            MinRounds = 1;
            MaxRounds = 1;
            HasTimer = false;
            TimerSeconds = 60;
            SelectionMode = SelectionMode.RandomFromTopic;
            name = $"runtime_{code}";
            Id = SanitizeId(name);
            hideFlags = HideFlags.HideAndDontSave;
        }
    }
}
