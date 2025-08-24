using UnityEngine;

namespace Antura.Discover.Activities
{
    public class ActivitySettingsAbstract : IdentifiedData
    {
        [Header("Activity Type")]
        public ActivityCode ActivityCode;

        [Header("Duration")]
        public Difficulty Difficulty = Difficulty.Normal;
        public int MinRounds = 1;
        public int MaxRounds = 3;

        [Tooltip("Timer for the activity, if enabled")]
        public bool HasTimer = true;
        [Range(1, 600)]
        public int TimerSeconds = 60;

        [Header("Points")]

        [Tooltip("Points to add when the activity fails")]
        public int PointsFail = -1;
        public int PointsEasy = 1;
        public int PointsNormal = 2;
        public int PointsExpert = 3;

    }
}
