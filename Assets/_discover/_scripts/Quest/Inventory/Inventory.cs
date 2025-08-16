using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Antura.Core;
using Antura.Utilities;

namespace Antura.Discover
{
    public class Inventory
    {
        static int MaxItemsInInventory = 5;

        public List<InventoryItem> Items { get; private set; } = new List<InventoryItem>();


        private HashSet<string> collectedItems;

        private int total_items;

        public Inventory()
        {
            collectedItems = new HashSet<string>();
        }

        public void Init(int maxItems)
        {
            total_items = maxItems;
        }

        public bool CollectItem(string itemCode)
        {
            YarnAnturaManager.I.Runner.VariableStorage.SetValue("$CURRENT_ITEM", itemCode);

            if (collectedItems.Contains(itemCode))
            {
                Debug.Log($"Item {itemCode} already collected.");
                return false;
            }
            else
            {
                collectedItems.Add(itemCode);
                Debug.Log($"Item {itemCode} collected successfully.");
                return true;
            }


        }

        public bool RemoveItem(string itemCode)
        {
            if (collectedItems.Contains(itemCode))
            {
                collectedItems.Remove(itemCode);
                Debug.Log($"Item {itemCode} removed successfully.");
                return true;
            }
            else
            {
                Debug.LogWarning($"Item {itemCode} not found in inventory.");
                return false;
            }
        }
    }
}
