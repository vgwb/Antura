using UnityEngine.Localization;
using Antura.Discover;

namespace Antura.Discover.Audio
{
    /// <summary>
    /// Provides a VoiceProfile for a given language/actor combination, with internal fallbacks.
    /// </summary>
    public interface IVoiceProvider
    {
        VoiceProfileData GetProfile(Locale locale, VoiceActors actor);
        VoiceProfileData GetProfile(string languageCode, VoiceActors actor);
    }
}
