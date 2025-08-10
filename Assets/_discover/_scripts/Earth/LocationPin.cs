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
            //            Debug.Log($"LocationPin clicked: {Location?.Name}");
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
            UIQuestMenuManager.I.ShowLocation(Location, this);
        }
    }
}