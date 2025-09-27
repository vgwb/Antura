using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Antura.AnturaSpace.UI;
using Antura.Dog;
using DG.DeExtensions;
using UnityEngine;
using UnityEngine.UI;
using static Antura.AnturaSpace.UI.AnturaSpaceCategoryButton;

namespace Antura.Rewards
{
    public enum RewardUnlockMethod
    {
        BaseColorCombo,
        NewBase,
        NewColor,
        NewBaseAndAllColors
    }

    public class RewardSystemManager
    {
        private static bool VERBOSE = false;

        private const string ANTURA_REWARDS_PARTS_CONFIG_PATH = "Rewards/AnturaRewardsPartsConfig";

        #region Additional Pet Rewards

        /// <summary>
        /// The configuration of items that can be unlocked
        /// </summary>
        private Dictionary<AnturaPetType, RewardPartsConfig> petPartsConfig = new();

        #endregion

        #region Events

        public delegate void RewardSystemEventHandler(RewardPack rewardPack);
        //public static event RewardSystemEventHandler OnNewRewardUnlocked;

        #endregion

        #region Rewards Configuration

        public void Init()
        {
            LoadConfigs();
        }
        /// <summary>
        /// Loads the reward system configurations
        /// </summary>
        private void LoadConfigs()
        {
            foreach (var petType in (AnturaPetType[])Enum.GetValues(typeof(AnturaPetType)))
            {
                var partsConfigData = Resources.Load($"{petType}/{ANTURA_REWARDS_PARTS_CONFIG_PATH}") as TextAsset;
                var parsed = JsonUtility.FromJson<RewardPartsConfig>(partsConfigData.text);
                BuildAllPacks(petType, parsed);
                petPartsConfig[petType] = parsed;
            }
        }

        void BuildAllPacks(AnturaPetType petType, RewardPartsConfig partsConfig)
        {
            petRewardPacksDict[petType] = new Dictionary<RewardBaseType, List<RewardPack>>();
            petRewardPacksDict[petType][RewardBaseType.Prop] = BuildPacks(partsConfig, RewardBaseType.Prop);
            petRewardPacksDict[petType][RewardBaseType.Texture] = BuildPacks(partsConfig, RewardBaseType.Texture);
            petRewardPacksDict[petType][RewardBaseType.Decal] = BuildPacks(partsConfig, RewardBaseType.Decal);

            if (VERBOSE)
                Debug.Log("Total packs built: "
                    + "\n " + RewardBaseType.Prop + ": " + +petRewardPacksDict[petType][RewardBaseType.Prop].Count
                    + "\n " + RewardBaseType.Texture + ": " + +petRewardPacksDict[petType][RewardBaseType.Texture].Count
                    + "\n " + RewardBaseType.Decal + ": " + +petRewardPacksDict[petType][RewardBaseType.Decal].Count
                    );
        }

        private List<RewardPack> BuildPacks(RewardPartsConfig partsConfig, RewardBaseType baseType)
        {
            var bases = partsConfig.GetBasesForType(baseType);
            var colors = partsConfig.GetColorsForType(baseType);

            if (VERBOSE)
                Debug.Log("Building packs for " + baseType
                + "\n Bases: " + bases.Count() + " Colors: " + colors.Count());

            List<RewardPack> rewardPacks = new List<RewardPack>();
            foreach (var b in bases)
            {
                foreach (var c in colors)
                {
                    RewardPack pack = new RewardPack(baseType, b, c);
                    rewardPacks.Add(pack);
                }
            }
            return rewardPacks;
        }

        // This is needed to create packs to place on Antura without using the reward system
        // Used by the EAR_L logic
        public RewardPack BuildFakePack(string id, RewardColor color, RewardBase rewardBase, RewardBaseType baseType)
        {
            var pack = new RewardPack(baseType, rewardBase, color);
            return pack;
        }

        public IEnumerable<RewardBase> GetRewardBasesOfType(RewardBaseType baseType, AnturaPetType petType = AnturaPetType.Dog)
        {
            return petPartsConfig[petType].GetBasesForType(baseType);
        }

        #region Reward Packs

        private Dictionary<AnturaPetType, Dictionary<RewardBaseType, List<RewardPack>>> petRewardPacksDict = new();
        private Dictionary<RewardBaseType, List<RewardPack>> dogRewardPacksDict => petRewardPacksDict[AnturaPetType.Dog];

        public RewardPack GetRewardPackByUniqueIdAnyPet(string uniqueId)
        {
            foreach (var anturaPetType in (AnturaPetType[])Enum.GetValues(typeof(AnturaPetType)))
            {
                var item = GetRewardPacks(anturaPetType).FirstOrDefault(p => p.UniqueId == uniqueId);
                if (item != null)
                    return item;
            }
            return null;
        }

