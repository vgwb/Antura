using Antura.Core;
using Antura.Discover.UI;
using DG.DeExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{

    public class EarthManager : MonoBehaviour
    {
        public static EarthManager I;

        void Awake()
        {
            if (I == null)
            {
                I = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            //            Debug.Log("EarthManager START");
            // Debug.Log("Current contet: " + AppManager.I.ContentEdition.ContentID);
            if (AppManager.I.ContentEdition.ContentID == LearningContentID.Discover_Poland)
            {
                SelectCountry(Countries.Poland);
            }
            else
            {
                SelectCountry(Countries.France);
            }
        }

        public void SelectCountry(Countries selectedCountry)
        {
            Debug.Log($"Selecting country: {selectedCountry}");
            CountryButton.SetSelectedCountry(selectedCountry);

            UIQuestMenuManager.I.ShowCountry(selectedCountry);
        }

    }
}
