using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Antura.Discover
{
    [Serializable]
    public class InventoryItem
    {
        public string Code;
        public Sprite Icon;
        public int Quantity;
        public string DescriptionNode;
    }
}