        private Dictionary<string, RewardPack> uniqueIdRewardPacksCache = new Dictionary<string, RewardPack>();
        public RewardPack GetRewardPackByUniqueId(string uniqueId, AnturaPetType petType)
        {
            var pet = petType;
            var key = $"{pet}_{uniqueId}";

            if (uniqueIdRewardPacksCache.TryGetValue(key, out var pack))
            {
                return pack;
            }

            var packs = GetRewardPacks(petType);
            foreach (var p in packs)
            {
                if (p.UniqueId.Equals(uniqueId, StringComparison.OrdinalIgnoreCase))
                {
                    uniqueIdRewardPacksCache[key] = p;
                    return p;
                }
            }
            return null;
        }

        public RewardPack GetRewardPackByPartsIds(string baseId, string colorId)
        {
            foreach (var p in GetRewardPacks(AppManager.I.Player.PetData.SelectedPet))
            {
                if (p.BaseId == baseId && p.ColorId == colorId)
                    return p;
            }
            return null;
        }

        public List<RewardPack> GetAllRewardPacksOfBaseType(RewardBaseType baseType, bool onePerBase = false, AnturaPetType petType = AnturaPetType.Dog)
        {
            if (!petRewardPacksDict[petType].ContainsKey(baseType))
            { throw new ArgumentNullException("Dict not initialised correctly!"); }
            var allRewardsOfBaseType = petRewardPacksDict[petType][baseType];
            return onePerBase ? FilterByOnePerBase(allRewardsOfBaseType, baseType) : allRewardsOfBaseType;
        }

        public List<RewardPack> GetUnlockedRewardPacksOfBaseType(RewardBaseType baseType, bool onePerBase = false)
        {
            if (!dogRewardPacksDict.ContainsKey(baseType))
            { throw new ArgumentNullException("Dict not initialised correctly!"); }
            var unlockedRewardsOfBaseType = dogRewardPacksDict[baseType].Where(x => x.IsUnlocked).ToList();
            return onePerBase ? FilterByOnePerBase(unlockedRewardsOfBaseType, baseType) : unlockedRewardsOfBaseType;
        }

        List<RewardPack> FilterByOnePerBase(List<RewardPack> inPacks, RewardBaseType baseType)
        {
            if (inPacks.Count == 0)
                return inPacks;
            List<RewardPack> basePacks = new List<RewardPack>();
            foreach (var rewardBase in GetRewardBasesOfType(baseType))
            {
                var firstBasePack = inPacks.FirstOrDefault(x => x.RewardBase == rewardBase);
                if (firstBasePack != null)
                    basePacks.Add(firstBasePack);
            }
            return basePacks;
        }

        public IEnumerable<RewardPack> GetRewardPacks(AnturaPetType petType = AnturaPetType.Dog)
        {
            foreach (var rewardPack in petRewardPacksDict[petType][RewardBaseType.Prop])
            {
                yield return rewardPack;
            }
            foreach (var rewardPack in petRewardPacksDict[petType][RewardBaseType.Decal])
            {
                yield return rewardPack;
            }
            foreach (var rewardPack in petRewardPacksDict[petType][RewardBaseType.Texture])
            {
                yield return rewardPack;
            }
        }
        public List<RewardPack> GetUnlockedRewardPacks(AnturaPetType petType = AnturaPetType.Dog)
        {
            var unlockedPacks = GetRewardPacks(petType).Where(p => p.IsUnlocked);
            return unlockedPacks.ToList();
        }
        public List<RewardPack> GetLockedRewardPacks()
        {
            var lockedPacks = GetRewardPacks().Where(p => p.IsLocked);
            return lockedPacks.ToList();
        }


        private IEnumerable<RewardPack> GetRewardPacksForBase(RewardBase _Base)
        {
            return GetRewardPacks().Where(x => x.RewardBase == _Base);
        }
        private IEnumerable<RewardPack> GetUnlockedRewardPacksForBase(RewardBase _Base)
        {
            return GetUnlockedRewardPacks().Where(x => x.RewardBase == _Base);
        }
        private IEnumerable<RewardPack> GetLockedRewardPacksForBase(RewardBase _Base)
        {
            return GetLockedRewardPacks().Where(x => x.RewardBase == _Base);
        }

        public List<RewardPack> GetLockedRewardPacksOfBaseType(RewardBaseType baseType)
        {
            var packsOfBase = dogRewardPacksDict[baseType];
            var lockedPacks = packsOfBase.Where(p => p.IsLocked);
            return lockedPacks.ToList();
        }

        List<RewardBase> GetLockedRewardBasesOfBaseType(RewardBaseType baseType)
        {
            var allBases = GetRewardBasesOfType(baseType);
            List<RewardBase> lockedBases = new List<RewardBase>();

            foreach (var rewardBase in allBases)
            {
                if (!IsRewardBaseUnlocked(rewardBase))
                {
                    lockedBases.Add(rewardBase);
                }
            }
            return lockedBases;
        }

