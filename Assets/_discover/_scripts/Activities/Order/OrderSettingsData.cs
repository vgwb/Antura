using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "OrderSettingsData", menuName = "Antura/Activity/Order Settings")]
    public class OrderSettingsData : ActivitySettingsAbstract
    {
        [Header("--- Activity Order Settings")]

        [Tooltip("Between 2 and 10 items. The order here is the correct solution")]
        public List<CardData> ItemsData = new List<CardData>();

    }
}
