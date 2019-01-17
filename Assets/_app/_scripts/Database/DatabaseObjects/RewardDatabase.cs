using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for RewardData. 
    /// </summary>
    public class RewardDatabase : ScriptableObject
    {
        [SerializeField]
        public RewardTable table;
    }
}