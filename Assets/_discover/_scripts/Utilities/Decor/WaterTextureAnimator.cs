using UnityEngine;

namespace Antura.Discover
{
    [RequireComponent(typeof(Renderer))]
    public class WaterTextureAnimator : MonoBehaviour
    {
        [Header("Scroll Settings")]
        public Vector2 scrollSpeed = new Vector2(0.05f, 0f);
        public bool pingPong = false;
        public float pingPongSpeed = 0.5f;

        private Renderer rend;
        private Material mat;
        private Vector2 currentOffset;

        void Awake()
        {
            rend = GetComponent<Renderer>();
            mat = rend.material;
        }

        void Update()
        {
            if (pingPong)
            {
                float t = Mathf.PingPong(Time.time * pingPongSpeed, 1f) * 2f - 1f;
                currentOffset += scrollSpeed * t * Time.deltaTime;
            }
            else
            {
                currentOffset += scrollSpeed * Time.deltaTime;
            }

            mat.mainTextureOffset = currentOffset;
        }
    }
}
