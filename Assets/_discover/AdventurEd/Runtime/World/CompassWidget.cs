using UnityEngine;
using UnityEngine.UI;

namespace AdventurEd
{
    /// <summary>
    /// Rotates a UI needle to indicate North relative to the player's heading.
    /// If showNorth = true, needle points to North (classic compass).
    /// If showNorth = false, needle points to player's direction (bearing), easier for kids
    /// </summary>
    public class CompassWidget : MonoBehaviour
    {
        [Header("Sources")]
        [SerializeField]
        [Tooltip("If Empty, will search for Player.headingProvider")]
        private HeadingProvider headingProvider;

        [Header("UI")]
        [SerializeField]
        private RectTransform needle; // pivot at center, tip up
        [SerializeField]
        private Image dial;           // optional background

        [Header("Options")]
        [SerializeField]
        [Tooltip("If true, points to the north, if false, show player's direction")]
        private bool showNorth = true;
        [SerializeField]
        private float smoothSpeed = 12f;   // UI smoothing

        private float _displayAngle;

        void Awake() {
            if (headingProvider == null) {
                headingProvider = GameObject.FindWithTag("Player").GetComponent<HeadingProvider>();
            }
        }

        void Reset() {
            smoothSpeed = 12f;
            showNorth = true;
        }

        void Update()
        {
            if (headingProvider == null || needle == null || WorldNorth.I == null) return;

            // Bearing of player from North (0..360, clockwise)
            float bearingCW = headingProvider.BearingFromNorth();

            // If showing North, rotate needle opposite of heading so it points North.
            // If showing heading, rotate needle to player's bearing.
            float targetAngle = showNorth ? -bearingCW : bearingCW;

            _displayAngle = Mathf.LerpAngle(_displayAngle, targetAngle, Time.deltaTime * smoothSpeed);
            needle.localEulerAngles = new Vector3(0, 0, _displayAngle);
        }
    }
}
