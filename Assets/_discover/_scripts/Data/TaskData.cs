using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using System;

namespace Antura.Discover
{
    public enum TaskType
    {
        None = 0,
        Collect = 1,
        Reach = 2,
        Interact = 3,
        Performance = 4
    }

    [CreateAssetMenu(fileName = "TaskData", menuName = "Antura/Discover/Task")]
    public class TaskData : IdentifiedData
    {
        [Header("Identity")]

        [Tooltip("Global or specific to a country?")]
        public Countries Country;

        public TaskType Type;

        [Header("Content")]
        public LocalizedString Title;

        [Tooltip("Optional description")]
        public LocalizedString Description;

        [Header("Achievements")]

        [Tooltip("Total or per item is collect Task")]
        public int ProgressPoints;

        [Tooltip("Gems gained")]
        public int Gems;


    }
}
