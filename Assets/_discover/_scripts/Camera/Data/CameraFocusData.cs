using UnityEngine;

namespace Antura.Discover
{
    public struct CameraFocus
    {
        public string Id;
        [Tooltip("if null, it'll stay at current position and just focus on LookAt")]
        public Transform Origin;
        public Transform LookAt;
    }
}
