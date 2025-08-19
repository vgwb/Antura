using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "EconomySettings", menuName = "Antura/Discover/Economy Settings")]
    public class EconomySettings : ScriptableObject
    {
        // -------- Stars → Gems (quests) --------
        [Header("Stars → Gems")]
        [Tooltip("Default cap for gems from quest stars (e.g., max 3).")]
        public int questStarCapDefault = 3;

        [Serializable] public struct QuestCap { public string questId; public int cap; }
        [Tooltip("Optional per-quest override for star→gem caps.")]
        public List<QuestCap> questStarCaps = new List<QuestCap>();

        public int GetQuestStarCap(string questId)
        {
            for (int i = 0; i < questStarCaps.Count; i++)
                if (string.Equals(questStarCaps[i].questId, questId, StringComparison.OrdinalIgnoreCase))
                    return Mathf.Max(0, questStarCaps[i].cap);
            return Mathf.Max(0, questStarCapDefault);
        }

        // -------- KP milestones → Gems --------
        [Header("Knowledge Point Milestones → Gem Awards")]
        [Tooltip("KP thresholds that each grant gems once (e.g., 1,000,000).")]
        public int[] xpMilestones = new int[] { 1_000_000 };

        [Tooltip("Gems awarded per KP milestone reached.")]
        public int xpMilestoneGemAward = 1;

        [Header("Gem Milestones (badges/no auto-award)")]
        public int[] gemMilestones = new int[] { 10, 25, 50 };

        // -------- Card → KP (per interaction) --------
        [Header("Card → Knowledge Points (per interaction)")]
        [Tooltip("KP for a correct answer on a card interaction.")]
        public int cardPointsCorrect = 2;

        [Tooltip("KP for an incorrect answer on a card interaction.")]
        public int cardPointsWrong = 1;

        [Tooltip("How much mastery reduces per-interaction KP. 0=no effect, 1=full reduction at master=1.")]
        [Range(0f, 1f)] public float pointsMasteryPenalty = 0.7f;

        [Tooltip("Gentle diminishing per interaction count. 0 = off.")]
        [Range(0f, 0.1f)] public float pointsDiminishPerInteraction = 0.0f;

        [Tooltip("Minimum KP given per interaction after penalties.")]
        public int minPointsPerInteraction = 1;

        // -------- Card → Mastery Points (per interaction) --------
        [Header("Card → Mastery Points (per interaction)")]
        [Tooltip("MP gained on correct.")]
        public int cardMpCorrect = 2;

        [Tooltip("MP gained on wrong.")]
        public int cardMpWrong = 1;
    }
}
