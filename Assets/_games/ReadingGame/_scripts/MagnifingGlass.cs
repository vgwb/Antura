using Antura.Language;
using DG.DeExtensions;
using UnityEngine;

namespace Antura.Minigames.ReadingGame
{
    public class MagnifingGlass : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;

        public SpriteRenderer[] shines;
        float[] startShinesAlpha;
        Vector3[] startLeftArrowPositions;
        Vector3[] startRightArrowPositions;

        public Color normalColor;
        public Color badColor;

        public float Shining = 0;
        public float Bad = 0;
        public float DistanceFactor = 0;

        public SpriteRenderer[] leftArrows;
        public SpriteRenderer[] rightArrows;
        public float[] leftArrowsAlpha;
        public float[] rightArrowsAlpha;

        public Transform handleOffset;

        float arrowAnimation = 0;

        public bool ShowArrows { get; internal set; }

        void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            startShinesAlpha = new float[shines.Length];
            startLeftArrowPositions = new Vector3[leftArrows.Length];
            startRightArrowPositions = new Vector3[rightArrows.Length];
            leftArrowsAlpha = new float[leftArrows.Length];
            rightArrowsAlpha = new float[rightArrows.Length];

            for (int i = 0; i < shines.Length; ++i)
            {
                startShinesAlpha[i] = shines[i].color.a;
            }

            int rtlDir = LanguageSwitcher.I.IsLearningLanguageRTL() ? 1 : -1;

            for (int i = 0; i < leftArrows.Length; ++i)
            {
                startLeftArrowPositions[i] = leftArrows[i].transform.localPosition;
                startLeftArrowPositions[i].x *= rtlDir;
                if (rtlDir == -1)
                    leftArrows[i].transform.Rotate(0, 0, 180);
                leftArrowsAlpha[i] = 0;
            }

            for (int i = 0; i < rightArrows.Length; ++i)
            {
                startRightArrowPositions[i] = rightArrows[i].transform.localPosition;
                startRightArrowPositions[i].x *= rtlDir;
                if (rtlDir == -1)
                    rightArrows[i].transform.Rotate(0, 0, 180);
                rightArrowsAlpha[i] = 0;
            }
        }

        void Start()
        {
            spriteRenderer.sharedMaterial = ((ReadingGameGame)ReadingGameGame.I).magnifyingGlassMaterial;
            Update();
        }

        public Vector3 GetSize()
        {
            return spriteRenderer.bounds.extents;
        }

        void Update()
        {
            arrowAnimation += Time.deltaTime;
            arrowAnimation = Mathf.Repeat(arrowAnimation, 1);

            var oldColor = spriteRenderer.color;
            var oldAlpha = oldColor.a;
            oldColor = Color.Lerp(normalColor, badColor, Bad);
            oldColor.a = oldAlpha;
            spriteRenderer.color = oldColor;

            for (int i = 0; i < shines.Length; ++i)
            {
                var old = shines[i].color;
                old.a = spriteRenderer.color.a * startShinesAlpha[i] * Shining;
                shines[i].color = old;
            }

            const float ARROW_ANIMATION_DISTANCE = 0.25f;

            for (int i = 0; i < leftArrows.Length; ++i)
            {
                leftArrows[i].transform.localPosition = startLeftArrowPositions[i] + Mathf.Cos(arrowAnimation * Mathf.PI * 2) * ARROW_ANIMATION_DISTANCE * Vector3.left;

                var old = leftArrows[i].color;
                leftArrowsAlpha[i] = Mathf.Lerp(leftArrowsAlpha[i], (ShowArrows && DistanceFactor < -(i + 1)) ? 1 : 0, Time.deltaTime * 5);
                old.a = spriteRenderer.color.a * leftArrowsAlpha[i];
                leftArrows[i].color = old;
            }

            for (int i = 0; i < rightArrows.Length; ++i)
            {
                rightArrows[i].transform.localPosition = startRightArrowPositions[i] + Mathf.Cos(arrowAnimation * Mathf.PI * 2) * ARROW_ANIMATION_DISTANCE * Vector3.left;

                var old = leftArrows[i].color;
                rightArrowsAlpha[i] = Mathf.Lerp(rightArrowsAlpha[i], (ShowArrows && DistanceFactor > (i + 1)) ? 1 : 0, Time.deltaTime * 5);
                old.a = spriteRenderer.color.a * rightArrowsAlpha[i];
                rightArrows[i].color = old;
            }
        }
    }
}
