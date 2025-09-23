using UnityEngine;

namespace AdventurEd
{
    /// <summary>
    /// Defines the 'North' direction for the current world.
    /// Use either a world-space angle (degrees clockwise from +Z) or a marker Transform.
    /// </summary>
    public class WorldNorth : MonoBehaviour
    {
        public enum Mode
        {
            AngleFromWorldForward,
            MarkerTransform
        }

        [SerializeField] private Mode mode = Mode.AngleFromWorldForward;

        [Tooltip("Degrees clockwise from Unity +Z (forward) on the XZ plane. 0 = +Z, 90 = +X.")]
        [SerializeField]
        [Range(0f, 360f)]
        private float northAngleDegrees = 0f;

        [Tooltip("Optional: a Transform that points toward North (its forward projected on XZ is used).")]
        [SerializeField]
        private Transform northMarker;

        private static WorldNorth _instance;
        public static WorldNorth I
        {
            get
            {
                if (_instance == null)
                    _instance = FindFirstObjectByType<WorldNorth>();
                return _instance;
            }
        }

        void Awake()
        {
            _instance = this;
        }

        /// <summary>
        /// Returns a normalized world-space vector on XZ pointing to North.
        /// </summary>
        public Vector3 GetNorthVector()
        {
            Vector3 n;
            if (mode == Mode.MarkerTransform && northMarker != null)
            {
                n = new Vector3(northMarker.forward.x, 0f, northMarker.forward.z);
            }
            else
            {
                // Rotate +Z by angle clockwise around Y
                n = Quaternion.AngleAxis(northAngleDegrees, Vector3.up) * Vector3.forward;
            }
            n.y = 0f;
            return n.sqrMagnitude > 0.0001f ? n.normalized : Vector3.forward;
        }

        /// <summary>
        /// heading angle (degrees) from North to a direction (clockwise).
        /// </summary>
        public float GetBearingFromNorth(Vector3 worldDir)
        {
            var north = GetNorthVector();
            var dir = new Vector3(worldDir.x, 0f, worldDir.z).normalized;
            if (dir.sqrMagnitude < 1e-6f)
                return 0f;
            float angle = Vector3.SignedAngle(north, dir, Vector3.up); // + = CCW from north
            // Convert to clockwise bearing 0..360 (0 = North, 90 = East)
            float bearingCW = Mathf.Repeat(360f - angle, 360f);
            return bearingCW;
        }
    }
}
