using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Antura.Core;
using Antura.Utilities;

namespace Antura.Minigames.DiscoverCountry
{
    public class Inventory
    {
        private HashSet<string> collectedItems;

        public Inventory()
        {
            collectedItems = new HashSet<string>();
        }

        public void Init(int maxItems)
        {

        }

        public bool CollectItem(string itemCode)
        {
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
    }
}
