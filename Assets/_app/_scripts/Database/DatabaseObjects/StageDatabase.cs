using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for StageData. 
    /// </summary>
    public class StageDatabase : ScriptableObject
    {
        [SerializeField]
        public StageTable table;
    }
}