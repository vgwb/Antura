using Antura.Core;
using Antura.Language;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class CurrentEditionIcon : MonoBehaviour
    {
        public void OnEnable()
        {
            GetComponentInChildren<TextRender>().SetText(AppManager.I.ContentEdition.Title, LanguageUse.Learning);
        }
    }
}
