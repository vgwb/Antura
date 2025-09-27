using System;
using UnityEngine;

namespace Antura.Discover.Audio
{
    [CreateAssetMenu(menuName = "Antura/Audio/Voice Profile", fileName = "VoiceProfile")]
    public class VoiceProfileData : IdentifiedData
    {
        [Header("ElevenLabs")]
        [Tooltip("ID of the ElevenLabs voice (from Voices API).")]
        public string VoiceId;

        [Tooltip("TTS model to use (e.g., eleven_flash_v2_5 for WS; eleven_multilingual_v2 for HTTP).")]
        public string ModelId = "eleven_flash_v2_5";

        [Header("Metadata")]
        [Tooltip("Human-friendly label (for designers only).")]
        public string DisplayName;

        [Tooltip("Locale codes e.g. en, it, fr. Comma-separated from ElevenLabs verified_languages.")]
        public string VerifiedLanguages = "";
        public string language;
        public string age;
        public string description;
        public string gender;
        public string preview_url;

        public VoiceActorGenre Genre = VoiceActorGenre.Neutral;

        [Tooltip("The actor key in Yarn (e.g., ANTURA, NARRATOR, TEACHER).")]
        public string ActorKey;

        [Header("Voice Settings")]
        [Range(0, 1)] public float Stability = 0.5f;
        [Range(0, 1)] public float SimilarityBoost = 0.8f;
        [Range(0, 1)] public float StyleExaggeration = 0.0f;
        public bool UseSpeakerBoost = false;

        [Header("Output")]
        [Tooltip("PCM sample rate: 24000 or 44100 are common choices.")]
        public int SampleRate = 24000;
    }
}
