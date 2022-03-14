using System;

namespace Antura.Rewards
{
    [Serializable]
    public class JourneyPositionRewardUnlock
    {
        // JP at which the following data is unlocked
        public string JourneyPositionID;

        // Numbers to unlock
        public int NewPropColor;
        public int NewPropBase;
        public int NewTexture;
        public int NewDecal;
    }
}
