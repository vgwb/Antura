using UnityEngine;
using UnityEngine.AI;

namespace Antura.Helpers
{
    /// <summary>
    /// Static helper class for gameplay purposes.
    /// </summary>
    static public class GameplayHelper
    {
        /// <summary>
        /// Get random point on walkable area of navmesh (id area 1).
        /// </summary>
        /// <param name="_center"></param>
        /// <param name="_range"></param>
        /// <param name="_result"></param>
        /// <returns></returns>
        public static bool RandomPointInWalkableArea(Vector3 _center, float _range, out Vector3 _result, int _areaMask = 1)
        {
            Vector3 randomPoint = _center + Random.insideUnitSphere * (_range + Random.Range(-_range / 2f, _range / 2f));
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, _areaMask))
            {
                _result = hit.position;
                return true;
            }
            _result = Vector3.zero;
            return false;
        }

        /// <summary>
        /// Lerp the current transform rotation to point towards world position "position" using t.
        /// The look-at is planar, meaning that the transform.up will be Vector3.up.
        /// </summary>
        public static void LerpLookAtPlanar(Transform transform, Vector3 position, float t)
        {
            Vector3 targetDir3D = (transform.position - position);
            if (targetDir3D.sqrMagnitude < 0.001f)
            {
                return;
            }

            Vector2 targetDir = new Vector2(targetDir3D.x, targetDir3D.z);
            Vector2 currentDir = new Vector2(transform.forward.x, transform.forward.z);

            targetDir.Normalize();
            currentDir.Normalize();

            var desiredAngle = MathHelper.AngleCounterClockwise(targetDir, Vector2.down) * Mathf.Rad2Deg;
            var currentAngle = MathHelper.AngleCounterClockwise(currentDir, Vector2.up) * Mathf.Rad2Deg;

            currentAngle = Mathf.LerpAngle(currentAngle, desiredAngle, t);

            transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
        }
    }
}
