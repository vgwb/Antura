using UnityEngine;

namespace Antura.Discover
{
    public class InventoryDisplay : MonoBehaviour
    {
        public GameObject CurrentItem;
        void Start()
        {
            CurrentItem.SetActive(false);
        }


    }
}
