using Antura.Discover;
using System;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [Serializable]
    public class ActivityConfig
    {
        [Tooltip("The settings data of the activity")]
        public ActivitySettingsAbstract ActivitySettings;

        // [Tooltip("Optional: if left empty, ActivityManager will instantiate the prefab from ActivityList using ActivitySettings.ActivityCode")]
        // public GameObject ActivityGO;
    }
}
