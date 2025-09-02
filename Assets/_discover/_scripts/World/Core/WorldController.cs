using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    [DisallowMultipleComponent]
    public class WorldController : MonoBehaviour, IWorldAPI
    {
        [Header("Identity")]
        public string WorldId = "city";

        [Header("Defaults")]
        public WorldSetupData DefaultSetup;
        public bool AutoApplyOnAwake = true;
        private readonly Dictionary<Type, IWorldSystem> _services = new();

        void Awake()
        {
            AutoRegisterServicesInChildren(includeInactive: true);
            if (AutoApplyOnAwake && DefaultSetup)
                ApplySetup(DefaultSetup);
        }

        public WorldSetupData GetEffectiveSetup(WorldSetupData questOverride)
            => questOverride ? questOverride : DefaultSetup;


        public void RegisterService(IWorldSystem service)
        {
            if (service == null)
                return;
            _services[service.GetType()] = service;
        }

        public T Get<T>() where T : class, IWorldSystem
        {
            if (_services.TryGetValue(typeof(T), out var svc))
                return (T)svc;
            foreach (var kv in _services)
                if (typeof(T).IsAssignableFrom(kv.Key))
                    return (T)kv.Value;
            return null;
        }

        public void AutoRegisterServicesInChildren(bool includeInactive = true)
        {
            var behaviours = GetComponentsInChildren<MonoBehaviour>(includeInactive);
            foreach (var mb in behaviours)
                if (mb is IWorldSystem svc)
                    RegisterService(svc);
        }

        public void ApplySetup(WorldSetupData setup)
        {
            if (setup == null)
                return;

            // Time
            var time = Get<TimeWorldSystem>();
            if (time != null)
                time.ApplyProfile(setup.Time);

            // Weather (asset)
            var weather = Get<WeatherWorldSystem>();
            if (weather != null && setup.Weather != null)
            {
                weather.ApplyProfile(setup.Weather);
                weather.UpdateSunForHour(time != null ? time.CurrentHour : setup.Time.Hour);
            }


            Get<LivingLetterWorldSystem>()?.ApplyProfile(setup.LivingLetters);

            Get<TrafficWorldSystem>()?.ApplyProfile(setup.Traffic);

            Get<AnimalWorldSystem>()?.ApplyProfile(setup.Animals);

        }

    }

}
