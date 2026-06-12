using UnityEngine;

namespace Antura.Discover
{
    public class CountryClickDetector : MonoBehaviour
    {
        public Camera mainCamera;

        void Update()
        {
            if (InputCompat.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(InputCompat.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    CountryDataInfo countryData = clickedObject.GetComponent<CountryDataInfo>();

                    if (countryData != null)
                    {
                        Debug.Log("Clicked on country: " + countryData.countryName);
                        // Display or handle country data
                    }
                }
            }
        }
    }
}
