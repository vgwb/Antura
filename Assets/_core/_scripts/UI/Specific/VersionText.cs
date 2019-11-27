using Antura.Core;
using UnityEngine;
using TMPro;

namespace Antura.UI
{
    /// <summary>
    /// Shows the version of the application. Used in the Home scene.
    /// </summary>
    public class VersionText : MonoBehaviour
    {
        void Start()
        {
            var label = "";
            if (AppManager.I.AppSettings.KioskMode) {
                label = "CROWFUNDING DEMO ";
            }
            label += EditionConfig.I.GetAppVersionString();
            gameObject.GetComponent<TextMeshProUGUI>().text = label;
        }
    }
}