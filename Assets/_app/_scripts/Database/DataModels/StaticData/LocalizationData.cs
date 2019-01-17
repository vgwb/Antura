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

        public string Character
        {
            get { return _Character; }
            set { _Character = value; }
        }
        [SerializeField]
        private string _Character;

        public string Area
        {
            get { return _Area; }
            set { _Area = value; }
        }
        [SerializeField]
        private string _Area;

        public string When
        {
            get { return _When; }
            set { _When = value; }
        }
        [SerializeField]
        private string _When;

        public string Context
        {
            get { return _Context; }
            set { _Context = value; }
        }
        [SerializeField]
        private string _Context;

        public string English
        {
            get { return _English; }
            set { _English = value; }
        }
        [SerializeField]
        private string _English;

        public string Italian
        {
            get { return _Italian; }
            set { _Italian = value; }
        }
        [SerializeField]
        private string _Italian;

        public string Arabic
        {
            get { return _Arabic; }
            set { _Arabic = value; }
        }
        [SerializeField]
        private string _Arabic;

        public string ArabicFemale
        {
            get { return _ArabicFemale; }
            set { _ArabicFemale = value; }
        }
        [SerializeField]
        private string _ArabicFemale;

        public string AudioFile
        {
            get { return _AudioFile; }
            set { _AudioFile = value; }
        }
        [SerializeField]
        private string _AudioFile;

        public override string ToString()
        {
            return Id + ": " + English;
        }

        public string GetId()
        {
            return Id;
        }

        public string GetLocalizedAudioFileName(PlayerGender playerGender)
        {
            if (playerGender == PlayerGender.F && ArabicFemale != string.Empty && AudioFile != string.Empty) {
                return AudioFile + "_F";
            }
            return AudioFile;
        }

        public string GetLocalizedText(PlayerGender playerGender)
        {
            if (playerGender == PlayerGender.F && ArabicFemale != string.Empty) {
                return ArabicFemale;
            }
            return Arabic;
        }

        public string GetSubtitleTranslation()
        {
            if (AppManager.I.AppSettings.AppLanguage == AppLanguages.Italian) {
                return Italian;
            } else {
                return English;
            }
        }
    }
}