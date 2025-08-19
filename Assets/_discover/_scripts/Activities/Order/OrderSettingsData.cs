using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "OrderSettingsData", menuName = "Antura/Activity/Order Settings")]
    public class OrderSettingsData : ActivitySettingsAbstract
    {
        [Header("Order Settings (CardData only)")]
        [Tooltip("Between 2 and 10 items. The order here is the correct solution")]
        public List<Antura.Discover.CardData> ItemsData = new List<Antura.Discover.CardData>();

    }
}
