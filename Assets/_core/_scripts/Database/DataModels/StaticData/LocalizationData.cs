using Antura.Core;
using Antura.Profile;
using System;
using Antura.Helpers;
using Antura.Language;
using SQLite;
using UnityEngine;

namespace Antura.Database
{

    [Serializable]
    public struct LocalizedData
    {
        public string Text;
        public string TextF;

        public LocalizedData(string text, string textF)
        {
            Text = text;
            TextF = textF;
        }

        public override string ToString()
        {
            return Text;
        }
    }
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


        [PrimaryKey]
        public string AudioKey
        {
            get { return _AudioKey; }
            set { _AudioKey = value; }
        }
        [SerializeField]
        private string _AudioKey;

        [SerializeField]
        public LocalizedData[] _LocalizedDatas;
        public string LocalizedDatas_list
        {
            get { return _LocalizedDatas.ToJoinedString(); }
            set { }
        }

        public LocalizedData GetLocalized(LanguageCode lang)
        {
            if (_LocalizedDatas == null)
            {
                Debug.LogWarning($"No LocalizedData found for ID {Id}");
                return new LocalizedData($"ERROR {Id}",$"ERROR {Id}");
            }

            return _LocalizedDatas[(int)lang - 1];
        }

        public string HelpText
        {
            get
            {
                var lang = AppManager.I.SpecificEdition.HelpLanguage;
                return GetLocalized(lang).Text;
            }
        }

        public string NativeText
        {
            get
            {
                var lang = AppManager.I.SpecificEdition.NativeLanguage;
                return GetLocalized(lang).Text;
            }
        }

        public string NativeText_F
        {
            get
            {
                var lang =  AppManager.I.SpecificEdition.NativeLanguage;
                return GetLocalized(lang).TextF;
            }
        }

        public string LearningText
        {
            get
            {
                var lang =  AppManager.I.SpecificEdition.LearningLanguage;
                return GetLocalized(lang).Text;
            }
        }

        public string LearningText_F
        {
            get
            {
                var lang =  AppManager.I.SpecificEdition.LearningLanguage;
                return GetLocalized(lang).TextF;
            }
        }


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
            if (playerGender == PlayerGender.F && AudioKey != string.Empty) {
                return AudioKey + "_F";
            }
            return AudioKey;
        }

        public string GetLocalizedText(PlayerGender playerGender)
        {
            if (playerGender == PlayerGender.F && LearningText_F != string.Empty) {
                return LearningText_F;
            }
            return LearningText;
        }

        public string GetNativeText()
        {
            return NativeText;
        }
        #endregion
    }
}