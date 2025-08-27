using Antura.Discover;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "MemorySettingData", menuName = "Antura/Activity/Memory Data")]
    public class MemorySettingData : ActivitySettingsAbstract
    {
        [Header("--- Activity Memory Settings")]
        public List<CardData> CardsData = new List<CardData>();
    }
}
