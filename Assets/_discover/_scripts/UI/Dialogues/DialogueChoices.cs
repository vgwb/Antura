using System;
using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using Homer;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class DialogueChoices : MonoBehaviour
    {
        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] DialogueChoice[] choices;

        #endregion

        #region Unity

        void Start()
        {
            this.gameObject.SetActive(false);
        }

        #endregion

        #region Public Methods

        public void Show(List<HomerElement> elements)
        {
            Debug.LogWarning("Show choices not implemented yet");
        }
        
        public void Hide()
        {
            Debug.LogWarning("Hide choices not implemented yet");
        }

        #endregion
    }
}