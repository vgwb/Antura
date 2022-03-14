using UnityEngine;
using System.Collections;

namespace Antura.Minigames.ThrowBalls
{
    public class CrateController : MonoBehaviour
    {
        public const float MAX_SWERVE_ANGLE = 15;
        public const float SWERVE_PERIOD_IN_SECS = 3f;
        public const float FRICTION = -120;

        public CratePileController cratePileController;

        public Rigidbody rigidBody;

        private IEnumerator customGravityCoroutine;
        private IEnumerator swervingCoroutine;

        public void SetSwerving(Vector3 leftPivot, Vector3 rightPivot, float factor, float phase)
        {
            swervingCoroutine = SwerveCoroutine(leftPivot, rightPivot, factor, phase);
            StartCoroutine(swervingCoroutine);
        }

        private IEnumerator SwerveCoroutine(Vector3 leftPivot, Vector3 rightPivot, float deviationFactor, float phase)
        {
            float previousAngle = 0;
            float swerveFrequency = 1 / SWERVE_PERIOD_IN_SECS;

            while (true)
            {
                float sinValue = Mathf.Sin(2 * Mathf.PI * swerveFrequency * Time.time + phase);
                float sinSign = Mathf.Sign(sinValue);

                float lerpCoeff = Mathf.Abs(sinValue);

                Vector3 temp = transform.rotation.eulerAngles;
                temp.z = Mathf.Lerp(0, MAX_SWERVE_ANGLE * deviationFactor, lerpCoeff);

                float rotateByAngle = temp.z - previousAngle;

                Vector3 rotateByPivot = sinSign > 0 ? rightPivot : leftPivot;
                Vector3 zVector = new Vector3(0, 0, sinSign * -1);

                transform.RotateAround(rotateByPivot, zVector, rotateByAngle);
                previousAngle = temp.z;

                cratePileController.OnSwerveUpdate(this, rotateByAngle, rotateByPivot, zVector);

                yield return new WaitForFixedUpdate();
            }
        }

        public void StopSwerving()
        {
            StopCoroutine(swervingCoroutine);
        }

        public bool IsSwerving()
        {
            return swervingCoroutine != null;
        }

        public void Reset()
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            SetIsKinematic(true);
            StopAllCoroutines();
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == Constants.TAG_POKEBALL)
            {
                cratePileController.OnCrateHit(this);
            }
        }

        public void Launch(Vector3 direction, float speed)
        {
            StartCoroutine(LaunchCoroutine(direction, speed));
        }

        private IEnumerator LaunchCoroutine(Vector3 direction, float speed)
        {
            direction.Normalize();

            while (true)
            {
                Vector3 position = transform.position;

                position.x += direction.x * speed * Time.fixedDeltaTime;
                position.y += direction.y * speed * Time.fixedDeltaTime;
                position.z += direction.z * speed * Time.fixedDeltaTime;

                transform.position = position;

                speed += FRICTION * Time.fixedDeltaTime;

                if (speed < 0)
                {
                    speed = 0;
                }

                yield return new WaitForFixedUpdate();
            }
        }

        public void ApplyCustomGravity()
        {
            customGravityCoroutine = ApplyCustomGravityCoroutine();
            StartCoroutine(customGravityCoroutine);
        }

        private IEnumerator ApplyCustomGravityCoroutine()
        {
            rigidBody.isKinematic = false;

            while (true)
            {
                rigidBody.AddForce(Constants.GRAVITY, ForceMode.Acceleration);

                yield return new WaitForFixedUpdate();
            }
        }

        public void StopCustomGravity()
        {
            StopCoroutine(customGravityCoroutine);
        }

        public void VanishAfterDelay(float delay)
        {
            StartCoroutine(VanishAfterDelayCoroutine(delay));
        }

        private IEnumerator VanishAfterDelayCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            GameObject poof = Instantiate(ThrowBallsGame.instance.cratePoofPrefab, transform.position, Quaternion.identity);
            Destroy(poof, 10);
            gameObject.SetActive(false);

            ThrowBallsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Poof);
        }

        public void SetIsKinematic(bool isKinematic)
        {
            rigidBody.isKinematic = isKinematic;
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
