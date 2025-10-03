using Antura.Discover.Audio;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Antura.Discover.Activities
{
    public class ActivityMatch : ActivityBase
    {
        [Header("Match Settings")]
        public MatchSettingsData Settings;

        [Header("References")]
        public Transform BoardArea;
        public GameObject questionItemPrefab;
        public GameObject answerItemPrefab;

        [Header("Drop Areas")]
        [Tooltip("If true, the script will create a full-board drop area behind all items. If false, you can place your own drop areas manually (MatchPoolDropArea or MatchDropSlot with IsPoolArea).")]
        public bool AutoCreateBoardDropArea = false;

        private DraggableTile[] placed;
        private readonly List<QuestionItem> questionItems = new();
        private readonly Dictionary<DraggableTile, Vector2> poolAnchoredPos = new();
        private readonly Dictionary<DraggableTile, int> poolSiblingIndex = new();

        public override void InitActivity()
        {
            BuildRound();
        }
        protected override void Update()
        {
            base.Update();
        }

        public override void ConfigureSettings(ActivitySettingsAbstract settings)
        {
            base.ConfigureSettings(settings);
            if (settings is MatchSettingsData csd)
                Settings = csd;
        }

        protected override ActivitySettingsAbstract GetSettings() => Settings;

        protected override void OnRoundAdvanced(bool lastRoundSuccess, int lastRoundPoints, float lastRoundSeconds, bool dueToTimeout)
        {
            BuildRound();
        }

        private void BuildRound()
        {
            ClearChildren(BoardArea);
            SetValidateEnabled(false);
            // DropSlots are re-created by QuestionItem
            questionItems.Clear();
            poolAnchoredPos.Clear();
            poolSiblingIndex.Clear();

            // Build simple pairs for the round by expanding GroupsData
            var pairs = new List<(CardData Question, CardData Answer)>();

            if (Settings != null && Settings.GroupsData != null && Settings.GroupsData.Count > 0)
            {
                foreach (var g in Settings.GroupsData)
                {
                    if (g == null || g.Question == null || g.Answers == null)
                        continue;
                    var question = g.Question;
                    // 1:1 mode: use only the first non-null answer, warn if more
                    var firstAnswer = g.Answers.Find(a => a != null);
                    if (firstAnswer != null)
                    {
                        pairs.Add((question, firstAnswer));
                        if (g.Answers.Count > 1)
                            Debug.LogWarning($"[ActivityMatch] 1:1 mode: Group with Question '{question?.Id}' has {g.Answers.Count} answers; using only the first.");
                    }
                }
            }
            if (pairs.Count == 0)
                return;

            placed = new DraggableTile[pairs.Count];


            // Build question prompts and drops
            for (int i = 0; i < pairs.Count; i++)
            {
                var pair = pairs[i];
                var slotGO = Instantiate(questionItemPrefab, BoardArea);
                var slotRT = slotGO.transform as RectTransform;
                if (slotRT != null)
                {
                    slotRT.anchorMin = new Vector2(0.5f, 0.5f);
                    slotRT.anchorMax = new Vector2(0.5f, 0.5f);
                    slotRT.pivot = new Vector2(0.5f, 0.5f);
                }
                // Prefer localized title, fallback to TitleEn or asset name
                string questionTitle = null;
                try
                { questionTitle = pair.Question.Title != null ? pair.Question.Title.GetLocalizedString() : null; }
                catch { }
                if (string.IsNullOrWhiteSpace(questionTitle))
                    questionTitle = !string.IsNullOrEmpty(pair.Question.TitleEn) ? pair.Question.TitleEn : pair.Question.name;
                slotGO.name = $"QuestionSlot_{i}_{questionTitle}";

                // Initialize via QuestionItem to encapsulate visuals and overlay
                var qItem = slotGO.GetComponent<QuestionItem>();
                if (qItem == null)
                    qItem = slotGO.AddComponent<QuestionItem>();
                var sprite = ResolveSprite(pair.Question);
                qItem.Init(questionTitle, sprite, i, this, pair.Question, pair.Answer?.Id);

                // Track structures: use the DropSlot created by QuestionItem, or ensure one exists
                var drop = qItem.DropSlot != null ? qItem.DropSlot : slotGO.GetComponentInChildren<MatchDropSlot>(includeInactive: true);
                if (drop == null)
                {
                    var dropGO = new GameObject("DropOverlay", typeof(RectTransform));
                    dropGO.transform.SetParent(slotGO.transform, false);
                    drop = dropGO.AddComponent<MatchDropSlot>();
                }
                drop.manager = this;
                drop.slotIndex = i;
                drop.Owner = qItem;
                // tracked through questionItems
                questionItems.Add(qItem);
                qItem.ExpectedAnswerId = pair.Answer.Id;
            }
            // Manually distribute question slots horizontally on the upper part
            var rootRT = BoardArea as RectTransform;
            if (rootRT != null)
            {
                float w = rootRT.rect.width;
                float h = rootRT.rect.height;
                int n = questionItems.Count;
                for (int i = 0; i < n; i++)
                {
                    var qi = questionItems[i];
                    var rt = qi ? qi.transform as RectTransform : null;
                    if (rt != null)
                    {
                        float t = (n == 1) ? 0.5f : (float)i / (n - 1);
                        float halfCW = rt.rect.width * 0.5f;
                        float halfCH = rt.rect.height * 0.5f;
                        float padX = Mathf.Max(halfCW, 24f); // keep fully inside + a tiny margin
                        float padYTop = Mathf.Max(halfCH, 24f);
                        float x = Mathf.Lerp(-w * 0.5f + padX, w * 0.5f - padX, t);
                        float y = h * 0.25f; // upper band center
                        rt.anchoredPosition = new Vector2(x, y);
                    }
                }
            }

            // Spawn answers tiles shuffled
            var answerList = new List<CardData>();
            foreach (var p in pairs)
                answerList.Add(p.Answer);
            Shuffle(answerList);
            // Build answers under the same board container
            var spawnedTiles = new List<DraggableTile>();
            foreach (var it in answerList)
            {
                var go = Instantiate(answerItemPrefab, BoardArea);
                go.name = it.name;
                var tile = go.GetComponent<DraggableTile>();
                tile.InitGeneric(
                    it,
                    this.transform,
                    poolGetter: () => BoardArea,
                    onLift: NotifyTileLiftedFromSlot,
                    onReturn: NotifyTileReturnedToPool,
                    onHint: null,
                    owner: this);
                // Ensure CanvasGroup exists for proper drag raycast toggling
                if (go.GetComponent<CanvasGroup>() == null)
                    go.AddComponent<CanvasGroup>();
                // Ensure AnswerItem tracker exists
                var answerItem = go.GetComponent<AnswerItem>();
                if (answerItem == null)
                    answerItem = go.AddComponent<AnswerItem>();
                answerItem.AttachedTo = null;
                answerItem.Data = it;
                // Clamp tile during drag within the board area
                var clamp = go.GetComponent<AnswerTileClamp>();
                if (clamp == null)
                    clamp = go.AddComponent<AnswerTileClamp>();
                clamp.manager = this;
                if (go.transform is RectTransform art)
                {
                    art.anchorMin = new Vector2(0.5f, 0.5f);
                    art.anchorMax = new Vector2(0.5f, 0.5f);
                    art.pivot = new Vector2(0.5f, 0.5f);
                }
                spawnedTiles.Add(tile);
            }

            // Distribute answers on the bottom part and memorize their home positions
            if (rootRT != null)
            {
                float w = rootRT.rect.width;
                float h = rootRT.rect.height;
                int nA = spawnedTiles.Count;
                for (int i = 0; i < nA; i++)
                {
                    var tile = spawnedTiles[i];
                    if (tile.transform is RectTransform rt)
                    {
                        float t = (nA == 1) ? 0.5f : (float)i / (nA - 1);
                        float x = Mathf.Lerp(-w * 0.45f, w * 0.45f, t);
                        float y = -h * 0.25f; // lower band
                        var pos = new Vector2(x, y);
                        rt.anchoredPosition = pos;
                        poolAnchoredPos[tile] = pos;
                        poolSiblingIndex[tile] = rt.GetSiblingIndex();
                    }
                }
            }

            // Add a board-wide drop area so non-question drops are captured without snapping
            EnsureBoardDropArea();
            UpdateSlotHighlights();
        }

        public void PlaceTileAt(DraggableTile tile, int slotIndex)
        {
            var srcSlot = tile.OriginalParentSlotIndex;
            if (slotIndex < 0 || slotIndex >= placed.Length)
                return;

            // If dropping same tile on same question, keep it attached and just refresh its position
            if (placed[slotIndex] == tile)
            {
                ParentAndPositionTileUnderQuestion(tile, slotIndex);
                UpdateValidateState();
                UpdateSlotHighlights();
                return;
            }

            // If target occupied, return previous to pool with a small animation
            if (placed[slotIndex] != null)
            {
                ReturnTileToPoolAnimated(placed[slotIndex]);
                placed[slotIndex] = null;
            }

            // Clear previous slot
            if (srcSlot >= 0 && srcSlot < placed.Length)
                placed[srcSlot] = null;

            // Place
            placed[slotIndex] = tile;
            // Parent inside the question slot and offset below its content (label safe)
            ParentAndPositionTileUnderQuestion(tile, slotIndex);
            // Highlight will be handled in UpdateSlotHighlights based on difficulty

            DiscoverAudioManager.I.PlaySfx(DiscoverSfx.ActivityDrop);

            // Pulse
            Pulse(tile.transform, 1.06f, 0.08f);

            UpdateValidateState();
            UpdateSlotHighlights();
        }

        private void ParentAndPositionTileUnderQuestion(DraggableTile tile, int slotIndex)
        {
            var parentForAnswer = questionItems[slotIndex] ? questionItems[slotIndex].transform : BoardArea.GetChild(slotIndex);
            tile.MoveToSlot(parentForAnswer, slotIndex);
            if (tile.transform is RectTransform rt)
            {
                var qrt = questionItems[slotIndex] ? questionItems[slotIndex].transform as RectTransform : null;
                float h = qrt != null ? qrt.rect.height : 100f;
                float spacing = 12f + 60f; // extra to avoid label overlap
                rt.anchorMin = new Vector2(0.5f, 1f);
                rt.anchorMax = new Vector2(0.5f, 1f);
                rt.pivot = new Vector2(0.5f, 1f);
                rt.anchoredPosition = new Vector2(0f, -(h + spacing));
                rt.SetAsLastSibling();
            }
            // Track attachment on the tile
            var ai = tile.GetComponent<AnswerItem>();
            if (ai != null)
                ai.AttachedTo = parentForAnswer;
        }

        public void NotifyTileLiftedFromSlot(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < placed.Length && placed[slotIndex] != null)
                placed[slotIndex] = null;
            if (slotIndex >= 0 && slotIndex < questionItems.Count)
                questionItems[slotIndex]?.SetHighlight(null);
            UpdateValidateState();
            UpdateSlotHighlights();
        }

        public void NotifyTileReturnedToPool()
        {
            UpdateValidateState();
            UpdateSlotHighlights();
        }

        public void OnTileDroppedInPool(DraggableTile tile, UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (tile == null)
                return;
            // Detach from any question slot state
            for (int i = 0; i < placed.Length; i++)
            {
                if (placed[i] == tile)
                {
                    // Add a board-wide drop area only if requested; otherwise expect manual setup
                    if (AutoCreateBoardDropArea)
                        EnsureBoardDropArea();

                    // Wire any manually placed pool drop areas under the board
                    WireExistingDropAreas();
                    placed[i] = null;
                    if (i >= 0 && i < questionItems.Count)
                        questionItems[i]?.SetHighlight(null);
                }
            }

            // Reparent to board and place at drop position (anchored)
            if (tile.transform is RectTransform rt && BoardArea is RectTransform poolRT)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(poolRT, eventData.position, eventData.pressEventCamera, out var local);
                rt.SetParent(BoardArea, worldPositionStays: false);
                SetCenterAnchors(rt);
                rt.anchoredPosition = ClampAnchoredToParent(poolRT, rt, local);
                rt.SetAsLastSibling();
                poolAnchoredPos[tile] = rt.anchoredPosition;
                poolSiblingIndex[tile] = rt.GetSiblingIndex();
            }
            else
            {
                tile.MoveToPool(BoardArea);
            }

            // Clear tile attachment tracking
            var ai = tile.GetComponent<AnswerItem>();
            if (ai != null)
                ai.AttachedTo = null;

            DiscoverAudioManager.I?.PlaySfx(DiscoverSfx.ActivityDrop);
            UpdateValidateState();
            UpdateSlotHighlights();
        }

        private void EnsureBoardDropArea()
        {
            if (BoardArea is not RectTransform rootRT)
                return;
            const string areaName = "BoardDropArea";
            var t = BoardArea.Find(areaName);
            RectTransform area;
            if (t == null)
            {
                var go = new GameObject(areaName, typeof(RectTransform));
                area = go.GetComponent<RectTransform>();
                area.SetParent(BoardArea, false);
            }
            else
            {
                area = t as RectTransform;
            }
            area.anchorMin = new Vector2(0f, 0f);
            area.anchorMax = new Vector2(1f, 1f);
            area.offsetMin = Vector2.zero;
            area.offsetMax = Vector2.zero;
            area.pivot = new Vector2(0.5f, 0.5f);

            var drop = area.GetComponent<MatchDropSlot>();
            if (drop == null)
                drop = area.gameObject.AddComponent<MatchDropSlot>();
            var g = area.GetComponent<Image>();
            if (g == null)
                g = area.gameObject.AddComponent<Image>();
            g.color = new Color(1, 1, 1, 0f);
            g.raycastTarget = true;
            drop.manager = this;
            drop.IsPoolArea = true;
            // Make sure this area stays behind other children to not block drag start
            area.SetAsFirstSibling();
        }
        private void WireExistingDropAreas()
        {
            if (!BoardArea)
                return;
            // Wire MatchPoolDropArea
            var poolAreas = BoardArea.GetComponentsInChildren<MatchPoolDropArea>(includeInactive: true);
            foreach (var pa in poolAreas)
            {
                if (pa != null)
                    pa.manager = this;
            }
            // Also wire any MatchDropSlot configured as pool areas
            var dropSlots = BoardArea.GetComponentsInChildren<MatchDropSlot>(includeInactive: true);
            foreach (var ds in dropSlots)
            {
                if (ds != null && ds.IsPoolArea)
                    ds.manager = this;
            }
        }

        public void OnTileDroppedInPoolKeepPosition(DraggableTile tile)
        {
            if (tile == null)
                return;
            // Detach from any question slot state and turn off highlights
            for (int i = 0; i < placed.Length; i++)
            {
                if (placed[i] == tile)
                {
                    placed[i] = null;
                    if (i >= 0 && i < questionItems.Count)
                        questionItems[i]?.SetHighlight(null);
                }
            }
            if (tile.transform is RectTransform rt)
            {
                // Preserve world position while changing parent and anchors
                var world = rt.position;
                rt.SetParent(BoardArea, worldPositionStays: true);
                SetCenterAnchors(rt);
                if (BoardArea is RectTransform prt)
                {
                    var local = (Vector2)prt.InverseTransformPoint(world);
                    rt.anchoredPosition = ClampAnchoredToParent(prt, rt, local);
                }
                poolAnchoredPos[tile] = rt.anchoredPosition;
                poolSiblingIndex[tile] = rt.GetSiblingIndex();
            }
            else
            {
                tile.MoveToPool(BoardArea);
            }
            DiscoverAudioManager.I?.PlaySfx(DiscoverSfx.ActivityDrop);
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
            var currentDifficulty = Settings != null ? Settings.Difficulty : Difficulty.Default;
            if (currentDifficulty != Difficulty.Easy)
            {
                // Clear all
                for (int i = 0; i < questionItems.Count; i++)
                    questionItems[i]?.SetHighlight(null);
                return;
            }

            for (int i = 0; i < questionItems.Count; i++)
            {
                var t = i < placed.Length ? placed[i] : null;
                if (t == null)
                { questionItems[i]?.SetHighlight(null); continue; }
                var expected = questionItems[i] != null ? questionItems[i].ExpectedAnswerId : null;
                if (!string.IsNullOrEmpty(expected) && t.CardData != null)
                {
                    bool isCorrect = t.CardData.Id == expected;
                    questionItems[i]?.SetHighlight(isCorrect);
                }
                else
                { questionItems[i]?.SetHighlight(null); }
            }
        }

        public bool IsTileAttached(DraggableTile tile)
        {
            if (tile == null || placed == null)
                return false;
            for (int i = 0; i < placed.Length; i++)
                if (placed[i] == tile)
                    return true;
            return false;
        }

        public override bool DoValidate()
        {
            int correct = 0;
            for (int i = 0; i < placed.Length; i++)
            {
                var t = placed[i];
                if (t == null)
                    continue;
                var expected = questionItems[i] != null ? questionItems[i].ExpectedAnswerId : null;
                if (t.CardData != null && !string.IsNullOrEmpty(expected) && t.CardData.Id == expected)
                    correct++;
            }

            // partial credit through base GetRoundScore01()
            _lastScore = placed.Length > 0 ? (float)correct / placed.Length : 0f;
            if (placed.Length > 0 && correct == placed.Length)
            {
                DiscoverAudioManager.I?.PlaySfx(DiscoverSfx.ActivitySuccess);
            }
            else if (placed.Length > 0)
            {
                DiscoverAudioManager.I?.PlaySfx(DiscoverSfx.ActivityBadMove);
            }
            return correct == placed.Length;
        }

        private float _lastScore = 0f;
        protected override float GetRoundScore01() => _lastScore;

        protected override void OnActivityFinished(bool lastRoundSuccess, int lastRoundPoints, float lastRoundSeconds, bool dueToTimeout)
        {
            if (!lastRoundSuccess)
            {
                Debug.Log("you failed", this);
            }
        }

        // Helpers
        private void ReturnTileToPoolAnimated(DraggableTile tile)
        {
            if (tile == null)
                return;
            if (tile.transform is not RectTransform rt)
            {
                tile.MoveToPool(BoardArea);
                return;
            }
            // Determine original pool position and sibling order
            poolAnchoredPos.TryGetValue(tile, out var target);
            poolSiblingIndex.TryGetValue(tile, out var sibIdx);

            // Move under pool while keeping world position, restore order
            rt.SetParent(BoardArea, worldPositionStays: true);
            if (sibIdx >= 0)
                rt.SetSiblingIndex(sibIdx);

            // Small upward bump then tween to memorized pool position
            var seq = DOTween.Sequence();
            var startPos = rt.anchoredPosition;
            seq.Append(rt.DOAnchorPos(startPos + new Vector2(0, 40f), 0.12f).SetEase(Ease.OutQuad));
            seq.Append(rt.DOAnchorPos(target, 0.18f).SetEase(Ease.OutCubic));
            seq.OnComplete(() =>
            {
                tile.MoveToPool(BoardArea);
                var ai2 = tile.GetComponent<AnswerItem>();
                if (ai2 != null)
                    ai2.AttachedTo = null;
            });
        }

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

        private Vector2 ClampAnchoredToParent(RectTransform parent, RectTransform child, Vector2 desired)
        {
            // Assumes both use centered anchors/pivot (we set that when spawning)
            var pRect = parent.rect;
            var cRect = child.rect;
            float halfPW = pRect.width * 0.5f;
            float halfPH = pRect.height * 0.5f;
            float halfCW = cRect.width * 0.5f;
            float halfCH = cRect.height * 0.5f;
            float minX = -halfPW + halfCW;
            float maxX = halfPW - halfCW;
            float minY = -halfPH + halfCH;
            float maxY = halfPH - halfCH;
            return new Vector2(Mathf.Clamp(desired.x, minX, maxX), Mathf.Clamp(desired.y, minY, maxY));
        }

        private static void SetCenterAnchors(RectTransform rt)
        {
            if (rt == null)
                return;
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
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
