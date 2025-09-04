using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Antura.Discover; // for ActivityCode enum

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "OrderSettingsData", menuName = "Antura/Activity/Order Settings")]
    public class OrderSettingsData : ActivitySettingsAbstract
    {
        private void OnEnable()
        {
            ActivityCode = ActivityCode.Order;
        }

        [Header("--- Activity Order Settings")]

        [Tooltip("Between 2 and 10 items. The order here is the correct solution")]
        public List<CardData> ItemsData = new List<CardData>();



    }
}
