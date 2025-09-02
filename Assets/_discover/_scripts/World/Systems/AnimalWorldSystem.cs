using UnityEngine;
using System;

namespace Antura.Discover
{

    [Serializable]
    public class AnimalProfile
    {
        public int MaxLive = 20;
        public float SpawnEverySeconds = 4f;
    }

    public class AnimalWorldSystem : MonoBehaviour, IWorldSystem
    {
        [Header("Default")]
        public AnimalProfile DefaultProfile = new AnimalProfile();
        public int MaxLive = 20;
        public float SpawnEverySeconds = 4f;

        void OnEnable() { ApplyProfile(DefaultProfile); }

        public void ApplyProfile(AnimalProfile p)
        {
            if (p == null)
                return;
            MaxLive = Mathf.Max(0, p.MaxLive);
            SpawnEverySeconds = Mathf.Max(0.1f, p.SpawnEverySeconds);
        }
    }
}
