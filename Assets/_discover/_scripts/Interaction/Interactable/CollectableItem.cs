using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public class CollectableItem : MonoBehaviour
    {
        public enum CollectableType
        {
            bone = 1,
            coin = 2,
            item = 3
        }

        public CollectableType Type;

        public string ItemTag; // Used to identify the item in the inventory

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
            if (Type == CollectableType.item)
            {
                QuestManager.I.OnCollectItem(gameObject);
            }
            //Destroy(this);
        }
    }
}
