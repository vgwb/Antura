using Antura.Discover;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.DeInspektor;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "MemorySettingData", menuName = "Antura/Activity/Memory Data")]
    public class MemorySettingData : ActivitySettingsAbstract
    {
        private void OnEnable()
        {
            ActivityCode = ActivityCode.Memory;
        }

        [Header("--- Activity Memory Settings")]
        public List<CardData> CardsData = new List<CardData>();

        /// <summary>
        /// Resolves the candidate card pool based on the current selection mode.
        /// </summary>
        public List<CardData> ResolveCardPool()
        {
            var pool = new List<CardData>();

            switch (SelectionMode)
            {
                case SelectionMode.ManualSet:
                    AppendUnique(CardsData, pool);
                    break;
                case SelectionMode.RandomFromTopic:
                    AppendUnique(GetTopicCards(), pool);
                    break;
            }

            if (pool.Count == 0)
            {
                AppendUnique(CardsData, pool);
                AppendUnique(GetTopicCards(), pool);
            }

            return pool;
        }

        private List<CardData> GetTopicCards()
        {
            if (MainTopic == null)
                return null;
            try
            {
                return MainTopic.GetAllCards();
            }
            catch
            {
                return null;
            }
        }

        private static void AppendUnique(List<CardData> source, List<CardData> destination)
        {
            if (source == null)
                return;

            for (int i = 0; i < source.Count; i++)
            {
                var card = source[i];
                if (card == null)
                    continue;
                if (!destination.Contains(card))
                    destination.Add(card);
            }
        }



#if UNITY_EDITOR
        [ContextMenu("Fetch Cards From MainTopic")]
        private void FetchCardsFromMainTopic()
        {
            if (MainTopic == null)
            {
                Debug.LogWarning($"[{nameof(MemorySettingData)}] No MainTopic set.", this);
                return;
            }
            List<CardData> all;
            try
            { all = MainTopic.GetAllCards(); }
            catch { all = null; }
            if (all == null || all.Count == 0)
            {
                Debug.LogWarning($"[{nameof(MemorySettingData)}] MainTopic has no cards.", this);
                return;
            }
            // Record undo
            UnityEditor.Undo.RecordObject(this, "Fetch Cards From MainTopic");
            CardsData.Clear();
            foreach (var c in all)
            {
                if (c != null && !CardsData.Contains(c))
                    CardsData.Add(c);
            }
            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log($"[{nameof(MemorySettingData)}] Imported {CardsData.Count} cards from topic '{MainTopic.Name}'.", this);
        }
#endif
    }
}
