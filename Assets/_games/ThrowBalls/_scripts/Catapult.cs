using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.ThrowBalls
{
    public class Catapult : MonoBehaviour
    {
        public static Catapult instance;

        private MeshCollider meshCollider;

        private void Awake()
        {
            instance = this;
            meshCollider = GetComponent<MeshCollider>();
        }

        public void EnableCollider()
        {
            meshCollider.enabled = true;
        }

        public void DisableCollider()
        {
            meshCollider.enabled = false;
        }

        private void OnMouseDown()
        {
            BallController.instance.OnBallTugged();
        }
    }
}
