using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for StageData.
    /// </summary>
    public class StageDatabase : AbstractDatabase
    {
        [SerializeField]
        public StageTable table;
    }
}
