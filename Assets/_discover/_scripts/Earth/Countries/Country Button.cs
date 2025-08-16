using System.Collections.Generic;
using UnityEngine;
using Antura.Core;
using TMPro;

namespace Antura.Discover
{
    public class CountryButton : MonoBehaviour
    {
        public CountryData country;

        public TMP_Text countryLabel;

        private SpriteRenderer _sprite;
        private static readonly List<CountryButton> _all = new List<CountryButton>();
        private const float OtherAlpha = 0.25f;

        void OnEnable()
        {
            if (!_all.Contains(this))
                _all.Add(this);
            if (_sprite == null)
                _sprite = GetComponentInChildren<SpriteRenderer>();
        }

        void OnDisable()
        {
            _all.Remove(this);
        }

        void Start()
        {
            countryLabel.text = country.CountryName.GetLocalizedString();
            if (_sprite == null)
                _sprite = GetComponentInChildren<SpriteRenderer>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        OnCountryClicked();
                    }
                }
            }
        }
        private void OnCountryClicked()
        {
            Debug.Log($"Country {country.CountryId} clicked.");
            EarthManager.I.SelectCountry(country.CountryId);
        }

        private void SetAlpha(float a)
        {
            if (_sprite != null)
            {
                var c = _sprite.color;
                c.a = a;
                _sprite.color = c;
            }
        }

        public static void SetSelectedCountry(Countries selected)
        {
            foreach (var cb in _all)
            {
                if (cb == null)
                    continue;
                float a = (cb.country != null && cb.country.CountryId == selected) ? 1f : OtherAlpha;
                cb.SetAlpha(a);
            }
        }
    }
}
