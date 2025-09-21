using System;
using System.Collections.Generic;
using System.Linq;
using Antura.Discover;
using UnityEngine;
using UnityEngine.Localization;

namespace Antura.Discover.Audio
{
    [CreateAssetMenu(fileName = "VoiceProfileCatalog", menuName = "Antura/Audio/Voice Profile Catalog")]
    public class VoiceProfileCatalog : ScriptableObject, IVoiceProvider
    {
        [Serializable]
        public class ActorVoice
        {
            public VoiceActors Actor = VoiceActors.Default;
            public VoiceProfileData Profile;
        }

        [Serializable]
        public class LanguageOverride
        {
            [Tooltip("Locale code e.g. en, it, fr.")]
            public string LanguageCode = "en";
            [Tooltip("Default profile for this language (used when actor not explicitly mapped).")]
            public VoiceProfileData DefaultProfile;
            [Tooltip("Optional per-actor profiles overriding the language default.")]
            public List<ActorVoice> PerActor = new();
        }

        [Header("Global Defaults")]
        [Tooltip("Global fallback used if no language-specific profile is found.")]
        public VoiceProfileData GlobalDefault;

        [Tooltip("Default profiles by actor regardless of language (used if language has no explicit per-actor override).")]
        public List<ActorVoice> GlobalPerActor = new();

        [Header("Language Overrides")]
        public List<LanguageOverride> Languages = new();

        public VoiceProfileData GetProfile(Locale locale, VoiceActors actor)
        {
            var code = locale?.Identifier.Code ?? string.Empty;
            return GetProfile(code, actor);
        }

        public VoiceProfileData GetProfile(string languageCode, VoiceActors actor)
        {
            var code = (languageCode ?? string.Empty).Trim().ToLowerInvariant();

            // 1) Exact language override per actor
            var lang = Languages.FirstOrDefault(l => string.Equals(l.LanguageCode?.Trim(), code, StringComparison.OrdinalIgnoreCase));
            if (lang != null)
            {
                var perActor = lang.PerActor?.FirstOrDefault(a => a.Actor == actor)?.Profile;
                if (perActor != null)
                    return perActor;

                // 2) Language default
                if (lang.DefaultProfile != null)
                    return lang.DefaultProfile;
            }

            // 3) Global per-actor
            var globalActor = GlobalPerActor?.FirstOrDefault(a => a.Actor == actor)?.Profile;
            if (globalActor != null)
                return globalActor;

            // 4) Global default
            return GlobalDefault;
        }

        // New: resolve without Actor (use per-language default → global default)
        public VoiceProfileData GetProfile(Locale locale)
        {
            var code = locale?.Identifier.Code ?? string.Empty;
            return GetProfile(code);
        }

        public VoiceProfileData GetProfile(string languageCode)
        {
            var code = (languageCode ?? string.Empty).Trim().ToLowerInvariant();
            var lang = Languages.FirstOrDefault(l => string.Equals(l.LanguageCode?.Trim(), code, StringComparison.OrdinalIgnoreCase));
            if (lang != null && lang.DefaultProfile != null)
            {
                return lang.DefaultProfile;
            }
            return GlobalDefault;
        }
    }
}
