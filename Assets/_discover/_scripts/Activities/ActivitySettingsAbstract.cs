using UnityEditor;
using UnityEngine;

namespace Antura.Discover.Activities
{

    public enum SelectionMode
    {
        ManualSet,
        RandomFromTopic,
    }

    public class ActivitySettingsAbstract : IdentifiedData
    {
        [ReadOnly]
        public ActivityCode ActivityCode;

        [Header("Duration")]
        public Difficulty Difficulty = Difficulty.Normal;
        public int MinRounds = 1;
        public int MaxRounds = 3;

        [Tooltip("Timer for the activity, if enabled")]
        public bool HasTimer = true;
        [Range(1, 600)]
        public int TimerSeconds = 120;

        [Header("Points")]

        [Tooltip("Points to add when the activity fails")]
        [HideInInspector]
        public int PointsFail = -1;
        [HideInInspector]
        public int PointsEasy = 1;
        [HideInInspector]
        public int PointsNormal = 2;
        [HideInInspector]
        public int PointsExpert = 3;

        [Header("Topic")]
        public TopicData MainTopic;
        [Tooltip("How to select the cards for the activity")]
        public SelectionMode SelectionMode;
    }
}
