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

        public IEnumerator PreloadDataCO()
        {
            // First release preloaded data
            foreach (var keyValuePair in spriteCache)
            {
                Addressables.Release(keyValuePair.Value);
            }
            spriteCache.Clear();


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

            foreach (var key in iconKeys)
            {
                //Debug.Log("READING:  " + key);
                var co = AssetLoader.ValidateAndLoad<Sprite>(key, obj =>
                {
                    //Debug.Log("Found: " + obj);
                    spriteCache[obj.name] = obj;
                });
                yield return co;
                //while(co.MoveNext()){}  // blocking load
            }

            // Badges
            if (VERBOSE) Debug.Log("[Assets] Preloading Badges");
            var badgeKeys = new HashSet<string>();
            foreach (var miniGameData in AppManager.I.DB.GetAllMiniGameData())
            {
                string spriteName = $"minigame_BadgeIco_{miniGameData.Badge}";
                badgeKeys.Add($"{languageCode}/Images/GameIcons/{spriteName}[{spriteName}]");
            }

            foreach (var key in badgeKeys)
            {
                //Debug.Log("READING:  " + key);
                var co = AssetLoader.ValidateAndLoad<Sprite>(key, obj =>
                {
                    //Debug.Log("Found: " + obj);
                    spriteCache[obj.name] = obj;
                });
                yield return co;
                //while(co.MoveNext()){}  // blocking load
            }

            // @note: this won't work, for some reason, even if the single loading works
            /*
            badgeKeys = new HashSet<string>(){badgeKeys.First()};
            Debug.LogError("LOADING " + badgeKeys.First());
            var opBadges =
                Addressables.LoadAssetsAsync<Sprite>(badgeKeys, obj => {
                    Debug.Log(obj.name);
                    //spriteCache[obj.name] = obj;
                });
            yield return opBadges.Task;
            */
        }

        public Sprite GetSprite(string type, string suffix)
        {
            var key = $"minigame_{type}_{suffix}";
            if (!spriteCache.ContainsKey(key))
            {
                Debug.LogError($"No sprite '{key}' found");
                return null;
            }
            return spriteCache[key];
        }

        public Sprite GetMainIcon(MiniGameData data)
        {
            return GetSprite("Ico", data.Main);
        }
        public Sprite GetBadgeIcon(MiniGameData data)
        {
            return GetSprite("BadgeIco", data.Badge);
        }
    }
}