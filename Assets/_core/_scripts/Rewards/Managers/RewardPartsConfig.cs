using System;
using System.Collections.Generic;
using Antura.Core;
using Antura.Database;

namespace Antura.Rewards
{
    /// <summary>
    /// Defines the relationship between unlocks and
    /// </summary>
    [Serializable]
    public class RewardsUnlocksConfig
    {
        public List<JourneyPositionRewardUnlock> JourneyPositionsUnlocks;
    }


    /// <summary>
    /// Defines all the possible item parts that can be unlocked.
    /// </summary>
    [Serializable]
    public class RewardPartsConfig
    {
        // A reward is made of 1 of each of the following PARTS
        // the combinations of 2 of these is a Reward Pack

        // These are all the different models
        public List<RewardProp> PropBases;
        public List<RewardColor> PropColors;

        // decals are just a different type of Reward
        public List<RewardDecal> DecalBases;          // decal
        public List<RewardColor> DecalColors;     // decal color

        // textures are just a different type of Reward
        public List<RewardTexture> TextureBases;
        public List<RewardColor> TextureColors;


        public IEnumerable<RewardBase> GetBasesForType(RewardBaseType type)
        {
            switch (type)
            {
                case RewardBaseType.Prop:
                    foreach (var b in PropBases)
                        yield return b;
                    break;
                case RewardBaseType.Decal:
                    foreach (var b in DecalBases)
                        yield return b;
                    break;
                case RewardBaseType.Texture:
                    foreach (var b in TextureBases)
                        yield return b;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
        }

        public IEnumerable<RewardColor> GetColorsForType(RewardBaseType type)
        {
            switch (type)
            {
                case RewardBaseType.Prop:
                    foreach (var col in PropColors)
                        yield return col;
                    break;
                case RewardBaseType.Decal:
                    foreach (var col in DecalColors)
                        yield return col;
                    break;
                case RewardBaseType.Texture:
                    foreach (var col in TextureColors)
                        yield return col;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
        }

        public RewardPartsConfig GetClone()
        {
            return MemberwiseClone() as RewardPartsConfig;
        }
    }

    /// <summary>
    /// A single reward pack (i.e. an effective rewards that can be obtained)
    /// </summary>
    [Serializable]
    public class RewardPack
    {
        public static bool FORCE_ALL_UNLOCKED = false;

        public RewardBaseType BaseType;

        public RewardBase RewardBase;
        public string BaseId { get { return RewardBase.ID; } }

        public RewardColor RewardColor;
        public string ColorId { get { return RewardColor.ID; } }

        public RewardPack(RewardBaseType baseType, RewardBase rewardBase, RewardColor rewardColor)
        {
            this.BaseType = baseType;
            this.RewardBase = rewardBase;
            this.RewardColor = rewardColor;
            this.unlockData = null; // Null = locked
        }

        public string Category
        {
            get
            {
                var prop = RewardBase as RewardProp;
                if (prop != null)
                    return prop.Category;
                return "";
            }
        }

        public string UniqueId
        {
            get { return BaseId + "_" + ColorId; }
        }

        #region State

        private RewardPackUnlockData unlockData;

        public void SetUnlockData(RewardPackUnlockData data)
        {
            this.unlockData = data;
        }

        public bool IsLocked
        {
            get
            {
                if (FORCE_ALL_UNLOCKED)
                    return false;
                if (unlockData == null)
                    return true;
                return unlockData.IsLocked;
            }
        }
        public bool IsUnlocked
        {
            get
            {
                return !IsLocked;
            }
        }

        public bool IsNew
        {
            get
            {
                if (FORCE_ALL_UNLOCKED)
                    return false;
                return unlockData.IsNew;
            }
        }

        public bool IsFoundAtJourneyPosition(JourneyPosition jp)
        {
            if (unlockData == null)
                return false;
            //if (IsLocked) return false;
            return jp.Equals(unlockData.GetJourneyPosition());
        }

        public void SetNew(bool b)
        {
            if (FORCE_ALL_UNLOCKED)
                return;
            unlockData.IsNew = b;
        }

        public bool HasUnlockData()
        {
            return unlockData != null;
        }

        public void SetLocked()
        {
            unlockData.IsLocked = true;
        }

        public void SetUnlocked()
        {
            unlockData.IsLocked = false;
        }

        #endregion

        public override string ToString()
        {
            return UniqueId;
        }

        #region Visuals

        public MaterialPair GetMaterialPair()
        {
            if (!(RewardBase is RewardProp))
                throw new Exception("Trying to get a material pair for a non-prop pack");

            RewardProp prop = RewardBase as RewardProp;
            RewardColor color = RewardColor;
            if (color == null || prop == null)
            {
                return new MaterialPair();
            }
            MaterialPair mp = new MaterialPair(color.Color1Name, prop.Material1, color.Color2Name, prop.Material2);
            return mp;
        }

        #endregion
    }
}
