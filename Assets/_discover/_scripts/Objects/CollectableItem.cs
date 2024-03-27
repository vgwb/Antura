using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class CollectableItem : MonoBehaviour
    {
        void Start()
        {

        }

        public void OnTriggerEnter(Collider other)
        {
            //Debug.Log("TRIGGER");
            QuestManager.I.OnCollectCoin(gameObject);
            //Destroy(this);
        }
    }
}
