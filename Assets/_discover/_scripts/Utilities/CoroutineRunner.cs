using System.Collections;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    /// <summary>
    /// Independent coroutine runner, accessed by MonoBehaviourExtensions in case we request an independent coroutine
    /// </summary>
    public class CoroutineRunner : MonoBehaviour
    {
        static CoroutineRunner I;
        
        static CoroutineRunner()
        {
            GameObject go = new GameObject("CoroutineRunner");
            I = go.AddComponent<CoroutineRunner>();
            DontDestroyOnLoad(go);
        }

        #region Public Methods
        
        public static void CancelCoroutine(ref Coroutine coroutine)
        {
            I.CancelCoroutine(ref coroutine);
        }

        public static void RestartCoroutine(ref Coroutine coroutine, IEnumerator coroutineMethod)
        {
            I.RestartCoroutine(ref coroutine, coroutineMethod);
        }

        #endregion
    }
}