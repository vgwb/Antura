using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Antura
{
    public static class AssetLoader
    {
        public static IEnumerator Load<T>(string path, Action<T> callback)
        {
            var async = Addressables.LoadAssetAsync<T>(path);
            while (!async.IsDone) yield return null;
            if (async.OperationException != null)
            {
                Debug.LogError(async.OperationException);
            }
            callback(async.Result);
        }

    }
}