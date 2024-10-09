using Antura.Core;
using System.Collections;
using System.Collections.Generic;
using Antura.Profile;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class HomeLogo : MonoBehaviour
    {
        public Image Logo;

        void Start()
        {
            if (!AppManager.PROFILE_INVERSION)
            {
                Logo.sprite = AppManager.I.ContentEdition.HomeLogo;
            }
        }

    }
}
