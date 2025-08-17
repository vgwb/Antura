using System;
using System.Collections.Generic;
using UnityEngine;

// HOT TO USE
// create or load your profile (v7)
// var profile = new DiscoverPlayerProfile { schemaVersion = 7 };
// drag your EconomySettings asset in via inspector or Resources.Load
// /* EconomySettings econ = /* ref to asset */ null;

// var svc = new Antura.Discover.ProfileService(profile, econ);

// // POINTS (XP)
// svc.AddPoints(500_000);
// svc.AddPoints(600_000);   // crosses 1,000,000 → adds 1 gem claim if configured

// // QUEST run (also syncs star→gem delta)
// svc.RecordQuestRun("PL_WAW_RIVER", score: 920, stars: 3, timeSec: 300);

// // CARD
// svc.RecordCardSeen("CARD_APPLE");
// svc.RecordCardAnswer("CARD_APPLE", correct: true);
// svc.ClaimCardGem("CARD_APPLE", gemValue: 1);   // only first time adds a claim

// // ACTIVITY
// svc.RecordActivityRun("Piano01", win:true, score:95, timeSec:60, topic:"notes", attempts:10, correct:8);

// // (Optional) ensure totals
// svc.RecalculateGemsFromLedger();


namespace Antura.Discover
{
    /// <summary>
    /// Minimal helper around DiscoverPlayerProfile:
    /// - points & XP milestones → gem claims
    /// - card gem claims (once)
    /// - quest star gem deltas
    /// - quest/activity stats updates
    /// </summary>
    public class ProfileService
    {
        public DiscoverPlayerProfile P { get; private set; }
        readonly EconomySettings economy;

        public ProfileService(DiscoverPlayerProfile profile, EconomySettings economySettings = null)
        {
            P = profile ?? throw new ArgumentNullException(nameof(profile));
            economy = economySettings;

            if (P.level == null)
                P.level = new LevelProgress();
            if (P.wallet == null)
                P.wallet = new Wallet();
            if (P.metadata == null)
                P.metadata = new Metadata();
            if (P.profile == null)
                P.profile = new ProfileHeader();
            if (P.stats == null)
                P.stats = new ProfileStats();
            if (P.cards == null)
                P.cards = new Dictionary<string, CardState>();
            if (P.achievements == null)
                P.achievements = new Dictionary<string, AchievementState>();
            if (P.avatar == null)
                P.avatar = new Avatar();
            if (P.rewards == null)
                P.rewards = new Rewards();

            if (P.level.gemClaims == null)
                P.level.gemClaims = new List<GemTokenClaim>();
            if (P.level.xpMilestonesClaimed == null)
                P.level.xpMilestonesClaimed = new List<int>();
            if (P.level.gemMilestonesClaimed == null)
                P.level.gemMilestonesClaimed = new List<int>();

            if (P.stats.totals == null)
                P.stats.totals = new Totals();
            if (P.stats.quests == null)
                P.stats.quests = new Dictionary<string, QuestStats>();
            if (P.stats.activities == null)
                P.stats.activities = new Dictionary<string, ActivityStats>();
        }

        // -----------------------------
        // Points / Gems / Milestones
        // -----------------------------

        /// <summary>Add points (XP-like). Checks XP milestones and awards gem claims if thresholds crossed.</summary>
        public AwardResult AddPoints(int delta)
        {
            if (delta == 0)
                return AwardResult.None;
            int before = P.level.points;
            P.level.points = Mathf.Max(0, before + delta);

            int newGems = 0;
            var newClaims = new List<GemTokenClaim>();

            if (economy != null && economy.xpMilestones != null && economy.xpMilestones.Length > 0)
            {
                foreach (var thr in economy.xpMilestones)
                {
                    if (before < thr && P.level.points >= thr && !P.level.xpMilestonesClaimed.Contains(thr))
                    {
                        P.level.xpMilestonesClaimed.Add(thr);
                        int amt = Mathf.Max(0, economy.xpMilestoneGemAward);
                        if (amt > 0)
                        {
                            var token = MakeToken($"xp:{thr}", GemTokenType.MilestoneXP, "system", amt, "XP milestone");
                            AddGemToken(token);
                            newClaims.Add(token);
                            newGems += amt;
                        }
                    }
                }
            }

            return new AwardResult(newGems, newClaims);
        }

        /// <summary>Claim gem(s) for collecting a card (only once per cardId).</summary>
        public AwardResult ClaimCardGem(string cardId, int gemValue)
        {
            if (string.IsNullOrEmpty(cardId) || gemValue <= 0)
                return AwardResult.None;
            string id = $"card:{cardId}";
            if (HasExactToken(id))
                return AwardResult.None;

            var token = MakeToken(id, GemTokenType.CardCollect, cardId, gemValue, "Card collect");
            AddGemToken(token);
            return new AwardResult(gemValue, token);
        }

