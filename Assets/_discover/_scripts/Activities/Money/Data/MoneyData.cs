using UnityEngine;
using System.Collections.Generic;

namespace Antura.Discover.Activities
{
    public class MoneyData
    {
        public float TargetAmount;
        public float CurrentAmount;
        public List<MoneySet.MoneyItem> AvailableItems = new List<MoneySet.MoneyItem>();
        public float TimeRemaining;
        public bool IsCompleted => Mathf.Approximately(CurrentAmount, TargetAmount);
    }
}
