using System;
using System.Collections;
using UnityEngine;

namespace PetanqueGame.Physics
{
    public static class ThrowHelper
    {
        public static event Action<Transform> OnBouleThrown;

        public static IEnumerator ThrowWithCurve(
            Transform obj,
            Vector3 target,
            float height,
            float duration,
            Transform parentAfterThrow,
            Action onComplete = null)
        {
            Vector3 startPos = obj.position;
            float elapsed = 0f;
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            if (rb != null)
                rb.isKinematic = true;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float curvedY = Mathf.Sin(Mathf.PI * t) * height;
                obj.position = Vector3.Lerp(startPos, target, t) + Vector3.up * curvedY;
                elapsed += Time.deltaTime;
                yield return null;
            }

            obj.position = target;

            if (rb != null)
                rb.isKinematic = false;

            if (parentAfterThrow != null)
                obj.SetParent(parentAfterThrow);

            onComplete?.Invoke();

            OnBouleThrown?.Invoke(obj);
        }
    }
}
