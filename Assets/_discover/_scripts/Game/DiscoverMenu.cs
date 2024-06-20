using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Antura.Core;

namespace Antura.Minigames.DiscoverCountry
{
    public class DiscoverMenu : MonoBehaviour
    {
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
            Debug.Log("Cube was clicked or tapped!");
            AppManager.I.NavigationManager.GoToCountryFrance();
            // Call any method you want here
        }
    }
}
