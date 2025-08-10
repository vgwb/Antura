using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [CreateAssetMenu(fileName = "CardItemLibrary", menuName = "Antura/Activity/Cards Library")]
    public class CardItemLibraryData : ScriptableObject
    {
        public List<CardItem> Items = new List<CardItem>();
    }
}
