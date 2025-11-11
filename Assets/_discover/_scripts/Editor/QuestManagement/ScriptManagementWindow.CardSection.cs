#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Antura.Discover.EditorTools
{
    public partial class ScriptManagementWindow
    {
        private sealed class CardSection
        {
            private readonly ScriptManagementWindow owner;

            public CardSection(ScriptManagementWindow owner)
            {
                this.owner = owner;
            }

            public void Draw(QuestData quest)
            {
                var selectedLocales = owner.GetTargetLocales().ToList();
                if (selectedLocales.Count == 0)
                {
                    EditorGUILayout.HelpBox("Select at least one locale to inspect card audio.", MessageType.Info);
                    return;
                }

                var displayLocale = owner.GetEnglishLocale() ?? selectedLocales.FirstOrDefault();
                if (displayLocale == null)
                {
                    EditorGUILayout.HelpBox("No locales available.", MessageType.Info);
                    return;
                }

                var occurrences = GetCardOccurrencesInScript(quest);
                if (!string.IsNullOrWhiteSpace(owner._filter))
                {
                    var f = owner._filter.Trim();
                    occurrences = occurrences.Where(o =>
                        (!string.IsNullOrEmpty(o.NodeTitle) && o.NodeTitle.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0)
                        || (!string.IsNullOrEmpty(o.CardId) && o.CardId.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0)
                    ).ToList();
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Node Title", EditorStyles.boldLabel, GUILayout.Width(300));
                    GUILayout.Label("Card Title", EditorStyles.boldLabel, GUILayout.Width(300));
                    GUILayout.Label("Card Audio", EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Regenerate missing card title audio", GUILayout.Width(280)))
                    {
                        RegenerateCardTitlesForOccurrences(occurrences, onlyMissing: true);
                    }
                    if (GUILayout.Button("Regenerate all card title audio", GUILayout.Width(260)))
                    {
                        if (EditorUtility.DisplayDialog("Regenerate all card audio?", "This will overwrite existing card title audio for the selected locales.", "Regenerate", "Cancel"))
                        {
                            RegenerateCardTitlesForOccurrences(occurrences, onlyMissing: false);
                        }
                    }
                }

                foreach (var occ in occurrences)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Label(occ.NodeTitle ?? string.Empty, EditorStyles.wordWrappedLabel, GUILayout.Width(300));
                        DrawCardInfoInline(occ.CardId, displayLocale, selectedLocales);
                    }
                }
            }

            private void DrawCardInfoInline(string cardId, Locale localeForTitle, List<Locale> audioLocales)
            {
                if (string.IsNullOrEmpty(cardId) || !owner._cardsById.TryGetValue(cardId, out var card) || card == null)
                {
                    GUILayout.Label("Card: -", GUILayout.Width(300));
                    GUILayout.Label("-", GUILayout.Width(90));
                    return;
                }

                var titleLocale = localeForTitle ?? owner.GetEnglishLocale();
                string cardTitle = titleLocale != null
                    ? (GetLocalizedString(card.Title.TableReference, card.Title.TableEntryReference, titleLocale) ?? card.Title.GetLocalizedString())
                    : (card.TitleEn ?? card.name);
                GUILayout.Label($"Card: {cardId} — {cardTitle}", EditorStyles.wordWrappedLabel, GUILayout.Width(300));
                if (GUILayout.Button("Ping Card", GUILayout.Width(90)))
                {
                    PingObject(card);
                }

                if (audioLocales == null || audioLocales.Count == 0)
                {
                    GUILayout.Label("Select locales to show audio controls", GUILayout.Width(220));
                    return;
                }

                using (new EditorGUILayout.VerticalScope(GUILayout.Width(240)))
                {
                    foreach (var loc in audioLocales)
                    {
                        DrawCardAudioRow(card, loc);
                    }
                }
                GUILayout.FlexibleSpace();
            }

            private void DrawCardAudioRow(CardData card, Locale locale)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    var code = locale != null ? locale.Identifier.Code : "-";
                    GUILayout.Label(code, GUILayout.Width(50));

                    var table = locale != null ? LocalizationSettings.AssetDatabase.GetTable("Cards audio", locale) : null;
                    AudioClip clip = null;
                    UnityEngine.Object obj = null;
                    bool missingOnDisk = false;
                    if (table != null)
                    {
                        var entry = table.GetEntry(card.Id);
                        if (entry != null && !string.IsNullOrEmpty(entry.Guid))
                        {
                            var path = AssetDatabase.GUIDToAssetPath(entry.Guid);
                            if (string.IsNullOrEmpty(path))
                            {
                                missingOnDisk = true;
                            }
                            else
                            {
                                obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                                clip = obj as AudioClip;
                            }
                        }
                    }

                    if (clip != null)
                    {
                        if (GUILayout.Button("Play", GUILayout.Width(44)))
                            owner.PlayClip(clip);
                        using (new EditorGUI.DisabledScope(obj == null))
                        {
                            if (GUILayout.Button("Ping", GUILayout.Width(44)))
                                PingObject(obj);
                        }
                        if (GUILayout.Button("R", GUILayout.Width(24)))
                            owner.RegenerateCardTitleAudio(card, locale);
                    }
                    else
                    {
                        if (missingOnDisk)
                        {
                            GUILayout.Label("missing", GUILayout.Width(70));
                        }
                        if (GUILayout.Button("Generate", GUILayout.Width(96)))
                            owner.RegenerateCardTitleAudio(card, locale);
                    }
                }
            }

            private void RegenerateCardTitlesForOccurrences(List<CardOccurrence> occurrences, bool onlyMissing)
            {
                if (occurrences == null || occurrences.Count == 0)
                {
                    EditorUtility.DisplayDialog("Cards Audio", "No card lines found in this script.", "OK");
                    return;
                }

                var ids = new HashSet<string>(occurrences.Where(o => !string.IsNullOrEmpty(o.CardId)).Select(o => o.CardId), StringComparer.Ordinal);
                var cards = ids.Select(id => owner._cardsById.TryGetValue(id, out var c) ? c : null).Where(c => c != null).Distinct().ToList();
                if (cards.Count == 0)
                {
                    EditorUtility.DisplayDialog("Cards Audio", "No matching Card assets found.", "OK");
                    return;
                }

                var voWindow = Resources.FindObjectsOfTypeAll<Antura.Discover.Audio.Editor.VoiceoverManagerWindow>().FirstOrDefault()
                               ?? EditorWindow.GetWindow<Antura.Discover.Audio.Editor.VoiceoverManagerWindow>();
                if (voWindow == null)
                {
                    Debug.LogWarning("Voiceover Manager window not found.");
                    return;
                }

                try
                {
                    var t = voWindow.GetType();
                    var onlyMissingField = t.GetField("_onlyGenerateMissing", BindingFlags.Instance | BindingFlags.NonPublic);
                    var includeDescField = t.GetField("_cardsIncludeDescriptions", BindingFlags.Instance | BindingFlags.NonPublic);
                    var createCapField = t.GetField("_createCapIndex", BindingFlags.Instance | BindingFlags.NonPublic);
                    var localesField = t.GetField("_locales", BindingFlags.Instance | BindingFlags.NonPublic);
                    var localeIdxField = t.GetField("_selectedLocaleIndex", BindingFlags.Instance | BindingFlags.NonPublic);
                    var runMethod = t.GetMethod("RunCreateCardAudio", BindingFlags.Instance | BindingFlags.NonPublic);

                    onlyMissingField?.SetValue(voWindow, onlyMissing);
                    includeDescField?.SetValue(voWindow, false);
                    createCapField?.SetValue(voWindow, 2);

                    int selIndex = 0;
                    var selectedLocales = owner.GetTargetLocales().ToList();
                    if (selectedLocales.Count == 1)
                    {
                        var voLocales = localesField?.GetValue(voWindow) as System.Collections.IList;
                        if (voLocales != null)
                        {
                            for (int i = 0; i < voLocales.Count; i++)
                            {
                                var loc = voLocales[i] as Locale;
                                if (loc != null && loc.Identifier.Code == selectedLocales[0].Identifier.Code)
                                { selIndex = i + 1; break; }
                            }
                        }
                    }
                    localeIdxField?.SetValue(voWindow, selIndex);

                    runMethod?.Invoke(voWindow, new object[] { cards });
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to call VO Manager (cards): {ex.Message}");
                }
            }

            private List<CardOccurrence> GetCardOccurrencesInScript(QuestData quest)
            {
                var list = new List<CardOccurrence>();
                try
                {
                    string text = quest?.YarnScript != null ? quest.YarnScript.text : null;
                    if (string.IsNullOrEmpty(text))
                        return list;
                    string currentTitle = string.Empty;
                    var rxTitle = new Regex(@"^\s*title:\s*(.+)$", RegexOptions.Multiline);
                    var rxCard = new Regex(@"<<\s*card\s+([A-Za-z0-9_\-]+)\s*>>", RegexOptions.Multiline);

                    var titleMatches = rxTitle.Matches(text).Cast<Match>().ToList();
                    var cardMatches = rxCard.Matches(text).Cast<Match>().ToList();

                    foreach (var cm in cardMatches)
                    {
                        string nodeTitle = string.Empty;
                        int pos = cm.Index;
                        for (int i = titleMatches.Count - 1; i >= 0; i--)
                        {
                            if (titleMatches[i].Index <= pos)
                            {
                                nodeTitle = titleMatches[i].Groups[1].Value.Trim();
                                break;
                            }
                        }
                        list.Add(new CardOccurrence { NodeTitle = nodeTitle, CardId = cm.Groups[1].Value });
                    }
                }
                catch { }
                return list;
            }

            private sealed class CardOccurrence
            {
                public string NodeTitle;
                public string CardId;
            }
        }
    }
}
#endif
