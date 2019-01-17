using System;
using UnityEngine;

namespace Antura.Collectible
{
    public class CollectibleBone : MonoBehaviour
    {
        private bool _collected;
        public Action OnPickUpBone;

        public void Initialise(float duration)
        {
            _collected = false;

            Invoke("DestroyObject", duration);
        }

        public void OnMouseDown()
        {
            if (_collected) {
                return;
            }

            _collected = true;

            if (OnPickUpBone != null) {
                OnPickUpBone.Invoke();
            }

            DestroyObject();
        }

        void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}