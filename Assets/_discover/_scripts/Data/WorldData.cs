using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "WorldData", menuName = "Antura/Discover/World Data")]
    public class WorldData : IdentifiedData
    {
        public Status Status = Status.Draft;
        public string Title;
        public string Description;
        public Countries Country;
        public LocationData Location;

        [Header("Media")]
        public Sprite PreviewImage;

        [Header("References")]
        public List<QuestData> Quests;

        [Header("Authoring Metadata")]
        public List<AuthorCredit> Credits;

    }
}
