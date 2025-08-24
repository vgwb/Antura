using Antura.Discover;
using System;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [Serializable]
    public class ActivityConfig
    {
        [Tooltip("Activity Code used in a Node to activate it")]
        public string Code;

        [Tooltip("The settings data of the activity")]
        public ActivitySettingsAbstract ActivitySettings;

        [Tooltip("The GameObject of the activity prefab")]
        public GameObject ActivityGO;
    }
}
