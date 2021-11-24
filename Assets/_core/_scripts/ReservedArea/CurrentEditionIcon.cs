using Antura.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class CurrentEditionIcon : MonoBehaviour
    {
        public void OnEnable()
        {
            GetComponent<Image>().sprite = AppManager.I.ContentEdition.Icon;
        }
    }
}