using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Antura.Discover.Audio;
using Antura.Utilities;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Antura.Discover
{
    /// <summary>
    /// Centralized data access layer for Discover content: scripted data, localized strings, and audio clips.
    /// Lives alongside <see cref="DiscoverAppManager"/>
    /// </summary>
    public sealed class DiscoverDataManager : SingletonMonoBehaviour<DiscoverDataManager>
    {
        [Header("Localization Tables")]
        [SerializeField]
        private LocalizedAssetTable cardsAudioTable;

        private ICardAudioService _cardAudioService;
        private DiscoverAppManager _app;

        public DatabaseManager Database => DatabaseProvider.I;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        internal void Initialize(DiscoverAppManager app)
        {
            _app = app;
            EnsureCardAudioService();
        }

        void EnsureCardAudioService()
        {
            if (_cardAudioService != null)
                return;

            if (cardsAudioTable != null)
            {
                _cardAudioService = new LocalizedCardAudioService(cardsAudioTable.TableReference);
            }
        }

        /// <summary>
        /// Resolve a card by id from the Discover database.
        /// </summary>
        public CardData GetCard(string cardId)
        {
            if (string.IsNullOrEmpty(cardId))
                return null;
            DatabaseProvider.TryGet(cardId, out CardData data);
            return data;
        }

        /// <summary>
        /// Enumerate all cards currently indexed in the database.
        /// </summary>
        public IEnumerable<CardData> GetAllCards()
        {
            var db = Database;
            return db != null ? db.All<CardData>() : Array.Empty<CardData>();
        }

        /// <summary>
        /// Resolve a topic by id from the Discover database.
        /// </summary>
        public TopicData GetTopic(string topicId)
        {
            if (string.IsNullOrEmpty(topicId))
                return null;
            DatabaseProvider.TryGet(topicId, out TopicData data);
            return data;
        }

        /// <summary>
        /// Enumerate all topics currently indexed in the database.
        /// </summary>
        public IEnumerable<TopicData> GetAllTopics()
        {
            var db = Database;
            return db != null ? db.All<TopicData>() : Array.Empty<TopicData>();
        }

        /// <summary>R
        /// esolve bonus/malus data by id from the Discover database.
        /// </summary>
        public BonusMalusData GetBonusMalus(string bonusMalusId)
        {
            if (string.IsNullOrEmpty(bonusMalusId))
                return null;
            DatabaseProvider.TryGet(bonusMalusId, out BonusMalusData data);
            return data;
        }

        /// <summary>
        /// Enumerate all bonus/malus entries currently indexed in the database.
        /// </summary>
        public IEnumerable<BonusMalusData> GetAllBonusMalus()
        {
            var db = Database;
            return db != null ? db.All<BonusMalusData>() : Array.Empty<BonusMalusData>();
        }

        /// <summary>
        /// Resolve a quest by id from the Discover database.
        /// </summary>
        public QuestData GetQuest(string questId)
        {
            if (string.IsNullOrEmpty(questId))
                return null;
            DatabaseProvider.TryGet(questId, out QuestData data);
            return data;
        }

        /// <summary>
        /// Enumerate all quests currently indexed in the database.
        /// </summary>
        public IEnumerable<QuestData> GetAllQuests()
        {
            var db = Database;
            return db != null ? db.All<QuestData>() : Array.Empty<QuestData>();
        }

        /// <summary>
        /// Get a localized card title string in the target locale (defaults to learning locale).
        /// </summary>
        public string GetCardTitle(CardData card, Locale locale = null, FallbackBehavior fallback = FallbackBehavior.UseFallback)
        {
            if (card == null || card.Title == null || card.Title.IsEmpty)
                return string.Empty;
            try
            {
                return LocalizationSettings.StringDatabase.GetLocalizedString(
                    card.Title.TableReference,
                    card.Title.TableEntryReference,
                    locale ?? GetLearningLocale(),
                    fallback);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get a localized card description string in the target locale (defaults to learning locale).
        /// </summary>
        public string GetCardDescription(CardData card, Locale locale = null, FallbackBehavior fallback = FallbackBehavior.UseFallback)
        {
            if (card == null || card.Description == null || card.Description.IsEmpty)
                return string.Empty;
            try
            {
                return LocalizationSettings.StringDatabase.GetLocalizedString(
                    card.Description.TableReference,
                    card.Description.TableEntryReference,
                    locale ?? GetLearningLocale(),
                    fallback);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Resolve any localized string entry via the string database (defaults to learning locale when available).
        /// </summary>
        public string GetLocalizedString(TableReference table, TableEntryReference entry, Locale locale = null, FallbackBehavior fallback = FallbackBehavior.UseFallback)
        {
            try
            {
                return LocalizationSettings.StringDatabase.GetLocalizedString(table, entry, locale ?? GetLearningLocale(), fallback);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Fetch the localized card title clip for the given languageToUse.
        /// </summary>
        public Task<AudioClip> GetCardTitleClipAsync(CardData card, CardAudioLanguage languageToUse, CancellationToken ct = default)
            => _cardAudioService?.GetTitleClipAsync(card, languageToUse, ct) ?? Task.FromResult<AudioClip>(null);

        /// <summary>
        /// Fetch the localized card description clip for the given languageToUse.
        /// </summary>
        public Task<AudioClip> GetCardDescriptionClipAsync(CardData card, CardAudioLanguage languageToUse, CancellationToken ct = default)
            => _cardAudioService?.GetDescriptionClipAsync(card, languageToUse, ct) ?? Task.FromResult<AudioClip>(null);

        Locale GetLearningLocale()
        {
            return _app != null ? _app.GetLearningLocale() : DiscoverAppManager.I?.GetLearningLocale();
        }
    }
}
