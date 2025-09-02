using System;
using UnityEngine;


namespace Antura.Discover
{

    [Serializable]
    public class TrafficProfile
    {
        [Range(0, 1)] public float Density = 0.6f;
    }

    public class TrafficWorldSystem : MonoBehaviour, IWorldSystem
    {
        [Header("Default")]
        public TrafficProfile DefaultProfile = new TrafficProfile();

        [Range(0, 1)] public float Density = 0.6f;

        void OnEnable() { ApplyProfile(DefaultProfile); }

        public void ApplyProfile(TrafficProfile p)
        {
            if (p == null)
                return;
            Density = Mathf.Clamp01(p.Density);
            // TODO: propagate to followers/spawners if needed
        }
    }
}
