using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class ActivityMoney : ActivityBase
    {
        public enum MoneyKind
        {
            Both = 0,
            CoinOnly = 1,
            BanknoteOnly = 2,
        }

        [Header("Activity Money")]
        [Tooltip("Target amount of money to collect")]
        public float MoneyTarget = 10f;
        [Tooltip("Difficulty: the number of coins to use.")]
        public int DifficultyLevel = 1;
        [Tooltip("Kind of money to use in the activity")]
        public MoneyKind Kind = MoneyKind.Both;

        void Awake()
        {

        }

        void Start()
        {
            //base.Start();
            // Initialize any additional properties or states here
        }

    }


}
