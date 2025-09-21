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

        ActivitySuccess = 20,
        ActivityFail = 21
    }

    [Serializable]
    public class SfxEntryAsset
    {
        public DiscoverSfx id;
        public AudioClip clip;
        public AssetData assetData;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [CreateAssetMenu(menuName = "Antura/Discover/Audio Sfx List", fileName = "SfxList")]
    public class DiscoverSfxData : ScriptableObject
    {
        public List<SfxEntryAsset> entries = new List<SfxEntryAsset>();
    }
}
