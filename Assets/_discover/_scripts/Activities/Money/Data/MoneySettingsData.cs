using UnityEngine;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "MoneySettingsData", menuName = "Antura/Activity/Money Settings")]
    public class MoneySettingsData : ActivitySettingsAbstract
    {
        public MoneySet MoneySet;

        [Tooltip("If > 0, starts the timer.")]
        public float TimeLimit;

        [Tooltip("Difficulty levels: determines how many extra items appear in the tray.")]
        public int MaxDifficulty = 5;

        [Tooltip("Minimum target amount for random generation.")]
        public float RangeMin = 1f;
        [Tooltip("maximum target amount for random generation.")]
        public float RangeMax = 20f;

        [Tooltip("If true, target is fixed . Otherwise, generated.")]
        public bool UseFixedTarget;
        public float FixedTargetAmount;

        [Header("Tray Generation")]
        [Tooltip("How many copies to spawn for each base combo item (minimum 2). Includes the original.")]
        public int CopiesPerBaseItem = 3;
        [Tooltip("Extra copies of the smallest denomination to add to ensure the target is reachable in many ways.")]
        public int ExtraCopiesSmallest = 6;
        [Tooltip("Maximum number of tokens to spawn in the tray to avoid excess.")]
        public int MaxTrayTokens = 48;
    }
}
