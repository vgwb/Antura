using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for RewardData.
    /// </summary>
    public class RewardDatabase : AbstractDatabase
    {
        [SerializeField]
        public RewardTable table;
    }
}
