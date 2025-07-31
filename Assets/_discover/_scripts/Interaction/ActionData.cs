using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.DeInspektor.Attributes;

namespace Antura.Minigames.DiscoverCountry
{
    public enum CommandType
    {
        Trigger = 1,
        UnityAction = 7,
        Area = 2,
        PlayerSpawn = 3,
        SetRespawn = 11,
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
        public CommandType Type;
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
        public CommandType Command;
        public GameObject mainObject;
        [Tooltip("Do not execute this command")]
        public bool Disabled = false;
        [Tooltip("Executes only if Commant = UnityAction")]
        public UnityEvent unityAction;
    }

    [System.Serializable]
    public class QuestActionData
    {
        public string ActionCode;
        public List<CommandData> Commands;
    }
}
