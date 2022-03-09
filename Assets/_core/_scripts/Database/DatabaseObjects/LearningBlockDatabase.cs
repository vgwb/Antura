using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for LearningBlockData.
    /// </summary>
    public class LearningBlockDatabase : AbstractDatabase
    {
        [SerializeField]
        public LearningBlockTable table;
    }
}
