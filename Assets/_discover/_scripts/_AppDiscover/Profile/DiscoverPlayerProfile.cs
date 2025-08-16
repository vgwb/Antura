using System;
using System.Collections.Generic;

namespace Antura.Discover
{
    public enum TalkToPlayerMode
    {
        DontTalk = 0,
        NativeOnly = 1,
        LearningLanguageOnly = 2,
        LearningThenNative = 3,
        NativeThenLearning = 4
    }

    /// <summary>
    /// The main class for the new Discover player profile.
    /// This is a simplified version of the old AnturaPlayerProfile, focusing on future proofing and extensibility.
    /// </summary>
    [Serializable]
    public class DiscoverPlayerProfile
    {
        public int schemaVersion = 1;

        public Metadata metadata = new();
        public ProfileHeader profile = new();

        public Settings settings = new();
        public Currency currency = new();

        public Avatar avatar = new();
        public Rewards rewards = new();

        public ProfileStats stats = new();

        // Per-card mastery/interactions; absent cardId means "never seen".
        public Dictionary<string, CardState> cards = new();

        // Achievement states keyed by achievementId (definitions live as ScriptableObjects).
        public Dictionary<string, AchievementState> achievements = new();
    }

    [Serializable]
    public class Metadata
    {
        public string createdUtc;     // ISO-8601, e.g., 2025-08-16T12:00:00Z
        public string lastUpdatedUtc; // ISO-8601
        public string platform;       // "mobile" | "desktop"
        public string appVersion;
    }

    [Serializable]
    public class ProfileHeader
    {
        public string id;               // incremental, e.g., "player_01"
        public string uuid;             // legacy UUID mapping
        public string displayName;      // shown in UI lists
        public string locale;           // e.g., "it"
        public string countryIso2;      // e.g., "PL" or ""
        public int classroom;           // for the Classroom mode, 0 means not in a classroom
        public bool easyMode;           // player is forced to easymode by teacher
        public TalkToPlayerMode talkToPlayerStyle;
    }

    [Serializable]
    public class Settings
    {
        public AudioSettings audio = new();
    }

    [Serializable]
    public class AudioSettings
    {
        public bool musicMuted = false;
        public bool sfxMuted = false;
    }

    [Serializable]
    public class Currency
    {
        public int points;   // general score
        public int gems;     // rewarded by achievements
        public int cookies;  // soft currency for shop
    }

    // ===== Avatar / Rewards =====

    [Serializable]
    public class Avatar
    {
        public string activeSpecies = "cat"; // future-proof: "dog", "fox", etc.
        public Dictionary<string, SpeciesAvatar> species = new()
        {
            { "cat", new SpeciesAvatar() }
        };
    }

    [Serializable]
    public class SpeciesAvatar
    {
        public string prefabId = ""; // e.g., "catV1"
        public List<string> equippedProps = new(); // e.g., "cat/head/hat_chef"
    }

    [Serializable]
    public class Rewards
    {
        public List<string> ownedItemIds = new();    // purchased/granted items
        public List<string> unlockedItemIds = new(); // visible in shop but not owned yet
    }

    // ===== Stats =====

    [Serializable]
    public class ProfileStats
    {
        public Totals totals = new();
        public Dictionary<string, QuestStats> quests = new();      // questId -> stats (absent => never played/locked)
        public Dictionary<string, ActivityStats> activities = new(); // activityId -> stats
    }

    [Serializable]
    public class Totals
    {
        public int timePlayedSec;
        public int sessions;
        public int answersAttempted;
        public int answersCorrect;
    }

    [Serializable]
    public class QuestStats
    {
        public int plays;
        public int completions;
        public int bestScore;
        public int lastScore;
        public int sumScore;
        public int timeSec;
        public int bestStars;     // 0..3, maximum earned
        public int lastStars;     // 0..3, last run
        public string firstPlayedUtc;
        public string lastPlayedUtc;
    }

    [Serializable]
    public class ActivityStats
    {
        public int plays;
        public int wins;
        public int bestScore;
        public int lastScore;
        public int sumScore;
        public int timeSec;
        public DidacticKpi didactic = new();
    }

    [Serializable]
    public class DidacticKpi
    {
        public string topic; // e.g., vocabulary domain / note name
        public int attempts;
        public int correct;
        public Dictionary<string, int> wrongItems = new(); // sparse per-item wrong counts
    }

    [Serializable]
    public class CardState
    {
        public bool unlocked;
        public string firstSeenUtc;
        public string lastSeenUtc;
        public int interactions;
        public AnswerCount answered = new();
        public int streakCorrect;
        public float mastery01; // normalized 0..1
    }

    [Serializable]
    public class AnswerCount
    {
        public int correct;
        public int wrong;
    }

    [Serializable]
    public class AchievementState
    {
        public bool unlocked;
        public int progress; // 0..100, e.g., for progress-based achievements
        public string unlockedUtc; // optional: when the unlock happened
    }
}
