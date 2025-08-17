using Antura.Profile;
using System;
using System.Collections.Generic;
using UnityEngine;

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

    [Serializable]
    public class DiscoverPlayerProfile
    {
        public int schemaVersion = 1;

        public Metadata metadata = new();
        public ProfileHeader profile = new();

        public Settings settings = new();
        public Wallet wallet = new();         // spendable
        public LevelProgress level = new();   // non-spendable + milestones + ledger

        public Avatar avatar = new();
        public Rewards rewards = new();

        public ProfileStats stats = new();
        public Dictionary<string, CardState> cards = new();
        public Dictionary<string, AchievementState> achievements = new();
    }

    [Serializable]
    public class Metadata
    {
        public string createdUtc;
        public string lastUpdatedUtc;
        public string platform;   // "mobile" | "desktop"
        public string appVersion;
    }

    [Serializable]
    public class ProfileHeader
    {
        public string id;
        public string uuid;
        public string displayName;
        public string locale;       // e.g., "it-IT"
        public string countryIso2;  // e.g., "PL"
        public int classroom;       // 0 => none
        public bool easyMode;
        public TalkToPlayerMode talkToPlayerStyle;

        // new
        public bool godMode = false;

        // new — store INTs; colors serialized as RGBA
        public DiscoverPlayerIcon playerIcon = new();
    }

    [Serializable]
    public class DiscoverPlayerIcon
    {
        public int gender = (int)PlayerGender.Undefined;
        public int tint = (int)PlayerTint.None;
        public ColorRGBA skinColor = Color.white;
        public ColorRGBA hairColor = Color.black;
        public ColorRGBA bgColor = new Color(0.1f, 0.1f, 0.1f);
    }

    [Serializable] public class Settings { public AudioSettings audio = new(); }
    [Serializable] public class AudioSettings { public bool musicMuted = false; public bool sfxMuted = false; }

    // spendable currency
    [Serializable] public class Wallet { public int cookies; }

    // non-spendable progress + ledger
    [Serializable]
    public class LevelProgress
    {
        public int points;      // XP-like, non-spendable
        public int gems;        // non-spendable (knowledge level), sum of claims
        public List<GemTokenClaim> gemClaims = new();
        public List<int> xpMilestonesClaimed = new();   // e.g., 1_000_000
        public List<int> gemMilestonesClaimed = new();  // e.g., 10
    }

    [Serializable]
    public class Avatar
    {
        public string activeSpecies = "cat";
        public Dictionary<string, SpeciesAvatar> species = new() { { "cat", new SpeciesAvatar() } };
    }
    [Serializable] public class SpeciesAvatar { public string prefabId = ""; public List<string> equippedProps = new(); }
    [Serializable] public class Rewards { public List<string> ownedItemIds = new(); public List<string> unlockedItemIds = new(); }

    [Serializable]
    public class ProfileStats
    {
        public Totals totals = new();
        public Dictionary<string, QuestStats> quests = new();
        public Dictionary<string, ActivityStats> activities = new();
    }
    [Serializable] public class Totals { public int timePlayedSec; public int sessions; public int answersAttempted; public int answersCorrect; }
    [Serializable]
    public class QuestStats
    {
        public int plays, completions, bestScore, lastScore, sumScore, timeSec, bestStars, lastStars;
        public string firstPlayedUtc, lastPlayedUtc;
    }
    [Serializable]
    public class ActivityStats
    {
        public int plays, wins, bestScore, lastScore, sumScore, timeSec;
        public DidacticKpi didactic = new();
    }
    [Serializable] public class DidacticKpi { public string topic; public int attempts, correct; public Dictionary<string, int> wrongItems = new(); }

    [Serializable]
    public class CardState
    {
        public bool unlocked;
        public string firstSeenUtc, lastSeenUtc;
        public int interactions, streakCorrect;
        public AnswerCount answered = new();
        public float mastery01; // 0..1
    }
    [Serializable] public class AnswerCount { public int correct, wrong; }
    [Serializable] public class AchievementState { public bool unlocked; public int progress; public string unlockedUtc; }

    public enum GemTokenType
    {
        Unknown = 0, CardCollect = 1, QuestStars = 2, Achievement = 3, MilestoneXP = 4,
        MilestoneGems = 5, ManualGrant = 6, LegacyCarry = 7
    }
    [Serializable]
    public class GemTokenClaim
    {
        public string id;            // e.g., "card:{cardId}", "quest:{questId}:stars", "xp:{threshold}"
        public GemTokenType type;
        public string sourceId;      // cardId / questId / achievementId / "system"
        public int amount;           // usually 1..3
        public string claimedUtc;    // ISO-8601
        public string note;          // optional
    }

    [Serializable]
    public struct ColorRGBA
    {
        public float r, g, b, a;

        public ColorRGBA(float r, float g, float b, float a = 1f)
        { this.r = r; this.g = g; this.b = b; this.a = a; }

        public static ColorRGBA From(Color c) => new ColorRGBA(c.r, c.g, c.b, c.a);
        public Color ToColor() => new Color(r, g, b, a);

        public static implicit operator ColorRGBA(Color c) => From(c);
        public static implicit operator Color(ColorRGBA c) => c.ToColor();
    }
}
