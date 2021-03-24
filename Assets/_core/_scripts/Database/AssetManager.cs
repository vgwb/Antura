using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Antura
{
    public class AssetManager
    {
        public static bool VERBOSE = true;

        private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
        private Dictionary<string, SideLetterData> sideDataCache = new Dictionary<string, SideLetterData>();
        private Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();
        private Dictionary<string, TextAsset> textCache = new Dictionary<string, TextAsset>();

        public IEnumerator PreloadDataCO()
        {
            const bool blockingLoad = true;

            // First release preloaded data
            ClearCache(spriteCache);
            ClearCache(sideDataCache);
            ClearCache(audioCache);
            ClearCache(textCache);

            // Pre-load images and icons for the wanted language
            var languageCode = LanguageSwitcher.I.GetLangConfig(LanguageUse.Learning).Code;

            // Icons
            if (VERBOSE) Debug.Log("[Assets] Preloading Icons");
            var iconKeys = new HashSet<string>();
            foreach (var miniGameData in AppManager.I.DB.GetAllMiniGameData())
            {
                string spriteName = $"minigame_Ico_{miniGameData.Main}";
                iconKeys.Add($"{languageCode}/Images/GameIcons/{spriteName}[{spriteName}]");
            }
            yield return LoadAssets(iconKeys, spriteCache, blockingLoad);

            // Badges
            if (VERBOSE) Debug.Log("[Assets] Preloading Badges");
            var badgeKeys = new HashSet<string>();
            foreach (var miniGameData in AppManager.I.DB.GetAllMiniGameData())
            {
                string spriteName = $"minigame_BadgeIco_{miniGameData.Badge}";
                badgeKeys.Add($"{languageCode}/Images/GameIcons/{spriteName}[{spriteName}]");
            }
            yield return LoadAssets(badgeKeys, spriteCache, blockingLoad);

            // Side data
            if (VERBOSE) Debug.Log("[Assets] Preloading Side Data");
            var sideKeys = new HashSet<string>();
            foreach (var letterData in AppManager.I.DB.GetAllLetterData())
            {
                sideKeys.Add($"{languageCode}/SideData/Letters/sideletter_{letterData.Id}");
            }
            yield return LoadAssets(sideKeys, sideDataCache, blockingLoad);

            /*
            // Song data
            // TODO:
            if (VERBOSE) Debug.Log("[Assets] Preloading Song Data");
            var songAudioKeys = new HashSet<string>();
            // TODO: we also need to load keys for the NATIVE songs too as a fallback
            yield return LoadAssets(songAudioKeys, audioCache);
            */

            //var songTextKeys = new HashSet<string>();
            //yield return LoadAssets(songTextKeys, textCache, blockingLoad);
        }

        private void ClearCache<T>(Dictionary<string, T> cache) where T : UnityEngine.Object
        {
            foreach (var keyValuePair in cache) Addressables.Release(keyValuePair.Value);
            cache.Clear();
        }

        private IEnumerator LoadAssets<T>(HashSet<string> keys, Dictionary<string,T> cache, bool blocking) where T : UnityEngine.Object
        {
            int n = 0;
            // Debug.LogError("Loading " + keys.Count);
            var op =
                Addressables.LoadAssetsAsync<T>(keys, obj => {
                    cache[obj.name] = obj;
                    n++;
                }, Addressables.MergeMode.Union);
            yield return op;
            //Debug.LogError("Found " + n + " items");
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
            return Get(sideDataCache, id);
        }

        public TextAsset GetSongSrt(string id)
        {
            return Get(textCache, id);
        }

        public AudioClip GetSongClip(string id)
        {
            //var langDir = LanguageSwitcher.I.GetLangConfig(LanguageUse.Learning).Code.ToString();
            //string completePath = $"{langDir}/Audio/Songs/{id}{suffix}";
            return Get(audioCache, id);
            //Debug.LogError("At path " +  completePath + " READ " + res);
/*
            // Fallback to native song (TODO: must load these too then!)
            if (res == null)
            {
                langDir = LanguageSwitcher.I.GetLangConfig(LanguageUse.Native).Code.ToString();
                completePath = $"{langDir}/Audio/Songs/{id}{suffix}";
                res = Resources.Load(completePath) as T;
                //Debug.LogError("OTHER READ " + res);
            }
            */
        }

    }
}