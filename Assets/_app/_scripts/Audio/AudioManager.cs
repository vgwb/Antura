using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using Antura.Minigames;
using Antura.Profile;
using DG.DeAudio;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Audio
{
    /// <summary>
    /// Handles audio requests throughout the application
    /// </summary>
    public class AudioManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        public static AudioManager I;

        // TODO make dynamic by language manager
        private string langResFolder = "arabic/Audio/";

        public bool IsAppPaused { get; private set; }

        private List<AudioSourceWrapper> playingAudio = new List<AudioSourceWrapper>();

        private DeAudioGroup musicGroup;
        private DeAudioGroup vocabularyGroup;
        private DeAudioGroup dialogueGroup;
        private DeAudioGroup sfxGroup;

        private Dictionary<IAudioSource, System.Action> dialogueEndedCallbacks = new Dictionary<IAudioSource, System.Action>();

        private bool previousMusicEnabled = true;
        private bool musicEnabled = true;
        private AudioClip customMusic;
        private Music currentMusic;

        public bool MusicEnabled
        {
            get { return musicEnabled; }

            set {
                if (musicEnabled == value) {
                    // Debug.Log("AudioManager MusicEnabled A");
                    return;
                }

                musicEnabled = previousMusicEnabled = value;
                // Debug.Log("AudioManager MusicEnabled B to " + musicEnabled);
                if (musicEnabled && (currentMusic != Music.Silence)) {
                    if (musicGroup != null) {
                        musicGroup.Resume();

                        bool hasToReset = false;

                        if (musicGroup.sources == null) {
                            hasToReset = true;
                        } else {
                            foreach (var s in musicGroup.sources) {
                                if (s.isPlaying) {
                                    goto Cont;
                                }
                            }
                            hasToReset = true;
                        }
                    Cont:
                        if (hasToReset) {
                            if (currentMusic == Music.Custom) {
                                musicGroup.Play(customMusic, 1, 1, true);
                            } else {
                                musicGroup.Play(GetAudioClip(currentMusic), 1, 1, true);
                            }
                        }
                    }
                } else {
                    if (musicGroup != null) {
                        musicGroup.Pause();
                    }
                }
            }
        }

        private Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();

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
            if (pauseStatus) {
                previousMusicEnabled = MusicEnabled;
                MusicEnabled = false;
            } else {
                MusicEnabled = previousMusicEnabled;
            }

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

        #region play various
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
            } else {
                if (callback != null) {
                    callback();
                }
            }
            return null;
        }
        #endregion

        #region Letters, Words and Phrases

        /// <summary>
        /// default values play Letter Phoneme
        /// </summary>
        /// <returns>The letter AudioClip</returns>
        /// <param name="data">Letter Data</param>
        /// <param name="exclusive">stops other letters?</param>
        /// <param name="soundType">Phoneme or Name?</param>
        public IAudioSource PlayLetter(LetterData data, bool exclusive = true, LetterDataSoundType soundType = LetterDataSoundType.Phoneme)
        {
            if (exclusive) {
                StopVocabularyGroup();
            }

            AudioClip clip = GetAudioClip(data, soundType);
            return new AudioSourceWrapper(vocabularyGroup.Play(clip), vocabularyGroup, this);
        }

        public IAudioSource PlayWord(WordData data, bool exclusive = true)
        {
            if (exclusive) {
                StopVocabularyGroup();
            }

            AudioClip clip = GetAudioClip(data);
            return new AudioSourceWrapper(vocabularyGroup.Play(clip), vocabularyGroup, this);
        }

        public IAudioSource PlayPhrase(PhraseData data, bool exclusive = true)
        {
            if (exclusive) {
                StopVocabularyGroup();
            }

            AudioClip clip = GetAudioClip(data);
            return new AudioSourceWrapper(vocabularyGroup.Play(clip), vocabularyGroup, this);
        }

        public void StopVocabularyGroup()
        {
            if (vocabularyGroup != null) {
                vocabularyGroup.Stop();
            }
        }

        #endregion

        #region Dialogue

        public IAudioSource PlayDialogue(string localizationData_id)
        {
            return PlayDialogue(LocalizationManager.GetLocalizationData(localizationData_id));
        }

        public IAudioSource PlayDialogue(LocalizationDataId id)
        {
            return PlayDialogue(LocalizationManager.GetLocalizationData(id));
        }

        public IAudioSource PlayDialogue(LocalizationData data, bool clearPreviousCallback = false)
        {
            //Debug.Log("PlayDialogue " + data.Id);

            if (clearPreviousCallback) {
                dialogueEndedCallbacks.Clear();
            }

            if (!string.IsNullOrEmpty(LocalizationManager.GetLocalizedAudioFileName(data.Id))) {
                AudioClip clip = GetAudioClip(data);
                //Debug.Log("PlayDialogue " + clip);
                return new AudioSourceWrapper(dialogueGroup.Play(clip), dialogueGroup, this);
            }
            return null;
        }

        public IAudioSource PlayDialogue(string localizationData_id, System.Action callback)
        {
            return PlayDialogue(LocalizationManager.GetLocalizationData(localizationData_id), callback);
        }

        public IAudioSource PlayDialogue(LocalizationDataId id, System.Action callback, bool clearPreviousCallback = false)
        {
            return PlayDialogue(LocalizationManager.GetLocalizationData(id), callback, clearPreviousCallback);
        }

        public IAudioSource PlayDialogue(LocalizationData data, System.Action callback, bool clearPreviousCallback = false)
        {
            if (clearPreviousCallback) {
                dialogueEndedCallbacks.Clear();
            }

            if (!string.IsNullOrEmpty(LocalizationManager.GetLocalizedAudioFileName(data.Id))) {
                AudioClip clip = GetAudioClip(data);
                var wrapper = new AudioSourceWrapper(dialogueGroup.Play(clip), dialogueGroup, this);
                if (callback != null) {
                    dialogueEndedCallbacks[wrapper] = callback;
                }
                return wrapper;
            } else {
                if (callback != null) {
                    callback();
                }
            }
            return null;
        }

        public void StopDialogue(bool clearPreviousCallback)
        {
            if (clearPreviousCallback) {
                dialogueEndedCallbacks.Clear();
            }

            dialogueGroup.Stop();
        }

        private PlayerGender GetPlayerGender()
        {
            return AppManager.I.Player != null ? AppManager.I.Player.Gender : PlayerGender.M;
        }

        #endregion

        #region Audio clip management

        public AudioClip GetAudioClip(LocalizationData data)
        {
            var localizedAudioFileName = LocalizationManager.GetLocalizedAudioFileName(data.Id);
            var res = GetCachedResource(langResFolder + "Dialogs/" + localizedAudioFileName);

            // Fallback to neutral version if not found
            if (res == null) {
                var neutralAudioFileName = LocalizationManager.GetLocalizedAudioFileName(data.Id, PlayerGender.M);
                if (localizedAudioFileName != neutralAudioFileName) {
                    Debug.LogWarning("Female audio file expected for localization ID " + data.Id + " was not found");
                    res = GetCachedResource(langResFolder + "Dialogs/" + neutralAudioFileName);
                }
            }

            return res;
        }

        public AudioClip GetAudioClip(LetterData data, LetterDataSoundType soundType = LetterDataSoundType.Phoneme)
        {
            AudioClip res;
            var audiofile = data.GetAudioFilename(soundType);
            res = GetCachedResource(langResFolder + "Letters/" + audiofile);

            if (res == null) {
                Debug.LogWarning("Warning: cannot find audio clip for letter:" + data + " filename:" + audiofile);
            }
            return res;
        }

        public AudioClip GetAudioClip(WordData data)
        {
            var res = GetCachedResource(langResFolder + "Words/" + data.Id);
            if (res == null) {
                Debug.LogWarning("Warning: cannot find audio clip for " + data);
            }
            return res;
        }

        public AudioClip GetAudioClip(PhraseData data)
        {
            var res = GetCachedResource(langResFolder + "Phrases/" + data.Id);
            if (res == null) {
                Debug.LogWarning("Warning: cannot find audio clip for " + data);
            }
            return res;
        }

        public AudioClip GetLearningBlockAudioClip(string AudioFile)
        {
            var res = GetCachedResource(langResFolder + "LearningBlocks/" + AudioFile);
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
            for (int i = 0; i < playingAudio.Count; ++i) {
                var source = playingAudio[i];
                if (source.Update()) {
                    // could be collected
                    playingAudio.RemoveAt(i--);

                    System.Action callback;
                    if (source.Group == dialogueGroup && dialogueEndedCallbacks.TryGetValue(source, out callback)) {
                        pendingCallbacks.Add(new KeyValuePair<AudioSourceWrapper, System.Action>(source, callback));
                    }
                }
            }

            for (int i = 0; i < pendingCallbacks.Count; ++i) {
                pendingCallbacks[i].Value();
                dialogueEndedCallbacks.Remove(pendingCallbacks[i].Key);
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
    }
}