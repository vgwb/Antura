#if UNITY_EDITOR
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Antura.Discover.Audio.Editor
{
    /// <summary>ElevenLabs TTS HTTP client (Editor-only, returns MP3 bytes).</summary>
    public sealed class ElevenLabsTtsService : ITtsService
    {
        [Serializable]
        private class ElevenLabsVoiceSettings
        {
            public float stability;
            public float similarity_boost;
            public float style;
            public bool use_speaker_boost;
        }

        [Serializable]
        private class ElevenLabsTTSRequest
        {
            public string text;
            public string model_id;
            public ElevenLabsVoiceSettings voice_settings;
            public string output_format;
        }

        public IEnumerator SynthesizeMp3Coroutine(string apiKey, VoiceProfileData profile, string text, Action<byte[]> onDone)
        {
            onDone ??= _ => { };
            if (string.IsNullOrWhiteSpace(text))
            { onDone(Array.Empty<byte>()); yield break; }

            string voiceId = profile != null ? profile.VoiceId : string.Empty;
            string modelId = profile != null && !string.IsNullOrEmpty(profile.ModelId) ? profile.ModelId : "eleven_flash_v2_5";

            var url = $"https://api.elevenlabs.io/v1/text-to-speech/{voiceId}";
            var request = new UnityWebRequest(url, "POST");
            request.SetRequestHeader("xi-api-key", apiKey);
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "audio/mpeg");

            var payload = new ElevenLabsTTSRequest
            {
                text = text,
                model_id = modelId,
                voice_settings = new ElevenLabsVoiceSettings
                {
                    stability = profile != null ? profile.Stability : 0.5f,
                    similarity_boost = profile != null ? profile.SimilarityBoost : 0.8f,
                    style = profile != null ? profile.StyleExaggeration : 0f,
                    use_speaker_boost = profile != null && profile.UseSpeakerBoost
                },
                output_format = "mp3_44100_96"
            };

            var json = JsonUtility.ToJson(payload);
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onDone(request.downloadHandler.data);
            }
            else
            {
                string serverMsg = null;
                try
                { serverMsg = Encoding.UTF8.GetString(request.downloadHandler.data); }
                catch { }
                Debug.LogError($"[QVM] TTS HTTP error: {request.responseCode} {request.error}\n{serverMsg}");
                onDone(null);
            }
        }
    }
}
#endif
