using UnityEngine;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "CountMoneyDefinition", menuName = "Antura/Activity/Count Money Definition")]
    public class CountMoneyDefinition : ScriptableObject
    {
        public MoneySet MoneySet;

        [Tooltip("If > 0, time in seconds to complete the game.")]
        public float TimeLimit;

        [Tooltip("Difficulty levels: determines how many extra items appear in the tray.")]
        public int MaxDifficulty = 5;

        [Tooltip("Minimum and maximum target amount for random generation.")]
        public Vector2 TargetAmountRange = new Vector2(1f, 20f);

        [Tooltip("If true, target is fixed (e.g., a price). Otherwise, generated.")]
        public bool UseFixedTarget;
        public float FixedTargetAmount;
    }
}
