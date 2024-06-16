using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class CollectableItem : MonoBehaviour
    {
        public enum CollectableType
        {
            bone = 1,
            coin = 2
        }

        public CollectableType Type;

        void Start()
        {

        }

        public void OnTriggerEnter(Collider other)
        {
            //Debug.Log("TRIGGER");
            if (Type == CollectableType.coin)
            {
                QuestManager.I.OnCollectCoin(gameObject);
            }
            if (Type == CollectableType.bone)
            {
                QuestManager.I.OnCollectBone(gameObject);
            }
            //Destroy(this);
        }
    }
}
