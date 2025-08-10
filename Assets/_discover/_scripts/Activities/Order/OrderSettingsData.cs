using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "OrderSettingsData", menuName = "Antura/Activity/Order Settings")]
    public class OrderSettingsData : ActivitySettingsAbstract
    {
        [Header("Order Settings")]
        [Tooltip("Between 2 and 10 items. The order here is the correct target order.")]
        public List<CardItem> Items;              // 2..10

    }
}
