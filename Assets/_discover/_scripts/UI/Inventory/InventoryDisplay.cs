using System.Collections.Generic;
using UnityEngine;
using DG.DeInspektor.Attributes;

namespace Antura.Discover
{
    public class InventoryDisplay : MonoBehaviour
    {

        public GameObject ItemsContainer;
        public GameObject ItemPrefab;
        public int MaxItems = 5;
        private List<InventoryItem> Items = new List<InventoryItem>();
        private InventoryItem CurrentItemData;

        void Start()
        {
            // clean container
            foreach (Transform child in ItemsContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void AddItem(InventoryItem itemData)
        {
            if (Items.Count >= MaxItems)
            {
                Debug.LogWarning("Inventory is full, cannot add more items.");
                return;
            }

            itemData.Quantity = 1;
            Items.Add(itemData);
            GameObject itemGO = Instantiate(ItemPrefab, ItemsContainer.transform);
            InventoryItemDisplay itemDisplay = itemGO.GetComponent<InventoryItemDisplay>();
            itemDisplay.SetItemData(itemData);

        }

        public void RemoveItem(InventoryItem itemData)
        {
            if (Items.Contains(itemData))
            {
                Items.Remove(itemData);
                // Optionally, you can also destroy the item display here
                // Find the corresponding GameObject and destroy it
                foreach (Transform child in ItemsContainer.transform)
                {
                    InventoryItemDisplay itemDisplay = child.GetComponent<InventoryItemDisplay>();
                    if (itemDisplay != null && itemDisplay.CurrentItemData == itemData)
                    {
                        Destroy(child.gameObject);
                        break;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Item not found in inventory.");
            }


        }

        public void SelectItem(InventoryItem itemData)
        {

        }

        #region Test

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void Test_AddItem()
        {
            AddItem(new InventoryItem());
        }

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void Test_RemoveItem()
        {
            RemoveItem(Items.Count > 0 ? Items[0] : null);
        }
        #endregion

    }
}
