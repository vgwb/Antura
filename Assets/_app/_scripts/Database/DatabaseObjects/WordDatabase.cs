using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for WordData. 
    /// </summary>
    public class WordDatabase : ScriptableObject
    {
        [SerializeField]
        public WordTable table;
    }
}