#if UNITY_EDITOR
using System;
using System.Collections;
using UnityEngine;

namespace Antura.Discover.Audio.Editor
{
    /// <summary>Editor coroutine-based TTS API that returns MP3 bytes.</summary>
    public interface ITtsService
    {
        IEnumerator SynthesizeMp3Coroutine(string apiKey, VoiceProfileData profile, string text, string languageCode, Action<byte[]> onDone);
    }
}
#endif
