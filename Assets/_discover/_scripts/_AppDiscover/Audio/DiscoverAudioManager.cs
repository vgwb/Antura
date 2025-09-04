using Antura.Core;
using Antura.Profile;
using Antura.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public class DiscoverAudioManager : SingletonMonoBehaviour<DiscoverAudioManager>
    {
        [Header("Sfx Source")]
        [SerializeField] private AudioSource sfxSource;
        [Range(0f, 1f)] public float sfxVolume = 1f;

        [Header("Custom Source")]
        [SerializeField] private AudioSource customSource;
        [Range(0f, 1f)] public float customVolume = 1f;



        public DiscoverSfxData sfxListAsset;
        public List<SfxEntryAsset> sfxMap = new List<SfxEntryAsset>();
        private struct SfxResolved { public AudioClip clip; public float vol; }
        private Dictionary<DiscoverSfx, SfxResolved> sfxLookup;

        protected override void Awake()
        {
            base.Awake();
            if (sfxSource == null)
            {
                sfxSource = gameObject.GetComponent<AudioSource>();
                if (sfxSource == null)
                    sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
                sfxSource.loop = false;
                sfxSource.spatialBlend = 0f; // 2D by default
            }
            if (customSource == null)
            {
                customSource = gameObject.AddComponent<AudioSource>();
                customSource.playOnAwake = false;
                customSource.loop = false;
                customSource.spatialBlend = 0f; // 2D by default
            }
            BuildLookup();
        }

        private void BuildLookup()
        {
            if (sfxLookup == null)
                sfxLookup = new Dictionary<DiscoverSfx, SfxResolved>();
            else
                sfxLookup.Clear();

            var list = sfxListAsset != null ? sfxListAsset.entries : sfxMap;
            if (list == null)
                return;
            foreach (var e in list)
            {
                if (e == null)
                    continue;
                AudioClip clip = e.clip;
                if (clip == null && e.assetData != null)
                    clip = e.assetData.Audio;
                if (clip != null)
                {
                    var vol = ResolveEntryVolume(e);
                    sfxLookup[e.id] = new SfxResolved { clip = clip, vol = vol };
                }
            }
        }

        private static float ResolveEntryVolume(SfxEntryAsset entry)
        {
            if (entry == null)
                return 1f;
            // Prefer direct field when available
            try
            {
                var f = typeof(SfxEntryAsset).GetField("volume");
                if (f != null && f.FieldType == typeof(float))
                {
                    var val = (float)f.GetValue(entry);
                    return Mathf.Clamp01(val);
                }
            }
            catch { }
            return 1f;
        }

        // --- Simple play methods ---

        public void Play(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
                return;
            EnsureCustomSource();
            customSource.volume = Mathf.Clamp01(volume) * customVolume;
            customSource.PlayOneShot(clip);
        }

        public void Play(AudioClip clip, Action onComplete, float volume = 1f)
        {
            if (clip == null)
                return;
            StartCoroutine(PlayWithCallbackCO(clip, Mathf.Clamp01(volume) * customVolume, onComplete));
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

        // Enum-based convenience (optional mapping)
        public void Play(DiscoverSfx id, float volume = 1f)
        {
            if (id == DiscoverSfx.None)
                return;
            var count = (sfxListAsset != null ? (sfxListAsset.entries?.Count ?? 0) : (sfxMap?.Count ?? 0));
            if (sfxLookup == null || sfxLookup.Count != count)
                BuildLookup();
            if (sfxLookup != null && sfxLookup.TryGetValue(id, out var res) && res.clip != null)
            {
                EnsureSfxSource();
                var finalVol = Mathf.Clamp01(volume) * Mathf.Clamp01(res.vol) * sfxVolume;
                sfxSource.PlayOneShot(res.clip, finalVol);
            }
        }

        public void Stop()
        {

            if (customSource != null)
                customSource.Stop();
        }

        public void StopAll()
        {
            if (sfxSource != null)
                sfxSource.Stop();
            if (customSource != null)
                customSource.Stop();
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
            if (customSource == null)
            {
                customSource = gameObject.AddComponent<AudioSource>();
                customSource.playOnAwake = false;
                customSource.loop = false;
                customSource.spatialBlend = 0f;
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
