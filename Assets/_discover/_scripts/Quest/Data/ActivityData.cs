using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.DeInspektor.Attributes;

namespace Antura.Discover
{
    [System.Serializable]
    public class ActivityData
    {
        [Tooltip("Activity Code used in a Node to activate it")]
        public string Code;

        [Tooltip("The GameObject containint the activity prefab")]
        public GameObject ActivityGO;

        [Tooltip("Optional permalink of the Node with the mission")]
        public string NodeDescription;

        [Tooltip("Optional permalink of the Node when success")]
        public string NodeSuccess;

        [Tooltip("Optional permalink of the Node when fail")]
        public string NodeFail;

    }
}
