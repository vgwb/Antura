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
        Key = 5,
        Animal = 6,
    }

    [CreateAssetMenu(fileName = "ItemIconData", menuName = "Antura/Discover/Inventory Icon Item")]
    public class ItemData : IdentifiedData
    {
        public LocalizedString Name;
        public ItemTag Tag;
        public Sprite Icon;
    }
}
