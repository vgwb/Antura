using Antura.Core;
using Antura.Language;
using Antura.Minigames;
using Antura.UI;
using UnityEngine;

namespace Antura.Minigames.ReadingGame
{
    public class ReadingBar : MonoBehaviour
    {
        public TextRender text;

        public RectTransform start;
        public RectTransform target;
        public RectTransform endCompleted;

        public Color clearColor;
        public Color doneColor;

        [Range(0, 1)]
        public float currentReading = 0;

        [Range(0, 1)]
        public float currentTarget = 1;

        public float startOffset = 2;
        public float endOffset = 4;

        bool completed = false;

        public MagnifingGlass glass;
        public ThreeSlicesSprite backSprite;
        SpriteFader glassFader;

        public bool showTarget = true;
        public bool showArrows = true;
        public bool shineWhenNearTarget = false;
        public float maxShineDistance = 100;
        public float minShineDistance = 50;

        [Range(0, 1)]
        public float alpha = 0;

        SpriteFader[] spriteFaders;

        Color[] startColors;

        public Color TextColor = Color.black;

        Vector2 lastSize = Vector2.one;
        bool active;

        public int Id;

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;

                glassFader.show = active;
                start.GetComponent<SpriteRenderer>().color = doneColor;
                target.gameObject.SetActive(active && !completed && showTarget);
            }

        }

        void Awake()
        {
            glassFader = glass.gameObject.GetComponent<SpriteFader>();

            Active = false;

            spriteFaders = GetComponentsInChildren<SpriteFader>(true);
            for (int i = 0; i < spriteFaders.Length; ++i)
            {
                spriteFaders[i].SetAlphaImmediate(0);
            }

            text.color = TextColor;
            text.TMPText.alpha = 0;
        }

        void Start()
        {
            UpdateParts();

            // Set glass and target
            target.localPosition = Vector3.Lerp(start.localPosition, endCompleted.localPosition, currentTarget);
            glass.transform.position = GetGlassWorldPosition();

            glass.ShowArrows = showArrows;

            target.gameObject.SetActive(active && showTarget);
            endCompleted.gameObject.SetActive(false);

            var startColor = active ? doneColor : clearColor;
            var oldColor = start.GetComponent<SpriteRenderer>().color;
            oldColor.r = startColor.r;
            oldColor.g = startColor.g;
            oldColor.b = startColor.b;
            oldColor.a = 0;
            start.GetComponent<SpriteRenderer>().color = oldColor;
        }

        void UpdateParts()
        {
            Vector2 size = lastSize;

            if (alpha > 0)
                size = lastSize = text.TMPText.GetPreferredValues();

            int rtlDir = LanguageSwitcher.I.IsLearningLanguageRTL() ? 1 : -1;
            text.TMPText.isRightToLeftText = LanguageSwitcher.I.IsLearningLanguageRTL();

            var oldStartPos = start.localPosition;
            oldStartPos.x = rtlDir * (size.x * 0.5f + startOffset);
            start.localPosition = oldStartPos;

            var oldEndPos = endCompleted.localPosition;
            oldEndPos.x = rtlDir * (-size.x * 0.5f - endOffset);
            endCompleted.localPosition = oldEndPos;

            // set target position
            var targetLocalPosition = Vector3.Lerp(start.localPosition, endCompleted.localPosition, currentTarget);
            target.localPosition = Vector3.Lerp(target.localPosition, targetLocalPosition, Time.deltaTime * 20);

            // set glass position
            glass.transform.position = Vector3.Lerp(glass.transform.position, GetGlassWorldPosition(), Time.deltaTime * 20);

            // Set Back Sprite
            float glassPercPos = Vector3.Distance(glass.transform.position, start.position) / Vector3.Distance(start.position, endCompleted.position);
            var oldPos = backSprite.transform.localPosition;

            oldPos.x = (start.localPosition.x + endCompleted.localPosition.x) * 0.5f;
            backSprite.transform.localPosition = oldPos;
            backSprite.donePercentage = 1 - glassPercPos;
            var oldScale = backSprite.transform.localScale;
            oldScale.x = Mathf.Abs(start.localPosition.x - endCompleted.localPosition.x) * 0.25f;
            backSprite.transform.localScale = oldScale;

            if (shineWhenNearTarget)
            {
                var distance = GetDistanceFromTarget();
                glass.Shining = 1.0f - Mathf.SmoothStep(0, 1, (Mathf.Abs(distance) - minShineDistance) / (maxShineDistance - minShineDistance));
                glass.Bad = Mathf.Lerp(glass.Bad, 1 - glass.Shining, Time.deltaTime * 5);
                glass.DistanceFactor = distance / 100;
            }
            else
            {
                glass.Shining = 0;
                glass.Bad = 0;
                glass.DistanceFactor = 0;
            }
        }

        void Update()
        {
            UpdateParts();
            UpdateText();

            glass.ShowArrows = showArrows;
            target.gameObject.SetActive(active && showTarget);
        }

        void UpdateText()
        {

            const float ALPHA_LERP_SPEED = 5.0f;

            var textAlpha = text.TMPText.alpha;

            var srcAlpha = textAlpha;
            var destAlpha = TextColor.a * alpha;


            if (Mathf.Abs(srcAlpha - destAlpha) < 0.01f)
                textAlpha = destAlpha;
            else
                textAlpha = Mathf.Lerp(srcAlpha, destAlpha, ALPHA_LERP_SPEED * Time.deltaTime);

            if (text.TMPText.alpha != textAlpha)
                text.TMPText.alpha = textAlpha;
        }

        public void Complete()
        {
            completed = true;
            target.gameObject.SetActive(false);
            endCompleted.gameObject.SetActive(true);
            var oldColor = endCompleted.GetComponent<SpriteRenderer>().color;
            oldColor.r = doneColor.r;
            oldColor.g = doneColor.g;
            oldColor.b = doneColor.b;
            endCompleted.GetComponent<SpriteRenderer>().color = oldColor;
            currentReading = 1;
            currentTarget = 1;
        }

        public bool SetGlassScreenPosition(Vector2 position, bool applyMagnetEffect)
        {
            if (!active)
                return completed;

            var startScreen = Camera.main.WorldToScreenPoint(start.position);
            var endScreen = Camera.main.WorldToScreenPoint(endCompleted.position);

            var glassScreenSize = Camera.main.WorldToScreenPoint(glass.transform.position + glass.GetSize())
                                  - Camera.main.WorldToScreenPoint(glass.transform.position);

            if (applyMagnetEffect && Mathf.Abs(endScreen.x - position.x) < Mathf.Abs(glassScreenSize.x) / 2)
            {
                position = endScreen;
                completed = true;
            }

            currentReading = 1.0f - Mathf.Clamp01((position.x - endScreen.x) / (startScreen.x - endScreen.x));

            if (currentReading >= 0.99f)
                completed = true;

            return completed;
        }

        public void Show(bool show)
        {
            alpha = show ? 1 : 0;

            for (int i = 0; i < spriteFaders.Length; ++i)
            {
                spriteFaders[i].show = show;
            }
            glassFader.show = active;
        }

        public Vector2 GetGlassScreenPosition()
        {
            var startScreen = Camera.main.WorldToScreenPoint(start.position);
            var endScreen = Camera.main.WorldToScreenPoint(endCompleted.position);

            return Vector3.Lerp(startScreen, endScreen, currentReading);
        }

        public Vector3 GetGlassWorldPosition()
        {
            return Vector3.Lerp(start.position, endCompleted.position, currentReading);
        }

        public float GetWidth()
        {
            var startScreen = Camera.main.WorldToScreenPoint(start.position);
            var endScreen = Camera.main.WorldToScreenPoint(endCompleted.position);

            return Vector3.Distance(startScreen, endScreen);
        }

        public float GetDistanceFromTarget()
        {
            return (currentTarget - currentReading) * GetWidth();
        }
    }
}
