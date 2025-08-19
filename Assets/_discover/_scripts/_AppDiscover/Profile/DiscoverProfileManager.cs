using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Antura.Discover
{

    /// <summary>
    /// On-disk index for all Discover profiles. Keeps the user-visible list and the next incremental number.
    /// </summary>

    [Serializable]
    public class DiscoverProfilesIndex
    {
        public int schemaVersion = 1;
        public int nextNumber = 1; // next "player_NN"
        public List<DiscoverProfileHeader> profiles = new(); // ordered for UI pickers
    }

    [Serializable]
    public class DiscoverProfileHeader
    {
        public string id;          // "player_01"
        public string uuid;        // legacy UUID mapping (from old profile system)
        public string displayName; // quick label for UI
    }

    /// <summary>
    /// - Profiles are JSON files under Application.persistentDataPath / discover_profiles.
    /// - PlayerPrefs holds only a tiny pointer: the current profile id.
    /// </summary>
    public sealed class DiscoverProfileManager
    {
        // ---- PlayerPrefs key for current profile id ----
        public const string CurrentIdPrefsKey = "Discover.CurrentProfileId";

        // ---- Paths / JSON ----
        private readonly string rootDir;
        private readonly string indexPath;
        private readonly JsonSerializerSettings json = new() { NullValueHandling = NullValueHandling.Ignore };

        public DiscoverProfileManager(string subdir = "discover_profiles")
        {
            rootDir = Path.Combine(Application.persistentDataPath, subdir);
            indexPath = Path.Combine(rootDir, "index.json");
            Directory.CreateDirectory(rootDir);
        }

        #region Public API

        /// <summary>Returns the current profile if set, otherwise null.</summary>
        public DiscoverPlayerProfile LoadCurrent()
        {
            var id = PlayerPrefs.GetString(CurrentIdPrefsKey, null);
            return string.IsNullOrEmpty(id) ? null : LoadById(id);
        }

        /// <summary>Set the current profile id in PlayerPrefs (does not load it).</summary>
        public void SetCurrent(string id)
        {
            PlayerPrefs.SetString(CurrentIdPrefsKey, id);
            PlayerPrefs.Save();
        }

        /// <summary>List headers for all profiles (in stored order).</summary>
        public IReadOnlyList<DiscoverProfileHeader> ListProfiles() => LoadIndex().profiles;

        /// <summary>Load a profile by its incremental id ("player_03"), or null if missing/corrupt.</summary>
        public DiscoverPlayerProfile LoadById(string id)
        {
            var path = ProfilePath(id);
            if (!File.Exists(path))
                return null;
            try
            { return JsonConvert.DeserializeObject<DiscoverPlayerProfile>(File.ReadAllText(path)); }
            catch (Exception e) { Debug.LogWarning($"Discover: failed to parse {id}: {e.Message}"); return null; }
        }

        /// <summary>Save a profile to disk (atomic write) and update lastUpdatedUtc.</summary>
        public void Save(DiscoverPlayerProfile p)
        {
            p.metadata.lastUpdatedUtc = DatetimeUtilities.GetNowUtcString();
            var jsonStr = JsonConvert.SerializeObject(p, Formatting.Indented, json);
            AtomicWrite(ProfilePath(p.profile.id), jsonStr);
        }

        /// <summary>Delete a profile by id (file + index). Clears current id if it was active.</summary>
        public bool Delete(string id)
        {
            var index = LoadIndex();
            var removed = index.profiles.RemoveAll(h => h.id == id) > 0;
            if (!removed)
                return false;

            SaveIndex(index);

            var path = ProfilePath(id);
            if (File.Exists(path))
                File.Delete(path);

            if (PlayerPrefs.GetString(CurrentIdPrefsKey, null) == id)
            {
                PlayerPrefs.DeleteKey(CurrentIdPrefsKey);
                PlayerPrefs.Save();
            }
            return true;
        }

        /// <summary>
        /// Core entry: arrive with a legacy UUID (from old PlayerProfile).
        /// - If a Discover profile is linked to that UUID, load it.
        /// - Otherwise, create a fresh Discover profile, assign a new incremental id, store the legacy UUID, and persist.
        /// Optionally pass the legacy PlayerProfile to import a few fields (name, class, easy mode, talk style, native language).
        /// </summary>
        /// <param name="legacyUuid">Legacy profile UUID used as stable mapping.</param>
        /// <param name="legacy">Optional legacy PlayerProfile to import basic fields. Pass null if unavailable.</param>
        /// <param name="platform">"mobile" or "desktop".</param>
        /// <param name="appVersion">Version string (defaults to Application.version if null).</param>
        public DiscoverPlayerProfile LoadOrCreateByLegacyUuid(string legacyUuid, Antura.Profile.PlayerProfile legacy = null, string platform = "mobile", string appVersion = null)
        {
            if (string.IsNullOrEmpty(legacyUuid))
                throw new ArgumentException("legacyUuid is null/empty");

            var index = LoadIndex();

            // 1) Match by legacy UUID
            var header = index.profiles.Find(h => string.Equals(h.uuid, legacyUuid, StringComparison.OrdinalIgnoreCase));
            if (header != null)
            {
                var existing = LoadById(header.id);
                if (existing != null)
                    return existing;

                // Header exists but file is missing/corrupt: recreate with the same id.
                var recreated = CreateNew(legacyUuid, header.displayName, platform, appVersion ?? Application.version, index);
                recreated.profile.id = header.id;
                Save(recreated);
                SaveIndex(index);
                return recreated;
            }

            // 2) Not found: create brand-new discover profile
            var displayName = legacy?.PlayerName ?? $"Player {index.nextNumber}";
            var created = CreateNew(legacyUuid, displayName, platform, appVersion ?? Application.version, index, legacy);
            Save(created);
            SaveIndex(index);
            return created;
        }
        #endregion

        #region Internal API

        private string ProfilePath(string id) => Path.Combine(rootDir, $"{id}.json");

        /// <summary>Atomic write with backup: write to .tmp, replace target, keep .bak.</summary>
        private void AtomicWrite(string path, string contents)
        {
            var tmp = path + ".tmp";
            File.WriteAllText(tmp, contents);
            try
            {
                if (File.Exists(path))
                    File.Replace(tmp, path, path + ".bak");
                else
                    File.Move(tmp, path);
            }
            catch
            {
                // Fallback (older FS): replace via delete+move
                if (File.Exists(path))
                    File.Delete(path);
                File.Move(tmp, path);
            }

            // Keep a copy as last-known-good
            try
            { File.Copy(path, path + ".bak", overwrite: true); }
            catch { /* ignore */ }
        }

        private DiscoverProfilesIndex LoadIndex()
        {
            if (!File.Exists(indexPath))
            {
                var fresh = new DiscoverProfilesIndex();
                SaveIndex(fresh);
                return fresh;
            }
            try
            {
                var idx = JsonConvert.DeserializeObject<DiscoverProfilesIndex>(File.ReadAllText(indexPath));
                if (idx == null)
                    throw new Exception("null index");
                if (idx.nextNumber < 1)
                    idx.nextNumber = 1;
                return idx;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Discover: index.json corrupted, recreating. {e.Message}");
                var fresh = new DiscoverProfilesIndex();
                SaveIndex(fresh);
                return fresh;
            }
        }

        private void SaveIndex(DiscoverProfilesIndex index)
        {
            var jsonStr = JsonConvert.SerializeObject(index, Formatting.Indented, json);
            AtomicWrite(indexPath, jsonStr);
        }

        private DiscoverPlayerProfile CreateNew(string legacyUuid, string displayName, string platform, string appVersion, DiscoverProfilesIndex index, Antura.Profile.PlayerProfile legacy = null)
        {
            var id = $"player_{index.nextNumber:00}";
            index.nextNumber++;
            index.profiles.Add(new DiscoverProfileHeader { id = id, uuid = legacyUuid, displayName = displayName });

            var now = DatetimeUtilities.GetNowUtcString();
            var p = new DiscoverPlayerProfile
            {
                schemaVersion = 2042,

                metadata = new Metadata
                {
                    createdUtc = now,
                    lastUpdatedUtc = now,
                    platform = platform,
                    appVersion = appVersion
                },

                profile = new ProfileHeader
                {
                    id = id,
                    uuid = legacyUuid,
                    displayName = displayName,
                    locale = MapLegacyLanguageToLocale(legacy?.NativeLanguage),
                    countryIso2 = "",
                    classroom = legacy?.Classroom ?? 0,
                    easyMode = legacy?.EasyMode ?? false,
                    talkToPlayerStyle = SafeMapTalkStyle(legacy),
                    godMode = SafeGetGodMode(legacy),
                    playerIcon = MapLegacyPlayerIcon(legacy)
                },

                settings = new Settings
                {
                    audio = new AudioSettings { musicMuted = false, sfxMuted = false }
                },

                wallet = new Wallet { cookies = 0 },

                level = new LevelProgress
                {
                    points = 0,
                    gems = 0,
                    gemClaims = new List<GemTokenClaim>(),
                    xpMilestonesClaimed = new List<int>(),
                    gemMilestonesClaimed = new List<int>()
                },

                avatar = new Avatar(),     // defaults: cat species, no props
                rewards = new Rewards(),   // empty ownership
                stats = new ProfileStats
                {
                    totals = new Totals(),
                    quests = new Dictionary<string, QuestStats>(),
                    activities = new Dictionary<string, ActivityStats>()
                },

                cards = new Dictionary<string, CardState>(),
                achievements = new Dictionary<string, AchievementState>()
            };

            return p;
        }
        #endregion

        // TODO we shoudl have already this utility somewhere
        private string MapLegacyLanguageToLocale(Antura.Language.LanguageCode? code)
        {
            if (!code.HasValue)
                return "en";
            var s = code.Value.ToString();
            return s switch
            {
                "Italian" => "it",
                "English" => "en",
                "Spanish" => "es",
                "French" => "fr",
                "German" => "de",
                "Polish" => "pl",
                "Arabic" => "ar",
                _ => "en"
            };
        }

        // ----------------- helpers -----------------

        private static TalkToPlayerMode SafeMapTalkStyle(Antura.Profile.PlayerProfile legacy)
        {
            if (legacy == null)
                return TalkToPlayerMode.LearningThenNative;
            try
            {
                // If legacy enum underlying value matches, this works:
                var val = Convert.ToInt32(legacy.TalkToPlayerStyle);
                if (Enum.IsDefined(typeof(TalkToPlayerMode), val))
                    return (TalkToPlayerMode)val;

                // Fallback by name
                var name = legacy.TalkToPlayerStyle.ToString();
                if (Enum.TryParse(name, out TalkToPlayerMode parsed))
                    return parsed;
            }
            catch { }
            return TalkToPlayerMode.LearningThenNative;
        }

        private static bool SafeGetGodMode(Antura.Profile.PlayerProfile legacy)
        {
            try
            {
                return (bool)(legacy?.GetType().GetProperty("IsDemoUser")?.GetValue(legacy) ?? false);
            }
            catch { return false; }
        }

        private static DiscoverPlayerIcon MapLegacyPlayerIcon(Antura.Profile.PlayerProfile legacy)
        {
            var icon = new DiscoverPlayerIcon();
            if (legacy == null)
                return icon;

            try
            { icon.gender = Convert.ToInt32(legacy.Gender); }
            catch { icon.gender = (int)Profile.PlayerGender.Undefined; }
            try
            { icon.tint = Convert.ToInt32(legacy.Tint); }
            catch { icon.tint = (int)Profile.PlayerTint.None; }

            try
            { icon.skinColor = legacy.SkinColor; }
            catch { }
            try
            { icon.hairColor = legacy.HairColor; }
            catch { }
            try
            { icon.bgColor = legacy.BgColor; }
            catch { }

            return icon;
        }

    }
}
