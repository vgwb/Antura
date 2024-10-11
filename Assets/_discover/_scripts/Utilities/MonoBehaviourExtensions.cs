using System.Collections;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// Stops a coroutine and nulls its ref variable, while also checking for NULL target/coroutine to prevent errors
        /// </summary>
        public static void CancelCoroutine(this MonoBehaviour target, ref Coroutine coroutine)
        {
            if (coroutine == null) return;
            if (target != null) target.StopCoroutine(coroutine);
            coroutine = null;
        }
        
        /// <summary>
        /// Stops the given coroutine ref if it exists, then assigns a new coroutine to it
        /// </summary>
        public static void RestartCoroutine(this MonoBehaviour target, ref Coroutine coroutine, IEnumerator coroutineMethod)
        {
            if (target == null) {
                coroutine = null;
                return;
            }
            if (coroutine != null) target.StopCoroutine(coroutine);
            coroutine = target.StartCoroutine(coroutineMethod);
        }
    }
}