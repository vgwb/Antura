using System;
using UnityEngine;

namespace Antura.Discover
{
    public class CameraFocusData : MonoBehaviour
    {
        public string Id;
        [Tooltip("if null, it'll stay at current position and just focus on LookAt")]
        public Transform Origin;
        public Transform LookAt;
    }


}
