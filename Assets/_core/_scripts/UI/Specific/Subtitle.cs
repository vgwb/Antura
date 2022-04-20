using Antura.Core;
using UnityEngine;

namespace Antura.UI
{
    /// <summary>
    /// constrain visibility of GameObject to SubtitlesEnabled setting
    /// </summary>
    public class Subtitle : MonoBehaviour
    {
        void Start()
        {
            gameObject.SetActive(AppManager.I.ContentEdition.LearnMethod.ShowHelpText);
        }
    }
}
