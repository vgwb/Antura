using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "EconomySettings", menuName = "Antura/Discover/Economy Settings")]
    public class EconomySettings : ScriptableObject
    {
        [Header("Stars → Gems")]
        [Tooltip("Default cap for gems from quest stars (e.g., max 3).")]
        public int questStarCapDefault = 3;

        [Serializable] public struct QuestCap { public string questId; public int cap; }
        [Tooltip("Optional per-quest override for star→gem caps.")]
        public List<QuestCap> questStarCaps = new();

        [Header("XP Milestones → Gem Awards")]
        [Tooltip("XP thresholds that each grant gems (one-time).")]
        public int[] xpMilestones = new int[] { 1_000_000 };
        [Tooltip("Gems awarded per XP milestone reached.")]
        public int xpMilestoneGemAward = 1;

        [Header("Gem Milestones (badges/rewards, no auto-award by default)")]
        public int[] gemMilestones = new int[] { 10, 25, 50 };

        public int GetQuestStarCap(string questId)
        {
            for (int i = 0; i < questStarCaps.Count; i++)
                if (string.Equals(questStarCaps[i].questId, questId, StringComparison.OrdinalIgnoreCase))
                    return Mathf.Max(0, questStarCaps[i].cap);
            return Mathf.Max(0, questStarCapDefault);
        }
    }
}

