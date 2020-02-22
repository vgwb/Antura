using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for MiniGameData. 
    /// </summary>
    public class MiniGameDatabase : AbstractDatabase
    {
        [SerializeField]
        public MiniGameTable table;
    }
}