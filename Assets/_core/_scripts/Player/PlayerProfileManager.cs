using Antura.Core;
using Antura.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Antura.Profile
{
    /// <summary>
    /// Handles the creation, selection, and deletion of player profiles.
    /// </summary>
    public class PlayerProfileManager
    {
        public PlayerGender TemporaryPlayerGender = PlayerGender.M;
        public const int NEW_AVATAR_ID_START = 100;  // The actual ID for new avatar starts from this value

        #region Current Player

        private PlayerProfile _currentPlayer;

        /// <summary>
        /// The player that is currently playing.
        /// </summary>
        public PlayerProfile CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                if (_currentPlayer != value)
                {
                    AppManager.I.Player = _currentPlayer = value;
                    //AppManager.I.Player.jsonAnturaCustomizationData = "{\"PetType\":0,\"PropPacksIds\":[],\"TexturePackId\":\"Antura_flower_tilemat_color1\",\"DecalPackId\":\"Antura_decalmap01_color1\"}";

                    if (_currentPlayer != null)
                    {
                        AppManager.I.Teacher.SetPlayerProfile(value);
                        // TODO refactor: make this part more clear, better create a SetCurrentPlayer() method for this!
                        if (AppManager.I.DB.HasLoadedPlayerProfile())
                        {
                            LogManager.I.LogInfo(InfoEvent.AppSessionEnd, "{\"AppSession\":\"" + LogManager.I.AppSession + "\"}");
                        }
                        AppManager.I.AppSettings.LastActivePlayerUUID = value.Uuid;
                        AppManager.I.AppSettingsManager.SaveSettings();
                        LogManager.I.LogInfo(InfoEvent.AppSessionStart, "{\"AppSession\":\"" + LogManager.I.AppSession + "\"}");
                        AppManager.I.NavigationManager.InitPlayerNavigationData(_currentPlayer);

                        AppManager.I.FirstContactManager.InitialiseForCurrentPlayer(_currentPlayer.FirstContactState);

                        _currentPlayer.LoadRewardPackUnlockDataList(); // refresh list of unlocked rewards
                        _currentPlayer.SetCurrentJourneyPosition(_currentPlayer.MaxJourneyPosition, _save: false);
                        if (OnProfileChanged != null)
                        {
                            OnProfileChanged();
                        }
                    }

                }
                _currentPlayer = value;
            }
        }

        #endregion

        #region Player UUID

        /// <summary>
        /// Sets the player as current player profile loading from db by UUID.
        /// </summary>
        /// <param name="playerUUID">The player UUID.</param>
        /// <returns></returns>
        public PlayerProfile SetPlayerAsCurrentByUUID(string playerUUID, bool hasUpgraded = false)
        {
            PlayerProfile returnProfile = GetPlayerProfileByUUID(playerUUID);
            if (hasUpgraded)
            {
                returnProfile.ContentID = LearningContentID.LearnToRead_Arabic;
                returnProfile.editionID = AppEditionID.Multi;
                SavePlayerProfile(returnProfile);
            }
            AppManager.I.PlayerProfileManager.CurrentPlayer = returnProfile;
            UpdateProfileToCurrentVersion();
            return returnProfile;
        }

        public void UpdateProfileToCurrentVersion()
        {
            // Check max position
            var currentPlayer = AppManager.I.PlayerProfileManager.CurrentPlayer;
            if (!AppManager.I.JourneyHelper.SupportsJourneyPosition(currentPlayer.MaxJourneyPosition))
            {
                var newMax = AppManager.I.JourneyHelper.FindExistingJourneyPositionBackwards(currentPlayer.MaxJourneyPosition);
                currentPlayer.SetMaxJourneyPosition(newMax, _forced: true);
            }

            // Check whether we should unlock the next max position
            // Can happen if we were at the end of the game, but we have new journey positions in the new journey
            var scoreAtMaxJP = AppManager.I.ScoreHelper.GetCurrentScoreForJourneyPosition(currentPlayer.MaxJourneyPosition);
            var nextJP = AppManager.I.JourneyHelper.FindNextJourneyPosition(currentPlayer.MaxJourneyPosition);
            if (scoreAtMaxJP > 0 && nextJP != null)
            {
                Debug.Log($"Updating max journey position to {nextJP} as we completed the previous one.");
                currentPlayer.SetMaxJourneyPosition(nextJP, _forced: true);
            }

            // Check current position
            if (!AppManager.I.JourneyHelper.SupportsJourneyPosition(currentPlayer.CurrentJourneyPosition))
            {
                var newCurrent = AppManager.I.JourneyHelper.FindExistingJourneyPositionBackwards(currentPlayer.CurrentJourneyPosition);
                currentPlayer.SetCurrentJourneyPosition(newCurrent);
            }
        }

        /// <summary>
        /// Gets the player profile from db by UUID.
        /// </summary>
        /// <param name="playerUUID">The player UUID.</param>
        /// <returns></returns>
        public PlayerProfile GetPlayerProfileByUUID(string playerUUID)
        {
            PlayerProfileData playerProfileDataFromDB = AppManager.I.DB.LoadDatabaseForPlayer(playerUUID);

            // If null, the player does not exist.
            // The DB got desynced. Remove this player!
            if (playerProfileDataFromDB == null)
            {
                Debug.LogError("ERROR: no profile data for player UUID " + playerUUID);
            }

            return new PlayerProfile().FromData(playerProfileDataFromDB);
        }

        #endregion

        #region Settings

        /// <summary>
        /// Reloads all the settings and, optionally, the current player
        /// TODO: rebuild database only for desynchronized profile
        /// </summary>
        public bool LoadPlayerSettings(bool alsoLoadCurrentPlayerProfile = true)
        {
            bool hasUpgraded = false;
            AppManager.I.AppSettingsManager.LoadSettings();

            if (alsoLoadCurrentPlayerProfile)
            {
                // No last active? Get the first one.
                if (AppManager.I.AppSettings.LastActivePlayerUUID == string.Empty)
                {
                    if (AppManager.I.AppSettings.SavedPlayers.Count > 0)
                    {
                        //UnityEngine.Debug.Log("No last! Get the first.");
                        AppManager.I.AppSettings.LastActivePlayerUUID = AppManager.I.AppSettings.SavedPlayers[0].Uuid;
                    }
                    else
                    {
                        AppManager.I.Player = null;
                        Debug.Log("Actual Player == null!!");
                    }
                }
                else
                {
                    string playerUUID = AppManager.I.AppSettings.LastActivePlayerUUID;

                    // Check whether the SQL DB is in-sync first
                    PlayerProfileData profileFromDB = AppManager.I.DB.LoadDatabaseForPlayer(playerUUID);

                    // If null, the player does not actually exist.
                    // The DB got desyinced. Do not load it!
                    if (profileFromDB != null)
                    {
                        //UnityEngine.Debug.Log("DB in sync! OK!");
                        SetPlayerAsCurrentByUUID(playerUUID, hasUpgraded);
                    }
                    else
                    {
                        //UnityEngine.Debug.Log("DB OUT OF SYNC. RESET");
                        ResetEverything();
                        LoadPlayerSettings();
                    }
                }
            }
            return hasUpgraded;
        }

        #endregion

        #region Players Icon Data

        /// <summary>
        /// Return the list of existing player profiles.
        /// </summary>
        public List<PlayerProfilePreview> GetPlayersIconData()
        {
            return FilterPlayerIconData(AppManager.I.AppSettings, AppManager.I.AppEdition.editionID, AppManager.I.ContentEdition.ContentID);
        }

        /// <summary>
        /// Return the list of existing player profiles filtered by the given classroom (where 0 is no classroom).
        /// </summary>
        public List<PlayerProfilePreview> GetPlayersIconDataForClassroom(int classroomIndex)
        {
            List<PlayerProfilePreview> result = FilterPlayerIconData(AppManager.I.AppSettings, AppManager.I.AppEdition.editionID, AppManager.I.ContentEdition.ContentID);
            for (int i = result.Count - 1; i >= 0; i--)
            {
                if (result[i].Classroom != classroomIndex)
                    result.RemoveAt(i);
            }
            return result;
        }

        public static List<PlayerProfilePreview> FilterPlayerIconData(AppSettings appSettings, AppEditionID appEditionID, LearningContentID contentID)
        {
            // With inverted profiles, the saved players are taken regardless of the content ID
            return appSettings.SavedPlayers.Where(pl => (pl.editionID == appEditionID || pl.editionID == AppEditionID.Multi)).ToList();
        }

        /// <summary>
        /// Updates the PlayerIconData for current player in list of PlayersIconData in GameSettings.
        /// </summary>
        public void UpdateCurrentPlayerIconDataInSettings()
        {
            for (int i = 0; i < AppManager.I.AppSettings.SavedPlayers.Count; i++)
            {
                if (AppManager.I.AppSettings.SavedPlayers[i].Uuid == _currentPlayer.Uuid)
                {
                    AppManager.I.AppSettings.SavedPlayers[i] = CurrentPlayer.GetPlayerPreview();
                }
            }
            AppManager.I.AppSettingsManager.SaveSettings();
        }

        /// <summary>
        /// Updates the PlayerIconData for the given player in list of PlayersIconData in GameSettings.
        /// </summary>
        public void UpdatePlayerIconDataInSettings(PlayerProfilePreview iconData)
        {
            for (int i = 0; i < AppManager.I.AppSettings.SavedPlayers.Count; i++)
            {
                if (AppManager.I.AppSettings.SavedPlayers[i].Uuid == iconData.Uuid)
                {
                    Debug.Log("SAVED " + iconData.PlayerName);
                    AppManager.I.AppSettings.SavedPlayers[i] = iconData;
                }
            }
            AppManager.I.AppSettingsManager.SaveSettings();
        }

        #endregion

        #region Player Profile Creation

        /// <summary>
        /// Creates the player profile.
        /// </summary>
        public string CreatePlayerProfile(int classroom, bool isNewAvatar, int avatarID, PlayerGender gender, PlayerTint tint, Color skinColor, Color hairColor, Color bgColor, int age, AppEditionID editionID, LearningContentID contentID, string appVersion, bool isDemoUser = false)
        {
            PlayerProfile returnProfile = new PlayerProfile();
            // Data
            returnProfile.Uuid = System.Guid.NewGuid().ToString();
            returnProfile.PlayerName = isDemoUser ? "teacher" : "student";
            returnProfile.Classroom = classroom;
            if (isNewAvatar)
                avatarID += NEW_AVATAR_ID_START;
            returnProfile.AvatarId = avatarID;
            returnProfile.Gender = gender;
            returnProfile.Tint = tint;
            returnProfile.SkinColor = skinColor;
            returnProfile.HairColor = hairColor;
            returnProfile.BgColor = bgColor;
            returnProfile.IsDemoUser = isDemoUser;
            returnProfile.Age = age;
            returnProfile.AppVersion = appVersion;
            returnProfile.editionID = editionID;
            returnProfile.ContentID = contentID;
            returnProfile.ProfileCompletion =
                isDemoUser ? ProfileCompletionState.GameCompletedAndFinalShown : ProfileCompletionState.New;
            returnProfile.GiftInitialBones();

            // DB Creation
            AppManager.I.DB.CreateDatabaseForPlayer(returnProfile.ToData());
            // Added to list
            AppManager.I.AppSettings.SavedPlayers.Add(returnProfile.GetPlayerPreview());
            // Set player profile as current player
            AppManager.I.PlayerProfileManager.CurrentPlayer = returnProfile;
            // Unlock the first Antura rewards
            AppManager.I.RewardSystemManager.UnlockFirstSetOfRewards();

            // Call Event Profile creation
            if (OnNewProfileCreated != null)
            {
                OnNewProfileCreated();
            }

            AppManager.I.Services.Analytics.TrackCompletedRegistration(returnProfile);

            return returnProfile.Uuid;
        }

        #endregion

        #region Player Profile Save/Load

        /// <summary>
        /// Saves the player profile.
        /// </summary>
        /// <param name="_playerProfile">The player profile.</param>
        public void SavePlayerProfile(PlayerProfile _playerProfile)
        {
            try
            {
                AppManager.I.DB.UpdatePlayerProfileData(_playerProfile.ToData());
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        #endregion

        #region Content Profile

        public ContentProfile GetContentProfile(LearningContentID contentId)
        {
            var dataFromDB = AppManager.I.DB.GetContentProfileData(contentId);
            if (dataFromDB == null)
            {
                // Not found, then create a new one instead
                var newProfile = new ContentProfile();
                newProfile.ContentID = contentId;
                return newProfile;
            }
            return new ContentProfile().FromData(dataFromDB);
        }

        /// <summary>
        /// Saves the content profile.
        /// </summary>
        /// <param name="_contentProfile">The content profile.</param>
        public void SaveContentProfile(ContentProfile _contentProfile)
        {
            try
            {
                AppManager.I.DB.UpdateContentProfileData(_contentProfile.ToData());
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        #endregion

        #region Player Profile Deletion

        /// <summary>
        /// Deletes the player profile.
        /// </summary>
        /// <param name="playerUUID">The player UUID.</param>
        /// <returns></returns>
        public PlayerProfile DeletePlayerProfile(string playerUUID)
        {
            PlayerProfile returnProfile = new PlayerProfile();
            // it prevents errors if rewards unlock coroutine is still running
            AppManager.I.StopAllCoroutines();
            // TODO: check if is necessary to hard delete DB
            PlayerProfilePreview playerIconData = GetPlayersIconData().Find(p => p.Uuid == playerUUID);
            if (playerIconData.Uuid == string.Empty)
            {
                return null;
            }
            // if setted as active player in gamesettings remove from it
            if (playerIconData.Uuid == AppManager.I.AppSettings.LastActivePlayerUUID)
            {
                // if possible set the first available player...
                PlayerProfilePreview newActivePlayerIcon = GetPlayersIconData().Find(p => p.Uuid != playerUUID);
                if (newActivePlayerIcon.Uuid != null)
                {
                    AppManager.I.PlayerProfileManager.SetPlayerAsCurrentByUUID(newActivePlayerIcon.Uuid);
                }
                else
                {
                    // ...else set to null
                    AppManager.I.PlayerProfileManager._currentPlayer = null;
                }
            }
            AppManager.I.AppSettings.SavedPlayers.Remove(playerIconData);

            AppManager.I.AppSettingsManager.SaveSettings();
            return returnProfile;
        }

        /// <summary>
        /// Resets everything.
        /// </summary>
        public void ResetEverything(bool clearOnly = false)
        {
            // Reset all the Databases
            if (AppManager.I.AppSettings.SavedPlayers != null)
            {
                foreach (PlayerProfilePreview pp in AppManager.I.AppSettings.SavedPlayers)
                {
                    AppManager.I.DB.LoadDatabaseForPlayer(pp.Uuid);
                    AppManager.I.DB.DropProfile();
                }
            }
            AppManager.I.DB.UnloadCurrentProfile();

            // Reset all settings too
            AppManager.I.AppSettingsManager.DeleteAllSettings();
            if (!clearOnly)
                LoadPlayerSettings(alsoLoadCurrentPlayerProfile: false);
            AppManager.I.Player = null;
        }

        /// <summary>
        /// delete all the players keeping all the other AppSettings intact.
        /// </summary>
        public void DeleteAllPlayers()
        {
            // Reset all the Databases
            if (AppManager.I.AppSettings.SavedPlayers != null)
            {
                foreach (PlayerProfilePreview pp in AppManager.I.AppSettings.SavedPlayers)
                {
                    AppManager.I.DB.LoadDatabaseForPlayer(pp.Uuid);
                    AppManager.I.DB.DropProfile();
                }
            }
            AppManager.I.DB.UnloadCurrentProfile();
            AppManager.I.AppSettingsManager.DeleteAllPlayers();
        }

        #endregion

        #region Import

        public void ImportAllPlayerProfiles()
        {
            ResetEverything();
            string[] importFilePaths = AppManager.I.DB.GetImportFilePaths();
            foreach (var filePath in importFilePaths)
            {
                // Check whether that is a DB and load it
                if (AppManager.I.DB.IsValidDatabasePath(filePath))
                {
                    ImportPlayerProfile(filePath);
                }
            }

            var firstPlayerUUID = AppManager.I.AppSettings.SavedPlayers[0].Uuid;
            AppManager.I.PlayerProfileManager.SetPlayerAsCurrentByUUID(firstPlayerUUID);
            AppManager.I.AppSettingsManager.SaveSettings();

        }

        public void ImportPlayerProfile(string filePath)
        {
            PlayerProfileData importedPlayerProfileData = AppManager.I.DB.ImportDynamicDatabase(filePath);
            if (importedPlayerProfileData != null)
            {
                PlayerProfile importedPlayerProfile = new PlayerProfile().FromData(importedPlayerProfileData);
                AppManager.I.AppSettings.SavedPlayers.Add(importedPlayerProfile.GetPlayerPreview());
            }
        }

        #endregion

        #region Events

        public delegate void ProfileEventHandler();

        /// <summary>
        /// Occurs when [on profile changed].
        /// </summary>
        public static event ProfileEventHandler OnProfileChanged;

        public static event ProfileEventHandler OnNewProfileCreated;

        #endregion

        #region Checks

        public bool IsDemoUserExisting()
        {
            bool demoUserExists = false;
            var playerList = GetPlayersIconData();
            foreach (var player in playerList)
            {
                if (player.IsDemoUser)
                {
                    demoUserExists = true;
                }
            }
            return demoUserExists;
        }

        #endregion
    }
}
