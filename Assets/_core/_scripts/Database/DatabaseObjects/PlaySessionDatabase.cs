using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Custom asset container for PlaySessionData. 
    /// </summary>
    public class PlaySessionDatabase : ScriptableObject
    {
        [SerializeField]
        public PlaySessionTable table;
    }
}