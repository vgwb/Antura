using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Antura.Minigames.DiscoverCountry.Interaction
{
    public class RotateBy : MonoBehaviour
    {
        public GameObject ObjectToRotate;

        public float rotationAngle = 90f;

        public bool OpenJustOnce = true;

        private bool isOpen = false;
        void Start()
        {
            OpenJustOnce = true;
        }

        public void Open()
        {
            if (!OpenJustOnce || (OpenJustOnce && !isOpen))
                ToggleRotationAngle();
        }

        void ToggleRotationAngle()
        {
            // Toggle the rotation state
            isOpen = !isOpen;
            // Calculate the new rotation
            float angle = isOpen ? rotationAngle : 0;

            // Apply the rotation to the GameObject
            ObjectToRotate.transform.DOLocalRotate(new Vector3(angle, 0, 0), 1f, RotateMode.Fast);
        }
    }
}
