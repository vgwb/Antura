using UnityEngine;

namespace Antura.Discover
{
    public class PlayerTrigger : Actable
    {
        public PlayerSpawnPoint SpawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") && other.GetComponentInParent<PlayerController>() == null)
                return;

            if (SpawnPoint == null)
            {
                Debug.LogWarning($"PlayerTrigger '{name}' has no SpawnPoint assigned.", this);
                return;
            }

            ActionManager.I?.SetPlayerSpawnPoint(SpawnPoint.gameObject);
        }
    }
}
