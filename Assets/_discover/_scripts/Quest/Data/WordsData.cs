using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Antura.Discover
{

    [CreateAssetMenu(fileName = "WordsLibrary", menuName = "Antura/Discover/Words Library")]
    public class WordsData : ScriptableObject
    {
        public List<WordData> Words = new List<WordData>();
    }

}
