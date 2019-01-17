using System;

namespace Antura.Rewards
{
    /// <summary>
    /// A single piece of reward.
    /// </summary>
    [Serializable]
    public class RewardBase
    {
        public string ID;
    }

    [Serializable]
    public class RewardProp : RewardBase
    {
        public string RewardName;
        public string BoneAttach;
        public string Material1;
        public string Material2;
        public string Category;
    }

    [Serializable]
    public class RewardDecal : RewardBase
    {
    }

    [Serializable]
    public class RewardTexture : RewardBase 
    {
    }
}