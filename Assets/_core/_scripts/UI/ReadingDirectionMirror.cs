using Antura.Core;
using Antura.Language;
using DG.DeExtensions;
using UnityEngine;

namespace Antura.UI
{
    /// <summary>
    /// Mirrors direct UI children transforms to match reading direction
    /// </summary>
    public class ReadingDirectionMirror : MonoBehaviour
    {
        public LanguageUse LanguageUse = LanguageUse.Learning;

        void Awake()
        {
            foreach (Transform childTr in GetComponentsInChildren<Transform>(true))
            {
                if (childTr.parent != transform)
                    continue;
                switch (LanguageSwitcher.I.GetLangConfig(LanguageUse).TextDirection)
                {
                    case TextDirection.RightToLeft:
                        // Default
                        break;
                    case TextDirection.LeftToRight:
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
