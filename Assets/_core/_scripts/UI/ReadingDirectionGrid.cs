using Antura.Core;
using UnityEngine;
using UnityEngine.UI;
using Antura.Language;

namespace Antura.UI
{
    /// <summary>
    /// Updates the attached GridLayoutGroup to match the App's reading direction
    /// </summary>
    public class ReadingDirectionGrid : MonoBehaviour
    {
        private GridLayoutGroup gridLayoutGroup;
        public LanguageUse LanguageUse = LanguageUse.Learning;

        void Awake()
        {
            gridLayoutGroup = GetComponent<GridLayoutGroup>();
            switch (LanguageSwitcher.I.GetLangConfig(LanguageUse).TextDirection)
            {
                case TextDirection.LeftToRight:
                    switch (gridLayoutGroup.startCorner)
                    {
                        case GridLayoutGroup.Corner.LowerRight:
                            gridLayoutGroup.startCorner = GridLayoutGroup.Corner.LowerLeft;
                            break;
                        case GridLayoutGroup.Corner.UpperRight:
                            gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
                            break;
                        case GridLayoutGroup.Corner.UpperLeft:
                            gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperRight;
                            break;
                        case GridLayoutGroup.Corner.LowerLeft:
                            gridLayoutGroup.startCorner = GridLayoutGroup.Corner.LowerRight;
                            break;
                    }

                    switch (gridLayoutGroup.childAlignment)
                    {
                        case TextAnchor.UpperLeft:
                            gridLayoutGroup.childAlignment = TextAnchor.UpperRight;
                            break;
                        case TextAnchor.UpperRight:
                            gridLayoutGroup.childAlignment = TextAnchor.UpperLeft;
                            break;
                        case TextAnchor.MiddleLeft:
                            gridLayoutGroup.childAlignment = TextAnchor.MiddleRight;
                            break;
                        case TextAnchor.MiddleRight:
                            gridLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
                            break;
                        case TextAnchor.LowerLeft:
                            gridLayoutGroup.childAlignment = TextAnchor.LowerRight;
                            break;
                        case TextAnchor.LowerRight:
                            gridLayoutGroup.childAlignment = TextAnchor.LowerLeft;
                            break;
                    }
                    break;
                case TextDirection.RightToLeft:
                    // Default
                    break;
            }
        }
    }
}
