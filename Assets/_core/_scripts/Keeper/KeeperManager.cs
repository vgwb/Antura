using System;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using Antura.LivingLetters;
using Antura.Minigames;
using Antura.UI;
using UnityEngine;

namespace Antura.Keeper
{
    public enum KeeperMode
    {
        Default,
        LearningNoSubtitles,
        LearningAndSubtitles,
        NativeNoSubtitles,
        NativeAndSubtitles,
        LearningThenNativeAndSubtitles,
        LearningThenNativeNoSubtitles,
        SubtitlesOnly,
    }

    /// <summary>
    /// Manages the Keeper throughout the application. The Keeper gives hints and explains minigames to the player.
    /// </summary>
    public class KeeperManager : MonoBehaviour
    {
        public static KeeperManager I;

        void Start()
        {
            I = this;
        }

        #region Play

        public void PlayDialogue(string id, bool isKeeper = true, bool autoClose = true, Action _callback = null, KeeperMode keeperMode = KeeperMode.Default)
        {
            PlayDialogue(LocalizationManager.GetLocalizationData(id), isKeeper, autoClose, _callback, keeperMode);
        }
        public void PlayDialogue(LocalizationDataId id, bool isKeeper = true, bool autoClose = true, Action _callback = null, KeeperMode keeperMode = KeeperMode.Default)
        {
            PlayDialogue(LocalizationManager.GetLocalizationData(id), isKeeper, autoClose, _callback, keeperMode);
        }
        public void PlayDialogue(LocalizationDataId id, KeeperMode keeperMode)
        {
            PlayDialogue(LocalizationManager.GetLocalizationData(id), true, true, null, keeperMode);
        }
        public void PlayDialogue(string id, KeeperMode keeperMode)
        {
            PlayDialogue(LocalizationManager.GetLocalizationData(id), true, true, null, keeperMode);
        }

        private void PlayDialogue(LocalizationData data, bool isKeeper = true, bool autoClose = true, Action _callback = null, KeeperMode keeperMode = KeeperMode.Default)
        {
            if (DebugConfig.I.VerboseAudio)
                Debug.Log("Keeper trying to play audio for PlayDialogue: " + data.Id);

            if (keeperMode == KeeperMode.Default)
            {
                keeperMode = AppManager.I.ContentEdition.LearnMethod.DefaultKeeperMode;
            }

            if (AppManager.I.ContentEdition.LearningLanguage == AppManager.I.AppSettings.NativeLanguage)
            {
                if (keeperMode == KeeperMode.LearningThenNativeAndSubtitles)
                    keeperMode = KeeperMode.LearningAndSubtitles;
                else if (keeperMode == KeeperMode.LearningThenNativeNoSubtitles)
                    keeperMode = KeeperMode.LearningNoSubtitles;
            }

            bool withSubtitles = keeperMode == KeeperMode.LearningThenNativeAndSubtitles ||
                                 keeperMode == KeeperMode.LearningAndSubtitles ||
                                 keeperMode == KeeperMode.NativeAndSubtitles || keeperMode == KeeperMode.SubtitlesOnly;

            LanguageCode langCodeRequested = LanguageCode.NONE;
            switch (keeperMode)
            {
                case KeeperMode.LearningThenNativeAndSubtitles:
                case KeeperMode.LearningAndSubtitles:
                    langCodeRequested = AppManager.I.LanguageSwitcher.GetLangConfig(LanguageUse.Learning).Code;
                    break;
                case KeeperMode.NativeAndSubtitles:
                    langCodeRequested = AppManager.I.LanguageSwitcher.GetLangConfig(LanguageUse.Native).Code;
                    break;
            }

            var sourcePath = new SourcePath(data.Id, "/Audio/Dialogs", langCodeRequested, gendered: true);
            if (withSubtitles && AudioManager.I.Exists(sourcePath, langCodeRequested))
            {
                WidgetSubtitles.I.DisplayDialogue(data, 2, isKeeper);
            }
            else
            {
                WidgetSubtitles.I.Close();
                autoClose = false;
            }

            switch (keeperMode)
            {
                case KeeperMode.LearningAndSubtitles:
                case KeeperMode.LearningNoSubtitles:
                    AudioManager.I.PlayDialogue(data, LanguageUse.Learning, () => OnEndSpeaking(_callback, autoClose), true);
                    break;
                case KeeperMode.NativeAndSubtitles:
                case KeeperMode.NativeNoSubtitles:
                    AudioManager.I.PlayDialogue(data, LanguageUse.Native, () => OnEndSpeaking(_callback, autoClose), true);
                    break;
                case KeeperMode.LearningThenNativeAndSubtitles:
                case KeeperMode.LearningThenNativeNoSubtitles:
                    AudioManager.I.PlayDialogue(data, LanguageUse.Learning,
                        () =>
                        {
                            AudioManager.I.PlayDialogue(data, LanguageUse.Native, () => OnEndSpeaking(_callback, autoClose), true);
                        }, true);
                    break;
                case KeeperMode.SubtitlesOnly:
                    // Nothing, we must close manually
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(keeperMode), keeperMode, null);
            }
        }

