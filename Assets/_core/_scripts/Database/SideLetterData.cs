using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Database
{

    [CreateAssetMenu]
    // Additional data partaining to a LetterData and not saved in the Static Database
    public class SideLetterData : ScriptableObject
    {
        public Vector2[] EmptyZones;
    }

}