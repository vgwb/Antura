using UnityEngine;
using System.Collections.Generic;

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
            public int ValueInCents;
            public Sprite Image;
        }

        public string SetName;

        public bool ValuesInCents = true;

        public List<MoneyItem> items = new List<MoneyItem>();
    }
}
