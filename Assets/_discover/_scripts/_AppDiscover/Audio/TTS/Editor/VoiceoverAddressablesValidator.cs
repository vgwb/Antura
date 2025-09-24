#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Antura.Discover.Audio.Editor
{
    public static class VoiceoverAddressablesValidator
    {
        private const string CardsAudioTableName = "Cards audio";
        // Accept multiple valid group naming patterns used by Unity Localization and legacy variants
        private static readonly string[] GroupNamePrefixes = new[]
        {
            "Localization-Assets-",          // Unity default
            "Localization-Assets-Tables-",   // Legacy project variant
            "Localization-Asset-Tables-"     // Legacy project variant
        };

        [MenuItem("Antura/Audio/Voiceover Addressables validator")]
        public static void Validate()
        {
            var output = new StringBuilder();
            int totalChecked = 0;
            int totalErrors = 0;

            var settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                EditorUtility.DisplayDialog("Voiceover Validator", "Addressables Settings not found. Enable Addressables first.", "OK");
                return;
            }

            var locales = LocalizationSettings.AvailableLocales?.Locales ?? new List<Locale>();
            if (locales.Count == 0)
            {
                EditorUtility.DisplayDialog("Voiceover Validator", "No locales configured.", "OK");
                return;
            }

            // Validate Quests
            var questGuids = AssetDatabase.FindAssets("t:QuestData");
            output.AppendLine("== Quests ==");
            foreach (var qguid in questGuids)
            {
                var qpath = AssetDatabase.GUIDToAssetPath(qguid);
                var quest = AssetDatabase.LoadAssetAtPath<QuestData>(qpath);
                if (quest == null)
                    continue;
                string questId = quest.Id ?? quest.name;
                foreach (var locale in locales)
                {
                    if (quest.QuestAssetsTable == null || quest.QuestAssetsTable.TableReference.Equals(default(TableReference)))
                    {
                        output.AppendLine($"[Skip] {questId} {locale.Identifier.Code} has no QuestAssetsTable reference.");
                        continue;
                    }
                    AssetTable at = null;
                    try
                    {
                        at = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, locale);
                    }
                    catch (Exception ex)
                    {
                        output.AppendLine($"[Error] {questId} {locale.Identifier.Code} cannot load AssetTable: {ex.Message}");
                        continue;
                    }
                    if (at == null)
                        continue;
                    var validGroupNames = GetValidGroupNames(locale);
                    foreach (var entry in at.Values)
                    {
                        if (entry == null || string.IsNullOrEmpty(entry.Guid))
                            continue;
                        var aep = settings.FindAssetEntry(entry.Guid);
                        totalChecked++;
                        if (aep == null)
                        {
                            totalErrors++;
                            output.AppendLine($"[Missing Entry] {questId} {locale.Identifier.Code} key={entry.SharedEntry?.Key}");
                            continue;
                        }
                        // Group check
                        if (aep.parentGroup == null || !validGroupNames.Contains(aep.parentGroup.Name))
                        {
                            totalErrors++;
                            output.AppendLine($"[Wrong Group] {questId} {locale.Identifier.Code} key={entry.SharedEntry?.Key} group='{aep.parentGroup?.Name}' expected one of: {string.Join(", ", validGroupNames)}");
                        }
                        // Address check
                        var expected = $"VO/{locale.Identifier.Code}/quest/{questId}/{NormalizeQuestKey(entry.SharedEntry?.Key)}";
                        if (!string.Equals(aep.address, expected, StringComparison.Ordinal))
                        {
                            totalErrors++;
                            output.AppendLine($"[Wrong Address] {questId} {locale.Identifier.Code} key={entry.SharedEntry?.Key} address='{aep.address}' expected='{expected}'");
                        }
                        // Labels check
                        var labels = aep.labels;
                        string langLabel = $"lang:{locale.Identifier.Code}";
                        string questLabel = $"quest:{questId}";
                        if (!labels.Contains("type:vo"))
                        { totalErrors++; output.AppendLine($"[Missing Label] type:vo for {questId} {locale.Identifier.Code} key={entry.SharedEntry?.Key}"); }
                        if (!labels.Contains(langLabel))
                        { totalErrors++; output.AppendLine($"[Missing Label] {langLabel} for {questId} {locale.Identifier.Code} key={entry.SharedEntry?.Key}"); }
                        if (!labels.Contains(questLabel))
                        { totalErrors++; output.AppendLine($"[Missing Label] {questLabel} for {questId} {locale.Identifier.Code} key={entry.SharedEntry?.Key}"); }
                    }
                }
            }

            // Validate Cards
            output.AppendLine();
            output.AppendLine("== Cards ==");
            foreach (var locale in locales)
            {
                var at = LocalizationSettings.AssetDatabase.GetTable(CardsAudioTableName, locale);
                if (at == null)
                    continue;
                var validGroupNames = GetValidGroupNames(locale);
                foreach (var entry in at.Values)
                {
                    if (entry == null || string.IsNullOrEmpty(entry.Guid))
                        continue;
                    var aep = settings.FindAssetEntry(entry.Guid);
                    totalChecked++;
                    if (aep == null)
                    {
                        totalErrors++;
                        output.AppendLine($"[Missing Entry] Cards {locale.Identifier.Code} key={entry.SharedEntry?.Key}");
                        continue;
                    }
                    if (aep.parentGroup == null || !validGroupNames.Contains(aep.parentGroup.Name))
                    {
                        totalErrors++;
                        output.AppendLine($"[Wrong Group] Cards {locale.Identifier.Code} key={entry.SharedEntry?.Key} group='{aep.parentGroup?.Name}' expected one of: {string.Join(", ", validGroupNames)}");
                    }
                    var expected = $"VO/{locale.Identifier.Code}/cards/{NormalizeCardId(entry.SharedEntry?.Key)}";
                    if (!string.Equals(aep.address, expected, StringComparison.Ordinal))
                    {
                        totalErrors++;
                        output.AppendLine($"[Wrong Address] Cards {locale.Identifier.Code} key={entry.SharedEntry?.Key} address='{aep.address}' expected='{expected}'");
                    }
                    var labels = aep.labels;
                    string langLabel = $"lang:{locale.Identifier.Code}";
                    if (!labels.Contains("type:vo"))
                    { totalErrors++; output.AppendLine($"[Missing Label] type:vo for Cards {locale.Identifier.Code} key={entry.SharedEntry?.Key}"); }
                    if (!labels.Contains(langLabel))
                    { totalErrors++; output.AppendLine($"[Missing Label] {langLabel} for Cards {locale.Identifier.Code} key={entry.SharedEntry?.Key}"); }
                }
            }

            // Report
            if (totalErrors == 0)
            {
                Debug.Log($"[VO Validator] PASS. Checked {totalChecked} entries.");
                EditorUtility.DisplayDialog("Voiceover Validator", $"PASS. Checked {totalChecked} entries.", "OK");
            }
            else
            {
                Debug.LogWarning($"[VO Validator] Found {totalErrors} issues across {totalChecked} entries.\n" + output.ToString());
                EditorUtility.DisplayDialog("Voiceover Validator", $"Found {totalErrors} issues across {totalChecked} entries. See Console for details.", "OK");
            }
        }

        public static void ValidateQuest(QuestData quest, IEnumerable<Locale> locales)
        {
            var settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                EditorUtility.DisplayDialog("Voiceover Validator", "Addressables Settings not found. Enable Addressables first.", "OK");
                return;
            }
            if (quest == null)
            {
                EditorUtility.DisplayDialog("Voiceover Validator", "No quest selected.", "OK");
                return;
            }
            var locs = locales?.ToList() ?? new List<Locale>();
            if (locs.Count == 0)
            {
                EditorUtility.DisplayDialog("Voiceover Validator", "No locales configured.", "OK");
                return;
            }

            int totalChecked = 0;
            int totalErrors = 0;
            var output = new StringBuilder();
            string questId = quest.Id ?? quest.name;

            foreach (var locale in locs)
            {
                if (quest.QuestAssetsTable == null || quest.QuestAssetsTable.TableReference.Equals(default(TableReference)))
                {
                    output.AppendLine($"[Skip] {questId} {locale.Identifier.Code} has no QuestAssetsTable reference.");
                    continue;
                }
                AssetTable at = null;
                try
                { at = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, locale); }
                catch (Exception ex) { output.AppendLine($"[Error] {questId} {locale.Identifier.Code} cannot load AssetTable: {ex.Message}"); continue; }
                if (at == null)
                    continue;

                var validGroupNames = GetValidGroupNames(locale);
                foreach (var entry in at.Values)
                {
                    if (entry == null || string.IsNullOrEmpty(entry.Guid))
                        continue;
                    var aep = settings.FindAssetEntry(entry.Guid);
                    totalChecked++;
                    if (aep == null)
                    { totalErrors++; output.AppendLine($"[Missing Entry] {questId} {locale.Identifier.Code} key={entry.SharedEntry?.Key}"); continue; }
                    if (aep.parentGroup == null || !validGroupNames.Contains(aep.parentGroup.Name))
                    { totalErrors++; output.AppendLine($"[Wrong Group] {questId} {locale.Identifier.Code} key={entry.SharedEntry?.Key} group='{aep.parentGroup?.Name}' expected one of: {string.Join(", ", validGroupNames)}"); }
                    var expected = $"VO/{locale.Identifier.Code}/quest/{questId}/{NormalizeQuestKey(entry.SharedEntry?.Key)}";
                    if (!string.Equals(aep.address, expected, StringComparison.Ordinal))
                    { totalErrors++; output.AppendLine($"[Wrong Address] {questId} {locale.Identifier.Code} key={entry.SharedEntry?.Key} address='{aep.address}' expected='{expected}'"); }
                    var labels = aep.labels;
                    string langLabel = $"lang:{locale.Identifier.Code}";
                    string questLabel = $"quest:{questId}";
                    if (!labels.Contains("type:vo"))
                    { totalErrors++; output.AppendLine($"[Missing Label] type:vo for {questId} {locale.Identifier.Code} key={entry.SharedEntry?.Key}"); }
                    if (!labels.Contains(langLabel))
                    { totalErrors++; output.AppendLine($"[Missing Label] {langLabel} for {questId} {locale.Identifier.Code} key={entry.SharedEntry?.Key}"); }
                    if (!labels.Contains(questLabel))
                    { totalErrors++; output.AppendLine($"[Missing Label] {questLabel} for {questId} {locale.Identifier.Code} key={entry.SharedEntry?.Key}"); }
                }
            }

            if (totalErrors == 0)
            { Debug.Log($"[VO Validator] PASS {questId}. Checked {totalChecked} entries."); EditorUtility.DisplayDialog("Voiceover Validator", $"PASS for '{questId}'. Checked {totalChecked} entries.", "OK"); }
            else
            { Debug.LogWarning($"[VO Validator] {questId}: {totalErrors} issues across {totalChecked} entries.\n" + output.ToString()); EditorUtility.DisplayDialog("Voiceover Validator", $"{questId}: {totalErrors} issues. See Console.", "OK"); }
        }

        public static void ValidateCards(IEnumerable<Locale> locales)
        {
            var settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            { EditorUtility.DisplayDialog("Voiceover Validator", "Addressables Settings not found. Enable Addressables first.", "OK"); return; }
            var locs = locales?.ToList() ?? new List<Locale>();
            if (locs.Count == 0)
            { EditorUtility.DisplayDialog("Voiceover Validator", "No locales configured.", "OK"); return; }

            int totalChecked = 0;
            int totalErrors = 0;
            var output = new StringBuilder();
            foreach (var locale in locs)
            {
                AssetTable at = null;
                try
                { at = LocalizationSettings.AssetDatabase.GetTable(CardsAudioTableName, locale); }
                catch (Exception ex) { output.AppendLine($"[Error] Cards {locale.Identifier.Code} cannot load AssetTable: {ex.Message}"); continue; }
                if (at == null)
                    continue;
                var validGroupNames = GetValidGroupNames(locale);
                foreach (var entry in at.Values)
                {
                    if (entry == null || string.IsNullOrEmpty(entry.Guid))
                        continue;
                    var aep = settings.FindAssetEntry(entry.Guid);
                    totalChecked++;
                    if (aep == null)
                    { totalErrors++; output.AppendLine($"[Missing Entry] Cards {locale.Identifier.Code} key={entry.SharedEntry?.Key}"); continue; }
                    if (aep.parentGroup == null || !validGroupNames.Contains(aep.parentGroup.Name))
                    { totalErrors++; output.AppendLine($"[Wrong Group] Cards {locale.Identifier.Code} key={entry.SharedEntry?.Key} group='{aep.parentGroup?.Name}' expected one of: {string.Join(", ", validGroupNames)}"); }
                    var expected = $"VO/{locale.Identifier.Code}/cards/{NormalizeCardId(entry.SharedEntry?.Key)}";
                    if (!string.Equals(aep.address, expected, StringComparison.Ordinal))
                    { totalErrors++; output.AppendLine($"[Wrong Address] Cards {locale.Identifier.Code} key={entry.SharedEntry?.Key} address='{aep.address}' expected='{expected}'"); }
                    var labels = aep.labels;
                    string langLabel = $"lang:{locale.Identifier.Code}";
                    if (!labels.Contains("type:vo"))
                    { totalErrors++; output.AppendLine($"[Missing Label] type:vo for Cards {locale.Identifier.Code} key={entry.SharedEntry?.Key}"); }
                    if (!labels.Contains(langLabel))
                    { totalErrors++; output.AppendLine($"[Missing Label] {langLabel} for Cards {locale.Identifier.Code} key={entry.SharedEntry?.Key}"); }
                }
            }
            if (totalErrors == 0)
            { Debug.Log($"[VO Validator] PASS Cards. Checked {totalChecked} entries."); EditorUtility.DisplayDialog("Voiceover Validator", $"PASS (Cards). Checked {totalChecked} entries.", "OK"); }
            else
            { Debug.LogWarning($"[VO Validator] Cards: {totalErrors} issues across {totalChecked} entries.\n" + output.ToString()); EditorUtility.DisplayDialog("Voiceover Validator", $"Cards: {totalErrors} issues. See Console.", "OK"); }
        }

        private static IEnumerable<string> GetValidGroupNames(Locale locale)
        {
            string code = locale.Identifier.Code;
            string englishName = locale.Identifier.CultureInfo != null ? locale.Identifier.CultureInfo.EnglishName : (locale.name ?? code);
            foreach (var prefix in GroupNamePrefixes)
                yield return $"{prefix}{englishName} ({code})";
        }

        private static string NormalizeQuestKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "item";
            if (key.StartsWith("line:", StringComparison.OrdinalIgnoreCase))
                return SafeSegment("line-" + key.Substring("line:".Length));
            if (key.StartsWith("line-", StringComparison.OrdinalIgnoreCase))
                return SafeSegment(key);
            return SafeSegment(key);
        }

        private static string NormalizeCardId(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "item";
            int dot = key.IndexOf('.');
            var id = dot > 0 ? key.Substring(0, dot) : key;
            return SafeSegment(id);
        }

        private static string SafeSegment(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "item";
            var sb = new StringBuilder(s.Length);
            foreach (var ch in s)
            {
                if (char.IsLetterOrDigit(ch) || ch == '-' || ch == '_')
                    sb.Append(ch);
                else if (char.IsWhiteSpace(ch) || ch == ':' || ch == '/' || ch == '\\' || ch == '.')
                    sb.Append('-');
                else
                    sb.Append(ch);
            }
            var seg = System.Text.RegularExpressions.Regex.Replace(sb.ToString(), "-+", "-").Trim('-');
            return string.IsNullOrEmpty(seg) ? "item" : seg;
        }
    }
}
#endif
