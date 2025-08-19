using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.DeInspektor.Attributes;

namespace Antura.Discover
{
    public enum CommandType
    {
        Activity = 6,
        Area = 2,
        Bones = 10,
        Collect = 4,
        InventoryAdd = 8,
        InventoryRemove = 9,
        PlaySfx = 13,
        ProgressPoints = 17,
        QuestEnd = 12,
        SetActive = 18,
        SpawnPlayer = 3,
        SpawnSet = 11,
        Target = 5,
        TaskStart = 14,
        TaskSuccess = 15,
        TaskFail = 16,
        Trigger = 1,
        UnityAction = 7,
    }

    [System.Serializable]
    public class CommandData
    {
        public CommandType Command;
        [Tooltip("Optional GameObject for the command")]
        public GameObject mainObject;
        [Tooltip("Optional parameter for the command")]
        public string Parameter;
        [Tooltip("Bypass this command")]
        public bool Bypass = false;
        [Tooltip("Executes only if Command = UnityAction")]
        public UnityEvent unityAction;
    }

    [System.Serializable]
    public class QuestActionData
    {
        public string ActionCode;
        public List<CommandData> Commands;
    }
}
