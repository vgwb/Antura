using Antura.Discover.Audio;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Antura.Discover.Activities
{
    public class ActivityMatch : ActivityBase, IActivity
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
            if (settings is MatchSettingsData matchSettings)
                Settings = matchSettings;
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
                foreach (var group in Settings.GroupsData)
                {
                    if (group == null || group.Question == null || group.Answers == null)
                        continue;
                    var question = group.Question;
                    // 1:1 mode: use only the first non-null answer, warn if more
                    var firstAnswer = group.Answers.Find(answer => answer != null);
                    if (firstAnswer != null)
                    {
                        pairs.Add((question, firstAnswer));
                        if (group.Answers.Count > 1)
                            Debug.LogWarning($"[ActivityMatch] 1:1 mode: Group with Question '{question?.Id}' has {group.Answers.Count} answers; using only the first.");
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
                var slotGameObject = Instantiate(questionItemPrefab, BoardArea);
                var slotRectTransform = slotGameObject.transform as RectTransform;
                if (slotRectTransform != null)
                {
                    slotRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    slotRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    slotRectTransform.pivot = new Vector2(0.5f, 0.5f);
                }
                // Prefer localized title, fallback to TitleEn or asset name
                string questionTitle = null;
                try
                { questionTitle = pair.Question.Title != null ? pair.Question.Title.GetLocalizedString() : null; }
                catch { }
                if (string.IsNullOrWhiteSpace(questionTitle))
                    questionTitle = !string.IsNullOrEmpty(pair.Question.TitleEn) ? pair.Question.TitleEn : pair.Question.name;
                slotGameObject.name = $"QuestionSlot_{i}_{questionTitle}";

                // Initialize via QuestionItem to encapsulate visuals and overlay
                var questionItem = slotGameObject.GetComponent<QuestionItem>();
                if (questionItem == null)
                    questionItem = slotGameObject.AddComponent<QuestionItem>();
                var sprite = ResolveSprite(pair.Question);
                questionItem.Init(questionTitle, sprite, i, this, pair.Question, pair.Answer?.Id);

                // Track structures: use the DropSlot created by QuestionItem, or ensure one exists
                var dropSlot = questionItem.DropSlot != null ? questionItem.DropSlot : slotGameObject.GetComponentInChildren<MatchDropSlot>(includeInactive: true);
                if (dropSlot == null)
                {
                    var dropGameObject = new GameObject("DropOverlay", typeof(RectTransform));
                    dropGameObject.transform.SetParent(slotGameObject.transform, false);
                    dropSlot = dropGameObject.AddComponent<MatchDropSlot>();
                }
                dropSlot.manager = this;
                dropSlot.slotIndex = i;
                dropSlot.Owner = questionItem;
                // tracked through questionItems
                questionItems.Add(questionItem);
                questionItem.ExpectedAnswerId = pair.Answer.Id;
            }
            // Manually distribute question slots horizontally on the upper part
            var rootRectTransform = BoardArea as RectTransform;
            if (rootRectTransform != null)
            {
                float width = rootRectTransform.rect.width;
                float height = rootRectTransform.rect.height;
                int count = questionItems.Count;
                for (int i = 0; i < count; i++)
                {
                    var questionItem = questionItems[i];
                    var rectTransform = questionItem ? questionItem.transform as RectTransform : null;
                    if (rectTransform != null)
                    {
                        float interpolationFactor = (count == 1) ? 0.5f : (float)i / (count - 1);
                        float halfChildWidth = rectTransform.rect.width * 0.5f;
                        float halfChildHeight = rectTransform.rect.height * 0.5f;
                        float paddingX = Mathf.Max(halfChildWidth, 24f); // keep fully inside + a tiny margin
                        float paddingYTop = Mathf.Max(halfChildHeight, 24f);
                        float xPosition = Mathf.Lerp(-width * 0.5f + paddingX, width * 0.5f - paddingX, interpolationFactor);
                        float yPosition = height * 0.25f; // upper band center
                        rectTransform.anchoredPosition = new Vector2(xPosition, yPosition);
                    }
                }
            }

            // Spawn answers tiles shuffled
            var answerList = new List<CardData>();
            foreach (var pair in pairs)
                answerList.Add(pair.Answer);
            Shuffle(answerList);
            // Build answers under the same board container
            var spawnedTiles = new List<DraggableTile>();
            foreach (var cardData in answerList)
            {
                var answerGameObject = Instantiate(answerItemPrefab, BoardArea);
                answerGameObject.name = cardData.name;
                var tile = answerGameObject.GetComponent<DraggableTile>();
                tile.InitGeneric(
                    cardData,
                    this.transform,
                    poolGetter: () => BoardArea,
                    onLift: NotifyTileLiftedFromSlot,
                    onReturn: NotifyTileReturnedToPool,
                    onHint: null,
                    owner: this);
                // Ensure CanvasGroup exists for proper drag raycast toggling
                if (answerGameObject.GetComponent<CanvasGroup>() == null)
                    answerGameObject.AddComponent<CanvasGroup>();
                // Ensure AnswerItem tracker exists
                var answerItem = answerGameObject.GetComponent<AnswerItem>();
                if (answerItem == null)
                    answerItem = answerGameObject.AddComponent<AnswerItem>();
                answerItem.AttachedTo = null;
                answerItem.Data = cardData;
                // Clamp tile during drag within the board area
                var clamp = answerGameObject.GetComponent<AnswerTileClamp>();
                if (clamp == null)
                    clamp = answerGameObject.AddComponent<AnswerTileClamp>();
                clamp.manager = this;
                if (answerGameObject.transform is RectTransform answerRectTransform)
                {
                    answerRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    answerRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    answerRectTransform.pivot = new Vector2(0.5f, 0.5f);
                }
                spawnedTiles.Add(tile);
            }

            // Distribute answers on the bottom part and memorize their home positions
            if (rootRectTransform != null)
            {
                float width = rootRectTransform.rect.width;
                float height = rootRectTransform.rect.height;
                int answerCount = spawnedTiles.Count;
                for (int i = 0; i < answerCount; i++)
                {
                    var tile = spawnedTiles[i];
                    if (tile.transform is RectTransform rectTransform)
                    {
                        float interpolationFactor = (answerCount == 1) ? 0.5f : (float)i / (answerCount - 1);
                        float xPosition = Mathf.Lerp(-width * 0.45f, width * 0.45f, interpolationFactor);
                        float yPosition = -height * 0.25f; // lower band
                        var position = new Vector2(xPosition, yPosition);
                        rectTransform.anchoredPosition = position;
                        poolAnchoredPos[tile] = position;
                        poolSiblingIndex[tile] = rectTransform.GetSiblingIndex();
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
            if (tile.transform is RectTransform tileRectTransform)
            {
                var questionRectTransform = questionItems[slotIndex] ? questionItems[slotIndex].transform as RectTransform : null;
                float questionHeight = questionRectTransform != null ? questionRectTransform.rect.height : 100f;
                float spacing = 12f + 60f; // extra to avoid label overlap
                tileRectTransform.anchorMin = new Vector2(0.5f, 1f);
                tileRectTransform.anchorMax = new Vector2(0.5f, 1f);
                tileRectTransform.pivot = new Vector2(0.5f, 1f);
                tileRectTransform.anchoredPosition = new Vector2(0f, -(questionHeight + spacing));
                tileRectTransform.SetAsLastSibling();
            }
            // Track attachment on the tile
            var answerItem = tile.GetComponent<AnswerItem>();
            if (answerItem != null)
                answerItem.AttachedTo = parentForAnswer;
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
            if (tile.transform is RectTransform tileRectTransform && BoardArea is RectTransform poolRectTransform)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(poolRectTransform, eventData.position, eventData.pressEventCamera, out var localPosition);
                tileRectTransform.SetParent(BoardArea, worldPositionStays: false);
                SetCenterAnchors(tileRectTransform);
                tileRectTransform.anchoredPosition = ClampAnchoredToParent(poolRectTransform, tileRectTransform, localPosition);
                tileRectTransform.SetAsLastSibling();
                poolAnchoredPos[tile] = tileRectTransform.anchoredPosition;
                poolSiblingIndex[tile] = tileRectTransform.GetSiblingIndex();
            }
            else
            {
                tile.MoveToPool(BoardArea);
            }

            // Clear tile attachment tracking
            var answerItem = tile.GetComponent<AnswerItem>();
            if (answerItem != null)
                answerItem.AttachedTo = null;

            DiscoverAudioManager.I?.PlaySfx(DiscoverSfx.ActivityDrop);
            UpdateValidateState();
            UpdateSlotHighlights();
        }

        private void EnsureBoardDropArea()
        {
            if (BoardArea is not RectTransform rootRectTransform)
                return;
            const string areaName = "BoardDropArea";
            var existingTransform = BoardArea.Find(areaName);
            RectTransform area;
            if (existingTransform == null)
            {
                var areaGameObject = new GameObject(areaName, typeof(RectTransform));
                area = areaGameObject.GetComponent<RectTransform>();
                area.SetParent(BoardArea, false);
            }
            else
            {
                area = existingTransform as RectTransform;
            }
            area.anchorMin = new Vector2(0f, 0f);
            area.anchorMax = new Vector2(1f, 1f);
            area.offsetMin = Vector2.zero;
            area.offsetMax = Vector2.zero;
            area.pivot = new Vector2(0.5f, 0.5f);

            var dropSlot = area.GetComponent<MatchDropSlot>();
            if (dropSlot == null)
                dropSlot = area.gameObject.AddComponent<MatchDropSlot>();
            var image = area.GetComponent<Image>();
            if (image == null)
                image = area.gameObject.AddComponent<Image>();
            image.color = new Color(1, 1, 1, 0f);
            image.raycastTarget = true;
            dropSlot.manager = this;
            dropSlot.IsPoolArea = true;
            // Make sure this area stays behind other children to not block drag start
            area.SetAsFirstSibling();
        }
        private void WireExistingDropAreas()
        {
            if (!BoardArea)
                return;
            // Wire MatchPoolDropArea
            var poolAreas = BoardArea.GetComponentsInChildren<MatchPoolDropArea>(includeInactive: true);
            foreach (var poolArea in poolAreas)
            {
                if (poolArea != null)
                    poolArea.manager = this;
            }
            // Also wire any MatchDropSlot configured as pool areas
            var dropSlots = BoardArea.GetComponentsInChildren<MatchDropSlot>(includeInactive: true);
            foreach (var dropSlot in dropSlots)
            {
                if (dropSlot != null && dropSlot.IsPoolArea)
                    dropSlot.manager = this;
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
            if (tile.transform is RectTransform tileRectTransform)
            {
                // Preserve world position while changing parent and anchors
                var worldPosition = tileRectTransform.position;
                tileRectTransform.SetParent(BoardArea, worldPositionStays: true);
                SetCenterAnchors(tileRectTransform);
                if (BoardArea is RectTransform parentRectTransform)
                {
                    var localPosition = (Vector2)parentRectTransform.InverseTransformPoint(worldPosition);
                    tileRectTransform.anchoredPosition = ClampAnchoredToParent(parentRectTransform, tileRectTransform, localPosition);
                }
                poolAnchoredPos[tile] = tileRectTransform.anchoredPosition;
                poolSiblingIndex[tile] = tileRectTransform.GetSiblingIndex();
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
                var placedTile = i < placed.Length ? placed[i] : null;
                if (placedTile == null)
                { questionItems[i]?.SetHighlight(null); continue; }
                var expected = questionItems[i] != null ? questionItems[i].ExpectedAnswerId : null;
                if (!string.IsNullOrEmpty(expected) && placedTile.CardData != null)
                {
                    bool isCorrect = placedTile.CardData.Id == expected;
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
                var placedTile = placed[i];
                if (placedTile == null)
                    continue;
                var expected = questionItems[i] != null ? questionItems[i].ExpectedAnswerId : null;
                if (placedTile.CardData != null && !string.IsNullOrEmpty(expected) && placedTile.CardData.Id == expected)
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
            if (tile.transform is not RectTransform rectTransform)
            {
                tile.MoveToPool(BoardArea);
                return;
            }
            // Determine original pool position and sibling order
            poolAnchoredPos.TryGetValue(tile, out var targetPosition);
            poolSiblingIndex.TryGetValue(tile, out var siblingIndex);

            // Move under pool while keeping world position, restore order
            rectTransform.SetParent(BoardArea, worldPositionStays: true);
            if (siblingIndex >= 0)
                rectTransform.SetSiblingIndex(siblingIndex);

            // Small upward bump then tween to memorized pool position
            var sequence = DOTween.Sequence();
            var startPosition = rectTransform.anchoredPosition;
            sequence.Append(rectTransform.DOAnchorPos(startPosition + new Vector2(0, 40f), 0.12f).SetEase(Ease.OutQuad));
            sequence.Append(rectTransform.DOAnchorPos(targetPosition, 0.18f).SetEase(Ease.OutCubic));
            sequence.OnComplete(() =>
            {
                tile.MoveToPool(BoardArea);
                var answerItem = tile.GetComponent<AnswerItem>();
                if (answerItem != null)
                    answerItem.AttachedTo = null;
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
