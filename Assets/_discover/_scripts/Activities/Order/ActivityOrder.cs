using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Antura.Discover
{
    public class ActivityOrder : ActivityBase
    {
        [Serializable]
        public struct Item
        {
            public string Name;
            public Sprite Texture;
            public AudioClip AudioClip;
        }

        [Header("Activity Order Settings")]
        [Tooltip("Items Textures to use in the activity")]
        public List<Item> Items;

        [Header("UI References")]
        public RectTransform ItemContainer; // Parent for draggable items
        public RectTransform SlotContainer; // Parent for slots

        [Header("UI Prefabs")]
        public GameObject ItemUIPrefab; // Prefab with Image+CanvasGroup+EventTrigger
        public GameObject SlotUIPrefab; // Prefab with Image (empty slot)
        public Button ValidateButton;

        private List<GameObject> itemUIs = new List<GameObject>();
        private List<GameObject> slotUIs = new List<GameObject>();
        private List<int> currentOrder = new List<int>();
        private List<int> correctOrder = new List<int>();

        private Canvas canvas;

        // --- Add these variables ---
        [Header("Layout Settings")]
        [Tooltip("Horizontal spacing between slots/items")]
        public float slotSpacing = 200f;
        [Tooltip("Y position for slots (top of screen)")]
        public float slotY = 300f;
        [Tooltip("Y position for items (bottom of screen)")]
        public float itemY = -300f;

        void Start()
        {
            canvas = FindObjectOfType<Canvas>();
            if (!canvas)
            { Debug.LogError("No Canvas found in scene!"); return; }
            if (Items.Count < 2)
            { Debug.LogError("Add at least 2 items!"); return; }
            if (!ItemUIPrefab || !SlotUIPrefab || !ValidateButton)
            { Debug.LogError("Assign all UI Prefabs and Button!"); return; }

            // Set correct order (0,1,2,...)
            correctOrder.Clear();
            for (int i = 0; i < Items.Count; i++)
                correctOrder.Add(i);

            // Shuffle for initial order
            currentOrder = new List<int>(correctOrder);
            Shuffle(currentOrder);

            // Create slots (top of screen)
            slotUIs.Clear();
            float slotContainerWidth = SlotContainer.rect.width;
            float slotStartX = -slotContainerWidth / 2 + slotSpacing / 2;
            for (int i = 0; i < Items.Count; i++)
            {
                var slot = Instantiate(SlotUIPrefab, SlotContainer);
                var slotRect = slot.GetComponent<RectTransform>();
                slotRect.anchoredPosition = GetSlotPosition(i, Items.Count);
                slotUIs.Add(slot);
            }

            // Create draggable items (bottom of screen)
            itemUIs.Clear();
            System.Random rng = new System.Random();
            for (int i = 0; i < Items.Count; i++)
            {
                int itemIdx = currentOrder[i];
                var item = Instantiate(ItemUIPrefab, ItemContainer);
                var img = item.GetComponent<Image>();
                img.sprite = Items[itemIdx].Texture;
                item.GetComponentInChildren<TextMeshProUGUI>().text = Items[itemIdx].Name;
                var itemRect = item.GetComponent<RectTransform>();

                float containerWidth = ItemContainer.rect.width;
                float containerHeight = ItemContainer.rect.height;
                float x = UnityEngine.Random.Range(-containerWidth / 2 + 50, containerWidth / 2 - 50);
                float y = UnityEngine.Random.Range(-containerHeight / 2 + 50, containerHeight / 2 - 50);
                itemRect.anchoredPosition = new Vector2(x, y);

                itemUIs.Add(item);

                // Add drag handlers
                var drag = item.AddComponent<DragHandler>();
                drag.Init(this, i);
            }

            // Validate button
            ValidateButton.onClick.AddListener(Validate);

            UpdateValidateButtonState();
        }

        void Shuffle(List<int> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        // Called by DragHandler when an item is dropped on a slot
        public void SwapItems(int fromIdx, int toIdx)
        {
            if (fromIdx == toIdx)
                return;

            // Swap in currentOrder
            int tmp = currentOrder[fromIdx];
            currentOrder[fromIdx] = currentOrder[toIdx];
            currentOrder[toIdx] = tmp;

            // Swap UI positions
            itemUIs[fromIdx].GetComponent<RectTransform>().anchoredPosition = GetSlotPosition(fromIdx, Items.Count);
            itemUIs[toIdx].GetComponent<RectTransform>().anchoredPosition = GetSlotPosition(toIdx, Items.Count);

            // Swap UI list
            var tempUI = itemUIs[fromIdx];
            itemUIs[fromIdx] = itemUIs[toIdx];
            itemUIs[toIdx] = tempUI;

            // Update drag handler indices
            itemUIs[fromIdx].GetComponent<DragHandler>().Index = fromIdx;
            itemUIs[toIdx].GetComponent<DragHandler>().Index = toIdx;

            UpdateValidateButtonState();
        }

        new void Validate()
        {
            bool correct = true;
            for (int i = 0; i < currentOrder.Count; i++)
            {
                if (currentOrder[i] != correctOrder[i])
                {
                    correct = false;
                    break;
                }
            }
            if (correct)
            {
                Debug.Log("Correct order!");
                // You can add win feedback here
            }
            else
            {
                Debug.Log("Wrong order, try again!");
                // You can add fail feedback here
            }
        }

        private Vector2 GetSlotPosition(int slotIndex, int totalSlots)
        {
            float containerWidth = SlotContainer.rect.width;
            float spacing = containerWidth / (totalSlots + 1);
            float x = -containerWidth / 2 + spacing * (slotIndex + 1);
            float y = slotY;
            return new Vector2(x, y);
        }

        private void UpdateValidateButtonState()
        {
            int itemsInSlots = 0;
            foreach (var item in itemUIs)
            {
                if (item.transform.parent == SlotContainer)
                    itemsInSlots++;
            }
            ValidateButton.interactable = (itemsInSlots == Items.Count);
        }

        // --- Drag Handler Inner Class ---
        public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
        {
            private ActivityOrder activity;
            private RectTransform rectTransform;
            private CanvasGroup canvasGroup;
            private Vector2 originalPos;
            private Transform originalParent;
            public int Index;

            public void Init(ActivityOrder activity, int index)
            {
                this.activity = activity;
                this.Index = index;
                rectTransform = GetComponent<RectTransform>();
                canvasGroup = GetComponent<CanvasGroup>();
            }

            public void OnBeginDrag(PointerEventData eventData)
            {
                originalParent = rectTransform.parent;
                originalPos = rectTransform.anchoredPosition;
                rectTransform.SetParent(activity.canvas.transform, true); // Move to canvas
                rectTransform.SetAsLastSibling(); // Ensure it's drawn above other UI
                canvasGroup.blocksRaycasts = false;
                rectTransform.position = eventData.position;
            }

            public void OnDrag(PointerEventData eventData)
            {
                rectTransform.position = eventData.position;
            }

            public void OnEndDrag(PointerEventData eventData)
            {
                canvasGroup.blocksRaycasts = true;
                // Find closest slot
                int closestSlot = -1;
                float minDist = float.MaxValue;
                for (int i = 0; i < activity.slotUIs.Count; i++)
                {
                    var slotRect = activity.slotUIs[i].GetComponent<RectTransform>();
                    Vector2 slotWorldPos = slotRect.TransformPoint(slotRect.rect.center);
                    float dist = Vector2.Distance(rectTransform.position, slotWorldPos);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closestSlot = i;
                    }
                }
                // Snap to closest slot if close enough
                if (minDist < 100f)
                {
                    rectTransform.SetParent(activity.SlotContainer, true);
                    rectTransform.anchoredPosition = activity.GetSlotPosition(closestSlot, activity.Items.Count);
                    activity.SwapItems(Index, closestSlot);
                }
                else
                {
                    rectTransform.SetParent(activity.ItemContainer, true);
                    rectTransform.anchoredPosition = originalPos;
                }
                activity.UpdateValidateButtonState(); // <-- Add this line
            }
        }
    }
}
