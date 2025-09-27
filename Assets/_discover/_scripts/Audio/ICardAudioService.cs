using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Antura.Discover;

namespace Antura.Discover.Audio
{
    /// <summary>
    /// Resolves localized AudioClips for Card data (title/description) from the project's localization tables.
    /// </summary>
    public interface ICardAudioService
    {
        /// <summary>Fetch the localized Title AudioClip for the given card, or null if not found.</summary>
        Task<AudioClip> GetTitleClipAsync(CardData card, CardAudioRoute route, CancellationToken ct = default);

        /// <summary>Fetch the localized Description AudioClip for the given card, or null if not found.</summary>
        Task<AudioClip> GetDescriptionClipAsync(CardData card, CardAudioRoute route, CancellationToken ct = default);
    }

    /// <summary>
    /// Which language route to use when resolving card audio.
    /// </summary>
    public enum CardAudioRoute
    {
        Learning,
        Native
    }
}
