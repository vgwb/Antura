using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Antura.Discover.Activities
{
    public class ActivityJigsawPuzzle : ActivityBase
    {
        [Header("Puzzle Settings")]
        public Texture2D PuzzeleImage;
        public int HorizontalPieces = 4;
        public int VerticalPieces = 4;
        public Difficulty Difficulty = Difficulty.Normal;

        [Header("Scene Refs")]
        public RectTransform gridParent;
        public RectTransform poolParent;
        public RectTransform dragLayer;
        public GameObject slotPrefab;
        public GameObject piecePrefab;
        public Button validateButton;
        public Image tutorialUnderlayImage;

        [Header("Layout")]
        public float slotSpacing = 8f;
        public float snapDistance = 60f;

        [Header("Visual")]
        [Range(0f, 1f)] public float tutorialUnderlayAlpha = 0.35f;

        private JigsawSlot[,] slots;
        private List<JigsawPiece> pieces = new List<JigsawPiece>();
        private bool built;
        private int cols;
        private int rows;

        public override void Init()
        {
            if (built)
                return;
            ResolveDifficulty();
            BuildPuzzle();
            UpdateValidateButton();
            built = true;
        }

        private void ResolveDifficulty()
        {
            switch (Difficulty)
            {
                case Difficulty.Tutorial:
                case Difficulty.Easy:
                    rows = cols = 3;
                    break;
                case Difficulty.Normal:
                    rows = cols = 4;
                    break;
                case Difficulty.Expert:
                    rows = cols = 5;
                    break;
                default:
                    rows = VerticalPieces;
                    cols = HorizontalPieces;
                    break;
            }
            // Allow manual overrides if fields explicitly changed
            if (HorizontalPieces > 0)
                cols = HorizontalPieces;
            if (VerticalPieces > 0)
                rows = VerticalPieces;
        }

        private void BuildPuzzle()
        {
            if (!PuzzeleImage || !gridParent || !poolParent || !slotPrefab || !piecePrefab)
            {
                Debug.LogWarning("Jigsaw: Missing references.");
                return;
            }

            // Clear previous
            for (int i = gridParent.childCount - 1; i >= 0; i--)
                Destroy(gridParent.GetChild(i).gameObject);
            for (int i = poolParent.childCount - 1; i >= 0; i--)
                Destroy(poolParent.GetChild(i).gameObject);
            slots = new JigsawSlot[rows, cols];
            pieces.Clear();

            // Underlay (tutorial)
            if (tutorialUnderlayImage)
            {
                if (Difficulty == Difficulty.Tutorial)
                {
                    tutorialUnderlayImage.gameObject.SetActive(true);
                    tutorialUnderlayImage.sprite = Sprite.Create(PuzzeleImage,
                        new Rect(0, 0, PuzzeleImage.width, PuzzeleImage.height),
                        new Vector2(0.5f, 0.5f), 100f);
                    var c = tutorialUnderlayImage.color;
                    c.a = tutorialUnderlayAlpha;
                    tutorialUnderlayImage.color = c;
                }
                else
                    tutorialUnderlayImage.gameObject.SetActive(false);
            }

            // Compute display scaling inside gridParent rect
            var gridRect = gridParent.rect;
            float scale = Mathf.Min(gridRect.width / PuzzeleImage.width, gridRect.height / PuzzeleImage.height);
            float pieceDisplayW = (PuzzeleImage.width / (float)cols) * scale;
            float pieceDisplayH = (PuzzeleImage.height / (float)rows) * scale;

            float totalW = cols * pieceDisplayW + (cols - 1) * slotSpacing;
            float totalH = rows * pieceDisplayH + (rows - 1) * slotSpacing;
            float originX = -totalW * 0.5f + pieceDisplayW * 0.5f;
            float originY = totalH * 0.5f - pieceDisplayH * 0.5f;

            // Create slots
            for (int r = 0; r < rows; r++)
            {
                for (int cIdx = 0; cIdx < cols; cIdx++)
                {
                    var slotGO = Instantiate(slotPrefab, gridParent);
                    var rt = slotGO.GetComponent<RectTransform>();
                    rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);
                    rt.sizeDelta = new Vector2(pieceDisplayW, pieceDisplayH);
                    rt.anchoredPosition = new Vector2(
                        originX + cIdx * (pieceDisplayW + slotSpacing),
                        originY - r * (pieceDisplayH + slotSpacing)
                    );

                    var slot = slotGO.GetComponent<JigsawSlot>();
                    slot.row = r;
                    slot.col = cIdx;
                    slot.manager = this;
                    slots[r, cIdx] = slot;
                }
            }

            // Slice texture & create pieces (sprites)
            var srcPW = PuzzeleImage.width / cols;
            var srcPH = PuzzeleImage.height / rows;

            List<JigsawPiece> tempPieces = new List<JigsawPiece>();
            for (int r = 0; r < rows; r++)
            {
                for (int cIdx = 0; cIdx < cols; cIdx++)
                {
                    Rect rect = new Rect(cIdx * srcPW, (rows - 1 - r) * srcPH, srcPW, srcPH); // flip Y for Unity texture coords
                    var sprite = Sprite.Create(PuzzeleImage, rect, new Vector2(0.5f, 0.5f), 100f);

                    var pieceGO = Instantiate(piecePrefab, poolParent);
                    var pieceRt = pieceGO.GetComponent<RectTransform>();
                    pieceRt.anchorMin = pieceRt.anchorMax = new Vector2(0.5f, 0.5f);
                    pieceRt.pivot = new Vector2(0.5f, 0.5f);
                    pieceRt.sizeDelta = new Vector2(pieceDisplayW, pieceDisplayH);

                    var img = pieceGO.GetComponent<Image>();
                    if (img)
                    { img.sprite = sprite; img.preserveAspect = true; }

                    var piece = pieceGO.GetComponent<JigsawPiece>();
                    piece.manager = this;
                    piece.targetRow = r;
                    piece.targetCol = cIdx;
                    piece.dragLayer = dragLayer;
                    piece.poolParent = poolParent;
                    piece.snapDistance = snapDistance;
                    piece.originalLocalPos = RandomScatterInPool(pieceRt, poolParent.rect);
                    pieceRt.anchoredPosition = piece.originalLocalPos;
                    tempPieces.Add(piece);
                }
            }

            // Randomize piece order in pool
            for (int i = tempPieces.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (tempPieces[i], tempPieces[j]) = (tempPieces[j], tempPieces[i]);
            }
            pieces.AddRange(tempPieces);
        }

        private Vector2 RandomScatterInPool(RectTransform piece, Rect poolRect)
        {
            // Cluster toward center (factor < 1 shrinks area)
            float factor = 0.45f;
            float halfW = poolRect.width * 0.5f * factor;
            float halfH = poolRect.height * 0.5f * factor;
            return new Vector2(Random.Range(-halfW, halfW), Random.Range(-halfH, halfH));
        }

        public void TryPlaceAtSlot(JigsawPiece piece)
        {
            // Find nearest slot (even if occupied)
            JigsawSlot best = null;
            float bestDist = float.MaxValue;
            var piecePos = piece.RectTransform.position;

            foreach (var slot in slots)
            {
                float d = Vector3.Distance(piecePos, slot.RectTransform.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    best = slot;
                }
            }

            if (best == null || bestDist > piece.snapDistance)
            {
                // No snap -> stay (return to previous slot if existed? design: back to pool/original)
                if (piece.PreviousSlot != null)
                {
                    piece.SetParentSlot(piece.PreviousSlot);
                }
                else
                {
                    piece.ReturnToPool();
                }
                UpdateValidateButton();
                return;
            }

            // Slot free -> just place
            if (best.currentPiece == null)
            {
                piece.SetParentSlot(best);
                UpdateValidateButton();
                return;
            }

            // Slot occupied -> swap
            var occupant = best.currentPiece;
            if (occupant == piece)
            {
                // Already there
                piece.SetParentSlot(best);
                UpdateValidateButton();
                return;
            }

            // Determine where occupant should go:
            // If piece came from another slot, occupant moves there.
            // Else occupant goes back (scattered) to pool.
            if (piece.PreviousSlot != null && piece.PreviousSlot != best)
            {
                // Move occupant to previous slot
                occupant.DetachFromSlot();
                occupant.SetParentSlot(piece.PreviousSlot);
            }
            else
            {
                // Occupant to pool (scatter)
                occupant.DetachFromSlot();
                var scatter = RandomScatterInPool(occupant.RectTransform, poolParent.rect);
                occupant.ReturnToPoolScatter(scatter);
            }

            // Place dragged piece into target slot
            piece.SetParentSlot(best);

            UpdateValidateButton();
        }

        private void UpdateValidateButton()
        {
            if (!validateButton)
                return;
            validateButton.interactable = AllSlotsFilled();
        }

        private bool AllSlotsFilled()
        {
            foreach (var slot in slots)
                if (slot.currentPiece == null)
                    return false;
            return true;
        }

        public override bool DoValidate()
        {
            if (!AllSlotsFilled())
            {
                Debug.Log("Jigsaw: not all pieces placed.");
                return false;
            }

            List<JigsawPiece> wrong = new List<JigsawPiece>();
            foreach (var slot in slots)
            {
                if (slot.currentPiece.targetRow != slot.row || slot.currentPiece.targetCol != slot.col)
                    wrong.Add(slot.currentPiece);
            }

            if (wrong.Count == 0)
            {
                Debug.Log("Jigsaw: success.");
                return true;
            }

            StartCoroutine(ShakePieces(wrong));
            return false;
        }

        private IEnumerator ShakePieces(List<JigsawPiece> targets)
        {
            float duration = 0.35f;
            float strength = 10f;
            Dictionary<RectTransform, Vector3> original = new Dictionary<RectTransform, Vector3>();
            foreach (var p in targets)
                original[p.RectTransform] = p.RectTransform.anchoredPosition;

            float t = 0f;
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                float phase = Mathf.Sin(t * 40f) * strength;
                foreach (var kv in original)
                    kv.Key.anchoredPosition = kv.Value + new Vector3(phase, 0, 0);
                yield return null;
            }
            foreach (var kv in original)
                kv.Key.anchoredPosition = kv.Value;
        }
    }
}
