using System;

namespace Antura.Rewards
{
    [Serializable]
    public class RewardColor // model/decal/texture color
    {
        public string ID;
        public string Color1Name;
        public string Color2Name;
        public string Color1RGB; // "rrggbbaa"
        public string Color2RGB; // "rrggbbaa"
        public string LocID;
    }
}
