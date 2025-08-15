using UnityEngine;
using System.Collections;

namespace Antura.Discover.UI
{
    public class UIShaker : MonoBehaviour
    {
        public IEnumerator Shake(RectTransform target, float duration, float strength)
        {
            if (!target)
                yield break;
            Vector3 origin = target.anchoredPosition;
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float dx = Mathf.Sin(t * 60f) * strength * (1f - t / duration);
                target.anchoredPosition = origin + new Vector3(dx, 0f, 0f);
                yield return null;
            }
            target.anchoredPosition = origin;
        }
    }
}
