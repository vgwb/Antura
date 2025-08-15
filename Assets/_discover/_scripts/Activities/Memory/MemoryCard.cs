using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Antura.Discover.Activities
{
    public class MemoryCard : MonoBehaviour, IPointerClickHandler
    {
        [Header("UI")]
        public Image faceImage;
        public Image backImage;

        [Header("State (runtime)")]
        public int pairId;
        public bool IsFaceUp { get; private set; }
        public bool IsLocked { get; private set; }

        [Header("Flip Settings")]
        public float flipDuration = 0.18f;
        public AnimationCurve flipCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private ActivityMemory manager;
        private RectTransform rt;
        private bool flipping;

        public void Init(ActivityMemory mgr, int id, Sprite face, Sprite back)
        {
            manager = mgr;
            pairId = id;
            faceImage.sprite = face;
            backImage.sprite = back;
            SetFaceUp(false, instant: true);
            IsLocked = false;
            rt = GetComponent<RectTransform>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (manager == null || flipping || IsLocked)
                return;
            if (IsFaceUp)
                return;
            manager.TryReveal(this);
        }

        public void Lock()
        {
            IsLocked = true;
            // (optional) add a little success pop
            StartCoroutine(ScalePunch(1.08f, 0.12f));
        }

        public void RevealUp()
        {
            if (!IsFaceUp)
                StartCoroutine(Flip(true));
        }

        public void HideDown()
        {
            if (IsFaceUp)
                StartCoroutine(Flip(false));
        }

        public void SetFaceUp(bool up, bool instant = false)
        {
            IsFaceUp = up;
            if (instant)
            {
                faceImage.enabled = up;
                backImage.enabled = !up;
                return;
            }
            StartCoroutine(Flip(up));
        }

        IEnumerator Flip(bool toFaceUp)
        {
            flipping = true;
            float t = 0f;

            // first half: shrink X to 0
            while (t < flipDuration)
            {
                t += Time.unscaledDeltaTime;
                float k = Mathf.Clamp01(t / flipDuration);
                float s = Mathf.Lerp(1f, 0f, flipCurve.Evaluate(k));
                transform.localScale = new Vector3(s, 1f, 1f);
                yield return null;
            }

            // swap visible side
            IsFaceUp = toFaceUp;
            faceImage.enabled = IsFaceUp;
            backImage.enabled = !IsFaceUp;

            // second half: grow X from 0 to 1
            t = 0f;
            while (t < flipDuration)
            {
                t += Time.unscaledDeltaTime;
                float k = Mathf.Clamp01(t / flipDuration);
                float s = Mathf.Lerp(0f, 1f, flipCurve.Evaluate(k));
                transform.localScale = new Vector3(s, 1f, 1f);
                yield return null;
            }
            transform.localScale = Vector3.one;

            flipping = false;
        }

        public IEnumerator ScalePunch(float amount, float time)
        {
            Vector3 baseScale = Vector3.one;
            Vector3 peak = baseScale * amount;
            float t = 0f;
            while (t < time)
            {
                t += Time.unscaledDeltaTime;
                float k = t / time;
                transform.localScale = Vector3.Lerp(peak, baseScale, k);
                yield return null;
            }
            transform.localScale = baseScale;
        }

        public void Wiggle(float angle = 7f, float time = 0.25f)
        {
            StopAllCoroutines();
            StartCoroutine(WiggleCo(angle, time));
        }

        IEnumerator WiggleCo(float angle, float time)
        {
            float t = 0f;
            while (t < time)
            {
                t += Time.unscaledDeltaTime;
                float s = Mathf.Sin(t / time * Mathf.PI * 3f) * angle;
                transform.localRotation = Quaternion.Euler(0, 0, s);
                yield return null;
            }
            transform.localRotation = Quaternion.identity;
        }
    }
}
