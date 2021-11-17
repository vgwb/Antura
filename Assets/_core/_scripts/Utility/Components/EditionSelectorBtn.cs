using Antura.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Utilities
{
    public class EditionSelectorBtn : MonoBehaviour
    {

        void Start()
        {
            bool isVisible = AppManager.I.AppEdition.HasMultipleLearningEditions;

            gameObject.SetActive(isVisible);
        }

    }
}