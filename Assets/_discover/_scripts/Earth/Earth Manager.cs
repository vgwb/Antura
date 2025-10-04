using Antura.Core;
using Antura.Discover.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{

    public class EarthManager : MonoBehaviour
    {
        public static EarthManager I;

        private readonly HashSet<Countries> allowedCountries = new HashSet<Countries>();
        private bool isClassroomRestricted;
        private Coroutine waitForAppManagerRoutine;
        private Coroutine waitForHomeRoutine;
        private Countries pendingCountryToShow = Countries.France;
        private bool hasPendingCountry;

        public IReadOnlyCollection<Countries> AllowedCountries => allowedCountries;
        public Countries CurrentCountry { get; private set; } = Countries.France;
        public bool HasClassroomRestriction => isClassroomRestricted;

        private void Awake()
        {
            if (I == null)
            {
                I = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            TryRegisterProfileListener();
        }

        private void OnDisable()
        {
            if (DiscoverAppManager.I != null)
            {
                DiscoverAppManager.I.OnProfileLoaded -= HandleProfileLoaded;
            }

            if (waitForAppManagerRoutine != null)
            {
                StopCoroutine(waitForAppManagerRoutine);
                waitForAppManagerRoutine = null;
            }

            if (waitForHomeRoutine != null)
            {
                StopCoroutine(waitForHomeRoutine);
                waitForHomeRoutine = null;
            }
            hasPendingCountry = false;
        }

        private void Start()
        {
            if (allowedCountries.Count == 0)
            {
                RefreshAccessRules();
            }

            EnsureCurrentCountryIsValid();
            ApplySelection(CurrentCountry);
        }

        public bool SelectCountry(Countries selectedCountry)
        {
            if (!IsCountryAllowed(selectedCountry))
            {
                Debug.LogWarning($"EarthManager: blocked selection of {selectedCountry} due to classroom restrictions.");
                return false;
            }

            if (CurrentCountry != selectedCountry)
            {
                CurrentCountry = selectedCountry;
            }

            ApplySelection(selectedCountry);
            return true;
        }

        public bool IsCountryAllowed(Countries country)
        {
            if (allowedCountries.Count == 0)
            {
                return true;
            }

            return allowedCountries.Contains(country);
        }

        private void TryRegisterProfileListener()
        {
            var app = DiscoverAppManager.I;
            if (app != null)
            {
                app.OnProfileLoaded -= HandleProfileLoaded;
                app.OnProfileLoaded += HandleProfileLoaded;
                HandleProfileLoaded(app.CurrentProfile);
            }
            else if (waitForAppManagerRoutine == null && isActiveAndEnabled)
            {
                waitForAppManagerRoutine = StartCoroutine(CoWaitForAppManager());
            }
        }

        private IEnumerator CoWaitForAppManager()
        {
            while (DiscoverAppManager.I == null)
            {
                yield return null;
            }

            waitForAppManagerRoutine = null;
            DiscoverAppManager.I.OnProfileLoaded += HandleProfileLoaded;
            HandleProfileLoaded(DiscoverAppManager.I.CurrentProfile);
        }

        private void HandleProfileLoaded(DiscoverPlayerProfile profile)
        {
            RefreshAccessRules();
            EnsureCurrentCountryIsValid();
            ApplySelection(CurrentCountry);
        }

        private void ApplySelection(Countries selectedCountry)
        {
            Debug.Log($"Selecting country: {selectedCountry}");
            CountryButton.SetSelectedCountry(selectedCountry);
            RequestShowCountry(selectedCountry);
        }

        private void RequestShowCountry(Countries country)
        {
            pendingCountryToShow = country;
            hasPendingCountry = true;
            EnsureShowCountryRoutine();
        }

        private void EnsureShowCountryRoutine()
        {
            if (waitForHomeRoutine != null || !isActiveAndEnabled)
            {
                return;
            }

            waitForHomeRoutine = StartCoroutine(CoShowCountryWhenReady());
        }

        private IEnumerator CoShowCountryWhenReady()
        {
            while (UIDiscoverHome.I == null)
            {
                yield return null;
            }

            yield return null; // wait a frame so UI panels finish layout

            waitForHomeRoutine = null;
            if (hasPendingCountry)
            {
                Countries target = pendingCountryToShow;
                UIDiscoverHome.I.ShowCountry(target, true);
                if (pendingCountryToShow == target)
                {
                    hasPendingCountry = false;
                }
            }

            if (hasPendingCountry)
            {
                EnsureShowCountryRoutine();
            }
        }

        private void RefreshAccessRules()
        {
            allowedCountries.Clear();
            isClassroomRestricted = false;

            DiscoverPlayerProfile profile = DiscoverAppManager.I != null ? DiscoverAppManager.I.CurrentProfile : null;
            bool inClassroom = profile?.profile?.classroom > 0;
            LearningContentID contentId = AppManager.I != null ? AppManager.I.ContentEdition.ContentID : LearningContentID.None;
            Countries? contentCountry = MapContentToCountry(contentId);

            if (inClassroom && contentCountry.HasValue)
            {
                isClassroomRestricted = true;
                allowedCountries.Add(contentCountry.Value);
                allowedCountries.Add(Countries.International);
            }
            else
            {
                foreach (Countries country in Enum.GetValues(typeof(Countries)))
                {
                    allowedCountries.Add(country);
                }
            }

            if (allowedCountries.Count == 0)
            {
                allowedCountries.Add(Countries.France);
            }
        }

        private void EnsureCurrentCountryIsValid()
        {
            if (allowedCountries.Count == 0)
            {
                CurrentCountry = Countries.France;
                return;
            }

            if (IsCountryAllowed(CurrentCountry))
            {
                return;
            }

            Countries preferred = DetermineDefaultCountry();
            if (IsCountryAllowed(preferred))
            {
                CurrentCountry = preferred;
                return;
            }

            foreach (Countries country in allowedCountries)
            {
                CurrentCountry = country;
                break;
            }
        }

        private Countries DetermineDefaultCountry()
        {
            LearningContentID contentId = AppManager.I != null ? AppManager.I.ContentEdition.ContentID : LearningContentID.None;
            Countries? mapped = MapContentToCountry(contentId);
            if (mapped.HasValue)
            {
                return mapped.Value;
            }

            return Countries.France;
        }

        private Countries? MapContentToCountry(LearningContentID contentId)
        {
            switch (contentId)
            {
                case LearningContentID.Discover_Poland:
                    return Countries.Poland;
                case LearningContentID.Discover_France:
                    return Countries.France;
                default:
                    return null;
            }
        }
    }
}
