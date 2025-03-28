using System;
using System.Collections.Generic;
using Antura.Core;
using Antura.Dog;
using Antura.Helpers;
using Antura.Profile;
using DG.DeExtensions;
using SQLite;
using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Database
{

    [Serializable]
    public class PetData
    {
        public AnturaPetType SelectedPet;
        public bool CatUnlocked;
    }

    public class PlayerProfileAdditionalData
    {
        public string PlayerName;
        public int Classroom;
        public TalkToPlayerStyle TalkToPlayerStyle;
        public List<DiscoverQuestSaved> Quests = new List<DiscoverQuestSaved>();

        /// <summary>
        /// Number of consecutive days of playin
        /// </summary>
        public int ConsecutivePlayDays;

        /// <summary>
        /// JSON data for the current shop unlocked state.
        /// </summary>
        public string CurrentShopStateJSON;

        /// <summary>
        /// JSON data for the current customiation shop unlocked state.
        /// </summary>
        public string CurrentCustomizationShopStateJSON;

        public PlayerProfileAdditionalData(
            string _PlayerName,
            int _Classroom,
            TalkToPlayerStyle _TalkToPlayerStyle,
            List<DiscoverQuestSaved> _SavedQuests,
            int _ConsecutivePlayDays,
            string currentShopStateJSON,
            string currentCustomizationShopStateJSON
            )
        {
            PlayerName = _PlayerName;
            Classroom = _Classroom;
            TalkToPlayerStyle = _TalkToPlayerStyle;
            Quests = _SavedQuests;
            ConsecutivePlayDays = _ConsecutivePlayDays;
            CurrentShopStateJSON = currentShopStateJSON;
            CurrentCustomizationShopStateJSON = currentCustomizationShopStateJSON;
        }
    }

    /// <summary>
    /// Serialized information about the player. Used by the Player Profile.
    /// </summary>
    [System.Serializable]
    public class PlayerProfileData : IData, IDataEditable
    {
        public const string UNIQUE_ID = "1";

        /// <summary>
        /// Primary key for the database.
        /// Unique, as there is only one row for this table.
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }

        /// <summary>
        /// Timestamp of creation of the profile data.
        /// </summary>
        public int Timestamp { get; set; }

        public string AppVersion { get; set; }

        public AppEditionID EditionID { get; set; }

        public LearningContentID ContentID { get; set; } // TODO: Move out, now part of the Content data

        #region Pet Data
        public AnturaPetType SelectedPet { get; set; }
        public bool CatUnlocked { get; set; }
        #endregion


        #region PlayerIconData

        /// <summary>
        /// Unique identifier for the player.
        /// Also used as the name of the database file.
        /// Part of PlayerIconData.
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// ID of the avatar icon for this player.
        /// Part of PlayerIconData.
        /// </summary>
        public int AvatarId { get; set; }

        /// <summary>
        /// Gender of the player.
        /// Part of PlayerIconData.
        /// </summary>
        public PlayerGender Gender { get; set; }

        /// <summary>
        /// Tint of the player icon.
        /// Part of PlayerIconData.
        /// Kept for backwards compatibility.
        /// </summary>
        public PlayerTint Tint { get; set; }

        /// <summary>Skin color</summary>
        public string SkinColor { get; set; }
        /// <summary>Hair color</summary>
        public string HairColor { get; set; }
        /// <summary>Bg color</summary>
        public string BgColor { get; set; }

        /// <summary>
        /// Is this player a demo user?
        /// Demo users have all the game unlocked.
        /// Part of PlayerIconData.
        /// </summary>
        public bool IsDemoUser { get; set; }

        /// <summary>
        /// Has the player completed the whole journey
        /// Used only for the player icons in the Home scene.
        /// Part of PlayerIconData.
        /// </summary>
        public bool JourneyCompleted { get; set; }  // TODO: Move out: now part of the Content Profile

        /// <summary>
        /// general total final overall score
        /// Used only for the player icons in the Home scene.
        /// Part of PlayerIconData.
        /// </summary>
        public float TotalScore { get; set; }   // TODO: Move out: now part of the Content Profile
        #endregion

        #region Details

        /// <summary>
        /// Age of the player, as selected during profile creation.
        /// </summary>
        public int Age { get; set; }

        #endregion

        #region Progression

        /// <summary>
        /// State of completion for the player profile.
        /// See PlayerProfile for further details.
        /// </summary>
        public ProfileCompletionState ProfileCompletion { get; set; }  // TODO: Move out!

        /// <summary>
        /// Maximum journey position: stage reached.
        /// </summary>
        public int MaxStage { get; set; }  // Deprecated: Now part of ContentProfile

        /// <summary>
        /// Maximum journey position: learning block reached.
        /// </summary>
        public int MaxLearningBlock { get; set; }  // Deprecated: Now part of ContentProfile

        /// <summary>
        /// Maximum journey position: play session reached.
        /// </summary>
        public int MaxPlaySession { get; set; }  // Deprecated: Now part of ContentProfile


        /// <summary>
        /// Current journey position: play session reached.
        /// </summary>
        public int CurrentStage { get; set; }  // Deprecated: Now part of ContentProfile

        /// <summary>
        /// Current journey position: learning block reached.
        /// </summary>
        public int CurrentLearningBlock { get; set; }  // Deprecated: Now part of ContentProfile

        /// <summary>
        /// Current journey position: play session reached.
        /// </summary>
        public int CurrentPlaySession { get; set; }   // Deprecated: Now part of ContentProfile

        /// <summary>
        /// State of the first contact in JSON format
        /// </summary>
        public string FirstContactStateJSON { get; set; }

        #endregion

        #region Rewards

        /// <summary>
        /// Total bones collected.
        /// </summary>
        public int TotalBones { get; set; }

        /// <summary>
        /// JSON data for the current customization set on Antura.
        /// </summary>
        public string CurrentAnturaCustomization { get; set; }
        #endregion

        #region Additional Data

        /// <summary>
        /// JSON-serialized additional data, may be added as needed.
        /// </summary>
        public string AdditionalData { get; set; }

        #endregion

        public PlayerProfileData()
        {
        }

        public PlayerProfileData(
                string _Uuid,
                string _PlayerName,
                int _Classroom,
                TalkToPlayerStyle _TalkToPlayerStyle,
                int _AvatarId,
                PlayerGender _Gender,
                PlayerTint _Tint,
                Color _SkinColor,
                Color _HairColor,
                Color _BgColor,
                int _Age,
                bool _IsDemoUser,
                bool _HasFinishedTheGame,
                bool _HasFinishedTheGameWithAllStars,
                bool _HasMaxStarsInCurrentPlaySessions,
                int totalBones,
                ProfileCompletionState profileCompletion,
                string currentAnturaCustomization,
                int comboPlayDays,
                AnturaSpace.ShopState currentShopState,
                CustomizationShopState currentCustomizationShopState,
                FirstContactState currentFirstContactState,
                AppEditionID editionID,
                LearningContentID contentID,
                string appVersion,
                PetData petData,
                List<DiscoverQuestSaved> _quests
                )
        {
            Id = UNIQUE_ID;  // Only one record
            AppVersion = appVersion;
            EditionID = editionID;
            ContentID = contentID;
            Uuid = _Uuid;
            AvatarId = _AvatarId;
            Gender = _Gender;
            Tint = _Tint;
            SkinColor = _SkinColor.ToHex();
            HairColor = _HairColor.ToHex();
            BgColor = _BgColor.ToHex();
            IsDemoUser = _IsDemoUser;
            //JourneyCompleted = _HasFinishedTheGame;
            //TotalScore = (_HasFinishedTheGameWithAllStars ? 1f : 0f);

            Age = _Age;
            //ProfileCompletion = profileCompletion;
            TotalBones = totalBones;
            SetMaxJourneyPosition(JourneyPosition.InitialJourneyPosition);
            SetCurrentJourneyPosition(JourneyPosition.InitialJourneyPosition);
            Timestamp = GenericHelper.GetTimestampForNow();
            CurrentAnturaCustomization = currentAnturaCustomization;
            AdditionalData = JsonUtility.ToJson(new PlayerProfileAdditionalData(_PlayerName, _Classroom, _TalkToPlayerStyle, _quests, comboPlayDays, currentShopState.ToJson(), currentCustomizationShopState.ToJson()));
            FirstContactStateJSON = JsonUtility.ToJson(currentFirstContactState);

            SelectedPet = petData.SelectedPet;
            CatUnlocked = petData.CatUnlocked;
        }

        public bool HasFinishedTheGameWithAllStars()
        {
            return (TotalScore >= 0.999f);
        }

        #region Journey Position

        public void SetMaxJourneyPosition(JourneyPosition pos)
        {
            MaxStage = pos.Stage;
            MaxLearningBlock = pos.LearningBlock;
            MaxPlaySession = pos.PlaySession;
        }

        public void SetCurrentJourneyPosition(JourneyPosition pos)
        {
            CurrentStage = pos.Stage;
            CurrentLearningBlock = pos.LearningBlock;
            CurrentPlaySession = pos.PlaySession;
        }

        public PlayerProfileAdditionalData GetAdditionalData()
        {
            var additionalData = JsonUtility.FromJson<PlayerProfileAdditionalData>(AdditionalData);
            if (additionalData != null)
            {
                return additionalData;
            }
            else
            {
                return new PlayerProfileAdditionalData("", 0, TalkToPlayerStyle.LearningThenNative, new List<DiscoverQuestSaved>(), 0, "", currentCustomizationShopStateJSON: "");
            }
        }

        public JourneyPosition GetMaxJourneyPosition()
        {
            return new JourneyPosition(MaxStage, MaxLearningBlock, MaxPlaySession);
        }

        public JourneyPosition GetCurrentJourneyPosition()
        {
            return new JourneyPosition(CurrentStage, CurrentLearningBlock, CurrentPlaySession);
        }

        #endregion

        #region Database API

        public string GetId()
        {
            return Id;
        }

        public void SetId(string _Id)
        {
            Id = _Id;
        }

        public override string ToString()
        {
            return string.Format("ID{0},U{1},Ts{2}, MaxJ({3}.{4}.{5}), CurrentJ({6}.{7}.{8}), ProfCompl:{9}, JourneyCompleted:{10}, Score:{11}, FirstContactPhaseJSON:{12}",
                Id,
                Uuid,
                Timestamp,

                MaxStage,
                MaxLearningBlock,
                MaxPlaySession,

                CurrentStage,
                CurrentLearningBlock,
                CurrentPlaySession,

                ProfileCompletion,
                JourneyCompleted,
                TotalScore,

                FirstContactStateJSON
            );
        }

        #endregion
    }
}
