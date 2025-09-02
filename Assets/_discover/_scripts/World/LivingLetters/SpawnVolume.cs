using UnityEngine;

namespace Antura.Discover
{
    public class SpawnVolume : MonoBehaviour
    {
        public float Radius = 8f;
        public int MaxLive = 12;
        public bool Enabled = true;

        public Vector3 RandomPointInside()
        {
            var r = Random.insideUnitCircle * Radius;
            return transform.position + new Vector3(r.x, 0f, r.y);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.2f);
            Gizmos.DrawSphere(transform.position, Radius);
        }
    }
}
