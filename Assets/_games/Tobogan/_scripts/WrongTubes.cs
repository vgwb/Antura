using UnityEngine;

namespace Antura.Minigames.Tobogan
{
    public class WrongTubes : MonoBehaviour
    {
        public WrongTube[] tubes;

        void Start()
        {

        }

        public void DropLetter(System.Action callback)
        {
            tubes[Random.Range(0, tubes.Length)].DropLetter(callback);
        }
    }
}
