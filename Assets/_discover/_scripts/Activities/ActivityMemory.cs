using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class ActivityMemory : ActivityBase
    {
        [Serializable]
        public struct Item
        {
            public string Name;
            public Sprite Texture;
            public AudioClip AudioClip;
        }

        [Tooltip("Items Textures to use in the activity")]
        public List<Item> Items;
        [Tooltip("Difficulty: the number of coins to use.")]
        public int DifficultyLevel = 1;

        void Start()
        {

        }

    }
}
