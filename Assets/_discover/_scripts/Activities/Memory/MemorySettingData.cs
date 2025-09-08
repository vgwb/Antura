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
