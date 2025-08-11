using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Antura.Discover.Activities
{
    public class ActivityQuiz : ActivityBase
    {
        [Header("Activity Quiz Settings")]
        public QuizSettingsData Settings;

        [Header("Override Settings")]
        public Difficulty ActivityDifficulty = Difficulty.Default;

        // Called by Validate button
        public override bool DoValidate()
        {
            return true;
        }

    }
}
