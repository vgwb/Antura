using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Minigames.DiscoverCountry
{
    public class MovingPlatform : MonoBehaviour
    {
        public enum Axis { X, Y, Z }
        [Header("Movement Axis")]
        public bool IsActivated;

        public Axis MoveAxis = Axis.Y;
        public float speed;
        public float distance = 2.0f;
        public float pauseDelay = 1f;

        private float currentPause = 0f;
        private Vector3 startingPosition;
        private bool movingPositive = true;

        void Start()
        {
            startingPosition = transform.position;
        }

        void Update()
        {
            if (IsActivated)
            {
                if (currentPause > 0f)
                {
                    currentPause -= Time.deltaTime;
                    return;
                }

                Vector3 direction = Vector3.zero;
                switch (MoveAxis)
                {
                    case Axis.X:
                        direction = Vector3.right;
                        break;
                    case Axis.Y:
                        direction = Vector3.up;
                        break;
                    case Axis.Z:
                        direction = Vector3.forward;
                        break;
                }

                float sign = movingPositive ? 1f : -1f;
                transform.position += direction * sign * speed * Time.deltaTime;

                float moved = 0f;
                switch (MoveAxis)
                {
                    case Axis.X:
                        moved = transform.position.x - startingPosition.x;
                        break;
                    case Axis.Y:
                        moved = transform.position.y - startingPosition.y;
                        break;
                    case Axis.Z:
                        moved = transform.position.z - startingPosition.z;
                        break;
                }

                if (movingPositive && moved >= distance)
                {
                    movingPositive = false;
                    currentPause = pauseDelay;
                }
                else if (!movingPositive && moved <= 0f)
                {
                    movingPositive = true;
                    currentPause = pauseDelay;
                }
            }
        }

        public void Activate(bool status)
        {
            IsActivated = status;
        }
    }
}
