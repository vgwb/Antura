using System;
using System.Collections.Generic;

namespace Antura.Discover
{
    [Serializable]
    public class CardUnlockByQuest
    {
        public string QuestId;
        public int Count;
        public long FirstUnixTime;
        public long LastUnixTime;
    }

    [Serializable]
    public class CardState
    {
        public string Id;
        public bool Unlocked;
        public long UnlockedUnixTime;
        public int UnlockCount;
        public int ProgressPoints;
        public List<CardUnlockByQuest> PerQuest = new();
    }

    [Serializable]
    public class PlayerCollectionSave
    {
        public int Version = 1;
        public List<CardState> Cards = new();
    }
}
