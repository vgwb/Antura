using UnityEngine;

namespace Antura.Discover
{
    public class ProximityEffect : MonoBehaviour
    {
        public Material proximityMaterial;
        public Transform Player;

        public float radius = 5.0f;

        void Start()
        {
            Player = GameObject.FindWithTag("Player").transform;
            if (proximityMaterial != null && Player != null)
            {
                proximityMaterial.SetVector("_PlayerPosition", Player.position);
                proximityMaterial.SetFloat("_Radius", radius);
            }
        }

        private void Update()
        {
            if (proximityMaterial != null && Player != null)
            {
                proximityMaterial.SetVector("_PlayerPosition", Player.position);
            }
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            if (proximityMaterial != null)
            {
                // Reset to zero so it doesn't get saved in the asset
                proximityMaterial.SetVector("_PlayerPosition", Vector3.zero);
            }
#endif
        }
    }
}
