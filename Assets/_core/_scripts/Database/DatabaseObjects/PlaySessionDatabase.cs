using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for PlaySessionData.
    /// </summary>
    public class PlaySessionDatabase : AbstractDatabase
    {
        [SerializeField]
        public PlaySessionTable table;
    }
}
