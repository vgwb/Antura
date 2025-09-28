using Antura.Discover.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.Discover
{
    public class LocationPin : MonoBehaviour
    {
        public LocationData Location;

        [Header("Internal References")]
        public GameObject Highlight;

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
                        OnClicked();
                    }
                }
            }
        }

        private void OnClicked()
        {
            // Debug.Log($"LocationPin clicked: {Location?.Name}");
            // Deselect all other pins, then select this one
            var allPins = FindObjectsByType<LocationPin>(FindObjectsSortMode.None);
            for (int i = 0; i < allPins.Length; i++)
            {
                if (allPins[i] != null && allPins[i] != this)
                    allPins[i].DeSelect();
            }
            Select();
            OpenQuest();
        }

        private void OnEnable()
        {
            DeSelect();
        }

        public void Select()
        {
            Highlight.SetActive(true);
        }

        public void DeSelect()
        {
            Highlight.SetActive(false);
        }

        public void OpenQuest()
        {
            if (Location is null)
            {
                Debug.LogWarning($"LocationPin on {name}: LocationId not set.");
                return;
            }
            UIDiscoverHome.I.ShowLocation(Location);
        }
    }
}
