using System;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [Serializable]
    public struct CardItem
    {
        public string Name;
        public Sprite Image;
        public string Description;
        public AudioClip AudioClip;
    }
}
