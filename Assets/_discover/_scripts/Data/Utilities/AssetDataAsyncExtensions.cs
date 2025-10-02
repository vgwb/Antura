using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Antura.Discover
{
    public static class AssetDataAsyncExtensions
    {
        public static async UniTask<Sprite> LoadSafeSpriteAsync(this AssetData asset, CancellationToken cancellationToken = default, bool allowEditorInstant = true)
        {
            if (asset == null)
            {
                return null;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying && allowEditorInstant)
            {
                return asset.Image;
            }
#endif

            if (!asset.HasImageAsset)
            {
                return null;
            }

            try
            {
                return await asset.LoadImageAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[AssetData] Failed to load sprite for {asset.name} ({asset.Id}). {ex.Message}", asset);
                return null;
            }
        }
    }
}
