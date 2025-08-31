using UnityEngine;
using System.Collections.Generic;
using System;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "MoneySet", menuName = "Antura/Activity/Money MoneySet")]
    public class MoneySet : ScriptableObject
    {
        [System.Serializable]
        public class MoneyItem
        {
            public MoneyType Type;
            public float Value;

            public Difficulty Difficulty;
            public Sprite Image;
            public int Width;
            public int Height;
        }

        public string SetName;

        [Tooltip("Currency symbol to show with values (€, zł, etc.).")]
        public string CurrencySymbol = "€";

        public List<MoneyItem> items = new List<MoneyItem>();
    }
}
