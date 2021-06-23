using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Antura
{
    public static class AssetLoader
    {

        public static IEnumerator ValidateAndLoad<T>(string key, Action<T> callback)
        {
            var validateAddress = Addressables.LoadResourceLocationsAsync(key);
            yield return validateAddress.Task;
            if (validateAddress.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                if (validateAddress.Result.Count > 0)
                {
                    yield return Load<T>(key, callback);
                }
            }
            else
            {
                Debug.LogError("Error loading: " + key);
            }
        }

        public static IEnumerator Load<T>(string path, Action<T> callback, bool sync = false)
        {
           // Debug.LogError("Start loading " + path);
           var async = Addressables.LoadAssetAsync<T>(path);
           while (!async.IsDone)
           {
               if (sync) async.WaitForCompletion();
               else yield return null;
           }
           if (async.OperationException != null)
           {
               Debug.LogError(async.OperationException);
           }
           callback(async.Result);
        }

    }
}