using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public enum ActionType
    {
        Trigger = 1,
        Area = 2,
        Spawn = 3,
        Collect = 4,
        Target = 5
    }

    [System.Serializable]
    public class ActionData
    {
        public ActionType Type;
        public string ActionCode;
        public GameObject mainObject;
        public GameObject Walls;
        public GameObject Area;
        public GameObject Beam;
        public GameObject Target;
        public GameObject SpawnPlayer;
        public GameObject DebugSpawn;
    }
}
