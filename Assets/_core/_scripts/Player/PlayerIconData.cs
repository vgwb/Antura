using Antura.Core;
using System;
using UnityEngine;

namespace Antura.Profile
{
    /// <summary>
    /// Contains the data to generate a saved player profile (icon in the main menu)
    /// </summary>
    [Serializable]
    public struct PlayerIconData
    {
        public string Uuid;
        public int AvatarId;
        public PlayerGender Gender;
        public PlayerTint Tint; // Kept for backwards compatibility
        public Color SkinColor;
        public Color HairColor;
        public Color BgColor;
        public bool IsDemoUser;
        public bool HasFinishedTheGame;
        public bool HasFinishedTheGameWithAllStars;
        public bool HasMaxStarsInCurrentPlaySessions;
        public JourneyPosition MaxJourneyPosition;
        public AppEditionID editionID;
        public LearningContentID contentID;
        public string AppVersion;

        public PlayerIconData(string _Uuid, int _AvatarId, PlayerTint _Tint, PlayerGender _Gender, Color _SkinColor, Color _HairColor, Color _BgColor, bool _IsDemoUser,
            bool _HasFinishedTheGame, bool _HasFinishedTheGameWithAllStars, bool _HasMaxStarsInCurrentPlaySessions, JourneyPosition _MaxJourneyPosition, AppEditionID editionID, LearningContentID contentID, string _AppVersion)
        {
            Uuid = _Uuid;
            AvatarId = _AvatarId;
            SkinColor = _SkinColor;
            HairColor = _HairColor;
            BgColor = _BgColor;
            Gender = _Gender;
            Tint = _Tint;
            IsDemoUser = _IsDemoUser;
            HasFinishedTheGame = _HasFinishedTheGame;
            HasFinishedTheGameWithAllStars = _HasFinishedTheGameWithAllStars;
            HasMaxStarsInCurrentPlaySessions = _HasMaxStarsInCurrentPlaySessions;
            MaxJourneyPosition = _MaxJourneyPosition;
            this.editionID = editionID;
            this.contentID = contentID;
            AppVersion = _AppVersion;
            Debug.Log("CREATE PLAYER ICON DATA > " + SkinColor + " > " + HairColor);
        }

        public int NewAvatarId => AvatarId - PlayerProfileManager.NEW_AVATAR_ID_START;
        public bool IsOldAvatar => AvatarId < PlayerProfileManager.NEW_AVATAR_ID_START;
    }
}