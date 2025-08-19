using System.Collections;
using UnityEngine;

namespace Antura.discover
{
    public class PetOnEarth : MonoBehaviour
    {
        [Header("Earth")]
        public Transform planetCenter;
        public float planetRadius = 10f;
        public bool autoDetectRadius = true;
        public float surfaceOffset = 0.0f;

        public float moveSpeed = 2.0f;
        public Vector2 idleTimeRange = new Vector2(0.5f, 2.0f);
        [Tooltip("Min/Max arc angle per hop (degrees).")]
        public Vector2 arcAngleRange = new Vector2(20f, 120f);

        [Header("Orientation")]
        [Tooltip("Face the tangent direction while moving.")]
        public bool orientAlongTangent = true;
        [Tooltip("Optional child to rotate instead of the root (leave null to rotate this transform).")]
        public Transform modelToRotate;

        [Header("Start")]
        [Tooltip("Clamp to sphere at start, preserving current direction.")]
        public bool snapToSphereOnStart = true;

        [Header("Debug")]
        public bool drawGizmos = false;

        Coroutine _loop;
        Vector3 _center;

        void Start()
        {
            _center = planetCenter ? planetCenter.position : Vector3.zero;

            // Ensure reasonable params
            arcAngleRange.x = Mathf.Clamp(arcAngleRange.x, 0.1f, 179f);
            arcAngleRange.y = Mathf.Clamp(arcAngleRange.y, arcAngleRange.x, 179f);

            if (autoDetectRadius)
                RecalculateRadius();

            if (snapToSphereOnStart)
                ClampToSpherePreserveDirection();

            _loop = StartCoroutine(WanderLoop());
        }

        void OnDisable()
        {
            if (_loop != null)
                StopCoroutine(_loop);
            _loop = null;
        }

        /// <summary>Recompute radius from current position and center.</summary>
        public void RecalculateRadius()
        {
            _center = planetCenter ? planetCenter.position : Vector3.zero;
            float dist = (transform.position - _center).magnitude;
            // If placed exactly at center (editor mishap), default to previous or 1.
            float detected = Mathf.Max(0.01f, dist - surfaceOffset);
            // If designer placed object not exactly on surface, still infer radius robustly.
            planetRadius = detected;
        }

        /// <summary>Clamp current position to the sphere using current direction.</summary>
        void ClampToSpherePreserveDirection()
        {
            Vector3 dir = (transform.position - _center);
            if (dir.sqrMagnitude < 1e-8f)
            {
                // Degenerate: pick a random direction if we started at the center.
                dir = Random.onUnitSphere;
            }
            else
                dir.Normalize();

            transform.position = _center + dir * (planetRadius + surfaceOffset);

            // Keep a stable forward that’s perpendicular to up
            Vector3 forwardHint = Vector3.Cross(dir, Vector3.up);
            if (forwardHint.sqrMagnitude < 1e-4f)
                forwardHint = Vector3.Cross(dir, Vector3.right);
            SetRotationForUpAndTangent(dir, forwardHint);
        }

        IEnumerator WanderLoop()
        {
            while (true)
            {
                float idle = Random.Range(idleTimeRange.x, idleTimeRange.y);
                if (idle > 0f)
                    yield return new WaitForSeconds(idle);

                if (autoDetectRadius)
                {
                    // In case the center moved (rare), keep radius in sync with our altitude.
                    RecalculateRadius();
                    // And reclamp to avoid drift
                    ClampToSpherePreserveDirection();
                }

                Vector3 startDir = (transform.position - _center).normalized;
                Vector3 targetDir;
                float angleDeg;

                int safety = 0;
                do
                {
                    targetDir = Random.onUnitSphere;
                    angleDeg = Vector3.Angle(startDir, targetDir);
                }
                while ((angleDeg < arcAngleRange.x || angleDeg > arcAngleRange.y) && ++safety < 50);

                if (safety >= 50)
                {
                    targetDir = Quaternion.AngleAxis(Random.Range(arcAngleRange.x, arcAngleRange.y),
                                                     Random.onUnitSphere) * startDir;
                    angleDeg = Vector3.Angle(startDir, targetDir);
                }

                yield return MoveAlongArc(startDir, targetDir, angleDeg);
            }
        }

        IEnumerator MoveAlongArc(Vector3 startDir, Vector3 targetDir, float angleDeg)
        {
            Vector3 axis = Vector3.Cross(startDir, targetDir);
            float axisMag = axis.magnitude;
            if (axisMag < 1e-5f)
                yield break;

            axis /= axisMag;

            float angleRad = angleDeg * Mathf.Deg2Rad;
            float arcLength = planetRadius * angleRad;
            float duration = Mathf.Max(0.01f, arcLength / Mathf.Max(0.01f, moveSpeed));

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                float u = Mathf.Clamp01(t);

                Quaternion rot = Quaternion.AngleAxis(angleDeg * u, axis);
                Vector3 dir = (rot * startDir).normalized;
                Vector3 up = dir;
                Vector3 pos = _center + dir * (planetRadius + surfaceOffset);

                transform.position = pos;

                if (orientAlongTangent)
                {
                    Vector3 tangent = Vector3.Cross(axis, dir).normalized;
                    SetRotationForUpAndTangent(up, tangent);
                }

                yield return null;
            }

            Vector3 finalDir = targetDir.normalized;
            transform.position = _center + finalDir * (planetRadius + surfaceOffset);
            if (orientAlongTangent)
            {
                Vector3 tangent = Vector3.Cross(axis, finalDir).normalized;
                SetRotationForUpAndTangent(finalDir, tangent);
            }
        }

        void SetRotationForUpAndTangent(Vector3 up, Vector3 forwardHint)
        {
            var target = modelToRotate ? modelToRotate : transform;

            if (forwardHint.sqrMagnitude < 1e-6f || Mathf.Abs(Vector3.Dot(forwardHint.normalized, up.normalized)) > 0.999f)
                forwardHint = Vector3.Cross(up, Vector3.right).sqrMagnitude > 1e-6f ? Vector3.Cross(up, Vector3.right) : Vector3.Cross(up, Vector3.forward);

            target.rotation = Quaternion.LookRotation(forwardHint.normalized, up.normalized);
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (!Application.isPlaying && autoDetectRadius)
            {
                _center = planetCenter ? planetCenter.position : Vector3.zero;
                float dist = (transform.position - _center).magnitude;
                planetRadius = Mathf.Max(0.01f, dist - surfaceOffset);
            }
        }

        [ContextMenu("Recalculate Radius Now")]
        void Ctx_RecalcRadius()
        {
            RecalculateRadius();
            ClampToSpherePreserveDirection();
        }
#endif

        void OnDrawGizmosSelected()
        {
            if (!drawGizmos)
                return;
            Vector3 c = planetCenter ? planetCenter.position : Vector3.zero;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(c, planetRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, c);
        }
    }
}
