using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Antura.Core;
using Antura.Utilities;
using System;

namespace Antura.Discover
{
    public class InventoryManager
    {
        static int MaxItemsInInventory = 5;

        // Public view of items (ordered list for UI)
        public List<InventoryItem> Items { get; private set; } = new List<InventoryItem>();
        // Fast lookup by code
        private readonly Dictionary<string, InventoryItem> itemsByCode = new Dictionary<string, InventoryItem>();

        // Selection
        public InventoryItem CurrentItem { get; private set; }

        // Events for UI wiring
        public event Action<InventoryItem> OnItemAdded;
        public event Action<InventoryItem> OnItemRemoved;
        public event Action<InventoryItem> OnItemUpdated; // quantity changed
        public event Action<InventoryItem> OnSelectionChanged;
        public event Action<int> OnCookiesChanged;

        private int total_items;
        private int cookies;

        public InventoryManager() { }

        public void Init(int maxItems)
        {
            total_items = maxItems;
            // Initialize cookies from current player profile, if available
            try
            {
                var playerProfile = DiscoverAppManager.I != null ? DiscoverAppManager.I.CurrentProfile : null;
                cookies = playerProfile != null ? Mathf.Max(0, playerProfile.wallet.cookies) : 0;
            }
            catch { cookies = 0; }
            OnCookiesChanged?.Invoke(cookies);
        }

        public void AddCookies(int n)
        {
            if (n == 0)
                return;
            cookies = Mathf.Max(0, cookies + n);
            OnCookiesChanged?.Invoke(cookies);
        }

        public int GetCookies() => cookies;

        public bool CollectItem(string itemCode, int quantity = 1)
        {
            Debug.Log($"InventoryManager() is Collecting item: {itemCode}");

            if (string.IsNullOrEmpty(itemCode))
                return false;

            if (itemsByCode.TryGetValue(itemCode, out var entry))
            {
                // we have already one, increase quantity
                Debug.Log($"Item {itemCode} already exists in inventory, increasing quantity.");
                // Respect stack rules if we can resolve a Card
                CardData card = entry.Card;
                if (card == null && DatabaseProvider.TryGet<CardData>(itemCode, out var c))
                    card = c;

                if (card != null)
                {
                    if (!card.IsStackable && entry.Quantity >= 1)
                    {
                        Debug.Log($"Item {itemCode} is not stackable and is already in inventory.");
                        // Still select it for convenience
                        SelectItem(itemCode);
                        return false;
                    }
                    if (card.IsStackable && card.MaxStack > 0 && entry.Quantity >= card.MaxStack)
                    {
                        Debug.Log($"Item {itemCode} reached max stack ({card.MaxStack}).");
                        SelectItem(itemCode);
                        return false;
                    }
                }

                entry.Quantity++;
                OnItemUpdated?.Invoke(entry);
                // Auto-select the newly updated item
                SelectItem(itemCode);
                Debug.Log($"Item {itemCode} quantity increased to {entry.Quantity}.");
                return true;
            }
            else
            {
                // Try to resolve Card and Item icon from DB for UI
                CardData card = null;
                ItemData icon = null;
                if (DatabaseProvider.TryGet<CardData>(itemCode, out var c))
                {
                    card = c;
                    icon = c.ItemIcon;
                }
                else if (DatabaseProvider.TryGet<ItemData>(itemCode, out var i))
                {
                    icon = i;
                }

                var newEntry = new InventoryItem { Code = itemCode, Card = card, Item = icon, Quantity = 1 };
                itemsByCode[itemCode] = newEntry;
                Items.Add(newEntry);

                OnItemAdded?.Invoke(newEntry);
                // Auto-select first time we collect
                SelectItem(itemCode);
                Debug.Log($"Item {itemCode} collected successfully.");
                return true;
            }


        }

        public bool RemoveItem(string itemCode)
        {
            if (string.IsNullOrEmpty(itemCode))
                return false;
            if (!itemsByCode.TryGetValue(itemCode, out var entry))
            {
                Debug.LogWarning($"Item {itemCode} not found in inventory.");
                return false;
            }

            entry.Quantity--;
            if (entry.Quantity <= 0)
            {
                Items.Remove(entry);
                itemsByCode.Remove(itemCode);
                OnItemRemoved?.Invoke(entry);
                if (CurrentItem == entry)
                {
                    // Clear selection or move to next available
                    CurrentItem = Items.Count > 0 ? Items[0] : null;
                    PushCurrentItemToYarn();
                    OnSelectionChanged?.Invoke(CurrentItem);
                }
                Debug.Log($"Item {itemCode} removed from inventory.");
            }
            else
            {
                OnItemUpdated?.Invoke(entry);
                Debug.Log($"Item {itemCode} quantity decreased to {entry.Quantity}.");
            }
            return true;
        }

        public bool HasItem(string itemCode, int atLeastQuantity = 1)
        {
            return itemsByCode.TryGetValue(itemCode, out var entry) && entry.Quantity >= atLeastQuantity;
        }

        public int GetItemCount(string itemCode)
        {
            return itemsByCode.TryGetValue(itemCode, out var entry) ? entry.Quantity : 0;
        }

        public bool CanCollect(string itemCode)
        {
            if (!itemsByCode.TryGetValue(itemCode, out var entry))
                return true;
            var card = entry.Card;
            if (card == null && DatabaseProvider.TryGet<CardData>(itemCode, out var c))
                card = c;
            if (card == null)
                return true; // no rule known
            if (!card.IsStackable && entry.Quantity >= 1)
                return false;
            if (card.IsStackable && card.MaxStack > 0 && entry.Quantity >= card.MaxStack)
                return false;
            return true;
        }

        public void SelectItem(string itemCode)
        {
            if (!itemsByCode.TryGetValue(itemCode, out var entry))
                return;
            CurrentItem = entry;
            PushCurrentItemToYarn();
            OnSelectionChanged?.Invoke(CurrentItem);
        }

        private void PushCurrentItemToYarn()
        {
            var storage = YarnAnturaManager.I?.Runner?.VariableStorage;
            if (storage == null)
                return;
            storage.SetValue("$CURRENT_ITEM", CurrentItem != null ? CurrentItem.Code : "");
        }
    }
}
