using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    public class DiscoArcadeFilterPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown subjectDropdown;
        [SerializeField] private TMP_InputField searchField;
        [SerializeField] private RectTransform countryListContainer;
        [SerializeField] private Toggle countryTogglePrefab;

        private readonly Dictionary<Countries, Toggle> _countryToggles = new();
        private readonly HashSet<Countries> _selectedCountries = new();
        private readonly List<Subject> _availableSubjects = new();

        private Subject? _selectedSubject;
        private string _searchText = string.Empty;
        private bool _suppressEvents;

        /// <summary>
        /// Snapshot of the current filter values.
        /// </summary>
        public readonly struct FilterState
        {
            public readonly Subject? SelectedSubject;
            public readonly string SearchText;
            public readonly IReadOnlyList<Countries> SelectedCountries;

            internal FilterState(Subject? subject, string search, IReadOnlyList<Countries> countries)
            {
                SelectedSubject = subject;
                SearchText = search ?? string.Empty;
                SelectedCountries = countries ?? Array.Empty<Countries>();
            }
        }

        public event Action<FilterState> FiltersChanged;

        private void Awake()
        {
            if (subjectDropdown != null)
            {
                subjectDropdown.onValueChanged.AddListener(HandleSubjectChanged);
            }

            if (searchField != null)
            {
                searchField.onValueChanged.AddListener(HandleSearchChanged);
            }
        }

        private void OnDestroy()
        {
            if (subjectDropdown != null)
            {
                subjectDropdown.onValueChanged.RemoveListener(HandleSubjectChanged);
            }

            if (searchField != null)
            {
                searchField.onValueChanged.RemoveListener(HandleSearchChanged);
            }

            foreach (var kvp in _countryToggles)
            {
                if (kvp.Value != null)
                {
                    kvp.Value.onValueChanged.RemoveAllListeners();
                }
            }
        }

        /// <summary>
        /// Populate dropdown and country list based on the available topics.
        /// </summary>
        public void Initialize(IEnumerable<TopicData> topics)
        {
            if (topics == null)
            {
                topics = Array.Empty<TopicData>();
            }

            _suppressEvents = true;
            BuildSubjectOptions(topics);
            BuildCountryToggles(topics);
            ResetFilterValues();
            _suppressEvents = false;
            RaiseFiltersChanged();
        }

        /// <summary>
        /// Returns the current filter state.
        /// </summary>
        public FilterState CurrentState => BuildState();

        /// <summary>
        /// Focus the search input for quick typing.
        /// </summary>
        public void FocusSearchField()
        {
            if (searchField != null)
            {
                searchField.ActivateInputField();
                searchField.Select();
            }
        }

        private void ResetFilterValues()
        {
            _selectedSubject = null;
            _searchText = string.Empty;

            if (subjectDropdown != null)
            {
                subjectDropdown.SetValueWithoutNotify(0);
            }

            if (searchField != null)
            {
                searchField.SetTextWithoutNotify(string.Empty);
            }

            _selectedCountries.Clear();
            foreach (var kvp in _countryToggles)
            {
                if (kvp.Value != null)
                {
                    kvp.Value.SetIsOnWithoutNotify(true);
                    _selectedCountries.Add(kvp.Key);
                }
            }
        }

        private void BuildSubjectOptions(IEnumerable<TopicData> topics)
        {
            _availableSubjects.Clear();

            var subjectSet = new HashSet<Subject>();
            foreach (var topic in topics)
            {
                if (topic == null)
                    continue;

                if (topic.Subjects != null)
                {
                    foreach (var subject in topic.Subjects)
                    {
                        if (IsDisplayableSubject(subject))
                        {
                            subjectSet.Add(subject);
                        }
                    }
                }

                var cards = topic.GetAllCards();
                foreach (var card in cards)
                {
                    if (card?.Subjects == null)
                        continue;

                    foreach (var subject in card.Subjects)
                    {
                        if (IsDisplayableSubject(subject))
                        {
                            subjectSet.Add(subject);
                        }
                    }
                }
            }

            _availableSubjects.AddRange(subjectSet.OrderBy(DisplayNameForSubject, StringComparer.OrdinalIgnoreCase));

            if (subjectDropdown != null)
            {
                var options = new List<TMP_Dropdown.OptionData>
                {
                    new TMP_Dropdown.OptionData("All subjects")
                };

                foreach (var subject in _availableSubjects)
                {
                    options.Add(new TMP_Dropdown.OptionData(DisplayNameForSubject(subject)));
                }

                subjectDropdown.options = options;
            }
        }

        private void BuildCountryToggles(IEnumerable<TopicData> topics)
        {
            var desiredCountries = new HashSet<Countries>();
            foreach (var topic in topics)
            {
                if (topic != null)
                {
                    desiredCountries.Add(topic.Country);
                }
            }

            if (desiredCountries.Count == 0)
            {
                foreach (Countries country in Enum.GetValues(typeof(Countries)))
                {
                    desiredCountries.Add(country);
                }
            }

            var order = desiredCountries.OrderBy(DisplayNameForCountry, StringComparer.OrdinalIgnoreCase).ToList();

            ClearCountryToggles();

            if (countryTogglePrefab == null || countryListContainer == null)
            {
                Debug.LogWarning("DiscoArcadeFilterPanel: missing country toggle prefab or container.");
                return;
            }

            foreach (var country in order)
            {
                var toggle = Instantiate(countryTogglePrefab, countryListContainer);
                toggle.gameObject.SetActive(true);
                toggle.SetIsOnWithoutNotify(true);
                SetToggleLabel(toggle, DisplayNameForCountry(country));

                toggle.onValueChanged.AddListener(isOn => HandleCountryToggleChanged(country, isOn));

                _countryToggles[country] = toggle;
                _selectedCountries.Add(country);
            }
        }

        private void ClearCountryToggles()
        {
            foreach (var toggle in _countryToggles.Values)
            {
                if (toggle != null)
                {
                    toggle.onValueChanged.RemoveAllListeners();
                    Destroy(toggle.gameObject);
                }
            }

            _countryToggles.Clear();
            _selectedCountries.Clear();
        }

        private void HandleSubjectChanged(int index)
        {
            if (_suppressEvents)
                return;

            if (index <= 0 || index > _availableSubjects.Count)
            {
                _selectedSubject = null;
            }
            else
            {
                _selectedSubject = _availableSubjects[index - 1];
            }

            RaiseFiltersChanged();
        }

        private void HandleSearchChanged(string value)
        {
            if (_suppressEvents)
                return;

            _searchText = value ?? string.Empty;
            RaiseFiltersChanged();
        }

        private void HandleCountryToggleChanged(Countries country, bool isOn)
        {
            if (_suppressEvents)
                return;

            if (isOn)
            {
                _selectedCountries.Add(country);
            }
            else
            {
                _selectedCountries.Remove(country);
            }

            RaiseFiltersChanged();
        }

        private void RaiseFiltersChanged()
        {
            if (_suppressEvents)
                return;

            FiltersChanged?.Invoke(BuildState());
        }

        private FilterState BuildState()
        {
            var countries = _selectedCountries.Count > 0
                ? _selectedCountries.ToList()
                : new List<Countries>();

            return new FilterState(_selectedSubject, _searchText, countries);
        }

        private static bool IsDisplayableSubject(Subject subject)
        {
            return subject > Subject.None;
        }

        private static string DisplayNameForSubject(Subject subject)
        {
            return SplitCamelCase(subject.ToString());
        }

        private static string DisplayNameForCountry(Countries country)
        {
            return SplitCamelCase(country.ToString());
        }

        private static string SplitCamelCase(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return Regex.Replace(value, "(\\B[A-Z])", " $1");
        }

        private static void SetToggleLabel(Toggle toggle, string label)
        {
            if (toggle == null)
                return;

            var tmp = toggle.GetComponentInChildren<TMP_Text>();
            if (tmp != null)
            {
                tmp.text = label;
                return;
            }

            var legacyText = toggle.GetComponentInChildren<Text>();
            if (legacyText != null)
            {
                legacyText.text = label;
            }
        }
    }
}
