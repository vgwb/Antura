using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Antura.Discover
{
    public enum ItemTag
    {
        None = 0,
        Custom = 1,
        Coin = 2,
        Flag = 3,
        Food = 4,
        Key = 5
    }

    [CreateAssetMenu(fileName = "ItemData", menuName = "Antura/Discover/Inventory Item")]
    public class ItemData : IdentifiedData
    {
        public LocalizedString Name;
        public ItemTag Tag;
        [Tooltip("Optional tag if set to Custom.")]
        public string CustomTag;
        public Sprite Icon;
    }
}
