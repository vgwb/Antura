using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for MiniGameData. 
    /// </summary>
    public class MiniGameDatabase : ScriptableObject
    {
        [SerializeField]
        public MiniGameTable table;
    }
}