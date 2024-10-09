using Antura.Core;
using Antura.Database;
using Antura.Dog;
using System;
using System.Collections.Generic;
using System.Linq;
using DG.DeExtensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Profile
{
    /// <summary>
    /// A Player Profile contains persistent data on details and on the progression status of a single player.
    /// </summary>
    [Serializable]
    public class PlayerProfile
    {
        public string Uuid;
        public int AvatarId;
        public PlayerGender Gender;
        public PlayerTint Tint; // Kept for backwards compatibility
        public int Age;
        public Color SkinColor;
        public Color HairColor;
        public Color BgColor;
        public bool IsDemoUser;
        public bool HasFinishedTheGame; // TODO: Move out (now tied to the content)
        public bool HasFinishedTheGameWithAllStars; // TODO: Move out (now tied to the content)
        public bool HasMaxStarsInCurrentPlaySessions; // TODO: Move out (now tied to the content)
        public int TotalNumberOfBones;
        public int ConsecutivePlayDays;
        public AppEditionID editionID;
        public LearningContentID ContentID; // @note: This will be updated with the selected content, so we know what to load / save
        public string AppVersion;
        public PetData PetData = new PetData();
        public ProfileCompletionState ProfileCompletion = ProfileCompletionState.New;   // TODO: Move out (now tied to the content)

        // @note: Now part of the Content Profile
        private JourneyPosition maxJourneyPosition = JourneyPosition.InitialJourneyPosition;
        public JourneyPosition MaxJourneyPosition
        {
            get
            {
                if (AppManager.I.NavigationManager.NavData.CurrentContent != null)
                {
                    maxJourneyPosition = AppManager.I.NavigationManager.NavData.CurrentContent.MaxJourneyPosition;
                }
                return maxJourneyPosition;
            }
            private set
            {
                maxJourneyPosition = value;
                if (AppManager.I.NavigationManager.NavData.CurrentContent == null)
                {
                    return;
                }
                AppManager.I.NavigationManager.NavData.CurrentContent.MaxJourneyPosition = value;
            }
        }

        // @note: Now part of the Content Profile
        private JourneyPosition currentJourneyPosition = JourneyPosition.InitialJourneyPosition;
        public JourneyPosition CurrentJourneyPosition
        {
            get
            {
                if (AppManager.I.NavigationManager.NavData.CurrentContent != null)
                {
                    currentJourneyPosition = AppManager.I.NavigationManager.NavData.CurrentContent.CurrentJourneyPosition;
                }
                return currentJourneyPosition;
            }
            private set
            {
                currentJourneyPosition = value;
                if (AppManager.I.NavigationManager.NavData.CurrentContent == null)
                {
                    return;
                }
                AppManager.I.NavigationManager.NavData.CurrentContent.CurrentJourneyPosition = value;
            }
        }

        // @note: Now part of the Content Profile
        private JourneyPosition previousJourneyPosition = JourneyPosition.InitialJourneyPosition;
        public JourneyPosition PreviousJourneyPosition
        {
            get
            {
                if (AppManager.I.NavigationManager.NavData.CurrentContent != null)
                {
                    previousJourneyPosition = AppManager.I.NavigationManager.NavData.CurrentContent.PreviousJourneyPosition;
                }
                return previousJourneyPosition;
            }
            private set
            {
                previousJourneyPosition = value;
                if (AppManager.I.NavigationManager.NavData.CurrentContent == null)
                {
                    return;
                }
                AppManager.I.NavigationManager.NavData.CurrentContent.PreviousJourneyPosition = value;
            }
        }

        #region First Contact State

        public FirstContactState FirstContactState;

        #endregion

        #region Bones

        // Initial number of bones when the game starts
        private const int INITIAL_BONES = 0;

        public void GiftInitialBones()
        {
            TotalNumberOfBones = INITIAL_BONES;
        }

        public void MakeSureHasEnoughBones(int wantedBones)
        {
            if (TotalNumberOfBones < wantedBones)
            {
                TotalNumberOfBones = wantedBones;
                Save();
            }
        }

        public int GetTotalNumberOfBones()
        {
            return TotalNumberOfBones;
        }

        public int AddBones(int _bonesToAdd)
        {
            TotalNumberOfBones += _bonesToAdd;
            Save();
            return TotalNumberOfBones;
        }

        public int RemoveBones(int _bonesToRemove)
        {
            TotalNumberOfBones -= _bonesToRemove;
            Save();
            return TotalNumberOfBones;
        }

        #endregion

        #region Management

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            if (AppManager.PROFILE_INVERSION)
            {
                if (AppManager.I.NavigationManager.NavData.CurrentContent != null)
                {
                    if (AppManager.VERBOSE_INVERSION) Debug.LogError($"[Inversion] Saving current data (profile and content for {AppManager.I.NavigationManager.NavData.CurrentContent.ContentID})");
                }
                else
                {
                    if (AppManager.VERBOSE_INVERSION) Debug.LogError("[Inversion] Saving current data (profile only)");
                }
            }

            AppManager.I.PlayerProfileManager.UpdateCurrentPlayerIconDataInSettings();
            AppManager.I.PlayerProfileManager.SavePlayerProfile(this);
            if (AppManager.I.NavigationManager.NavData.CurrentContent != null)
            {
                AppManager.I.PlayerProfileManager.SaveContentProfile(AppManager.I.NavigationManager.NavData.CurrentContent);
            }
        }

        #endregion

        #region Journey position

        /// <summary>
        /// Sets the actual journey position and save to profile.
        /// @note: check valid data before insert.
        /// </summary>
        /// <param name="_stage">The stage.</param>
        /// <param name="_lb">The lb.</param>
        /// <param name="_ps">The ps.</param>
        /// <param name="_save">if set to <c>true</c> [save] profile at the end.</param>
        public void SetCurrentJourneyPosition(int _stage, int _lb, int _ps, bool _save = true)
        {
            SetCurrentJourneyPosition(new JourneyPosition(_stage, _lb, _ps));
            if (_save)
            {
                Save();
            }
        }

        /// <summary>
        /// Sets the actual journey position and save to profile.
        /// @note: check valid data before insert.
        /// </summary>
        /// <param name="_journeyPosition">The journey position.</param>
        /// <param name="_save">if set to <c>true</c> [save] profile at the end.</param>
        public void SetCurrentJourneyPosition(JourneyPosition _journeyPosition, bool _save = true, bool _updatePrevToo = true)
        {
            CurrentJourneyPosition = _journeyPosition;
            if (_updatePrevToo)
            {
                UpdatePreviousJourneyPosition();
            }
            if (_save)
            {
                Save();
            }
        }

        /// <summary>
        /// Advance the Max journey position based on the next after the Current one.
        /// </summary>
        public void AdvanceMaxJourneyPosition()
        {
            UpdatePreviousJourneyPosition();
            JourneyPosition p = AppManager.I.JourneyHelper.FindNextJourneyPosition(CurrentJourneyPosition);
            SetMaxJourneyPosition(p);

            AppManager.I.Player.CheckStarsState();
        }

        /// <summary>
        /// Sets the maximum journey position and save to profile.
        /// @note: check valid data before insert.
        /// </summary>
        /// <param name="newJourneyPosition">The journey position.</param>
        /// <param name="_save">if set to <c>true</c> [save] profile at the end.</param>
        public void SetMaxJourneyPosition(JourneyPosition newJourneyPosition, bool _save = true, bool _forced = false)
        {
            if (MaxJourneyPosition.IsMinor(newJourneyPosition) || _forced)
            {
                MaxJourneyPosition = new JourneyPosition(newJourneyPosition.Stage, newJourneyPosition.LearningBlock,
                    newJourneyPosition.PlaySession);
                CurrentJourneyPosition = new JourneyPosition(newJourneyPosition.Stage, newJourneyPosition.LearningBlock,
                    newJourneyPosition.PlaySession);

                if (!_forced)
                {
                    AppManager.I.Services.Analytics.TrackReachedJourneyPosition(MaxJourneyPosition);
                }

                if (_save)
                {
                    Save();
                }
            }
        }


        /// <summary>
        /// Check whether the game has finished and update the player icon.
        /// Called only when we actually finish the game.
        /// </summary>
        public void CheckGameFinished()
        {
            if (!HasFinishedTheGame)
            {
                HasFinishedTheGame = AppManager.I.JourneyHelper.HasFinishedTheGame();
                if (HasFinishedTheGame)
                {
                    Save();
                }
            }
        }


        /// <summary>
        /// Check whether the game has finished with all stars and update the player icon.
        /// Called at each end of play session.
        /// </summary>
        public void CheckStarsState()
        {
            if (HasFinishedTheGame && !HasFinishedTheGameWithAllStars)
            {
                HasFinishedTheGameWithAllStars = AppManager.I.ScoreHelper.HasFinishedTheGameWithAllStars();
                if (HasFinishedTheGameWithAllStars)
                {
                    Save();
                }
            }

            HasMaxStarsInCurrentPlaySessions = AppManager.I.ScoreHelper.HasEarnedMaxStarsInCurrentPlaySessions();
            Save();
        }

        /// <summary>
        /// Resets the maximum journey position to 1,1,1.
        /// </summary>
        public void ResetMaxJourneyPosition(bool _save = true)
        {
            MaxJourneyPosition = new JourneyPosition(JourneyPosition.InitialJourneyPosition);
            CurrentJourneyPosition = new JourneyPosition(JourneyPosition.InitialJourneyPosition);
            UpdatePreviousJourneyPosition();
            if (_save)
            {
                Save();
            }
        }

        /// <summary>
        /// checks if we are at the max joiurney position
        /// </summary>
        /// <returns></returns>
        public bool IsAtMaxJourneyPosition()
        {
            return (CurrentJourneyPosition.Stage == MaxJourneyPosition.Stage) &&
            (CurrentJourneyPosition.LearningBlock == MaxJourneyPosition.LearningBlock) &&
            (CurrentJourneyPosition.PlaySession == MaxJourneyPosition.PlaySession);
        }

        public void AdvanceCurrentStage()
        {
            CurrentJourneyPosition.Stage++;
        }

        public void RetractCurrentStage()
        {
            CurrentJourneyPosition.Stage--;
        }

        public void UpdatePreviousJourneyPosition()
        {
            PreviousJourneyPosition = new JourneyPosition(CurrentJourneyPosition);
            //Debug.LogError("Updating Prev to " + PreviousJourneyPosition);
        }

        public void ForcePreviousJourneyPosition(JourneyPosition journeyPosition)
        {
            PreviousJourneyPosition = journeyPosition;
        }

        #endregion

        #region Shop State

        public AnturaSpace.ShopState CurrentShopState = new AnturaSpace.ShopState();

        #endregion

        #region Antura Customization and Rewards

        public CustomizationShopState CustomizationShopState = new CustomizationShopState();

        private AllPetsAnturaCustomization _currentAllPetsAnturaCustomization = new();

        public void MigrateOldCustomization(AnturaCustomization dogCustomization)
        {
            _currentAllPetsAnturaCustomization = new AllPetsAnturaCustomization();
            if (dogCustomization != null) _currentAllPetsAnturaCustomization.Append(dogCustomization);
            jsonAnturaCustomizationData = _currentAllPetsAnturaCustomization.GetJsonListOfIds();
            Save();
        }

        public AnturaCustomization AnturaCustomization(AnturaPetType petType)
        {
            if (_currentAllPetsAnturaCustomization.Customizations == null) _currentAllPetsAnturaCustomization.Customizations = Array.Empty<AnturaCustomization>();
            var customization = _currentAllPetsAnturaCustomization.Customizations.FirstOrDefault(x => x.PetType == petType);
            if (customization == null)
            {
                customization = new AnturaCustomization();
                customization.PetType = petType;
                customization.LoadFromListOfIds(jsonAnturaCustomizationData);
                _currentAllPetsAnturaCustomization.Append(customization);
            }
            return customization;
        }

        /// <summary>
        /// The current antura customizations
        /// </summary>
        public AnturaCustomization CurrentSingleAnturaCustomization
        {
            get => AnturaCustomization(PetData.SelectedPet);
            set
            {
                if (_currentAllPetsAnturaCustomization.Customizations == null) _currentAllPetsAnturaCustomization.Customizations = Array.Empty<AnturaCustomization>();
                var list = _currentAllPetsAnturaCustomization.Customizations.ToList();
                list.RemoveAll(x => x.PetType == PetData.SelectedPet);
                _currentAllPetsAnturaCustomization.Customizations = list.ToArray();
                _currentAllPetsAnturaCustomization.Append(value);
                SaveAnturaCustomization();
            }
        }

        #region Data for unlocked rewards

        private List<RewardPackUnlockData> _rewardPackUnlockDataList = new List<RewardPackUnlockData>();

        /// <summary>
        /// Resets the rewards unlocked data.
        /// </summary>
        public void ResetRewardPackUnlockData()
        {
            _rewardPackUnlockDataList.Clear();
            AppManager.I.DB.DeleteAll<RewardPackUnlockData>();
        }

        /// <summary>
        /// Loads the rewards unlocked from database.
        /// </summary>
        /// <returns></returns>
        public void LoadRewardPackUnlockDataList()
        {
            _rewardPackUnlockDataList = AppManager.I.DB.GetAllRewardPackUnlockData();
            AppManager.I.RewardSystemManager.InjectRewardsUnlockData(_rewardPackUnlockDataList);
        }

        public void SaveRewardPackUnlockDataList()
        {
            var listToUpdate = _rewardPackUnlockDataList.Where(x => x.Edited).ToList();
            AppManager.I.DB.UpdateRewardPackUnlockDataAll(listToUpdate);
            _rewardPackUnlockDataList.ForEach(x => x.Edited = false);
        }

        public void RegisterUnlockData(RewardPackUnlockData unlockData)
        {
            if (!_rewardPackUnlockDataList.Contains(unlockData))
                _rewardPackUnlockDataList.Add(unlockData);
        }

        /// <summary>
        /// Gets the not yet unlocked rewards list.
        /// </summary>
        /// <param name="_rewardType">Type of the reward.</param>
        /// <returns></returns>
        /*public int GetNotYetUnlockedRewardCountForType(RewardTypes _rewardType)
        {
            int counter = 0;
            //foreach (PlaySessionRewardUnlock plsRew in RewardSystemManager.GetConfig().PlaySessionRewardsUnlock) {
            //    // Check if PlaySessionRewardUnlock contain requested type.
            //    switch (_rewardType) {
            //        case RewardTypes.reward:
            //            if (plsRew.Reward == "")
            //                continue;
            //            break;
            //        case RewardTypes.texture:
            //            if (plsRew.Texture == "")
            //                continue;
            //            break;
            //        case RewardTypes.decal:
            //            if (plsRew.Decal == "")
            //                continue;
            //            break;
            //        default:
            //            continue;
            //            break;
            //    }

            //    RewardPackUnlockData unlockedRewardData = RewardsUnlocked.Find(r => r.Type == _rewardType && r.JourneyPosition == plsRew.PlaySession);
            //    if (unlockedRewardData == null)
            //        counter++;
            //}
            switch (_rewardType) {
                case RewardTypes.reward:
                    counter = AppManager.I.RewardSystemManager.ItemsConfig.PropBases.Count - RewardsUnlocked.FindAll(r => r.Type == _rewardType).Count;
                    break;
                case RewardTypes.texture:
                    counter = AppManager.I.RewardSystemManager.ItemsConfig.TextureBases.Count - RewardsUnlocked.FindAll(r => r.Type == _rewardType).Count;
                    break;
                case RewardTypes.decal:
                    counter = AppManager.I.RewardSystemManager.ItemsConfig.DecalBases.Count -
                    RewardsUnlocked.FindAll(r => r.Type == _rewardType).Count;
                    break;
            }

            return counter;
        }*/

        /// <summary>
        /// Return true if rewards for this type available.
        /// </summary>
        /// <param name="_rewardType">Type of the reward.</param>
        /// <returns></returns>
        /*public bool RewardForTypeAvailableYet(RewardTypes _rewardType)
        {
            return GetNotYetUnlockedRewardCountForType(_rewardType) <= 0 ? false : true;
        }*/

        /// <summary>
        /// Used to store antura customization data in json and load it at runtime.
        /// </summary>
        public string jsonAnturaCustomizationData = string.Empty;

        #endregion

        /*/// <summary>
        /// Mark RewardPackUnlockData as not new and update db entry.
        /// </summary>
        public void SetRewardPackUnlockedToNotNew(string _rewardPackId)
        {
            RewardPackUnlockData rewardPackToUpdate = UnlockedRewardsData.Find(r => r.Id == _rewardPackId && r.IsNew == true);
            if (rewardPackToUpdate != null) {
                rewardPackToUpdate.IsNew = false;
            }
            AppManager.I.DB.UpdateRewardPackUnlockData(rewardPackToUpdate);
        }*/

        /* /// <summary>
         /// Delete all reward unlocks from the Dynamic DB.
         /// </summary>
         private void DeleteAllRewardUnlocks()
         {
             AppManager.I.DB.DeleteAll<RewardPackUnlockData>();
         }

         /// <summary>
         /// Adds or update the reward unlocked and persist it.
         /// </summary>
         /// <param name="rewardPackUnlockData">The reward pack.</param>
         public void AddRewardUnlocked(RewardPackUnlockData rewardPackUnlockData)
         {
             // DEPRECATE THIS!!!
             AppManager.I.Player.UnlockedRewardsData.Add(rewardPackUnlockData);
             AppManager.I.DB.UpdateRewardPackUnlockData(rewardPackUnlockData);
         }

        /// <summary>
        /// Add update to db all 'this' reward unlocked.
        /// </summary>
        public void AddRewardUnlockedAll(RewardPackUnlockData _rewardPackUnlockData)
        {
            List<RewardPackUnlockData> rewards = new List<RewardPackUnlockData>();
            rewards.Add(_rewardPackUnlockData);
            AppManager.I.DB.UpdateRewardPackUnlockDataAll(rewards);
        }*/

        /// <summary>
        /// Adds or update a list of unlocked rewards and persist it.
        /// </summary>
        public void AddRewardUnlockedRange()//List<RewardPackUnlockData> rewardPackUnlockDatas)
        {
            //Debug.Log(this.RewardsUnlocked);
            //AppManager.I.Player.UnlockedRewardsData.AddRange(rewardPackUnlockDatas);
            AppManager.I.DB.UpdateRewardPackUnlockDataAll(_rewardPackUnlockDataList);
        }

        /// <summary>
        /// Saves the customization on db.
        /// </summary>
        /// <param name="_anturaCustomization">The antura customization. If null save only on db.</param>
        public void SaveAnturaCustomization(AnturaCustomization _anturaCustomization = null)
        {
            if (_anturaCustomization != null)
            {
                CurrentSingleAnturaCustomization = _anturaCustomization;
            }
            jsonAnturaCustomizationData = _currentAllPetsAnturaCustomization.GetJsonListOfIds();
            //Debug.LogError("SAVED CUSTOMIZATION " + jsonAnturaCustomizationData);
            Save();

            AppManager.I.LogManager.LogInfo(InfoEvent.AnturaCustomization, jsonAnturaCustomizationData);
        }

        #endregion

        #region Profile completion

        /// <summary>
        /// Resets the player profile completion.
        /// </summary>
        public void ResetPlayerProfileCompletion()
        {
            ProfileCompletion = ProfileCompletionState.New;
            Save();
        }

        #region GameEnded

        public bool IsGameCompleted()
        {
            if (ProfileCompletion < ProfileCompletionState.GameCompleted)
                return false;
            return true;
        }

        public void SetGameCompleted()
        {
            ProfileCompletion = ProfileCompletionState.GameCompleted;
            CheckGameFinished();
            Save();
        }

        public bool HasFinalBeenShown()
        {
            return ProfileCompletion >= ProfileCompletionState.GameCompletedAndFinalShown;
        }

        public void SetFinalShown(bool isInitialising = false)
        {
            if (!isInitialising)
                AppManager.I.RewardSystemManager.UnlockAllMissingExtraPacks();
            ProfileCompletion = ProfileCompletionState.GameCompletedAndFinalShown;
        }

        #endregion

        #endregion

        #region Input

        /// <summary>
        /// Charge this with PlayerProfileData.
        /// </summary>
        public PlayerProfile FromData(PlayerProfileData _data)
        {
            Uuid = _data.Uuid;

            AvatarId = _data.AvatarId;
            Tint = _data.Tint;
            Age = _data.Age;
            AppVersion = _data.AppVersion;
            editionID = _data.EditionID;
            ContentID = _data.ContentID;
            Gender = _data.Gender;
            SkinColor = string.IsNullOrEmpty(_data.SkinColor) ? Color.white : _data.SkinColor.HexToColor();
            HairColor = string.IsNullOrEmpty(_data.HairColor) ? Color.white : _data.HairColor.HexToColor();
            BgColor = string.IsNullOrEmpty(_data.BgColor) ? Color.white : _data.BgColor.HexToColor();
            IsDemoUser = _data.IsDemoUser;
            HasFinishedTheGame = _data.JourneyCompleted;
            HasFinishedTheGameWithAllStars = _data.HasFinishedTheGameWithAllStars();
            ProfileCompletion = _data.ProfileCompletion;
            TotalNumberOfBones = _data.TotalBones;

            HasMaxStarsInCurrentPlaySessions = _data.GetAdditionalData().HasMaxStarsInCurrentPlaySessions;
            ConsecutivePlayDays = _data.GetAdditionalData().ConsecutivePlayDays;
            CurrentShopState = AnturaSpace.ShopState.CreateFromJson(_data.GetAdditionalData().CurrentShopStateJSON);
            FirstContactState = JsonUtility.FromJson<FirstContactState>(_data.FirstContactStateJSON);
            CustomizationShopState = CustomizationShopState.CreateFromJson(_data.GetAdditionalData().CurrentCustomizationShopStateJSON);

            PetData.SelectedPet = _data.SelectedPet;
            PetData.CatUnlocked = _data.CatUnlocked;

            // DEPRECATED - SetCurrentJourneyPosition(_data.GetCurrentJourneyPosition(), false);
            // DEPRECATED - SetMaxJourneyPosition(_data.GetMaxJourneyPosition(), false);
            // Antura customization save only customization data
            jsonAnturaCustomizationData = _data.CurrentAnturaCustomization;

            return this;
        }

        #endregion

        #region Output

        /// <summary>
        /// Converts this instance to PlayerProfileData.
        /// </summary>
        /// <returns></returns>
        public PlayerProfileData ToData()
        {
            PlayerProfileData newProfileData = new PlayerProfileData(
                    Uuid, AvatarId, Gender, Tint, SkinColor, HairColor, BgColor, Age, IsDemoUser, HasFinishedTheGame, HasFinishedTheGameWithAllStars, HasMaxStarsInCurrentPlaySessions,
                    TotalNumberOfBones, ProfileCompletion, _currentAllPetsAnturaCustomization.GetJsonListOfIds(), ConsecutivePlayDays, CurrentShopState, CustomizationShopState,
                    FirstContactState, editionID, ContentID, AppVersion, PetData
            );
            newProfileData.SetCurrentJourneyPosition(CurrentJourneyPosition);
            newProfileData.SetMaxJourneyPosition(MaxJourneyPosition);
            return newProfileData;
        }

        public PlayerIconData GetPlayerIconData()
        {
            PlayerIconData returnIconData = new PlayerIconData
            {
                Uuid = this.Uuid,
                AvatarId = this.AvatarId,
                Gender = this.Gender,
                Tint = this.Tint,
                SkinColor = this.SkinColor,
                HairColor = this.HairColor,
                BgColor = this.BgColor,
                IsDemoUser = this.IsDemoUser,
                HasFinishedTheGame = this.HasFinishedTheGame,
                HasFinishedTheGameWithAllStars = this.HasFinishedTheGameWithAllStars,
                HasMaxStarsInCurrentPlaySessions = this.HasMaxStarsInCurrentPlaySessions,
                MaxJourneyPosition = this.MaxJourneyPosition,
                editionID = this.editionID,
                contentID = this.ContentID,
                AppVersion = this.AppVersion,
                PetData = this.PetData
            };
            return returnIconData;
        }

        public string ToJsonData()
        {
            return JsonUtility.ToJson(this);
        }

        public override string ToString()
        {
            return string.Format("ID{0}, MaxJ({1}.{2}.{3}), CurrentJ({4}.{5}.{6}), ProfCompl{7}",
                Uuid,
                MaxJourneyPosition.Stage,
                MaxJourneyPosition.LearningBlock,
                MaxJourneyPosition.PlaySession,

                CurrentJourneyPosition.Stage,
                CurrentJourneyPosition.LearningBlock,
                CurrentJourneyPosition.PlaySession,
                ProfileCompletion
            );
        }

        #endregion

    }
}
