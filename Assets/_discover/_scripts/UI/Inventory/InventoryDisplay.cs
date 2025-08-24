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
        private readonly List<InventoryItem> Items = new List<InventoryItem>();
        private InventoryItem CurrentItemData;
        private readonly Dictionary<string, GameObject> itemViews = new Dictionary<string, GameObject>();

        void Start()
        {
            // clean container
            foreach (Transform child in ItemsContainer.transform)
            {
                Destroy(child.gameObject);
            }
            // Subscribe to inventory events
            if (QuestManager.I != null && QuestManager.I.Inventory != null)
            {
                Debug.Log("InventoryDisplay: Subscribing to inventory events.");
                var inv = QuestManager.I.Inventory;
                inv.OnItemAdded += HandleItemAdded;
                inv.OnItemRemoved += HandleItemRemoved;
                inv.OnItemUpdated += HandleItemUpdated;
                inv.OnSelectionChanged += HandleSelectionChanged;
                // Initialize from existing items if any
                foreach (var it in inv.Items)
                    HandleItemAdded(it);
                HandleSelectionChanged(inv.CurrentItem);
            }
        }

        public void AddItem(InventoryItem itemData)
        {
            if (Items.Count >= MaxItems)
            {
                Debug.LogWarning("Inventory is full, cannot add more items.");
                return;
            }

            Items.Add(itemData);
            var go = CreateOrUpdateItemView(itemData);
            itemViews[itemData.Code] = go;

        }

        public void RemoveItem(InventoryItem itemData)
        {
            if (Items.Contains(itemData))
            {
                Items.Remove(itemData);
                if (itemData != null && !string.IsNullOrEmpty(itemData.Code) && itemViews.TryGetValue(itemData.Code, out var go))
                {
                    Destroy(go);
                    itemViews.Remove(itemData.Code);
                }
            }
            else
            {
                Debug.LogWarning("Item not found in inventory.");
            }
        }

        public void SelectItem(InventoryItem itemData)
        {
            if (itemData == null)
                return;
            CurrentItemData = itemData;
            QuestManager.I?.Inventory?.SelectItem(itemData.Code);
        }

        private GameObject CreateOrUpdateItemView(InventoryItem item)
        {
            if (item == null)
                return null;
            GameObject go;
            if (!itemViews.TryGetValue(item.Code, out go) || go == null)
            {
                go = Instantiate(ItemPrefab, ItemsContainer.transform);
                var btn = go.GetComponent<UnityEngine.UI.Button>();
                if (btn != null)
                {
                    btn.onClick.AddListener(() => SelectItem(item));
                }
            }
            var display = go.GetComponent<InventoryItemDisplay>();
            if (display != null)
            {
                display.SetItemData(item);
                if (item.Quantity == 1)
                {
                    display.PlayAppear();
                }
                // Apply selection state if this is the current item
                if (QuestManager.I != null && QuestManager.I.Inventory != null)
                {
                    var isSelected = QuestManager.I.Inventory.CurrentItem == item;
                    display.SetSelected(isSelected);
                }
            }
            return go;
        }

        private void HandleItemAdded(InventoryItem item)
        {
            Debug.Log($"InventoryDisplay: HandleItemAdded {item?.Code}");
            if (item == null)
                return;
            if (!Items.Contains(item))
                Items.Add(item);
            var go = CreateOrUpdateItemView(item);
            itemViews[item.Code] = go;
        }

        private void HandleItemRemoved(InventoryItem item)
        {
            if (item == null)
                return;
            Items.Remove(item);
            if (itemViews.TryGetValue(item.Code, out var go))
            {
                var display = go.GetComponent<InventoryItemDisplay>();
                if (display != null)
                {
                    display.PlayRemoveAndDestroy(() =>
                    {
                        Destroy(go);
                    });
                }
                else
                {
                    Destroy(go);
                }
                itemViews.Remove(item.Code);
            }
        }

        private void HandleItemUpdated(InventoryItem item)
        {

            if (item == null)
                return;
            // Updating will trigger quantity transition animations
            CreateOrUpdateItemView(item);
        }

        private void HandleSelectionChanged(InventoryItem item)
        {
            CurrentItemData = item;
            // Highlight selection in UI
            foreach (var kv in itemViews)
            {
                var disp = kv.Value != null ? kv.Value.GetComponent<InventoryItemDisplay>() : null;
                if (disp != null)
                    disp.SetSelected(item != null && disp.CurrentItemData == item);
            }
        }

        #region Test

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void Test_AddItem()
        {
            AddItem(new InventoryItem { Code = "test_item", Quantity = 1 });
        }

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void Test_RemoveItem()
        {
            RemoveItem(Items.Count > 0 ? Items[0] : null);
        }
        #endregion

    }
}
