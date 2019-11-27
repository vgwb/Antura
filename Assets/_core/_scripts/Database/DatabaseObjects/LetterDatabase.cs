using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for LetterData. 
    /// </summary>
    public class LetterDatabase : ScriptableObject
    {
        [SerializeField]
        public LetterTable table;
    }
}