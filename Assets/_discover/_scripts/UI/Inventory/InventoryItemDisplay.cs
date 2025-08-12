using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Antura.Discover
{
    public class InventoryItemDisplay : MonoBehaviour
    {
        public GameObject CounterGO;
        public TMP_Text CounterText;
        public InventoryItem CurrentItemData;

        void Start()
        {
        }
        public void SetItemData(InventoryItem itemData)
        {
            CurrentItemData = itemData;
            if (itemData.Quantity > 1)
            {
                CounterGO.SetActive(true);
                CounterText.text = itemData.Quantity.ToString();
            }
            else
            {
                CounterGO.SetActive(false);
            }
        }

        public void OnCLick()
        {
            if (CurrentItemData == null)
            {
                Debug.LogWarning("CurrentItemData is null, cannot display item.");
                return;
            }

            // Display the item details in the UI
            // Debug.Log($"Displaying item: {CurrentItemData.ItemName}");
            // // Here you would typically update the UI elements to show the item's details
            // CounterGO.SetActive(true);
            // CounterText.text = CurrentItemData.ItemCount.ToString();
        }

    }
}
