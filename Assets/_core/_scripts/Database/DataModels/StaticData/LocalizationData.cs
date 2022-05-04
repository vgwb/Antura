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
                return new LocalizedData($"ERROR {Id}", $"ERROR {Id}");
            }

            // We force Text data to use the shared arabic text
            if (lang == LanguageCode.arabic_legacy)
                lang = LanguageCode.arabic;

            var index = (int)lang - 1;
            if (index >= _LocalizedDatas.Length)
            {
                Debug.LogWarning($"No LocalizedData found for ID {Id} and language {lang}");
                return new LocalizedData($"ERROR {Id} {lang}", $"ERROR {Id} {lang}");
            }

            return _LocalizedDatas[index];
        }

        public string GetText(LanguageUse use)
        {
            switch (use)
            {
                case LanguageUse.Learning:
                    return GetLearningText(LocalizationManager.CurrentPlayerGender);
                case LanguageUse.Native:
                    return NativeText;
                case LanguageUse.Help:
                    return HelpText;
                default:
                    throw new ArgumentOutOfRangeException(nameof(use), use, null);
            }
        }

        public string HelpText
        {
            get
            {
                var lang = AppManager.I.ContentEdition.HelpLanguage;
                return GetLocalized(lang).Text;
            }
        }

        public string NativeText
        {
            get
            {
                var lang = AppManager.I.AppSettings.NativeLanguage;
                return GetLocalized(lang).Text;
            }
        }

        public string NativeText_F
        {
            get
            {
                var lang = AppManager.I.AppSettings.NativeLanguage;
                return GetLocalized(lang).TextF;
            }
        }

        public string LearningText
        {
            get
            {
                var lang = AppManager.I.ContentEdition.LearningLanguage;
                return GetLocalized(lang).Text;
            }
        }

        public string LearningText_F
        {
            get
            {
                var lang = AppManager.I.ContentEdition.LearningLanguage;
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
            if (playerGender == PlayerGender.F && AudioKey != string.Empty)
            {
                return AudioKey + "_F";
            }
            return AudioKey;
        }

        public string GetLearningText(PlayerGender playerGender)
        {
            if (playerGender == PlayerGender.F && LearningText_F != string.Empty)
            {
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
