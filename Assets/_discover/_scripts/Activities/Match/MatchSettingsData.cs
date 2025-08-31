using Antura.Discover;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [Serializable]
    public class MatchGroupData
    {
        [Tooltip("Question CardData")]
        public CardData Question;

        [Tooltip("All CardData answers that should match this question")]
        public List<CardData> Answers = new List<CardData>();
    }

    [CreateAssetMenu(fileName = "MatchSettingsData", menuName = "Antura/Activity/Match Settings")]
    public class MatchSettingsData : ActivitySettingsAbstract
    {
        [Header("--- Activity Memory Settings")]

        [Tooltip("Define each question with >= 1 answers")]
        public List<MatchGroupData> GroupsData = new List<MatchGroupData>();
    }
}
