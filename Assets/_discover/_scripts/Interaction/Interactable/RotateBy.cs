using DG.DeInspektor.Attributes;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Interaction
{
    public class RotateBy : ActableAbstract
    {
        public GameObject ObjectToRotate;

        public RotationAxis rotationAxis = RotationAxis.Y;

        public float rotationAngle = 90f;

        public bool OpenJustOnce = true;

        private Vector3 axisVector;


        private bool isOpen = false;
        void Start()
        {
            switch (rotationAxis)
            {
                case RotationAxis.X:
                    axisVector = Vector3.right;
                    break;
                case RotationAxis.Y:
                    axisVector = Vector3.up;
                    break;
                case RotationAxis.Z:
                    axisVector = Vector3.forward;
                    break;
            }

            OpenJustOnce = true;
        }
        public override void OnTrigger()
        {
            if (!OpenJustOnce || (OpenJustOnce && !isOpen))
                ToggleRotationAngle();
        }

        void ToggleRotationAngle()
        {
            Debug.Log("ToggleRotationAngle: " + ObjectToRotate.name + " to " + (isOpen ? "closed" : "open"));
            // Toggle the rotation state
            isOpen = !isOpen;
            // Calculate the new rotation
            float angle = isOpen ? rotationAngle : 0;

            // Apply the rotation to the GameObject
            ObjectToRotate.transform.DOLocalRotate(angle * axisVector, 1f, RotateMode.Fast);
        }

        [DeMethodButton(mode = DeButtonMode.Default)]
        void TestTrigger()
        {
            Trigger();
        }
    }
}
