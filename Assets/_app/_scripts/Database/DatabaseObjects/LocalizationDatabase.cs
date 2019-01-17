using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for LocalizationData. 
    /// </summary>
    public class LocalizationDatabase : ScriptableObject
    {
        [SerializeField]
        public LocalizationTable table;
    }
}