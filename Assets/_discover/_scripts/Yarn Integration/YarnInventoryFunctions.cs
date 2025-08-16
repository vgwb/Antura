using UnityEngine;
using Yarn.Unity;

namespace Antura.Discover
{
    public static class YarnInventoryFunctions
    {
        [YarnFunction("item_count")]
        public static float ItemCount(string itemCode)
        {
            var inv = QuestManager.I?.inventory;
            if (inv == null || string.IsNullOrEmpty(itemCode))
                return 0f;
            return inv.GetItemCount(itemCode);
        }

        [YarnFunction("has_item")]
        public static bool HasItem(string itemCode)
        {
            var inv = QuestManager.I?.inventory;
            if (inv == null || string.IsNullOrEmpty(itemCode))
                return false;
            return inv.HasItem(itemCode);
        }

        [YarnFunction("has_item_at_least")]
        public static bool HasItemAtLeast(string itemCode, float minQty)
        {
            var inv = QuestManager.I?.inventory;
            if (inv == null || string.IsNullOrEmpty(itemCode))
                return false;
            int min = Mathf.Max(1, Mathf.RoundToInt(minQty));
            return inv.HasItem(itemCode, min);
        }

        [YarnFunction("can_collect")]
        public static bool CanCollect(string itemCode)
        {
            var inv = QuestManager.I?.inventory;
            if (inv == null || string.IsNullOrEmpty(itemCode))
                return false;
            return inv.CanCollect(itemCode);
        }
    }
}
