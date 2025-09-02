using UnityEngine;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "WorldSetupData", menuName = "Antura/Discover/World Setup Data")]
    public class WorldSetupData : ScriptableObject
    {
        [Header("Time")]
        public TimeProfile Time = new TimeProfile();

        [Header("Weather")]
        public WeatherProfileData Weather;

        [Header("Living Letters")]
        public LivingLettersProfile LivingLetters = new LivingLettersProfile();

        [Header("Traffic")]
        public TrafficProfile Traffic = new TrafficProfile();

        [Header("Animals")]
        public AnimalProfile Animals = new AnimalProfile();

        private void OnValidate()
        {
            LivingLetters?.Normalize();
        }
    }
}
