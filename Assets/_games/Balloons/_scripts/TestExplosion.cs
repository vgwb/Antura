using UnityEngine;
using System.Collections;

namespace Antura.Minigames.Balloons
{
    public class TestExplosion : MonoBehaviour
    {
        public Collider explosiveCollider;

        [Range(0, 100)]
        public float radius;
        [Range(0, 100)]
        public float power;


        void OnMouseDown()
        {
            Explode();
        }

        void Explode()
        {
            Vector3 explosionPosition = transform.position + Vector3.up;
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);

            foreach (Collider hit in colliders)
            {
                if (hit.gameObject.GetComponent<TestPhysics>() != null && hit.gameObject.GetComponent<TestPhysics>().affectedByExplosions)
                {
                    Rigidbody hitBody = hit.GetComponent<Rigidbody>();

                    if (hitBody != null)
                    {
                        var direction = (hitBody.transform.position - transform.position).normalized;
                        var distance = (hitBody.transform.position - transform.position).magnitude;
                        var displacement = (direction * power) / distance;
                        displacement.z = 0f;

                        Knockback(hitBody, displacement);
                    }
                }

            }
        }

        void Knockback(Rigidbody hitBody, Vector3 force)
        {
            StartCoroutine(Knockback_Coroutine(hitBody, force));
        }

        IEnumerator Knockback_Coroutine(Rigidbody hitBody, Vector3 force)
        {
            float duration = 0.5f;
            float progress = 0f;
            float percentage = 0f;

            var initialPosition = hitBody.transform.position;
            var finalPosition = hitBody.transform.position + force;

            while (progress < duration && hitBody != null)
            {
                hitBody.transform.position = Vector3.Lerp(initialPosition, finalPosition, percentage);
                progress += Time.deltaTime;
                percentage = progress / duration;
                percentage = Mathf.Sin(percentage * Mathf.PI * 0.5f);

                yield return null;
            }
        }

        /*
        void FixedUpdate()
        {
            if (adjustMiddleBalloon)
            {
                if (adjustmentProgress < adjustmentDuration)
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, adjustedLocalPosition, adjustmentProgressRatio);
                    adjustmentProgress += Time.deltaTime;
                    adjustmentProgressRatio = adjustmentProgress / adjustmentDuration;
                }
                else
                {
                    adjustMiddleBalloon = false;
                }
            }
        }
         * */
    }
}
