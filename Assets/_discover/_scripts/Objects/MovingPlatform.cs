using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Minigames.DiscoverCountry
{
    public class MovingPlatform : MonoBehaviour
    {
        public bool IsActivated;
        public float speed;
        public float distance = 2.0f;
        public float pauseDelay = 1f;

        private float currentPause = 0f;
        private Vector3 startingPosition;
        private bool movingUp = true;

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

                float direction = movingUp ? 1f : -1f;
                transform.position += Vector3.up * direction * speed * Time.deltaTime;

                if (movingUp && transform.position.y >= startingPosition.y + distance)
                {
                    movingUp = false;
                    currentPause = pauseDelay;
                }
                else if (!movingUp && transform.position.y <= startingPosition.y)
                {
                    movingUp = true;
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
