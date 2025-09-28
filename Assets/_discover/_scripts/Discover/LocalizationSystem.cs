using Antura.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Antura.Discover
{
    public class LocalizationSystem : SingletonMonoBehaviour<LocalizationSystem>
    {
        /// <summary>
        /// Initialize the localization system.
        /// as early as possible.
        /// </summary>
        /// <returns></returns>
        IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
        }

    }
}
