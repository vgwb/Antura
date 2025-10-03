using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Audio
{
    public enum DiscoverSfx
    {
        None = 0,
        ActivityClick = 10,
        ActivityDrop = 11,
        ActivityHint = 12,
        ActivityGoodMove = 13,
        ActivityBadMove = 14,
        FlipCard = 15,
        ActivityAttach = 16,
        ActivityDetach = 17,

        ActivitySuccess = 20,
        ActivityFail = 21
    }

    public enum DiscoverSfxCategory
    {
        UI,
        Gameplay,
        Ambient,
    }

    [Serializable]
    public class SfxEntryAsset
    {
        public DiscoverSfx id;
        [Tooltip("Optional string identifier for Addressables or future dynamic lookups.")]
        public string key;
        public DiscoverSfxCategory category = DiscoverSfxCategory.UI;
        public AudioClip clip;
        public AssetData assetData;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0f, 1f)] public float pitchVariance = 0f;
        public bool spatialize;
    }

    [CreateAssetMenu(menuName = "Antura/Discover/Audio Sfx List", fileName = "SfxList")]
    public class DiscoverSfxData : ScriptableObject
    {
        public List<SfxEntryAsset> entries = new List<SfxEntryAsset>();
    }
}
