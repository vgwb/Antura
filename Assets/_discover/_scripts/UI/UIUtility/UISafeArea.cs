using UnityEngine;

namespace Antura.Discover.UI
{
    /// <summary>
    /// Applies the device safe area (notch, rounded corners) to this RectTransform.
    /// Place on a top-level UI panel (child of Canvas).
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UISafeArea : MonoBehaviour
    {
        private RectTransform panel;
        private Rect lastSafeArea = new Rect(0, 0, 0, 0);

        void Awake()
        {
            panel = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        void Update()
        {
            // Reapply if orientation/resolution changes
            if (Screen.safeArea != lastSafeArea)
                ApplySafeArea();
        }

        void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;
            lastSafeArea = safeArea;

            // Normalize safe area rect to [0..1] anchor space
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;
        }
    }
}
