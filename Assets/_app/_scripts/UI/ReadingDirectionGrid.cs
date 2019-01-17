using Antura.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    /// <summary>
    /// Updates the attached GridLayoutGroup to match the App's reading direction
    /// </summary>
    public class ReadingDirectionGrid : MonoBehaviour
    {
        private GridLayoutGroup gridLayoutGroup;

        void Awake()
        {
            gridLayoutGroup = GetComponent<GridLayoutGroup>();
            switch (AppConfig.TextDirection)
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
                    }
                    break;
                case TextDirection.RightToLeft:
                    switch (gridLayoutGroup.startCorner)
                    {
                        case GridLayoutGroup.Corner.LowerLeft:
                            gridLayoutGroup.startCorner = GridLayoutGroup.Corner.LowerRight;
                            break;
                        case GridLayoutGroup.Corner.UpperLeft:
                            gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperRight;
                            break;
                    }
                    break;
            }
        }
    }
}