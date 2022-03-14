using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
            AsyncOperationHandle<T> async = default;
            try
            {
                async = Addressables.LoadAssetAsync<T>(path);
            }
            catch (Exception)
            {
                Debug.LogWarning("Exception while trying to load " + path);
            }

            while (!async.IsDone)
            {
                if (sync)
                    async.WaitForCompletion();
                else
                    yield return null;
            }
            if (async.OperationException != null)
            {
                Debug.LogError($"Error loading {path}: {async.OperationException}");
            }
            callback(async.Result);
        }

    }
}
