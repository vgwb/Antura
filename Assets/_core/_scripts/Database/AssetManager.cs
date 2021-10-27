using System;
using System.Collections;
using System.Collections.Generic;
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
        public static bool VERBOSE = false;

        private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
        private Dictionary<string, SideLetterData> sideDataCache = new Dictionary<string, SideLetterData>();
        private Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();
        private Dictionary<string, TextAsset> textCache = new Dictionary<string, TextAsset>();
        private Dictionary<string, AudioClip> nativeAudioCache = new Dictionary<string, AudioClip>();

        public IEnumerator PreloadDataCO()
        {
            // First release preloaded data
            ClearCache(spriteCache);
            ClearCache(sideDataCache);
            ClearCache(audioCache);
            ClearCache(nativeAudioCache);
            ClearCache(textCache);

            // Pre-load images and icons for the wanted language
            var learningLanguageCode = LanguageSwitcher.I.GetLangConfig(LanguageUse.Learning).Code;
            var nativeLanguageCode = LanguageSwitcher.I.GetLangConfig(LanguageUse.Native).Code;

            // Icons
            if (VERBOSE) Debug.Log("[Assets] Preloading Icons");
            var iconKeys = new HashSet<string>();
            foreach (var miniGameData in AppManager.I.DB.GetAllMiniGameData())
            {
                string spriteName = $"minigame_Ico_{miniGameData.Main}";
                iconKeys.Add($"{learningLanguageCode}/Images/GameIcons/{spriteName}[{spriteName}]");
            }
            yield return LoadAssets(iconKeys, spriteCache, AppManager.BlockingLoad);

            // Badges
            if (VERBOSE) Debug.Log("[Assets] Preloading Badges");
            var badgeKeys = new HashSet<string>();
            foreach (var miniGameData in AppManager.I.DB.GetAllMiniGameData())
            {
                string spriteName = $"minigame_BadgeIco_{miniGameData.Badge}";
                badgeKeys.Add($"{learningLanguageCode}/Images/GameIcons/{spriteName}[{spriteName}]");
            }
            yield return LoadAssets(badgeKeys, spriteCache, AppManager.BlockingLoad);

            // Side data
            if (VERBOSE) Debug.Log("[Assets] Preloading Side Data");
            var sideKeys = new HashSet<string>();
            foreach (var letterData in AppManager.I.DB.GetAllLetterData())
            {
                sideKeys.Add($"{learningLanguageCode}/SideData/Letters/sideletter_{letterData.Id}");
            }
            yield return LoadAssets(sideKeys, sideDataCache, AppManager.BlockingLoad);


            // Song data
            if (VERBOSE) Debug.Log("[Assets] Preloading Song Data");
            var songAudioKeys = new HashSet<string>();

            var prefix = $"{nativeLanguageCode}/Audio/Songs/";
            songAudioKeys.Add($"{prefix}SimonSong_Part_120");
            songAudioKeys.Add($"{prefix}SimonSong_Part_140");
            songAudioKeys.Add($"{prefix}SimonSong_Part_160");
            yield return LoadAssets(songAudioKeys, nativeAudioCache, AppManager.BlockingLoad);

            prefix = $"{learningLanguageCode}/Audio/Songs/";
            songAudioKeys = new HashSet<string>();
            songAudioKeys.Add($"{prefix}AlphabetSong");
            //songAudioKeys.Add($"{prefix}DiacriticSong");

            // TODO: the intro should be loaded regardless of language (it's always the same)
            songAudioKeys.Add($"{prefix}SimonSong_Intro_120");
            songAudioKeys.Add($"{prefix}SimonSong_Intro_140");
            songAudioKeys.Add($"{prefix}SimonSong_Intro_160");

            songAudioKeys.Add($"{prefix}SimonSong_Part_120");
            songAudioKeys.Add($"{prefix}SimonSong_Part_140");
            songAudioKeys.Add($"{prefix}SimonSong_Part_160");
            yield return LoadAssets(songAudioKeys, audioCache, AppManager.BlockingLoad);

            var songTextKeys = new HashSet<string>();
            prefix = $"{learningLanguageCode}/Audio/Songs/";
            songTextKeys.Add($"{prefix}AlphabetSong.akr");
            //songTextKeys.Add($"{prefix}DiacriticSong.akr");
            yield return LoadAssets(songTextKeys, textCache, AppManager.BlockingLoad);
        }

        private void ClearCache<T>(Dictionary<string, T> cache) where T : UnityEngine.Object
        {
            foreach (var keyValuePair in cache) Addressables.Release(keyValuePair.Value);
            cache.Clear();
        }

        private IEnumerator LoadAssets<T>(HashSet<string> keys, Dictionary<string,T> cache, bool sync = false) where T : UnityEngine.Object
        {
            int n = 0;
            if (VERBOSE) Debug.Log($"Loading {keys.Count}");

            AsyncOperationHandle<IList<T>> op = default;
            try
            {
                op =
                    Addressables.LoadAssetsAsync<T>(keys, obj =>
                    {
                        cache[obj.name] = obj;
                        n++;
                    }, Addressables.MergeMode.Union);

            }
            catch (Exception e)
            {
                Debug.LogError("Asset reading exception: " + e.Message);
            }

            while (!op.IsDone)
            {
                if (sync) op.WaitForCompletion();
                else yield return null;
            }

            //yield return op;
            if (VERBOSE) Debug.Log($"Found {n} items");
        }

        public T Get<T>(Dictionary<string,T> cache, string key)
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

        public SideLetterData GetSideLetterData(string id)
        {
            return Get(sideDataCache, $"sideletter_{id}");
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