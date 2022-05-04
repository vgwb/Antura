using Antura.Core;
using Antura.Language;
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
            if (AppManager.I.AppSettings.KioskMode)
            {
                label = "CROWFUNDING DEMO ";
            }
            label += AppManager.I.AppEdition.GetAppVersionString();
            label += " - " + AppManager.I.ContentEdition.ContentID.ToString();
            label += " (" + LanguageUtilities.GetISO3Code(AppManager.I.AppSettings.NativeLanguage) + ")";
            gameObject.GetComponent<TextMeshProUGUI>().text = label;
        }
    }
}
