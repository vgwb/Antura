using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Antura.Minigames.DiscoverCountry
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

        [Header("Activity Order")]
        [Tooltip("Items Textures to use in the activity")]
        public List<Item> Items;
        [Tooltip("Difficulty: the number of coins to use.")]
        public int DifficultyLevel = 1;

        [Header("UI Prefabs")]
        public GameObject ItemUIPrefab; // Prefab with Image+CanvasGroup+EventTrigger
        public GameObject SlotUIPrefab; // Prefab with Image (empty slot)
        public Button ValidateButton;

        private List<GameObject> itemUIs = new List<GameObject>();
        private List<GameObject> slotUIs = new List<GameObject>();
        private List<int> currentOrder = new List<int>();
        private List<int> correctOrder = new List<int>();

        private Canvas canvas;

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
            float slotY = Screen.height * 0.7f;
            float slotSpacing = Screen.width / (Items.Count + 1);
            slotUIs.Clear();
            for (int i = 0; i < Items.Count; i++)
            {
                var slot = Instantiate(SlotUIPrefab, canvas.transform);
                slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(slotSpacing * (i + 1) - Screen.width / 2, slotY - Screen.height / 2);
                slotUIs.Add(slot);
            }

            // Create draggable items (bottom of screen)
            float itemY = Screen.height * 0.2f;
            itemUIs.Clear();
            for (int i = 0; i < Items.Count; i++)
            {
                int itemIdx = currentOrder[i];
                var item = Instantiate(ItemUIPrefab, canvas.transform);
                var img = item.GetComponent<Image>();
                img.sprite = Items[itemIdx].Texture;
                item.GetComponentInChildren<Text>().text = Items[itemIdx].Name;
                item.GetComponent<RectTransform>().anchoredPosition = new Vector2(slotSpacing * (i + 1) - Screen.width / 2, itemY - Screen.height / 2);
                itemUIs.Add(item);

                // Add drag handlers
                var drag = item.AddComponent<DragHandler>();
                drag.Init(this, i);
            }

            // Validate button
            ValidateButton.onClick.AddListener(Validate);
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
            var tempPos = itemUIs[fromIdx].GetComponent<RectTransform>().anchoredPosition;
            itemUIs[fromIdx].GetComponent<RectTransform>().anchoredPosition = itemUIs[toIdx].GetComponent<RectTransform>().anchoredPosition;
            itemUIs[toIdx].GetComponent<RectTransform>().anchoredPosition = tempPos;

            // Swap UI list
            var tempUI = itemUIs[fromIdx];
            itemUIs[fromIdx] = itemUIs[toIdx];
            itemUIs[toIdx] = tempUI;

            // Update drag handler indices
            itemUIs[fromIdx].GetComponent<DragHandler>().Index = fromIdx;
            itemUIs[toIdx].GetComponent<DragHandler>().Index = toIdx;
        }

        void Validate()
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

        // --- Drag Handler Inner Class ---
        public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
        {
            private ActivityOrder activity;
            private RectTransform rectTransform;
            private CanvasGroup canvasGroup;
            private Vector2 originalPos;
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
                originalPos = rectTransform.anchoredPosition;
                canvasGroup.blocksRaycasts = false;
            }

            public void OnDrag(PointerEventData eventData)
            {
                rectTransform.anchoredPosition += eventData.delta / activity.canvas.scaleFactor;
            }

            public void OnEndDrag(PointerEventData eventData)
            {
                canvasGroup.blocksRaycasts = true;
                // Check if dropped on a slot
                int closestSlot = -1;
                float minDist = float.MaxValue;
                for (int i = 0; i < activity.slotUIs.Count; i++)
                {
                    float dist = Vector2.Distance(rectTransform.anchoredPosition, activity.slotUIs[i].GetComponent<RectTransform>().anchoredPosition);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closestSlot = i;
                    }
                }
                // Snap to closest slot if close enough
                if (minDist < 100f)
                {
                    activity.SwapItems(Index, closestSlot);
                }
                else
                {
                    rectTransform.anchoredPosition = originalPos;
                }
            }
        }
    }
}
