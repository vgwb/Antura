using UnityEngine;

namespace Antura.Minigames.ThrowBalls
{
    public class ArrowHeadController : MonoBehaviour
    {
        public static ArrowHeadController instance;

        void Awake()
        {
            instance = this;
        }

        public void OnUpdate()
        {
            if (Mathf.Approximately(ArrowBodyController.instance.transform.localScale.x, 0f))
            {
                transform.position = new Vector3(0, 0, -100f);
                return;
            }

            Vector3 arrowBodyRotation = ArrowBodyController.instance.transform.rotation.eulerAngles;
            Vector3 arrowHeadRotation = arrowBodyRotation;
            arrowHeadRotation.y += 180;
            transform.rotation = Quaternion.Euler(arrowHeadRotation);

            Vector3 distanceFromHead = new Vector3();
            distanceFromHead.x = ArrowBodyController.instance.transform.localScale.x * 0.5f + 0.5f;

            Vector3 rotatedVector = new Vector3();
            rotatedVector.x = Mathf.Cos(-Mathf.Deg2Rad * transform.localRotation.eulerAngles.y) * distanceFromHead.x - Mathf.Sin(-Mathf.Deg2Rad * transform.localRotation.eulerAngles.y) * distanceFromHead.z;
            rotatedVector.z = Mathf.Sin(-Mathf.Deg2Rad * transform.localRotation.eulerAngles.y) * distanceFromHead.x + Mathf.Cos(-Mathf.Deg2Rad * transform.localRotation.eulerAngles.y) * distanceFromHead.z;

            transform.position = ArrowBodyController.instance.transform.position + rotatedVector;
        }

        public void Reset()
        {
            transform.position = new Vector3(0, 0, -100f);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
