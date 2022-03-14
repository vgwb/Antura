using UnityEngine;

namespace Antura.Minigames.ThrowBalls
{
    public class BushController : MonoBehaviour
    {
        public LetterController letterController;

        void Start()
        {
        }

        void Update()
        {
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            GameObject letter = letterController.gameObject;
            transform.position = new Vector3(letter.transform.position.x, letter.transform.position.y + 3.4f, letter.transform.position.z - 2.5f);
        }
    }
}
