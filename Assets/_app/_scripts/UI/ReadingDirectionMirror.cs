using Antura.Core;
using DG.DeExtensions;
using UnityEngine;

namespace Antura.UI
{
    /// <summary>
    /// Mirrors UI children transforms to match reading direction
    /// </summary>
    public class ReadingDirectionMirror : MonoBehaviour
    {
        void Awake()
        {
            foreach (Transform childTr in transform)
            {
                switch (AppConfig.TextDirection)
                {
                    case TextDirection.LeftToRight:
                        // Default
                        break;
                    case TextDirection.RightToLeft:
                        var rectTransform = childTr.GetComponent<RectTransform>();
                        if (rectTransform != null)
                        {
                            rectTransform.SetAnchoredPosX(-rectTransform.anchoredPosition.x);
                        }
                        break;
                }
            }

        }
    }
}