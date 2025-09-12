using UnityEngine;

namespace AdventurEd
{
    /// <summary>
    /// Provides the player's current heading vector projected on XZ.
    /// </summary>
    public class HeadingProvider : MonoBehaviour
    {
        [SerializeField]
        private Transform target; // player/root

        public Vector3 HeadingXZ => target ? new Vector3(target.forward.x, 0f, target.forward.z).normalized : Vector3.forward;

        public float BearingFromNorth()
        {
            return WorldNorth.I ? WorldNorth.I.GetBearingFromNorth(HeadingXZ) : 0f;
        }
    }
}