        public IAudioSource PlayVocabularyData(ILivingLetterData data,
            bool exclusive = false,
            bool isKeeper = true, bool autoClose = true, Action _callback = null,
            KeeperMode keeperMode = KeeperMode.Default,
            LetterDataSoundType soundType = LetterDataSoundType.Phoneme)
        {
            //Debug.LogWarning("Keeper playing Vocabulary " + data.Id);

            if (keeperMode == KeeperMode.Default)
                keeperMode = AppManager.I.ContentEdition.LearnMethod.DefaultKeeperMode;

            bool withSubtitles = keeperMode == KeeperMode.LearningThenNativeAndSubtitles ||
                                 keeperMode == KeeperMode.LearningAndSubtitles ||
                                 keeperMode == KeeperMode.NativeAndSubtitles || keeperMode == KeeperMode.SubtitlesOnly;
            if (withSubtitles)
            {
                WidgetSubtitles.I.DisplayVocabularyData(data, 2, isKeeper);
            }

            if (!withSubtitles)
                autoClose = false;

            IAudioSource playingSource = null;
            switch (keeperMode)
            {
                case KeeperMode.LearningAndSubtitles:
                case KeeperMode.LearningNoSubtitles:
                    playingSource = AudioManager.I.PlayVocabularyDataAudio(data, exclusive, soundType, LanguageUse.Learning, () => OnEndSpeaking(_callback, autoClose), true);
                    break;
                case KeeperMode.NativeAndSubtitles:
                case KeeperMode.NativeNoSubtitles:
                    playingSource = AudioManager.I.PlayVocabularyDataAudio(data, exclusive, soundType, LanguageUse.Native, () => OnEndSpeaking(_callback, autoClose), true);
                    break;
                case KeeperMode.LearningThenNativeAndSubtitles:
                case KeeperMode.LearningThenNativeNoSubtitles:
                    playingSource = AudioManager.I.PlayVocabularyDataAudio(data, exclusive, soundType, LanguageUse.Learning,
                    () =>
                    {
                        AudioManager.I.PlayVocabularyDataAudio(data, exclusive, soundType, LanguageUse.Native, () => OnEndSpeaking(_callback, autoClose), true);
                    }
                        , true);
                    break;
                case KeeperMode.SubtitlesOnly:
                    // Nothing, we must close manually
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(keeperMode), keeperMode, null);
            }
            return playingSource;
        }

        private void OnEndSpeaking(Action callback, bool closeSubtitles)
        {
            //Debug.Log("OnEndSpeaking");
            if (closeSubtitles)
            { CloseSubtitles(); }
            if (callback != null)
            {
                //Debug.Log("OnEndSpeaking - callback");
                callback.Invoke();
            }
        }

        #endregion

        #region Stop / Reset

        public void StopSpeaking()
        {
            AudioManager.I.StopDialogue(true);
            AudioManager.I.ClearCallbacks();
        }

        public void CloseSubtitles(bool _immediate = false)
        {
            WidgetSubtitles.I.Close(_immediate);
        }

        public void ResetKeeper()
        {
            WidgetSubtitles.I.Close(true);
            StopSpeaking();
        }

        #endregion


    }
}
