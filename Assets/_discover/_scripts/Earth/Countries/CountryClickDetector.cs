using UnityEngine;

namespace Antura.Discover
{
    public class CountryClickDetector : MonoBehaviour
    {
        public Camera mainCamera;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    CountryData countryData = clickedObject.GetComponent<CountryData>();

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