        private List<RewardBase> GetUnlockedRewardBasesOfBaseType(RewardBaseType baseType)
        {
            var allBases = GetRewardBasesOfType(baseType, AnturaPetType.Dog);
            List<RewardBase> unlockedBases = new List<RewardBase>();

            foreach (var rewardBase in allBases)
            {
                if (IsRewardBaseUnlocked(rewardBase))
                {
                    unlockedBases.Add(rewardBase);
                }
            }

            return unlockedBases.ToList();
        }

        #endregion

        #endregion

        #region Rewards Unlocking

        #region Save / Load

        /// <summary>
        /// Called by the Player Profile to load the state of unlocked rewards.
        /// </summary>
        // TODO: let this call the PlayerProfile, and not vice-versa
        public void InjectRewardsUnlockData(List<RewardPackUnlockData> unlockDataList)
        {
            //Debug.Log("Loading unlock datas: " + unlockDataList.Count);

            // First reset all packs
            foreach (var pack in GetRewardPacks())
            {
                pack.SetUnlockData(null);
            }

            // Load the data in (for the Dog, first)
            foreach (var unlockData in unlockDataList)
            {
                var id = unlockData.Id;
                var pack = GetRewardPackByUniqueId(id, AnturaPetType.Dog);
                if (pack != null)
                {
                    pack.SetUnlockData(unlockData);
                }
            }

            // Unlock all packs of additional pets if they have a common SharedID with dog data
            if (AppManager.I.Player.PetData.SelectedPet == AnturaPetType.Dog)
            {
                foreach (var petType in petRewardPacksDict.Keys)
                {
                    if (petType == AnturaPetType.Dog)
                        continue;

                    // @note: may slow down a lot if the SharedID is not available
                    var rewards = petRewardPacksDict[petType];
                    foreach (var baseType in rewards.Keys)
                    {
                        foreach (var pack in rewards[baseType])
                        {
                            if (pack.RewardBase.SharedID.IsNullOrEmpty())
                                continue;
                            var bases = GetRewardBasesOfType(baseType, AnturaPetType.Dog);
                            var originalBase = bases.FirstOrDefault(b => string.Equals(b.SharedID, pack.RewardBase.SharedID, StringComparison.OrdinalIgnoreCase));
                            if (originalBase != null)
                            {
                                if (!pack.HasUnlockData())
                                {
                                    RegisterLockedPack(pack, JourneyPosition.InitialJourneyPosition);   // Fake journey position, just so we can unlock it
                                }
                                UnlockPack(pack);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called to save the state of unlocked rewards.
        /// </summary>
        public void SaveRewardsUnlockDataChanges()
        {
            AppManager.I.Player.SaveRewardPackUnlockDataList();
        }

        /// <summary>
        /// Called to reset the state of unlocked rewards.
        /// </summary>
        public void ResetRewardsUnlockData()
        {
            AppManager.I.Player.ResetRewardPackUnlockData();

            foreach (var pack in GetRewardPacks())
            {
                pack.SetUnlockData(null);
            }
        }

        #endregion

        #region Checks

        public bool IsThereSomeNewReward()
        {
            return GetUnlockedRewardPacks().Any(r => r.IsNew);
        }

        private bool IsRewardColorNew(RewardBase rewardBase, RewardColor rewardColor)
        {
            return GetUnlockedRewardPacks().Any(r => r.BaseId == rewardBase.ID && r.ColorId == rewardColor.ID && r.IsNew);
        }

        private bool IsRewardBaseNew(RewardBase rewardBase)
        {
            return GetUnlockedRewardPacks().Any(r => r.BaseId == rewardBase.ID && r.IsNew);
        }

        public bool IsRewardBaseUnlocked(RewardBase rewardBase, AnturaPetType petType = AnturaPetType.Dog)
        {
            return GetUnlockedRewardPacks(petType).Any(x => x.RewardBase == rewardBase);
        }

        public bool DoesRewardCategoryContainNewElements(RewardBaseType baseType, string _rewardCategory = "")
        {
            return GetUnlockedRewardPacks().Any(r => r.BaseType == baseType && r.Category == _rewardCategory && r.IsNew);
        }

        public bool DoesRewardCategoryContainUnlockedElements(RewardBaseType baseType, string _rewardCategory = "")
        {
            return GetUnlockedRewardPacks().Any(r => r.BaseType == baseType && r.Category == _rewardCategory);
        }

        /// <summary>
        /// Return true if all rewards for this JourneyPosition are already unlocked.
        /// </summary>
        /// <param name="_journeyPosition">The journey position.</param>
        public bool AreAllJourneyPositionRewardsAlreadyUnlocked(JourneyPosition journeyPosition)
        {
            int nAlreadyUnlocked = GetRewardPacksAlreadyUnlockedForJourneyPosition(journeyPosition).Count();
            int nForJourneyPosition = GetRewardPacksForJourneyPosition(journeyPosition).Count();
            //if (nForJourneyPosition == 0) throw new Exception("No rewards were added for JP " + journeyPosition);
            return nForJourneyPosition > 0 && nAlreadyUnlocked == nForJourneyPosition;
        }

        #endregion

        #region Getters

        /// <summary>
        /// Gets the total count of all reward packs.
        /// Any base with any color variation available in game.
        /// </summary>
        public int GetTotalRewardPacksCount(bool mergePropColors = false)
        {
            if (mergePropColors)
            {
                int tot = 0;
                tot += GetAllRewardPacksOfBaseType(RewardBaseType.Decal).Count;
                tot += GetAllRewardPacksOfBaseType(RewardBaseType.Texture).Count;
                tot += GetAllRewardPacksOfBaseType(RewardBaseType.Prop, true).Count;
                return tot;
            }
            else
            {
                return GetRewardPacks().Count();
            }
        }

        /// <summary>
        /// Gets the total count of all reward packs.
        /// Any base with any color variation available in game.
        /// </summary>
        public int GetUnlockedRewardPacksCount(bool mergePropColors = false)
        {
            if (mergePropColors)
            {
                int tot = 0;
                tot += GetUnlockedRewardPacksOfBaseType(RewardBaseType.Decal).Count;
                tot += GetUnlockedRewardPacksOfBaseType(RewardBaseType.Texture).Count;
                tot += GetUnlockedRewardPacksOfBaseType(RewardBaseType.Prop, true).Count;
                return tot;
            }
            else
            {
                return GetUnlockedRewardPacks().Count;
            }
        }

        private IEnumerable<RewardPack> GetRewardPacksAlreadyUnlockedForJourneyPosition(JourneyPosition journeyPosition)
        {
            return GetRewardPacks().Where(x => x.IsFoundAtJourneyPosition(journeyPosition) && x.IsUnlocked);
        }

        private List<RewardPack> GetRewardPacksForJourneyPosition(JourneyPosition journeyPosition)
        {
            if (rewardPacksCache.ContainsKey(journeyPosition))
            {
                return rewardPacksCache[journeyPosition];
            }

            var list = new List<RewardPack>();
            foreach (RewardPack x in GetRewardPacks())
            {
                if (x.IsFoundAtJourneyPosition(journeyPosition))
                {
                    list.Add(x);
                    //yield return x;
                }
            }
            rewardPacksCache[journeyPosition] = list;
            return list;
        }

        /// <summary>
        /// Gets the total number of rewards we need for the current journey
        /// </summary>
        public int CountTotalNeededRewards()
        {
            // Based on the journey...
            int neededRewards = 0;
            neededRewards += 3; // Initial rewards
            foreach (var jp in AppManager.I.JourneyHelper.GetAllJourneyPositions())
            {
                if (!jp.IsAssessment())
                    continue;    // Non-assessment do not gift a reward
                neededRewards++;
            }
            return neededRewards;
        }
        #endregion

        #region Pack Creation and Unlocking

        private void RegisterLockedPacks(List<RewardPack> packs, JourneyPosition jp, bool save = true)
        {
            // Packs are at first added and registered as Locked
            foreach (var pack in packs)
            {
                RegisterLockedPack(pack, jp);
            }
            if (save)
                SaveRewardsUnlockDataChanges();
        }

        private void RegisterLockedPack(RewardPack pack, JourneyPosition jp)
        {
            if (pack.HasUnlockData())
            {
                Debug.LogWarning($"Pack {pack} is already registered! Cannot register again");
                return;
            }

            // Add the unlock data and register it
            var unlockData = new RewardPackUnlockData(LogManager.I.AppSession, pack.UniqueId, jp)
            {
                IsLocked = true,
                IsNew = true,
                Edited = true
            };
            AppManager.I.Player.RegisterUnlockData(unlockData);
            pack.SetUnlockData(unlockData);

            if (VERBOSE)
                Debug.Log("Registered locked pack " + pack);
        }

        public void UnlockPacksSelection(List<RewardPack> packs, int nToUnlock, bool save = true)
        {
            if (packs.Count == 0)
                Debug.LogError("No packs to unlock!");
            var packsSelection = packs.RandomSelect(nToUnlock, true);
            UnlockPacks(packsSelection, save);
        }

        private void UnlockPacks(List<RewardPack> packs, bool save = true)
        {
            foreach (var pack in packs)
            {
                UnlockPack(pack);
            }
            if (save)
                SaveRewardsUnlockDataChanges();
        }

        public void UnlockPack(RewardPack pack)
        {
            pack.SetUnlocked();
            pack.SetNew(true);
            pack.SetEdited();
            if (VERBOSE)
                Debug.Log("Unlocked pack " + pack);
        }

        /// <summary>
        /// Unlocks ALL reward packs that have not been unlocked yet
        /// </summary>
        /// <param name="save"></param>
        public void UnlockAllMissingExtraPacks(bool save = true)
        {
            JourneyPosition extraRewardJP = new JourneyPosition(100, 100, 100);

            var packs = new List<RewardPack>();
            packs.AddRange(GetLockedRewardPacksOfBaseType(RewardBaseType.Prop));
            packs.AddRange(GetLockedRewardPacksOfBaseType(RewardBaseType.Decal));
            packs.AddRange(GetLockedRewardPacksOfBaseType(RewardBaseType.Texture));

            RegisterLockedPacks(packs, extraRewardJP, false);
            UnlockPacks(packs, save);
        }

        /// <summary>
        /// Unlocks all rewards in the game.
        /// </summary>
        public void UnlockAllPacks()
        {
            var allPlaySessionInfos = AppManager.I.ScoreHelper.GetAllPlaySessionInfo();
            for (int i = 0; i < allPlaySessionInfos.Count; i++)
            {
                var jp = AppManager.I.JourneyHelper.PlaySessionIdToJourneyPosition(allPlaySessionInfos[i].data.Id);
                UnlockAllRewardPacksForJourneyPosition(jp, false);
                //if (packs != null) Debug.LogFormat("Unlocked rewards for playsession {0} : {1}", jp, packs.Count);
            }

            //Debug.LogFormat("Unlocking also all extra rewards!");
            UnlockAllMissingExtraPacks(false);
            SaveRewardsUnlockDataChanges();
        }

        public List<RewardPack> UnlockAllRewardPacksForJourneyPosition(JourneyPosition journeyPosition, bool save = true)
        {
            var packs = GetOrGenerateAllRewardPacksForJourneyPosition(journeyPosition, save);

            if (AreAllJourneyPositionRewardsAlreadyUnlocked(journeyPosition))
            {
                Debug.LogError("We already unlocked all rewards for JP " + journeyPosition);
                return null;
            }

            UnlockPacks(packs, save);
            return packs;
        }

        #endregion

        #region Pack Generation

        private Dictionary<JourneyPosition, List<RewardPack>> rewardPacksCache = new Dictionary<JourneyPosition, List<RewardPack>>();

        /// <summary>
        /// Generates all Reward Packs for a given journey position
        /// </summary>
        /// <returns></returns>
        public List<RewardPack> GetOrGenerateAllRewardPacksForJourneyPosition(JourneyPosition journeyPosition, bool save = true)
        {
            // First check whether we already generated them
            var rewardPacks = GetRewardPacksForJourneyPosition(journeyPosition);
            if (rewardPacks.Any())
            {
                return rewardPacks;
            }

            // If not, we need to generate them from scratch
            var jpPacks = new List<RewardPack>();
            GeneratePacksFromUnlockFunction(journeyPosition, jpPacks);

            // We register the generated packs as locked
            RegisterLockedPacks(jpPacks, journeyPosition, save);

            return jpPacks;
        }

        private void GeneratePacksFromUnlockFunction(JourneyPosition journeyPosition, List<RewardPack> jpPacks)
        {
            // Non-assessment PS do not generate any reward
            if (!journeyPosition.IsAssessment())
                return;

            // Force to unlock a prop and all its colors at the first JP
            if (journeyPosition.Equals(new JourneyPosition(1, 1, 100)))
            {
                jpPacks.AddRange(GenerateNewRewardPacks(RewardBaseType.Prop, RewardUnlockMethod.NewBaseAndAllColors));
                return;
            }

            // Else, randomly choose between locked props, decals, or textures
            int nDecalsLeft = GetAllRewardPacksOfBaseType(RewardBaseType.Decal).Count(x => x.IsLocked);
            int nTexturesLeft = GetAllRewardPacksOfBaseType(RewardBaseType.Texture).Count(x => x.IsLocked);
            int nPropsLeft = GetAllRewardPacksOfBaseType(RewardBaseType.Prop, true).Count(x => x.IsLocked);

            //Debug.Log($"We have left: {nDecalsLeft} decals, {nTexturesLeft} textures, {nPropsLeft} props");

            List<RewardBaseType> choices = new List<RewardBaseType>();
            if (nDecalsLeft > 0)
                choices.Add(RewardBaseType.Decal);
            if (nTexturesLeft > 0)
                choices.Add(RewardBaseType.Texture);
            if (nPropsLeft > 0)
                choices.Add(RewardBaseType.Prop);

            if (choices.Count == 0)
            {
                // Nothing left...
                Debug.LogWarning($"Nothing left to unlock for JP {journeyPosition}");
                return;
            }

            RewardBaseType choice = choices.RandomSelectOne();

            if (choice == RewardBaseType.Prop)
            {
                jpPacks.AddRange(GenerateNewRewardPacks(choice, RewardUnlockMethod.NewBaseAndAllColors));
            }
            else
            {
                jpPacks.AddRange(GenerateNewRewardPacks(choice, RewardUnlockMethod.BaseColorCombo));
            }
        }

        #endregion

        /// <summary>
        /// Generate a new list of reward packs to be unlocked.
        /// </summary>
        /// <param name="baseType">Type of the reward.</param>
        private List<RewardPack> GenerateNewRewardPacks(RewardBaseType baseType, RewardUnlockMethod unlockMethod, string[] allowedCategories = null)
        {
            List<RewardPack> newRewardPacks = new List<RewardPack>();
            switch (unlockMethod)
            {
                case RewardUnlockMethod.NewBase:
                {
                    // We force a NEW base
                    var lockedBases = GetLockedRewardBasesOfBaseType(baseType);

                    if (allowedCategories != null)
                        lockedBases = lockedBases.Where(x => allowedCategories.Contains((x as RewardProp).Category)).ToList();

                    if (lockedBases.Count == 0)
                        throw new NullReferenceException(
                            "We do not have enough rewards to get a new base of type " + baseType);

                    var newBase = lockedBases.RandomSelectOne();
                    var lockedPacks = GetLockedRewardPacksOfBaseType(baseType);
                    var lockedPacksOfNewBase = lockedPacks.Where(x => x.BaseId == newBase.ID).ToList();

                    // We add one random pack of the new base
                    newRewardPacks.Add(lockedPacksOfNewBase.RandomSelectOne());
                }
                break;
                case RewardUnlockMethod.NewBaseAndAllColors:
                {
                    // We force a NEW base
                    //Debug.Log("Tot locked rewards count: " + GetAllLockedRewardPacks().Count());

                    var lockedBases = GetLockedRewardBasesOfBaseType(baseType);

                    // Debug.Log("locked bases count: " + lockedBases.Count);

                    if (allowedCategories != null)
                    {
                        Debug.Log("Allowed categories: " + allowedCategories.ToDebugString());
                        lockedBases = lockedBases.Where(x => allowedCategories.Contains((x as RewardProp).Category)).ToList();
                    }

                    if (lockedBases.Count == 0)
                        throw new NullReferenceException(
                            "We do not have enough rewards to get a new base of type " + baseType);

                    var newBase = lockedBases.RandomSelectOne();
                    var lockedPacks = GetLockedRewardPacksOfBaseType(baseType);
                    var lockedPacksOfNewBase = lockedPacks.Where(x => x.BaseId == newBase.ID).ToList();

                    // We add all locked packs of that base
                    newRewardPacks.AddRange(lockedPacksOfNewBase);
                }
                break;
                case RewardUnlockMethod.NewColor:
                {
                    // We force an OLD base
                    var unlockedBases = GetUnlockedRewardBasesOfBaseType(baseType);
                    var unlockedBasesWithColorsLeft = unlockedBases.Where(b => GetLockedRewardPacksOfBaseType(baseType).Count(p => p.BaseId == b.ID) > 0).ToList();

                    if (unlockedBasesWithColorsLeft.Count == 0)
                        throw new NullReferenceException(
                            "We do not have unlocked bases that still have colors to be unlocked for base type " + baseType);

                    var oldBase = unlockedBasesWithColorsLeft.RandomSelectOne();
                    var lockedPacks = GetLockedRewardPacksOfBaseType(baseType);
                    var lockedPacksOfOldBase = lockedPacks.Where(x => x.BaseId == oldBase.ID).ToList();
                    if (lockedPacksOfOldBase.Count == 0)
                        throw new NullReferenceException(
                            "We do not have enough rewards to get a new color for an old base of type " + baseType);

                    newRewardPacks.Add(lockedPacksOfOldBase.RandomSelectOne());
                }
                break;
                case RewardUnlockMethod.BaseColorCombo:
                {
                    // We get any reward pack
                    var lockedPacks = GetLockedRewardPacksOfBaseType(baseType);

                    if (lockedPacks.Count == 0)
                        throw new NullReferenceException(
                            "We do not have enough rewards left of type " + baseType);

                    newRewardPacks.Add(lockedPacks.RandomSelectOne());
                }
                break;
            }
            return newRewardPacks;
        }

        /// <summary>
        /// Unlocks the first set of rewards for current player.
        /// </summary>
        public void UnlockFirstSetOfRewards()
        {
            var _player = AppManager.I.Player;
            if (_player == null)
            {
                Debug.LogError("No current player available!");
                return;
            }

            var zeroJP = new JourneyPosition(0, 0, 0);

            if (AreAllJourneyPositionRewardsAlreadyUnlocked(zeroJP))
            {
                Debug.LogError("We already unlocked the first set of rewards!");
                return;
            }

            var propPacks = GenerateFirstRewards(RewardBaseType.Prop);          // 1 prop and colors
            var texturePacks = GenerateFirstRewards(RewardBaseType.Texture);    // 1 texture
            var decalPacks = GenerateFirstRewards(RewardBaseType.Decal);        // 1 decal

            List<RewardPack> packs = new List<RewardPack>();
            packs.AddRange(propPacks);
            packs.AddRange(texturePacks);
            packs.AddRange(decalPacks);

            RegisterLockedPacks(packs, zeroJP, false);
            UnlockPacks(packs, false);

            // Force as already seen (only for texture and decals)
            foreach (var pack in texturePacks)
                pack.SetNew(false);
            foreach (var pack in decalPacks)
                pack.SetNew(false);

            // force to to wear decal and texture
            _player.CurrentSingleAnturaCustomization.DecalPack = decalPacks[0];
            _player.CurrentSingleAnturaCustomization.DecalPackId = decalPacks[0].UniqueId;
            _player.CurrentSingleAnturaCustomization.TexturePack = texturePacks[0];
            _player.CurrentSingleAnturaCustomization.TexturePackId = texturePacks[0].UniqueId;
            _player.SaveAnturaCustomization();

            // Save initial packs and customization
            SaveRewardsUnlockDataChanges();

            Debug.Log("Unlocked first set of rewards!");
        }

        /// <summary>
        /// Gets the first RewardPacks that are unlocked when the game starts.
        /// </summary>
        /// <param name="baseType">Base type of the rewards to generate.</param>
        private List<RewardPack> GenerateFirstRewards(RewardBaseType baseType)
        {
            List<RewardPack> list = new List<RewardPack>();
            switch (baseType)
            {
                case RewardBaseType.Prop:
                    string[] allowedCategories = {
                        "HEAD", "NOSE", "BACK", "TAIL"
                    };
                    list = GenerateNewRewardPacks(baseType, RewardUnlockMethod.NewBaseAndAllColors, allowedCategories);
                    break;
                case RewardBaseType.Texture:
                    list.Add(GetRewardPackByPartsIds("Antura_wool_tilemat", "color1"));
                    break;
                case RewardBaseType.Decal:
                    list.Add(GetRewardPackByPartsIds("Antura_decalmap01", "color1"));
                    break;
            }
            return list;
        }

        #endregion

        #region Customization (i.e. UI, selection, view)

        /// <summary>
        /// Gets the reward base items (null if a base is not unlocked)
        /// </summary>
        /// <param name="baseType">Base type of the reward.</param>
        /// <param name="_parentsTransForModels">The parents transform for models.</param>
        /// <param name="_category">The category reward identifier.</param>
        public List<RewardBaseItem> GetRewardBaseItems(RewardBaseType baseType, List<Transform> _parentsTransForModels, string _category = "")
        {
            List<RewardBaseItem> returnList = new List<RewardBaseItem>();

            // Load the return list with an item for each base, or a NULL if no base has been unlocked
            var currentAnturaCustomizations = AppManager.I.Player.CurrentSingleAnturaCustomization;
            var rewardBases = GetRewardBasesOfType(baseType, currentAnturaCustomizations.PetType);

            if (baseType == RewardBaseType.Prop && _category != "")
                rewardBases = rewardBases.Where(rewardBase => (rewardBase as RewardProp).Category == _category).ToList();

            foreach (var rewardBase in rewardBases)
            {
                bool isToBeShown = IsRewardBaseUnlocked(rewardBase, currentAnturaCustomizations.PetType);
                if (AnturaSpaceUI.REWARDS_CAN_BE_BOUGHT)
                    isToBeShown = true;
                // Debug.Log("Reward prop base "  + rewardBase.ID + " to be shown? " + isToBeShown);

                if (isToBeShown)
                {
                    returnList.Add(new RewardBaseItem
                    {
                        data = rewardBase,
                        IsNew = IsRewardBaseNew(rewardBase),
                        IsSelected = currentAnturaCustomizations.HasBaseEquipped(rewardBase.ID)
                    });
                }
                else
                {
                    returnList.Add(null);
                }
            }


            // Load models and textures for the buttons
            switch (baseType)
            {

                case RewardBaseType.Prop:
                    for (int i = 0; i < returnList.Count; i++)
                    {
                        if (returnList[i] != null && i < _parentsTransForModels.Count)
                        {
                            var targetParentTr = _parentsTransForModels[i];
                            if (AnturaSpaceUI.MERGE_REMOVE_INTO_PROPS)
                                targetParentTr = _parentsTransForModels[i + 1];
                            ModelsManager.MountModel(AppManager.I.Player.PetData.SelectedPet, returnList[i].data.ID, targetParentTr, checkExisting: true);
                        }
                    }
                    break;

                case RewardBaseType.Texture:
                    for (int i = 0; i < returnList.Count; i++)
                    {
                        if (returnList[i] != null)
                        {
                            var targetParentTr = _parentsTransForModels[i];
                            string texturePath = $"{AppManager.I.Player.PetData.SelectedPet}/Textures_and_Materials/";
                            Texture2D inputTexture = Resources.Load<Texture2D>(texturePath + returnList[i].data.ID);
                            targetParentTr.GetComponent<RawImage>().texture = inputTexture;
                        }
                    }
                    break;

                case RewardBaseType.Decal:
                    for (int i = 0; i < returnList.Count; i++)
                    {
                        if (returnList[i] != null)
                        {
                            var targetParentTr = _parentsTransForModels[i];
                            string texturePath = $"{AppManager.I.Player.PetData.SelectedPet}/Textures_and_Materials/";
                            Texture2D inputTexture = Resources.Load<Texture2D>(texturePath + returnList[i].data.ID);
                            targetParentTr.GetComponent<RawImage>().texture = inputTexture;
                            //Debug.Log("Returned texture " + inputTexture.name + " for reward " + returnList[i].data.ID);
                        }
                    }
                    break;
                default:
                    Debug.LogWarningFormat("Reward base type requested {0} not found", baseType);
                    break;
            }

            return returnList;
        }

        /// <summary>
        /// Gets all the color items for a given base
        /// </summary>
        public List<RewardColorItem> GetRewardColorItemsForBase(RewardBase _Base)
        {
            List<RewardColorItem> returnList = new List<RewardColorItem>();

            // Load all colors for the given reward base
            var packsOfBase = GetRewardPacks(AppManager.I.Player.PetData.SelectedPet).Where(x => x.RewardBase == _Base);
            foreach (var pack in packsOfBase)
            {
                bool isToBeShown = AnturaSpaceUI.REWARDS_CAN_BE_BOUGHT || pack.IsUnlocked;
                if (isToBeShown)
                {
                    RewardColorItem rci = new RewardColorItem
                    {
                        data = pack.RewardColor,
                        IsNew = pack.IsNew
                    };

                    returnList.Add(rci);
                    //Debug.Log("Found color: " + pack.RewardColor.Color1RGB + " and " + pack.RewardColor.Color2RGB);
                }
                else
                {
                    returnList.Add(null);
                }
            }

            // Selection state
            RewardPack alreadyEquippedPack = AppManager.I.Player.CurrentSingleAnturaCustomization.GetEquippedPack(_Base.ID);
            if (alreadyEquippedPack != null)
            {
                // If we already equipped a pack of that base, we use the previous color
                returnList.Find(item => item != null && item.data == alreadyEquippedPack.RewardColor).IsSelected = true;
            }
            else
            {
                // Else, we select the first available color
                foreach (var firstItem in returnList)
                {
                    if (firstItem != null)
                    {
                        firstItem.IsSelected = true;
                        return returnList;
                    }
                }
            }

            return returnList;
        }


        public event RewardSystemEventHandler OnRewardSelectionChanged;

        /// <summary>
        /// Selects the actual reward to show on Antura
        /// </summary>
        public void SelectRewardColorItem(RewardBase _rewardBase, RewardColor _rewardColor)
        {
            PreviewReward(_rewardBase, _rewardColor);

            // Makes sure to set every pack with that color as seen
            foreach (var pack in GetUnlockedRewardPacksForBase(_rewardBase))
            {
                pack.SetNew(false);
                pack.SetEdited();
            }
            SaveRewardsUnlockDataChanges();
        }

        public void PreviewReward(RewardBase _rewardBase, RewardColor _rewardColor)
        {
            var currentSelectedReward = GetRewardPackByPartsIds(_rewardBase.ID, _rewardColor.ID);
            if (OnRewardSelectionChanged != null)
                OnRewardSelectionChanged(currentSelectedReward);
        }

        /// <summary>
        /// Gets the antura rotation angle view for reward category.
        /// </summary>
        /// <param name="_categoryId">The category identifier.</param>
        /// <returns></returns>
        public float GetAnturaRotationAngleViewForRewardCategory(string _categoryId, AnturaPetType _petType)
        {
            var id = Enum.Parse<AnturaSpaceCategory>(_categoryId);

            switch (_petType)
            {
                case AnturaPetType.Dog:

                    switch (id)
                    {
                        case AnturaSpaceCategory.HEAD:
                            return 40;
                        case AnturaSpaceCategory.NOSE:
                            return -20;
                        case AnturaSpaceCategory.BACK:
                            return 200;
                        case AnturaSpaceCategory.NECK:
                            return -80;
                        case AnturaSpaceCategory.JAW:
                            return 60;
                        case AnturaSpaceCategory.TAIL:
                            return 160;
                        case AnturaSpaceCategory.Ears:
                        case AnturaSpaceCategory.EAR_R:
                            return 10;
                        case AnturaSpaceCategory.EAR_L:
                            return 40;
                        default:
                            return 0;
                    }
                case AnturaPetType.Cat:

                    switch (id)
                    {
                        case AnturaSpaceCategory.HEAD:
                            return 40;
                        case AnturaSpaceCategory.NOSE:
                            return -20;
                        case AnturaSpaceCategory.BACK:
                            return 200;
                        case AnturaSpaceCategory.NECK:
                            return -80;
                        case AnturaSpaceCategory.JAW:
                            return 60;
                        case AnturaSpaceCategory.TAIL:
                            return 120;
                        case AnturaSpaceCategory.Ears:
                        case AnturaSpaceCategory.EAR_R:
                            return -40;
                        case AnturaSpaceCategory.EAR_L:
                            return 40;
                        default:
                            return 0;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(_petType), _petType, null);
            }
        }

        #endregion

    }

}
