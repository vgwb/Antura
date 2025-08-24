using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using System;

namespace Antura.Discover
{
    public enum TaskScope
    {
        Global = 1,
        Quest = 2
    }

    public enum RepeatMode
    {
        Once, Repeatable
    }

    public enum TaskType
    {
        None = 0,
        Collect = 1,
        Interact = 2,
        Performance = 3
    }

    [CreateAssetMenu(fileName = "TaskData", menuName = "Antura/Discover/Task")]
    public class TaskData : IdentifiedData
    {
        [Header("Identity")]

        [Tooltip("Global or specific to a country?")]
        public Countries Country;

        public TaskType Type;
        public TaskScope Scope = TaskScope.Quest;

        [Header("Content")]
        public LocalizedString Title;

        [Tooltip("Optional description")]
        public LocalizedString Description;

        [Header("Rewards")]
        [Tooltip("Cookies")]
        [Range(0, 20)]
        public int Cookies = 0;
        [Tooltip("Knowledge points")]
        public int Points;

        [Tooltip("Gems gained")]
        [Range(0, 1)]
        public int Gems;


    }
}
