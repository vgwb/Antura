using Antura.Core;
using Antura.UI;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace Antura.Discover
{
    public class CountryButton : MonoBehaviour
    {
        public CountryData country;
        public TextRender countryLabel;
        private SpriteRenderer _sprite;
        private static readonly List<CountryButton> _all = new List<CountryButton>();
        private const float OtherAlpha = 0.25f;
        private const float DisabledAlpha = 0.08f;

        void OnEnable()
        {
            if (!_all.Contains(this))
                _all.Add(this);
            if (_sprite == null)
                _sprite = GetComponentInChildren<SpriteRenderer>();

            if (EarthManager.I != null)
            {
                SetSelectedCountry(EarthManager.I.CurrentCountry);
            }
        }

        void OnDisable()
        {
            _all.Remove(this);
        }

        void Start()
        {
            countryLabel.text = LocalizationSystem.I.GetLocalizedString(country.CountryName, true);
            if (_sprite == null)
                _sprite = GetComponentInChildren<SpriteRenderer>();
        }

        void Update()
        {
            // Detect press start (mouse or first touch)
            Vector3 screenPos;
            bool pressed = false;
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                pressed = true;
                screenPos = Input.touches[0].position;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                pressed = true;
                screenPos = Input.mousePosition;
            }
            else
            {
                return;
            }

            // If press began over UI, ignore
            if (IsPointerOverUI())
                return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    OnCountryClicked();
                }
            }
        }

        private bool IsPointerOverUI()
        {
            if (EventSystem.current == null)
                return false;
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (EventSystem.current.IsPointerOverGameObject(Input.touches[i].fingerId))
                        return true;
                }
                return false;
            }
            return EventSystem.current.IsPointerOverGameObject();
        }
        private void OnCountryClicked()
        {
            //  Debug.Log($"Country {country.CountryId} clicked.");
            if (EarthManager.I != null)
            {
                if (!EarthManager.I.IsCountryAllowed(country.CountryId))
                {
                    Debug.Log($"CountryButton: selection of {country.CountryId} blocked due to classroom restrictions.", this);
                    return;
                }

                EarthManager.I.SelectCountry(country.CountryId);
            }
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
                bool allowed = EarthManager.I == null || cb.country == null || EarthManager.I.IsCountryAllowed(cb.country.CountryId);
                float a;
                if (!allowed)
                {
                    a = DisabledAlpha;
                }
                else
                {
                    a = (cb.country != null && cb.country.CountryId == selected) ? 1f : OtherAlpha;
                }
                cb.SetAlpha(a);
            }
        }
    }
}
