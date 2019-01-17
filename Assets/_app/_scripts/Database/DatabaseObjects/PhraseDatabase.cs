using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for PhraseData. 
    /// </summary>
    public class PhraseDatabase : ScriptableObject
    {
        [SerializeField]
        public PhraseTable table;
    }
}