using UnityEngine;
namespace Antura.Discover
{
    public class HideOnStart : MonoBehaviour
    {
        public GameObject[] objectsToHide;
        void Start()
        {
            HideObjects();
        }

        void HideObjects()
        {
            foreach (var obj in objectsToHide)
            {
                obj.SetActive(false);
            }
        }
    }
}
