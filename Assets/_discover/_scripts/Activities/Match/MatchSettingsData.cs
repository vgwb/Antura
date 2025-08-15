using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "MatchSettingsData", menuName = "Antura/Activity/Match Settings")]
    public class MatchSettingsData : ActivitySettingsAbstract
    {
        [Serializable]
        public class MatchPair
        {
            [Tooltip("Left/prompt item shown on slot")] public CardItem Left;
            [Tooltip("Right/answer item to match by dragging")] public CardItem Right;
        }

        [Header("Match Settings")]
        [Tooltip("Pairs to match. Left is shown as prompt per slot; Right are tiles to drag.")]
        public List<MatchPair> Pairs = new List<MatchPair>();
    }
}
