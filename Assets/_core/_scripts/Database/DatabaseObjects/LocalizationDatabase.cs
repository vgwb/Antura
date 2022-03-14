using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for LocalizationData.
    /// </summary>
    public class LocalizationDatabase : AbstractDatabase
    {
        [SerializeField]
        public LocalizationTable table;
    }
}
