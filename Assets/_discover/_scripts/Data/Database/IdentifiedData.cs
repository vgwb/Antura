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

        /// Build a sanitized ID from a base name with an optional code prefix.
        public static string BuildSanitizedId(string baseName, string prefixCode = null)
        {
            return SanitizeId(baseName);
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

#if UNITY_EDITOR
        // Editor-only setters used by tools
        public void Editor_SetId(string newId, bool lockAfter = false)
        {
            var sanitized = SanitizeId(newId);
            // Ensure uniqueness among assets of the same type by appending _2, _3, ...
            sanitized = EnsureUniqueIdForType(GetType(), sanitized, this);
            Id = sanitized;
        }

        private static string EnsureUniqueIdForType(System.Type type, string baseId, UnityEngine.Object self)
        {
            if (string.IsNullOrEmpty(baseId))
                return baseId;
            try
            {
                var ids = UnityEditor.AssetDatabase.FindAssets($"t:{type.Name}");
                var existing = new System.Collections.Generic.HashSet<string>();
                string selfPath = UnityEditor.AssetDatabase.GetAssetPath(self);
                foreach (var guid in ids)
                {
                    var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                    var obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                    if (obj == null || obj == self)
                        continue;
                    if (obj is IdentifiedData data && !string.IsNullOrEmpty(data.Id))
                    {
                        existing.Add(data.Id);
                    }
                }

                if (!existing.Contains(baseId))
                    return baseId;

                int i = 2;
                while (true)
                {
                    var candidate = SanitizeId($"{baseId}_{i}");
                    if (!existing.Contains(candidate))
                        return candidate;
                    i++;
                    if (i > 9999)
                        return SanitizeId(baseId + "_x");
                }
            }
            catch
            {
                return baseId;
            }
        }
#endif
    }
}
