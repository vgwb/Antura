using UnityEngine;

namespace Antura.Discover
{
    [ExecuteAlways]
    public class WeatherWorldSystem : MonoBehaviour, IWorldSystem
    {
        [Header("Profile")]
        public WeatherProfileData Profile;

        [Header("Sun")]
        public Light Sun;
        [Tooltip("Hour the sun rises above the horizon.")]
        public float SunriseHour = 6f;
        [Tooltip("Hour the sun sets below the horizon.")]
        public float SunsetHour = 18f;

        [Tooltip("Sun elevation at night (below horizon).")]
        public float NightElevationDeg = -10f;
        [Tooltip("Sun elevation at noon.")]
        public float NoonElevationDeg = 60f;

        [Tooltip("Azimuth (yaw) when Hour = 0 (midnight).")]
        public float AzimuthAtMidnightDeg = 0f;
        [Header("Intensity (optional)")]
        public bool ControlIntensity = true;
        public float NightIntensity = 0f;
        public float DayIntensity = 1f;

        [Header("Rain & Ambience (optional)")]
        public ParticleSystem RainSystem;
        public AudioSource AmbienceSource;

        TimeWorldSystem _time;

        void OnEnable()
        {
            ApplyProfile(Profile);

            // Hook to time (if present)
            _time = ResolveTimeSystem();
            if (_time != null)
                _time.OnHourChanged += OnHourChanged;

            float hour = _time != null ? _time.CurrentHour : 12f;
            UpdateSunForHour(hour);
        }

        void OnDisable()
        {
            if (_time != null)
                _time.OnHourChanged -= OnHourChanged;
        }

        public void ApplyProfile(WeatherProfileData p)
        {
            if (!p)
                return;

            if (p.Skybox)
                RenderSettings.skybox = p.Skybox;
            RenderSettings.ambientLight = p.AmbientColor;
            RenderSettings.fog = p.Fog;
            RenderSettings.fogDensity = p.FogDensity;

            if (RainSystem)
            {
                var em = RainSystem.emission;
                em.enabled = p.Rain;
                if (p.Rain && !RainSystem.isEmitting)
                    RainSystem.Play();
                if (!p.Rain && RainSystem.isEmitting)
                    RainSystem.Stop();
            }

            if (AmbienceSource)
            {
                AmbienceSource.clip = p.Ambience;
                AmbienceSource.loop = true;
                if (p.Ambience)
                    AmbienceSource.Play();
                else
                    AmbienceSource.Stop();
            }
        }

        void OnHourChanged(float hour) => UpdateSunForHour(hour);

        private TimeWorldSystem ResolveTimeSystem()
        {
            if (_time != null)
                return _time;
            var wm = WorldManager.I;
            var time = wm != null ? wm.Get<TimeWorldSystem>() : null;
            if (time == null)
            {
                // Fallback: find in scene (include inactive)
                var all = Resources.FindObjectsOfTypeAll<TimeWorldSystem>();
                if (all != null && all.Length > 0)
                    time = all[0];
            }
            _time = time;
            return _time;
        }

        public void UpdateSunForHour(float hour)
        {
            //            Debug.Log("UpdateSunForHour " + hour);
            if (!Sun)
                return;

            // Wrap hour
            hour = (float)(((double)hour % 24d + 24d) % 24d);

            // Elevation: 0 at sunrise/sunset, peak at noon — simple sine between Sunrise..Sunset
            float elev = NightElevationDeg;
            float tDay = Mathf.InverseLerp(SunriseHour, SunsetHour, hour);

            float dayFactor = 0f;
            if (hour >= SunriseHour && hour <= SunsetHour)
            {
                // tDay 0..1 over daytime. Use sin(pi * t) to get 0 at ends, 1 at noon.
                dayFactor = Mathf.Sin(tDay * Mathf.PI);
                elev = Mathf.Lerp(NightElevationDeg, NoonElevationDeg, dayFactor);
            }

            // Azimuth: simple full rotation over 24h
            float az = AzimuthAtMidnightDeg + (hour / 24f) * 360f;

            Sun.transform.rotation = Quaternion.Euler(elev, az, 0f);

            if (ControlIntensity)
                Sun.intensity = Mathf.Lerp(NightIntensity, DayIntensity, dayFactor);
        }
    }
}
