using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Antura
{
    public class AssetManager
    {
        public bool VERBOSE => DebugConfig.I.VerboseAssetsManager;

        private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
        private Dictionary<string, ShapeLetterData> shapeDataCache = new Dictionary<string, ShapeLetterData>();
        private Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();
        private Dictionary<string, TextAsset> textCache = new Dictionary<string, TextAsset>();
        private Dictionary<string, AudioClip> nativeAudioCache = new Dictionary<string, AudioClip>();

        public IEnumerator PreloadDataCO()
        {
            // We ovverride the exception handler, or addressables will use theirs and we do not want that.
            UnityEngine.ResourceManagement.ResourceManager.ExceptionHandler = ((op, exception) =>
            {
                if (VERBOSE)
                {
                    var msg = exception.ToString();
                    if (msg.Contains("InvalidKeyException"))
                    {
                        var index = msg.IndexOf("Keys=");
                        if (index >= 0)
                            msg = "Could not find subset of keys: " + msg.Substring(index, msg.Length - index);
                    }
                    Debug.LogError(msg);
                }
            });

            // First release preloaded data
            ClearCache(spriteCache);
            ClearCache(shapeDataCache);
            ClearCache(audioCache);
            ClearCache(nativeAudioCache);
            ClearCache(textCache);

            // Pre-load images and icons for the wanted language
            var learningLanguageCode = LanguageSwitcher.I.GetLangConfig(LanguageUse.Learning).Code;
            var nativeLanguageCode = LanguageSwitcher.I.GetLangConfig(LanguageUse.Native).Code;

            // Icons
            if (VERBOSE)
                Debug.Log("[Assets] Preloading Icons");
            var iconKeys = new HashSet<string>();
            foreach (var miniGameData in AppManager.I.DB.GetAllMiniGameData())
            {
                string spriteName = $"minigame_Ico_{miniGameData.Main}";
                // iconKeys.Add($"{learningLanguageCode}/Images/GameIcons/{spriteName}[{spriteName}]");
                iconKeys.Add($"common/Images/GameIcons/{spriteName}[{spriteName}]");
            }
            yield return LoadAssets(iconKeys, spriteCache, DebugConfig.I.AddressablesBlockingLoad);

            // Badges
            if (VERBOSE)
                Debug.Log("[Assets] Preloading Badges");
            var badgeKeys = new HashSet<string>();
            foreach (var miniGameData in AppManager.I.DB.GetAllMiniGameData())
            {
                string spriteName = $"minigame_BadgeIco_{miniGameData.Badge}";
                // badgeKeys.Add($"{learningLanguageCode}/Images/GameIcons/{spriteName}[{spriteName}]");
                badgeKeys.Add($"common/Images/GameIcons/{spriteName}[{spriteName}]");
            }
            yield return LoadAssets(badgeKeys, spriteCache, DebugConfig.I.AddressablesBlockingLoad);

            // Shape data
            if (VERBOSE)
                Debug.Log("[Assets] Preloading Shape Data");
            if (AppManager.I.DB.GetActiveMinigames().Any(x => x.Code == MiniGameCode.Maze_lettername
                                                                    || x.Code == MiniGameCode.SickLetters_lettername))
            {
                var sideKeys = new HashSet<string>();
                var learningFont = AppManager.I.LanguageSwitcher.GetLangConfig(LanguageUse.Learning).LanguageFont;
                var fontName = learningFont.name.Split(" ").First().Split('_').Last();
                foreach (var letterData in AppManager.I.DB.GetAllLetterData())
                {
                    sideKeys.Add($"{fontName}/shapedata_{letterData.GetCompleteUnicodes()}");
                }
                yield return LoadAssets(sideKeys, shapeDataCache, DebugConfig.I.AddressablesBlockingLoad);
            }

            // Song data
            if (VERBOSE)
                Debug.Log("[Assets] Preloading Song Data");

            bool hasSimonSong = AppManager.I.DB.GetActiveMinigames().Any(x =>
                x.Code == MiniGameCode.Song_word_animals
                || x.Code == MiniGameCode.Song_word_body
                || x.Code == MiniGameCode.Song_word_city
                || x.Code == MiniGameCode.Song_word_family
                || x.Code == MiniGameCode.Song_word_food
                || x.Code == MiniGameCode.Song_word_home
                || x.Code == MiniGameCode.Song_word_nature
                || x.Code == MiniGameCode.Song_word_objectsclothes);

            bool hasAlphabetSong = AppManager.I.DB.GetActiveMinigames().Any(x => x.Code == MiniGameCode.Song_alphabet);

            var songAudioKeys = new HashSet<string>();
            var prefix = $"{nativeLanguageCode}/Audio/Songs/";

            if (hasSimonSong)
            {
                songAudioKeys.Add($"{prefix}SimonSong_Voice_120");
                songAudioKeys.Add($"{prefix}SimonSong_Voice_140");
                songAudioKeys.Add($"{prefix}SimonSong_Voice_160");
            }

            if (songAudioKeys.Count > 0)
                yield return LoadAssets(songAudioKeys, nativeAudioCache, DebugConfig.I.AddressablesBlockingLoad);

            prefix = $"{learningLanguageCode}/Audio/Songs/";
            songAudioKeys = new HashSet<string>();
            if (AppManager.I.DB.GetActiveMinigames().Any(x => x.Code == MiniGameCode.Song_alphabet))
            {
                songAudioKeys.Add($"{prefix}AlphabetSong");
            }
            //songAudioKeys.Add($"{prefix}DiacriticSong");

            if (hasSimonSong)
            {
                // @note: the intro is always the same regardless of the language, so it is saved in Common
                songAudioKeys.Add($"common/Audio/Songs/SimonSong_Intro_120");
                songAudioKeys.Add($"common/Audio/Songs/SimonSong_Intro_140");
                songAudioKeys.Add($"common/Audio/Songs/SimonSong_Intro_160");

                songAudioKeys.Add($"common/Audio/Songs/SimonSong_MusicFirstHalf_120");
                songAudioKeys.Add($"common/Audio/Songs/SimonSong_MusicFirstHalf_140");
                songAudioKeys.Add($"common/Audio/Songs/SimonSong_MusicFirstHalf_160");

                songAudioKeys.Add($"common/Audio/Songs/SimonSong_MusicSecondHalf_120");
                songAudioKeys.Add($"common/Audio/Songs/SimonSong_MusicSecondHalf_140");
                songAudioKeys.Add($"common/Audio/Songs/SimonSong_MusicSecondHalf_160");

                songAudioKeys.Add($"{prefix}SimonSong_Voice_120");
                songAudioKeys.Add($"{prefix}SimonSong_Voice_140");
                songAudioKeys.Add($"{prefix}SimonSong_Voice_160");
            }

            if (songAudioKeys.Count > 0)
                yield return LoadAssets(songAudioKeys, audioCache, DebugConfig.I.AddressablesBlockingLoad);

            var songTextKeys = new HashSet<string>();
            prefix = $"{learningLanguageCode}/Audio/Songs/";
            if (hasAlphabetSong)
            {
                songTextKeys.Add($"{prefix}AlphabetSong.akr");
            }
            //songTextKeys.Add($"{prefix}DiacriticSong.akr");
            if (songTextKeys.Count > 0)
                yield return LoadAssets(songTextKeys, textCache, DebugConfig.I.AddressablesBlockingLoad);
        }

        private void ClearCache<T>(Dictionary<string, T> cache) where T : UnityEngine.Object
        {
            foreach (var keyValuePair in cache)
            {
                try
                {
                    Addressables.Release(keyValuePair.Value);
                }
                catch (Exception e)
                {
                    if (VERBOSE)
                        Debug.LogWarning(e.ToString());
                }
            }
            cache.Clear();
        }

        private IEnumerator LoadAssets<T>(HashSet<string> keys, Dictionary<string, T> cache, bool sync = false) where T : UnityEngine.Object
        {
            int n = 0;
            if (VERBOSE)
                Debug.Log($"Loading {keys.Count} (first is {keys.FirstOrDefault()}");
            var op = Addressables.LoadAssetsAsync<T>(keys, obj =>
                {
                    cache[obj.name] = obj;
                    n++;
                }, Addressables.MergeMode.Union);

            while (!op.IsDone)
            {
                if (sync)
                    op.WaitForCompletion();
                else
                    yield return null;
            }

            if (VERBOSE)
                Debug.Log($"Found {n} items");
        }

        public T Get<T>(Dictionary<string, T> cache, string key)
        {
            if (!cache.ContainsKey(key))
            {
                Debug.LogError($"No {typeof(T).Name} '{key}' found");
                return default(T);
            }
            return cache[key];
        }

        public Sprite GetSprite(string type, string suffix)
        {
            var key = $"minigame_{type}_{suffix}";
            return Get(spriteCache, key);
        }

        public Sprite GetMainIcon(MiniGameData data)
        {
            return GetSprite("Ico", data.Main);
        }
        public Sprite GetBadgeIcon(MiniGameData data)
        {
            return GetSprite("BadgeIco", data.Badge);
        }

        public ShapeLetterData GetShapeLetterData(LetterData letterData)
        {
            var unicode = letterData.GetUnicode();
            if (letterData.Kind == LetterDataKind.DiacriticCombo)
                unicode += $"_{letterData.Symbol_Unicode}";
            return Get(shapeDataCache, $"shapedata_{unicode}");
        }

        public TextAsset GetSongSrt(string id)
        {
            return Get(textCache, id);
        }

        public AudioClip GetSongClip(string id)
        {
            return Get(audioCache, id);
        }

        public AudioClip GetNativeSongClip(string id)
        {
            return Get(nativeAudioCache, id);
        }

    }
}
