using UnityEngine;

namespace Antura.Minigames.Scanner
{
    public class ScannerScrollBelt : MonoBehaviour
    {

        const float BELT_FACTOR = -0.4f;

        public ScannerGame game;
        private Renderer rend;

        void Start()
        {
            rend = GetComponent<Renderer>();
        }

        void Update()
        {
            float offset = Time.time * BELT_FACTOR * game.beltSpeed;
            rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }

    }
}
