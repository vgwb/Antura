using UnityEngine;

namespace Antura.Discover.Activities
{

    public enum MoneyType
    {
        Coin = 0,
        Paper = 1
    }

    public enum MoneyTypeFilter
    {
        Both = 0,
        CoinOnly = 1,
        BanknoteOnly = 2,
    }

    [CreateAssetMenu(fileName = "MoneySettingsData", menuName = "Antura/Activity/Money Settings")]
    public class MoneySettingsData : ActivitySettingsAbstract
    {
        private void OnEnable()
        {
            ActivityCode = ActivityCode.MoneyCount;
        }

        [Header("--- Activity Money Settings")]
        public MoneySet MoneySet;


        [Tooltip("If true, target is fixed. Otherwise, random generated.")]
        public bool UseFixedTarget;
        public float FixedTargetAmount;

        [Header("Random Target Generation")]
        [Tooltip("Minimum target amount for random generation.")]
        public float RangeMin = 1f;
        [Tooltip("maximum target amount for random generation.")]
        public float RangeMax = 20f;

        [Header("Tray Generation")]
        public MoneyTypeFilter GenType = MoneyTypeFilter.Both;

        [Tooltip("How many copies to spawn for each base combo item (minimum 2). Includes the original.")]
        public int GenDuplicates = 3;
        [Tooltip("Extra copies of the smallest denomination to add to ensure the target is reachable in many ways.")]
        public int GenExtraCopies = 6;
        [Tooltip("Maximum number of tokens to spawn in the tray to avoid excess.")]
        public int GenMaxTokens = 12;
    }
}
