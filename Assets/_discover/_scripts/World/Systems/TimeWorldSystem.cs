using UnityEngine;
using System;

namespace Antura.Discover
{

    [Serializable]
    public class TimeProfile
    {
        public bool EnableTime = false;
        [Range(0, 24)] public float Hour = 10f;
        [Tooltip("0 = static; 1 = 1 world hour per real minute.")]
        public float HoursPerRealMinute = 0f;
    }

    public class TimeWorldSystem : MonoBehaviour, IWorldSystem
    {
        [Header("Default (used if no override)")]
        public TimeProfile DefaultProfile = new TimeProfile();

        [Range(0f, 24f)]
        public float CurrentHour = 10f;

        [Tooltip("0 = no auto progression. 1 = 1 world hour per real minute.")]
        public float WorldHoursPerRealMinute = 0f;

        public event Action<float> OnHourChanged; // new hour (0..24)

        public float NormalizedTime => CurrentHour / 24f;

        public void ApplyProfile(TimeProfile p)
        {
            if (p == null)
                return;
            SetHour(p.Hour);
            WorldHoursPerRealMinute = Mathf.Max(0f, p.HoursPerRealMinute);
        }

        void Update()
        {
            if (WorldHoursPerRealMinute <= 0f)
                return;

            float dh = (WorldHoursPerRealMinute / 60f) * Time.deltaTime;
            SetHour(CurrentHour + dh);
        }

        public void SetHour(float hour)
        {
            hour = (float)(((double)hour % 24d + 24d) % 24d); // wrap 0..24
            if (!Mathf.Approximately(hour, CurrentHour))
            {
                CurrentHour = hour;
                OnHourChanged?.Invoke(CurrentHour);
            }
        }
    }
}
