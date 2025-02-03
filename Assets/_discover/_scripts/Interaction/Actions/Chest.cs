using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Antura.Minigames.DiscoverCountry.Interaction
{
    public class Chest : MonoBehaviour
    {
        public GameObject Lid;

        public float rotationAngle = 90f;

        private bool isOpen = false;
        void Start()
        {

        }

        public void Open()
        {
            ToggleRotationAngle();

        }

        void ToggleRotationAngle()
        {
            // Toggle the rotation state
            isOpen = !isOpen;

            // Calculate the new rotation
            float angle = isOpen ? rotationAngle : 0;

            // Apply the rotation to the GameObject
            Lid.transform.DOLocalRotate(new Vector3(angle, 0, 0), 1f, RotateMode.Fast);
        }
    }
}
