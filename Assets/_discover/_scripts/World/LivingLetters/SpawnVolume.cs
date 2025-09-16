using UnityEngine;

namespace Antura.Discover
{
    public class SpawnVolume : MonoBehaviour
    {
        public float Radius = 8f;
        public int MaxLive = 12;
        public bool Enabled = true;

        [Header("Content Source (optional)")]
        [Tooltip("If set, LivingLetters spawned inside this volume will build their pools from this Topic instead of the spawner's global Topic.")]
        public TopicData TopicOverride;

        [Header("Yarn integration (optional)")]
        [Tooltip("Yarn spawn_group label used in quest scripts (e.g., 'buyers'). Used only for authoring/context.")]
        public string SpawnGroup;
        [Tooltip("Nodes to trigger when interacting with LivingLetters spawned in this volume. If empty and SpawnGroup is set, SpawnGroup will be used as the node name.")]
        public string[] YarnNodes;

        public Vector3 RandomPointInside()
        {
            var r = Random.insideUnitCircle * Radius;
            return transform.position + new Vector3(r.x, 0f, r.y);
        }

        public string PickYarnNode()
        {
            if (YarnNodes != null && YarnNodes.Length > 0)
            {
                int i = Random.Range(0, YarnNodes.Length);
                return YarnNodes[i];
            }
            if (!string.IsNullOrWhiteSpace(SpawnGroup))
                return SpawnGroup; // fallback: use the group name directly as node id
            return null;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.2f);
            Gizmos.DrawSphere(transform.position, Radius);
        }
    }
}
