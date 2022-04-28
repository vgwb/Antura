using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Antura.Database
{
    [Serializable]
    public class DrawingData
    {
        public String Id;
        public String Unicode;
        public String Value;
        public String Atlas;
    }

    [CreateAssetMenu(menuName = "Antura/Drawing Data")]
    public class DrawingsData : ScriptableObject
    {
        public List<DrawingData> Drawings = new List<DrawingData>();
    }

}
