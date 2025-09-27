using System;
using System.Threading;
using System.Threading.Tasks;
using Antura.Discover;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Antura.Discover.Audio
{
    /// <summary>
    /// Card audio provider backed by Unity Localization Asset Tables ("Cards audio").
    /// </summary>
    public sealed class LocalizedCardAudioService : ICardAudioService
    {
        readonly TableReference _cardsAudioTableRef;

        public LocalizedCardAudioService(TableReference cardsAudioTableRef)
        {
            _cardsAudioTableRef = cardsAudioTableRef;
        }

        public async Task<AudioClip> GetTitleClipAsync(CardData card, CardAudioRoute route, CancellationToken ct = default)
        {
            if (card == null)
                return null;
            var key = GetCardTitleKey(card);
            return await GetAudioFromCardsTableAsync(key, route, ct);
        }

        public async Task<AudioClip> GetDescriptionClipAsync(CardData card, CardAudioRoute route, CancellationToken ct = default)
        {
            if (card == null)
                return null;
            var key = GetCardDescriptionKey(card);
            return await GetAudioFromCardsTableAsync(key, route, ct);
        }

        static string GetCardTitleKey(CardData card)
        {
            // Convention: title audio entries are keyed by the card id.
            return card.Id;
        }

        static string GetCardDescriptionKey(CardData card)
        {
            // Conventional fallback: description key derived from id.
            return card.Id + "_DESC";
        }

        async Task<AudioClip> GetAudioFromCardsTableAsync(string entryKey, CardAudioRoute route, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(entryKey))
                return null;

            // Determine target locale.
            Locale locale = route == CardAudioRoute.Learning
                ? DiscoverAppManager.I?.GetLearningLocale()
                : null; // null uses active SelectedLocale as per GetLocalizedAssetAsync API

            // Use the same pattern as DiscoverLineProvider to fetch localized assets.
            var loadOp = LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<UnityEngine.Object>(_cardsAudioTableRef, entryKey, locale, FallbackBehavior.UseFallback);
            var obj = await Yarn.Unity.YarnTask.WaitForAsyncOperation(loadOp, ct);
            return obj as AudioClip;
        }
    }
}
