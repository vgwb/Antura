using Antura.Dog;
using UnityEngine;

namespace Antura.Minigames.Balloons
{
    public class RunningAnturaController : MonoBehaviour
    {
        public AnturaAnimationController anturaAnimator;
        public Vector3 startingPosition1;
        public Vector3 startingPosition2;
        public float runningSpeed;

        void Start()
        {
            anturaAnimator.DoCharge(null);
        }

        void FixedUpdate()
        {
            if (transform.position.x > startingPosition2.x)
            {
                transform.position = startingPosition2;
                transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                gameObject.SetActive(false);
            }

            if (transform.position.x < startingPosition1.x)
            {
                transform.position = startingPosition1;
                transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                gameObject.SetActive(false);
            }
            else
            {
                transform.Translate(runningSpeed * Vector3.back); // run, doggy, run!
            }
        }
    }
}
