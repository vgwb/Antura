using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Antura.Discover
{
    [System.Serializable]
    public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
    {
        public AssetReferenceAudioClip() : base(string.Empty) { }
        public AssetReferenceAudioClip(string guid) : base(guid) { }
    }
}
