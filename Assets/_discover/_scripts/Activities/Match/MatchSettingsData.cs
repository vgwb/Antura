using Antura.Discover;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "MatchSettingsData", menuName = "Antura/Activity/Match Settings")]
    public class MatchSettingsData : ActivitySettingsAbstract
    {
        [Serializable]
        public class MatchGroupData
        {
            [Tooltip("Question/master CardData")]
            public CardData Question;
            [Tooltip("All valid CardData answers that should match this question")]
            public List<CardData> Answers = new List<CardData>();
        }

        [Header("Groups using CardData (supports 1:1 and 1:many)")]
        [Tooltip("Define each Question with one or more Answers; a 1:1 pair is just a group with a single Answer.")]
        public List<MatchGroupData> GroupsData = new List<MatchGroupData>();
    }
}
