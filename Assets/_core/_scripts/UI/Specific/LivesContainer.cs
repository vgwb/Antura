using UnityEngine;

namespace Antura.UI
{
    // TODO refactor: not clear where this is used
    public class LivesContainer : MonoBehaviour
    {
        public GameObject Heart1;
        public GameObject Heart2;
        public GameObject Heart3;
        public GameObject Heart4;

        public void SetLives(int howmany)
        {
            switch (howmany)
            {
                case 4:
                    Heart1.SetActive(true);
                    Heart2.SetActive(true);
                    Heart3.SetActive(true);
                    Heart4.SetActive(true);
                    break;
                case 3:
                    Heart1.SetActive(true);
                    Heart2.SetActive(true);
                    Heart3.SetActive(true);
                    Heart4.SetActive(false);
                    break;
                case 2:
                    Heart1.SetActive(true);
                    Heart2.SetActive(true);
                    Heart3.SetActive(false);
                    Heart4.SetActive(false);
                    break;
                case 1:
                    Heart1.SetActive(true);
                    Heart2.SetActive(false);
                    Heart3.SetActive(false);
                    Heart4.SetActive(false);
                    break;
                case 0:
                    Heart1.SetActive(false);
                    Heart2.SetActive(false);
                    Heart3.SetActive(false);
                    Heart4.SetActive(false);
                    break;
            }
        }
    }
}
