using UnityEngine;

namespace Antura.Discover.Activities
{

    public enum MoneyType
    {
        Coin = 0,
        Paper = 1
    }

    public enum MoneyGenerationType
    {
        Automatic = 0,
        Manual = 1,
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
        public MoneyGenerationType GenType = MoneyGenerationType.Automatic;

        [System.Serializable]
        public class ManualMoneyConfig
        {
            [Tooltip("Value of the coin/bill")]
            public float Value;
            [Tooltip("Type (Coin/Paper)")]
            public MoneyType Type;
            [Tooltip("How many to spawn")]
            public int Count;
        }
        public System.Collections.Generic.List<ManualMoneyConfig> ManualComposition;

        [Tooltip("Maximum number of tokens to spawn in the tray to avoid excess.")]
        public int GenMaxTokens = 12;
    }
}
