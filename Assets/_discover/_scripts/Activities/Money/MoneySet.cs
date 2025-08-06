using UnityEngine;
using System.Collections.Generic;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "MoneySet", menuName = "Antura/Activity/MoneySet")]
    public class MoneySet : ScriptableObject
    {
        public enum MoneyType { Coin, Paper }

        [System.Serializable]
        public class MoneyItem
        {
            public MoneyType Type;
            public int ValueInCents;
            public Sprite Image;
        }

        public List<MoneyItem> items = new List<MoneyItem>();
    }
}
