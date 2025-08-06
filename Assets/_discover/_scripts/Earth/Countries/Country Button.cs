using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Antura.Core;

namespace Antura.Discover
{
    public class CountryButton : MonoBehaviour
    {
        public Countries country;

        void Start()
        {
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
                        OnCubeClicked();
                    }
                }
            }
        }
        private void OnCubeClicked()
        {
            // Debug.Log("Cube was clicked or tapped!");
            EarthManager.I.SelectCountry(country);
        }
    }
}
