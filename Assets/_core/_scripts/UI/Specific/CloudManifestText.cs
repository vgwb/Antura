using Antura.Core;
using UnityEngine;
using TMPro;

namespace Antura.UI
{
    public class CloudManifestText : MonoBehaviour
    {
        void Start()
        {
            var str = "CLOUD MANIFEST TEXT: \n" + AppManager.I.AppEdition.CloudManifest;
            gameObject.GetComponent<TextMeshProUGUI>().text = str;
        }
    }
}