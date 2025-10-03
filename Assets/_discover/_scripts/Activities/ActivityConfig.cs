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

    }
}
