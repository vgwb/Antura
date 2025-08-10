using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Antura.Core;
using TMPro;

namespace Antura.Discover
{
    public class CountryButton : MonoBehaviour
    {
        public CountryData country;

        public TMP_Text countryLabel;

        void Start()
        {
            countryLabel.text = country.CountryName.GetLocalizedString();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        OnCountryClicked();
                    }
                }
            }
        }
        private void OnCountryClicked()
        {
            Debug.Log($"Country {country.CountryId} clicked.");
            EarthManager.I.SelectCountry(country.CountryId);
        }
    }
}
