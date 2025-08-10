using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Antura.Discover.Activities
{
    public class ActivityOrder : ActivityBase
    {
        [Header("Data")]
        [Tooltip("Between 2 and 10 items. The order here is the correct target order.")]
        public List<CardItem> Items;              // 2..10
        public Difficulty difficulty = Difficulty.Normal;

        [Header("Scene Refs")]
        public Transform tilesPoolParent;
        public Transform slotsParent;
        public GameObject slotPrefab;         // has DropSlot (+ optional Image ref for highlight)
        public GameObject tilePrefab;         // has DraggableTile (+ CanvasGroup)
        public Button validateButton;

        [Header("Sfx")]
        public AudioSource audioSource;
        public AudioClip dropSound;

        private int minItemsToValidate;

        private CardItem[] correctOrder;          // solution = original Items order
        private DraggableTile[] slots;        // current occupants (null if empty)
        private List<DropSlot> slotViews = new List<DropSlot>();

        private void Awake()
        {
            if (Items == null)
                Items = new List<CardItem>();
            if (Items.Count < 2)
                Debug.LogWarning("Puzzle needs at least 2 items.");
            if (Items.Count > 10)
                Debug.LogWarning("Puzzle supports max 10 items.");
        }

        public override void Init()
        {
            BuildSlots(Items.Count);
            SetValidateInteractable(false);

            minItemsToValidate = Items.Count;

            // Store solution order
            correctOrder = Items.ToArray();

            // Spawn shuffled tiles into pool
            var shuffled = new List<CardItem>(Items);
            Shuffle(shuffled);

            foreach (var it in shuffled)
            {
                var go = Instantiate(tilePrefab, tilesPoolParent);
                go.name = it.Name;

                var tile = go.GetComponent<DraggableTile>();
                tile.Init(this, it, this.transform);
            }

            UpdateSlotHighlights(); // clear/prepare highlights
        }

        private void BuildSlots(int count)
        {
            // Clear existing
            for (int i = slotsParent.childCount - 1; i >= 0; i--)
                Destroy(slotsParent.GetChild(i).gameObject);

            slotViews.Clear();
            slots = new DraggableTile[count];

            for (int i = 0; i < count; i++)
            {
                var slotGO = Instantiate(slotPrefab, slotsParent);
                var drop = slotGO.GetComponent<DropSlot>();
                drop.activityManager = this;
                drop.slotIndex = i;
                slotViews.Add(drop);
            }
        }

        public void PlaceTile(DraggableTile tile, int slotIndex)
        {
            var targetOccupied = slots[slotIndex] != null;
            var srcSlot = tile.OriginalParentSlotIndex;

            // If target occupied, handle swap or push to pool
            if (targetOccupied)
            {
                var other = slots[slotIndex];
                if (srcSlot >= 0)
                {
                    // swap between two slots: move 'other' to tile's source slot
                    slots[srcSlot] = other;
                    other.transform.SetParent(other.transform.root);
                    other.SnapTo(GetSlotTransform(srcSlot));
                    other.OriginalParentSlotIndex = srcSlot;
                }
                else
                {
                    // from pool -> send existing to pool
                    other.MoveToPool(tilesPoolParent);
                    slots[slotIndex] = null;
                }
            }

            // Clear previous source slot only if target wasn't occupied (i.e., we didn't swap into it)
            if (srcSlot >= 0 && !targetOccupied)
            {
                slots[srcSlot] = null;
            }

            // Place into new slot
            slots[slotIndex] = tile;
            tile.MoveToSlot(GetSlotTransform(slotIndex), slotIndex);

            if (audioSource && dropSound)
                audioSource.PlayOneShot(dropSound);

            UpdateValidateState();
            UpdateSlotHighlights();
        }

        public void NotifyTileLiftedFromSlot(int slotIndex)
        {
            if (slotIndex < 0 || slots == null || slotIndex >= slots.Length)
                return;
            if (slots[slotIndex] != null)
            {
                slots[slotIndex] = null;
            }
            UpdateValidateState();
            UpdateSlotHighlights();
        }

        public void NotifyTileReturnedToPool()
        {
            UpdateValidateState();
            UpdateSlotHighlights();
        }

        private Transform GetSlotTransform(int slotIndex) => slotsParent.GetChild(slotIndex);

        private void Shuffle(List<CardItem> list)
        {
            // Fisher–Yates shuffle
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1); // int max is exclusive, so i+1 yields [0..i]
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        private void UpdateValidateState()
        {
            int itemsInSlots = CountItemsInSlots();
            SetValidateInteractable(itemsInSlots >= minItemsToValidate);
        }

        private int CountItemsInSlots()
        {
            if (slots == null || slots.Length == 0)
                return 0;
            int count = 0;
            for (int i = 0; i < slots.Length; i++)
                if (slots[i] != null)
                    count++;
            return count;
        }

        private bool AllSlotsFilled()
        {
            if (slots == null || slots.Length == 0)
                return false;
            for (int i = 0; i < slots.Length; i++)
                if (slots[i] == null)
                    return false;
            return true;
        }

        private void SetValidateInteractable(bool status)
        {
            if (validateButton)
                validateButton.interactable = status;
        }

        // Live visual hints
        private void UpdateSlotHighlights()
        {
            if (slotViews.Count == 0)
                return;

            if (difficulty == Difficulty.Easy || difficulty == Difficulty.Tutorial)
            {
                for (int i = 0; i < slotViews.Count; i++)
                {
                    var view = slotViews[i];
                    if (view == null)
                        continue;

                    if (slots[i] != null && slots[i].ItemData.Name == correctOrder[i].Name)
                        view.SetHighlight(Color.green, 0.35f);
                    else
                        view.ClearHighlight();
                }
            }
            else
            {
                for (int i = 0; i < slotViews.Count; i++)
                    slotViews[i]?.ClearHighlight();
            }
        }

        // Called by Validate button
        public override bool DoValidate()
        {
            if (!AllSlotsFilled())
            {
                Debug.Log("Not all slots filled.");
                return false;
            }

            List<int> wrongIndices = new List<int>();

            for (int i = 0; i < slots.Length; i++)
            {
                var tile = slots[i];
                if (tile == null)
                { wrongIndices.Add(i); continue; }

                var expected = correctOrder[i].Name;
                if (tile.ItemData.Name != expected)
                    wrongIndices.Add(i);
            }

            if (wrongIndices.Count == 0)
            {
                Debug.Log("✅ Correct order!");
                // TODO: success animation / next level
                return true;
            }
            else
            {
                StartCoroutine(ShakeWrongTiles(wrongIndices));
                Debug.Log($"❌ Wrong order in {wrongIndices.Count} slot(s).");
                return false;
            }
        }

        private IEnumerator ShakeWrongTiles(List<int> indices)
        {
            float duration = 0.25f;
            float strength = 10f;

            var originals = new Dictionary<RectTransform, Vector2>();
            foreach (var idx in indices)
            {
                var rt = slots[idx].Rect;
                originals[rt] = rt.anchoredPosition;
            }

            float t = 0f;
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                float phase = (t / duration) * Mathf.PI * 4f;
                float offset = Mathf.Sin(phase) * strength;

                foreach (var kv in originals)
                {
                    var rt = kv.Key;
                    rt.anchoredPosition = kv.Value + new Vector2(offset, 0f);
                }
                yield return null;
            }

            foreach (var kv in originals)
                kv.Key.anchoredPosition = kv.Value;
        }

        // ---- Tutorial hint ----
        public void FlashCorrectSlot(CardItem item, DraggableTile tile)
        {
            if (difficulty != Difficulty.Tutorial)
                return;

            int correctIndex = -1;
            for (int i = 0; i < correctOrder.Length; i++)
            {
                if (correctOrder[i].Name == item.Name)
                {
                    correctIndex = i;
                    break;
                }
            }

            if (correctIndex >= 0 && correctIndex < slotViews.Count)
                StartCoroutine(FlashSlotAndBounce(slotViews[correctIndex], tile));
        }

        private IEnumerator FlashSlotAndBounce(DropSlot slotView, DraggableTile tile)
        {
            if (slotView == null || tile == null)
                yield break;

            slotView.SetHighlight(Color.yellow, 0.5f);

            // world-space bounce there and back
            Vector3 startPos = tile.transform.position;
            Transform startParent = tile.transform.parent;
            Vector3 targetPos = slotView.transform.position;

            float duration = 0.4f;
            float t = 0f;

            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                float p = t / duration;
                float height = Mathf.Sin(p * Mathf.PI) * 20f;
                Vector3 pos = Vector3.Lerp(startPos, targetPos, p) + Vector3.up * height;
                tile.transform.position = pos;
                yield return null;
            }

            // snap back
            tile.transform.SetParent(startParent);
            tile.transform.position = startPos;

            yield return new WaitForSecondsRealtime(0.5f);
            UpdateSlotHighlights();
        }

        public void PlayItemSound(AudioClip clip)
        {
            if (clip == null)
                return;
            if (audioSource != null)
                audioSource.PlayOneShot(clip);
            else
                AudioSource.PlayClipAtPoint(clip, Camera.main ? Camera.main.transform.position : Vector3.zero);
        }
    }
}
