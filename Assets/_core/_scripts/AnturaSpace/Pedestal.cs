using UnityEngine;

namespace Antura.AnturaSpace
{
    public class Pedestal : MonoBehaviour
    {
        [Range(0, 360)]
        public float Angle;

        public bool Activated;

        float targetAngle;

        void Update()
        {
            if (Activated)
            {
                targetAngle = Mathf.LerpAngle(targetAngle, Angle, Time.deltaTime * 8.0f);
            }
            else
            {
                targetAngle = Mathf.LerpAngle(targetAngle, 0, Time.deltaTime * 8.0f);
            }
            transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.up);
        }
    }
}
