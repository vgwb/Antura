using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for WordData.
    /// </summary>
    public class WordDatabase : AbstractDatabase
    {
        [SerializeField]
        public WordTable table;
    }
}
