using UnityEngine;
using System.Collections;

namespace Antura.Minigames.SickLetters
{
    public class SickLettersCamera : MonoBehaviour
    {
        public Transform endGameCameraPose;

        // Use this for initialization
        void Start()
        {

        }

        public void moveCamera(float delay = 0)
        {
            StartCoroutine(coMoveCamera(delay));
        }

        IEnumerator coMoveCamera(float delay)
        {
            yield return new WaitForSeconds(delay);
            while (true)
            {
                transform.position = Vector3.Lerp(transform.position, endGameCameraPose.position, Time.deltaTime);
                yield return null;
            }
        }

        public IEnumerator rotatCamera(float dgree, float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            while (true)
            {
                transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, dgree, 4 * Time.deltaTime), transform.eulerAngles.y, transform.eulerAngles.z);
                yield return null;
            }
        }

    }
}
