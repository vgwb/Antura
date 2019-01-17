using DG.DeInspektor.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Map
{
    /// <summary>
    /// UI that shows which stage is currently selected among available stages.
    /// </summary>
    public class MapStageIndicator : MonoBehaviour
    {
        [Header("Prefabs")]
        public MapStageIndicatorIcon Icon;

        private readonly List<MapStageIndicatorIcon> icons = new List<MapStageIndicatorIcon>();

        /// <summary>
        /// Initializes the widget
        /// </summary>
        /// <param name="currStage">Current map stage (starting from 0)</param>
        /// <param name="totStages">Total stages</param>
        [DeMethodButton("TEST Assign 3/6", 0, 3, 6, mode = DeButtonMode.PlayModeOnly)]
        [DeMethodButton("TEST Assign 1/8", 0, 1, 8, mode = DeButtonMode.PlayModeOnly)]
        public void Init(int currStage, int totStages)
        {
            // Create correct number of stages
            if (icons.Count == 0) {
                icons.Add(Icon);
            }
            int len = icons.Count;
            if (len < totStages) {
                for (int i = len; i < totStages; ++i) {
                    MapStageIndicatorIcon ico = Instantiate(Icon);
                    ico.transform.SetParent(Icon.transform.parent, false);
                    icons.Add(ico);
                }
            } else if (len > totStages) {
                for (int i = len - 1; i > totStages - 1; --i) {
                    MapStageIndicatorIcon ico = icons[i];
                    icons.RemoveAt(i);
                    Destroy(ico.gameObject);
                }
            }

            // Assigns stage numbers
            len = icons.Count;
            for (int i = 0; i < len; ++i) {
                icons[i].AssignedStage = len - i;
            }

            // Set current stage
            len = icons.Count;
            for (int i = 0; i < len; ++i) {
                icons[i].Select(i == len - currStage - 1);
            }
        }
    }
}