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

        public async Task<AudioClip> GetTitleClipAsync(CardData card, CardAudioLanguage route, CancellationToken ct = default)
        {
            // Debug.Log($"[LocalizedCardAudioService] GetTitleClipAsync for card '{card?.Id}' route '{route}'");
            if (card == null)
                return null;
            var key = GetCardTitleKey(card);
            return await GetAudioFromCardsTableAsync(key, route, ct);
        }

        public async Task<AudioClip> GetDescriptionClipAsync(CardData card, CardAudioLanguage route, CancellationToken ct = default)
        {
            if (card == null)
                return null;
            var key = GetCardDescriptionKey(card);
            return await GetAudioFromCardsTableAsync(key, route, ct);
        }

        static string GetCardTitleKey(CardData card)
        {
            return card.Id;
        }

        static string GetCardDescriptionKey(CardData card)
        {
            return card.Id + "_DESC";
        }

        async Task<AudioClip> GetAudioFromCardsTableAsync(string entryKey, CardAudioLanguage route, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(entryKey))
                return null;

            Debug.Log($"----- LearningLocale is {LocalizationSystem.I.GetLearningLocale()?.Identifier.Code}, NativeLocale is {LocalizationSystem.I.GetNativeLocale()?.Identifier.Code}");

            // Determine target locale.
            Locale locale = route == CardAudioLanguage.Learning
                ? LocalizationSystem.I.GetLearningLocale()
                : LocalizationSystem.I.GetNativeLocale();

            // Use the same pattern as DiscoverLineProvider to fetch localized assets.
            // Debug.Log($"[LocalizedCardAudioService] Loading audio for key '{entryKey}' in locale '{locale?.Identifier.Code ?? "native"}' from table '{_cardsAudioTableRef.TableCollectionName}'");
            var loadOp = LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<UnityEngine.Object>(_cardsAudioTableRef, entryKey, locale, FallbackBehavior.UseFallback);
            var asset = await Yarn.Unity.YarnTask.WaitForAsyncOperation(loadOp, ct);

            return asset as AudioClip;
        }
    }
}
