using Antura.UI;
using Antura.Database;
using Antura.Language;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System;
using DG.DeInspektor.Attributes;
using DG.Tweening;


namespace Antura.Discover.Activities
{
    public class DebugActivityLauncher : MonoBehaviour
    {
        public bool AutoLaunch = false;

        public ActivityConfig ActivityConfig;

        [Tooltip("Force the difficulty of the activity")]
        public Difficulty Difficulty = Difficulty.Default;

        void Start()
        {
            if (AutoLaunch)
            {
                LaunchActivity();
            }
        }

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        public void LaunchActivity()
        {
            var settings = ActivityConfig.ActivitySettings;
            if (Difficulty != Difficulty.Default)
            {
                settings.Difficulty = Difficulty;
            }
            ActivityManager.I.Launch(settings, "");
        }
    }
}
