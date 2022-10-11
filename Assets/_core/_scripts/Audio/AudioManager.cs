//#define PRELOAD_DATA
using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using Antura.Minigames;
using Antura.Profile;
using Antura.Language;
using Antura.LivingLetters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.DeAudio;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
            set
            {
                if (musicEnabled == value)
                {
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
                                musicGroup.Play(currentMusic == Music.Custom ? customMusic : GetMusicAudioClip(currentMusic),
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

            if (id >= 0)
            {
                sfxConfs.RemoveAt(id);
            }

            sfxConfs.Add(conf);
            sfxConfigurationMap[conf.sfx] = conf;
        }

        public void UpdateMusicConfiguration(MusicConfiguration conf)
        {
            var id = musicConfs.FindIndex((a) => { return a.music == conf.music; });

            if (id >= 0)
            {
                musicConfs.RemoveAt(id);
            }

            musicConfs.Add(conf);
            musicConfigurationMap[conf.music] = conf;
        }

        public MusicConfiguration GetMusicConfiguration(Music music)
        {
            MusicConfiguration v;
            if (musicConfigurationMap.TryGetValue(music, out v))
            {
                return v;
            }
            return null;
        }

        public SfxConfiguration GetSfxConfiguration(Sfx sfx)
        {
            SfxConfiguration v;
            if (sfxConfigurationMap.TryGetValue(sfx, out v))
            {
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

        public IEnumerator PreloadDataCO()
        {
            // Optional pre-loading of all data
#if PRELOAD_DATA
            var opDialog =
                Addressables.LoadAssetsAsync<AudioClip>("audio_dialog", obj =>
                {
                    // Debug.Log(obj.name);
                    //audioCache[obj.name] = obj;
                });
            yield return opDialog;

            var opData =
                Addressables.LoadAssetsAsync<AudioClip>("audio_data", obj =>
                {
                    // Debug.Log(obj.name);
                    // audioCache[obj.name] = obj;
                });
            yield return opData;
#endif
            yield break;
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
            if (currentMusic != newMusic)
            {
                currentMusic = newMusic;
                musicGroup.Stop();
                customMusic = null;
                var musicClip = GetMusicAudioClip(currentMusic);

                if (currentMusic == Music.Silence || musicClip == null)
                {
                    StopMusic();
                }
                else
                {
                    if (musicEnabled)
                    {
                        var source = musicGroup.Play(musicClip, 1, 1, true);
                        return new AudioSourceWrapper(source, musicGroup, this);
                    }
                    else
                    {
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
            AudioClip clip = GetSfxAudioClip(sfx);
            var source = new AudioSourceWrapper(sfxGroup.Play(clip), sfxGroup, this);
            var conf = GetConfiguration(sfx);

            if (conf != null)
            {
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

        public IAudioSource PlayLearningBlock(string Id, bool clearPreviousCallback = false, LanguageUse use = LanguageUse.Learning)
        {
            var sourcePath = new SourcePath(Id, "/Audio/LearningBlocks", use);
            return PlayClip(sourcePath, dialogueEndedCallbacks, dialogueGroup, clearPreviousCallback: clearPreviousCallback);
        }

        #endregion

        #region Letters, Words and Phrases

        public IAudioSource PlayVocabularyDataAudio(ILivingLetterData data, bool exclusive = true, LetterDataSoundType soundType = LetterDataSoundType.Phoneme,
            LanguageUse use = LanguageUse.Learning, System.Action callback = null, bool clearPreviousCallback = false)
        {
            switch (data.DataType)
            {
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
            // if (DebugConfig.I.VerboseAudio)
            //     Debug.Log("PlayLetter " + data.GetAudioFilename(soundType) + " use " + use);
            var sourcePath = new SourcePath(data.GetAudioFilename(soundType), "/Audio/Letters", use);
            return PlayClip(sourcePath, vocabularyEndedCallbacks, vocabularyGroup, exclusive, callback, clearPreviousCallback);
        }

        public IAudioSource PlayWord(WordData data, bool exclusive = true, LanguageUse use = LanguageUse.Learning, System.Action callback = null, bool clearPreviousCallback = false)
        {
            var sourcePath = new SourcePath(data.Id, "/Audio/Words", use);
            return PlayClip(sourcePath, vocabularyEndedCallbacks, vocabularyGroup, exclusive, callback, clearPreviousCallback);
        }

        public IAudioSource PlayPhrase(PhraseData data, bool exclusive = true, LanguageUse use = LanguageUse.Learning, System.Action callback = null, bool clearPreviousCallback = false)
        {
            var sourcePath = new SourcePath(data.Id, "/Audio/Phrases", use);
            return PlayClip(sourcePath, vocabularyEndedCallbacks, vocabularyGroup, exclusive, callback, clearPreviousCallback);
        }

        private IAudioSource PlayClip(SourcePath path, Dictionary<IAudioSource, System.Action> callbacksDict, DeAudioGroup audioGroup,
            bool exclusive = true, System.Action callback = null, bool clearPreviousCallback = false)
        {
            if (DebugConfig.I.VerboseAudio)
                Debug.Log("[PlayClip] path: " + path.code + "/" + path.folder + "/" + path.id + " - " + path.gendered);

            if (exclusive)
                audioGroup?.Stop();
            if (clearPreviousCallback)
                callbacksDict.Clear();

            var wrapper = new AudioSourceWrapper(path, audioGroup, this);
            if (callback != null)
                callbacksDict[wrapper] = callback;
            return wrapper;
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

        public IAudioSource PlayDialogue(LocalizationData data, LanguageUse use = LanguageUse.Learning, Action callback = null, bool clearPreviousCallback = false)
        {
            return PlayDialogue(data, AppManager.I.LanguageSwitcher.GetLangConfig(use).Code, callback, clearPreviousCallback);
        }

        private LanguageCode currentLangCode;
        public IAudioSource PlayDialogue(LocalizationData data, LanguageCode code, Action callback = null, bool clearPreviousCallback = false)
        {
            currentLangCode = code;
            if (clearPreviousCallback)
            {
                dialogueEndedCallbacks.Clear();
            }

            if (skipNext)
            {
                skipNext = false;
                callback?.Invoke();
                return null;
            }

            if (!string.IsNullOrEmpty(LocalizationManager.GetLocalizedAudioFileName(data.Id)))
            {
                var sourcePath = new SourcePath(data.Id, "/Audio/Dialogs", code, gendered: true);
                var wrapper = new AudioSourceWrapper(sourcePath, dialogueGroup, this);
                if (callback != null)
                {
                    dialogueEndedCallbacks[wrapper] = callback;
                }
                return wrapper;
            }

            callback?.Invoke();

            return null;
        }

        private bool skipNext = false;
        public void SkipCurrentDialogue()
        {
            StopDialogue(false);
            if (!AppManager.I.ContentEdition.LearnMethod.SkipSingleLanguage
                && dialogueEndedCallbacks.Any(x => x.Value.Method.Name.Contains("PlayDialogue"))
                && currentLangCode == AppManager.I.ContentEdition.LearningLanguage)
            {
                skipNext = true;
            }
        }

        void StopDialogueNoClear()
        {
            StopDialogue(false);
        }

        public void StopDialogue(bool clearPreviousCallback)
        {
            if (clearPreviousCallback)
            {
                dialogueEndedCallbacks.Clear();
            }

            dialogueGroup.Stop();
        }

        #endregion

        #region Audio clip management

        #region Audio Get

        public AudioClip GetSfxAudioClip(Sfx sfx)
        {
            SfxConfiguration conf = GetSfxConfiguration(sfx);
            if (conf == null || conf.clips == null || conf.clips.Count == 0)
            {
                if (DebugConfig.I.VerboseAudio)
                    Debug.LogWarning("[Audio] No Audio clips configured for: " + sfx);
                return null;
            }
            return conf.clips.GetRandom();
        }

        public AudioClip GetMusicAudioClip(Music music)
        {
            MusicConfiguration conf = GetMusicConfiguration(music);
            if (conf == null)
            {
                return null;
            }
            return conf.clip;
        }

        #endregion

        public SfxConfiguration GetConfiguration(Sfx sfx)
        {
            SfxConfiguration conf = GetSfxConfiguration(sfx);
            if (conf == null || conf.clips == null || conf.clips.Count == 0)
            {
                if (DebugConfig.I.VerboseAudio)
                    Debug.LogWarning("[Audio] No Audio clips configured for: " + sfx);
                return null;
            }
            return conf;
        }

        AudioClip GetCachedResource(string resource)
        {
            AudioClip clip;
            if (audioCache.TryGetValue(resource, out clip))
            {
                return clip;
            }
            return null;
        }

        public void ClearCache()
        {
            foreach (var r in audioCache)
            {
                Resources.UnloadAsset(r.Value);
            }
            audioCache.Clear();
        }

        #region Addressable Loading

        public bool Exists(SourcePath path, LanguageCode langCode)
        {
            var langDir = langCode.ToString();
            var completePath = $"{langDir}{path.folder}/{path.id}";
            return AssetLoader.Exists<AudioClip>(completePath);
        }

        private IEnumerator LoadAudio(AudioSourceWrapper source)
        {
            //Debug.Log($"Start loading {source.Path.id}");
            var clip = new Ref<AudioClip>();
            yield return LoadAudioClip(source.Path, clip);
            source.Loaded = true;
            if (clip.item != null)
                source.ApplyClip(clip.item);
        }

        public IEnumerator LoadAudioClip(SourcePath path, Ref<AudioClip> result)
        {
            if (!path.gendered)
            {
                yield return LoadAudioClip(path.folder, path.id, result, path.code);
                yield break;
            }

            // For dialogs, we localize them
            var localizedAudioFileName = LocalizationManager.GetLocalizedAudioFileName(path.id);
            yield return LoadAudioClip(path.folder, localizedAudioFileName, result, path.code, logIfNotFound: false);

            // Fallback to neutral version if not found
            if (result.item == null)
            {
                var neutralAudioFileName = LocalizationManager.GetLocalizedAudioFileName(path.id, PlayerGender.M);
                if (localizedAudioFileName != neutralAudioFileName)
                {
                    // No female found
                    if (DebugConfig.I.VerboseAudio)
                        Debug.Log($"[Audio] No Female audio file for localization ID {path.id} was found. Fallback to male/neutral.");
                    yield return LoadAudioClip("/Audio/Dialogs", neutralAudioFileName, result, path.code);
                }
            }

            if (result.item == null && DebugConfig.I.VerboseAudio)
            {
                var neutralAudioFileName = LocalizationManager.GetLocalizedAudioFileName(path.id, PlayerGender.M);
                Debug.LogError($"[Audio] Could not find any audio with name /Audio/Dialogs/{neutralAudioFileName}");
            }
        }

        private IEnumerator LoadAudioClip(string folder, string id, Ref<AudioClip> result, LanguageCode code = LanguageCode.NONE, bool logIfNotFound = true)
        {
            var langDir = code.ToString();
            string completePath = $"{langDir}{folder}/{id}";
            var cachedResource = GetCachedResource(completePath);
            if (cachedResource != null)
            {
                result.item = cachedResource;
                yield break;
            }

            yield return AssetLoader.ValidateAndLoad<AudioClip>(completePath, audioClip => result.item = audioClip);
            //Debug.LogError("Loaded: " + result.item + " for id " + id);

            if (logIfNotFound && result.item == null)
            {
                if (DebugConfig.I.VerboseAudio)
                    Debug.LogWarning($"[Audio] Cannot find audio clip at {completePath}");
            }
        }

        public class Ref<T>
        {
            public T item;
        }
        #endregion

        #endregion

        List<KeyValuePair<AudioSourceWrapper, System.Action>> pendingCallbacks = new List<KeyValuePair<AudioSourceWrapper, System.Action>>();

        public void Update()
        {
            // Check ended callbacks
            for (int i = 0; i < playingAudio.Count; ++i)
            {
                var source = playingAudio[i];

                if (source.Asynch && !source.Loaded)
                {
                    //Debug.Log("Waiting for source to load... " + source.Path.id);
                    continue;
                }
                bool failed = source.Loaded && source.CurrentSource == null;

                if (source.Update() || failed)
                {
                    // could be collected
                    playingAudio.RemoveAt(i--);

                    if (failed && source.Clip == null)
                    {
                        Debug.LogError($"Missing audio '{source.Path.id}' for language '{source.Path.code}'");
                    }
                    else
                    {
                        //Debug.LogError("Source ended for " + source.Path.id);
                    }

                    System.Action callback;

                    if (source.Group == dialogueGroup && dialogueEndedCallbacks.TryGetValue(source, out callback))
                    {
                        pendingCallbacks.Add(new KeyValuePair<AudioSourceWrapper, System.Action>(source, callback));
                    }

                    if (source.Group == vocabularyGroup && vocabularyEndedCallbacks.TryGetValue(source, out callback))
                    {
                        pendingCallbacks.Add(new KeyValuePair<AudioSourceWrapper, System.Action>(source, callback));
                    }
                }
            }

            for (int i = 0; i < pendingCallbacks.Count; ++i)
            {
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
            for (int i = 0, count = sfxConfs.Count; i < count; ++i)
            {
                sfxConfigurationMap[sfxConfs[i].sfx] = sfxConfs[i];
            }

            musicConfigurationMap.Clear();
            for (int i = 0, count = musicConfs.Count; i < count; ++i)
            {
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
            if (!playingAudio.Contains(source))
            {
                playingAudio.Add(source);
                if (source.Asynch)
                    StartCoroutine(LoadAudio(source));
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
            if (group == null)
                return false;
            if (group.sources == null)
                return false;
            if (group.sources.Any(s => s.isPlaying))
                return true;
            return false;
        }
    }

    public struct SourcePath
    {
        public string id;
        public string folder;
        public LanguageCode code;
        public bool gendered;

        public SourcePath(string id, string folder, LanguageUse use, bool gendered = false)
        {
            this.id = id;
            this.folder = folder;
            this.code = AppManager.I.LanguageSwitcher.GetLangConfig(use).Code;
            this.gendered = gendered;
        }

        public SourcePath(string id, string folder, LanguageCode code, bool gendered = false)
        {
            this.id = id;
            this.folder = folder;
            this.code = code;
            this.gendered = gendered;
        }
    }
}
