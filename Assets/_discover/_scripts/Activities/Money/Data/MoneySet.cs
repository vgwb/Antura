using UnityEngine;
using System.Collections.Generic;
using System;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "MoneySet", menuName = "Antura/Activity/Money MoneySet")]
    public class MoneySet : ScriptableObject
    {
        public enum MoneyType { Coin, Paper }

        [System.Serializable]
        public class MoneyItem
        {
            public MoneyType Type;
            public float Value;

            public Difficulty Difficulty;
            public Sprite Image;
        }

        public string SetName;

        public List<MoneyItem> items = new List<MoneyItem>();
    }
}
