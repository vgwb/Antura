using Antura.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Utilities
{
    // DEPRECATED
    public class LanguageSelectorBtn : MonoBehaviour
    {

        void Start()
        {
            bool isVisible = AppManager.I.ContentEdition.OverridenNativeLanguages.Length > 1;

            gameObject.SetActive(isVisible);
        }

    }
}
