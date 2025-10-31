using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public class CollectableItem : MonoBehaviour
    {
        public enum CollectableType
        {
            cookie = 1,
            coin = 2,
            item = 3,
            card = 4
        }

        public bool AutoCollect = true; // Automatically collect the item when the player collides with it
        public CollectableType Type;
        public string ItemTag; // Used to identify the item in the inventory

        [Header("Effects")]
        public GameObject particleEffectPrefab;

        [Header("Motion Settings")]
        public bool enableRotation = true; // Enable or disable rotation
        public float rotationSpeed = 100f; // Rotation speed in degrees per second
        public float bobbingAmount = 0.1f; // Amplitude of bobbing motion
        public float bobbingSpeed = 1f; // Speed of bobbing motion


        public InventoryItem ItemData; // Used to store item data for inventory

        private Vector3 startPosition;
        private float timer;

        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
            if (enableRotation)
            {
                // Rotate the object around its up axis
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

                // Create a bobbing motion up and down
                timer += Time.deltaTime * bobbingSpeed;
                float newY = startPosition.y + Mathf.Sin(timer) * bobbingAmount;
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (AutoCollect && other.CompareTag("Player"))
            {
                Collect();
            }
        }

        public void Collect()
        {
            if (particleEffectPrefab != null)
            {
                Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
            }

            if (Type == CollectableType.coin)
            {
                QuestManager.I.OnCollectCoin();
                Destroy(gameObject);
            }
            if (Type == CollectableType.cookie)
            {
                QuestManager.I.OnCollectCookie();
                UIManager.I.CookiesCounter.PlayPickupFromWorld(transform.position);
                Destroy(gameObject);
            }
            if (Type == CollectableType.item)
            {
                QuestManager.I.OnCollectItem(ItemTag);
                gameObject.SetActive(false); // Disable the item instead of destroying it
            }

            if (Type == CollectableType.card)
            {
                QuestManager.I.OnCollectItemCode(ItemTag);
                gameObject.SetActive(false); // Disable the item instead of destroying it
            }

        }
    }
}
