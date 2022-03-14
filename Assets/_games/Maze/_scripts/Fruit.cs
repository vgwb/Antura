using UnityEngine;

namespace Antura.Minigames.Maze
{
    public class Fruit : MonoBehaviour
    {

        public float rotationSpeed = 2.0f;

        void Update()
        {
            transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
        }
    }
}
