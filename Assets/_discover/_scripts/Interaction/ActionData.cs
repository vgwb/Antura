using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Antura.Minigames.DiscoverCountry
{
    public enum ActionType
    {
        Trigger = 1,
        UnityAction = 7,
        Area = 2,
        PlayerSpawn = 3,
        Collect = 4,
        Target = 5,
        Activity = 6,
        InventoryAdd = 8,
        InventoryRemove = 9,
        Bones = 10,
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

    [System.Serializable]
    public class CommandData
    {
        public ActionType Command;
        public GameObject mainObject;
        public UnityEvent unityAction;
    }

    [System.Serializable]
    public class ActionNewData
    {
        public string ActionCode;
        public List<CommandData> Commands;
    }
}
