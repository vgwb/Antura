using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "MemorySettingData", menuName = "Antura/Activity/Memory Data")]
    public class MemorySettingData : ActivitySettingsAbstract
    {
        [Header("Memory Game Settings")]
        public CardItemLibraryData CardLibrary;
    }
}
