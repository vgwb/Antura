using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    /// <summary>
    /// Project-wide list of Bonus/Malus definitions for indexing and validation.
    /// </summary>
    [CreateAssetMenu(fileName = "BonusMalusListData", menuName = "Antura/Discover/Bonus-Malus Library")]
    public class BonusMalusListData : ScriptableObject
    {
        public List<BonusMalusData> Items = new();
    }
}
