using UnityEngine;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "WeatherProfile", menuName = "Antura/World/Weather Profile")]
    public class WeatherProfileData : ScriptableObject
    {
        public Material Skybox;
        public Color AmbientColor = Color.white;
        public bool Fog = true;
        public float FogDensity = 0.002f;
        public bool Rain;
        public AudioClip Ambience;
    }
}
