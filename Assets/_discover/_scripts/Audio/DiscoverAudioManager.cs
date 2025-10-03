using Antura.Core;
using Antura.Profile;
using Antura.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Antura.Discover.Audio
{
    public class DiscoverAudioManager : SingletonMonoBehaviour<DiscoverAudioManager>
    {
        [Header("Mixer Groups")]
        [SerializeField] private AudioMixerGroup sfxMixerGroup;
        [SerializeField] private AudioMixerGroup musicMixerGroup;
        [SerializeField] private AudioMixerGroup ambientMixerGroup;
        [SerializeField] private AudioMixerGroup systemVoiceMixerGroup;
        [SerializeField] private AudioMixerGroup dialogueVoiceMixerGroup;

        [Header("Sfx Source")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField, Min(1)] private int sfxPoolSize = 8;
        [Range(0f, 1f)] public float sfxVolume = 1f;
        private readonly List<AudioSource> sfxPool = new List<AudioSource>();

        [Header("System Voice")]
        [SerializeField] private AudioSource systemVoiceSource;
        [Range(0f, 1f)] public float systemVoiceVolume = 1f;

        [Header("Dialogue Source")]
        [SerializeField] private AudioSource dialogueVoiceSource;
        [Range(0f, 1f)] public float dialogueVolume = 1f;

        [Header("Ambient Source")]
        [SerializeField] private AudioSource ambientSource;
        [Range(0f, 1f)] public float ambientVolume = 1f;
        private Coroutine ambientFadeCo;
        private AudioClip currentAmbient;

        public DiscoverSfxData sfxListAsset;
        private struct SfxResolved
        {
            public AudioClip clip;
            public float vol;
            public float pitchVariance;
            public bool spatial;
            public DiscoverSfxCategory category;
        }
        private Dictionary<DiscoverSfx, SfxResolved> sfxLookup;
        private Dictionary<string, SfxResolved> sfxKeyLookup;

        protected override void Awake()
        {
            base.Awake();
            EnsureSfxPool();
            BuildLookup();
        }

        private void BuildLookup()
        {
            if (sfxLookup == null)
                sfxLookup = new Dictionary<DiscoverSfx, SfxResolved>();
            else
                sfxLookup.Clear();
            if (sfxKeyLookup == null)
                sfxKeyLookup = new Dictionary<string, SfxResolved>(StringComparer.OrdinalIgnoreCase);
            else
                sfxKeyLookup.Clear();

            if (sfxListAsset == null || sfxListAsset.entries == null)
                return;

            foreach (var e in sfxListAsset.entries)
            {
                if (e == null)
                    continue;
                AudioClip clip = e.clip;
                if (clip == null && e.assetData != null)
                    clip = e.assetData.Audio;
                if (clip != null)
                {
                    var resolved = new SfxResolved
                    {
                        clip = clip,
                        vol = e.volume,
                        pitchVariance = Mathf.Max(0f, e.pitchVariance),
                        spatial = e.spatialize,
                        category = e.category
                    };

                    sfxLookup[e.id] = resolved;
                    if (!string.IsNullOrEmpty(e.key) && !sfxKeyLookup.ContainsKey(e.key))
                        sfxKeyLookup.Add(e.key, resolved);
                }
            }
        }

        #region play

        public void PlayDialogue(AudioClip clip)
        {
            if (clip == null)
                return;

            if (dialogueVoiceSource.isPlaying)
                dialogueVoiceSource.Stop();

            dialogueVoiceSource.volume = Mathf.Clamp01(dialogueVolume);
            dialogueVoiceSource.clip = clip;
            dialogueVoiceSource.Play();
        }

        public void PlayAmbient(AudioClip clip, float volume = 1f, bool loop = true, float fadeDuration = 0.5f)
        {

            if (ambientFadeCo != null)
            {
                StopCoroutine(ambientFadeCo);
                ambientFadeCo = null;
            }

            if (clip == null)
            {
                ambientFadeCo = StartCoroutine(FadeOutAmbient(fadeDuration));
                return;
            }

            if (currentAmbient == clip && ambientSource.isPlaying)
            {
                ambientSource.volume = Mathf.Clamp01(volume) * ambientVolume;
                return;
            }

            ambientFadeCo = StartCoroutine(SwapAmbientClip(clip, Mathf.Clamp01(volume) * ambientVolume, loop, fadeDuration));
        }

        public void StopAmbient(float fadeDuration = 0.5f)
        {
            if (ambientSource == null)
                return;

            if (ambientFadeCo != null)
            {
                StopCoroutine(ambientFadeCo);
                ambientFadeCo = null;
            }

            ambientFadeCo = StartCoroutine(FadeOutAmbient(fadeDuration));
        }

        /// <summary>
        /// Play a one-shot audio clip on the system voice source.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="volume"></param>
        public void Play(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
                return;
            systemVoiceSource.volume = Mathf.Clamp01(volume) * systemVoiceVolume;
            systemVoiceSource.PlayOneShot(clip);
        }

        public void Play(AudioClip clip, Action onComplete, float volume = 1f)
        {
            if (clip == null)
                return;
            StartCoroutine(PlayWithCallbackCO(clip, Mathf.Clamp01(volume) * systemVoiceVolume, onComplete));
        }

        public void Play(AssetData asset, float volume = 1f)
        {
            if (asset == null)
                return;
            Play(asset.Audio, volume);
        }

        public void Play(AssetData asset, Action onComplete, float volume = 1f)
        {
            if (asset == null)
                return;
            Play(asset.Audio, onComplete, volume);
        }
        #endregion

        public void PlaySfx(DiscoverSfx id, float volume = 1f)
        {
            if (id == DiscoverSfx.None)
                return;

            if (sfxLookup != null && sfxLookup.TryGetValue(id, out var res) && res.clip != null)
            {
                PlayResolvedSfx(res, volume);
            }
        }

        public void Play(string key, float volume = 1f)
        {
            if (string.IsNullOrEmpty(key) || sfxKeyLookup == null)
                return;

            if (sfxKeyLookup.TryGetValue(key, out var res) && res.clip != null)
            {
                PlayResolvedSfx(res, volume);
            }
        }

        public void Stop()
        {
            foreach (var src in sfxPool)
            {
                if (src != null)
                    src.Stop();
            }
            if (sfxSource != null)
                sfxSource.Stop();
            if (systemVoiceSource != null)
                systemVoiceSource.Stop();
            if (dialogueVoiceSource != null)
                dialogueVoiceSource.Stop();
            if (ambientSource != null)
            {
                ambientSource.Stop();
                currentAmbient = null;
                if (ambientFadeCo != null)
                {
                    StopCoroutine(ambientFadeCo);
                    ambientFadeCo = null;
                }
            }
        }

        public void StopAll()
        {
            foreach (var src in sfxPool)
            {
                if (src != null)
                    src.Stop();
            }
            if (sfxSource != null)
                sfxSource.Stop();
            if (systemVoiceSource != null)
                systemVoiceSource.Stop();
            if (dialogueVoiceSource != null)
                dialogueVoiceSource.Stop();
            if (ambientSource != null)
            {
                ambientSource.Stop();
                currentAmbient = null;
                if (ambientFadeCo != null)
                {
                    StopCoroutine(ambientFadeCo);
                    ambientFadeCo = null;
                }
            }
        }

        private IEnumerator PlayWithCallbackCO(AudioClip clip, float volume, Action onComplete)
        {
            var src = GetAvailableSfxSource();
            var originalGroup = src.outputAudioMixerGroup;
            var targetGroup = systemVoiceMixerGroup != null
                ? systemVoiceMixerGroup
                : systemVoiceSource != null ? systemVoiceSource.outputAudioMixerGroup : originalGroup;
            if (targetGroup != null)
                src.outputAudioMixerGroup = targetGroup;
            src.volume = volume;
            src.clip = clip;
            src.loop = false;
            src.Play();

            var duration = Mathf.Max(0f, clip.length / Mathf.Max(0.01f, src.pitch));
            var start = Time.unscaledTime;
            while (Time.unscaledTime - start < duration)
                yield return null;

            try
            { onComplete?.Invoke(); }
            catch (Exception ex) { Debug.LogException(ex, this); }

            src.Stop();
            src.clip = null;
            src.outputAudioMixerGroup = originalGroup;
        }

        private void PlayResolvedSfx(SfxResolved res, float volume)
        {
            var src = GetAvailableSfxSource();
            var finalVol = Mathf.Clamp01(volume) * Mathf.Clamp01(res.vol) * sfxVolume;
            var originalPitch = src.pitch;
            var originalSpatial = src.spatialBlend;
            var pitch = res.pitchVariance > 0f ? UnityEngine.Random.Range(1f - res.pitchVariance, 1f + res.pitchVariance) : 1f;

            src.pitch = pitch;
            src.spatialBlend = res.spatial ? 1f : 0f;
            if (sfxMixerGroup != null)
                src.outputAudioMixerGroup = sfxMixerGroup;
            src.clip = res.clip;
            src.loop = false;
            src.volume = finalVol;
            src.Play();

            StartCoroutine(ResetSfxSourceAfter(src, res.clip.length / Mathf.Max(0.01f, pitch), originalPitch, originalSpatial));
        }


        private void EnsureSfxPool()
        {
            if (sfxPoolSize < 1)
                sfxPoolSize = 1;

            while (sfxPool.Count < sfxPoolSize)
            {
                sfxPool.Add(CreateSfxSourceObject(sfxPool.Count));
            }
        }

        private AudioSource CreateSfxSourceObject(int index)
        {
            var go = new GameObject($"SFX_Source_{index}");
            go.transform.SetParent(transform, false);
            var src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = false;
            src.spatialBlend = 0f;
            if (sfxMixerGroup != null)
                src.outputAudioMixerGroup = sfxMixerGroup;
            else if (sfxSource != null)
                src.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;
            return src;
        }

        private AudioSource GetAvailableSfxSource()
        {
            foreach (var src in sfxPool)
            {
                if (src != null && !src.isPlaying)
                    return src;
            }

            var newSource = CreateSfxSourceObject(sfxPool.Count);
            sfxPool.Add(newSource);
            return newSource;
        }

        private IEnumerator ResetSfxSourceAfter(AudioSource src, float duration, float originalPitch, float originalSpatial)
        {
            if (src == null)
                yield break;

            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            src.Stop();
            src.clip = null;
            src.pitch = originalPitch;
            src.spatialBlend = originalSpatial;
        }

        private IEnumerator SwapAmbientClip(AudioClip clip, float targetVolume, bool loop, float fadeDuration)
        {
            if (fadeDuration > 0f && ambientSource.isPlaying)
            {
                yield return FadeSourceVolume(ambientSource, 0f, fadeDuration * 0.5f);
            }

            ambientSource.Stop();
            ambientSource.clip = clip;
            ambientSource.loop = loop;
            ambientSource.volume = 0f;
            ambientSource.Play();
            currentAmbient = clip;

            if (fadeDuration > 0f)
            {
                yield return FadeSourceVolume(ambientSource, targetVolume, fadeDuration * 0.5f);
            }
            else
            {
                ambientSource.volume = targetVolume;
            }

            ambientFadeCo = null;
        }

        private IEnumerator FadeOutAmbient(float fadeDuration)
        {
            if (ambientSource == null || !ambientSource.isPlaying)
            {
                currentAmbient = null;
                ambientFadeCo = null;
                yield break;
            }

            if (fadeDuration > 0f)
            {
                yield return FadeSourceVolume(ambientSource, 0f, fadeDuration);
            }

            ambientSource.Stop();
            ambientSource.clip = null;
            currentAmbient = null;
            ambientFadeCo = null;
        }

        private static IEnumerator FadeSourceVolume(AudioSource source, float target, float duration)
        {
            if (source == null)
                yield break;

            var startVolume = source.volume;
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                var t = duration > 0f ? Mathf.Clamp01(elapsed / duration) : 1f;
                source.volume = Mathf.Lerp(startVolume, target, t);
                yield return null;
            }
            source.volume = target;
        }
    }

}
