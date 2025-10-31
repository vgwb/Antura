using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public class LightBeam : MonoBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            gameObject.SetActive(false);
        }
    }
}
