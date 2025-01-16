using Antura.Core;
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
    /// </summary>
    [Serializable]
    public struct PlayerIconData
    {
        public string Uuid;
        public string PlayerName;
        public int Classroom;
        public TalkToPlayerStyle TalkToPlayerStyle;
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

        public PlayerIconData(
            string _Uuid,
            string _PlayerName,
            int _Classroom,
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
            NativeLanguage = LanguageCode.english;
            TalkToPlayerStyle = TalkToPlayerStyle.LearningThenNative;
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
            Debug.Log("CREATE PLAYER ICON DATA > " + SkinColor + " > " + HairColor);
        }

        public int NewAvatarId => AvatarId - PlayerProfileManager.NEW_AVATAR_ID_START;
        public bool IsOldAvatar => AvatarId < PlayerProfileManager.NEW_AVATAR_ID_START;
    }
}
