using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Antura.Discover.Activities;

namespace Antura.Discover
{
    public enum ActivityCode
    {
        Unknown = 0,
        Order = 1,
        Match = 2,
        Quiz = 3,
        CleanCanvas = 4,
        JigsawPuzzle = 5,
        Memory = 6,
        MoneyCount = 7,
        Piano = 8,
        TraceLine = 9,
    }

    [CreateAssetMenu(fileName = "ActivityData", menuName = "Antura/Discover/Activity")]
    public class ActivityData : IdentifiedData
    {
        public LocalizedString Name;

        [Header("Identification")]
        public ActivityCode Code = ActivityCode.Unknown;

        [Header("Media")]
        public Sprite Image;

        [Header("Skills engaged")]
        public List<ActivityCategory> Category;
        public List<ActivitySkill> Skills;

        [Header("Prefab")]
        public GameObject ActivityPrefab;

        [Tooltip("for the Website. This is a Markdown file that contains additional informations.")]
        public TextAsset AdditionalResources;

        [Header("Credits")]
        public List<AuthorCredit> Credits;
    }
}
