using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public enum DiscoverSfx
    {
        None = 0,
        Click,
        Success,
        Fail
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
