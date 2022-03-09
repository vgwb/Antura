using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for PhraseData.
    /// </summary>
    public class PhraseDatabase : AbstractDatabase
    {
        [SerializeField]
        public PhraseTable table;
    }
}
