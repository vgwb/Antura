using Antura.Audio;
using Antura.Database;
using Antura.UI;
using Antura.Helpers;
using System.Collections.Generic;
using Antura.Keeper;
using Antura.Language;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class CurrentLanguageIcon : MonoBehaviour
    {
        public void OnEnable()
        {
            if (LanguageManager.I == null)
                return;
            GetComponent<Image>().sprite = LanguageManager.I.GetLangConfig(LanguageUse.Native).FlagIcon;
        }
    }
}
