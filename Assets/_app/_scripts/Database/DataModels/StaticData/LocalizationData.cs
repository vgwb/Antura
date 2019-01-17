using Antura.Core;
using Antura.Profile;
using System;
using SQLite;
using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Data defining a Localization key-value pair.
    /// </summary>
    [Serializable]
    public class LocalizationData : IData
    {
        [PrimaryKey]
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        [SerializeField]
        private string _Id;

        // TODO: merge these two
        public string InstructionText
        {
            get { return _LocalizedText; }
            set { _LocalizedText = value; }
        }
        public string LearningText
        {
            get { return InstructionText; }
            set { InstructionText = value; }
        }
        [SerializeField]
        private string _LocalizedText;

        public string LocalizedTextFemale
        {
            get { return _LocalizedTextFemale; }
            set { _LocalizedTextFemale = value; }
        }
        [SerializeField]
        private string _LocalizedTextFemale;

        public string AudioFile
        {
            get { return _AudioFile; }
            set { _AudioFile = value; }
        }
        [SerializeField]
        private string _AudioFile;

        public override string ToString()
        {
            return Id + ": " + InstructionText;
        }

        public string GetId()
        {
            return Id;
        }

        public string GetLocalizedAudioFileName(PlayerGender playerGender)
        {
            if (playerGender == PlayerGender.F && LocalizedTextFemale != string.Empty && AudioFile != string.Empty) {
                return AudioFile + "_F";
            }
            return AudioFile;
        }

        public string GetLocalizedText(PlayerGender playerGender)
        {
            if (playerGender == PlayerGender.F && LocalizedTextFemale != string.Empty) {
                return LocalizedTextFemale;
            }
            return InstructionText;
        }

        public string GetSubtitleTranslation()
        {
            return InstructionText;
            // TODO: the distintion should be performed before accessing this by the localizer
            if (AppManager.I.AppSettings.AppLanguage == AppLanguages.Italian) {
                return InstructionText;
            } else {
                return InstructionText;
            }
        }
    }
}