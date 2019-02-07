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

        // NATIVE
        public string NativeText
        {
            get { return _NativeText; }
            set { _NativeText = value; }
        }
        [SerializeField]
        private string _NativeText;

        public string NativeText_F
        {
            get { return _NativeText_F; }
            set { _NativeText_F = value; }
        }
        [SerializeField]
        private string _NativeText_F;

        public string NativeAudio
        {
            get { return _NativeAudio; }
            set { _NativeAudio = value; }
        }
        [SerializeField]
        private string _NativeAudio;

        // LEARNING
        public string LearningText
        {
            get { return _LearningText; }
            set { _LearningText = value; }
        }
        [SerializeField]
        private string _LearningText;

        public string LearningText_F
        {
            get { return _LearningText_F; }
            set { _LearningText_F = value; }
        }
        [SerializeField]
        private string _LearningText_F;

        public string LearningAudio
        {
            get { return _LearningAudio; }
            set { _LearningAudio = value; }
        }
        [SerializeField]
        private string _LearningAudio;

        #region methods
        public override string ToString()
        {
            return Id + ": " + NativeText;
        }

        public string GetId()
        {
            return Id;
        }

        public string GetLocalizedAudioFileName(PlayerGender playerGender)
        {
            if (playerGender == PlayerGender.F && LearningText_F != string.Empty && LearningAudio != string.Empty) {
                return LearningAudio + "_F";
            }
            return LearningAudio;
        }

        public string GetLocalizedText(PlayerGender playerGender)
        {
            if (playerGender == PlayerGender.F && LearningText_F != string.Empty) {
                return LearningText_F;
            }
            return LearningText;
        }

        public string GetSubtitleTranslation()
        {
            return NativeText;
            // TODO: the distintion should be performed before accessing this by the localizer
            //if (AppManager.I.AppSettings.AppLanguage == AppLanguages.Italian) {
            //    return InstructionText;
            //} else {
            //    return InstructionText;
            //}
        }
        #endregion
    }
}