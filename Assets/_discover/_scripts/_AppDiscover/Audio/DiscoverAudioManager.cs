using Antura.Core;
using Antura.Profile;
using Antura.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Audio
{
    public class DiscoverAudioManager : SingletonMonoBehaviour<DiscoverAudioManager>
    {
        [Header("Sfx Source")]
        [SerializeField] private AudioSource sfxSource;
        [Range(0f, 1f)] public float sfxVolume = 1f;

        [Header("Cards Source")]
        [SerializeField] private AudioSource cardSource;
        [Range(0f, 1f)] public float cardVolume = 1f;

        [Header("VO Source")]
        [SerializeField] private AudioSource voiceOverSource;
        [Range(0f, 1f)] public float voiceOverVolume = 1f;

        public DiscoverSfxData sfxListAsset;
        private struct SfxResolved { public AudioClip clip; public float vol; }
        private Dictionary<DiscoverSfx, SfxResolved> sfxLookup;

        protected override void Awake()
        {
            base.Awake();
            if (sfxSource == null)
            {
                if (sfxSource == null)
                    sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
                sfxSource.loop = false;
                sfxSource.spatialBlend = 0f;
            }
            if (cardSource == null)
            {
                cardSource = gameObject.AddComponent<AudioSource>();
                cardSource.playOnAwake = false;
                cardSource.loop = false;
                cardSource.spatialBlend = 0f;
            }
            if (voiceOverSource == null)
            {
                voiceOverSource = gameObject.AddComponent<AudioSource>();
                voiceOverSource.playOnAwake = false;
                voiceOverSource.loop = false;
                voiceOverSource.spatialBlend = 0f;
            }
            BuildLookup();
        }

        private void BuildLookup()
        {
            if (sfxLookup == null)
                sfxLookup = new Dictionary<DiscoverSfx, SfxResolved>();
            else
                sfxLookup.Clear();

            foreach (var e in sfxListAsset.entries)
            {
                if (e == null)
                    continue;
                AudioClip clip = e.clip;
                if (clip == null && e.assetData != null)
                    clip = e.assetData.Audio;
                if (clip != null)
                {
                    sfxLookup[e.id] = new SfxResolved { clip = clip, vol = e.volume };
                }
            }
        }

        #region play

        public void PlayVO(AudioClip clip)
        {
            if (clip == null)
                return;

            if (voiceOverSource.isPlaying)
                voiceOverSource.Stop();

            voiceOverSource.PlayOneShot(clip);
        }

        public void Play(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
                return;
            EnsureCustomSource();
            cardSource.volume = Mathf.Clamp01(volume) * cardVolume;
            cardSource.PlayOneShot(clip);
        }

        public void Play(AudioClip clip, Action onComplete, float volume = 1f)
        {
            if (clip == null)
                return;
            StartCoroutine(PlayWithCallbackCO(clip, Mathf.Clamp01(volume) * cardVolume, onComplete));
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

        public void Play(DiscoverSfx id, float volume = 1f)
        {
            if (id == DiscoverSfx.None)
                return;

            if (sfxLookup != null && sfxLookup.TryGetValue(id, out var res) && res.clip != null)
            {
                EnsureSfxSource();
                var finalVol = Mathf.Clamp01(volume) * Mathf.Clamp01(res.vol) * sfxVolume;
                sfxSource.PlayOneShot(res.clip, finalVol);
            }
        }

        public void Stop()
        {
            if (cardSource != null)
                cardSource.Stop();
            if (voiceOverSource != null)
                voiceOverSource.Stop();
        }

        public void StopAll()
        {
            if (sfxSource != null)
                sfxSource.Stop();
            if (cardSource != null)
                cardSource.Stop();
            if (voiceOverSource != null)
                voiceOverSource.Stop();
        }

        private void EnsureSfxSource()
        {
            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
                sfxSource.loop = false;
                sfxSource.spatialBlend = 0f;
            }
        }

        private void EnsureCustomSource()
        {
            if (cardSource == null)
            {
                cardSource = gameObject.AddComponent<AudioSource>();
                cardSource.playOnAwake = false;
                cardSource.loop = false;
                cardSource.spatialBlend = 0f;
            }
        }

        private IEnumerator PlayWithCallbackCO(AudioClip clip, float volume, Action onComplete)
        {
            // Use a temporary AudioSource so multiple parallel callbacks can work
            var go = new GameObject("SFX_Temp");
            go.transform.SetParent(transform, false);
            var src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = false;
            src.spatialBlend = 0f;
            src.volume = volume;
            src.clip = clip;
            src.Play();

            var duration = Mathf.Max(0f, clip.length / Mathf.Max(0.01f, src.pitch));
            var start = Time.unscaledTime;
            while (Time.unscaledTime - start < duration)
                yield return null;

            try
            { onComplete?.Invoke(); }
            catch (Exception ex) { Debug.LogException(ex, this); }
            Destroy(go);
        }
    }

}