        /// <summary>
        /// Ensure quest star gems are up to date: if bestStars improved (capped), add a delta token.
        /// Token id pattern: quest:{questId}:stars:{newBest} so we never double-claim the same step.
        /// </summary>
        public AwardResult SyncQuestStarGems(string questId, int bestStarsNow)
        {
            if (string.IsNullOrEmpty(questId))
                return AwardResult.None;
            int cap = economy != null ? economy.GetQuestStarCap(questId) : 3;
            int target = Mathf.Clamp(bestStarsNow, 0, cap);

            // Sum previously claimed stars for this quest (prefix scan)
            int claimed = 0;
            string prefix = $"quest:{questId}:stars";
            for (int i = 0; i < P.level.gemClaims.Count; i++)
            {
                var t = P.level.gemClaims[i];
                if (t.type == GemTokenType.QuestStars && t.id.StartsWith(prefix, StringComparison.Ordinal))
                    claimed += Mathf.Max(0, t.amount);
            }

            int delta = target - claimed;
            if (delta <= 0)
                return AwardResult.None;

            string id = $"quest:{questId}:stars:{target}";
            if (HasExactToken(id))
                return AwardResult.None; // already added this step

            var token = MakeToken(id, GemTokenType.QuestStars, questId, delta, "Quest stars delta");
            AddGemToken(token);
            return new AwardResult(delta, token);
        }

        /// <summary>Claim gem(s) for an achievement (only once per achievementId).</summary>
        public AwardResult ClaimAchievementGem(string achievementId, int amount)
        {
            if (string.IsNullOrEmpty(achievementId) || amount <= 0)
                return AwardResult.None;
            string id = $"ach:{achievementId}";
            if (HasExactToken(id))
                return AwardResult.None;

            var token = MakeToken(id, GemTokenType.Achievement, achievementId, amount, "Achievement");
            AddGemToken(token);
            return new AwardResult(amount, token);
        }

        /// <summary>Manual grant (tools/debug/recovery). Generates a unique id.</summary>
        public AwardResult GrantManualGems(int amount, string note = "Manual")
        {
            if (amount <= 0)
                return AwardResult.None;
            string guid = Guid.NewGuid().ToString("N");
            var token = MakeToken($"manual:{guid}", GemTokenType.ManualGrant, "system", amount, note);
            AddGemToken(token);
            return new AwardResult(amount, token);
        }

        /// <summary>Recompute total gems from ledger (use if you ever edited gemClaims externally).</summary>
        public int RecalculateGemsFromLedger()
        {
            int sum = 0;
            for (int i = 0; i < P.level.gemClaims.Count; i++)
                sum += Mathf.Max(0, P.level.gemClaims[i].amount);
            P.level.gems = sum;
            return sum;
        }

        // -----------------------------
        // Quests / Activities / Cards stats
        // -----------------------------

        /// <summary>Record a quest run and auto-sync star gems delta.</summary>
        public AwardResult RecordQuestRun(string questId, int score, int stars, int timeSec)
        {
            if (string.IsNullOrEmpty(questId))
                return AwardResult.None;

            var s = GetOrCreateQuest(questId);
            s.plays++;
            s.lastScore = score;
            s.sumScore += Mathf.Max(0, score);
            s.timeSec += Mathf.Max(0, timeSec);
            s.lastStars = Mathf.Clamp(stars, 0, 3);
            s.bestScore = Mathf.Max(s.bestScore, score);
            s.bestStars = Mathf.Max(s.bestStars, s.lastStars);
            var now = NowIso();
            if (string.IsNullOrEmpty(s.firstPlayedUtc))
                s.firstPlayedUtc = now;
            s.lastPlayedUtc = now;
            if (stars > 0)
                s.completions++; // consider completion only if >=1 star

            return SyncQuestStarGems(questId, s.bestStars);
        }

        /// <summary>Record an activity attempt with didactic KPIs.</summary>
        public void RecordActivityRun(string activityId, bool win, int score, int timeSec,
                                      string topic = null, int attempts = 0, int correct = 0,
                                      Dictionary<string, int> wrongItems = null)
        {
            if (string.IsNullOrEmpty(activityId))
                return;
            var s = GetOrCreateActivity(activityId);
            s.plays++;
            if (win)
                s.wins++;
            s.lastScore = score;
            s.sumScore += Mathf.Max(0, score);
            s.timeSec += Mathf.Max(0, timeSec);
            s.bestScore = Mathf.Max(s.bestScore, score);

            if (!string.IsNullOrEmpty(topic))
                s.didactic.topic = topic;
            s.didactic.attempts += Mathf.Max(0, attempts);
            s.didactic.correct += Mathf.Max(0, correct);
            if (wrongItems != null)
            {
                foreach (var kv in wrongItems)
                {
                    if (string.IsNullOrEmpty(kv.Key))
                        continue;
                    if (!s.didactic.wrongItems.TryGetValue(kv.Key, out int cur))
                        cur = 0;
                    s.didactic.wrongItems[kv.Key] = cur + Mathf.Max(0, kv.Value);
                }
            }
        }

