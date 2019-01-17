using Antura.Core;
using UnityEngine;

namespace Antura.UI
{
    /// <summary>
    /// constrain visibility of GameObject to EnglishSubtitles setting
    /// </summary>
    public class EnglishSubtitle : MonoBehaviour
    {
        void Start()
        {
            gameObject.SetActive(AppManager.I.AppSettings.EnglishSubtitles);
        }
    }
}