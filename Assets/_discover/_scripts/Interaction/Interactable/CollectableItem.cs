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

        [Header("Effects")]
        public GameObject particleEffectPrefab;

        [Header("Motion Settings")]
        public float rotationSpeed = 100f; // Rotation speed in degrees per second
        public float bobbingAmount = 0.1f; // Amplitude of bobbing motion
        public float bobbingSpeed = 1f; // Speed of bobbing motion

        private Vector3 startPosition;
        private float timer;

        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
            // Rotate the object around its up axis
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

            // Create a bobbing motion up and down
            timer += Time.deltaTime * bobbingSpeed;
            float newY = startPosition.y + Mathf.Sin(timer) * bobbingAmount;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (particleEffectPrefab != null)
                {
                    Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
                }

                if (Type == CollectableType.coin)
                {
                    QuestManager.I.OnCollectCoin();
                }
                if (Type == CollectableType.bone)
                {
                    QuestManager.I.OnCollectBone();
                }
                if (Type == CollectableType.item)
                {
                    QuestManager.I.OnCollectItem(ItemTag);
                }

                Destroy(gameObject);
            }

        }
    }
}
