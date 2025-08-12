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
        Ingredient = 4,
    }

    [CreateAssetMenu(menuName = "Antura/Discover/Inventory Item")]
    public class ItemData : ScriptableObject
    {
        [Tooltip("Unique, stable ID. lowercase, Never change after shipping.")]
        public string Id;
        public LocalizedString Name;
        public ItemTag Tag;
        [Tooltip("Optional tag if set to Custom.")]
        public string CustomTag;
        public Sprite Icon;
    }
}
