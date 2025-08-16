using Antura.Core;
using Antura.Discover;
using Antura.Language;
using Antura.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Antura.Profile
{
    /// <summary>
    /// Contains the data to generate a saved player profile (icon in the main menu)
    /// it is saved in PlayerPrefs as List in AppSettings.SavedPlayers
    /// </summary>
    [Serializable]
    public struct PlayerProfilePreview
    {
        public string Uuid;
        public string PlayerName;
        public int Classroom;
        public bool EasyMode;
        public TalkToPlayerMode TalkToPlayerStyle;
        public LanguageCode NativeLanguage;
        public int AvatarId;
        public PlayerGender Gender;
        public PlayerTint Tint; // Kept for backwards compatibility
        public Color SkinColor;
        public Color HairColor;
        public Color BgColor;
        public bool IsDemoUser;
        public AppEditionID editionID;
        public string AppVersion;
        public PetData PetData;

        public PlayerProfilePreview(
            string _Uuid,
            string _PlayerName,
            int _Classroom,
            bool _EasyMode,
            int _AvatarId,
            PlayerGender _Gender,
            PlayerTint _Tint,
            Color _SkinColor,
            Color _HairColor,
            Color _BgColor,
            bool _IsDemoUser,
            AppEditionID editionID,
            string _AppVersion,
            PetData _petData)
        {
            Uuid = _Uuid;
            PlayerName = _PlayerName;
            Classroom = _Classroom;
            EasyMode = _EasyMode;
            NativeLanguage = LanguageCode.english;
            TalkToPlayerStyle = TalkToPlayerMode.LearningThenNative;
            AvatarId = _AvatarId;
            SkinColor = _SkinColor;
            HairColor = _HairColor;
            BgColor = _BgColor;
            Gender = _Gender;
            Tint = _Tint;
            IsDemoUser = _IsDemoUser;
            this.editionID = editionID;
            AppVersion = _AppVersion;
            PetData = _petData;
        }

        public int NewAvatarId => AvatarId - PlayerProfileManager.NEW_AVATAR_ID_START;
        public bool IsOldAvatar => AvatarId < PlayerProfileManager.NEW_AVATAR_ID_START;
    }
}
