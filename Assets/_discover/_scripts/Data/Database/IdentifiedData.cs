using UnityEngine;
using System.Text.RegularExpressions;
using System;

namespace Antura.Discover
{
    public abstract class IdentifiedData : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Unique, stable ID. Lowercase snake_case. Never change after shipping.")]
        public string Id;

        public static string SanitizeId(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return string.Empty;
            string s = raw.Trim().ToLowerInvariant();
            s = s.Replace(' ', '_').Replace('-', '_');
            s = Regex.Replace(s, @"[^a-z0-9_]+", "_");
            s = Regex.Replace(s, "_{2,}", "_").Trim('_');
            return s;
        }

        /// Build a sanitized ID from a base name with an optional country two-letter code prefix.
        public static string BuildSanitizedId(string baseName, string countryCode = null)
        {
            var withPrefix = string.IsNullOrEmpty(countryCode)
                ? baseName
                : PrefixOnce(countryCode, baseName);
            return SanitizeId(withPrefix);
        }

        /// Adds prefix_ only if not already present (case-insensitive)
        public static string PrefixOnce(string prefix, string baseName)
        {
            if (string.IsNullOrEmpty(prefix))
                return baseName ?? string.Empty;
            var expected = prefix.ToLowerInvariant() + "_";
            if (!string.IsNullOrEmpty(baseName) && baseName.Length >= expected.Length && baseName.Substring(0, expected.Length).ToLowerInvariant() == expected)
                return baseName;
            return expected + (baseName ?? string.Empty);
        }

        /// Best-effort mapping from a Countries enum name to a two-letter lowercase code.
        public static string CountryNameToCode(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            switch (name)
            {
                case "Global":
                    return null; // No prefix for global items
                case "Italy":
                    return "it";
                case "France":
                    return "fr";
                case "Poland":
                    return "pl";
                case "Spain":
                    return "es";
                case "Germany":
                    return "de";
                case "Portugal":
                    return "pt";
                case "Greece":
                    return "gr";
                case "UnitedKingdom":
                    return "uk";

                default:
                    return name.Length >= 2 ? name.Substring(0, 2).ToLowerInvariant() : name.ToLowerInvariant();
            }
        }

#if UNITY_EDITOR
        // Editor-only setters used by tools
        public void Editor_SetId(string newId, bool lockAfter = false)
        {
            Id = SanitizeId(newId);
        }

#endif
    }
}
