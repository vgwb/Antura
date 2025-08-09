using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.DeInspektor.Attributes;
using System;

namespace Antura.Discover
{
    [System.Serializable]
    public class ItemData
    {
        public string Code;
        public Sprite Icon;
        public int Quantity;
        public string DescriptionNode;
    }
}
