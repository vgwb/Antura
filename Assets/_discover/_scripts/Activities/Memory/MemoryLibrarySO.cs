using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "CardItemLibrary", menuName = "Antura/Activity/Memory Card Library")]
    public class CardItemLibrarySO : ScriptableObject
    {
        public List<CardItem> Items = new List<CardItem>();
    }
}
