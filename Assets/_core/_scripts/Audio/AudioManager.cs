using System;
using System.Collections;
using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using Antura.Minigames;
using Antura.Profile;
using DG.DeAudio;
using System.Collections.Generic;
using System.Linq;
using Antura.Language;
using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Audio
{
    /// <summary>
    /// Handles audio requests throughout the application
    /// </summary>
    public class AudioManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        public static AudioManager I;

        public bool IsAppPaused { get; private set; }

        private List<AudioSourceWrapper> playingAudio = new List<AudioSourceWrapper>();

        private DeAudioGroup musicGroup;
        private DeAudioGroup vocabularyGroup;
        private DeAudioGroup dialogueGroup;
        private DeAudioGroup sfxGroup;

        private Dictionary<IAudioSource, System.Action> dialogueEndedCallbacks = new Dictionary<IAudioSource, System.Action>();
        private Dictionary<IAudioSource, System.Action> vocabularyEndedCallbacks = new Dictionary<IAudioSource, System.Action>();

        private bool musicEnabled = true;
        private AudioClip customMusic;
        private Music currentMusic;

        public bool MusicEnabled
        {
            get => musicEnabled;

            set {
                if (musicEnabled == value) {
                    return;
                }

                try
                {
                    musicEnabled = value;
                    if (musicEnabled && currentMusic != Music.Silence)
                    {
                        if (musicGroup != null)
                        {
                            musicGroup.Resume();

                            bool hasToReset = true;
                            if (musicGroup.sources != null)
                            {
                                foreach (var s in musicGroup.sources)
                                {
                                    if (s.isPlaying)
                                    {
                                        hasToReset = false;
                                        break;
                                    }
                                }
                            }

                            if (hasToReset)
                            {
                                musicGroup.Play(currentMusic == Music.Custom ? customMusic : GetAudioClip(currentMusic),
                                    1, 1, true);
                            }
                        }
                    }
                    else
                    {
                        musicGroup?.Pause();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception ignored: " + e.Message);
                }

            }
        }

        private Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();

        public void ClearCallbacks()
        {
            dialogueEndedCallbacks.Clear();
            vocabularyEndedCallbacks.Clear();
        }

        #region Serialized Configuration

        [SerializeField, HideInInspector]
        private List<SfxConfiguration> sfxConfs = new List<SfxConfiguration>();

        [SerializeField, HideInInspector]
        private List<MusicConfiguration> musicConfs = new List<MusicConfiguration>();

        private Dictionary<Sfx, SfxConfiguration> sfxConfigurationMap = new Dictionary<Sfx, SfxConfiguration>();
        private Dictionary<Music, MusicConfiguration> musicConfigurationMap = new Dictionary<Music, MusicConfiguration>();

        public void ClearConfiguration()
        {
            sfxConfs.Clear();
            musicConfs.Clear();
            sfxConfigurationMap.Clear();
            musicConfigurationMap.Clear();
        }

        public void UpdateSfxConfiguration(SfxConfiguration conf)
        {
            var id = sfxConfs.FindIndex((a) => { return a.sfx == conf.sfx; });

            if (id >= 0) {
                sfxConfs.RemoveAt(id);
            }

            sfxConfs.Add(conf);
            sfxConfigurationMap[conf.sfx] = conf;
        }

        public void UpdateMusicConfiguration(MusicConfiguration conf)
        {
            var id = musicConfs.FindIndex((a) => { return a.music == conf.music; });

            if (id >= 0) {
                musicConfs.RemoveAt(id);
            }

            musicConfs.Add(conf);
            musicConfigurationMap[conf.music] = conf;
        }

        public MusicConfiguration GetMusicConfiguration(Music music)
        {
            MusicConfiguration v;
            if (musicConfigurationMap.TryGetValue(music, out v)) {
                return v;
            }
            return null;
        }

        public SfxConfiguration GetSfxConfiguration(Sfx sfx)
        {
            SfxConfiguration v;
            if (sfxConfigurationMap.TryGetValue(sfx, out v)) {
                return v;
            }
            return null;
        }

        #endregion

        void Awake()
        {
            I = this;

            sfxGroup = DeAudioManager.GetAudioGroup(DeAudioGroupId.FX);
            musicGroup = DeAudioManager.GetAudioGroup(DeAudioGroupId.Music);
            vocabularyGroup = DeAudioManager.GetAudioGroup(DeAudioGroupId.Custom0);
            dialogueGroup = DeAudioManager.GetAudioGroup(DeAudioGroupId.Custom1);

            musicEnabled = true;
        }

        /// <summary>
        /// called from AppManager
        /// </summary>
        /// <param name="pauseStatus">If set to <c>true</c> pause status.</param>
        public void OnAppPause(bool pauseStatus)
        {
            IsAppPaused = pauseStatus;
        }

        #region Music

        public void ToggleMusic()
        {
            MusicEnabled = !musicEnabled;
            AppManager.I.AppSettingsManager.SaveMusicSetting(MusicEnabled);
        }

        public IAudioSource PlayMusic(Music newMusic)
        {
            // Debug.Log("PlayMusic " + newMusic);
            if (currentMusic != newMusic) {
                currentMusic = newMusic;
                musicGroup.Stop();
                customMusic = null;
                var musicClip = GetAudioClip(currentMusic);

                if (currentMusic == Music.Silence || musicClip == null) {
                    StopMusic();
                } else {
                    if (musicEnabled) {
                        var source = musicGroup.Play(musicClip, 1, 1, true);
                        return new AudioSourceWrapper(source, musicGroup, this);
                    } else {
                        musicGroup.Stop();
                    }
                }
            }
            return null;
        }

        public void StopMusic()
        {
            customMusic = null;
            currentMusic = Music.Silence;
            musicGroup.Stop();
        }

        #endregion

        #region Sfx

        /// <summary>
        /// Play a soundFX
        /// </summary>
        /// <param name="sfx">Sfx.</param>
        public IAudioSource PlaySound(Sfx sfx)
        {
            AudioClip clip = GetAudioClip(sfx);
            var source = new AudioSourceWrapper(sfxGroup.Play(clip), sfxGroup, this);
            var conf = GetConfiguration(sfx);

            if (conf != null) {
                source.Pitch = 1 + ((UnityEngine.Random.value - 0.5f) * conf.randomPitchOffset) * 2;
                source.Volume = conf.volume;
            }

            return source;
        }

        public void StopSfxGroup()
        {
            sfxGroup.Stop();
        }

        #endregion

        #region Learning BLocks

        public IAudioSource PlayLearningBlock(string AudioFile, bool clearPreviousCallback = false)
        {
            if (clearPreviousCallback) {
                dialogueEndedCallbacks.Clear();
            }

            if (!string.IsNullOrEmpty(AudioFile)) {
                AudioClip clip = GetLearningBlockAudioClip(AudioFile);
                return new AudioSourceWrapper(dialogueGroup.Play(clip), dialogueGroup, this);
            }
            return null;
        }

        public IAudioSource PlayLearningBlock(string AudioFile, System.Action callback, bool clearPreviousCallback = false)
        {
            if (clearPreviousCallback) {
                dialogueEndedCallbacks.Clear();
            }

            if (!string.IsNullOrEmpty(AudioFile)) {
                AudioClip clip = GetLearningBlockAudioClip(AudioFile);
                var wrapper = new AudioSourceWrapper(dialogueGroup.Play(clip), dialogueGroup, this);
                if (callback != null) {
                    dialogueEndedCallbacks[wrapper] = callback;
                }
                return wrapper;
            }

            callback?.Invoke();
            return null;
        }

        #endregion

        #region Letters, Words and Phrases

        public IAudioSource PlayVocabularyDataAudio(ILivingLetterData data, bool exclusive = true, LetterDataSoundType soundType = LetterDataSoundType.Phoneme,
            LanguageUse use = LanguageUse.Learning, System.Action callback = null, bool clearPreviousCallback = false)
        {
            switch (data.DataType) {
                case LivingLetterDataType.Letter:
                    return PlayLetter((data as LL_LetterData).Data, exclusive, soundType, use, callback, clearPreviousCallback);
                case LivingLetterDataType.Word:
                    return PlayWord((data as LL_WordData).Data, exclusive, use, callback, clearPreviousCallback);
                case LivingLetterDataType.Image:
                    return PlayWord((data as LL_ImageData).Data, exclusive, use, callback, clearPreviousCallback);
                case LivingLetterDataType.Phrase:
                    return PlayPhrase((data as LL_PhraseData).Data, exclusive, use, callback, clearPreviousCallback);
                default:
                    return null;
            }
        }

        /// <summary>
        /// default values play Letter Phoneme
        /// </summary>
        /// <returns>The letter AudioClip</returns>
        /// <param name="data">Letter Data</param>
        /// <param name="exclusive">stops other letters?</param>
        /// <param name="soundType">Phoneme or Name?</param>
        public IAudioSource PlayLetter(LetterData data, bool exclusive = true, LetterDataSoundType soundType = LetterDataSoundType.Phoneme, LanguageUse use = LanguageUse.Learning, System.Action callback = null, bool clearPreviousCallback = false)
        {
            AudioClip clip = GetAudioClip(data, soundType, use);
            return PlayClip(clip, vocabularyEndedCallbacks, vocabularyGroup, exclusive, callback, clearPreviousCallback);
        }

        public IAudioSource PlayWord(WordData data, bool exclusive = true, LanguageUse use = LanguageUse.Learning, System.Action callback = null, bool clearPreviousCallback = false)
        {
            AudioClip clip = GetAudioClip(data, use);
            return PlayClip(clip, vocabularyEndedCallbacks, vocabularyGroup, exclusive, callback, clearPreviousCallback);
        }

        public IAudioSource PlayPhrase(PhraseData data, bool exclusive = true, LanguageUse use = LanguageUse.Learning, System.Action callback = null, bool clearPreviousCallback = false)
        {
            AudioClip clip = GetAudioClip(data, use);
            return PlayClip(clip, vocabularyEndedCallbacks, vocabularyGroup, exclusive, callback, clearPreviousCallback);
        }

        private IAudioSource PlayClip(AudioClip clip, Dictionary<IAudioSource, System.Action> callbacksDict, DeAudioGroup audioGroup,
            bool exclusive = true, System.Action callback = null, bool clearPreviousCallback = false)
        {
            if (exclusive) audioGroup?.Stop();
            if (clearPreviousCallback) callbacksDict.Clear();

            if (clip != null) {
                var wrapper = new AudioSourceWrapper(audioGroup.Play(clip), audioGroup, this);
                if (callback != null) callbacksDict[wrapper] = callback;
                return wrapper;
            }
            callback?.Invoke();
            return null;
        }



        public void StopVocabularyGroup()
        {
            vocabularyGroup?.Stop();
        }

        #endregion

        #region Dialogue

        public IAudioSource PlayDialogue(string localizationData_id, LanguageUse use = LanguageUse.Learning, System.Action callback = null)
        {
            return PlayDialogue(LocalizationManager.GetLocalizationData(localizationData_id), use, callback);
        }
        public IAudioSource PlayDialogue(LocalizationData data, LanguageUse use = LanguageUse.Learning, System.Action callback = null, bool clearPreviousCallback = false)
        {
            if (clearPreviousCallback) {
                //Debug.Log("-- PlayDialogue - clearPreviousCallback " + data.Id);
                dialogueEndedCallbacks.Clear();
            }

            if (!string.IsNullOrEmpty(LocalizationManager.GetLocalizedAudioFileName(data.Id))) {
                //Debug.Log("-- PlayDialogue - audio file exists - " + data.Id);
                AudioClip clip = GetAudioClip(data, use);
                var wrapper = new AudioSourceWrapper(dialogueGroup.Play(clip), dialogueGroup, this);
                if (callback != null) {
                    //Debug.Log("-- PlayDialogue - callback 1 - " + data.Id);
                    dialogueEndedCallbacks[wrapper] = callback;
                }
                return wrapper;
            }
            if (callback != null) {
                //Debug.Log("-- PlayDialogue - callback 2 - " + data.Id);
                callback.Invoke();
            }

            return null;
        }

        public void SkipCurrentDialogue()
        {
            StopDialogueNoClear();
            if (!AppManager.I.ParentEdition.SkipSingleLanguage)
            {
                Invoke("StopDialogueNoClear", 0.01f);
            }
        }

        void StopDialogueNoClear()
        {
            StopDialogue(false);
        }

        public void StopDialogue(bool clearPreviousCallback)
        {
            if (clearPreviousCallback) {
                dialogueEndedCallbacks.Clear();
            }

            dialogueGroup.Stop();
        }

        #endregion

        #region Audio clip management

        public AudioClip GetAudioClip(LocalizationData data, LanguageUse use)
        {
            var localizedAudioFileName = LocalizationManager.GetLocalizedAudioFileName(data.Id);
            var res = GetAudioClip("/Audio/Dialogs", localizedAudioFileName, use);

            Debug.LogWarning(localizedAudioFileName);

            // Fallback to neutral version if not found
            if (res == null) {
                var neutralAudioFileName = LocalizationManager.GetLocalizedAudioFileName(data.Id, PlayerGender.M);
                if (localizedAudioFileName != neutralAudioFileName) {
                    Debug.LogWarning("Female audio file expected for localization ID " + data.Id + " was not found");
                    res = GetAudioClip("/Audio/Dialogs", neutralAudioFileName, use);
                }
            }
            return res;
        }

        public AudioClip GetAudioClip(LetterData data, LetterDataSoundType soundType = LetterDataSoundType.Phoneme, LanguageUse use = LanguageUse.Learning)
        {
            var audiofileName = data.GetAudioFilename(soundType);
            return GetAudioClip("/Audio/Letters", audiofileName, use);
        }

        public AudioClip GetAudioClip(WordData data, LanguageUse use = LanguageUse.Learning)
        {
            return GetAudioClip("/Audio/Words", data.Id, use);
        }

        public AudioClip GetAudioClip(PhraseData data, LanguageUse use = LanguageUse.Learning)
        {
            return GetAudioClip("/Audio/Phrases", data.Id, use);
        }

        private AudioClip GetAudioClip(string folder, string id, LanguageUse use = LanguageUse.Learning)
        {
            var langDir = LanguageSwitcher.I.GetLangConfig(use).Code.ToString();
            string completePath = langDir + folder + "/" + id;
            var res = GetCachedResource(completePath);
            if (res == null) {
                Debug.LogWarning("Warning: cannot find audio clip at " + completePath);
            }
            return res;
        }

        public AudioClip GetLearningBlockAudioClip(string AudioFile, LanguageUse use = LanguageUse.Learning)
        {
            var langDir = LanguageSwitcher.I.GetLangConfig(use).Code.ToString();
            var res = GetCachedResource(langDir + "/Audio/LearningBlocks/" + AudioFile);
            if (res == null) {
                Debug.LogWarning("Warning: cannot find audio clip for LearningBlocks" + AudioFile);
            }
            return res;
        }

        public AudioClip GetAudioClip(Sfx sfx)
        {
            SfxConfiguration conf = GetSfxConfiguration(sfx);
            if (conf == null || conf.clips == null || conf.clips.Count == 0) {
                Debug.LogWarning("No Audio clips configured for: " + sfx);
                return null;
            }
            return conf.clips.GetRandom();
        }

        public SfxConfiguration GetConfiguration(Sfx sfx)
        {
            SfxConfiguration conf = GetSfxConfiguration(sfx);
            if (conf == null || conf.clips == null || conf.clips.Count == 0) {
                Debug.LogWarning("No Audio clips configured for: " + sfx);
                return null;
            }
            return conf;
        }

        public AudioClip GetAudioClip(Music music)
        {
            MusicConfiguration conf = GetMusicConfiguration(music);

            if (conf == null) {
                return null;
            }

            return conf.clip;
        }

        AudioClip GetCachedResource(string resource)
        {
            AudioClip clip = null;

            if (audioCache.TryGetValue(resource, out clip)) {
                return clip;
            }

            clip = Resources.Load(resource) as AudioClip;

            audioCache[resource] = clip;
            return clip;
        }

        public void ClearCache()
        {
            foreach (var r in audioCache) {
                Resources.UnloadAsset(r.Value);
            }
            audioCache.Clear();
        }

        #endregion

        List<KeyValuePair<AudioSourceWrapper, System.Action>> pendingCallbacks = new List<KeyValuePair<AudioSourceWrapper, System.Action>>();

        public void Update()
        {
            // Check ended callbacks
            for (int i = 0; i < playingAudio.Count; ++i) {
                var source = playingAudio[i];
                if (source.Update()) {
                    // could be collected
                    playingAudio.RemoveAt(i--);

                    System.Action callback;

                    if (source.Group == dialogueGroup && dialogueEndedCallbacks.TryGetValue(source, out callback)) {
                        pendingCallbacks.Add(new KeyValuePair<AudioSourceWrapper, System.Action>(source, callback));
                    }

                    if (source.Group == vocabularyGroup && vocabularyEndedCallbacks.TryGetValue(source, out callback)) {
                        pendingCallbacks.Add(new KeyValuePair<AudioSourceWrapper, System.Action>(source, callback));
                    }
                }
            }

            for (int i = 0; i < pendingCallbacks.Count; ++i) {
                pendingCallbacks[i].Value();
                dialogueEndedCallbacks.Remove(pendingCallbacks[i].Key);
                vocabularyEndedCallbacks.Remove(pendingCallbacks[i].Key);
            }

            pendingCallbacks.Clear();
        }

        public void OnAfterDeserialize()
        {
            // Update map from serialized data
            sfxConfigurationMap.Clear();
            for (int i = 0, count = sfxConfs.Count; i < count; ++i) {
                sfxConfigurationMap[sfxConfs[i].sfx] = sfxConfs[i];
            }

            musicConfigurationMap.Clear();
            for (int i = 0, count = musicConfs.Count; i < count; ++i) {
                musicConfigurationMap[musicConfs[i].music] = musicConfs[i];
            }
        }

        public void OnBeforeSerialize()
        {
        }

        public IAudioSource PlaySound(AudioClip clip)
        {
            return new AudioSourceWrapper(sfxGroup.Play(clip), sfxGroup, this);
        }

        public IAudioSource PlayMusic(AudioClip clip)
        {
            StopMusic();
            currentMusic = Music.Custom;

            var source = musicGroup.Play(clip);

            customMusic = clip;

            return new AudioSourceWrapper(source, musicGroup, this);
        }

        /// <summary>
        /// Used by AudioSourceWrappers.
        /// </summary>
        /// <param name="source"></param>
        public void OnAudioStarted(AudioSourceWrapper source)
        {
            if (!playingAudio.Contains(source)) {
                playingAudio.Add(source);
            }
        }

        /// <summary>
        /// Used to refresh the state of MusicEnabled if something goes wrong
        /// </summary>
        public void RefreshMusicEnabled()
        {
            MusicEnabled = IsGroupPlaying(musicGroup);
        }

        bool IsGroupPlaying(DeAudioGroup group)
        {
            if (group == null) return false;
            if (group.sources == null) return false;
            if (group.sources.Any(s => s.isPlaying)) return true;
            return false;
        }
    }
}