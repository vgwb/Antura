using UnityEngine;
using System;
using System.Collections;

namespace Antura.Minigames.MissingLetter
{
    class Utils
    {
        public static IEnumerator LaunchDelay<T>(float delayTime, Action<T> action, T param)
        {
            yield return new WaitForSeconds(delayTime);
            action(param);
        }

        public static IEnumerator LaunchDelay(float delayTime, Action action)
        {
            yield return new WaitForSeconds(delayTime);
            action();
        }
    }
}
