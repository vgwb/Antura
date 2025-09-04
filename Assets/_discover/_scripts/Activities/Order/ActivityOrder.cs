using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

namespace Antura.Discover.Activities
{
    public class ActivityOrder : ActivityBase
    {
        [Header("Activity Order Settings")]
        public OrderSettingsData Settings;

        [Header("Override Settings")]
        public Difficulty ActivityDifficulty = Difficulty.Default;

        [Header("Scene Refs")]
        public Transform tilesPoolParent;
        public Transform slotsParent;
        public GameObject slotPrefab;         // has DropSlot (+ optional Image ref for highlight)
        public GameObject tilePrefab;         // has DraggableTile (+ CanvasGroup)
        [Header("Tutorial Ghosts")]
        public bool showGhostsInTutorial = true;

        private int minItemsToValidate;

        private CardData[] correctOrder;          // solution = original Items order
        private DraggableTile[] slots;        // current occupants (null if empty)
        private List<DropSlot> slotViews = new List<DropSlot>();

        public override void InitActivity()
        {
            ActivityDifficulty = Settings.Difficulty;
            var dataItems = BuildItemsFromSettings();
            minItemsToValidate = dataItems.Count;
            correctOrder = dataItems.ToArray();
            BuildRound();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void ConfigureSettings(ActivitySettingsAbstract settings)
        {
            base.ConfigureSettings(settings);
            if (settings is OrderSettingsData csd)
                Settings = csd;
        }
        protected override ActivitySettingsAbstract GetSettings() => Settings;

        protected override void OnRoundAdvanced(bool lastRoundSuccess, int lastRoundPoints, float lastRoundSeconds, bool dueToTimeout)
        {
            BuildRound();
        }

        private void BuildRound()
        {
            // Prepare slots and tiles for a round
            var dataItems = BuildItemsFromSettings();
            BuildSlots(dataItems.Count);
            ClearChildren(tilesPoolParent);
            SetValidateEnabled(false);

            // Spawn shuffled tiles into pool
            var shuffled = new List<CardData>(dataItems);
            Shuffle(shuffled);

            foreach (var it in shuffled)
            {
                var go = Instantiate(tilePrefab, tilesPoolParent);
                go.name = it.name;

                var tile = go.GetComponent<DraggableTile>();
                tile.Init(this, it, this.transform);
            }

            UpdateSlotHighlights();
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

                // Optional: ghost image in Tutorial difficulty
                if (showGhostsInTutorial && (ActivityDifficulty == Difficulty.Tutorial))
                {
                    var img = slotGO.GetComponentInChildren<Image>();
                    if (img != null && i < correctOrder.Length)
                    {
                        var sprite = ResolveSprite(correctOrder[i]);
                        if (sprite != null)
                        {
                            img.sprite = sprite;
                            var c = img.color;
                            c.a = 0.25f;
                            img.color = c;
                        }
                    }
                }
            }
        }

        private List<CardData> BuildItemsFromSettings()
        {
            var result = new List<CardData>();
            if (Settings.ItemsData != null && Settings.ItemsData.Count > 0)
            {
                foreach (var cd in Settings.ItemsData)
                {
                    if (cd == null)
                        continue;
                    result.Add(cd);
                }
            }
            else
            {
                Debug.LogError("Order: ItemsData is empty");
            }
            return result;
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

            // Pulse feedback
            Pulse(tile.transform, 1.06f, 0.08f);

            DiscoverAudioManager.I.Play(DiscoverSfx.ActivityDrop);


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

        private void Shuffle<T>(List<T> list)
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

        private void SetValidateInteractable(bool status) => SetValidateEnabled(status);

        // Live visual hints
        private void UpdateSlotHighlights()
        {
            if (slotViews.Count == 0)
                return;

            if (ActivityDifficulty == Difficulty.Easy || ActivityDifficulty == Difficulty.Tutorial)
            {
                for (int i = 0; i < slotViews.Count; i++)
                {
                    var view = slotViews[i];
                    if (view == null)
                        continue;

                    if (slots[i] != null && slots[i].CardData != null && slots[i].CardData.Id == correctOrder[i].Id)
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

                var expected = correctOrder[i].Id;
                if (tile.CardData == null || tile.CardData.Id != expected)
                    wrongIndices.Add(i);
            }

            if (wrongIndices.Count == 0)
            {
                Debug.Log("✅ Correct order!");
                DiscoverAudioManager.I.Play(DiscoverSfx.ActivitySuccess);

                return true;
            }
            else
            {
                StartCoroutine(ShakeWrongTiles(wrongIndices));
                DiscoverAudioManager.I.Play(DiscoverSfx.ActivityFail);

                return false;
            }
        }

        private void ClearChildren(Transform parent)
        {
            if (parent == null)
                return;
            for (int i = parent.childCount - 1; i >= 0; i--)
                Destroy(parent.GetChild(i).gameObject);
        }

        private IEnumerator ShakeWrongTiles(List<int> indices)
        {
            float duration = 0.25f;
            foreach (var idx in indices)
            {
                var rt = slots[idx].Rect;
                rt.DOKill();
                rt.DOPunchAnchorPos(new Vector2(18f, 0f), duration, vibrato: 12, elasticity: 0.6f).SetUpdate(true);
            }
            yield return new WaitForSecondsRealtime(duration);
        }

        // ---- Tutorial hint ----
        public void FlashCorrectSlot(CardData item, DraggableTile tile)
        {
            if (ActivityDifficulty != Difficulty.Tutorial)
                return;

            int correctIndex = -1;
            for (int i = 0; i < correctOrder.Length; i++)
            {
                if (correctOrder[i].Id == item.Id)
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
            DiscoverAudioManager.I.Play(clip);
        }

        private static Sprite ResolveSprite(CardData data)
        {
            if (data == null)
                return null;
            if (data.ImageAsset != null)
                return data.ImageAsset.Image;
            return null;
        }
    }
}
