using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class LightBeam : MonoBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            gameObject.SetActive(false);
        }
    }
}
