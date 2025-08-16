using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Antura.Discover
{
    public class InventoryItem
    {
        // Unique code used to identify the item (usually the CardData.Id)
        public string Code;
        // Reference to the source Card (optional but useful for UI)
        public CardData Card;
        public ItemData Item;
        public int Quantity;

    }
}
