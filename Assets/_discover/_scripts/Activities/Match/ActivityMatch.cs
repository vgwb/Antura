using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Antura.Discover.Activities
{
    public class ActivityMatch : ActivityBase
    {
        [Header("Match Settings")]
        public MatchSettingsData Settings;

        [Header("Scene Refs")]
        public Transform leftSlotsParent;   // visual prompts
        public Transform rightTilesPool;    // draggable answers
        public GameObject leftSlotPrefab;   // simple placeholder with Image/Label (no drop)
        public GameObject rightTilePrefab;  // DraggableTile with Card visuals
        public GameObject dropSlotPrefab;   // DropSlot placed over each left slot
        public AudioSource audioSource;     // optional
        public AudioClip dropSound;         // optional

        private Dictionary<int, string> expectedBySlot = new(); // slotIndex -> Right.Id
        private DraggableTile[] placed; // placed right tiles by left slot
        private readonly List<DropSlot> leftDropSlots = new();

        public override void InitActivity()
        {
            BuildRound();
        }
        protected override void Update()
        {
            base.Update();
        }

        protected override ActivitySettingsAbstract GetSettings() => Settings;

        protected override void OnRoundAdvanced(bool lastRoundSuccess, int lastRoundPoints, float lastRoundSeconds, bool dueToTimeout)
        {
            BuildRound();
        }

        private void BuildRound()
        {
            ClearChildren(leftSlotsParent);
            ClearChildren(rightTilesPool);
            SetValidateEnabled(false);
            leftDropSlots.Clear();

            // Build simple pairs for the round by expanding GroupsData (supports 1:1 and 1:many)
            var pairs = new List<(Antura.Discover.CardData Left, Antura.Discover.CardData Right)>();
            if (Settings != null && Settings.GroupsData != null && Settings.GroupsData.Count > 0)
            {
                foreach (var g in Settings.GroupsData)
                {
                    if (g == null || g.Question == null || g.Answers == null)
                        continue;
                    var left = g.Question;
                    foreach (var answer in g.Answers)
                    {
                        if (answer == null)
                            continue;
                        pairs.Add((left, answer));
                    }
                }
            }
            if (pairs.Count == 0)
                return;

            placed = new DraggableTile[pairs.Count];
            expectedBySlot.Clear();

            // Build left prompts and drops
            for (int i = 0; i < pairs.Count; i++)
            {
                var pair = pairs[i];
                var slotGO = Instantiate(leftSlotPrefab, leftSlotsParent);
                var leftTitle = pair.Left.Title != null ? pair.Left.Title.GetLocalizedString() : pair.Left.name;
                slotGO.name = $"LeftSlot_{i}_{leftTitle}";

                // Optional: show prompt visuals from Left
                var img = slotGO.GetComponentInChildren<Image>();
                if (img)
                {
                    var sprite = ResolveSprite(pair.Left);
                    if (sprite != null)
                        img.sprite = sprite;
                }

                // Optional: show prompt label
                var label = slotGO.GetComponentInChildren<TextMeshProUGUI>();
                if (label)
                    label.text = leftTitle;

                // Add a DropSlot overlay to accept answers
                var dropGO = Instantiate(dropSlotPrefab, slotGO.transform);
                var drop = dropGO.GetComponent<DropSlot>();
                drop.activityManager = null; // we'll proxy
                drop.slotIndex = i;
                // Bind proxy so DropSlot can call place on this activity
                var proxy = dropGO.AddComponent<MatchDropProxy>();
                proxy.Init(this, i);

                leftDropSlots.Add(drop);

                expectedBySlot[i] = pair.Right.Id;
            }

            // Spawn right tiles shuffled
            var rightList = new List<Antura.Discover.CardData>();
            foreach (var p in pairs)
                rightList.Add(p.Right);
            Shuffle(rightList);

            foreach (var it in rightList)
            {
                var go = Instantiate(rightTilePrefab, rightTilesPool);
                go.name = it.name;
                var tile = go.GetComponent<DraggableTile>();
                tile.InitGeneric(
                    it,
                    this.transform,
                    poolGetter: () => rightTilesPool,
                    onLift: NotifyTileLiftedFromSlot,
                    onReturn: NotifyTileReturnedToPool,
                    onPlay: PlayItemSound,
                    onHint: null,
                    owner: this);
            }

            UpdateSlotHighlights();
        }

        public void PlaceTileAt(DraggableTile tile, int slotIndex)
        {
            var srcSlot = tile.OriginalParentSlotIndex;
            if (slotIndex < 0 || slotIndex >= placed.Length)
                return;

            // If target occupied, send previous to pool
            if (placed[slotIndex] != null)
            {
                placed[slotIndex].MoveToPool(rightTilesPool);
                placed[slotIndex] = null;
            }

            // Clear previous slot
            if (srcSlot >= 0 && srcSlot < placed.Length)
                placed[srcSlot] = null;

            // Place
            placed[slotIndex] = tile;
            tile.MoveToSlot(leftSlotsParent.GetChild(slotIndex), slotIndex);

            // SFX
            if (audioSource && dropSound)
                audioSource.PlayOneShot(dropSound);

            // Pulse
            Pulse(tile.transform, 1.06f, 0.08f);

            UpdateValidateState();
            UpdateSlotHighlights();
        }

        public void NotifyTileLiftedFromSlot(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < placed.Length && placed[slotIndex] != null)
                placed[slotIndex] = null;
            UpdateValidateState();
            UpdateSlotHighlights();
        }

        public void NotifyTileReturnedToPool()
        {
            UpdateValidateState();
            UpdateSlotHighlights();
        }

        private void UpdateValidateState()
        {
            int count = 0;
            for (int i = 0; i < placed.Length; i++)
                if (placed[i] != null)
                    count++;
            SetValidateEnabled(count == placed.Length);
        }

        private void UpdateSlotHighlights()
        {
            // Give gentle guidance in Tutorial/Easy
            var diff = Settings != null ? Settings.Difficulty : Difficulty.Default;
            if (diff != Difficulty.Tutorial && diff != Difficulty.Easy)
            {
                // Clear all
                for (int i = 0; i < leftDropSlots.Count; i++)
                    leftDropSlots[i]?.ClearHighlight();
                return;
            }

            for (int i = 0; i < leftDropSlots.Count; i++)
            {
                var drop = leftDropSlots[i];
                if (drop == null)
                    continue;
                var t = i < placed.Length ? placed[i] : null;
                if (t != null && expectedBySlot.TryGetValue(i, out var expected) && t.ItemData != null && t.ItemData.Id == expected)
                    drop.SetHighlight(Color.green, 0.35f);
                else
                    drop.ClearHighlight();
            }
        }

        public override bool DoValidate()
        {
            int correct = 0;
            for (int i = 0; i < placed.Length; i++)
            {
                var t = placed[i];
                if (t == null)
                    continue;
                if (t.ItemData != null && t.ItemData.Id == expectedBySlot[i])
                    correct++;
            }

            // partial credit through base GetRoundScore01()
            _lastScore = placed.Length > 0 ? (float)correct / placed.Length : 0f;
            return correct == placed.Length;
        }

        private float _lastScore = 0f;
        protected override float GetRoundScore01() => _lastScore;

        public void PlayItemSound(AudioClip clip)
        {
            if (!clip)
                return;
            if (audioSource)
                audioSource.PlayOneShot(clip);
            else
                AudioSource.PlayClipAtPoint(clip, Camera.main ? Camera.main.transform.position : Vector3.zero);
        }

        // Helpers
        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        private void ClearChildren(Transform parent)
        {
            if (!parent)
                return;
            for (int i = parent.childCount - 1; i >= 0; i--)
                Destroy(parent.GetChild(i).gameObject);
        }

        private static Sprite ResolveSprite(Antura.Discover.CardData data)
        {
            if (data == null)
                return null;
            if (data.ImageAsset != null)
                return data.ImageAsset.Image;
            return null;
        }
    }

    // Small proxy so we can reuse DropSlot prefab without coupling to ActivityOrder
    public class MatchDropProxy : MonoBehaviour, UnityEngine.EventSystems.IDropHandler
    {
        private ActivityMatch manager;
        private int slotIndex;
        public void Init(ActivityMatch mgr, int index) { manager = mgr; slotIndex = index; }
        public void OnDrop(UnityEngine.EventSystems.PointerEventData eventData)
        {
            var tile = eventData.pointerDrag ? eventData.pointerDrag.GetComponent<DraggableTile>() : null;
            if (tile == null)
                return;
            manager.PlaceTileAt(tile, slotIndex);
        }
    }
}
