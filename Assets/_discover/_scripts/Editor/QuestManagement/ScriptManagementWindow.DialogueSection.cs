#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Antura.Discover.Audio.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Antura.Discover.EditorTools
{
    public partial class ScriptManagementWindow
    {
        private sealed class DialogueSection
        {
            private readonly ScriptManagementWindow owner;

            public DialogueSection(ScriptManagementWindow owner)
            {
                this.owner = owner;
            }

            public void Draw(QuestData quest)
            {
                DrawSummaryAndBatchActions(quest);
                EditorGUILayout.LabelField("Legend: !!! = needs regeneration (string changed since audio was generated); 'missing' = no audio", EditorStyles.miniLabel);
                DrawManifestToolbar();
                DrawLinesTable(quest);
                EditorGUILayout.Space(8);
                DrawOrphansSection(quest);
            }

            private void DrawSummaryAndBatchActions(QuestData quest)
            {
                var meta = YarnLineMapBuilder.BuildMeta(quest);
                var allIds = GetLineIdsInScriptOrder(quest, meta);
                if (!string.IsNullOrWhiteSpace(owner._filter))
                {
                    var f = owner._filter.Trim();
                    allIds = allIds.Where(id =>
                    {
                        if (id.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0)
                            return true;
                        if (meta.Titles.TryGetValue(id, out var title))
                            return title != null && title.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0;
                        return false;
                    }).ToList();
                }

                var locales = owner.GetTargetLocales().ToList();
                int totalLines = allIds.Count;
                int missingStrings = 0;
                int missingAudio = 0;
                int changedAudio = 0;
                foreach (var id in allIds)
                {
                    var key = "line:" + id;
                    foreach (var loc in locales)
                    {
                        if (!owner.HasNonEmptyString(quest, loc, key))
                            missingStrings++;
                        var ai = owner.GetAudioInfo(quest, loc, key);
                        if (!(ai.Clip != null && !ai.IsZeroLength))
                            missingAudio++;
                        else if (owner.IsLineStale(quest, loc, key))
                            changedAudio++;
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label($"Lines: {totalLines}", GUILayout.Width(100));
                    GUILayout.Label($"Missing strings: {missingStrings}", GUILayout.Width(160));
                    GUILayout.Label($"Missing audio: {missingAudio}", GUILayout.Width(150));
                    GUILayout.Label($"Changed audio: {changedAudio}", GUILayout.Width(160));

                    using (new EditorGUI.DisabledScope(missingAudio == 0 || locales.Count == 0))
                    {
                        if (GUILayout.Button("Regenerate all missing (selected)", GUILayout.Width(240)))
                        {
                            owner.RegenerateAllMissingForSelectedLocales(quest, locales);
                        }
                    }
                    using (new EditorGUI.DisabledScope(changedAudio == 0 || locales.Count == 0))
                    {
                        if (GUILayout.Button("Regenerate all changed (selected)", GUILayout.Width(250)))
                        {
                            owner.RegenerateAllChangedForSelectedLocales(quest, locales, allIds);
                        }
                    }
                    using (new EditorGUI.DisabledScope(totalLines == 0 || locales.Count == 0))
                    {
                        if (GUILayout.Button("Regenerate all (overwrite)", GUILayout.Width(220)))
                        {
                            owner.RegenerateAllOverwriteForSelectedLocales(quest, locales);
                        }
                    }
                }
            }

            private void DrawManifestToolbar()
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Reload manifest (re-read _index.json)", GUILayout.Width(260)))
                    {
                        owner._manifestCache.Clear();
                        AssetDatabase.Refresh();
                        owner.Repaint();
                    }
                }
            }

            private void DrawLinesTable(QuestData quest)
            {
                var meta = YarnLineMapBuilder.BuildMeta(quest);
                var rows = GetRowsInScriptOrder(quest, meta);
                if (!string.IsNullOrWhiteSpace(owner._filter))
                {
                    var f = owner._filter.Trim();
                    rows = rows.Where(r =>
                    {
                        if (r.ShortId.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0)
                            return true;
                        if (meta.Titles.TryGetValue(r.ShortId, out var title))
                            return title != null && title.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0;
                        return false;
                    }).ToList();
                }

                var targetLocales = owner.GetTargetLocales().ToList();
                if (targetLocales.Count == 0)
                {
                    EditorGUILayout.HelpBox("No locales configured in Project Settings/Localization.", MessageType.Info);
                    return;
                }

                var enLocale = owner.GetEnglishLocale();
                var displayLocale = enLocale ?? targetLocales.FirstOrDefault();
                var secondTextLocale = targetLocales.FirstOrDefault(l => displayLocale != null && !string.Equals(l.Identifier.Code, displayLocale.Identifier.Code, StringComparison.OrdinalIgnoreCase));

                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColNodeTitleW)))
                    { GUILayout.Label("Node title", EditorStyles.boldLabel, GUILayout.Width(ColNodeTitleW)); }
                    using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColLineIdW)))
                    { GUILayout.Label("Line Id", EditorStyles.boldLabel, GUILayout.Width(ColLineIdW)); }
                    using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColTextW)))
                    { GUILayout.Label($"Text ({GetLocaleDisplayName(displayLocale)})", EditorStyles.boldLabel, GUILayout.Width(ColTextW)); }
                    if (secondTextLocale != null)
                    {
                        using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColTextW)))
                        { GUILayout.Label($"Text ({GetLocaleDisplayName(secondTextLocale)})", EditorStyles.boldLabel, GUILayout.Width(ColTextW)); }
                    }
                    if (targetLocales.Count > 1)
                    {
                        using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColStringsW)))
                        { GUILayout.Label("Strings (per locale)", EditorStyles.boldLabel, GUILayout.Width(ColStringsW)); }
                        using (new EditorGUILayout.VerticalScope("box"))
                        { GUILayout.Label("Audio (per locale)", EditorStyles.boldLabel); }
                    }
                    else
                    {
                        using (new EditorGUILayout.VerticalScope("box"))
                        { GUILayout.Label("Audio", EditorStyles.boldLabel); }
                    }
                }

                foreach (var row in rows)
                {
                    var shortId = row.ShortId;
                    var key = (row.IsShadow ? "shadow:" : "line:") + shortId;
                    var displayId = row.IsShadow ? ($"shadow:{shortId}") : shortId;
                    var nodeTitle = meta.Titles.TryGetValue(shortId, out var t) ? t : string.Empty;

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColNodeTitleW)))
                        { GUILayout.Label(nodeTitle, EditorStyles.wordWrappedLabel, GUILayout.Width(ColNodeTitleW)); }
                        using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColLineIdW)))
                        { EditorGUILayout.SelectableLabel(displayId, GUILayout.Height(18)); }

                        if (row.IsShadow)
                        {
                            var oldCol = GUI.color;
                            GUI.color = Color.gray;
                            using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColTextW)))
                            { GUILayout.Label(string.Empty, EditorStyles.wordWrappedLabel, GUILayout.Width(ColTextW)); }
                            if (secondTextLocale != null)
                            {
                                using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColTextW)))
                                { GUILayout.Label(string.Empty, EditorStyles.wordWrappedLabel, GUILayout.Width(ColTextW)); }
                            }
                            GUI.color = oldCol;

                            if (targetLocales.Count > 1)
                            {
                                using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColStringsW)))
                                { }
                                using (new EditorGUILayout.VerticalScope("box"))
                                { }
                            }
                            else
                            {
                                using (new EditorGUILayout.VerticalScope("box"))
                                { }
                            }
                            continue;
                        }

                        string entryTextEn = displayLocale != null ? (GetStringValue(quest, displayLocale, key) ?? string.Empty) : string.Empty;
                        using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColTextW)))
                        {
                            GUILayout.Label(entryTextEn, EditorStyles.wordWrappedLabel, GUILayout.Width(ColTextW));
                            if (displayLocale != null && owner.IsLineStale(quest, displayLocale, key))
                            { GUILayout.Label("!!!", GUILayout.Width(26)); }
                        }
                        if (secondTextLocale != null)
                        {
                            string entryText2 = GetStringValue(quest, secondTextLocale, key) ?? string.Empty;
                            using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColTextW)))
                            {
                                GUILayout.Label(entryText2, EditorStyles.wordWrappedLabel, GUILayout.Width(ColTextW));
                                if (owner.IsLineStale(quest, secondTextLocale, key))
                                { GUILayout.Label("!!!", GUILayout.Width(26)); }
                            }
                        }

                        if (targetLocales.Count > 1)
                        {
                            using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(ColStringsW)))
                            {
                                using (new EditorGUILayout.HorizontalScope())
                                {
                                    foreach (var loc in targetLocales)
                                    {
                                        bool has = HasString(quest, loc, key);
                                        using (new EditorGUI.DisabledScope(true))
                                        { GUILayout.Toggle(has, loc.Identifier.Code, GUILayout.Width(50)); }
                                    }
                                }
                            }
                            using (new EditorGUILayout.VerticalScope("box"))
                            { DrawAllLocalesRowAudioOnly(quest, key, targetLocales); }
                        }
                        else
                        {
                            using (new EditorGUILayout.VerticalScope("box"))
                            { DrawSingleLocaleRow(quest, key, targetLocales[0]); }
                        }
                    }
                }
            }

            private void DrawAllLocalesRowAudioOnly(QuestData quest, string lineKey, List<Locale> locales)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("R All", GUILayout.Width(60)))
                    {
                        owner.RegenerateLineAudioForAllLocales(quest, lineKey);
                    }
                    GUILayout.Space(6);
                    foreach (var loc in locales)
                    {
                        var info = owner.GetAudioInfo(quest, loc, lineKey);
                        bool stale = owner.IsLineStale(quest, loc, lineKey);
                        if (info.Clip != null && !info.IsZeroLength)
                        {
                            if (stale)
                            { GUILayout.Label("!!!", GUILayout.Width(26)); }
                            if (GUILayout.Button("▶", GUILayout.Width(24)))
                            { owner.PlayClip(info.Clip); }
                            if (GUILayout.Button("R", GUILayout.Width(22)))
                            { owner.RegenerateLineAudioViaVoManager(quest, loc, lineKey); }
                            if (GUILayout.Button("Ping", GUILayout.Width(40)))
                            { PingObject(info.Obj); }
                        }
                        else
                        {
                            if (GUILayout.Button("Generate", GUILayout.Width(80)))
                            { owner.RegenerateLineAudioViaVoManager(quest, loc, lineKey); }
                        }
                    }
                }
            }

            private void DrawSingleLocaleRow(QuestData quest, string lineKey, Locale locale)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("R All", GUILayout.Width(60)))
                    {
                        owner.RegenerateLineAudioForAllLocales(quest, lineKey);
                    }
                    GUILayout.Space(6);
                    var info = owner.GetAudioInfo(quest, locale, lineKey);
                    bool stale = owner.IsLineStale(quest, locale, lineKey);
                    if (info.Clip != null && !info.IsZeroLength)
                    {
                        if (stale)
                        {
                            GUILayout.Label("!!!", GUILayout.Width(26));
                        }
                        if (GUILayout.Button("Play", GUILayout.Width(44)))
                            owner.PlayClip(info.Clip);
                        if (GUILayout.Button("R", GUILayout.Width(24)))
                            owner.RegenerateLineAudioViaVoManager(quest, locale, lineKey);
                        if (GUILayout.Button("Ping", GUILayout.Width(44)))
                            PingObject(info.Obj);
                        if (GUILayout.Button("Delete", GUILayout.Width(58)))
                            ClearAudioAssignment(quest, locale, lineKey);
                    }
                    else
                    {
                        if (info.MissingOnDisk)
                        {
                            GUILayout.Label("missing on disk", GUILayout.Width(100));
                            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(info.Path)))
                            {
                                if (!string.IsNullOrEmpty(info.Path) && GUILayout.Button("Reimport", GUILayout.Width(70)))
                                {
                                    AssetDatabase.ImportAsset(info.Path, ImportAssetOptions.ForceUpdate);
                                }
                            }
                            if (GUILayout.Button("Clear", GUILayout.Width(50)))
                            {
                                ClearAudioAssignment(quest, locale, lineKey);
                            }
                            if (GUILayout.Button("Generate", GUILayout.Width(90)))
                                owner.RegenerateLineAudioViaVoManager(quest, locale, lineKey);
                        }
                        else
                        {
                            if (GUILayout.Button("Generate", GUILayout.Width(90)))
                                owner.RegenerateLineAudioViaVoManager(quest, locale, lineKey);
                        }
                    }
                }

                var addonText = GetStringValue(quest, locale, lineKey) ?? string.Empty;
                TryDrawCardAddon(locale, addonText);
            }

            private void TryDrawCardAddon(Locale locale, string entryText)
            {
                if (string.IsNullOrEmpty(entryText))
                    return;
                var rxCard = new Regex(@"<<\s*card\s+([A-Za-z0-9_\-]+)\s*>>");
                var m = rxCard.Match(entryText);
                if (!m.Success)
                    return;
                var cardId = m.Groups[1].Value;
                if (!owner._cardsById.TryGetValue(cardId, out var card) || card == null)
                    return;

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Space(584);

                    string cardTitle = GetLocalizedString(card.Title.TableReference, card.Title.TableEntryReference, locale) ?? card.Title.GetLocalizedString();
                    GUILayout.Label($"Card: {cardId} — {cardTitle}", EditorStyles.wordWrappedLabel, GUILayout.Width(300));

                    var at = LocalizationSettings.AssetDatabase.GetTable("Cards audio", locale);
                    AudioClip clip = null;
                    UnityEngine.Object obj = null;
                    if (at != null)
                    {
                        var e = at.GetEntry(card.Id);
                        if (e != null && !string.IsNullOrEmpty(e.Guid))
                        {
                            var path = AssetDatabase.GUIDToAssetPath(e.Guid);
                            obj = string.IsNullOrEmpty(path) ? null : AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                            clip = obj as AudioClip;
                        }
                    }
                    if (clip != null)
                    {
                        if (GUILayout.Button("Play", GUILayout.Width(44)))
                            owner.PlayClip(clip);
                        if (GUILayout.Button("Ping", GUILayout.Width(44)))
                            PingObject(obj);
                    }
                    else
                    {
                        GUILayout.Label("card audio missing");
                    }
                }
            }

            private void DrawOrphansSection(QuestData quest)
            {
                GUILayout.Label("Orphan cleanup", EditorStyles.boldLabel);
                var meta = YarnLineMapBuilder.BuildMeta(quest);
                var validKeys = new HashSet<string>(meta.Titles.Keys.Select(k => "line:" + k), StringComparer.Ordinal);

                var locales = owner.GetTargetLocales().ToList();
                if (locales.Count == 0)
                    return;

                foreach (var loc in locales)
                {
                    using (new EditorGUILayout.VerticalScope("box"))
                    {
                        GUILayout.Label($"Locale: {GetLocaleDisplayName(loc)}", EditorStyles.miniBoldLabel);

                        var st = LocalizationSettings.StringDatabase.GetTable(quest.QuestStringsTable.TableReference, loc);
                        var stringOrphans = new List<StringTableEntry>();
                        if (st != null)
                        {
                            foreach (var e in st.Values)
                            {
                                var key = e?.SharedEntry?.Key;
                                if (string.IsNullOrEmpty(key))
                                    continue;
                                if (key.StartsWith("line:", StringComparison.Ordinal) && !validKeys.Contains(key))
                                    stringOrphans.Add(e);
                            }
                        }

                        GUILayout.Label($"String orphans: {stringOrphans.Count}");
                        if (stringOrphans.Count > 0)
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                if (GUILayout.Button("Delete all string orphans", GUILayout.Width(220)))
                                {
                                    if (EditorUtility.DisplayDialog("Delete all string orphans?", $"This will remove {stringOrphans.Count} string entries not present in the Yarn script.", "Delete", "Cancel"))
                                    {
                                        foreach (var e in stringOrphans)
                                            st.RemoveEntry(e.SharedEntry.Id);
                                        MarkDirty(st);
                                    }
                                }
                            }
                        }

                        var at = LocalizationSettings.AssetDatabase.GetTable(quest.QuestAssetsTable.TableReference, loc);
                        var assetOrphans = new List<AssetTableEntry>();
                        if (at != null)
                        {
                            foreach (var e in at.Values)
                            {
                                var key = e?.SharedEntry?.Key;
                                if (string.IsNullOrEmpty(key))
                                    continue;
                                if (key.StartsWith("line:", StringComparison.Ordinal) && !validKeys.Contains(key))
                                    assetOrphans.Add(e);
                            }
                        }
                        GUILayout.Label($"Audio orphans: {assetOrphans.Count}");
                        if (assetOrphans.Count > 0)
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                if (GUILayout.Button("Delete all audio orphans", GUILayout.Width(200)))
                                {
                                    if (EditorUtility.DisplayDialog("Delete all audio orphans?", $"This will remove {assetOrphans.Count} audio entries not present in the Yarn script.", "Delete", "Cancel"))
                                    {
                                        foreach (var e in assetOrphans)
                                            at.RemoveEntry(e.SharedEntry.Id);
                                        MarkDirty(at);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            private List<RowItem> GetRowsInScriptOrder(QuestData quest, YarnLineMapBuilder.YarnLineMeta meta)
            {
                var rows = new List<RowItem>();
                try
                {
                    string text = quest?.YarnScript != null ? quest.YarnScript.text : null;
                    if (!string.IsNullOrEmpty(text))
                    {
                        var rx = new Regex(@"#(line|shadow):([A-Za-z0-9_\-]+)");
                        foreach (Match m in rx.Matches(text))
                        {
                            var kind = m.Groups[1].Value;
                            var id = m.Groups[2].Value;
                            if (string.IsNullOrEmpty(id))
                                continue;
                            bool isShadow = string.Equals(kind, "shadow", StringComparison.OrdinalIgnoreCase);
                            rows.Add(new RowItem { ShortId = id, IsShadow = isShadow });
                        }
                    }
                }
                catch { }

                if (rows.Count == 0 && meta != null)
                {
                    foreach (var id in meta.Titles.Keys)
                    {
                        rows.Add(new RowItem { ShortId = id, IsShadow = false });
                    }
                }
                return rows;
            }

            private List<string> GetLineIdsInScriptOrder(QuestData quest, YarnLineMapBuilder.YarnLineMeta meta)
            {
                var ordered = new List<string>();
                var seen = new HashSet<string>(StringComparer.Ordinal);
                try
                {
                    string text = quest?.YarnScript != null ? quest.YarnScript.text : null;
                    if (!string.IsNullOrEmpty(text))
                    {
                        var rxLine = new Regex(@"#line:([A-Za-z0-9_\-]+)");
                        foreach (Match m in rxLine.Matches(text))
                        {
                            var id = m.Groups[1].Value;
                            if (!seen.Contains(id) && meta.Titles.ContainsKey(id))
                            {
                                seen.Add(id);
                                ordered.Add(id);
                            }
                        }
                    }
                }
                catch { }
                if (ordered.Count == 0)
                {
                    ordered.AddRange(meta.Titles.Keys);
                }
                return ordered;
            }
        }
    }
}
#endif
