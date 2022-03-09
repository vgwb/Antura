using Antura.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class HomeLogo : MonoBehaviour
    {
        public Image Logo;

        void Start()
        {
            Logo.sprite = AppManager.I.ContentEdition.HomeLogo;
        }

    }
}
