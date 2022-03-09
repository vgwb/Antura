using UnityEngine;

namespace Antura.Minigames.Scanner
{

    public class ScannerRotateGear : MonoBehaviour
    {

        public ScannerGame game;
        public float direction;
        // Use this for initialization

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0, 0, direction * game.beltSpeed);
        }
    }
}