        /// <summary>Mark that a card was seen / interacted with.</summary>
        public void RecordCardSeen(string cardId)
        {
            if (string.IsNullOrEmpty(cardId))
                return;
            var cs = GetOrCreateCard(cardId);
            var now = NowIso();
            if (string.IsNullOrEmpty(cs.firstSeenUtc))
                cs.firstSeenUtc = now;
            cs.lastSeenUtc = now;
            cs.unlocked = true;
        }

        /// <summary>Record a card answer; updates mastery and streak.</summary>
        public void RecordCardAnswer(string cardId, bool correct)
        {
            if (string.IsNullOrEmpty(cardId))
                return;
            var cs = GetOrCreateCard(cardId);
            cs.interactions++;
            if (correct)
            { cs.answered.correct++; cs.streakCorrect++; }
            else
            { cs.answered.wrong++; cs.streakCorrect = 0; }
            int total = Mathf.Max(1, cs.answered.correct + cs.answered.wrong);
            cs.mastery01 = Mathf.Clamp01(cs.answered.correct / (float)total);
            cs.lastSeenUtc = NowIso();
        }

        // -----------------------------
        // Small helpers / internals
        // -----------------------------

        public static string NowIso() => DateTime.UtcNow.ToString("o");

        bool HasExactToken(string tokenId)
        {
            for (int i = 0; i < P.level.gemClaims.Count; i++)
                if (P.level.gemClaims[i].id == tokenId)
                    return true;
            return false;
        }

        GemTokenClaim MakeToken(string id, GemTokenType type, string sourceId, int amount, string note)
        {
            return new GemTokenClaim
            {
                id = id,
                type = type,
                sourceId = sourceId ?? string.Empty,
                amount = Mathf.Max(0, amount),
                claimedUtc = NowIso(),
                note = note ?? string.Empty
            };
        }

        void AddGemToken(GemTokenClaim token)
        {
            if (token.amount <= 0)
                return;
            P.level.gemClaims.Add(token);
            P.level.gems += token.amount;

            // Mark gem milestones as claimed once crossed (no auto-award here).
            if (economy != null && economy.gemMilestones != null)
            {
                foreach (var thr in economy.gemMilestones)
                {
                    if (P.level.gems >= thr && !P.level.gemMilestonesClaimed.Contains(thr))
                        P.level.gemMilestonesClaimed.Add(thr);
                }
            }
        }

        QuestStats GetOrCreateQuest(string questId)
        {
            if (!P.stats.quests.TryGetValue(questId, out var s) || s == null)
            {
                s = new QuestStats();
                P.stats.quests[questId] = s;
            }
            return s;
        }

        ActivityStats GetOrCreateActivity(string activityId)
        {
            if (!P.stats.activities.TryGetValue(activityId, out var s) || s == null)
            {
                s = new ActivityStats();
                P.stats.activities[activityId] = s;
            }
            return s;
        }

        CardState GetOrCreateCard(string cardId)
        {
            if (!P.cards.TryGetValue(cardId, out var cs) || cs == null)
            {
                cs = new CardState();
                P.cards[cardId] = cs;
            }
            return cs;
        }
    }

    /// <summary>Return info when something awarded gems.</summary>
    public struct AwardResult
    {
        public int gemsAdded;
        public GemTokenClaim[] newClaims;
        public bool Any => gemsAdded > 0;

        // Cached empty array for older runtimes (instead of Array.Empty<T>())
        private static readonly GemTokenClaim[] Empty = new GemTokenClaim[0];

        public static AwardResult None => new AwardResult(0, Empty);

        public AwardResult(int gems, GemTokenClaim claim)
        {
            gemsAdded = gems;
            newClaims = claim == null ? Empty : new GemTokenClaim[] { claim };
        }

        public AwardResult(int gems, System.Collections.Generic.List<GemTokenClaim> claims)
        {
            gemsAdded = gems;
            newClaims = (claims != null) ? claims.ToArray() : Empty;
        }

        public AwardResult(int gems, GemTokenClaim[] claims)
        {
            gemsAdded = gems;
            newClaims = claims ?? Empty;
        }
    }
}
